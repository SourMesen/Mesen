using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Debugger;
using Mesen.GUI.Forms.Cheats;
using Mesen.GUI.Forms.HdPackEditor;
using Mesen.GUI.Forms.NetPlay;

namespace Mesen.GUI.Forms
{
	public partial class frmMain
	{
		private void mnuDebugger_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.Debugger);
		}

		private void mnuDebugDebugger_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.Debugger);
		}

		private void mnuTraceLogger_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.TraceLogger);
		}

		private void mnuPpuViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.PpuViewer);
		}

		private void mnuMemoryViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.MemoryViewer);
		}

		private void mnuAssembler_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.Assembler);
		}

		private void mnuScriptWindow_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.ScriptWindow);
		}

		private void mnuEditHeader_Click(object sender, EventArgs e)
		{
			using(frmEditHeader frm = new frmEditHeader()) {
				frm.ShowDialog(sender, this);
			}
		}

		private void RecordMovie(bool resetEmu)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter(ResourceHelper.GetMessage("FilterMovie"));
				sfd.InitialDirectory = ConfigManager.MovieFolder;
				sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".mmo";
				if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.MovieRecord(sfd.FileName, resetEmu);
				}
			}
		}

		private void mnuPlayMovie_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterMovie"));
				ofd.InitialDirectory = ConfigManager.MovieFolder;
				if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.MoviePlay(ofd.FileName);
				}
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

		private void mnuWaveRecord_Click(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter(ResourceHelper.GetMessage("FilterWave"));
				sfd.InitialDirectory = ConfigManager.WaveFolder;
				sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".wav";
				if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.WaveRecord(sfd.FileName);
				}
			}
		}

		private void mnuWaveStop_Click(object sender, EventArgs e)
		{
			InteropEmu.WaveStop();
		}

		private void mnuAviRecord_Click(object sender, EventArgs e)
		{
			using(frmRecordAvi frm = new frmRecordAvi()) {
				if(frm.ShowDialog(mnuVideoRecorder) == DialogResult.OK) {
					InteropEmu.AviRecord(frm.Filename, ConfigManager.Config.AviRecordInfo.Codec, ConfigManager.Config.AviRecordInfo.CompressionLevel);
				}
			}
		}

		private void mnuAviStop_Click(object sender, EventArgs e)
		{
			InteropEmu.AviStop();
		}

		private void mnuCheats_Click(object sender, EventArgs e)
		{
			if(_cheatListWindow == null) {
				_cheatListWindow = new frmCheatList();
				_cheatListWindow.Show(sender, this);
				_cheatListWindow.FormClosed += (s, evt) => {
					if(_cheatListWindow.DialogResult == DialogResult.OK) {
						CheatInfo.ApplyCheats();
					}
					_cheatListWindow = null;
				};
			} else {
				_cheatListWindow.Focus();
			}
		}

		private void mnuRandomGame_Click(object sender, EventArgs e)
		{
			IEnumerable<string> gameFolders = ConfigManager.Config.RecentFiles.Select(recentFile => recentFile.RomFile.Folder.ToLowerInvariant()).Distinct();
			List<string> gameRoms = new List<string>();

			foreach(string folder in gameFolders) {
				if(Directory.Exists(folder)) {
					gameRoms.AddRange(Directory.EnumerateFiles(folder, "*.nes", SearchOption.TopDirectoryOnly));
					gameRoms.AddRange(Directory.EnumerateFiles(folder, "*.unf", SearchOption.TopDirectoryOnly));
					gameRoms.AddRange(Directory.EnumerateFiles(folder, "*.fds", SearchOption.TopDirectoryOnly));
				}
			}

			if(gameRoms.Count == 0) {
				MesenMsgBox.Show("RandomGameNoGameFound", MessageBoxButtons.OK, MessageBoxIcon.Information);
			} else {
				Random random = new Random();
				string randomGame = gameRoms[random.Next(gameRoms.Count - 1)];
				LoadFile(randomGame);
			}
		}

		private void mnuLogWindow_Click(object sender, EventArgs e)
		{
			if(_logWindow == null) {
				_logWindow = new frmLogWindow();
				_logWindow.Show(sender, this);
				_logWindow.FormClosed += (object a, FormClosedEventArgs b) => {
					_logWindow = null;
				};
			} else {
				_logWindow.Focus();
			}
		}

		private void mnuHdPackEditor_Click(object sender, EventArgs e)
		{
			if(_hdPackEditorWindow == null) {
				_hdPackEditorWindow = new frmHdPackEditor();
				_hdPackEditorWindow.Show(sender, this);
				_hdPackEditorWindow.FormClosed += (object a, FormClosedEventArgs b) => {
					_hdPackEditorWindow = null;
				};
			} else {
				_hdPackEditorWindow.Focus();
			}
		}
		
		private void mnuTakeScreenshot_Click(object sender, EventArgs e)
		{
			InteropEmu.TakeScreenshot();
		}

		private void mnuStartServer_Click(object sender, EventArgs e)
		{
			if(InteropEmu.IsServerRunning()) {
				Task.Run(() => InteropEmu.StopServer());
			} else {
				using(frmServerConfig frm = new frmServerConfig()) {
					if(frm.ShowDialog(sender) == System.Windows.Forms.DialogResult.OK) {
						InteropEmu.StartServer(ConfigManager.Config.ServerInfo.Port, ConfigManager.Config.Profile.PlayerName);
					}
				}
			}
		}

		private void mnuConnect_Click(object sender, EventArgs e)
		{
			if(InteropEmu.IsConnected()) {
				Task.Run(() => InteropEmu.Disconnect());
			} else {
				using(frmClientConfig frm = new frmClientConfig()) {
					if(frm.ShowDialog(sender) == System.Windows.Forms.DialogResult.OK) {
						Task.Run(() => {
							InteropEmu.Connect(ConfigManager.Config.ClientConnectionInfo.Host, ConfigManager.Config.ClientConnectionInfo.Port, ConfigManager.Config.Profile.PlayerName, ConfigManager.Config.ClientConnectionInfo.Spectator);
						});
					}
				}
			}
		}

		private void mnuProfile_Click(object sender, EventArgs e)
		{
			using(frmPlayerProfile frm = new frmPlayerProfile()) {
				frm.ShowDialog(sender);
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
	}
}
