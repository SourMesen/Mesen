using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
		private void mnuDebug_DropDownOpening(object sender, EventArgs e)
		{
			mnuEditHeader.Enabled = _emuThread != null && InteropEmu.GetRomInfo().Format == RomFormat.iNes;
		}

		private void mnuEditHeader_Click(object sender, EventArgs e)
		{
			using(frmEditHeader frm = new frmEditHeader()) {
				frm.ShowDialog(sender, this);
			}
		}

		private void mnuPlayMovie_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterMovie"));
				ofd.InitialDirectory = ConfigManager.MovieFolder;
				if(ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.MoviePlay(ofd.FileName);
				}
			}
		}

		private void mnuStopMovie_Click(object sender, EventArgs e)
		{
			InteropEmu.MovieStop();
		}
		
		private void mnuRecordMovie_Click(object sender, EventArgs e)
		{
			using(frmRecordMovie frm = new frmRecordMovie()) {
				frm.ShowDialog(mnuMovies, this);
			}
		}

		private void mnuWaveRecord_Click(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter(ResourceHelper.GetMessage("FilterWave"));
				sfd.InitialDirectory = ConfigManager.WaveFolder;
				sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".wav";
				if(sfd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
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
				if(frm.ShowDialog(mnuVideoRecorder, this) == DialogResult.OK) {
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
					CheatInfo.ApplyCheats();
					_cheatListWindow = null;
				};
			} else {
				_cheatListWindow.WindowState = FormWindowState.Normal;
				_cheatListWindow.BringToFront();
				_cheatListWindow.Focus();
			}
		}

		private void mnuHistoryViewer_Click(object sender, EventArgs e)
		{
			if(_historyViewerWindow == null) {
				_historyViewerWindow = new frmHistoryViewer();
				_historyViewerWindow.Show();
				_historyViewerWindow.FormClosed += (s, evt) => {
					_historyViewerWindow = null;
				};
			} else {
				_historyViewerWindow.WindowState = FormWindowState.Normal;
				_historyViewerWindow.BringToFront();
				_historyViewerWindow.Focus();
			}
		}

		private void LoadRandomGame()
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
				string randomGame = gameRoms[random.Next(gameRoms.Count)];
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
				_logWindow.WindowState = FormWindowState.Normal;
				_logWindow.BringToFront();
				_logWindow.Focus();
			}
		}
		
		private void mnuInstallHdPack_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterZipFiles"));
				if(ofd.ShowDialog(this) == DialogResult.OK) {
					try {
						using(FileStream stream = File.Open(ofd.FileName, FileMode.Open)) {
							ZipArchive zip = new ZipArchive(stream);

							//Find the hires.txt file
							ZipArchiveEntry hiresEntry = null;
							foreach(ZipArchiveEntry entry in zip.Entries) {
								if(entry.Name == "hires.txt") {
									hiresEntry = entry;
									break;
								}
							}

							if(hiresEntry != null) {
								using(Stream entryStream = hiresEntry.Open()) {
									using(StreamReader reader = new StreamReader(entryStream)) {
										string hiresData = reader.ReadToEnd();
										RomInfo romInfo = InteropEmu.GetRomInfo();

										//If there's a "supportedRom" tag, check if it matches the current ROM
										Regex supportedRomRegex = new Regex("<supportedRom>([^\\n]*)");
										Match match = supportedRomRegex.Match(hiresData);
										if(match.Success) {
											if(!match.Groups[1].Value.ToUpper().Contains(InteropEmu.GetRomInfo().Sha1.ToUpper())) {
												MesenMsgBox.Show("InstallHdPackWrongRom", MessageBoxButtons.OK, MessageBoxIcon.Error);
												return;
											}
										}

										//Extract HD pack
										try {
											string targetFolder = Path.Combine(ConfigManager.HdPackFolder, Path.GetFileNameWithoutExtension(romInfo.GetRomName()));
											if(Directory.Exists(targetFolder)) {
												//Warn if the folder already exists
												if(MesenMsgBox.Show("InstallHdPackConfirmOverwrite", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, targetFolder) != DialogResult.OK) {
													return;
												}
											} else {
												Directory.CreateDirectory(targetFolder);
											}

											string hiresFileFolder = hiresEntry.FullName.Substring(0, hiresEntry.FullName.Length - "hires.txt".Length);
											foreach(ZipArchiveEntry entry in zip.Entries) {
												//Extract only the files in the same subfolder as the hires.txt file (and only if they have a name & size > 0)
												if(!string.IsNullOrWhiteSpace(entry.Name) && entry.Length > 0 && entry.FullName.StartsWith(hiresFileFolder)) {
													entry.ExtractToFile(Path.Combine(targetFolder, entry.Name), true);
												}
											}
										} catch(Exception ex) {
											MesenMsgBox.Show("InstallHdPackError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
											return;
										}
									}

									//Turn on HD Pack support automatically after installation succeeds
									ConfigManager.Config.VideoInfo.UseHdPacks = true;
									ConfigManager.ApplyChanges();
									ConfigManager.Config.ApplyConfig();

									if(MesenMsgBox.Show("InstallHdPackConfirmReset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
										//Power cycle game if the user agrees
										InteropEmu.PowerCycle();
									}
								}
							} else {
								MesenMsgBox.Show("InstallHdPackInvalidPack", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					} catch {
						//Invalid file (file missing, not a zip file, etc.)
						MesenMsgBox.Show("InstallHdPackInvalidZipFile", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
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
				_hdPackEditorWindow.WindowState = FormWindowState.Normal;
				_hdPackEditorWindow.BringToFront();
				_hdPackEditorWindow.Focus();
			}
		}

		private void mnuStartServer_Click(object sender, EventArgs e)
		{
			if(InteropEmu.IsServerRunning()) {
				Task.Run(() => InteropEmu.StopServer());
			} else {
				using(frmServerConfig frm = new frmServerConfig()) {
					if(frm.ShowDialog(sender, this) == System.Windows.Forms.DialogResult.OK) {
						InteropEmu.StartServer(ConfigManager.Config.ServerInfo.Port, ConfigManager.Config.ServerInfo.Password, ConfigManager.Config.Profile.PlayerName);
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
					if(frm.ShowDialog(sender, this) == System.Windows.Forms.DialogResult.OK) {
						Task.Run(() => {
							InteropEmu.Connect(
								ConfigManager.Config.ClientConnectionInfo.Host,
								ConfigManager.Config.ClientConnectionInfo.Port,
								ConfigManager.Config.ClientConnectionInfo.Password,
								ConfigManager.Config.Profile.PlayerName,
								ConfigManager.Config.ClientConnectionInfo.Spectator
							);
						});
					}
				}
			}
		}

		private void mnuProfile_Click(object sender, EventArgs e)
		{
			using(frmPlayerProfile frm = new frmPlayerProfile()) {
				frm.ShowDialog(sender, this);
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
		
		private void mnuNetPlayPlayer5_Click(object sender, EventArgs e)
		{
			InteropEmu.NetPlaySelectController(4);
		}

		private void mnuNetPlaySpectator_Click(object sender, EventArgs e)
		{
			InteropEmu.NetPlaySelectController(0xFF);
		}

		private void mnuApuViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.ApuViewer);
		}

		private void mnuAssembler_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.Assembler);
		}

		private void mnuDebugger_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.Debugger);
		}

		private void mnuDebugDebugger_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.Debugger);
		}

		private void mnuMemoryViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.MemoryViewer);
		}

		private void mnuEventViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.EventViewer);
		}

		private void mnuPpuViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.PpuViewer);
		}

		private void mnuScriptWindow_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.ScriptWindow);
		}

		private void mnuTraceLogger_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.TraceLogger);
		}

		private void mnuTextHooker_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.TextHooker);
		}

		private void mnuDebugDualSystemSecondaryCpu_Click(object sender, EventArgs e)
		{
			bool switchCpu = false;

			if(!DebugWindowManager.HasOpenedWindow) {
				switchCpu = true;
			} else {
				if(MessageBox.Show("Warning: Changing this setting will close all currently opened debug tools!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
					DebugWindowManager.CloseAll();
					if(!DebugWindowManager.HasOpenedWindow) {
						switchCpu = true;
					}
				}
			}

			if(switchCpu) {
				mnuDebugDualSystemSecondaryCpu.Checked = !mnuDebugDualSystemSecondaryCpu.Checked;
				ConfigManager.Config.DebugInfo.DebugConsoleId = mnuDebugDualSystemSecondaryCpu.Checked ? InteropEmu.ConsoleId.Slave : InteropEmu.ConsoleId.Master;
				ConfigManager.ApplyChanges();
				InteropEmu.DebugSetDebuggerConsole(ConfigManager.Config.DebugInfo.DebugConsoleId == InteropEmu.ConsoleId.Slave ? 1 : 0);
			}
		}
	}

}
