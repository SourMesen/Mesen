using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Debugger;
using Mesen.GUI.Forms.Cheats;
using Mesen.GUI.Forms.NetPlay;

namespace Mesen.GUI.Forms
{
	public partial class frmMain : Form
	{
		private InteropEmu.NotificationListener _notifListener;
		private Thread _emuThread;
		private Thread _renderThread;
		private bool _stop = false;
		
		public frmMain()
		{
			Application.ThreadException += Application_ThreadException;
			InitializeComponent();

			InteropEmu.InitializeEmu(this.Handle, this.dxViewer.Handle);
			InteropEmu.SetFlags((int)EmulationFlags.LimitFPS);
			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			UpdateMenus();
			UpdateRecentFiles();
			StartRenderThread();

			Icon = Properties.Resources.MesenIcon;
		}

		void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.GameLoaded) {
				this.StartEmuThread();
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
					mnuSaveState.Enabled = mnuLoadState.Enabled = mnuPause.Enabled = mnuStop.Enabled = mnuReset.Enabled = (_emuThread != null && !InteropEmu.IsConnected());
					mnuPause.Text = InteropEmu.IsPaused() ? "Resume" : "Pause";

					bool netPlay = InteropEmu.IsServerRunning() || InteropEmu.IsConnected();

					mnuStartServer.Enabled = !netPlay;
					mnuStopServer.Enabled = !mnuStartServer.Enabled && !InteropEmu.IsConnected();

					mnuConnect.Enabled = !netPlay;
					mnuDisconnect.Enabled = !mnuConnect.Enabled && !InteropEmu.IsServerRunning();

					bool moviePlaying = InteropEmu.MoviePlaying();
					bool movieRecording = InteropEmu.MovieRecording();
					mnuPlayMovie.Enabled = _emuThread != null && !netPlay && !moviePlaying && !movieRecording;
					mnuStopMovie.Enabled = _emuThread != null && !netPlay && (moviePlaying || movieRecording);
					mnuRecordFrom.Enabled = _emuThread != null && !moviePlaying && !movieRecording;
					mnuRecordFromStart.Enabled = _emuThread != null && !InteropEmu.IsConnected() && !moviePlaying && !movieRecording;
					mnuRecordFromNow.Enabled = _emuThread != null && !moviePlaying && !movieRecording;

					mnuDebugger.Enabled = !netPlay && _emuThread != null;

					mnuTakeScreenshot.Enabled = _emuThread != null;
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
				int count = 0;
				while(true) {
					InteropEmu.Render();
					count++;
					if(count == 20) {
						count = 0;
						UpdateMenus();
					}
					System.Threading.Thread.Sleep(5);
					System.Threading.Thread.MemoryBarrier();
					if(_stop) {
						break;
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

		private void mnuLimitFPS_Click(object sender, EventArgs e)
		{
			if(mnuLimitFPS.Checked) {
				InteropEmu.ClearFlags((int)EmulationFlags.LimitFPS);
			} else {
				InteropEmu.SetFlags((int)EmulationFlags.LimitFPS);
			}
			mnuLimitFPS.Checked = !mnuLimitFPS.Checked;
		}

		private void mnuShowFPS_Click(object sender, EventArgs e)
		{

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

		}
		
		private void mnuDebugger_Click(object sender, EventArgs e)
		{
			new frmDebugger().Show();
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
		
		private enum EmulationFlags
		{
			LimitFPS = 0x01,
			Paused = 0x02,
		}

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
			new frmCheatList().Show(this);
		}
	}
}
