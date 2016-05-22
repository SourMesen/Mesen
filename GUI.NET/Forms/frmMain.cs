using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml;
using Mesen.GUI.Config;
using Mesen.GUI.Debugger;
using Mesen.GUI.Forms.Cheats;
using Mesen.GUI.Forms.Config;
using Mesen.GUI.Forms.NetPlay;
using Mesen.GUI.GoogleDriveIntegration;

namespace Mesen.GUI.Forms
{
	public partial class frmMain : BaseForm
	{
		private InteropEmu.NotificationListener _notifListener;
		private Thread _emuThread;
		private frmDebugger _debugger;
		private string _romToLoad = null;
		private string _currentGame = null;
		private bool _customSize = false;
		private bool _fullscreenMode = false;
		private double _regularScale = ConfigManager.Config.VideoInfo.VideoScale;

		public frmMain(string[] args)
		{
			if(args.Length > 0 && File.Exists(args[0])) {
				_romToLoad = args[0];
			}

			InitializeComponent();
		}

		public void ProcessCommandLineArguments(string[] args)
		{
			if(args.Length > 0 && File.Exists(args[0])) {
				this.LoadFile(args[0]);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			#if HIDETESTMENU
			mnuTests.Visible = false;
			#endif

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			menuTimer.Start();

			VideoInfo.ApplyConfig();
			InitializeVsSystemMenu();
			InitializeFdsDiskMenu();
			InitializeEmulationSpeedMenu();
			
			UpdateVideoSettings();

			InitializeEmu();

			UpdateMenus();
			UpdateRecentFiles();

			UpdateViewerSize();

			if(ConfigManager.Config.PreferenceInfo.CloudSaveIntegration) {
				Task.Run(() => CloudSyncHelper.Sync());
			}

			if(_romToLoad != null) {
				LoadFile(this._romToLoad);
			}

			if(ConfigManager.Config.PreferenceInfo.AutomaticallyCheckForUpdates) {
				CheckForUpdates(false);
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			PerformUpgrade();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if(_notifListener != null) {
				_notifListener.Dispose();
				_notifListener = null;
			}
			if(_debugger != null) {
				_debugger.Close();
			}

			ConfigManager.Config.VideoInfo.VideoScale = _regularScale;
			ConfigManager.ApplyChanges();

			StopEmu();

			if(ConfigManager.Config.PreferenceInfo.CloudSaveIntegration) {
				CloudSyncHelper.Sync();
			}

			InteropEmu.Release();

			base.OnClosing(e);
		}

		void PerformUpgrade()
		{
			if(new Version(ConfigManager.Config.MesenVersion) < new Version(InteropEmu.GetMesenVersion())) {
				//Upgrade
				ConfigManager.Config.MesenVersion = InteropEmu.GetMesenVersion();
				ConfigManager.ApplyChanges();

				MesenMsgBox.Show("UpgradeSuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void menuTimer_Tick(object sender, EventArgs e)
		{
			this.UpdateMenus();
		}

		void InitializeEmu()
		{
			InteropEmu.InitializeEmu(ConfigManager.HomeFolder, this.Handle, this.ctrlRenderer.Handle);
			foreach(string romPath in ConfigManager.Config.RecentFiles) {
				InteropEmu.AddKnowGameFolder(Path.GetDirectoryName(romPath).ToLowerInvariant());
			}

			ConfigManager.Config.ApplyConfig();
		
			UpdateEmulationFlags();
		}

		private void InitializeEmulationSpeedMenu()
		{
			mnuEmuSpeedNormal.Tag = 100;
			mnuEmuSpeedTriple.Tag = 300;
			mnuEmuSpeedDouble.Tag = 200;
			mnuEmuSpeedHalf.Tag = 50;
			mnuEmuSpeedQuarter.Tag = 25;
			mnuEmuSpeedMaximumSpeed.Tag = 0;

			UpdateEmulationSpeedMenu();
		}

		private void UpdateEmulationSpeedMenu()
		{
			foreach(ToolStripMenuItem item in new ToolStripMenuItem[] { mnuEmuSpeedDouble, mnuEmuSpeedHalf, mnuEmuSpeedNormal, mnuEmuSpeedQuarter, mnuEmuSpeedTriple, mnuEmuSpeedMaximumSpeed }) {
				item.Checked = ((int)item.Tag == ConfigManager.Config.VideoInfo.EmulationSpeed);
			}
		}

		private void SetEmulationSpeed(uint emulationSpeed)
		{
			if(emulationSpeed == 0) {
				InteropEmu.DisplayMessage("EmulationSpeed", "EmulationMaximumSpeed");
			} else {
				InteropEmu.DisplayMessage("EmulationSpeed", "EmulationSpeedPercent", emulationSpeed.ToString());
			}
			ConfigManager.Config.VideoInfo.EmulationSpeed = emulationSpeed;
			ConfigManager.ApplyChanges();
			UpdateEmulationSpeedMenu();
			VideoInfo.ApplyConfig();
		}

		private void mnuIncreaseSpeed_Click(object sender, EventArgs e)
		{
			if(ConfigManager.Config.VideoInfo.EmulationSpeed > 0) {
				if(ConfigManager.Config.VideoInfo.EmulationSpeed < 100) {
					SetEmulationSpeed(ConfigManager.Config.VideoInfo.EmulationSpeed + 25);
				} else if(ConfigManager.Config.VideoInfo.EmulationSpeed < 450) {
					SetEmulationSpeed(ConfigManager.Config.VideoInfo.EmulationSpeed + 50);
				} else {
					SetEmulationSpeed(0);
				}
			}
		}

		private void mnuDecreaseSpeed_Click(object sender, EventArgs e)
		{
			if(ConfigManager.Config.VideoInfo.EmulationSpeed == 0) {
				SetEmulationSpeed(450);
			} else if(ConfigManager.Config.VideoInfo.EmulationSpeed <= 100) {
				if(ConfigManager.Config.VideoInfo.EmulationSpeed > 25) {
					SetEmulationSpeed(ConfigManager.Config.VideoInfo.EmulationSpeed - 25);
				}
			} else {
				SetEmulationSpeed(ConfigManager.Config.VideoInfo.EmulationSpeed - 50);
			}
		}

		private void mnuEmuSpeedMaximumSpeed_Click(object sender, EventArgs e)
		{
			if(ConfigManager.Config.VideoInfo.EmulationSpeed == 0) {
				SetEmulationSpeed(100);
			} else {
				SetEmulationSpeed(0);
			}
		}

		private void mnuEmulationSpeedOption_Click(object sender, EventArgs e)
		{
			SetEmulationSpeed((uint)(int)((ToolStripItem)sender).Tag);
		}

		private void UpdateEmulationFlags()
		{
			ConfigManager.Config.VideoInfo.ShowFPS = mnuShowFPS.Checked;
			ConfigManager.ApplyChanges();

			VideoInfo.ApplyConfig();
		}

		private void UpdateVideoSettings()
		{
			mnuShowFPS.Checked = ConfigManager.Config.VideoInfo.ShowFPS;
			UpdateEmulationSpeedMenu();
			UpdateScaleMenu(ConfigManager.Config.VideoInfo.VideoScale);
			UpdateFilterMenu(ConfigManager.Config.VideoInfo.VideoFilter);

			UpdateViewerSize();
		}

		private void UpdateViewerSize()
		{
			InteropEmu.ScreenSize size = InteropEmu.GetScreenSize(false);

			if(!_customSize && this.WindowState != FormWindowState.Maximized) {
				this.Resize -= frmMain_Resize;
				this.ClientSize = new Size(size.Width, size.Height + menuStrip.Height);
				this.Resize += frmMain_Resize;
			}

			ctrlRenderer.Size = new Size(size.Width, size.Height);
			ctrlRenderer.Left = (panelRenderer.Width - ctrlRenderer.Width) / 2;
			ctrlRenderer.Top = (panelRenderer.Height - ctrlRenderer.Height) / 2;
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			if(this.WindowState != FormWindowState.Minimized) {
				SetScaleBasedOnWindowSize();
			}
		}

		private void SetScaleBasedOnWindowSize()
		{
			_customSize = true;
			InteropEmu.ScreenSize size = InteropEmu.GetScreenSize(true);
			double verticalScale = (double)panelRenderer.ClientSize.Height / size.Height;
			double horizontalScale = (double)panelRenderer.ClientSize.Width / size.Width;
			double scale = Math.Min(verticalScale, horizontalScale);
			UpdateScaleMenu(scale);
			VideoInfo.ApplyConfig();
		}

		private void SetFullscreenState(bool enabled)
		{
			this.Resize -= frmMain_Resize;
			if(enabled) {
				this.menuStrip.Visible = false;
				this.WindowState = FormWindowState.Normal;
				this.FormBorderStyle = FormBorderStyle.None;
				this.WindowState = FormWindowState.Maximized;
				SetScaleBasedOnWindowSize();
			} else {
				this.menuStrip.Visible = true;
				this.WindowState = FormWindowState.Normal;
				this.FormBorderStyle = FormBorderStyle.Sizable;
				this.UpdateScaleMenu(_regularScale);
				VideoInfo.ApplyConfig();
			}
			this.Resize += frmMain_Resize;

			_fullscreenMode = enabled;
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.GameLoaded:
					_currentGame = Path.GetFileNameWithoutExtension(InteropEmu.GetROMPath());
					InitializeFdsDiskMenu();
					InitializeVsSystemMenu();
					CheatInfo.ApplyCheats();
					VsConfigInfo.ApplyConfig();
					InitializeStateMenu(mnuSaveState, true);
					InitializeStateMenu(mnuLoadState, false);
					this.StartEmuThread();
					break;

				case InteropEmu.ConsoleNotificationType.DisconnectedFromServer:
					ConfigManager.Config.ApplyConfig();
					break;

				case InteropEmu.ConsoleNotificationType.GameStopped:
					CheatInfo.ClearCheats();
					break;

				case InteropEmu.ConsoleNotificationType.ResolutionChanged:
					this.BeginInvoke((MethodInvoker)(() => {
						UpdateVideoSettings();
					}));
					break;

				case InteropEmu.ConsoleNotificationType.FdsBiosNotFound:
					this.BeginInvoke((MethodInvoker)(() => {
						SelectFdsBiosPrompt();
					}));
					break;
			}
			UpdateMenus();
		}

		private void mnuOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = ResourceHelper.GetMessage("FilterRomIps");
			if(ConfigManager.Config.RecentFiles.Count > 0) {
				ofd.InitialDirectory = Path.GetDirectoryName(ConfigManager.Config.RecentFiles[0]);
			}			
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				LoadFile(ofd.FileName);
			}
		}

		private void LoadFile(string filename)
		{
			if(File.Exists(filename)) {
				if(Path.GetExtension(filename).ToLowerInvariant() == ".ips") {
					LoadIpsFile(filename);
				} else if(Path.GetExtension(filename).ToLowerInvariant() == ".mmo") {
					InteropEmu.MoviePlay(filename);
				} else {
					LoadROM(filename, ConfigManager.Config.PreferenceInfo.AutoLoadIpsPatches);
				}
			}
		}

		private void LoadIpsFile(string filename)
		{
			string ipsFile = filename;
			string romFile = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));

			if(File.Exists(romFile+".nes") || File.Exists(romFile+".zip") || File.Exists(romFile+".fds")) {
				string ext = string.Empty;
				if(File.Exists(romFile+".nes"))
					ext = ".nes";
				if(File.Exists(romFile+".zip"))
					ext = ".zip";
				if(File.Exists(romFile+".fds"))
					ext = ".fds";
				LoadROM(romFile + ext);
				InteropEmu.ApplyIpsPatch(ipsFile);
			} else {
				if(_emuThread == null) {
					if(MesenMsgBox.Show("SelectRomIps", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK) {
						OpenFileDialog ofd = new OpenFileDialog();
						ofd.Filter = ResourceHelper.GetMessage("FilterRom");
						if(ConfigManager.Config.RecentFiles.Count > 0) {
							ofd.InitialDirectory = Path.GetDirectoryName(ConfigManager.Config.RecentFiles[0]);
						}
						if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
							LoadROM(ofd.FileName);
						}
						InteropEmu.ApplyIpsPatch(ipsFile);
					}
				} else if(MesenMsgBox.Show("PatchAndReset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.ApplyIpsPatch(ipsFile);
				}
			}
		}

		private void LoadROM(string filename, bool autoLoadIps = false)
		{
			_romToLoad = filename;
			if(File.Exists(filename)) {
				ConfigManager.Config.AddRecentFile(filename);
				InteropEmu.LoadROM(filename);
				UpdateRecentFiles();

				string ipsFile = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename)) + ".ips";
				if(File.Exists(ipsFile)) {
					InteropEmu.ApplyIpsPatch(ipsFile);
				}
			} else {
				MesenMsgBox.Show("FileNotFound", MessageBoxButtons.OK, MessageBoxIcon.Error, filename);
			}
		}

