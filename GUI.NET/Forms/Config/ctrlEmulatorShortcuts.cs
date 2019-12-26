﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using Mesen.GUI.Config;
using System.Reflection;
using Mesen.GUI.Controls;
using System.Collections.Generic;

namespace Mesen.GUI.Forms.Config
{
	public partial class ctrlEmulatorShortcuts : BaseControl
	{
		public ctrlEmulatorShortcuts()
		{
			InitializeComponent();

			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime) {
				InitializeGrid();
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.colAction.Width = (int)(this.Width / 2.2);
			this.colBinding1.Width = this.Width / 4;
			this.colBinding2.Width = this.Width / 4;
		}

		private void InitializeGrid()
		{
			EmulatorShortcut[] displayOrder = new EmulatorShortcut[] {
				EmulatorShortcut.FastForward,
				EmulatorShortcut.ToggleFastForward,
				EmulatorShortcut.Rewind,
				EmulatorShortcut.ToggleRewind,
				EmulatorShortcut.RewindTenSecs,
				EmulatorShortcut.RewindOneMin,

				EmulatorShortcut.Pause,
				EmulatorShortcut.Reset,
				EmulatorShortcut.PowerCycle,
				EmulatorShortcut.ReloadRom,
				EmulatorShortcut.PowerOff,
				EmulatorShortcut.Exit,

				EmulatorShortcut.InsertNextDisk,
				EmulatorShortcut.SwitchDiskSide,
				EmulatorShortcut.EjectDisk,

				EmulatorShortcut.InsertCoin1,
				EmulatorShortcut.InsertCoin2,
				EmulatorShortcut.InsertCoin3,
				EmulatorShortcut.InsertCoin4,
				EmulatorShortcut.VsServiceButton,
				EmulatorShortcut.VsServiceButton2,

				EmulatorShortcut.InputBarcode,

				EmulatorShortcut.TakeScreenshot,
				EmulatorShortcut.LoadRandomGame,
				EmulatorShortcut.RunSingleFrame,

				EmulatorShortcut.SetScale1x,
				EmulatorShortcut.SetScale2x,
				EmulatorShortcut.SetScale3x,
				EmulatorShortcut.SetScale4x,
				EmulatorShortcut.SetScale5x,
				EmulatorShortcut.SetScale6x,
				EmulatorShortcut.ToggleFullscreen,

				EmulatorShortcut.ToggleDebugInfo,
				EmulatorShortcut.ToggleFps,
				EmulatorShortcut.ToggleGameTimer,
				EmulatorShortcut.ToggleFrameCounter,
				EmulatorShortcut.ToggleLagCounter,
				EmulatorShortcut.ToggleOsd,
				EmulatorShortcut.ToggleAlwaysOnTop,
				EmulatorShortcut.ToggleBackground,
				EmulatorShortcut.ToggleSprites,
				EmulatorShortcut.ToggleCheats,
				EmulatorShortcut.ToggleAudio,
				EmulatorShortcut.ToggleKeyboardMode,

				EmulatorShortcut.MaxSpeed,
				EmulatorShortcut.IncreaseSpeed,
				EmulatorShortcut.DecreaseSpeed,

				EmulatorShortcut.OpenFile,
								
				EmulatorShortcut.MoveToNextStateSlot,
				EmulatorShortcut.MoveToPreviousStateSlot,
				EmulatorShortcut.SaveState,
				EmulatorShortcut.LoadState,

				EmulatorShortcut.SaveStateSlot1,
				EmulatorShortcut.SaveStateSlot2,
				EmulatorShortcut.SaveStateSlot3,
				EmulatorShortcut.SaveStateSlot4,
				EmulatorShortcut.SaveStateSlot5,
				EmulatorShortcut.SaveStateSlot6,
				EmulatorShortcut.SaveStateSlot7,
				EmulatorShortcut.SaveStateSlot8,
				EmulatorShortcut.SaveStateSlot9,
				EmulatorShortcut.SaveStateSlot10,
				EmulatorShortcut.SaveStateToFile,

				EmulatorShortcut.LoadStateSlot1,
				EmulatorShortcut.LoadStateSlot2,
				EmulatorShortcut.LoadStateSlot3,
				EmulatorShortcut.LoadStateSlot4,
				EmulatorShortcut.LoadStateSlot5,
				EmulatorShortcut.LoadStateSlot6,
				EmulatorShortcut.LoadStateSlot7,
				EmulatorShortcut.LoadStateSlot8,
				EmulatorShortcut.LoadStateSlot9,
				EmulatorShortcut.LoadStateSlot10,
				EmulatorShortcut.LoadStateSlotAuto,
				EmulatorShortcut.LoadStateFromFile,

				EmulatorShortcut.SelectSaveSlot1,
				EmulatorShortcut.SelectSaveSlot2,
				EmulatorShortcut.SelectSaveSlot3,
				EmulatorShortcut.SelectSaveSlot4,
				EmulatorShortcut.SelectSaveSlot5,
				EmulatorShortcut.SelectSaveSlot6,
				EmulatorShortcut.SelectSaveSlot7,
				EmulatorShortcut.SelectSaveSlot8,
				EmulatorShortcut.SelectSaveSlot9,
				EmulatorShortcut.SelectSaveSlot10,

				EmulatorShortcut.LoadLastSession,
			};

			HashSet<string> keyCombinations = new HashSet<string>();

			foreach(EmulatorShortcut shortcut in displayOrder) {
				int i = gridShortcuts.Rows.Add();
				gridShortcuts.Rows[i].Cells[0].Tag = shortcut;
				gridShortcuts.Rows[i].Cells[0].Value = ResourceHelper.GetMessage("EmulatorShortcutMappings_" + shortcut.ToString());

				int keyIndex = ConfigManager.Config.PreferenceInfo.ShortcutKeys1.FindIndex((ShortcutKeyInfo shortcutInfo) => shortcutInfo.Shortcut == shortcut);
				if(keyIndex >= 0) {
					KeyCombination keyComb = ConfigManager.Config.PreferenceInfo.ShortcutKeys1[keyIndex].KeyCombination;
					keyCombinations.Add(keyComb.ToString());
					gridShortcuts.Rows[i].Cells[1].Value = keyComb.ToString();
					gridShortcuts.Rows[i].Cells[1].Tag = keyComb;
				}

				keyIndex = ConfigManager.Config.PreferenceInfo.ShortcutKeys2.FindIndex((ShortcutKeyInfo shortcutInfo) => shortcutInfo.Shortcut == shortcut);
				if(keyIndex >= 0) {
					KeyCombination keyComb = ConfigManager.Config.PreferenceInfo.ShortcutKeys2[keyIndex].KeyCombination;
					keyCombinations.Add(keyComb.ToString());
					gridShortcuts.Rows[i].Cells[2].Value = keyComb.ToString();
					gridShortcuts.Rows[i].Cells[2].Tag = keyComb;
				}
			}

			CheckConflicts();
		}

