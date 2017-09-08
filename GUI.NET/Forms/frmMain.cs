using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Mesen.GUI.Config;
using Mesen.GUI.Debugger;
using Mesen.GUI.Forms.Cheats;
using Mesen.GUI.Forms.Config;
using Mesen.GUI.Forms.HdPackEditor;
using Mesen.GUI.Forms.NetPlay;
using Mesen.GUI.GoogleDriveIntegration;

namespace Mesen.GUI.Forms
{
	public partial class frmMain : BaseForm, IMessageFilter
	{
		private const int WM_KEYDOWN = 0x100;
		private const int WM_KEYUP = 0x101;
		const int WM_SYSKEYDOWN = 0x104;
		const int WM_SYSKEYUP = 0x105;

		private InteropEmu.NotificationListener _notifListener;
		private Thread _emuThread;
		private frmLogWindow _logWindow;
		private frmCheatList _cheatListWindow;
		private frmHdPackEditor _hdPackEditorWindow;
		private ResourcePath? _currentRomPath = null;
		private Image _pauseButton = Mesen.GUI.Properties.Resources.Pause;
		private Image _playButton = Mesen.GUI.Properties.Resources.Play;
		private string _currentGame = null;
		private bool _customSize = false;
		private FormWindowState _originalWindowState;
		private bool _fullscreenMode = false;
		private Size? _nonNsfSize = null;
		private bool _isNsfPlayerMode = false;
		private object _loadRomLock = new object();
		private int _romLoadCounter = 0;
		private bool _removeFocus = false;
		private bool _showUpgradeMessage = false;

		private Dictionary<EmulatorShortcut, Func<bool>> _actionEnabledFuncs = new Dictionary<EmulatorShortcut, Func<bool>>();

		private string[] _commandLineArgs;
		private bool _noAudio = false;
		private bool _noVideo = false;
		private bool _noInput = false;

		private PrivateFontCollection _fonts = new PrivateFontCollection();

		public frmMain(string[] args)
		{
			InitializeComponent();

			this.StartPosition = FormStartPosition.CenterScreen;

			Version currentVersion = new Version(InteropEmu.GetMesenVersion());
			lblVersion.Text = currentVersion.ToString();

			_fonts.AddFontFile(Path.Combine(ConfigManager.HomeFolder, "Resources", "PixelFont.ttf"));
			lblVersion.Font = new Font(_fonts.Families[0], 11);

			_commandLineArgs = args;

			Application.AddMessageFilter(this);
			this.Resize += ResizeRecentGames;
			this.FormClosed += (s, e) => Application.RemoveMessageFilter(this);
		}

		public void ProcessCommandLineArguments(string[] args, bool forStartup)
		{
			var switches = new List<string>();
			for(int i = 0; i < args.Length; i++) {
				if(args[i] != null) {
					switches.Add(args[i].ToLowerInvariant().Replace("--", "/").Replace("-", "/").Replace("=/", "=-"));
				}
			}

			if(forStartup) {
				_noVideo = switches.Contains("/novideo");
				_noAudio = switches.Contains("/noaudio");
				_noInput = switches.Contains("/noinput");
			}

			if(switches.Contains("/fullscreen")) {
				this.SetFullscreenState(true);
			}
			if(switches.Contains("/donotsavesettings")) {
				ConfigManager.DoNotSaveSettings = true;
			}
			ConfigManager.ProcessSwitches(switches);
		}

