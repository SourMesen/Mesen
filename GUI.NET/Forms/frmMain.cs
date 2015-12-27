using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Debugger;
using Mesen.GUI.Forms.Cheats;
using Mesen.GUI.Forms.Config;
using Mesen.GUI.Forms.NetPlay;

namespace Mesen.GUI.Forms
{
	public partial class frmMain : Form
	{
		private InteropEmu.NotificationListener _notifListener;
		private Thread _emuThread;
		private frmDebugger _debugger;
		
		public frmMain()
		{
			Application.ThreadException += Application_ThreadException;
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			menuTimer.Start();

			InitializeEmulationSpeedMenu();
			
			UpdateVideoSettings();
			InitializeEmu();

			UpdateMenus();
			UpdateRecentFiles();

			if(!DesignMode) {
				Icon = Properties.Resources.MesenIcon;
			}
		}
		
		private void menuTimer_Tick(object sender, EventArgs e)
		{
			this.UpdateMenus();
		}

		void InitializeEmu()
		{
			InteropEmu.InitializeEmu(ConfigManager.HomeFolder, this.Handle, this.dxViewer.Handle);
			foreach(string romPath in ConfigManager.Config.RecentFiles) {
				InteropEmu.AddKnowGameFolder(System.IO.Path.GetDirectoryName(romPath).ToLowerInvariant());
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
				InteropEmu.DisplayMessage("Emulation Speed", "Maximum speed");
			} else {
				InteropEmu.DisplayMessage("Emulation Speed", emulationSpeed + "%");
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
		
		void UpdateEmulationFlags()
		{
			ConfigManager.Config.VideoInfo.ShowFPS = mnuShowFPS.Checked;
			ConfigManager.ApplyChanges();

			VideoInfo.ApplyConfig();
		}

		void UpdateVideoSettings()
		{
			mnuShowFPS.Checked = ConfigManager.Config.VideoInfo.ShowFPS;
			UpdateEmulationSpeedMenu();
			dxViewer.Size = VideoInfo.GetViewerSize();
		}

		void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.GameLoaded) {
				CheatInfo.ApplyCheats();
				InitializeStateMenu(mnuSaveState, true);
				InitializeStateMenu(mnuLoadState, false);
				this.StartEmuThread();
			} else if(e.NotificationType == InteropEmu.ConsoleNotificationType.GameStopped) {
				CheatInfo.ClearCheats();
			}
			UpdateMenus();
		}

