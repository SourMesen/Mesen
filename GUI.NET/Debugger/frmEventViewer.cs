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
		private bool _refreshing = false;

		public frmEventViewer()
		{
			InitializeComponent();

			this.mnuRefreshOnBreak.Checked = ConfigManager.Config.DebugInfo.EventViewerRefreshOnBreak;
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
				this.GetData();
				this.RefreshViewer();

				DebugWorkspaceManager.GetWorkspace();

				if(!ConfigManager.Config.DebugInfo.EventViewerSize.IsEmpty) {
					this.StartPosition = FormStartPosition.Manual;
					this.Size = ConfigManager.Config.DebugInfo.EventViewerSize;
					this.Location = ConfigManager.Config.DebugInfo.EventViewerLocation;
				}

				this._notifListener = new InteropEmu.NotificationListener();
				this._notifListener.OnNotification += this._notifListener_OnNotification;
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			this._notifListener.OnNotification -= this._notifListener_OnNotification;
			ConfigManager.Config.DebugInfo.EventViewerSize = this.WindowState == FormWindowState.Maximized ? this.RestoreBounds.Size : this.Size;
			ConfigManager.Config.DebugInfo.EventViewerLocation = this.WindowState == FormWindowState.Maximized ? this.RestoreBounds.Location : this.Location;
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.CodeBreak:
				case InteropEmu.ConsoleNotificationType.GamePaused:
					if(ConfigManager.Config.DebugInfo.EventViewerRefreshOnBreak) {
						this.GetData();
						this.BeginInvoke((MethodInvoker)(() => this.RefreshViewer()));
					}
					break;

				case InteropEmu.ConsoleNotificationType.EventViewerDisplayFrame:
					if(!_refreshing && (DateTime.Now - _lastUpdate).Milliseconds >= 32) {
						//Update at ~30 fps at most
						this.GetData();
						this.BeginInvoke((MethodInvoker)(() => this.RefreshViewer()));
						_lastUpdate = DateTime.Now;
					}
					break;
			}
		}

		private void GetData()
		{
			ctrlEventViewerPpuView.GetData();
		}

		private void RefreshViewer()
		{
			_refreshing = true;
			ctrlEventViewerPpuView.RefreshViewer();
			_refreshing = false;
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void mnuRefreshOnBreak_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerRefreshOnBreak = this.mnuRefreshOnBreak.Checked;
			ConfigManager.ApplyChanges();
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
			if(frmEventViewerColors.Instance != null) {
				frmEventViewerColors.Instance.BringToFront();
			} else {
				frmEventViewerColors frm = new frmEventViewerColors();
				frm.Show(this, this);
				frm.FormClosed += (s, evt) => {
					this.GetData();
					this.RefreshViewer();
				};
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
