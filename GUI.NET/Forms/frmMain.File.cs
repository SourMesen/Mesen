﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Properties;

namespace Mesen.GUI.Forms
{
	partial class frmMain
	{
		const int NumberOfSaveSlots = 10;
		private void UpdateStateMenu(ToolStripMenuItem menu, bool forSave)
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)(() => this.UpdateStateMenu(menu, forSave)));
			} else {
				for(uint i = 1; i <= frmMain.NumberOfSaveSlots + (forSave ? 0 : 1); i++) {
					string statePath = Path.Combine(ConfigManager.SaveStateFolder, InteropEmu.GetRomInfo().GetRomName() + "_" + i + ".mst");
					string label;
					bool isAutoSaveSlot = i == NumberOfSaveSlots + 1;
					string slotName = isAutoSaveSlot ? "Auto" : i.ToString();

					if(!File.Exists(statePath)) {
						label = slotName + ". " + ResourceHelper.GetMessage("EmptyState");
					} else {
						DateTime dateTime = new FileInfo(statePath).LastWriteTime;
						label = slotName + ". " + dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
					}

					if(i == NumberOfSaveSlots + 1) {
						//Autosave slot (load only)
						menu.DropDownItems[NumberOfSaveSlots + 1].Text = label;
					} else {
						menu.DropDownItems[(int)i - 1].Text = label;
					}
				}
			}
		}

		private void InitializeStateMenu(ToolStripMenuItem menu, bool forSave)
		{
			Action<uint> addSaveStateInfo = (i) => {
				ToolStripMenuItem item = new ToolStripMenuItem();
				menu.DropDownItems.Add(item);

				if(forSave) {
					BindShortcut(item, (EmulatorShortcut)((int)EmulatorShortcut.SaveStateSlot1 + i - 1));
				} else {
					BindShortcut(item, (EmulatorShortcut)((int)EmulatorShortcut.LoadStateSlot1 + i - 1));
				}
			};

			for(uint i = 1; i <= frmMain.NumberOfSaveSlots; i++) {
				addSaveStateInfo(i);
			}

			if(!forSave) {
				menu.DropDownItems.Add("-");
				addSaveStateInfo(NumberOfSaveSlots+1);
				menu.DropDownItems.Add("-");

				ToolStripMenuItem loadDialog = new ToolStripMenuItem(ResourceHelper.GetMessage("LoadStateDialog"), Resources.SplitView);
				menu.DropDownItems.Add(loadDialog);
				BindShortcut(loadDialog, EmulatorShortcut.LoadStateDialog, () => _emuThread != null && !InteropEmu.IsConnected() && !InteropEmu.IsNsf());

				ToolStripMenuItem loadFromFile = new ToolStripMenuItem(ResourceHelper.GetMessage("LoadFromFile"), Resources.FolderOpen);
				menu.DropDownItems.Add(loadFromFile);
				BindShortcut(loadFromFile, EmulatorShortcut.LoadStateFromFile);					
			} else {
				menu.DropDownItems.Add("-");
				ToolStripMenuItem saveDialog = new ToolStripMenuItem(ResourceHelper.GetMessage("SaveStateDialog"), Resources.SplitView);
				menu.DropDownItems.Add(saveDialog);
				BindShortcut(saveDialog, EmulatorShortcut.SaveStateDialog, () => _emuThread != null && !InteropEmu.IsConnected() && !InteropEmu.IsNsf());

				ToolStripMenuItem saveToFile = new ToolStripMenuItem(ResourceHelper.GetMessage("SaveToFile"), Resources.Floppy);
				menu.DropDownItems.Add(saveToFile);
				BindShortcut(saveToFile, EmulatorShortcut.SaveStateToFile);
			}
		}

		private void SaveState(uint slot)
		{
			if(_emuThread != null && !InteropEmu.IsNsf()) {
				InteropEmu.SaveState(slot);
			}
		}

		private void LoadState(uint slot)
		{
			if(_emuThread != null && !InteropEmu.IsNsf()) {
				if(!InteropEmu.MoviePlaying() && !InteropEmu.MovieRecording()) {
					InteropEmu.LoadState(slot);
				}
			}
		}

		private void LoadStateFromFile()
		{
			if(_emuThread != null) {
				using(OpenFileDialog ofd = new OpenFileDialog()) {
					ofd.InitialDirectory = ConfigManager.SaveStateFolder;
					ofd.SetFilter(ResourceHelper.GetMessage("FilterSavestate"));
					if(ofd.ShowDialog(this) == DialogResult.OK) {
						InteropEmu.LoadStateFile(ofd.FileName);
					}
				}
			}
		}

		private void SaveStateToFile()
		{
			if(_emuThread != null) {
				using(SaveFileDialog sfd = new SaveFileDialog()) {
					sfd.InitialDirectory = ConfigManager.SaveStateFolder;
					sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".mst";
					sfd.SetFilter(ResourceHelper.GetMessage("FilterSavestate"));
					if(sfd.ShowDialog(this) == DialogResult.OK) {
						InteropEmu.SaveStateFile(sfd.FileName);
					}
				}
			}
		}

		private void mnuSaveState_DropDownOpening(object sender, EventArgs e)
		{
			UpdateStateMenu(mnuSaveState, true);
		}

		private void mnuLoadState_DropDownOpening(object sender, EventArgs e)
		{
			UpdateStateMenu(mnuLoadState, false);
		}

		private void LoadLastSession()
		{
			string recentGameFile = Path.Combine(ConfigManager.RecentGamesFolder, Path.GetFileNameWithoutExtension(_currentRomPath.Value.FileName) + ".rgd");
			if(File.Exists(recentGameFile)) {
				InteropEmu.LoadRecentGame(recentGameFile, false);
			}
		}

		private void OpenFile()
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterRomIps"));

				if(ConfigManager.Config.PreferenceInfo.OverrideGameFolder && Directory.Exists(ConfigManager.Config.PreferenceInfo.GameFolder)) {
					ofd.InitialDirectory = ConfigManager.Config.PreferenceInfo.GameFolder;
				} else if(ConfigManager.Config.RecentFiles.Count > 0) {
					ofd.InitialDirectory = ConfigManager.Config.RecentFiles[0].RomFile.Folder;
				}

				if(ofd.ShowDialog(this) == DialogResult.OK) {
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
				} else if(Path.GetExtension(filename).ToLowerInvariant() == ".mst") {
					InteropEmu.LoadStateFile(filename);
				} else if(Path.GetExtension(filename).ToLowerInvariant() == ".mmo") {
					InteropEmu.MoviePlay(filename);
				} else {
					LoadROM(filename, ConfigManager.Config.PreferenceInfo.AutoLoadIpsPatches);
				}
			}
		}

		private void LoadPatchFile(string patchFile)
		{
			string patchFolder = Path.GetDirectoryName(patchFile);
			HashSet<string> romExtensions = new HashSet<string>() { ".nes", ".fds", ".unf", "*.unif" };
			List<string> romsInFolder = new List<string>();
			foreach(string filepath in Directory.EnumerateFiles(patchFolder)) {
				if(romExtensions.Contains(Path.GetExtension(filepath).ToLowerInvariant())) {
					romsInFolder.Add(filepath);
				}
			}

			if(romsInFolder.Count == 1) {
				//There is a single rom in the same folder as the IPS/BPS patch, use it automatically
				LoadROM(romsInFolder[0], true, patchFile);
			} else {
				if(_emuThread == null) {
					//Prompt the user for a rom to load
					if(MesenMsgBox.Show("SelectRomIps", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
						using(OpenFileDialog ofd = new OpenFileDialog()) {
							ofd.SetFilter(ResourceHelper.GetMessage("FilterRom"));
							if(ConfigManager.Config.RecentFiles.Count > 0) {
								ofd.InitialDirectory = ConfigManager.Config.RecentFiles[0].RomFile.Folder;
							}

							if(ofd.ShowDialog(this) == DialogResult.OK) {
								LoadROM(ofd.FileName, true, patchFile);
							}
						}
					}
				} else if(MesenMsgBox.Show("PatchAndReset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
					//Confirm that the user wants to patch the current rom and reset
					LoadROM(_currentRomPath.Value, true, patchFile);
				}
			}
		}

		private ResourcePath? GetIpsFile(ResourcePath romFile)
		{
			string[] extensions = new string[3] { ".ips", ".ups", ".bps" };
			foreach(string ext in extensions) {
				//Check if [romname].ips exists
				string file = Path.Combine(romFile.Folder, Path.GetFileNameWithoutExtension(romFile.FileName)) + ext;
				if(File.Exists(file)) {
					return file;
				} else {
					//Check if [romname].[romext].ips exists
					file = Path.Combine(romFile.Folder, Path.GetFileName(romFile.FileName)) + ext;
					if(File.Exists(file)) {
						return file;
					}
				}
			}
			return null;
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
						patchFile = GetIpsFile(romFile);
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
						}));
					});

					loadRomTask.Start();
				}
			} else {
				MesenMsgBox.Show("FileNotFound", MessageBoxButtons.OK, MessageBoxIcon.Error, romFile.Path);
			}
		}
		
		private void mnuRecentFiles_DropDownOpening(object sender, EventArgs e)
		{
			UpdateRecentFiles();
		}
		
		private void mnuFile_DropDownOpening(object sender, EventArgs e)
		{
			mnuRecentFiles.Enabled = ConfigManager.Config.RecentFiles.Count > 0;
			mnuLoadLastSession.Visible = !ConfigManager.Config.PreferenceInfo.DisableGameSelectionScreen;
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

			mnuRecentFiles.DropDownItems.Add(new ToolStripSeparator());

			ToolStripMenuItem clearHistory = new ToolStripMenuItem();
			clearHistory.Text = ResourceHelper.GetMessage("ClearHistory");
			clearHistory.Image = Resources.Close;
			clearHistory.Click += (object sender, EventArgs args) => {
				ConfigManager.Config.RecentFiles = new List<RecentItem>();
			};
			mnuRecentFiles.DropDownItems.Add(clearHistory);
		}
	}
}