		private void mnuOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "All supported formats (*.nes, *.zip)|*.NES;*.ZIP|NES Roms (*.nes)|*.NES|ZIP Archives (*.zip)|*.ZIP|All (*.*)|*.*";
			ofd.InitialDirectory = System.IO.Path.GetDirectoryName(ConfigManager.Config.RecentFiles[0]);
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				LoadROM(ofd.FileName);
			}
		}

		private void LoadROM(string filename)
		{
			ConfigManager.Config.AddRecentFile(filename);
			InteropEmu.LoadROM(filename);
			UpdateRecentFiles();
		}

		private void UpdateMenus()
		{
			try {
				if(this.InvokeRequired) {
					this.BeginInvoke((MethodInvoker)(() => this.UpdateMenus()));
				} else {
					string romFilename = System.IO.Path.GetFileNameWithoutExtension(InteropEmu.GetROMPath());
					if(string.IsNullOrWhiteSpace(romFilename)) {
						this.Text = "Mesen";
					} else {
						this.Text = "Mesen - " + romFilename;
					}

					mnuSaveState.Enabled = mnuLoadState.Enabled = mnuPause.Enabled = mnuStop.Enabled = mnuReset.Enabled = (_emuThread != null && !InteropEmu.IsConnected());
					mnuPause.Text = InteropEmu.IsPaused() ? "Resume" : "Pause";

					bool netPlay = InteropEmu.IsServerRunning() || InteropEmu.IsConnected();

					mnuStartServer.Enabled = !netPlay;
					mnuStopServer.Enabled = !mnuStartServer.Enabled && !InteropEmu.IsConnected();

					mnuConnect.Enabled = !netPlay;
					mnuDisconnect.Enabled = !mnuConnect.Enabled && !InteropEmu.IsServerRunning();

					mnuEmulationSpeed.Enabled = !InteropEmu.IsConnected();

					bool moviePlaying = InteropEmu.MoviePlaying();
					bool movieRecording = InteropEmu.MovieRecording();
					mnuPlayMovie.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuStopMovie.Enabled = _emuThread != null && !netPlay && (moviePlaying || movieRecording);
					mnuRecordFrom.Enabled = _emuThread != null && !moviePlaying && !movieRecording;
					mnuRecordFromStart.Enabled = _emuThread != null && !InteropEmu.IsConnected() && !moviePlaying && !movieRecording;
					mnuRecordFromNow.Enabled = _emuThread != null && !moviePlaying && !movieRecording;

					bool testRecording = InteropEmu.RomTestRecording();
					mnuTestRun.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuTestStopRecording.Enabled = _emuThread != null && testRecording;
					mnuTestRecordStart.Enabled = _emuThread != null && !InteropEmu.IsConnected() && !moviePlaying && !movieRecording;
					mnuTestRecordNow.Enabled = _emuThread != null && !moviePlaying && !movieRecording;
					mnuTestRecordMovie.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuTestRecordTest.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuTestRecordFrom.Enabled = (mnuTestRecordStart.Enabled || mnuTestRecordNow.Enabled || mnuTestRecordMovie.Enabled || mnuTestRecordTest.Enabled);

					mnuDebugger.Enabled = !netPlay && _emuThread != null;

					mnuTakeScreenshot.Enabled = _emuThread != null;

					mnuRegionAuto.Checked = ConfigManager.Config.Region == NesModel.Auto;
					mnuRegionNtsc.Checked = ConfigManager.Config.Region == NesModel.NTSC;
					mnuRegionPal.Checked = ConfigManager.Config.Region == NesModel.PAL;
				}
			} catch { }
		}

		private void UpdateRecentFiles()
		{
			mnuRecentFiles.DropDownItems.Clear();
			foreach(string filepath in ConfigManager.Config.RecentFiles) {
				ToolStripMenuItem tsmi = new ToolStripMenuItem();
				tsmi.Text = System.IO.Path.GetFileName(filepath);
				tsmi.Click += (object sender, EventArgs args) => {
					LoadROM(filepath);
				};
				mnuRecentFiles.DropDownItems.Add(tsmi);
			}
		}

		private void StartEmuThread()
		{
			if(_emuThread == null) {
				_emuThread = new Thread(() => {
					try {
						InteropEmu.Run();
						_emuThread = null;
					} catch(Exception ex) {
						MessageBox.Show(ex.Message);
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
						label = i.ToString() + ". <empty>";
					} else {
						DateTime dateTime = DateTime.FromFileTime(fileTime);
						label = i.ToString() + ". " + dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
					}

					ToolStripMenuItem item = (ToolStripMenuItem)menu.DropDownItems.Add(label);
					uint stateIndex = i;
					item.Click += (object sender, EventArgs e) => {
						if(forSave) {
							InteropEmu.SaveState(stateIndex);
						} else {
							InteropEmu.LoadState(stateIndex);
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
			frmServerConfig frm = new frmServerConfig();
			if(frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				InteropEmu.StartServer(ConfigManager.Config.ServerInfo.Port);
			}
		}

		private void mnuConnect_Click(object sender, EventArgs e)
		{
			frmClientConfig frm = new frmClientConfig();
			if(frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				InteropEmu.Connect(ConfigManager.Config.ClientConnectionInfo.Host, ConfigManager.Config.ClientConnectionInfo.Port, ConfigManager.Config.Profile.PlayerName, ConfigManager.Config.Profile.PlayerAvatar, (UInt16)ConfigManager.Config.Profile.PlayerAvatar.Length);
			}
		}

		private void mnuProfile_Click(object sender, EventArgs e)
		{
			new frmPlayerProfile().ShowDialog();
		}

		private void mnuStopServer_Click(object sender, EventArgs e)
		{
			Task.Run(() => InteropEmu.StopServer());
		}

		private void mnuDisconnect_Click(object sender, EventArgs e)
		{
			Task.Run(() => InteropEmu.Disconnect());
		}
		
		private void mnuExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
		private void mnuVideoConfig_Click(object sender, EventArgs e)
		{
			new frmVideoConfig().ShowDialog();
			UpdateVideoSettings();
		}
		
		private void mnuDebugger_Click(object sender, EventArgs e)
		{
			if(_debugger == null) {
				_debugger = new frmDebugger();
				_debugger.FormClosed += (obj, args) => {
					_debugger = null;
				};
				_debugger.Show();
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
			sfd.Filter = "Movie files (*.mmo)|*.mmo|All (*.*)|*.*";
			sfd.InitialDirectory = ConfigManager.MovieFolder;
			sfd.FileName = System.IO.Path.GetFileNameWithoutExtension(InteropEmu.GetROMPath()) + ".mmo";
			if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				InteropEmu.MovieRecord(sfd.FileName, resetEmu);
			}
		}

		private void mnuPlayMovie_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Movie files (*.mmo)|*.mmo|All (*.*)|*.*";
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
			ofd.Filter = "Test files (*.mtp)|*.mtp|All (*.*)|*.*";
			ofd.InitialDirectory = ConfigManager.TestFolder;
			ofd.Multiselect = true;
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				List<string> passedTests = new List<string>();
				List<string> failedTests = new List<string>();

				this.menuStrip.Enabled = false;

				Task.Run(() => {
					foreach(string filename in ofd.FileNames) {
						bool result = InteropEmu.RomTestRun(filename);

						if(result) {
							passedTests.Add(System.IO.Path.GetFileNameWithoutExtension(filename));
						} else {
							failedTests.Add(System.IO.Path.GetFileNameWithoutExtension(filename));
						}
					}

					this.BeginInvoke((MethodInvoker)(() => {
						if(failedTests.Count == 0) {
							MessageBox.Show("All tests passed.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
						} else if(passedTests.Count == 0) {
							MessageBox.Show("All tests failed.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
						} else {
							StringBuilder message = new StringBuilder();
							message.AppendLine("Passed tests:");
							foreach(string test in passedTests) {
								message.AppendLine("  -" + test);
							}
							message.AppendLine("");
							message.AppendLine("Failed tests:");
							foreach(string test in failedTests) {
								message.AppendLine("  -" + test);
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
			sfd.Filter = "Test files (*.mtp)|*.mtp|All (*.*)|*.*";
			sfd.InitialDirectory = ConfigManager.TestFolder;
			sfd.FileName = System.IO.Path.GetFileNameWithoutExtension(InteropEmu.GetROMPath()) + ".mtp";
			if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				InteropEmu.RomTestRecord(sfd.FileName, resetEmu);
			}
		}

		private void mnuTestRecordMovie_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Movie files (*.mmo)|*.mmo|All (*.*)|*.*";
			ofd.InitialDirectory = ConfigManager.MovieFolder;
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Filter = "Test files (*.mtp)|*.mtp|All (*.*)|*.*";
				sfd.InitialDirectory = ConfigManager.TestFolder;
				sfd.FileName = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName) + ".mtp";
				if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.RomTestRecordFromMovie(sfd.FileName, ofd.FileName);
				}
			}
		}

		private void mnuTestRecordTest_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Test files (*.mtp)|*.mtp|All (*.*)|*.*";
			ofd.InitialDirectory = ConfigManager.TestFolder;

			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Filter = "Test files (*.mtp)|*.mtp|All (*.*)|*.*";
				sfd.InitialDirectory = ConfigManager.TestFolder;
				sfd.FileName = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName) + ".mtp";
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
			frm.Show(this);
			frm.FormClosed += (object a, FormClosedEventArgs b) => {
				frm = null;
				CheatInfo.ApplyCheats();
			};
		}

		private void mnuInput_Click(object sender, EventArgs e)
		{
			new frmInputConfig().ShowDialog();
		}

		private void mnuAudioConfig_Click(object sender, EventArgs e)
		{
			new frmAudioConfig().ShowDialog();
		}

		private void mnuRegion_Click(object sender, EventArgs e)
		{
			if(sender == mnuRegionAuto) {
				ConfigManager.Config.Region = NesModel.Auto;
			} else if(sender == mnuRegionNtsc) {
				ConfigManager.Config.Region = NesModel.NTSC;
			} else if(sender == mnuRegionPal) {
				ConfigManager.Config.Region = NesModel.PAL;
			}
			ConfigManager.Config.ApplyConfig();
		}
	}
}
