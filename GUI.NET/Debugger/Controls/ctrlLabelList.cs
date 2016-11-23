using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlLabelList : UserControl
	{
		public event EventHandler OnLabelSelected;
		public ctrlLabelList()
		{
			InitializeComponent();
		}

		public static void EditLabel(UInt32 address, AddressType type)
		{
			CodeLabel existingLabel = LabelManager.GetLabel(address, type);
			CodeLabel newLabel = new CodeLabel() { Address = address, AddressType = type, Label = existingLabel?.Label, Comment = existingLabel?.Comment };

			frmEditLabel frm = new frmEditLabel(newLabel, existingLabel);
			if(frm.ShowDialog() == DialogResult.OK) {
				bool empty = string.IsNullOrWhiteSpace(newLabel.Label) && string.IsNullOrWhiteSpace(newLabel.Comment);
				if(existingLabel != null) {
					LabelManager.DeleteLabel(existingLabel.Address, existingLabel.AddressType, empty);
				}
				if(!empty) {
					LabelManager.SetLabel(newLabel.Address, newLabel.AddressType, newLabel.Label, newLabel.Comment);
				}
			}
		}

		public void UpdateLabelList()
		{
			Int32[] entryPoints = InteropEmu.DebugGetFunctionEntryPoints();

			lstLabels.BeginUpdate();
			lstLabels.Items.Clear();
			foreach(KeyValuePair<string, CodeLabel> kvp in LabelManager.GetLabels()) {
				if(kvp.Value.Label.Length > 0) {
					ListViewItem item = lstLabels.Items.Add(kvp.Value.Label);

					Int32 relativeAddress = InteropEmu.DebugGetRelativeAddress(kvp.Value.Address, kvp.Value.AddressType);
					if(relativeAddress >= 0) {
						item.SubItems.Add("$" + relativeAddress.ToString("X4"));
					} else {
						item.SubItems.Add("[n/a]");
						item.ForeColor = Color.Gray;
						item.Font = new Font(item.Font, FontStyle.Italic);
					}
					item.SubItems.Add("$" + kvp.Value.Address.ToString("X4"));
					item.SubItems[1].Tag = kvp.Value;

					item.Tag = relativeAddress;
				}
			}
			lstLabels.Sort();
			lstLabels.EndUpdate();
		}

		private void lstLabels_DoubleClick(object sender, EventArgs e)
		{
			if(lstLabels.SelectedItems.Count > 0) {
				Int32 relativeAddress = (Int32)lstLabels.SelectedItems[0].Tag;

				if(relativeAddress >= 0) {
					OnLabelSelected?.Invoke(relativeAddress, e);
				}
			}
		}

		private void mnuActions_Opening(object sender, CancelEventArgs e)
		{
			mnuDelete.Enabled = lstLabels.SelectedItems.Count > 0;
			mnuEdit.Enabled = lstLabels.SelectedItems.Count == 1;
		}

		private void mnuDelete_Click(object sender, EventArgs e)
		{
			for(int i = lstLabels.SelectedItems.Count - 1; i >= 0; i--) {
				CodeLabel label = (CodeLabel)lstLabels.SelectedItems[i].SubItems[1].Tag;
				LabelManager.DeleteLabel(label.Address, label.AddressType, i == 0);
			}
		}

		private void mnuAdd_Click(object sender, EventArgs e)
		{
			CodeLabel newLabel = new CodeLabel() { Address = 0, AddressType = AddressType.InternalRam, Label = "", Comment = "" };

			frmEditLabel frm = new frmEditLabel(newLabel);
			if(frm.ShowDialog() == DialogResult.OK) {
				LabelManager.SetLabel(newLabel.Address, newLabel.AddressType, newLabel.Label, newLabel.Comment);
			}
		}

		private void mnuEdit_Click(object sender, EventArgs e)
		{
			CodeLabel label = (CodeLabel)lstLabels.SelectedItems[0].SubItems[1].Tag;
			EditLabel(label.Address, label.AddressType);
		}
	}
}