		public void LoadGameFromCommandLine(string[] args)
		{
			if(args.Length > 0) {
				foreach(string arg in args) {
					if(arg != null) {
						string path = arg;
						try {
							if(File.Exists(path)) {
								this.LoadFile(path);
								break;
							}

							//Try loading file as a relative path to the folder Mesen was started from
							path = Path.Combine(Program.OriginalFolder, path);
							if(File.Exists(path)) {
								this.LoadFile(path);
								break;
							}
						} catch { }
					}
				}
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			ResourceHelper.LoadResources(ConfigManager.Config.PreferenceInfo.DisplayLanguage);
			ResourceHelper.UpdateEmuLanguage();

			base.OnLoad(e);

			#if HIDETESTMENU
			mnuTests.Visible = false;
			#endif

			PerformUpgrade();

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			menuTimer.Start();

			this.ProcessCommandLineArguments(_commandLineArgs, true);

			VideoInfo.ApplyConfig();
			InitializeVsSystemMenu();
			InitializeFdsDiskMenu();
			InitializeEmulationSpeedMenu();

			UpdateVideoSettings();

			InitializeEmu();

			UpdateMenus();
			UpdateRecentFiles();

			UpdateViewerSize();

			InitializeStateMenu(mnuSaveState, true);
			InitializeStateMenu(mnuLoadState, false);

			if(ConfigManager.Config.WindowLocation.HasValue) {
				this.StartPosition = FormStartPosition.Manual;
				this.Location = ConfigManager.Config.WindowLocation.Value;
			}

			if(ConfigManager.Config.PreferenceInfo.CloudSaveIntegration) {
				Task.Run(() => CloudSyncHelper.Sync());
			}

			if(ConfigManager.Config.PreferenceInfo.AutomaticallyCheckForUpdates) {
				CheckForUpdates(false);
			}

			if(ConfigManager.Config.WindowSize.HasValue) {
				this.ClientSize = ConfigManager.Config.WindowSize.Value;
			}

			if(Program.IsMono) {
				//Mono does not trigger the activate/deactivate events when opening a modal popup, but it does set the form to disabled
				//Use this to reset key states
				this.EnabledChanged += (object s, EventArgs evt) => {
					_removeFocus = !this.Enabled;
					InteropEmu.ResetKeyState();
				};
			}
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);
			_removeFocus = true;
			InteropEmu.ResetKeyState();
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			_removeFocus = false;
			InteropEmu.ResetKeyState();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.BindShortcuts();

			if(ConfigManager.Config.WindowSize.HasValue) {
				this.Size = ConfigManager.Config.WindowSize.Value;
			}

			this.LoadGameFromCommandLine(_commandLineArgs);

			this.menuStrip.VisibleChanged += new System.EventHandler(this.menuStrip_VisibleChanged);
			this.UpdateRendererLocation();

			if(_showUpgradeMessage) {
				MesenMsgBox.Show("UpgradeSuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
				_showUpgradeMessage = false;
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if(ConfigManager.Config.PreferenceInfo.ConfirmExitResetPower && MesenMsgBox.Show("ConfirmExit", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
				e.Cancel = true;
				return;
			}

			if(_notifListener != null) {
				_notifListener.Dispose();
				_notifListener = null;
			}
			DebugWindowManager.CloseAll();

			ConfigManager.Config.EmulationInfo.EmulationSpeed = InteropEmu.GetEmulationSpeed();
			if(this.WindowState == FormWindowState.Normal) {
				ConfigManager.Config.WindowLocation = this.Location;
				ConfigManager.Config.WindowSize = this.Size;
			} else {
				ConfigManager.Config.WindowLocation = this.RestoreBounds.Location;
				ConfigManager.Config.WindowSize = this.RestoreBounds.Size;
			}
			if(this._nonNsfSize.HasValue) {
				ConfigManager.Config.WindowSize = this._nonNsfSize.Value;
			}

			ConfigManager.ApplyChanges();

			StopEmu();

			if(ConfigManager.Config.PreferenceInfo.CloudSaveIntegration) {
				CloudSyncHelper.Sync();
			}

			InteropEmu.Release();

			ConfigManager.SaveConfig();

			base.OnClosing(e);
		}

		private void menuTimer_Tick(object sender, EventArgs e)
		{
			this.UpdateMenus();
		}

		void InitializeEmu()
		{
			InteropEmu.InitializeEmu(ConfigManager.HomeFolder, this.Handle, this.ctrlRenderer.Handle, _noAudio, _noVideo, _noInput);
			if(ConfigManager.Config.PreferenceInfo.OverrideGameFolder && Directory.Exists(ConfigManager.Config.PreferenceInfo.GameFolder)) {
				InteropEmu.AddKnownGameFolder(ConfigManager.Config.PreferenceInfo.GameFolder);
			}
			foreach(RecentItem recentItem in ConfigManager.Config.RecentFiles) {
				InteropEmu.AddKnownGameFolder(recentItem.RomFile.Folder);
			}

			ConfigManager.Config.InitializeDefaults();
			ConfigManager.ApplyChanges();
			ConfigManager.Config.ApplyConfig();

			UpdateEmulationFlags();
		}

		private void UpdateViewerSize()
		{
			this.Resize -= frmMain_Resize;

			InteropEmu.ScreenSize size = InteropEmu.GetScreenSize(false);

			if(!_customSize && this.WindowState != FormWindowState.Maximized) {
				Size sizeGap = this.Size - this.ClientSize;

				UpdateScaleMenu(size.Scale);
				this.ClientSize = new Size(Math.Max(this.MinimumSize.Width - sizeGap.Width, size.Width), Math.Max(this.MinimumSize.Height - sizeGap.Height, size.Height + (this.HideMenuStrip ? 0 : menuStrip.Height)));
			}

			ctrlRenderer.Size = new Size(size.Width, size.Height);
			ctrlRenderer.Left = (panelRenderer.Width - ctrlRenderer.Width) / 2;
			ctrlRenderer.Top = (panelRenderer.Height - ctrlRenderer.Height) / 2;

			if(this.HideMenuStrip) {
				this.menuStrip.Visible = false;
			}

			this.Resize += frmMain_Resize;
		}

		private void ResizeRecentGames(object sender, EventArgs e)
		{
			if(this.ClientSize.Height < 400) {
				ctrlRecentGames.Height = this.ClientSize.Height - 125 + Math.Min(50, (400 - this.ClientSize.Height));
			} else {
				ctrlRecentGames.Height = this.ClientSize.Height - 125;
			}
			ctrlRecentGames.Width = this.ClientSize.Width;
			ctrlRecentGames.Top = (this.HideMenuStrip && this.menuStrip.Visible) ? -menuStrip.Height : 0;
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			if(this.WindowState != FormWindowState.Minimized) {
				SetScaleBasedOnWindowSize();
				ctrlRenderer.Left = (panelRenderer.Width - ctrlRenderer.Width) / 2;
				ctrlRenderer.Top = (panelRenderer.Height - ctrlRenderer.Height) / 2;
			}
		}

		private void SetScaleBasedOnWindowSize()
		{
			_customSize = true;
			InteropEmu.ScreenSize size = InteropEmu.GetScreenSize(true);
			double verticalScale = (double)panelRenderer.ClientSize.Height / size.Height;
			double horizontalScale = (double)panelRenderer.ClientSize.Width / size.Width;
			double scale = Math.Min(verticalScale, horizontalScale);
			if(_fullscreenMode && ConfigManager.Config.VideoInfo.FullscreenForceIntegerScale) {
				scale = Math.Floor(scale);
			}
			UpdateScaleMenu(scale);
			VideoInfo.ApplyConfig();
		}

		private void SetFullscreenState(bool enabled)
		{
			this.Resize -= frmMain_Resize;
			_fullscreenMode = enabled;
			if(enabled) {
				this.menuStrip.Visible = false;
				_originalWindowState = this.WindowState;
				this.WindowState = FormWindowState.Normal;
				this.FormBorderStyle = FormBorderStyle.None;
				this.WindowState = FormWindowState.Maximized;
				SetScaleBasedOnWindowSize();
			} else {
				this.menuStrip.Visible = true;
				this.WindowState = _originalWindowState;
				this.FormBorderStyle = FormBorderStyle.Sizable;
				this.frmMain_Resize(null, EventArgs.Empty);
			}
			this.Resize += frmMain_Resize;
			mnuFullscreen.Checked = enabled;
		}

		private bool HideMenuStrip
		{
			get
			{
				return _fullscreenMode || ConfigManager.Config.PreferenceInfo.AutoHideMenu;
			}
		}

		private void ctrlRenderer_MouseMove(object sender, MouseEventArgs e)
		{
			if(sender != this.ctrlRecentGames) {
				CursorManager.OnMouseMove((Control)sender);
			}

			if(this.HideMenuStrip && !this.menuStrip.ContainsFocus) {
				if(sender == ctrlRenderer) {
					this.menuStrip.Visible = ctrlRenderer.Top + e.Y < 30;
				} else {
					this.menuStrip.Visible = e.Y < 30;
				}
			}
		}

		private void ctrlRenderer_MouseClick(object sender, MouseEventArgs e)
		{
			if(this.HideMenuStrip) {
				this.menuStrip.Visible = false;
			}
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.GameLoaded:
					_currentGame = InteropEmu.GetRomInfo().GetRomName();
					InteropEmu.SetNesModel(ConfigManager.Config.Region);
					InitializeNsfMode(false, true);
					InitializeFdsDiskMenu();
					InitializeVsSystemMenu();
					CheatInfo.ApplyCheats();
					VsConfigInfo.ApplyConfig();
					UpdateStateMenu(mnuSaveState, true);
					UpdateStateMenu(mnuLoadState, false);
					if(ConfigManager.Config.PreferenceInfo.ShowVsConfigOnLoad && InteropEmu.IsVsSystem()) {
						this.Invoke((MethodInvoker)(() => {
							this.ShowVsGameConfig();
						}));
					}

					this.StartEmuThread();
					this.BeginInvoke((MethodInvoker)(() => {
						UpdateViewerSize();
					}));

					if(ConfigManager.Config.PreferenceInfo.DeveloperMode) {
						//Preload workspace in the background to be able to open debugging tools faster
						Task.Run(() => { DebugWorkspace.GetWorkspace(); });
					}
					break;

				case InteropEmu.ConsoleNotificationType.PpuFrameDone:
					if(InteropEmu.IsNsf()) {
						this.ctrlNsfPlayer.CountFrame();
					}
					break;

				case InteropEmu.ConsoleNotificationType.GameReset:
					InitializeNsfMode();
					break;

				case InteropEmu.ConsoleNotificationType.DisconnectedFromServer:
					ConfigManager.Config.ApplyConfig();
					break;

				case InteropEmu.ConsoleNotificationType.GameStopped:
					this._currentGame = null;
					InitializeNsfMode();
					CheatInfo.ClearCheats();
					this.BeginInvoke((MethodInvoker)(() => {
						if(_hdPackEditorWindow != null) {
							_hdPackEditorWindow.Close();
						}
						ctrlRecentGames.Initialize();
					}));
					break;

				case InteropEmu.ConsoleNotificationType.ResolutionChanged:
					this.BeginInvoke((MethodInvoker)(() => {
						UpdateViewerSize();
					}));
					break;

				case InteropEmu.ConsoleNotificationType.FdsBiosNotFound:
					this.BeginInvoke((MethodInvoker)(() => {
						SelectFdsBiosPrompt();
					}));
					break;

				case InteropEmu.ConsoleNotificationType.ExecuteShortcut:
					this.BeginInvoke((MethodInvoker)(() => {
						ExecuteShortcut((EmulatorShortcut)e.Parameter);
					}));
					break;

				case InteropEmu.ConsoleNotificationType.CodeBreak:
					this.BeginInvoke((MethodInvoker)(() => {
						if(DebugWindowManager.GetDebugger() == null) {
							DebugWindowManager.OpenDebugWindow(DebugWindow.Debugger);
						}
					}));
					break;
			}

			if(e.NotificationType != InteropEmu.ConsoleNotificationType.PpuFrameDone) {
				UpdateMenus();
			}
		}

		private void BindShortcuts()
		{
			Func<bool> notClient = () => { return !InteropEmu.IsConnected(); };
			Func<bool> runningNotClient = () => { return _emuThread != null && !InteropEmu.IsConnected(); };
			Func<bool> runningNotClientNotMovie = () => { return _emuThread != null && !InteropEmu.IsConnected() && !InteropEmu.MoviePlaying() && !InteropEmu.MovieRecording(); };

			Func<bool> runningNotNsf = () => { return _emuThread != null && !InteropEmu.IsNsf(); };
			Func<bool> runningFdsNoAutoInsert = () => { return _emuThread != null && InteropEmu.FdsGetSideCount() > 0 && !InteropEmu.FdsIsAutoInsertDiskEnabled() && !InteropEmu.MoviePlaying() && !InteropEmu.MovieRecording(); };
			Func<bool> runningVsSystem = () => { return _emuThread != null && InteropEmu.IsVsSystem() && !InteropEmu.MoviePlaying() && !InteropEmu.MovieRecording(); };

			BindShortcut(mnuOpen, EmulatorShortcut.OpenFile);
			BindShortcut(mnuExit, EmulatorShortcut.Exit);
			BindShortcut(mnuIncreaseSpeed, EmulatorShortcut.IncreaseSpeed, notClient);
			BindShortcut(mnuDecreaseSpeed, EmulatorShortcut.DecreaseSpeed, notClient);
			BindShortcut(mnuEmuSpeedMaximumSpeed, EmulatorShortcut.MaxSpeed, notClient);

			BindShortcut(mnuPause, EmulatorShortcut.Pause, runningNotClient);
			BindShortcut(mnuReset, EmulatorShortcut.Reset, runningNotClientNotMovie);
			BindShortcut(mnuPowerCycle, EmulatorShortcut.PowerCycle, runningNotClientNotMovie);
			BindShortcut(mnuPowerOff, EmulatorShortcut.PowerOff, runningNotClient);

			BindShortcut(mnuSwitchDiskSide, EmulatorShortcut.SwitchDiskSide, runningFdsNoAutoInsert);
			BindShortcut(mnuEjectDisk, EmulatorShortcut.EjectDisk, runningFdsNoAutoInsert);

			BindShortcut(mnuInsertCoin1, EmulatorShortcut.InsertCoin1, runningVsSystem);
			BindShortcut(mnuInsertCoin2, EmulatorShortcut.InsertCoin2, runningVsSystem);

			BindShortcut(mnuShowFPS, EmulatorShortcut.ToggleFps);

			BindShortcut(mnuScale1x, EmulatorShortcut.SetScale1x);
			BindShortcut(mnuScale2x, EmulatorShortcut.SetScale2x);
			BindShortcut(mnuScale3x, EmulatorShortcut.SetScale3x);
			BindShortcut(mnuScale4x, EmulatorShortcut.SetScale4x);
			BindShortcut(mnuScale5x, EmulatorShortcut.SetScale5x);
			BindShortcut(mnuScale6x, EmulatorShortcut.SetScale6x);

			BindShortcut(mnuFullscreen, EmulatorShortcut.ToggleFullscreen);

			BindShortcut(mnuTakeScreenshot, EmulatorShortcut.TakeScreenshot, runningNotNsf);
			BindShortcut(mnuRandomGame, EmulatorShortcut.LoadRandomGame);

			BindShortcut(mnuDebugDebugger, EmulatorShortcut.OpenDebugger, runningNotClient);
			BindShortcut(mnuDebugger, EmulatorShortcut.OpenDebugger, runningNotClient);
			BindShortcut(mnuAssembler, EmulatorShortcut.OpenAssembler, runningNotClient);
			BindShortcut(mnuMemoryViewer, EmulatorShortcut.OpenMemoryTools, runningNotClient);
			BindShortcut(mnuPpuViewer, EmulatorShortcut.OpenPpuViewer, runningNotClient);
			BindShortcut(mnuScriptWindow, EmulatorShortcut.OpenScriptWindow, runningNotClient);
			BindShortcut(mnuTraceLogger, EmulatorShortcut.OpenTraceLogger, runningNotClient);
		}
		
		private void BindShortcut(ToolStripMenuItem item, EmulatorShortcut shortcut, Func<bool> isActionEnabled = null)
		{
			item.Click += (object sender, EventArgs e) => {
				if(isActionEnabled == null || isActionEnabled()) {
					ExecuteShortcut(shortcut);
				}
			};

			_actionEnabledFuncs[shortcut] = isActionEnabled;

			if(item.OwnerItem is ToolStripMenuItem) {
				Action updateShortcut = () => {
					int keyIndex = ConfigManager.Config.PreferenceInfo.ShortcutKeys1.FindIndex((ShortcutKeyInfo shortcutInfo) => shortcutInfo.Shortcut == shortcut);
					if(keyIndex >= 0) {
						item.ShortcutKeyDisplayString = ConfigManager.Config.PreferenceInfo.ShortcutKeys1[keyIndex].KeyCombination.ToString();
					} else {
						keyIndex = ConfigManager.Config.PreferenceInfo.ShortcutKeys2.FindIndex((ShortcutKeyInfo shortcutInfo) => shortcutInfo.Shortcut == shortcut);
						if(keyIndex >= 0) {
							item.ShortcutKeyDisplayString = ConfigManager.Config.PreferenceInfo.ShortcutKeys2[keyIndex].KeyCombination.ToString();
						} else {
							item.ShortcutKeyDisplayString = "";
						}
					}
					item.Enabled = isActionEnabled == null || isActionEnabled();
				};

				updateShortcut();

				//Update item shortcut text when its parent opens
				((ToolStripMenuItem)item.OwnerItem).DropDownOpening += (object sender, EventArgs e) => { updateShortcut(); };
			}
		}

		private void ExecuteShortcut(EmulatorShortcut shortcut)
		{
			Func<bool> isActionEnabled;
			if(_actionEnabledFuncs.TryGetValue(shortcut, out isActionEnabled)) {
				isActionEnabled = _actionEnabledFuncs[shortcut];
				if(isActionEnabled != null && !isActionEnabled()) {
					//Action disabled
					return;
				}
			}

			switch(shortcut) {
				case EmulatorShortcut.Pause: PauseEmu(); break;
				case EmulatorShortcut.Reset: this.ResetEmu(); break;
				case EmulatorShortcut.PowerCycle: this.PowerCycleEmu(); break;
				case EmulatorShortcut.PowerOff: InteropEmu.Stop(); break;
				case EmulatorShortcut.Exit: this.Close(); break;

				case EmulatorShortcut.ToggleCheats: ToggleCheats(); break;
				case EmulatorShortcut.ToggleAudio: ToggleAudio(); break;
				case EmulatorShortcut.ToggleFps: ToggleFps(); break;
				case EmulatorShortcut.MaxSpeed: ToggleMaxSpeed(); break;
				case EmulatorShortcut.ToggleFullscreen: ToggleFullscreen(); break;

				case EmulatorShortcut.OpenFile: OpenFile(); break;
				case EmulatorShortcut.IncreaseSpeed: InteropEmu.IncreaseEmulationSpeed(); break;
				case EmulatorShortcut.DecreaseSpeed: InteropEmu.DecreaseEmulationSpeed(); break;
				case EmulatorShortcut.SwitchDiskSide: InteropEmu.FdsSwitchDiskSide(); break;
				case EmulatorShortcut.EjectDisk: InteropEmu.FdsEjectDisk(); break;

				case EmulatorShortcut.SetScale1x: SetScale(1); break;
				case EmulatorShortcut.SetScale2x: SetScale(2); break;
				case EmulatorShortcut.SetScale3x: SetScale(3); break;
				case EmulatorShortcut.SetScale4x: SetScale(4); break;
				case EmulatorShortcut.SetScale5x: SetScale(5); break;
				case EmulatorShortcut.SetScale6x: SetScale(6); break;
					
				case EmulatorShortcut.InsertCoin1: InteropEmu.VsInsertCoin(0); break;
				case EmulatorShortcut.InsertCoin2: InteropEmu.VsInsertCoin(1); break;

				case EmulatorShortcut.TakeScreenshot: InteropEmu.TakeScreenshot(); break;
				case EmulatorShortcut.LoadRandomGame: LoadRandomGame(); break;

				case EmulatorShortcut.OpenAssembler: DebugWindowManager.OpenDebugWindow(DebugWindow.Assembler); break;
				case EmulatorShortcut.OpenDebugger: DebugWindowManager.OpenDebugWindow(DebugWindow.Debugger); break;
				case EmulatorShortcut.OpenTraceLogger: DebugWindowManager.OpenDebugWindow(DebugWindow.TraceLogger); break;
				case EmulatorShortcut.OpenPpuViewer: DebugWindowManager.OpenDebugWindow(DebugWindow.PpuViewer); break;
				case EmulatorShortcut.OpenMemoryTools: DebugWindowManager.OpenDebugWindow(DebugWindow.MemoryViewer); break;
				case EmulatorShortcut.OpenScriptWindow: DebugWindowManager.OpenDebugWindow(DebugWindow.ScriptWindow); break;

				case EmulatorShortcut.LoadStateFromFile: LoadStateFromFile(); break;
				case EmulatorShortcut.SaveStateToFile: SaveStateToFile(); break;

				case EmulatorShortcut.SaveStateSlot1: SaveState(1); break;
				case EmulatorShortcut.SaveStateSlot2: SaveState(2); break;
				case EmulatorShortcut.SaveStateSlot3: SaveState(3); break;
				case EmulatorShortcut.SaveStateSlot4: SaveState(4); break;
				case EmulatorShortcut.SaveStateSlot5: SaveState(5); break;
				case EmulatorShortcut.SaveStateSlot6: SaveState(6); break;
				case EmulatorShortcut.SaveStateSlot7: SaveState(7); break;
				case EmulatorShortcut.LoadStateSlot1: LoadState(1); break;
				case EmulatorShortcut.LoadStateSlot2: LoadState(2); break;
				case EmulatorShortcut.LoadStateSlot3: LoadState(3); break;
				case EmulatorShortcut.LoadStateSlot4: LoadState(4); break;
				case EmulatorShortcut.LoadStateSlot5: LoadState(5); break;
				case EmulatorShortcut.LoadStateSlot6: LoadState(6); break;
				case EmulatorShortcut.LoadStateSlot7: LoadState(7); break;
				case EmulatorShortcut.LoadStateSlot8: LoadState(8); break;
			}
		}

		private void ToggleFullscreen()
		{
			SetFullscreenState(!_fullscreenMode);
			mnuFullscreen.Checked = _fullscreenMode;
		}

		private void ToggleMaxSpeed()
		{
			if(ConfigManager.Config.EmulationInfo.EmulationSpeed == 0) {
				SetEmulationSpeed(100);
			} else {
				SetEmulationSpeed(0);
			}
		}

		private void ToggleFps()
		{
			mnuShowFPS.Checked = !mnuShowFPS.Checked;
			UpdateEmulationFlags();
		}

		private static void ToggleAudio()
		{
			ConfigManager.Config.AudioInfo.EnableAudio = !ConfigManager.Config.AudioInfo.EnableAudio;
			AudioInfo.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private static void ToggleCheats()
		{
			ConfigManager.Config.DisableAllCheats = !ConfigManager.Config.DisableAllCheats;
			if(ConfigManager.Config.DisableAllCheats) {
				InteropEmu.DisplayMessage("Cheats", "CheatsDisabled");
			}
			CheatInfo.ApplyCheats();
			ConfigManager.ApplyChanges();
		}

		private void UpdateFocusFlag()
		{
			bool hasFocus = false;
			if(Application.OpenForms.Count > 0) {
				if(Application.OpenForms[0].ContainsFocus) {
					hasFocus = true;
				}
			}

			if(_removeFocus) {
				hasFocus = false;
			}

			InteropEmu.SetFlag(EmulationFlags.InBackground, !hasFocus);
		}

		private void UpdateMenus()
		{
			try {
				if(this.InvokeRequired) {
					this.BeginInvoke((MethodInvoker)(() => this.UpdateMenus()));
				} else {
					bool running = _emuThread != null;
					bool devMode = ConfigManager.Config.PreferenceInfo.DeveloperMode;
					mnuDebug.Visible = devMode;

					panelInfo.Visible = !running;
					ctrlRecentGames.Visible = !running;

					ctrlLoading.Visible = (_romLoadCounter > 0);

					UpdateFocusFlag();
					UpdateWindowTitle();

					bool isNetPlayClient = InteropEmu.IsConnected();

					mnuSaveState.Enabled = (running && !isNetPlayClient && !InteropEmu.IsNsf());
					mnuLoadState.Enabled = (running && !isNetPlayClient && !InteropEmu.IsNsf() && !InteropEmu.MoviePlaying() && !InteropEmu.MovieRecording());

					mnuPause.Text = InteropEmu.IsPaused() ? ResourceHelper.GetMessage("Resume") : ResourceHelper.GetMessage("Pause");
					mnuPause.Image = InteropEmu.IsPaused() ? _playButton : _pauseButton;

					bool netPlay = InteropEmu.IsServerRunning() || isNetPlayClient;

					mnuStartServer.Enabled = !isNetPlayClient;
					mnuConnect.Enabled = !InteropEmu.IsServerRunning();
					mnuNetPlaySelectController.Enabled = isNetPlayClient || InteropEmu.IsServerRunning();
					if(mnuNetPlaySelectController.Enabled) {
						int availableControllers = InteropEmu.NetPlayGetAvailableControllers();
						int currentControllerPort = InteropEmu.NetPlayGetControllerPort();
						mnuNetPlayPlayer1.Enabled = (availableControllers & 0x01) == 0x01;
						mnuNetPlayPlayer2.Enabled = (availableControllers & 0x02) == 0x02;
						mnuNetPlayPlayer3.Enabled = (availableControllers & 0x04) == 0x04;
						mnuNetPlayPlayer4.Enabled = (availableControllers & 0x08) == 0x08;
						mnuNetPlayPlayer1.Text = ResourceHelper.GetMessage("PlayerNumber", "1") + " (" + ResourceHelper.GetEnumText(InteropEmu.NetPlayGetControllerType(0)) + ")";
						mnuNetPlayPlayer2.Text = ResourceHelper.GetMessage("PlayerNumber", "2") + " (" + ResourceHelper.GetEnumText(InteropEmu.NetPlayGetControllerType(1)) + ")";
						mnuNetPlayPlayer3.Text = ResourceHelper.GetMessage("PlayerNumber", "3") + " (" + ResourceHelper.GetEnumText(InteropEmu.NetPlayGetControllerType(2)) + ")";
						mnuNetPlayPlayer4.Text = ResourceHelper.GetMessage("PlayerNumber", "4") + " (" + ResourceHelper.GetEnumText(InteropEmu.NetPlayGetControllerType(3)) + ")";

						mnuNetPlayPlayer1.Checked = (currentControllerPort == 0);
						mnuNetPlayPlayer2.Checked = (currentControllerPort == 1);
						mnuNetPlayPlayer3.Checked = (currentControllerPort == 2);
						mnuNetPlayPlayer4.Checked = (currentControllerPort == 3);
						mnuNetPlaySpectator.Checked = (currentControllerPort == 0xFF);

						mnuNetPlaySpectator.Enabled = true;
					}

					mnuStartServer.Text = InteropEmu.IsServerRunning() ? ResourceHelper.GetMessage("StopServer") : ResourceHelper.GetMessage("StartServer");
					mnuConnect.Text = isNetPlayClient ? ResourceHelper.GetMessage("Disconnect") : ResourceHelper.GetMessage("ConnectToServer");

					mnuCheats.Enabled = !isNetPlayClient;
					mnuEmulationSpeed.Enabled = !isNetPlayClient;
					mnuInput.Enabled = !isNetPlayClient;
					mnuRegion.Enabled = !isNetPlayClient;

					bool moviePlaying = InteropEmu.MoviePlaying();
					bool movieRecording = InteropEmu.MovieRecording();
					mnuPlayMovie.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuStopMovie.Enabled = running && !netPlay && (moviePlaying || movieRecording);
					mnuRecordFrom.Enabled = running && !moviePlaying && !movieRecording;
					mnuRecordFromStart.Enabled = running && !isNetPlayClient && !moviePlaying && !movieRecording;
					mnuRecordFromNow.Enabled = running && !moviePlaying && !movieRecording;

					bool waveRecording = InteropEmu.WaveIsRecording();
					mnuWaveRecord.Enabled = running && !waveRecording;
					mnuWaveStop.Enabled = running && waveRecording;

					bool aviRecording = InteropEmu.AviIsRecording();
					mnuAviRecord.Enabled = running && !aviRecording;
					mnuAviStop.Enabled = running && aviRecording;
					mnuVideoRecorder.Enabled = !_isNsfPlayerMode;

					bool testRecording = InteropEmu.RomTestRecording();
					mnuTestRun.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuTestStopRecording.Enabled = running && testRecording;
					mnuTestRecordStart.Enabled = running && !isNetPlayClient && !moviePlaying && !movieRecording;
					mnuTestRecordNow.Enabled = running && !moviePlaying && !movieRecording;
					mnuTestRecordMovie.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuTestRecordTest.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuTestRecordFrom.Enabled = (mnuTestRecordStart.Enabled || mnuTestRecordNow.Enabled || mnuTestRecordMovie.Enabled || mnuTestRecordTest.Enabled);

					mnuDebugger.Visible = !devMode;
					mnuHdPackEditor.Enabled = !netPlay && running;

					mnuNetPlay.Enabled = !InteropEmu.IsNsf();
					if(running && InteropEmu.IsNsf()) {
						mnuPowerCycle.Enabled = false;
						mnuMovies.Enabled = mnuPlayMovie.Enabled = mnuStopMovie.Enabled = mnuRecordFrom.Enabled = mnuRecordFromStart.Enabled = mnuRecordFromNow.Enabled = false;
					}

					mnuRegionAuto.Checked = ConfigManager.Config.Region == NesModel.Auto;
					mnuRegionNtsc.Checked = ConfigManager.Config.Region == NesModel.NTSC;
					mnuRegionPal.Checked = ConfigManager.Config.Region == NesModel.PAL;
					mnuRegionDendy.Checked = ConfigManager.Config.Region == NesModel.Dendy;

					bool autoInsertDisabled = !InteropEmu.FdsIsAutoInsertDiskEnabled();
					mnuSelectDisk.Enabled = autoInsertDisabled;

					bool isHdPackLoader = InteropEmu.IsHdPpu();
					mnuNtscFilter.Enabled = !isHdPackLoader;
					mnuNtscBisqwitQuarterFilter.Enabled = !isHdPackLoader;
					mnuNtscBisqwitHalfFilter.Enabled = !isHdPackLoader;
					mnuNtscBisqwitFullFilter.Enabled = !isHdPackLoader;
				}
			} catch { }
		}

		private void UpdateWindowTitle()
		{
			string title = "Mesen";
			if(!string.IsNullOrWhiteSpace(_currentGame)) {
				title += " - " + _currentGame;
			}
			if(ConfigManager.Config.PreferenceInfo.DisplayTitleBarInfo) {
				title += string.Format(" - {0}x{1} ({2:0.##}x, {3}) - {4}", ctrlRenderer.Width, ctrlRenderer.Height, ConfigManager.Config.VideoInfo.VideoScale, ResourceHelper.GetEnumText(ConfigManager.Config.VideoInfo.AspectRatio), ResourceHelper.GetEnumText(ConfigManager.Config.VideoInfo.VideoFilter));
			}
			this.Text = title;
		}
		
		private void StartEmuThread()
		{
			if(_emuThread == null) {
				_emuThread = new Thread(() => {
					try {
						InteropEmu.Run();
						_emuThread = null;
					} catch(Exception ex) {
						MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
						_emuThread = null;
					}
				});
				_emuThread.Start();
			}
			UpdateMenus();
		}

		private void StopEmu()
		{
			InteropEmu.Stop();
		}

		private void PauseEmu()
		{
			frmDebugger debugger = DebugWindowManager.GetDebugger();
			if(debugger != null) {
				InteropEmu.DebugStep(1);
			} else {
				if(InteropEmu.IsPaused()) {
					InteropEmu.Resume();
				} else {
					InteropEmu.Pause();
				}

				ctrlNsfPlayer.UpdateText();
			}
		}

		private void ResetEmu()
		{
			if(!ConfigManager.Config.PreferenceInfo.ConfirmExitResetPower || MesenMsgBox.Show("ConfirmReset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
				InteropEmu.Reset();
			}
		}

		private void PowerCycleEmu()
		{
			if(!ConfigManager.Config.PreferenceInfo.ConfirmExitResetPower || MesenMsgBox.Show("ConfirmPowerCycle", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
				InteropEmu.PowerCycle();
			}
		}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			if(Application.OpenForms.Count > 0 && Application.OpenForms[0].ContainsFocus) {
				if(m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP) {
					int scanCode = (Int32)(((Int64)m.LParam & 0x1FF0000) >> 16);
					InteropEmu.SetKeyState(scanCode, false);
				} else if(m.Msg == WM_SYSKEYDOWN || m.Msg == WM_KEYDOWN) {
					int scanCode = (Int32)(((Int64)m.LParam & 0x1FF0000) >> 16);
					InteropEmu.SetKeyState(scanCode, true);
				}
			}
			return false;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(!this.menuStrip.Enabled) {
				//Make sure we disable all shortcut keys while the bar is disabled (i.e when running tests)
				return false;
			}

			if(this.HideMenuStrip && (keyData & Keys.Alt) == Keys.Alt) {
				if(this.menuStrip.Visible && !this.menuStrip.ContainsFocus) {
					this.menuStrip.Visible = false;
				} else {
					this.menuStrip.Visible = true;
					this.menuStrip.Focus();
				}
			}

			#if !HIDETESTMENU
			if(keyData == Keys.Pause) {
				if(InteropEmu.RomTestRecording()) {
					InteropEmu.RomTestStop();
				} else {
					InteropEmu.RomTestRecord(ConfigManager.TestFolder + "\\" + InteropEmu.GetRomInfo().GetRomName() + ".mtp", true);
				}
			}
			#endif

			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void SelectFdsBiosPrompt()
		{
			if(MesenMsgBox.Show("FdsBiosNotFound", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
				using(OpenFileDialog ofd = new OpenFileDialog()) {
					ofd.SetFilter(ResourceHelper.GetMessage("FilterAll"));
					if(ofd.ShowDialog() == DialogResult.OK) {
						string hash = MD5Helper.GetMD5Hash(ofd.FileName).ToLowerInvariant();
						if(hash == "ca30b50f880eb660a320674ed365ef7a" || hash == "c1a9e9415a6adde3c8563c622d4c9fce") {
							File.Copy(ofd.FileName, Path.Combine(ConfigManager.HomeFolder, "FdsBios.bin"));
							LoadROM(_currentRomPath.Value, ConfigManager.Config.PreferenceInfo.AutoLoadIpsPatches);
						} else {
							MesenMsgBox.Show("InvalidFdsBios", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}
			}
		}

		private void frmMain_DragDrop(object sender, DragEventArgs e)
		{
			try {
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				if(File.Exists(files[0])) {
					LoadFile(files[0]);
					this.Activate();
				} else {
					InteropEmu.DisplayMessage("Error", "File not found: " + files[0]);
				}
			} catch(Exception ex) {
				MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
			}
		}

		private void frmMain_DragEnter(object sender, DragEventArgs e)
		{
			try {
				if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
					e.Effect = DragDropEffects.Copy;
				} else {
					InteropEmu.DisplayMessage("Error", "Unsupported operation.");
				}
			} catch(Exception ex) {
				MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
			}
		}

		private void ctrlRenderer_DoubleClick(object sender, EventArgs e)
		{
			if(!CursorManager.NeedMouseIcon && !InteropEmu.HasArkanoidPaddle()) {
				//Disable double clicking (used to switch to fullscreen mode) when using zapper/arkanoid controller
				SetFullscreenState(!_fullscreenMode);
			}
		}

		private void panelRenderer_Click(object sender, EventArgs e)
		{
			if(this.HideMenuStrip) {
				this.menuStrip.Visible = false;
			}

			ctrlRenderer.Focus();
		}

		private void ctrlRenderer_Enter(object sender, EventArgs e)
		{
			if(this.HideMenuStrip) {
				this.menuStrip.Visible = false;
			}
		}

		private void menuStrip_VisibleChanged(object sender, EventArgs e)
		{
			this.UpdateRendererLocation();
		}

		private void UpdateRendererLocation()
		{
			if(this.HideMenuStrip) {
				IntPtr handle = this.Handle;
				this.BeginInvoke((MethodInvoker)(() => {
					int rendererTop = (panelRenderer.Height + (this.menuStrip.Visible ? menuStrip.Height : 0) - ctrlRenderer.Height) / 2;
					this.ctrlRenderer.Top = rendererTop + (this.menuStrip.Visible ? -menuStrip.Height : 0);
					this.ctrlRecentGames.Top = this.menuStrip.Visible ? -menuStrip.Height : 0;
				}));
			}
		}

		private void InitializeNsfMode(bool updateTextOnly = false, bool gameLoaded = false)
		{
			if(this.InvokeRequired) {
				if(InteropEmu.IsNsf()) {
					if(InteropEmu.IsConnected()) {
						InteropEmu.Disconnect();
					}
					if(InteropEmu.IsServerRunning()) {
						InteropEmu.StopServer();
					}
				}
				this.BeginInvoke((MethodInvoker)(() => this.InitializeNsfMode(updateTextOnly, gameLoaded)));
			} else {
				if(InteropEmu.IsNsf()) {
					if(gameLoaded) {
						//Force emulation speed to 100 when loading a NSF
						SetEmulationSpeed(100);
					}

					if(!this._isNsfPlayerMode) {
						this._nonNsfSize = this.WindowState == FormWindowState.Maximized ? this.RestoreBounds.Size : this.Size;
						this.Size = new Size(380, 320);
						this.MinimumSize = new Size(380, 320);
					}
					this._isNsfPlayerMode = true;
					this.ctrlNsfPlayer.UpdateText();
					if(!updateTextOnly) {
						this.ctrlNsfPlayer.ResetCount();
					}
					this.ctrlNsfPlayer.Visible = true;					
					this.ctrlNsfPlayer.Focus();


					NsfHeader header = InteropEmu.NsfGetHeader();
					if(header.HasSongName) {
						_currentGame = header.GetSongName();
					}
				} else if(this._isNsfPlayerMode) {
					this.MinimumSize = new Size(340, 280);
					this.Size = this._nonNsfSize.Value;
					this._nonNsfSize = null;
					this._isNsfPlayerMode = false;
					this.ctrlNsfPlayer.Visible = false;
				}
			}
		}

		private void panelRenderer_MouseLeave(object sender, EventArgs e)
		{
			CursorManager.OnMouseLeave();
		}

		private void ctrlRecentGames_OnRecentGameLoaded(Controls.RecentGameInfo gameInfo)
		{
			_currentRomPath = gameInfo.RomPath;
		}
	}
}
