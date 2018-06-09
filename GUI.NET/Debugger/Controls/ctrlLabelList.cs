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
using Mesen.GUI.Controls;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlLabelList : BaseControl
	{
		public event EventHandler OnFindOccurrence;
		public event EventHandler OnLabelSelected;
		private List<ListViewItem> _listItems = new List<ListViewItem>();

		private class LabelComparer : IComparer
		{
			private int _columnIndex;
			private bool _sortOrder;
			public LabelComparer(int columnIndex, bool sortOrder)
			{
				_columnIndex = columnIndex;
				_sortOrder = sortOrder;
			}

			public int Compare(object x, object y)
			{
				int result = String.Compare(((ListViewItem)x).SubItems[_columnIndex].Text, ((ListViewItem)y).SubItems[_columnIndex].Text);
				if(result == 0 && (_columnIndex == 0 || _columnIndex == 3)) {
					result = String.Compare(((ListViewItem)x).SubItems[2].Text, ((ListViewItem)y).SubItems[2].Text);
				}

				return result * (_sortOrder ? -1 : 1);
			}
		}

		public ctrlLabelList()
		{
			InitializeComponent();
			lstLabels.ListViewItemSorter = new LabelComparer(0, false);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(!IsDesignMode) {
				mnuShowComments.Checked = ConfigManager.Config.DebugInfo.ShowCommentsInLabelList;
				InitShortcuts();
			}
		}

		private void InitShortcuts()
		{
			mnuAdd.InitShortcut(this, nameof(DebuggerShortcutsConfig.LabelList_Add));
			mnuEdit.InitShortcut(this, nameof(DebuggerShortcutsConfig.LabelList_Edit));
			mnuDelete.InitShortcut(this, nameof(DebuggerShortcutsConfig.LabelList_Delete));

			mnuAddToWatch.InitShortcut(this, nameof(DebuggerShortcutsConfig.LabelList_AddToWatch));
			mnuAddBreakpoint.InitShortcut(this, nameof(DebuggerShortcutsConfig.LabelList_AddBreakpoint));
			mnuFindOccurrences.InitShortcut(this, nameof(DebuggerShortcutsConfig.LabelList_FindOccurrences));
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
						item.SubItems[1].Text = "$" + relativeAddress.ToString("X4");
						item.ForeColor = Color.Black;
						item.Font = new Font(item.Font, FontStyle.Regular);
					} else {
						item.SubItems[1].Text = "[n/a]";
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
			List<CodeLabel> labels = LabelManager.GetLabels();
			List<ListViewItem> items = new List<ListViewItem>(labels.Count);
			foreach(CodeLabel label in labels) {
				if(label.Label.Length > 0 || ConfigManager.Config.DebugInfo.ShowCommentsInLabelList) {
					ListViewItem item = new ListViewItem(label.Label);

					Int32 relativeAddress = label.GetRelativeAddress();
					if(relativeAddress >= 0) {
						item.SubItems.Add("$" + relativeAddress.ToString("X4"));
					} else {
						item.SubItems.Add("[n/a]");
						item.ForeColor = Color.Gray;
						item.Font = new Font(item.Font, FontStyle.Italic);
					}
					string absAddress = string.Empty;
					switch(label.AddressType) {
						case AddressType.InternalRam: absAddress += "RAM: "; break;
						case AddressType.PrgRom: absAddress += "PRG: "; break;
						case AddressType.Register: absAddress += "REG: "; break;
						case AddressType.SaveRam: absAddress += "SRAM: "; break;
						case AddressType.WorkRam: absAddress += "WRAM: "; break;
					}
					absAddress += "$" + label.Address.ToString("X4");
					item.SubItems.Add(absAddress);
					item.SubItems.Add(ConfigManager.Config.DebugInfo.ShowCommentsInLabelList ? label.Comment : "");
					item.SubItems[1].Tag = label;

					item.Tag = relativeAddress;
					items.Add(item);
				}
			}

			lstLabels.BeginUpdate();
			lstLabels.Items.Clear();
			lstLabels.Items.AddRange(items.ToArray());
			lstLabels.Sort();

			colComment.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			if(!ConfigManager.Config.DebugInfo.ShowCommentsInLabelList) {
				colComment.Width = 0;
			}

			lstLabels.EndUpdate();

			_listItems = items;
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
		
		private void lstLabels_SelectedIndexChanged(object sender, EventArgs e)
		{
			mnuDelete.Enabled = lstLabels.SelectedItems.Count > 0;
			mnuEdit.Enabled = lstLabels.SelectedItems.Count == 1;
			mnuFindOccurrences.Enabled = lstLabels.SelectedItems.Count == 1;
			mnuAddToWatch.Enabled = lstLabels.SelectedItems.Count == 1;
			mnuAddBreakpoint.Enabled = lstLabels.SelectedItems.Count == 1;
		}

		private void mnuDelete_Click(object sender, EventArgs e)
		{
			if(lstLabels.SelectedItems.Count > 0) {
				int topIndex = lstLabels.TopItem.Index;
				int lastSelectedIndex = lstLabels.SelectedIndices[lstLabels.SelectedIndices.Count - 1];
				for(int i = lstLabels.SelectedItems.Count - 1; i >= 0; i--) {
					CodeLabel label = (CodeLabel)lstLabels.SelectedItems[i].SubItems[1].Tag;
					LabelManager.DeleteLabel(label.Address, label.AddressType, i == 0);
				}
				
				//Reposition scroll bar and selected/focused item
				if(lstLabels.Items.Count > topIndex) {
					lstLabels.TopItem = lstLabels.Items[topIndex];
				}
				if(lastSelectedIndex < lstLabels.Items.Count) {
					lstLabels.Items[lastSelectedIndex].Selected = true;
				} else if(lstLabels.Items.Count > 0) {
					lstLabels.Items[lstLabels.Items.Count - 1].Selected = true;
				}
				if(lstLabels.SelectedItems.Count > 0) {
					lstLabels.SelectedItems[0].Focused = true;
				}
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
			if(lstLabels.SelectedItems.Count > 0) {
				CodeLabel label = (CodeLabel)lstLabels.SelectedItems[0].SubItems[1].Tag;
				EditLabel(label.Address, label.AddressType);
			}
		}

		private void mnuFindOccurrences_Click(object sender, EventArgs e)
		{
			OnFindOccurrence?.Invoke(lstLabels.SelectedItems[0].SubItems[1].Tag, null);
		}

		int _prevSortColumn = 0;
		bool _descSort = false;
		private void lstLabels_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if(_prevSortColumn == e.Column) {
				_descSort = !_descSort;
			}
			lstLabels.ListViewItemSorter = new LabelComparer(e.Column, _descSort);
			_prevSortColumn = e.Column;
		}

		private void mnuAddBreakpoint_Click(object sender, EventArgs e)
		{
			if(lstLabels.SelectedItems.Count > 0) {
				CodeLabel label = (CodeLabel)lstLabels.SelectedItems[0].SubItems[1].Tag;
				if(label.AddressType == AddressType.InternalRam || label.AddressType == AddressType.Register) {
					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType(label.Address, ref info);
					if(BreakpointManager.GetMatchingBreakpoint((Int32)label.Address, info) == null) {
						BreakpointManager.AddBreakpoint(new Breakpoint() {
							MemoryType = DebugMemoryType.CpuMemory,
							BreakOnExec = true,
							BreakOnRead = true,
							BreakOnWrite = true,
							Address = label.Address,
							StartAddress = label.Address,
							EndAddress = label.Address,
							AddressType = BreakpointAddressType.SingleAddress
						});
					}
				} else {
					BreakpointManager.AddBreakpoint(new Breakpoint() {
						MemoryType = DebugMemoryType.PrgRom,
						BreakOnExec = true,
						BreakOnRead = true,
						BreakOnWrite = false,
						Address = label.Address,
						StartAddress = label.Address,
						EndAddress = label.Address,
						AddressType = BreakpointAddressType.SingleAddress
					});
				}
			}
		}

		private void mnuAddToWatch_Click(object sender, EventArgs e)
		{
			if(lstLabels.SelectedItems.Count > 0) {
				CodeLabel label = (CodeLabel)lstLabels.SelectedItems[0].SubItems[1].Tag;
				WatchManager.AddWatch("[" + label.Label + "]");
			}			
		}

		private void mnuShowComments_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowCommentsInLabelList = mnuShowComments.Checked;
			ConfigManager.ApplyChanges();
			this.UpdateLabelList();
		}
	}
}
