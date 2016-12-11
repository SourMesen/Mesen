using System;
using System.ComponentModel;
using System.Windows.Forms;
using Mesen.GUI.Config;
using System.Reflection;
using Mesen.GUI.Controls;

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

		private void InitializeGrid()
		{
			FieldInfo[] fields = typeof(EmulatorKeyMappings).GetFields();
			foreach(FieldInfo fieldInfo in fields) {
				int index = gridShortcuts.Rows.Add();
				gridShortcuts.Rows[index].Cells[0].Tag = fieldInfo;
				gridShortcuts.Rows[index].Cells[0].Value = ResourceHelper.GetMessage("EmulatorShortcutMappings_" + fieldInfo.Name);

				UInt32 keyCode = (UInt32)fieldInfo.GetValue(ConfigManager.Config.PreferenceInfo.EmulatorKeySet1);
				gridShortcuts.Rows[index].Cells[1].Value = InteropEmu.GetKeyName(keyCode);
				gridShortcuts.Rows[index].Cells[1].Tag = keyCode;

				keyCode = (UInt32)fieldInfo.GetValue(ConfigManager.Config.PreferenceInfo.EmulatorKeySet2);
				gridShortcuts.Rows[index].Cells[2].Value = InteropEmu.GetKeyName(keyCode);
				gridShortcuts.Rows[index].Cells[2].Tag = keyCode;
			}
		}

		public void UpdateConfig()
		{
			//Need to box the structs into objects for SetValue to work properly
			object keySet1 = new EmulatorKeyMappings();
			object keySet2 = new EmulatorKeyMappings();

			for(int i = gridShortcuts.Rows.Count - 1; i >= 0; i--) {
				FieldInfo field = (FieldInfo)gridShortcuts.Rows[i].Cells[0].Tag;
				field.SetValue(keySet1, gridShortcuts.Rows[i].Cells[1].Tag);
				field.SetValue(keySet2, gridShortcuts.Rows[i].Cells[2].Tag);
			}

			ConfigManager.Config.PreferenceInfo.EmulatorKeySet1 = (EmulatorKeyMappings)keySet1;
			ConfigManager.Config.PreferenceInfo.EmulatorKeySet2 = (EmulatorKeyMappings)keySet2;
		}

		private void gridShortcuts_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if(gridShortcuts.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0) {
				DataGridViewButtonCell button = gridShortcuts.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
				if(button != null) {
					frmGetKey frm = new frmGetKey();
					frm.ShowDialog();
					button.Value = frm.BindedKeyName;
					button.Tag = frm.BindedKeyCode;
				}
			}
		}
	}
}
