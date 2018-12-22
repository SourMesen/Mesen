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
using System.Text.RegularExpressions;
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
using Mesen.GUI.Properties;

namespace Mesen.GUI.Forms
{
	public partial class frmMain : BaseInputForm
	{
		private InteropEmu.NotificationListener _notifListener;
		private Thread _emuThread;
		private frmLogWindow _logWindow;
		private frmCheatList _cheatListWindow;
		private frmHistoryViewer _historyViewerWindow;
		private frmHdPackEditor _hdPackEditorWindow;
		private ResourcePath? _currentRomPath = null;

		private string _movieToRecord = null;
		private List<string> _luaScriptsToLoad = new List<string>();

		private Image _pauseButton = Resources.Pause;
		private Image _playButton = Resources.Play;
		private string _currentGame = null;
		private bool _customSize = false;
		private double? _switchOptionScale = null;
		private bool _fullscreenRequested = false;
		private FormWindowState _originalWindowState;
		private Size _originalWindowMinimumSize;
		private bool _fullscreenMode = false;
		private Size? _nonNsfSize = null;
		private Size _nonNsfMinimumSize;
		private bool _isNsfPlayerMode = false;
		private object _loadRomLock = new object();
		private int _romLoadCounter = 0;
		private bool _showUpgradeMessage = false;
		private float _yFactor = 1;
		private bool _enableResize = false;
		private bool _overrideWindowSize = false;
		private bool _shuttingDown = false;
		private bool _isDualSystem = false;

		private frmFullscreenRenderer _frmFullscreenRenderer = null;

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

#if AUTOBUILD
			string devVersion = ResourceManager.ReadZippedResource("DevBuild.txt");
			if(devVersion != null) {
				Size versionSize = TextRenderer.MeasureText(devVersion, lblVersion.Font);
				lblVersion.Text = devVersion;
				lblVersion.Anchor = AnchorStyles.Left;
				int newWidth = versionSize.Width + 30;
				panelInfo.Left -= newWidth - panelInfo.Width;
				panelInfo.Width = newWidth;
			}
#endif

			_commandLineArgs = (string[])args.Clone();

			Application.AddMessageFilter(this);
			this.Resize += ResizeRecentGames;
			this.FormClosed += (s, e) => Application.RemoveMessageFilter(this);
		}
		
		public void ProcessCommandLineArguments(List<string> switches, bool forStartup)
		{
			if(forStartup) {
				_noVideo = switches.Contains("/novideo");
				_noAudio = switches.Contains("/noaudio");
				_noInput = switches.Contains("/noinput");
			}

			if(switches.Contains("/donotsavesettings")) {
				ConfigManager.DoNotSaveSettings = true;
			}

			Regex recordMovieCommand = new Regex("/recordmovie=([^\"]+)");
			foreach(string command in switches) {
				Match match = recordMovieCommand.Match(command);
				if(match.Success) {
					string moviePath = match.Groups[1].Value;
					string folder = Path.GetDirectoryName(moviePath);
					if(string.IsNullOrWhiteSpace(folder)) {
						moviePath = Path.Combine(ConfigManager.MovieFolder, moviePath);
					} else if(!Path.IsPathRooted(moviePath)) {
						moviePath = Path.Combine(Program.OriginalFolder, moviePath);
					}
					if(!moviePath.ToLower().EndsWith(".mmo")) {
						moviePath += ".mmo";
					}
					_movieToRecord = moviePath;
					break;
				}
			}

			ConfigManager.ProcessSwitches(switches);
		}

		public void LoadGameFromCommandLine(List<string> switches)
		{
			string romPath;
			CommandLineHelper.GetRomPathFromCommandLine(switches, out romPath, out _luaScriptsToLoad);

			if(romPath != null) {
				this.LoadFile(romPath);
			} else {
				if(_emuThread == null) {
					//When no ROM is loaded, only process Lua scripts if a ROM was specified as a command line param
					_luaScriptsToLoad.Clear();
					_movieToRecord = null;
				} else {
					//No game was specified, but a game is running already, load the scripts right away
					ProcessPostLoadCommandSwitches();
				}
			}
		}