		private void UpdateFocusFlag()
		{
			bool hasFocus = false;
			if(Application.OpenForms.Count > 0) {
				if(Application.OpenForms[0].ContainsFocus) {
					hasFocus = true;
				}
			}

			foreach(ToolStripItem item in menuStrip.Items) {
				if(item.Pressed || item.Selected) {
					hasFocus = false;
					break;
				}
			}

			InteropEmu.SetFlag(EmulationFlags.InBackground, !hasFocus);
		}

		private void UpdateMenus()
		{
			try {
				if(this.InvokeRequired) {
					this.BeginInvoke((MethodInvoker)(() => this.UpdateMenus()));
				} else {
					UpdateFocusFlag();

					if(string.IsNullOrWhiteSpace(_currentGame)) {
						this.Text = "Mesen";
					} else {
						this.Text = "Mesen - " + _currentGame;
					}

					bool isNetPlayClient = InteropEmu.IsConnected();

					mnuSaveState.Enabled = mnuLoadState.Enabled = mnuPause.Enabled = mnuStop.Enabled = mnuReset.Enabled = (_emuThread != null && !isNetPlayClient);
					mnuPause.Text = InteropEmu.IsPaused() ? ResourceHelper.GetMessage("Resume") : ResourceHelper.GetMessage("Pause");
					mnuPause.Image = InteropEmu.IsPaused() ? Mesen.GUI.Properties.Resources.Play : Mesen.GUI.Properties.Resources.Pause;

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
					mnuIncreaseSpeed.Enabled = !isNetPlayClient;
					mnuDecreaseSpeed.Enabled = !isNetPlayClient;
					mnuEmuSpeedMaximumSpeed.Enabled = !isNetPlayClient;
					mnuInput.Enabled = !isNetPlayClient;
					mnuRegion.Enabled = !isNetPlayClient;

					bool moviePlaying = InteropEmu.MoviePlaying();
					bool movieRecording = InteropEmu.MovieRecording();
					mnuPlayMovie.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuStopMovie.Enabled = _emuThread != null && !netPlay && (moviePlaying || movieRecording);
					mnuRecordFrom.Enabled = _emuThread != null && !moviePlaying && !movieRecording;
					mnuRecordFromStart.Enabled = _emuThread != null && !isNetPlayClient && !moviePlaying && !movieRecording;
					mnuRecordFromNow.Enabled = _emuThread != null && !moviePlaying && !movieRecording;

					bool testRecording = InteropEmu.RomTestRecording();
					mnuTestRun.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuTestStopRecording.Enabled = _emuThread != null && testRecording;
					mnuTestRecordStart.Enabled = _emuThread != null && !isNetPlayClient && !moviePlaying && !movieRecording;
					mnuTestRecordNow.Enabled = _emuThread != null && !moviePlaying && !movieRecording;
					mnuTestRecordMovie.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuTestRecordTest.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuTestRecordFrom.Enabled = (mnuTestRecordStart.Enabled || mnuTestRecordNow.Enabled || mnuTestRecordMovie.Enabled || mnuTestRecordTest.Enabled);

					mnuDebugger.Enabled = !netPlay && _emuThread != null;

					mnuTakeScreenshot.Enabled = _emuThread != null;

					mnuRegionAuto.Checked = ConfigManager.Config.Region == NesModel.Auto;
					mnuRegionNtsc.Checked = ConfigManager.Config.Region == NesModel.NTSC;
					mnuRegionPal.Checked = ConfigManager.Config.Region == NesModel.PAL;
					mnuRegionDendy.Checked = ConfigManager.Config.Region == NesModel.Dendy;
				}
			} catch { }
		}

