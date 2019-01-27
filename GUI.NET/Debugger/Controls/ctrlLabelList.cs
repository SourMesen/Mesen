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
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlLabelList : BaseControl
	{
		public event EventHandler OnFindOccurrence;
		public event GoToDestinationEventHandler OnLabelSelected;

		private List<ListViewItem> _listItems = new List<ListViewItem>();
		private int _sortColumn = 0;
		private bool _descSort = false;

		public ctrlLabelList()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(!IsDesignMode) {
				mnuShowComments.Checked = ConfigManager.Config.DebugInfo.ShowCommentsInLabelList;
				mnuShowJumpLabels.Checked = ConfigManager.Config.DebugInfo.ShowJumpLabels;
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

			mnuViewInCpuMemory.InitShortcut(this, nameof(DebuggerShortcutsConfig.LabelList_ViewInCpuMemory));
			mnuViewInMemoryType.InitShortcut(this, nameof(DebuggerShortcutsConfig.LabelList_ViewInMemoryType));
		}

		public static void EditLabel(UInt32 address, AddressType type)
		{
			CodeLabel existingLabel = LabelManager.GetLabel(address, type);
			CodeLabel newLabel = new CodeLabel() { Address = address, AddressType = type, Label = existingLabel?.Label, Comment = existingLabel?.Comment, Length = existingLabel?.Length ?? 1 };

			frmEditLabel frm = new frmEditLabel(newLabel, existingLabel);
			if(frm.ShowDialog() == DialogResult.OK) {
				bool empty = string.IsNullOrWhiteSpace(newLabel.Label) && string.IsNullOrWhiteSpace(newLabel.Comment);
				if(existingLabel != null) {
					LabelManager.DeleteLabel(existingLabel, empty);
				}
				if(!empty) {
					LabelManager.SetLabel(newLabel.Address, newLabel.AddressType, newLabel.Label, newLabel.Comment, true, CodeLabelFlags.None, newLabel.Length);
				}
			}
		}

		public int CompareLabels(ListViewItem x, ListViewItem y)
		{
			int result = String.Compare(((ListViewItem)x).SubItems[_sortColumn].Text, ((ListViewItem)y).SubItems[_sortColumn].Text);
			if(result == 0 && (_sortColumn == 0 || _sortColumn == 3)) {
				result = String.Compare(((ListViewItem)x).SubItems[2].Text, ((ListViewItem)y).SubItems[2].Text);
			}
			return result * (_descSort ? -1 : 1);
		}

		private void SortItems()
		{
			_listItems.Sort(CompareLabels);
		}

		public void UpdateLabelListAddresses()
		{
			bool needUpdate = false;
			Font italicFont = null;
			Font regularFont = null;

			foreach(ListViewItem item in _listItems) {
				CodeLabel label = (CodeLabel)item.SubItems[1].Tag;

				Int32 relativeAddress = InteropEmu.DebugGetRelativeAddress(label.Address, label.AddressType);
				if(relativeAddress != (Int32)item.Tag) {
					needUpdate = true;
					if(relativeAddress >= 0) {
						item.SubItems[1].Text = "$" + relativeAddress.ToString("X4");
						item.ForeColor = ThemeHelper.Theme.LabelForeColor;
						if(regularFont == null) {
							regularFont = new Font(item.Font, FontStyle.Regular);
						}
						item.Font = regularFont;
					} else {
						item.SubItems[1].Text = "[n/a]";
						item.ForeColor = ThemeHelper.Theme.LabelDisabledForeColor;
						if(italicFont == null) {
							italicFont = new Font(item.Font, FontStyle.Italic);
						}
						item.Font = italicFont;
					}
					item.Tag = relativeAddress;
				}
			}
			if(needUpdate) {
				SortItems();
			}
		}

		public void UpdateLabelList()
		{
			List<CodeLabel> labels = LabelManager.GetLabels();
			List<ListViewItem> items = new List<ListViewItem>(labels.Count);
			Font italicFont = null;
			foreach(CodeLabel label in labels) {
				if((label.Label.Length > 0 || ConfigManager.Config.DebugInfo.ShowCommentsInLabelList) && (!label.Flags.HasFlag(CodeLabelFlags.AutoJumpLabel) || ConfigManager.Config.DebugInfo.ShowJumpLabels)) {
					ListViewItem item = new ListViewItem(label.Label);

					Int32 relativeAddress = label.GetRelativeAddress();
					if(relativeAddress >= 0) {
						item.SubItems.Add("$" + relativeAddress.ToString("X4"));
					} else {
						item.SubItems.Add("[n/a]");
						item.ForeColor = ThemeHelper.Theme.LabelDisabledForeColor;
						if(italicFont == null) {
							italicFont = new Font(item.Font, FontStyle.Italic);
						}
						item.Font = italicFont;
					}
					string prefix = string.Empty;
					switch(label.AddressType) {
						case AddressType.InternalRam: prefix = "RAM: $"; break;
						case AddressType.PrgRom: prefix = "PRG: $"; break;
						case AddressType.Register: prefix = "REG: $"; break;
						case AddressType.SaveRam: prefix = "SRAM: $"; break;
						case AddressType.WorkRam: prefix = "WRAM: $"; break;
					}
					item.SubItems.Add(prefix + label.Address.ToString("X4"));
					item.SubItems.Add(ConfigManager.Config.DebugInfo.ShowCommentsInLabelList ? label.Comment : "");
					item.SubItems[1].Tag = label;

					item.Tag = relativeAddress;
					items.Add(item);
				}
			}

			_listItems = items;
			SortItems();

			lstLabels.BeginUpdate();
			lstLabels.VirtualMode = true;
			lstLabels.VirtualListSize = items.Count;
			lstLabels.EndUpdate();

			colComment.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			if(!ConfigManager.Config.DebugInfo.ShowCommentsInLabelList) {
				colComment.Width = 0;
			}
		}

		private ListViewItem GetSelectedItem()
		{
			return _listItems[lstLabels.SelectedIndices[0]];
		}

		private void lstLabels_DoubleClick(object sender, EventArgs e)
		{
			if(lstLabels.SelectedIndices.Count > 0) {
				Int32 relativeAddress = (Int32)GetSelectedItem().Tag;
				CodeLabel label = (CodeLabel)GetSelectedItem().SubItems[1].Tag;
				OnLabelSelected?.Invoke(new GoToDestination() {
					CpuAddress = relativeAddress,
					Label = label
				});
			}
		}
		
		private void lstLabels_SelectedIndexChanged(object sender, EventArgs e)
		{
			mnuDelete.Enabled = lstLabels.SelectedIndices.Count > 0;
			mnuEdit.Enabled = lstLabels.SelectedIndices.Count == 1;
			mnuFindOccurrences.Enabled = lstLabels.SelectedIndices.Count == 1;
			mnuAddToWatch.Enabled = lstLabels.SelectedIndices.Count == 1;
			mnuAddBreakpoint.Enabled = lstLabels.SelectedIndices.Count == 1;

			if(lstLabels.SelectedIndices.Count == 1) {
				ListViewItem item = GetSelectedItem();

				bool availableInCpuMemory = (int)item.Tag >= 0;
				mnuViewInCpuMemory.Enabled = availableInCpuMemory;

				CodeLabel label = (CodeLabel)item.SubItems[1].Tag;
				if(label.AddressType != AddressType.Register && label.AddressType != AddressType.InternalRam) {
					mnuViewInMemoryType.Text = "View in " + ResourceHelper.GetEnumText(label.AddressType);
					mnuViewInMemoryType.Visible = true;
				} else {
					mnuViewInMemoryType.Visible = false;
				}
			} else {
				mnuViewInCpuMemory.Enabled = false;
				mnuViewInMemoryType.Visible = false;
			}
		}

		private void mnuDelete_Click(object sender, EventArgs e)
		{
			if(lstLabels.SelectedIndices.Count > 0) {
				int topIndex = lstLabels.TopItem.Index;
				int lastSelectedIndex = lstLabels.SelectedIndices[lstLabels.SelectedIndices.Count - 1];
				List<int> selectedIndexes = new List<int>(lstLabels.SelectedIndices.Cast<int>().ToList());
				for(int i = selectedIndexes.Count - 1; i >= 0; i--) {
					CodeLabel label = (CodeLabel)_listItems[selectedIndexes[i]].SubItems[1].Tag;
					LabelManager.DeleteLabel(label, i == 0);
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
				if(lstLabels.SelectedIndices.Count > 0) {
					GetSelectedItem().Focused = true;
				}
			}
		}

		private void mnuAdd_Click(object sender, EventArgs e)
		{
			CodeLabel newLabel = new CodeLabel() { Address = 0, AddressType = AddressType.InternalRam, Label = "", Comment = "" };

			frmEditLabel frm = new frmEditLabel(newLabel);
			if(frm.ShowDialog() == DialogResult.OK) {
				LabelManager.SetLabel(newLabel.Address, newLabel.AddressType, newLabel.Label, newLabel.Comment, true, CodeLabelFlags.None, newLabel.Length);
			}
		}

		private void mnuEdit_Click(object sender, EventArgs e)
		{
			if(lstLabels.SelectedIndices.Count > 0) {
				CodeLabel label = (CodeLabel)GetSelectedItem().SubItems[1].Tag;
				EditLabel(label.Address, label.AddressType);
			}
		}

		private void mnuFindOccurrences_Click(object sender, EventArgs e)
		{
			OnFindOccurrence?.Invoke(GetSelectedItem().SubItems[1].Tag, null);
		}

		private void lstLabels_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			lstLabels.BeginUpdate();
			if(_sortColumn == e.Column) {
				_descSort = !_descSort;
			}
			_sortColumn = e.Column;
			SortItems();
			lstLabels.EndUpdate();
		}

		private void mnuAddBreakpoint_Click(object sender, EventArgs e)
		{
			if(lstLabels.SelectedIndices.Count > 0) {
				CodeLabel label = (CodeLabel)GetSelectedItem().SubItems[1].Tag;
				if(label.AddressType == AddressType.InternalRam || label.AddressType == AddressType.Register) {
					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType(label.Address, info);
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
			if(lstLabels.SelectedIndices.Count > 0) {
				CodeLabel label = (CodeLabel)GetSelectedItem().SubItems[1].Tag;
				WatchManager.AddWatch("[" + label.Label + "]");
			}			
		}

		private void mnuShowComments_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowCommentsInLabelList = mnuShowComments.Checked;
			ConfigManager.ApplyChanges();
			this.UpdateLabelList();
		}
		
		private void mnuShowJumpLabels_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowJumpLabels = mnuShowJumpLabels.Checked;
			ConfigManager.ApplyChanges();
			this.UpdateLabelList();
		}

		private void lstLabels_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			e.Item = _listItems[e.ItemIndex];
		}

		private void lstLabels_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
		{
			for(int i = 0; i < _listItems.Count; i++) {
				if(_listItems[i].Text.StartsWith(e.Text, StringComparison.InvariantCultureIgnoreCase)) {
					e.Index = i;
					return;
				}
			}
		}

		private void mnuViewInCpuMemory_Click(object sender, EventArgs e)
		{
			if(lstLabels.SelectedIndices.Count == 1) {
				ListViewItem item = GetSelectedItem();
				int address = (int)item.Tag;
				if(address >= 0) {
					DebugWindowManager.OpenMemoryViewer(address, DebugMemoryType.CpuMemory);
				}
			}
		}

		private void mnuViewInMemoryType_Click(object sender, EventArgs e)
		{
			if(lstLabels.SelectedIndices.Count == 1) {
				ListViewItem item = GetSelectedItem();
				CodeLabel label = (CodeLabel)item.SubItems[1].Tag;
				if(label.AddressType != AddressType.Register && label.AddressType != AddressType.InternalRam) {
					DebugWindowManager.OpenMemoryViewer((int)label.Address, label.AddressType.ToMemoryType());
				}
			}
		}
	}
}