		private void ProcessPostLoadCommandSwitches()
		{
			if(_luaScriptsToLoad.Count > 0) {
				foreach(string luaScript in _luaScriptsToLoad) {
					frmScript scriptWindow = DebugWindowManager.OpenScriptWindow(true);
					scriptWindow.LoadScriptFile(luaScript);
				}
				_luaScriptsToLoad.Clear();
			}

			if(_movieToRecord != null) {
				RecordMovieOptions options = new RecordMovieOptions(_movieToRecord, "", "", RecordMovieFrom.StartWithSaveData);
				InteropEmu.MovieRecord(ref options);
				_movieToRecord = null;
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
			InteropEmu.InitDll();

			_notifListener = new InteropEmu.NotificationListener(InteropEmu.ConsoleId.Master);
			_notifListener.OnNotification += _notifListener_OnNotification;

			menuTimer.Start();

			InteropEmu.ScreenSize originalSize = InteropEmu.GetScreenSize(false);
			VideoInfo.ApplyConfig();
			this.ProcessCommandLineArguments(CommandLineHelper.PreprocessCommandLineArguments(_commandLineArgs, true), true);
			VideoInfo.ApplyConfig();
			InteropEmu.ScreenSize newSize = InteropEmu.GetScreenSize(false);
			if(originalSize.Width != newSize.Width || originalSize.Height != newSize.Height) {
				_overrideWindowSize = true;
			}

			InitializeEmulationSpeedMenu();

			UpdateVideoSettings();

			InitializeCore();
			PerformUpgrade();
			InitializeEmu();

			TopMost = ConfigManager.Config.PreferenceInfo.AlwaysOnTop;

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

			if(ConfigManager.Config.WindowSize.HasValue && !_overrideWindowSize) {
				this.ClientSize = ConfigManager.Config.WindowSize.Value;
			}

			if(ConfigManager.Config.PreferenceInfo.DisableMouseResize) {
				this.FormBorderStyle = FormBorderStyle.Fixed3D;
			}

			mnuDebugDualSystemSecondaryCpu.Checked = ConfigManager.Config.DebugInfo.DebugConsoleId == InteropEmu.ConsoleId.Slave;
			InteropEmu.DebugSetDebuggerConsole(ConfigManager.Config.DebugInfo.DebugConsoleId);
		}

		private void ProcessFullscreenSwitch(List<string> switches)
		{
			if(switches.Contains("/fullscreen")) {
				double scale = ConfigManager.Config.VideoInfo.VideoScale;
				if(!ConfigManager.Config.VideoInfo.UseExclusiveFullscreen) {
					//Go into fullscreen mode right away
					SetFullscreenState(true);
				}

				_fullscreenRequested = true;
				foreach(string option in switches) {
					if(option.StartsWith("/videoscale=")) {
						_switchOptionScale = scale;
					}
				}
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.BindShortcuts();

			if(ConfigManager.Config.WindowSize.HasValue && !_overrideWindowSize) {
				this.Size = ConfigManager.Config.WindowSize.Value;
			}

			this.LoadGameFromCommandLine(CommandLineHelper.PreprocessCommandLineArguments(_commandLineArgs, false));

			this.menuStrip.VisibleChanged += new System.EventHandler(this.menuStrip_VisibleChanged);
			this.UpdateRendererLocation();

			if(_showUpgradeMessage) {
				MesenMsgBox.Show("UpgradeSuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
				_showUpgradeMessage = false;
			}

			//Ensure the resize event is not fired until the form is fully shown
			//This is needed when DPI display settings is not set to 100%
			_enableResize = true;

			ProcessFullscreenSwitch(CommandLineHelper.PreprocessCommandLineArguments(_commandLineArgs, true));
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if(ConfigManager.Config.PreferenceInfo.ConfirmExitResetPower && MesenMsgBox.Show("ConfirmExit", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) {
				e.Cancel = true;
				return;
			}

			if(!_shuttingDown && Program.IsMono) {
				//This appears to prevent Mono from locking up when closing the form
				Task.Run(() => {
					StopEmu();
					_shuttingDown = true;
					this.BeginInvoke((Action)(() => this.Close()));
				});
				e.Cancel = true;
				return;
			}

			_shuttingDown = true;
			BaseForm.StopBackgroundTimer();
			_logWindow?.Close();
			_historyViewerWindow?.Close();
			_cheatListWindow?.Close();
			_hdPackEditorWindow?.Close();
			_frmFullscreenRenderer?.Close();

			//Stop menu update timer, and process all pending events before stopping the core
			//This prevents some rare crashes on shutdown
			menuTimer.Stop();
			Application.DoEvents();

			if(_notifListener != null) {
				_notifListener.Dispose();
				_notifListener = null;
			}
			DebugWindowManager.CloseAll();

			ConfigManager.Config.EmulationInfo.EmulationSpeed = InteropEmu.GetEmulationSpeed();
			ConfigManager.Config.WindowLocation = this.WindowState == FormWindowState.Normal ? this.Location : this.RestoreBounds.Location;
			ConfigManager.Config.WindowSize = this.WindowState == FormWindowState.Normal ? this.Size : this.RestoreBounds.Size;

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

		void InitializeCore()
		{
			InteropEmu.InitializeEmu(ConfigManager.HomeFolder, this.Handle, ctrlRenderer.Handle, _noAudio, _noVideo, _noInput);
		}

		void InitializeEmu()
		{
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

		private void UpdateViewerSize(bool forceUpdate = false)
		{
			this.Resize -= frmMain_Resize;

			InteropEmu.ScreenSize size = InteropEmu.GetScreenSize(false);

			if(forceUpdate || (!_customSize && this.WindowState != FormWindowState.Maximized)) {
				Size sizeGap = this.Size - this.ClientSize;

				bool doubleScreenMode = _isDualSystem && ConfigManager.Config.PreferenceInfo.VsDualVideoOutput == VsDualOutputOption.Both;
				int width = doubleScreenMode ? (size.Width * 2) : size.Width;

				UpdateScaleMenu(size.Scale);
				this.ClientSize = new Size(Math.Max(this.MinimumSize.Width - sizeGap.Width, width), Math.Max(this.MinimumSize.Height - sizeGap.Height, size.Height + (this.HideMenuStrip ? 0 : menuStrip.Height)));
			}

			ctrlRenderer.Size = new Size(size.Width, size.Height);
			UpdateRendererPosition();

			if(this.HideMenuStrip) {
				this.menuStrip.Visible = false;
			}

			this.Resize += frmMain_Resize;
		}

		private void UpdateRendererPosition()
		{
			ctrlRenderer.Top = (panelRenderer.Height - ctrlRenderer.Height) / 2;

			if(_isDualSystem) {
				UpdateDualSystemViewer();
			} else {
				ctrlRenderer.Left = (panelRenderer.Width - ctrlRenderer.Width) / 2;
				ctrlRendererDualSystem.Visible = false;
			}
		}

		private void UpdateDualSystemViewer()
		{
			ctrlRendererDualSystem.Size = new Size(ctrlRenderer.Width, ctrlRenderer.Height);
			ctrlRendererDualSystem.Top = (panelRenderer.Height - ctrlRenderer.Height) / 2;

			if(ConfigManager.Config.PreferenceInfo.VsDualVideoOutput == VsDualOutputOption.Both) {
				ctrlRenderer.Left = (panelRenderer.Width / 2 - ctrlRenderer.Width) / 2;
				ctrlRendererDualSystem.Left = ctrlRenderer.Left + ctrlRenderer.Width;

				ctrlRendererDualSystem.Visible = true;
			} else {
				ctrlRenderer.Left = (panelRenderer.Width - ctrlRenderer.Width) / 2;
				ctrlRendererDualSystem.Left = (panelRenderer.Width - ctrlRenderer.Width) / 2;

				if(ConfigManager.Config.PreferenceInfo.VsDualVideoOutput == VsDualOutputOption.SlaveOnly) {
					ctrlRendererDualSystem.Visible = true;
					ctrlRendererDualSystem.BringToFront();
				} else {
					ctrlRendererDualSystem.Visible = false;
				}
			}
		}

		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			_yFactor = factor.Height;
			base.ScaleControl(factor, specified);
		}

		private void ResizeRecentGames(object sender, EventArgs e)
		{
			if(this.ClientSize.Height < 400 * _yFactor) {
				ctrlRecentGames.Height = this.ClientSize.Height - (int)((125 - Math.Min(50, 400 - (int)(this.ClientSize.Height / _yFactor))) * _yFactor);
			} else {
				ctrlRecentGames.Height = this.ClientSize.Height - (int)(125 * _yFactor);
			}
			ctrlRecentGames.Width = this.ClientSize.Width;
			ctrlRecentGames.Top = (this.HideMenuStrip && this.menuStrip.Visible) ? -menuStrip.Height : 0;
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			if(_enableResize && this.WindowState != FormWindowState.Minimized) {
				SetScaleBasedOnWindowSize();
				UpdateRendererPosition();
			}
		}

		private void SetScaleBasedOnDimensions(Size dimensions, bool allowVsDualScreen)
		{
			_customSize = true;
			InteropEmu.ScreenSize size = InteropEmu.GetScreenSize(true);

			if(allowVsDualScreen && _isDualSystem && ConfigManager.Config.PreferenceInfo.VsDualVideoOutput == VsDualOutputOption.Both) {
				size = new InteropEmu.ScreenSize() {
					Width = size.Width * 2,
					Height = size.Height,
					Scale = size.Scale
				};
			}

			double verticalScale = (double)dimensions.Height / size.Height;
			double horizontalScale = (double)dimensions.Width / size.Width;
			double scale = Math.Min(verticalScale, horizontalScale);
			if(_fullscreenMode && ConfigManager.Config.VideoInfo.FullscreenForceIntegerScale) {
				scale = Math.Floor(scale);
			}
			UpdateScaleMenu(scale);
			VideoInfo.ApplyConfig();
		}

		private void SetScaleBasedOnWindowSize()
		{
			SetScaleBasedOnDimensions(panelRenderer.ClientSize, true);
		}

		private void SetScaleBasedOnScreenSize()
		{
			SetScaleBasedOnDimensions(Screen.FromControl(this).Bounds.Size, false);
		}

		private void StopExclusiveFullscreenMode()
		{
			if(_frmFullscreenRenderer != null) {
				_frmFullscreenRenderer.Close();
			}
		}

		private void StartExclusiveFullscreenMode()
		{
			Size screenSize = Screen.FromControl(this).Bounds.Size;
			_frmFullscreenRenderer = new frmFullscreenRenderer();
			_frmFullscreenRenderer.Shown += (object sender, EventArgs e) => {
				ctrlRenderer.Visible = false;
				SetScaleBasedOnScreenSize();
				InteropEmu.SetFullscreenMode(true, _frmFullscreenRenderer.Handle, (UInt32)screenSize.Width, (UInt32)screenSize.Height);
			};
			_frmFullscreenRenderer.FormClosing += (object sender, FormClosingEventArgs e) => {
				InteropEmu.SetFullscreenMode(false, ctrlRenderer.Handle, (UInt32)screenSize.Width, (UInt32)screenSize.Height);
				_frmFullscreenRenderer = null;
				ctrlRenderer.Visible = true;
				_fullscreenMode = false;
				frmMain_Resize(null, EventArgs.Empty);
			};

			Screen currentScreen = Screen.FromHandle(this.Handle);
			_frmFullscreenRenderer.StartPosition = FormStartPosition.Manual;
			_frmFullscreenRenderer.Top = currentScreen.Bounds.Top;
			_frmFullscreenRenderer.Left = currentScreen.Bounds.Left;
			_frmFullscreenRenderer.Show();
		}

		private void StartFullscreenWindowMode(bool saveState)
		{
			this.menuStrip.Visible = false;
			if(saveState) {
				_originalWindowState = this.WindowState;
				_originalWindowMinimumSize = this.MinimumSize;
			}
			this.MinimumSize = new Size(0, 0);
			this.WindowState = FormWindowState.Normal;
			this.FormBorderStyle = FormBorderStyle.None;
			this.WindowState = FormWindowState.Maximized;
			SetScaleBasedOnWindowSize();
		}

		private void StopFullscreenWindowMode()
		{
			this.menuStrip.Visible = true;
			this.WindowState = _originalWindowState;
			this.MinimumSize = _originalWindowMinimumSize;
			this.FormBorderStyle = ConfigManager.Config.PreferenceInfo.DisableMouseResize ? FormBorderStyle.Fixed3D : FormBorderStyle.Sizable;
			this.frmMain_Resize(null, EventArgs.Empty);
		}

		private void SetFullscreenState(bool enabled)
		{
			if(this._isNsfPlayerMode) {
				enabled = false;
			}

			//Setup message to show on screen when paused while in fullscreen (instructions to revert to windowed mode)
			InteropEmu.SetPauseScreenMessage("");
			if(enabled) {
				ShortcutKeyInfo shortcut = ConfigManager.Config.PreferenceInfo.ShortcutKeys1.Find(ski => ski.Shortcut == EmulatorShortcut.ToggleFullscreen);
				if(shortcut == null || shortcut.KeyCombination.IsEmpty) {
					shortcut = ConfigManager.Config.PreferenceInfo.ShortcutKeys2.Find(ski => ski.Shortcut == EmulatorShortcut.ToggleFullscreen);
				}

				if(shortcut != null && !shortcut.KeyCombination.IsEmpty) {
					InteropEmu.SetPauseScreenMessage(ResourceHelper.GetMessage("PressToExitFullscreen", shortcut.KeyCombination.ToString()));
				}
			}

			bool saveState = !_fullscreenMode;
			_fullscreenMode = enabled;
			mnuFullscreen.Checked = enabled;

			if(ConfigManager.Config.VideoInfo.UseExclusiveFullscreen) {
				if(_emuThread != null) {
					if(enabled) {
						StartExclusiveFullscreenMode();
					} else {
						StopExclusiveFullscreenMode();
					}
				}
			} else {
				this.Resize -= frmMain_Resize;
				if(enabled) {
					StartFullscreenWindowMode(saveState);
				} else {
					StopFullscreenWindowMode();
				}
				this.Resize += frmMain_Resize;
				UpdateViewerSize();
			}
		}

		private bool HideMenuStrip
		{
			get
			{
				return (_fullscreenMode && !ConfigManager.Config.VideoInfo.UseExclusiveFullscreen) || ConfigManager.Config.PreferenceInfo.AutoHideMenu;
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
			CursorManager.CaptureMouse();
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.GameLoaded:
					VideoInfo.ApplyOverscanConfig();
					_currentGame = InteropEmu.GetRomInfo().GetRomName();
					InteropEmu.SetNesModel(ConfigManager.Config.Region);
					InitializeNsfMode(false, true);
					CheatInfo.ApplyCheats();
					GameSpecificInfo.ApplyGameSpecificConfig();
					UpdateStateMenu(mnuSaveState, true);
					UpdateStateMenu(mnuLoadState, false);
					if(ConfigManager.Config.PreferenceInfo.ShowVsConfigOnLoad && InteropEmu.IsVsSystem()) {
						this.Invoke((MethodInvoker)(() => {
							this.ShowGameConfig();
						}));
					}

					this.StartEmuThread();
					this.BeginInvoke((MethodInvoker)(() => {
						UpdateViewerSize();
						ProcessPostLoadCommandSwitches();
					}));

					Task.Run(() => {
						//If a workspace is already loaded for this game, make sure we setup the labels, watch, etc properly
						DebugWorkspaceManager.SetupWorkspace();
					});
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
					this.BeginInvoke((MethodInvoker)(() => {
						ConfigManager.Config.ApplyConfig();
					}));
					break;

				case InteropEmu.ConsoleNotificationType.GameStopped:
					this._currentGame = null;
					InitializeNsfMode();
					CheatInfo.ClearCheats();
					this.BeginInvoke((MethodInvoker)(() => {
						if(_hdPackEditorWindow != null) {
							_hdPackEditorWindow.Close();
						}
						if(!ConfigManager.Config.PreferenceInfo.DisableGameSelectionScreen) {
							ctrlRecentGames.Initialize();
						}
						if(e.Parameter == IntPtr.Zero) {
							//We are completely stopping the emulation, close fullscreen mode
							StopExclusiveFullscreenMode();
						}
					}));
					break;

				case InteropEmu.ConsoleNotificationType.BeforeEmulationStop:
					this.Invoke((Action)(() => {
						DebugWindowManager.CloseAll();
					}));
					break;

				case InteropEmu.ConsoleNotificationType.ResolutionChanged:
					this.BeginInvoke((MethodInvoker)(() => {
						ProcessResolutionChanged();
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

				case InteropEmu.ConsoleNotificationType.VsDualSystemStarted:
					_isDualSystem = true;
					this.BeginInvoke((MethodInvoker)(() => {
						UpdateViewerSize(true);
						InteropEmu.InitializeDualSystem(this.Handle, ctrlRendererDualSystem.Handle);
					}));
					break;

				case InteropEmu.ConsoleNotificationType.VsDualSystemStopped:
					_isDualSystem = false;
					InteropEmu.ReleaseDualSystemAudioVideo();
					this.BeginInvoke((MethodInvoker)(() => {
						UpdateViewerSize(true);
					}));
					break;
			}

			if(e.NotificationType != InteropEmu.ConsoleNotificationType.PpuFrameDone) {
				UpdateMenus();
			}
		}

		private void ProcessResolutionChanged()
		{
			//Force scale specified by command line options, when using /fullscreen
			if(_fullscreenRequested) {
				SetFullscreenState(true);
				if(_switchOptionScale.HasValue) {
					//If a VideoScale is specified in command line, apply it
					SetScale(_switchOptionScale.Value);
				}
				_fullscreenRequested = false;
			} else if(_switchOptionScale.HasValue) {
				//For exclusive fullscreen, resolution changed will be called twice, need to set the scale one more time here
				SetScale(_switchOptionScale.Value);
				_switchOptionScale = null;
			} else {
				UpdateViewerSize();
			}
		}

		private void BindShortcuts()
		{
			Func<bool> notClient = () => { return !InteropEmu.IsConnected(); };
			Func<bool> runningNotClient = () => { return _emuThread != null && !InteropEmu.IsConnected(); };
			Func<bool> runningNotClientNotMovie = () => { return _emuThread != null && !InteropEmu.IsConnected() && !InteropEmu.MoviePlaying(); };

			Func<bool> runningNotNsf = () => { return _emuThread != null && !InteropEmu.IsNsf(); };
			Func<bool> runningFdsNoAutoInsert = () => { return _emuThread != null && InteropEmu.FdsGetSideCount() > 0 && !InteropEmu.FdsIsAutoInsertDiskEnabled() && !InteropEmu.MoviePlaying() && !InteropEmu.IsConnected(); };
			Func<bool> runningFdsMultipleDisks = () => { return runningFdsNoAutoInsert() && InteropEmu.FdsGetSideCount() > 1; };
			Func<bool> runningVsSystem = () => { return _emuThread != null && InteropEmu.IsVsSystem() && !InteropEmu.MoviePlaying() && !InteropEmu.IsConnected(); };
			Func<bool> runningVsDualSystem = () => { return runningVsSystem() && InteropEmu.IsVsDualSystem(); };
			Func<bool> hasBarcodeReader = () => { return InteropEmu.GetAvailableFeatures().HasFlag(ConsoleFeatures.BarcodeReader) && !InteropEmu.IsConnected(); };

			Func<bool> enableLoadLastSession = () => {
				string recentGameFile = _currentRomPath.HasValue ? Path.Combine(ConfigManager.RecentGamesFolder, Path.GetFileNameWithoutExtension(_currentRomPath.Value.FileName) + ".rgd") : "";
				return _emuThread != null && File.Exists(recentGameFile) && !ConfigManager.Config.PreferenceInfo.DisableGameSelectionScreen;
			};
			
			BindShortcut(mnuOpen, EmulatorShortcut.OpenFile);
			BindShortcut(mnuExit, EmulatorShortcut.Exit);
			BindShortcut(mnuIncreaseSpeed, EmulatorShortcut.IncreaseSpeed, notClient);
			BindShortcut(mnuDecreaseSpeed, EmulatorShortcut.DecreaseSpeed, notClient);
			BindShortcut(mnuEmuSpeedMaximumSpeed, EmulatorShortcut.MaxSpeed, notClient);

			BindShortcut(mnuPause, EmulatorShortcut.Pause, runningNotClient);
			BindShortcut(mnuReset, EmulatorShortcut.Reset, runningNotClientNotMovie);
			BindShortcut(mnuPowerCycle, EmulatorShortcut.PowerCycle, runningNotClientNotMovie);
			BindShortcut(mnuPowerOff, EmulatorShortcut.PowerOff, runningNotClient);

			BindShortcut(mnuSwitchDiskSide, EmulatorShortcut.SwitchDiskSide, runningFdsMultipleDisks);
			BindShortcut(mnuEjectDisk, EmulatorShortcut.EjectDisk, runningFdsNoAutoInsert);

			BindShortcut(mnuInsertCoin1, EmulatorShortcut.InsertCoin1, runningVsSystem);
			BindShortcut(mnuInsertCoin2, EmulatorShortcut.InsertCoin2, runningVsSystem);
			BindShortcut(mnuInsertCoin3, EmulatorShortcut.InsertCoin3, runningVsDualSystem);
			BindShortcut(mnuInsertCoin4, EmulatorShortcut.InsertCoin4, runningVsDualSystem);

			BindShortcut(mnuInputBarcode, EmulatorShortcut.InputBarcode, hasBarcodeReader);

			BindShortcut(mnuShowFPS, EmulatorShortcut.ToggleFps);

			BindShortcut(mnuScale1x, EmulatorShortcut.SetScale1x);
			BindShortcut(mnuScale2x, EmulatorShortcut.SetScale2x);
			BindShortcut(mnuScale3x, EmulatorShortcut.SetScale3x);
			BindShortcut(mnuScale4x, EmulatorShortcut.SetScale4x);
			BindShortcut(mnuScale5x, EmulatorShortcut.SetScale5x);
			BindShortcut(mnuScale6x, EmulatorShortcut.SetScale6x);

			BindShortcut(mnuFullscreen, EmulatorShortcut.ToggleFullscreen);

			BindShortcut(mnuLoadLastSession, EmulatorShortcut.LoadLastSession, enableLoadLastSession);

			BindShortcut(mnuTakeScreenshot, EmulatorShortcut.TakeScreenshot, runningNotNsf);
			BindShortcut(mnuRandomGame, EmulatorShortcut.LoadRandomGame);

			mnuDebugDebugger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenDebugger));
			mnuDebugger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenDebugger));
			mnuApuViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenApuViewer));
			mnuAssembler.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenAssembler));
			mnuMemoryViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenMemoryTools));
			mnuEventViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenEventViewer));
			mnuPpuViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenPpuViewer));
			mnuScriptWindow.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenScriptWindow));
			mnuTraceLogger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenTraceLogger));
			mnuTextHooker.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenTextHooker));
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

			bool restoreFullscreen = _frmFullscreenRenderer != null;

			switch(shortcut) {
				case EmulatorShortcut.Pause: PauseEmu(); break;
				case EmulatorShortcut.Reset: this.ResetEmu(); break;
				case EmulatorShortcut.PowerCycle: this.PowerCycleEmu(); break;
				case EmulatorShortcut.PowerOff: Task.Run(() => InteropEmu.Stop()); break;
				case EmulatorShortcut.Exit: this.Close(); break;

				case EmulatorShortcut.ToggleCheats: ToggleCheats(); break;
				case EmulatorShortcut.ToggleAudio: ToggleAudio(); break;
				case EmulatorShortcut.ToggleFps: ToggleFps(); break;
				case EmulatorShortcut.ToggleBackground: ToggleBackground(); break;
				case EmulatorShortcut.ToggleSprites: ToggleSprites(); break;
				case EmulatorShortcut.ToggleGameTimer: ToggleGameTimer(); break;
				case EmulatorShortcut.ToggleFrameCounter: ToggleFrameCounter(); break;
				case EmulatorShortcut.ToggleLagCounter: ToggleLagCounter(); break;
				case EmulatorShortcut.ToggleOsd: ToggleOsd(); break;
				case EmulatorShortcut.ToggleAlwaysOnTop: ToggleAlwaysOnTop(); break;
				case EmulatorShortcut.ToggleDebugInfo: ToggleDebugInfo(); break;
				case EmulatorShortcut.MaxSpeed: ToggleMaxSpeed(); break;
				case EmulatorShortcut.ToggleFullscreen: ToggleFullscreen(); restoreFullscreen = false; break;

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
				case EmulatorShortcut.InsertCoin3: InteropEmu.VsInsertCoin(2); break;
				case EmulatorShortcut.InsertCoin4: InteropEmu.VsInsertCoin(3); break;

				case EmulatorShortcut.InputBarcode:
					using(frmInputBarcode frm = new frmInputBarcode()) {
						frm.ShowDialog(this, this);
					}
					break;

				case EmulatorShortcut.TakeScreenshot: InteropEmu.TakeScreenshot(); break;
				case EmulatorShortcut.LoadRandomGame: LoadRandomGame(); break;

				case EmulatorShortcut.LoadStateFromFile: LoadStateFromFile(); break;
				case EmulatorShortcut.SaveStateToFile: SaveStateToFile(); break;

				case EmulatorShortcut.SaveStateSlot1: SaveState(1); break;
				case EmulatorShortcut.SaveStateSlot2: SaveState(2); break;
				case EmulatorShortcut.SaveStateSlot3: SaveState(3); break;
				case EmulatorShortcut.SaveStateSlot4: SaveState(4); break;
				case EmulatorShortcut.SaveStateSlot5: SaveState(5); break;
				case EmulatorShortcut.SaveStateSlot6: SaveState(6); break;
				case EmulatorShortcut.SaveStateSlot7: SaveState(7); break;
				case EmulatorShortcut.SaveStateSlot8: SaveState(8); break;
				case EmulatorShortcut.SaveStateSlot9: SaveState(9); break;
				case EmulatorShortcut.SaveStateSlot10: SaveState(10); break;
				case EmulatorShortcut.LoadStateSlot1: LoadState(1); break;
				case EmulatorShortcut.LoadStateSlot2: LoadState(2); break;
				case EmulatorShortcut.LoadStateSlot3: LoadState(3); break;
				case EmulatorShortcut.LoadStateSlot4: LoadState(4); break;
				case EmulatorShortcut.LoadStateSlot5: LoadState(5); break;
				case EmulatorShortcut.LoadStateSlot6: LoadState(6); break;
				case EmulatorShortcut.LoadStateSlot7: LoadState(7); break;
				case EmulatorShortcut.LoadStateSlot8: LoadState(8); break;
				case EmulatorShortcut.LoadStateSlot9: LoadState(9); break;
				case EmulatorShortcut.LoadStateSlot10: LoadState(10); break;
				case EmulatorShortcut.LoadStateSlotAuto: LoadState(11); break;

				case EmulatorShortcut.LoadLastSession: LoadLastSession(); break;
			}

			if(restoreFullscreen && _frmFullscreenRenderer == null && !_shuttingDown) {
				//Need to restore fullscreen mode after showing a dialog
				this.SetFullscreenState(true);
			}
		}

		private void ToggleFullscreen()
		{
			SetFullscreenState(!_fullscreenMode);
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

		private void ToggleAudio()
		{
			ConfigManager.Config.AudioInfo.EnableAudio = !ConfigManager.Config.AudioInfo.EnableAudio;
			AudioInfo.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void ToggleFrameCounter()
		{
			ConfigManager.Config.PreferenceInfo.ShowFrameCounter = !ConfigManager.Config.PreferenceInfo.ShowFrameCounter;
			PreferenceInfo.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void ToggleLagCounter()
		{
			ConfigManager.Config.EmulationInfo.ShowLagCounter = !ConfigManager.Config.EmulationInfo.ShowLagCounter;
			EmulationInfo.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void ToggleGameTimer()
		{
			ConfigManager.Config.PreferenceInfo.ShowGameTimer = !ConfigManager.Config.PreferenceInfo.ShowGameTimer;
			PreferenceInfo.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void ToggleOsd()
		{
			ConfigManager.Config.PreferenceInfo.DisableOsd = !ConfigManager.Config.PreferenceInfo.DisableOsd;
			PreferenceInfo.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void ToggleAlwaysOnTop()
		{
			ConfigManager.Config.PreferenceInfo.AlwaysOnTop = !ConfigManager.Config.PreferenceInfo.AlwaysOnTop;
			ConfigManager.ApplyChanges();
			this.TopMost = ConfigManager.Config.PreferenceInfo.AlwaysOnTop;
		}

		private void ToggleSprites()
		{
			ConfigManager.Config.VideoInfo.DisableSprites = !ConfigManager.Config.VideoInfo.DisableSprites;
			VideoInfo.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void ToggleBackground()
		{
			ConfigManager.Config.VideoInfo.DisableBackground = !ConfigManager.Config.VideoInfo.DisableBackground;
			VideoInfo.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void ToggleDebugInfo()
		{
			ConfigManager.Config.PreferenceInfo.DisplayDebugInfo = !ConfigManager.Config.PreferenceInfo.DisplayDebugInfo;
			PreferenceInfo.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void ToggleCheats()
		{
			ConfigManager.Config.DisableAllCheats = !ConfigManager.Config.DisableAllCheats;
			if(ConfigManager.Config.DisableAllCheats) {
				InteropEmu.DisplayMessage("Cheats", "CheatsDisabled");
			}
			CheatInfo.ApplyCheats();
			ConfigManager.ApplyChanges();
		}

		private void UpdateMenus()
		{
			if(_shuttingDown) {
				return;
			}

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

						bool isFamicom = InteropEmu.GetConsoleType() == ConsoleType.Famicom;
						mnuNetPlayPlayer5.Visible = isFamicom;
						mnuNetPlayPlayer5.Enabled = (availableControllers & 0x10) == 0x10 && InteropEmu.GetExpansionDevice() != InteropEmu.ExpansionPortDevice.FourPlayerAdapter;

						mnuNetPlayPlayer1.Text = ResourceHelper.GetMessage("PlayerNumber", "1") + " (" + ResourceHelper.GetEnumText(InteropEmu.NetPlayGetControllerType(0)) + ")";
						mnuNetPlayPlayer2.Text = ResourceHelper.GetMessage("PlayerNumber", "2") + " (" + ResourceHelper.GetEnumText(InteropEmu.NetPlayGetControllerType(1)) + ")";
						mnuNetPlayPlayer3.Text = ResourceHelper.GetMessage("PlayerNumber", "3") + " (" + ResourceHelper.GetEnumText(InteropEmu.NetPlayGetControllerType(2)) + ")";
						mnuNetPlayPlayer4.Text = ResourceHelper.GetMessage("PlayerNumber", "4") + " (" + ResourceHelper.GetEnumText(InteropEmu.NetPlayGetControllerType(3)) + ")";
						mnuNetPlayPlayer5.Text = ResourceHelper.GetMessage("ExpansionDevice") + " (" + ResourceHelper.GetEnumText(InteropEmu.GetExpansionDevice()) + ")";

						mnuNetPlayPlayer1.Checked = (currentControllerPort == 0);
						mnuNetPlayPlayer2.Checked = (currentControllerPort == 1);
						mnuNetPlayPlayer3.Checked = (currentControllerPort == 2);
						mnuNetPlayPlayer4.Checked = (currentControllerPort == 3);
						mnuNetPlayPlayer5.Checked = (currentControllerPort == 4);
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
					mnuRecordMovie.Enabled = running && !moviePlaying && !movieRecording && !isNetPlayClient;
					mnuGameConfig.Enabled = !moviePlaying && !movieRecording;
					mnuHistoryViewer.Enabled = running && !InteropEmu.IsNsf() && InteropEmu.HistoryViewerEnabled();

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

					bool tapeRecording = InteropEmu.IsRecordingTapeFile();
					mnuTapeRecorder.Enabled = !isNetPlayClient;
					mnuLoadTapeFile.Enabled = !isNetPlayClient;
					mnuStartRecordTapeFile.Enabled = !tapeRecording && !isNetPlayClient;
					mnuStopRecordTapeFile.Enabled = tapeRecording;

					mnuDebugger.Visible = !devMode;
					mnuDebugger.Enabled = !devMode && running;
					mnuDebugDebugger.Enabled = devMode && running;
					mnuApuViewer.Enabled = running;
					mnuAssembler.Enabled = running;
					mnuMemoryViewer.Enabled = running;
					mnuEventViewer.Enabled = running;
					mnuPpuViewer.Enabled = running;
					mnuScriptWindow.Enabled = running;
					mnuTextHooker.Enabled = running;
					mnuTraceLogger.Enabled = running;

#if !HIDETESTMENU
					//Keep this option hidden for now, until some remaining issues are fixed.
					mnuDebugDualSystemSecondaryCpu.Visible = _isDualSystem;
					sepDebugDualSystemSecondaryCpu.Visible = _isDualSystem;
#else
					mnuDebugDualSystemSecondaryCpu.Visible = false;
					sepDebugDualSystemSecondaryCpu.Visible = false;
#endif

					mnuInstallHdPack.Enabled = running;
					mnuHdPackEditor.Enabled = !netPlay && running;

					mnuNetPlay.Enabled = !InteropEmu.IsNsf();
					if(running && InteropEmu.IsNsf()) {
						mnuPowerCycle.Enabled = false;
						mnuMovies.Enabled = mnuPlayMovie.Enabled = mnuStopMovie.Enabled = mnuRecordMovie.Enabled = false;
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
				debugger.TogglePause();
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
					if(ofd.ShowDialog(this) == DialogResult.OK) {
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
				if(e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) {
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
			if(!CursorManager.NeedMouseIcon && !CursorManager.AllowMouseCapture && !DebugWindowManager.ScriptWindowOpened) {
				//Disable double clicking (used to switch to fullscreen mode) when using a mouse-controlled device
				ToggleFullscreen();
			}
		}

		private void panelRenderer_Click(object sender, EventArgs e)
		{
			if(this.HideMenuStrip) {
				this.menuStrip.Visible = false;
			}
			CursorManager.CaptureMouse();

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
					this.SetFullscreenState(false);

					if(gameLoaded) {
						//Force emulation speed to 100 when loading a NSF
						SetEmulationSpeed(100);
					}

					if(!this._isNsfPlayerMode) {
						this._nonNsfSize = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Size : this.Size;
						this._nonNsfMinimumSize = this.MinimumSize;
						this.Size = ctrlNsfPlayer.WindowMinimumSize;
						this.MinimumSize = ctrlNsfPlayer.WindowMinimumSize;
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
					this.MinimumSize = this._nonNsfMinimumSize;
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

		private void ctrlNsfPlayer_MouseMove(object sender, MouseEventArgs e)
		{
			if(this.HideMenuStrip && !this.menuStrip.ContainsFocus) {
				Point nsfPlayerPosition = this.ctrlNsfPlayer.PointToClient(((Control)sender).PointToScreen(e.Location));
				this.menuStrip.Visible = nsfPlayerPosition.Y < 30;
			}
		}

		private void mnu_DropDownOpened(object sender, EventArgs e)
		{
			_inMenu++;
		}

		private void mnu_DropDownClosed(object sender, EventArgs e)
		{
			Task.Run(() => {
				Thread.Sleep(100);
				this.BeginInvoke((Action)(() => {
					_inMenu--;
				}));
			});
		}
	}
}
