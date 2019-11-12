﻿using System;
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
		private Font _markedColumnFont;

		public ctrlBreakpoints()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(!IsDesignMode) {
				_markedColumnFont = new Font(this.Font.FontFamily, 13f);

				mnuShowLabels.Checked = ConfigManager.Config.DebugInfo.ShowBreakpointLabels;
				mnuShowLabels.CheckedChanged += mnuShowLabels_CheckedChanged;

				BreakpointManager.BreakpointsChanged += BreakpointManager_OnBreakpointChanged;
				mnuRemoveBreakpoint.Enabled = false;
				mnuEditBreakpoint.Enabled = false;
				mnuGoToLocation.Enabled = false;

				InitShortcuts();
			}
		}

		private void InitShortcuts()
		{
			mnuAddBreakpoint.InitShortcut(this, nameof(DebuggerShortcutsConfig.BreakpointList_Add));
			mnuEditBreakpoint.InitShortcut(this, nameof(DebuggerShortcutsConfig.BreakpointList_Edit));
			mnuRemoveBreakpoint.InitShortcut(this, nameof(DebuggerShortcutsConfig.BreakpointList_Delete));
			mnuGoToLocation.InitShortcut(this, nameof(DebuggerShortcutsConfig.BreakpointList_GoToLocation));
		}

		private void BreakpointManager_OnBreakpointChanged(object sender, EventArgs e)
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((Action)(() => RefreshList()));
			} else {
				RefreshList();
			}
		}

		public void RefreshListAddresses()
		{
			lstBreakpoints.BeginUpdate();
			ReadOnlyCollection<Breakpoint> breakpoints = BreakpointManager.Breakpoints;
			for(int i = 0; i < breakpoints.Count; i++) {
				lstBreakpoints.Items[i].SubItems[3].Text = breakpoints[i].GetAddressString(mnuShowLabels.Checked);
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
				item.UseItemStyleForSubItems = false;
				item.SubItems.Add(breakpoint.MarkEvent ? "☑" : "☐").Font = _markedColumnFont;
				item.SubItems.Add(breakpoint.ToReadableType());
				item.SubItems.Add(breakpoint.GetAddressString(mnuShowLabels.Checked));
				item.SubItems.Add(breakpoint.Condition);
				lstBreakpoints.Items.Add(item);
			}
			lstBreakpoints.EndUpdate();
			if(lstBreakpoints.Items.Count > 0) {
				lstBreakpoints.TopItem = lstBreakpoints.Items[lstBreakpoints.Items.Count > topIndex ? topIndex : lstBreakpoints.Items.Count - 1];
			}

			lstBreakpoints.ItemChecked += new ItemCheckedEventHandler(lstBreakpoints_ItemChecked);
			UpdateContextMenu();
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
				Breakpoint bp = lstBreakpoints.SelectedItems[0].Tag as Breakpoint;
				if(bp.IsCpuBreakpoint && bp.GetRelativeAddress() >= 0) {
					BreakpointNavigation(bp, null);
				}
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
			UpdateContextMenu();
		}

		private void UpdateContextMenu()
		{
			mnuRemoveBreakpoint.Enabled = (lstBreakpoints.SelectedItems.Count > 0);
			mnuEditBreakpoint.Enabled = (lstBreakpoints.SelectedItems.Count == 1);
			if(lstBreakpoints.SelectedItems.Count == 1) {
				Breakpoint bp = lstBreakpoints.SelectedItems[0].Tag as Breakpoint;
				mnuGoToLocation.Enabled = bp.IsCpuBreakpoint && bp.GetRelativeAddress() >= 0;
			} else {
				mnuGoToLocation.Enabled = false;
			}
		}

		private void mnuShowLabels_CheckedChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowBreakpointLabels = mnuShowLabels.Checked;
			ConfigManager.ApplyChanges();

			this.RefreshListAddresses();
		}

		private void lstBreakpoints_MouseDown(object sender, MouseEventArgs e)
		{
			ListViewHitTestInfo info = lstBreakpoints.HitTest(e.X, e.Y);
			if(info != null && info.Item != null) {
				int row = info.Item.Index;
				int col = info.Item.SubItems.IndexOf(info.SubItem);

				if(col == 1 && row < lstBreakpoints.Items.Count) {
					this.BeginInvoke((Action)(() => {
						Breakpoint bp = lstBreakpoints.Items[row].Tag as Breakpoint;
						bp?.SetMarked(!bp.MarkEvent);
					}));
				}
			}
		}
	}
}
