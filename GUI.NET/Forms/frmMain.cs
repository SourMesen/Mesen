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
		private Thread _renderThread;
		private frmDebugger _debugger;
		private bool _stop = false;
		private List<ToolStripMenuItem> _fpsLimitOptions = new List<ToolStripMenuItem>();
		
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

			UpdateVideoSettings();

			InitializeEmu();
			InitializeFpsLimitMenu();

			UpdateMenus();
			UpdateRecentFiles();
			StartRenderThread();

			if(!DesignMode) {
				Icon = Properties.Resources.MesenIcon;
			}
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

		void InitializeFpsLimitMenu()
		{
			int[] fpsValues = new int[] { 120, 100, 60, 50 , 30, 25, 15, 12 };
			mnuFpsLimitDefault.Tag = -1;
			mnuFpsLimitNoLimit.Tag = 0;
			_fpsLimitOptions.Add(mnuFpsLimitDefault);
			_fpsLimitOptions.Add(mnuFpsLimitNoLimit);
			foreach(int fpsValue in fpsValues) {
				ToolStripMenuItem item = (ToolStripMenuItem)mnuFpsLimit.DropDownItems.Add(fpsValue.ToString());
				item.Tag = fpsValue;
				_fpsLimitOptions.Add(item);
			}

			foreach(ToolStripMenuItem item in _fpsLimitOptions) {
				item.Click += mnuFpsLimitValue_Click;
			}

			UpdateFpsLimitMenu();
		}

		void UpdateFpsLimitMenu()
		{
			foreach(ToolStripMenuItem item in _fpsLimitOptions) {
				item.Checked = ((int)item.Tag == ConfigManager.Config.VideoInfo.FpsLimit);
			}
		}

		private void mnuFpsLimitValue_Click(object sender, EventArgs e)
		{
			int fpsLimit;
			if(sender == mnuFpsLimitNoLimit) {
				fpsLimit = mnuFpsLimitNoLimit.Checked ? -1 : 0;
			} else {
				fpsLimit = (int)((ToolStripItem)sender).Tag;
			}
			ConfigManager.Config.VideoInfo.FpsLimit = fpsLimit;
			ConfigManager.ApplyChanges();
			UpdateFpsLimitMenu();

			VideoInfo.ApplyConfig();
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
			UpdateFpsLimitMenu();
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

					mnuFpsLimit.Enabled = !InteropEmu.IsConnected();

					bool moviePlaying = InteropEmu.MoviePlaying();
					bool movieRecording = InteropEmu.MovieRecording();
					mnuPlayMovie.Enabled = !netPlay && !moviePlaying && !movieRecording;
					mnuStopMovie.Enabled = _emuThread != null && !netPlay && (moviePlaying || movieRecording);
					mnuRecordFrom.Enabled = _emuThread != null && !moviePlaying && !movieRecording;
					mnuRecordFromStart.Enabled = _emuThread != null && !InteropEmu.IsConnected() && !moviePlaying && !movieRecording;
					mnuRecordFromNow.Enabled = _emuThread != null && !moviePlaying && !movieRecording;

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

		private void StartRenderThread()
		{
			_renderThread = new Thread(() => {
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				sw.Start();
				while(true) {
					if(sw.ElapsedMilliseconds > 100) {
						sw.Restart();
						UpdateMenus();
						System.Threading.Thread.MemoryBarrier();
						if(_stop) {
							break;
						}
					}
					if(!InteropEmu.Render()) {
						System.Threading.Thread.Sleep(5);
					}
				}
			});
			_renderThread.Start();
		}
		
		private void StopEmu()
		{
			InteropEmu.Stop();
			_stop = true;
			_renderThread.Join();
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
			if(keyData == Keys.Escape && _emuThread != null && mnuPause.Enabled) {
				PauseEmu();
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

		private void mnuSaveState1_Click(object sender, EventArgs e)
		{
			InteropEmu.SaveState(1);
		}

		private void mnuLoadState1_Click(object sender, EventArgs e)
		{
			InteropEmu.LoadState(1);
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
			if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				InteropEmu.MovieRecord(sfd.FileName, resetEmu);
			}
		}

		private void mnuPlayMovie_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Movie files (*.mmo)|*.mmo|All (*.*)|*.*";
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
