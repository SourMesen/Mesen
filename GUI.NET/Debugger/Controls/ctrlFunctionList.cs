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
	public partial class ctrlFunctionList : BaseControl
	{
		public event EventHandler OnFindOccurrence;
		public event EventHandler OnFunctionSelected;

		public ctrlFunctionList()
		{
			InitializeComponent();

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				this.InitShortcuts();
			}
		}

		private void InitShortcuts()
		{
			mnuEditLabel.InitShortcut(this, nameof(DebuggerShortcutsConfig.FunctionList_EditLabel));
			mnuAddBreakpoint.InitShortcut(this, nameof(DebuggerShortcutsConfig.FunctionList_AddBreakpoint));
			mnuFindOccurrences.InitShortcut(this, nameof(DebuggerShortcutsConfig.FunctionList_FindOccurrences));
		}

		private class FunctionComparer : IComparer
		{
			int IComparer.Compare(object x, object y)
			{
				ListViewItem a = x as ListViewItem;
				ListViewItem b = y as ListViewItem;

				string aText = string.IsNullOrWhiteSpace(a.Text) ? "ZZZZZZZZZZZZZZZZZZZZZZZ" : a.Text;
				string bText = string.IsNullOrWhiteSpace(b.Text) ? "ZZZZZZZZZZZZZZZZZZZZZZZ" : b.Text;
				Int32 aRelative = (Int32)a.SubItems[1].Tag == -1 ? Int32.MaxValue : (Int32)a.SubItems[1].Tag;
				Int32 bRelative = (Int32)b.SubItems[1].Tag == -1 ? Int32.MaxValue : (Int32)b.SubItems[1].Tag;
				Int32 aAbsolute = (Int32)a.SubItems[2].Tag;
				Int32 bAbsolute = (Int32)b.SubItems[2].Tag;

				if(a.Text == b.Text) {
					if(a.Tag == b.Tag) {
						return aAbsolute > bAbsolute ? 1 : -1;
					} else {
						return aRelative > bRelative ? 1 : -1;
					}
				} else {
					return string.Compare(aText, bText);
				}
			}
		}

		private Dictionary<Int32, ListViewItem> _functions = new Dictionary<int, ListViewItem>(); 
		public void UpdateFunctionList(bool reset)
		{
			if(reset) {
				lstFunctions.Items.Clear();
				_functions.Clear();
			}

			Int32[] entryPoints = InteropEmu.DebugGetFunctionEntryPoints();
			bool updating = false;

			for(int i = 0; i < entryPoints.Length && entryPoints[i] >= 0; i++) {
				Int32 entryPoint = entryPoints[i];
				ListViewItem item;
				if(!_functions.TryGetValue(entryPoint, out item)) {
					if(!updating) {
						updating = true;
						lstFunctions.BeginUpdate();
						lstFunctions.ListViewItemSorter = null;
					}

					CodeLabel label = LabelManager.GetLabel((UInt32)entryPoint, AddressType.PrgRom);
					item = lstFunctions.Items.Add(label?.Label);
					item.Tag = label;

					item.SubItems.Add("[n/a]");
					item.SubItems[1].Tag = -1;
					item.ForeColor = Color.Gray;
					item.Font = new Font(item.Font, FontStyle.Italic);

					item.SubItems.Add("$" + entryPoint.ToString("X4"));
					item.SubItems[2].Tag = entryPoint;

					_functions[entryPoint] = item;
				}

				Int32 relativeAddress = InteropEmu.DebugGetRelativeAddress((UInt32)entryPoint, AddressType.PrgRom);
				if(relativeAddress != (Int32)item.SubItems[1].Tag) {
					if(!updating) {
						updating = true;
						lstFunctions.BeginUpdate();
						lstFunctions.ListViewItemSorter = null;
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
					item.SubItems[1].Tag = relativeAddress;
				}
			}

			if(updating) {
				lstFunctions.ListViewItemSorter = new FunctionComparer();
				lstFunctions.Sort();
				lstFunctions.EndUpdate();
			}
		}

		private void lstFunctions_DoubleClick(object sender, EventArgs e)
		{
			if(lstFunctions.SelectedItems.Count > 0) {
				Int32 relativeAddress = (Int32)lstFunctions.SelectedItems[0].SubItems[1].Tag;

				if(relativeAddress >= 0) {
					OnFunctionSelected?.Invoke(relativeAddress, e);
				}
			}
		}

		private void mnuEditLabel_Click(object sender, EventArgs e)
		{
			if(lstFunctions.SelectedItems.Count > 0) {
				CodeLabel label = lstFunctions.SelectedItems[0].Tag as CodeLabel;
				if(label != null) {
					ctrlLabelList.EditLabel(label.Address, label.AddressType);
				} else {
					ctrlLabelList.EditLabel((UInt32)(Int32)lstFunctions.SelectedItems[0].SubItems[2].Tag, AddressType.PrgRom);
				}
			}
		}

		private void mnuFindOccurrences_Click(object sender, EventArgs e)
		{
			if(lstFunctions.SelectedItems.Count > 0) {
				int relativeAddress = (int)lstFunctions.SelectedItems[0].SubItems[1].Tag;
				if(relativeAddress >= 0) {
					CodeLabel label = lstFunctions.SelectedItems[0].Tag as CodeLabel;
					if(label != null) {
						OnFindOccurrence?.Invoke(label.Label, null);
					} else {
						OnFindOccurrence?.Invoke("$" + ((int)lstFunctions.SelectedItems[0].SubItems[1].Tag).ToString("X4"), null);
					}
				}
			}
		}
		
		private void lstFunctions_SelectedIndexChanged(object sender, EventArgs e)
		{
			mnuEditLabel.Enabled = lstFunctions.SelectedItems.Count == 1;
			mnuFindOccurrences.Enabled = lstFunctions.SelectedItems.Count == 1;
			if(lstFunctions.SelectedItems.Count == 1) {
				int relativeAddress = (int)lstFunctions.SelectedItems[0].SubItems[1].Tag;
				if(relativeAddress < 0) {
					mnuFindOccurrences.Enabled = false;
				}
			}
		}

		private void mnuAddBreakpoint_Click(object sender, EventArgs e)
		{
			if(lstFunctions.SelectedItems.Count > 0) {
				CodeLabel label = lstFunctions.SelectedItems[0].Tag as CodeLabel;
				int absoluteAddress = (int)lstFunctions.SelectedItems[0].SubItems[2].Tag;
				BreakpointManager.AddBreakpoint(new Breakpoint() {
					MemoryType = DebugMemoryType.PrgRom,
					BreakOnExec = true,
					BreakOnRead = false,
					BreakOnWrite = false,
					Address = (UInt32)absoluteAddress,
					StartAddress = (UInt32)absoluteAddress,
					EndAddress = (UInt32)absoluteAddress,
					AddressType = BreakpointAddressType.SingleAddress
				});
			}
		}
	}
}
