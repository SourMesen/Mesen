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
		private List<ListViewItem> _listItems = new List<ListViewItem>();

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

		public void UpdateLabelListAddresses()
		{
			bool updating = false;
			foreach(ListViewItem item in _listItems) {
				CodeLabel label = (CodeLabel)item.SubItems[1].Tag;

				Int32 relativeAddress = InteropEmu.DebugGetRelativeAddress(label.Address, label.AddressType);
				if(relativeAddress != (Int32)item.Tag) {
					if(!updating) {
						lstLabels.BeginUpdate();
						updating = true;
					}
					if(relativeAddress >= 0) {
						item.SubItems[0].Text = "$" + relativeAddress.ToString("X4");
						item.ForeColor = Color.Black;
						item.Font = new Font(item.Font, FontStyle.Regular);
					} else {
						item.SubItems[0].Text = "[n/a]";
						item.ForeColor = Color.Gray;
						item.Font = new Font(item.Font, FontStyle.Italic);
					}
					item.Tag = relativeAddress;
				}
			}
			if(updating) {
				lstLabels.Sort();
				lstLabels.EndUpdate();
			}
		}

		public void UpdateLabelList()
		{
			lstLabels.BeginUpdate();
			lstLabels.Items.Clear();
			foreach(CodeLabel label in LabelManager.GetLabels()) {
				if(label.Label.Length > 0) {
					ListViewItem item = lstLabels.Items.Add(label.Label);

					Int32 relativeAddress = InteropEmu.DebugGetRelativeAddress(label.Address, label.AddressType);
					if(relativeAddress >= 0) {
						item.SubItems.Add("$" + relativeAddress.ToString("X4"));
					} else {
						item.SubItems.Add("[n/a]");
						item.ForeColor = Color.Gray;
						item.Font = new Font(item.Font, FontStyle.Italic);
					}
					item.SubItems.Add("$" + label.Address.ToString("X4"));
					item.SubItems[1].Tag = label;

					item.Tag = relativeAddress;
				}
			}
			lstLabels.Sort();
			lstLabels.EndUpdate();

			_listItems = new List<ListViewItem>();
			foreach(ListViewItem item in lstLabels.Items) {
				_listItems.Add(item);
			}			
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
