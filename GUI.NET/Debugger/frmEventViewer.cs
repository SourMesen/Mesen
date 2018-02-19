using Mesen.GUI.Config;
using Mesen.GUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmEventViewer : BaseForm
	{
		private DateTime _lastUpdate = DateTime.MinValue;
		private InteropEmu.NotificationListener _notifListener;
		private TabPage _selectedTab;
		private bool _refreshing = false;

		public frmEventViewer()
		{
			InitializeComponent();

			this._selectedTab = this.tpgPpuView;
			this.chkShowPpuRegisterWrites.Checked = ConfigManager.Config.DebugInfo.EventViewerShowPpuRegisterWrites;
			this.chkShowPpuRegisterReads.Checked = ConfigManager.Config.DebugInfo.EventViewerShowPpuRegisterReads;
			this.chkShowIrq.Checked = ConfigManager.Config.DebugInfo.EventViewerShowIrq;
			this.chkShowNmi.Checked = ConfigManager.Config.DebugInfo.EventViewerShowNmi;
			this.chkShowSpriteZero.Checked = ConfigManager.Config.DebugInfo.EventViewerShowSpriteZeroHit;
			this.chkShowMapperRegisterWrites.Checked = ConfigManager.Config.DebugInfo.EventViewerShowMapperRegisterWrites;
			this.chkShowMapperRegisterReads.Checked = ConfigManager.Config.DebugInfo.EventViewerShowMapperRegisterReads;
			this.chkBreakpoints.Checked = ConfigManager.Config.DebugInfo.EventViewerShowMarkedBreakpoints;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!this.DesignMode) {
				this._notifListener = new InteropEmu.NotificationListener();
				this._notifListener.OnNotification += this._notifListener_OnNotification;

				this.GetData();
				this.RefreshViewer();

				if(!ConfigManager.Config.DebugInfo.EventViewerSize.IsEmpty) {
					this.Size = ConfigManager.Config.DebugInfo.EventViewerSize;
				}
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			this._notifListener.OnNotification -= this._notifListener_OnNotification;
			ConfigManager.Config.DebugInfo.EventViewerSize = this.WindowState == FormWindowState.Maximized ? this.RestoreBounds.Size : this.Size;
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.CodeBreak) {
				this.GetData();
				this.BeginInvoke((MethodInvoker)(() => this.RefreshViewer()));
			} else if(e.NotificationType == InteropEmu.ConsoleNotificationType.EventViewerDisplayFrame) {
				if(!_refreshing && (DateTime.Now - _lastUpdate).Milliseconds > 66) {
					//Update at 15 fps most
					this.GetData();
					this.BeginInvoke((MethodInvoker)(() => this.RefreshViewer()));
					_lastUpdate = DateTime.Now;
				}
			}
		}

		private void GetData()
		{
			ctrlEventViewerPpuView.GetData();
		}

		private void RefreshViewer()
		{
			ctrlEventViewerPpuView.RefreshViewer();
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._selectedTab = this.tabMain.SelectedTab;
		}

		private void chkShowPpuRegisterWrites_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowPpuRegisterWrites = this.chkShowPpuRegisterWrites.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkShowPpuRegisterReads_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowPpuRegisterReads = this.chkShowPpuRegisterReads.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkShowIrq_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowIrq = this.chkShowIrq.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkShowNmi_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowNmi = this.chkShowNmi.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkShowSpriteZero_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowSpriteZeroHit = chkShowSpriteZero.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkShowMapperRegisterWrites_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowMapperRegisterWrites = chkShowMapperRegisterWrites.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkShowMapperRegisterReads_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowMapperRegisterReads = chkShowMapperRegisterReads.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkBreakpoints_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowMarkedBreakpoints = chkBreakpoints.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void mnuConfigureColors_Click(object sender, EventArgs e)
		{
			using(frmEventViewerColors frm = new frmEventViewerColors()) {
				if(frm.ShowDialog(this, this) == DialogResult.OK) {
					this.GetData();
					this.RefreshViewer();
				}
			}
		}

		private void ctrlEventViewerPpuView_SizeChanged(object sender, EventArgs e)
		{
			int newWidth = ctrlEventViewerPpuView.Width + 20;
			int widthDiff = newWidth - panel1.Width;
			panel1.Width = newWidth;

			int newHeight = ctrlEventViewerPpuView.Height + 5;
			int heightDiff = newHeight - panel1.Height;
			panel1.Height = newHeight;

			int minWidth = this.MinimumSize.Width + widthDiff;
			int minHeight = this.MinimumSize.Height + heightDiff;
			this.MinimumSize = new Size(minWidth, Math.Min(800, minHeight));
		}
	}
}
