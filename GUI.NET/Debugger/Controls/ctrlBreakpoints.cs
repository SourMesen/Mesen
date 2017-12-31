using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;
using System.Collections.ObjectModel;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlBreakpoints : BaseControl
	{
		public event EventHandler BreakpointNavigation;

		public ctrlBreakpoints()
		{
			InitializeComponent();

			mnuShowLabels.Checked = ConfigManager.Config.DebugInfo.ShowBreakpointLabels;
			mnuShowLabels.CheckedChanged += mnuShowLabels_CheckedChanged;

			BreakpointManager.BreakpointsChanged += BreakpointManager_OnBreakpointChanged;
			mnuRemoveBreakpoint.Enabled = false;
			mnuEditBreakpoint.Enabled = false;
			mnuGoToLocation.Enabled = false;
		}

		void BreakpointManager_OnBreakpointChanged(object sender, EventArgs e)
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((Action)(() => RefreshList()));
			} else {
				RefreshList();
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			AdjustColumnWidth();
		}

		public void RefreshListAddresses()
		{
			lstBreakpoints.BeginUpdate();
			ReadOnlyCollection<Breakpoint> breakpoints = BreakpointManager.Breakpoints;
			for(int i = 0; i < breakpoints.Count; i++) {
				lstBreakpoints.Items[i].SubItems[2].Text = breakpoints[i].GetAddressString(mnuShowLabels.Checked);
			}
			lstBreakpoints.EndUpdate();
		}

		public void RefreshList()
		{
			lstBreakpoints.ItemChecked -= new ItemCheckedEventHandler(lstBreakpoints_ItemChecked);

			int topIndex = lstBreakpoints.TopItem != null ? lstBreakpoints.TopItem.Index : 0;
			lstBreakpoints.BeginUpdate();
			lstBreakpoints.Items.Clear();
			foreach(Breakpoint breakpoint in BreakpointManager.Breakpoints) {
				ListViewItem item = new ListViewItem();
				item.Tag = breakpoint;
				item.Checked = breakpoint.Enabled;
				item.SubItems.Add(breakpoint.Type.ToString());
				item.SubItems.Add(breakpoint.GetAddressString(mnuShowLabels.Checked));
				item.SubItems.Add(breakpoint.Condition);
				lstBreakpoints.Items.Add(item);
			}
			lstBreakpoints.EndUpdate();
			if(lstBreakpoints.Items.Count > 0) {
				lstBreakpoints.TopItem = lstBreakpoints.Items[lstBreakpoints.Items.Count > topIndex ? topIndex : lstBreakpoints.Items.Count - 1];
			}

			lstBreakpoints.ItemChecked += new ItemCheckedEventHandler(lstBreakpoints_ItemChecked);
		}

		private void lstBreakpoints_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			if(e.ColumnIndex == 2) {
				e.Cancel = true;
			}
			AdjustColumnWidth();
		}

		private void lstBreakpoints_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			AdjustColumnWidth();
		}

		private void AdjustColumnWidth()
		{
			lstBreakpoints.ColumnWidthChanging -= lstBreakpoints_ColumnWidthChanging;
			lstBreakpoints.ColumnWidthChanged -= lstBreakpoints_ColumnWidthChanged;

			//Force watch values to take the full width of the list
			int totalWidth = lstBreakpoints.Columns[0].Width + lstBreakpoints.Columns[1].Width + lstBreakpoints.Columns[2].Width + lstBreakpoints.Columns[3].Width;
			colLastColumn.Width = lstBreakpoints.ClientSize.Width - totalWidth;

			lstBreakpoints.ColumnWidthChanging += lstBreakpoints_ColumnWidthChanging;
			lstBreakpoints.ColumnWidthChanged += lstBreakpoints_ColumnWidthChanged;
		}

		private void lstBreakpoints_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if(((Breakpoint)e.Item.Tag).Enabled != e.Item.Checked) {
				((Breakpoint)e.Item.Tag).SetEnabled(e.Item.Checked);
			}
		}

		private void lstBreakpoints_DoubleClick(object sender, EventArgs e)
		{
			if(lstBreakpoints.SelectedItems.Count > 0) {
				BreakpointManager.EditBreakpoint(((Breakpoint)lstBreakpoints.SelectedItems[0].Tag));
			}
		}

		private void mnuRemoveBreakpoint_Click(object sender, EventArgs e)
		{
			foreach(ListViewItem item in lstBreakpoints.SelectedItems) {
				BreakpointManager.RemoveBreakpoint((Breakpoint)item.Tag);
			}
		}

		private void mnuAddBreakpoint_Click(object sender, EventArgs e)
		{
			Breakpoint breakpoint = new Breakpoint();
			if(new frmBreakpoint(breakpoint).ShowDialog() == DialogResult.OK) {
				BreakpointManager.AddBreakpoint(breakpoint);
			}
		}

		private void mnuGoToLocation_Click(object sender, EventArgs e)
		{
			if(BreakpointNavigation != null) {
				BreakpointNavigation(lstBreakpoints.SelectedItems[0].Tag, null);
			}
		}

		private void mnuEditBreakpoint_Click(object sender, EventArgs e)
		{
			if(lstBreakpoints.SelectedItems.Count > 0) {
				BreakpointManager.EditBreakpoint(((Breakpoint)lstBreakpoints.SelectedItems[0].Tag));
			}
		}

		private void lstBreakpoints_SelectedIndexChanged(object sender, EventArgs e)
		{
			mnuRemoveBreakpoint.Enabled = (lstBreakpoints.SelectedItems.Count > 0);
			mnuEditBreakpoint.Enabled = (lstBreakpoints.SelectedItems.Count == 1);
			mnuGoToLocation.Enabled = (lstBreakpoints.SelectedItems.Count == 1 && ((Breakpoint)lstBreakpoints.SelectedItems[0].Tag).IsCpuBreakpoint);
		}
		
		private void mnuShowLabels_CheckedChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowBreakpointLabels = mnuShowLabels.Checked;
			ConfigManager.ApplyChanges();

			this.RefreshListAddresses();
		}
	}
}