		private void CheckConflicts()
		{
			HashSet<string> keyCombinations = new HashSet<string>();

			for(int i = gridShortcuts.Rows.Count - 1; i >= 0; i--) {
				EmulatorShortcut shortcut = (EmulatorShortcut)gridShortcuts.Rows[i].Cells[0].Tag;
				for(int j = 1; j <= 2; j++) {
					if(gridShortcuts.Rows[i].Cells[j].Tag != null) {
						KeyCombination keyComb = (KeyCombination)gridShortcuts.Rows[i].Cells[j].Tag;
						if(!keyComb.IsEmpty && !keyCombinations.Add(keyComb.ToString())) {
							pnlConflictWarning.Visible = true;
							return;
						}
					}
				}
			}
			
			pnlConflictWarning.Visible = false;
		}

		public void UpdateConfig()
		{
			//Need to box the structs into objects for SetValue to work properly
			var keySet1 = new List<ShortcutKeyInfo>();
			var keySet2 = new List<ShortcutKeyInfo>();

			for(int i = gridShortcuts.Rows.Count - 1; i >= 0; i--) {
				EmulatorShortcut shortcut = (EmulatorShortcut)gridShortcuts.Rows[i].Cells[0].Tag;
				if(gridShortcuts.Rows[i].Cells[1].Tag != null) {
					KeyCombination keyComb = (KeyCombination)gridShortcuts.Rows[i].Cells[1].Tag;
					if(!keyComb.IsEmpty) {
						keySet1.Add(new ShortcutKeyInfo(shortcut, keyComb));
					}
				}
				if(gridShortcuts.Rows[i].Cells[2].Tag != null) {
					KeyCombination keyComb = (KeyCombination)gridShortcuts.Rows[i].Cells[2].Tag;
					if(!keyComb.IsEmpty) {
						keySet2.Add(new ShortcutKeyInfo(shortcut, keyComb));
					}
				}
			}

			ConfigManager.Config.PreferenceInfo.ShortcutKeys1 = keySet1;
			ConfigManager.Config.PreferenceInfo.ShortcutKeys2 = keySet2;
		}

		private void gridShortcuts_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			//Right-click on buttons to clear mappings
			if(gridShortcuts.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0) {
				DataGridViewButtonCell button = gridShortcuts.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
				if(button != null) {
					if(e.Button == MouseButtons.Right) {
						button.Value = "";
						button.Tag = new KeyCombination();
						CheckConflicts();
					} else if(e.Button == MouseButtons.Left) {
						using(frmGetKey frm = new frmGetKey(false)) {
							frm.ShowDialog();
							button.Value = frm.ShortcutKey.ToString();
							button.Tag = frm.ShortcutKey;

							CheckConflicts();
						}
					}
				}
			}
		}
	}
}