		private void UpdateRecentFiles()
		{
			mnuRecentFiles.DropDownItems.Clear();
			foreach(string filepath in ConfigManager.Config.RecentFiles) {
				ToolStripMenuItem tsmi = new ToolStripMenuItem();
				tsmi.Text = Path.GetFileName(filepath);
				tsmi.Click += (object sender, EventArgs args) => {
					LoadROM(filepath);
				};
				mnuRecentFiles.DropDownItems.Add(tsmi);
			}

			mnuRecentFiles.Enabled = mnuRecentFiles.DropDownItems.Count > 0;
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
			if(InteropEmu.IsPaused()) {
				InteropEmu.Resume();
			} else {
				InteropEmu.Pause();
			}
		}

		private void ResetEmu()
		{
			InteropEmu.Reset();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(!this.menuStrip.Enabled) {
				//Make sure we disable all shortcut keys while the bar is disabled (i.e when running tests)
				return false;
			}

			if(_fullscreenMode && (keyData & Keys.Alt) == Keys.Alt) {
				if(this.menuStrip.Visible && !this.menuStrip.ContainsFocus) {
					this.menuStrip.Visible = false;
				} else {
					this.menuStrip.Visible = true;
					this.menuStrip.Focus();
				}
			}

			if(keyData == Keys.Escape && _emuThread != null && mnuPause.Enabled) {
				PauseEmu();
				return true;
			} else if(keyData == Keys.Oemplus) {
				mnuIncreaseSpeed.PerformClick();
				return true;
			} else if(keyData == Keys.OemMinus) {
				mnuDecreaseSpeed.PerformClick();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		const int NumberOfSaveSlots = 5;
		private void InitializeStateMenu(ToolStripMenuItem menu, bool forSave)
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)(() => this.InitializeStateMenu(menu, forSave)));
			} else {
				menu.DropDownItems.Clear();
				for(uint i = 1; i <= frmMain.NumberOfSaveSlots; i++) {
					Int64 fileTime = InteropEmu.GetStateInfo(i);
					string label;
					if(fileTime == 0) {
						label = i.ToString() + ". " + ResourceHelper.GetMessage("EmptyState");
					} else {
						DateTime dateTime = DateTime.FromFileTime(fileTime);
						label = i.ToString() + ". " + dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
					}

					ToolStripMenuItem item = (ToolStripMenuItem)menu.DropDownItems.Add(label);
					uint stateIndex = i;
					item.Click += (object sender, EventArgs e) => {
						if(_emuThread != null) {
							if(forSave) {
								InteropEmu.SaveState(stateIndex);
							} else {
								InteropEmu.LoadState(stateIndex);
							}
						}
					};
					item.ShortcutKeys = (Keys)((int)Keys.F1 + i - 1);
					if(forSave) {
						item.ShortcutKeys |= Keys.Shift;
					}
				}
			}
		}

		#region Events

		private void mnuPause_Click(object sender, EventArgs e)
		{
			PauseEmu();
		}

		private void mnuReset_Click(object sender, EventArgs e)
		{
			ResetEmu();
		}

		private void mnuStop_Click(object sender, EventArgs e)
		{
			InteropEmu.Stop();
		}

		private void mnuShowFPS_Click(object sender, EventArgs e)
		{
			UpdateEmulationFlags();
		}

		private void mnuStartServer_Click(object sender, EventArgs e)
		{
			if(InteropEmu.IsServerRunning()) {
				Task.Run(() => InteropEmu.StopServer());
			} else {
				frmServerConfig frm = new frmServerConfig();
				if(frm.ShowDialog(sender) == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.StartServer(ConfigManager.Config.ServerInfo.Port, ConfigManager.Config.Profile.PlayerName);
				}
			}
		}

		private void mnuConnect_Click(object sender, EventArgs e)
		{
			if(InteropEmu.IsConnected()) {
				Task.Run(() => InteropEmu.Disconnect());
			} else {
				frmClientConfig frm = new frmClientConfig();
				if(frm.ShowDialog(sender) == System.Windows.Forms.DialogResult.OK) {
					Task.Run(() => {
						InteropEmu.Connect(ConfigManager.Config.ClientConnectionInfo.Host, ConfigManager.Config.ClientConnectionInfo.Port, ConfigManager.Config.Profile.PlayerName, null, 0, ConfigManager.Config.ClientConnectionInfo.Spectator);
					});
				}
			}
		}

		private void mnuProfile_Click(object sender, EventArgs e)
		{
			new frmPlayerProfile().ShowDialog(sender);
		}
		
		private void mnuExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
		private void mnuVideoConfig_Click(object sender, EventArgs e)
		{
			new frmVideoConfig().ShowDialog(sender);
			UpdateVideoSettings();
		}
		
		private void mnuDebugger_Click(object sender, EventArgs e)
		{
			if(_debugger == null) {
				_debugger = new frmDebugger();
				_debugger.FormClosed += (obj, args) => {
					_debugger = null;
				};
				_debugger.Show(sender);
			} else {
				_debugger.Focus();
			}
		}
		
		private void mnuSaveState_DropDownOpening(object sender, EventArgs e)
		{
			InitializeStateMenu(mnuSaveState, true);
		}

		private void mnuLoadState_DropDownOpening(object sender, EventArgs e)
		{
			InitializeStateMenu(mnuLoadState, false);
		}

		private void mnuTakeScreenshot_Click(object sender, EventArgs e)
		{
			InteropEmu.TakeScreenshot();
		}

		#endregion
		
		private void RecordMovie(bool resetEmu)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = ResourceHelper.GetMessage("FilterMovie");
			sfd.InitialDirectory = ConfigManager.MovieFolder;
			sfd.FileName = Path.GetFileNameWithoutExtension(InteropEmu.GetROMPath()) + ".mmo";
			if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				InteropEmu.MovieRecord(sfd.FileName, resetEmu);
			}
		}

		private void mnuPlayMovie_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = ResourceHelper.GetMessage("FilterMovie");
			ofd.InitialDirectory = ConfigManager.MovieFolder;
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				InteropEmu.MoviePlay(ofd.FileName);
			}
		}

		private void mnuStopMovie_Click(object sender, EventArgs e)
		{
			InteropEmu.MovieStop();
		}

		private void mnuRecordFromStart_Click(object sender, EventArgs e)
		{
			RecordMovie(true);
		}

		private void mnuRecordFromNow_Click(object sender, EventArgs e)
		{
			RecordMovie(false);
		}

		private void mnuTestRun_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = ResourceHelper.GetMessage("FilterTest");
			ofd.InitialDirectory = ConfigManager.TestFolder;
			ofd.Multiselect = true;
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				List<string> passedTests = new List<string>();
				List<string> failedTests = new List<string>();
				List<int> failedFrameCount = new List<int>();

				this.menuStrip.Enabled = false;

				Task.Run(() => {
					foreach(string filename in ofd.FileNames) {
						int result = InteropEmu.RomTestRun(filename);

						if(result == 0) {
							passedTests.Add(Path.GetFileNameWithoutExtension(filename));
						} else {
							failedTests.Add(Path.GetFileNameWithoutExtension(filename));
							failedFrameCount.Add(result);
						}
					}

					this.BeginInvoke((MethodInvoker)(() => {
						if(failedTests.Count == 0) {
							MessageBox.Show("All tests passed.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
						} else {
							StringBuilder message = new StringBuilder();
							if(passedTests.Count > 0) {
								message.AppendLine("Passed tests:");
								foreach(string test in passedTests) {
									message.AppendLine("  -" + test);
								}
								message.AppendLine("");
							}
							message.AppendLine("Failed tests:");
							for(int i = 0, len = failedTests.Count; i < len; i++) {
								message.AppendLine("  -" + failedTests[i] + " (" + failedFrameCount[i] + ")");
							}
							MessageBox.Show(message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}

						this.menuStrip.Enabled = true;
					}));
				});
			}
		}

		private void mnuTestRecordStart_Click(object sender, EventArgs e)
		{
			RecordTest(true);
		}

		private void mnuTestRecordNow_Click(object sender, EventArgs e)
		{
			RecordTest(false);
		}

		private void RecordTest(bool resetEmu)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = ResourceHelper.GetMessage("FilterTest");
			sfd.InitialDirectory = ConfigManager.TestFolder;
			sfd.FileName = Path.GetFileNameWithoutExtension(InteropEmu.GetROMPath()) + ".mtp";
			if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				InteropEmu.RomTestRecord(sfd.FileName, resetEmu);
			}
		}

		private void mnuTestRecordMovie_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = ResourceHelper.GetMessage("FilterMovie");
			ofd.InitialDirectory = ConfigManager.MovieFolder;
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Filter = ResourceHelper.GetMessage("FilterTest");
				sfd.InitialDirectory = ConfigManager.TestFolder;
				sfd.FileName = Path.GetFileNameWithoutExtension(ofd.FileName) + ".mtp";
				if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.RomTestRecordFromMovie(sfd.FileName, ofd.FileName);
				}
			}
		}

		private void mnuTestRecordTest_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = ResourceHelper.GetMessage("FilterTest");
			ofd.InitialDirectory = ConfigManager.TestFolder;

			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Filter = ResourceHelper.GetMessage("FilterTest");
				sfd.InitialDirectory = ConfigManager.TestFolder;
				sfd.FileName = Path.GetFileNameWithoutExtension(ofd.FileName) + ".mtp";
				if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.RomTestRecordFromTest(sfd.FileName, ofd.FileName);
				}
			}
		}

		private void mnuTestStopRecording_Click(object sender, EventArgs e)
		{
			InteropEmu.RomTestStop();
		}

		private void mnuCheats_Click(object sender, EventArgs e)
		{
			frmCheatList frm = new frmCheatList();
			frm.Show(sender, this);
			frm.FormClosed += (object a, FormClosedEventArgs b) => {
				frm = null;
				CheatInfo.ApplyCheats();
			};
		}

		private void mnuInput_Click(object sender, EventArgs e)
		{
			new frmInputConfig().ShowDialog(sender);
		}

		private void mnuAudioConfig_Click(object sender, EventArgs e)
		{
			new frmAudioConfig().ShowDialog(sender);
		}

		private void mnuPreferences_Click(object sender, EventArgs e)
		{
			new frmPreferences().ShowDialog(sender);
			ResourceHelper.LoadResources(ConfigManager.Config.PreferenceInfo.DisplayLanguage);
			ResourceHelper.UpdateEmuLanguage();
			ResourceHelper.ApplyResources(this);
			UpdateMenus();
			InitializeFdsDiskMenu();
		}

		private void mnuRegion_Click(object sender, EventArgs e)
		{
			if(sender == mnuRegionAuto) {
				ConfigManager.Config.Region = NesModel.Auto;
			} else if(sender == mnuRegionNtsc) {
				ConfigManager.Config.Region = NesModel.NTSC;
			} else if(sender == mnuRegionPal) {
				ConfigManager.Config.Region = NesModel.PAL;
			} else if(sender == mnuRegionDendy) {
				ConfigManager.Config.Region = NesModel.Dendy;
			}
			ConfigManager.Config.ApplyConfig();
		}

		private void mnuRunAllTests_Click(object sender, EventArgs e)
		{
			string workingDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.FileName = "TestHelper.exe";
			startInfo.WorkingDirectory = workingDirectory;
			Process.Start(startInfo);
		}

		private void UpdateScaleMenu(double scale)
		{
			mnuScale1x.Checked = (scale == 1.0) && !_customSize;
			mnuScale2x.Checked = (scale == 2.0) && !_customSize;
			mnuScale3x.Checked = (scale == 3.0) && !_customSize;
			mnuScale4x.Checked = (scale == 4.0) && !_customSize;
			mnuScaleCustom.Checked = _customSize || !mnuScale1x.Checked && !mnuScale2x.Checked && !mnuScale3x.Checked && !mnuScale4x.Checked;

			ConfigManager.Config.VideoInfo.VideoScale = scale;
			ConfigManager.ApplyChanges();
		}

		private void UpdateFilterMenu(VideoFilterType filterType)
		{
			mnuNoneFilter.Checked = (filterType == VideoFilterType.None);
			mnuNtscFilter.Checked = (filterType == VideoFilterType.NTSC);

			ConfigManager.Config.VideoInfo.VideoFilter = filterType;
			ConfigManager.ApplyChanges();
		}

		private void mnuScale_Click(object sender, EventArgs e)
		{
			UInt32 scale = UInt32.Parse((string)((ToolStripMenuItem)sender).Tag);
			_customSize = false;
			_regularScale = scale;
			InteropEmu.SetVideoScale(scale);
			UpdateScaleMenu(scale);
		}

		private void mnuNoneFilter_Click(object sender, EventArgs e)
		{
			InteropEmu.SetVideoFilter(VideoFilterType.None);
			UpdateFilterMenu(VideoFilterType.None);
			_customSize = false;
		}

		private void mnuNtscFilter_Click(object sender, EventArgs e)
		{
			InteropEmu.SetVideoFilter(VideoFilterType.NTSC);
			UpdateFilterMenu(VideoFilterType.NTSC);
			_customSize = false;
		}

		private void InitializeFdsDiskMenu()
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)(() => this.InitializeFdsDiskMenu()));
			} else {
				UInt32 sideCount = InteropEmu.FdsGetSideCount();

				mnuSelectDisk.DropDownItems.Clear();

				if(sideCount > 0) {
					for(UInt32 i = 0; i < sideCount; i++) {
						UInt32 diskNumber = i;
						ToolStripItem item = mnuSelectDisk.DropDownItems.Add(ResourceHelper.GetMessage("FdsDiskSide", (diskNumber/2+1).ToString(), (diskNumber % 2 == 0 ? "A" : "B")));
						item.Click += (object sender, EventArgs args) => {
							InteropEmu.FdsInsertDisk(diskNumber);
						};
					}
					sepFdsDisk.Visible = true;
					mnuSelectDisk.Visible = true;
					mnuEjectDisk.Visible = true;
					mnuSwitchDiskSide.Visible = sideCount > 1;
				} else {
					sepFdsDisk.Visible = false;
					mnuSelectDisk.Visible = false;
					mnuEjectDisk.Visible = false;
					mnuSwitchDiskSide.Visible = false;
				}
			}
		}

		private void mnuEjectDisk_Click(object sender, EventArgs e)
		{
			InteropEmu.FdsEjectDisk();
		}

		private void mnuSwitchDiskSide_Click(object sender, EventArgs e)
		{
			InteropEmu.FdsSwitchDiskSide();
		}

		private void SelectFdsBiosPrompt()
		{
			if(MesenMsgBox.Show("FdsBiosNotFound", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.Filter = ResourceHelper.GetMessage("FilterAll");
				if(ofd.ShowDialog() == DialogResult.OK) {
					if(MD5Helper.GetMD5Hash(ofd.FileName).ToLowerInvariant() == "ca30b50f880eb660a320674ed365ef7a") {
						File.Copy(ofd.FileName, Path.Combine(ConfigManager.HomeFolder, "FdsBios.bin"));
						LoadROM(_romToLoad);
					} else {
						MesenMsgBox.Show("InvalidFdsBios", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void frmMain_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if(File.Exists(files[0])) {
				LoadFile(files[0]);
				AutomationElement element = AutomationElement.FromHandle(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
				if(element != null) {
					element.SetFocus();
				}
			}
		}

		private void frmMain_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void mnuNetPlayPlayer1_Click(object sender, EventArgs e)
		{
			InteropEmu.NetPlaySelectController(0);
		}

		private void mnuNetPlayPlayer2_Click(object sender, EventArgs e)
		{
			InteropEmu.NetPlaySelectController(1);
		}

		private void mnuNetPlayPlayer3_Click(object sender, EventArgs e)
		{
			InteropEmu.NetPlaySelectController(2);
		}

		private void mnuNetPlayPlayer4_Click(object sender, EventArgs e)
		{
			InteropEmu.NetPlaySelectController(3);
		}

		private void mnuNetPlaySpectator_Click(object sender, EventArgs e)
		{
			InteropEmu.NetPlaySelectController(0xFF);
		}

		private void mnuFullscreen_Click(object sender, EventArgs e)
		{
			SetFullscreenState(!_fullscreenMode);
			mnuFullscreen.Checked = _fullscreenMode;
		}

		private void mnuScaleCustom_Click(object sender, EventArgs e)
		{
			SetScaleBasedOnWindowSize();
		}

		private void panelRenderer_Click(object sender, EventArgs e)
		{
			ctrlRenderer.Focus();
		}

		private void ctrlRenderer_Enter(object sender, EventArgs e)
		{
			if(_fullscreenMode) {
				this.menuStrip.Visible = false;
			}
		}

		private void menuStrip_VisibleChanged(object sender, EventArgs e)
		{
			IntPtr handle = this.Handle;
			this.BeginInvoke((MethodInvoker)(() => {
				if(_fullscreenMode && _customSize) {
					SetScaleBasedOnWindowSize();
				}
			}));
		}

		private void mnuAbout_Click(object sender, EventArgs e)
		{
			new frmAbout().ShowDialog();
		}

		private void CheckForUpdates(bool displayResult)
		{
			Task.Run(() => {
				try {
					using(var client = new WebClient()) {
						XmlDocument xmlDoc = new XmlDocument();
						string langCode = "";
						switch(ResourceHelper.GetCurrentLanguage()) {
							case Language.English: langCode = "en"; break;
							case Language.French: langCode = "fr"; break;
							case Language.Japanese: langCode = "ja"; break;
						}
						xmlDoc.LoadXml(client.DownloadString("http://www.mesen.ca/Services/GetLatestVersion.php?v=" + InteropEmu.GetMesenVersion() + "&p=win&l=" + langCode));
						Version currentVersion = new Version(InteropEmu.GetMesenVersion());
						Version latestVersion = new Version(xmlDoc.SelectSingleNode("VersionInfo/LatestVersion").InnerText);
						string changeLog = xmlDoc.SelectSingleNode("VersionInfo/ChangeLog").InnerText;
						string fileHash = xmlDoc.SelectSingleNode("VersionInfo/Sha1Hash").InnerText;

						if(latestVersion > currentVersion) {
							this.BeginInvoke((MethodInvoker)(() => {
								frmUpdatePrompt frmUpdate = new frmUpdatePrompt(currentVersion, latestVersion, changeLog, fileHash);
								if(frmUpdate.ShowDialog(null, this) == DialogResult.OK) {
									Application.Exit();
								}
							}));
						} else if(displayResult) {
							MesenMsgBox.Show("MesenUpToDate", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
				} catch(Exception ex) {
					if(displayResult) {
						MesenMsgBox.Show("ErrorWhileCheckingUpdates", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
					}
				}
			});
		}

		private void mnuCheckForUpdates_Click(object sender, EventArgs e)
		{
			CheckForUpdates(true);
		}

		private void InitializeVsSystemMenu()
		{
			sepVsSystem.Visible = InteropEmu.IsVsSystem();
			mnuInsertCoin1.Visible = InteropEmu.IsVsSystem();
			mnuInsertCoin2.Visible = InteropEmu.IsVsSystem();
			mnuVsGameConfig.Visible = InteropEmu.IsVsSystem();
		}

		private void mnuInsertCoin1_Click(object sender, EventArgs e)
		{
			InteropEmu.VsInsertCoin(0);
		}

		private void mnuInsertCoin2_Click(object sender, EventArgs e)
		{
			InteropEmu.VsInsertCoin(1);
		}

		private void mnuVsGameConfig_Click(object sender, EventArgs e)
		{
			VsConfigInfo configInfo = VsConfigInfo.GetCurrentGameConfig(true);
			if(new frmVsGameConfig(configInfo).ShowDialog(sender, this) == DialogResult.OK) {
				VsConfigInfo.ApplyConfig();
			}
		}
	}
}
