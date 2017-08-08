using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms
{
	partial class frmMain
	{
		const int NumberOfSaveSlots = 7;
		private void InitializeStateMenu(ToolStripMenuItem menu, bool forSave)
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)(() => this.InitializeStateMenu(menu, forSave)));
			} else {
				menu.DropDownItems.Clear();

				Action<uint> addSaveStateInfo = (i) => {
					Int64 fileTime = InteropEmu.GetStateInfo(i);
					string label;
					if(fileTime == 0) {
						label = i.ToString() + ". " + ResourceHelper.GetMessage("EmptyState");
					} else {
						DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(fileTime).ToLocalTime();
						label = i.ToString() + ". " + dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
					}

					ToolStripMenuItem item = new ToolStripMenuItem(label);
					uint stateIndex = i;
					item.Click += (object sender, EventArgs e) => {
						if(_emuThread != null && !InteropEmu.IsNsf()) {
							if(forSave) {
								InteropEmu.SaveState(stateIndex);
							} else {
								if(!InteropEmu.MoviePlaying() && !InteropEmu.MovieRecording()) {
									InteropEmu.LoadState(stateIndex);
								}
							}
						}
					};

					item.ShortcutKeys = (Keys)((int)Keys.F1 + i - 1);
					if(forSave) {
						item.ShortcutKeys |= Keys.Shift;
					}
					menu.DropDownItems.Add(item);
				};

				for(uint i = 1; i <= frmMain.NumberOfSaveSlots; i++) {
					addSaveStateInfo(i);
				}

				if(!forSave) {
					menu.DropDownItems.Add("-");
					addSaveStateInfo(NumberOfSaveSlots+1);
				}
			}
		}
		
		private void mnuExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void mnuSaveState_DropDownOpening(object sender, EventArgs e)
		{
			InitializeStateMenu(mnuSaveState, true);
		}

		private void mnuLoadState_DropDownOpening(object sender, EventArgs e)
		{
			InitializeStateMenu(mnuLoadState, false);
		}
		
		private void mnuOpen_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterRomIps"));
				if(ConfigManager.Config.RecentFiles.Count > 0) {
					ofd.InitialDirectory = ConfigManager.Config.RecentFiles[0].RomFile.Folder;
				}
				if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					LoadFile(ofd.FileName);
				}
			}
		}

		private bool IsPatchFile(string filename)
		{
			using(FileStream stream = File.OpenRead(filename)) {
				byte[] header = new byte[5];
				stream.Read(header, 0, 5);
				if(header[0] == 'P' && header[1] == 'A' && header[2] == 'T' && header[3] == 'C' && header[4] == 'H') {
					return true;
				} else if((header[0] == 'U' || header[0] == 'B') && header[1] == 'P' && header[2] == 'S' && header[3] == '1') {
					return true;
				}
			}
			return false;
		}

		private void LoadFile(string filename)
		{
			if(File.Exists(filename)) {
				if(IsPatchFile(filename)) {
					LoadPatchFile(filename);
				} else if(Path.GetExtension(filename).ToLowerInvariant() == ".mmo") {
					InteropEmu.MoviePlay(filename);
				} else {
					LoadROM(filename, ConfigManager.Config.PreferenceInfo.AutoLoadIpsPatches);
				}
			}
		}

		private void LoadPatchFile(string patchFile)
		{
			if(_emuThread == null) {
				if(MesenMsgBox.Show("SelectRomIps", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK) {
					using(OpenFileDialog ofd = new OpenFileDialog()) {
						ofd.SetFilter(ResourceHelper.GetMessage("FilterRom"));
						if(ConfigManager.Config.RecentFiles.Count > 0) {
							ofd.InitialDirectory = ConfigManager.Config.RecentFiles[0].RomFile.Folder;
						}

						if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
							LoadROM(ofd.FileName, true, patchFile);
						}
					}
				}
			} else if(MesenMsgBox.Show("PatchAndReset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK) {
				LoadROM(_currentRomPath.Value, true, patchFile);
			}
		}

		private void LoadROM(ResourcePath romFile, bool autoLoadPatches = false, ResourcePath? patchFileToApply = null)
		{
			if(romFile.Exists) {
				if(frmSelectRom.SelectRom(ref romFile)) {
					_currentRomPath = romFile;

					if(romFile.Compressed) {
						Interlocked.Increment(ref _romLoadCounter);
						ctrlNsfPlayer.Visible = false;
						ctrlLoading.Visible = true;
					}

					ResourcePath? patchFile = patchFileToApply;
					if(patchFile == null && autoLoadPatches) {
						string[] extensions = new string[3] { ".ips", ".ups", ".bps" };
						foreach(string ext in extensions) {
							string file = Path.Combine(romFile.Folder, Path.GetFileNameWithoutExtension(romFile.FileName)) + ext;
							if(File.Exists(file)) {
								patchFile = file;
								break;
							}
						}
					}

					Task loadRomTask = new Task(() => {
						lock(_loadRomLock) {
							InteropEmu.LoadROM(romFile, (patchFile.HasValue && patchFile.Value.Exists) ? (string)patchFile.Value : string.Empty);
						}
					});

					loadRomTask.ContinueWith((Task prevTask) => {
						this.BeginInvoke((MethodInvoker)(() => {
							if(romFile.Compressed) {
								Interlocked.Decrement(ref _romLoadCounter);
							}

							ConfigManager.Config.AddRecentFile(romFile, patchFileToApply);
							UpdateRecentFiles();
						}));
					});

					loadRomTask.Start();
				}
			} else {
				MesenMsgBox.Show("FileNotFound", MessageBoxButtons.OK, MessageBoxIcon.Error, romFile.Path);
			}
		}

		private void UpdateRecentFiles()
		{
			mnuRecentFiles.DropDownItems.Clear();
			foreach(RecentItem recentItem in ConfigManager.Config.RecentFiles) {
				ToolStripMenuItem tsmi = new ToolStripMenuItem();
				tsmi.Text = recentItem.ToString();
				tsmi.Click += (object sender, EventArgs args) => {
					LoadROM(recentItem.RomFile, ConfigManager.Config.PreferenceInfo.AutoLoadIpsPatches, recentItem.PatchFile);
				};
				mnuRecentFiles.DropDownItems.Add(tsmi);
			}

			mnuRecentFiles.Enabled = mnuRecentFiles.DropDownItems.Count > 0;
		}
	}
}
