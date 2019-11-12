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
		private bool _inListViewTab = false;
		private bool _refreshing = false;
		private bool _isZoomed = false;
		private bool _isCompact = false;
		private Size _originalSize;
		private Size _previousPictureSize;

		public frmEventViewer()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!this.DesignMode) {
				this.mnuRefreshOnBreak.Checked = ConfigManager.Config.DebugInfo.EventViewerRefreshOnBreak;
				this.chkShowPpuRegisterWrites.Checked = ConfigManager.Config.DebugInfo.EventViewerShowPpuRegisterWrites;
				this.chkShowPpuRegisterReads.Checked = ConfigManager.Config.DebugInfo.EventViewerShowPpuRegisterReads;
				this.chkShowIrq.Checked = ConfigManager.Config.DebugInfo.EventViewerShowIrq;
				this.chkShowNmi.Checked = ConfigManager.Config.DebugInfo.EventViewerShowNmi;
				this.chkShowSpriteZero.Checked = ConfigManager.Config.DebugInfo.EventViewerShowSpriteZeroHit;
				this.chkShowMapperRegisterWrites.Checked = ConfigManager.Config.DebugInfo.EventViewerShowMapperRegisterWrites;
				this.chkShowMapperRegisterReads.Checked = ConfigManager.Config.DebugInfo.EventViewerShowMapperRegisterReads;
				this.chkBreakpoints.Checked = ConfigManager.Config.DebugInfo.EventViewerShowMarkedBreakpoints;
				this.chkShowPreviousFrameEvents.Checked = ConfigManager.Config.DebugInfo.EventViewerShowPreviousFrameEvents;

				string toggleViewTooltip = "Toggle Compact/Normal View";
				if(ConfigManager.Config.DebugInfo.Shortcuts.PpuViewer_ToggleView != Keys.None) {
					toggleViewTooltip += " (" + DebuggerShortcutsConfig.GetShortcutDisplay(ConfigManager.Config.DebugInfo.Shortcuts.PpuViewer_ToggleView) + ")";
				}
				this.toolTip.SetToolTip(this.btnToggleView, toggleViewTooltip);

				string toggleZoomTooltip = "Toggle 2x Zoom";
				if(ConfigManager.Config.DebugInfo.Shortcuts.PpuViewer_ToggleZoom != Keys.None) {
					toggleZoomTooltip += " (" + DebuggerShortcutsConfig.GetShortcutDisplay(ConfigManager.Config.DebugInfo.Shortcuts.PpuViewer_ToggleZoom) + ")";
				}
				this.toolTip.SetToolTip(this.chkToggleZoom, toggleZoomTooltip);

				_previousPictureSize = ctrlEventViewerPpuView.Size;

				this.GetData();
				this.RefreshViewer();

				DebugWorkspaceManager.GetWorkspace();

				RestoreLocation(ConfigManager.Config.DebugInfo.EventViewerLocation);

				this._notifListener = new InteropEmu.NotificationListener(ConfigManager.Config.DebugInfo.DebugConsoleId);
				this._notifListener.OnNotification += this._notifListener_OnNotification;
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			this._notifListener.OnNotification -= this._notifListener_OnNotification;
			ConfigManager.Config.DebugInfo.EventViewerLocation = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Location : this.Location;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == ConfigManager.Config.DebugInfo.Shortcuts.PpuViewer_ToggleZoom) {
				ToggleZoom();
				return true;
			} else if(keyData == ConfigManager.Config.DebugInfo.Shortcuts.PpuViewer_ToggleView) {
				ToggleView();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
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
			if(_inListViewTab) {
				ctrlEventViewerListView.GetData();
			} else {
				ctrlEventViewerPpuView.GetData();
			}
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
			_inListViewTab = tabMain.SelectedTab == tpgListView;
			GetData();
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

		private void chkShowPreviousFrameEvents_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowPreviousFrameEvents = chkShowPreviousFrameEvents.Checked;
			ConfigManager.ApplyChanges();
			if(InteropEmu.DebugIsExecutionStopped()) {
				this.GetData();
				this.RefreshViewer();
			}
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

		private void ctrlEventViewerPpuView_OnPictureResized(object sender, EventArgs e)
		{
			Size picSize = ctrlEventViewerPpuView.GetCompactSize(false);
			this.Size += (picSize - _previousPictureSize);
			_originalSize += (picSize - _previousPictureSize);
			ctrlEventViewerPpuView.Size += (picSize - _previousPictureSize);
			_previousPictureSize = picSize;
		}

		private void ToggleView()
		{
			if(!_isCompact) {
				_isCompact = true;
				_originalSize = this.Size;

				this.ClientSize = ctrlEventViewerPpuView.GetCompactSize(false) + new Size(3, menuStrip1.Height + 3);

				this.Controls.Add(ctrlEventViewerPpuView);
				ctrlEventViewerPpuView.BringToFront();
				ctrlEventViewerPpuView.Dock = DockStyle.Fill;

				tabMain.Visible = false;
			} else {
				_isCompact = false;
				this.Size = _originalSize;
				ctrlEventViewerPpuView.Dock = DockStyle.None;
				ctrlEventViewerPpuView.Size = ctrlEventViewerPpuView.GetCompactSize(false);
				tabMain.Visible = true;
				tpgPpuView.Controls.Add(ctrlEventViewerPpuView);
			}

			btnToggleView.Image = _isCompact ? Properties.Resources.Expand : Properties.Resources.Collapse;
			RefreshViewer();
		}

		private void ToggleZoom()
		{
			ICompactControl ctrl = ctrlEventViewerPpuView;

			if(!_isZoomed) {
				Size pictureSize = ctrl.GetCompactSize(false);
				ctrl.ScaleImage(2);
				_isZoomed = true;
			} else {
				Size pictureSize = ctrl.GetCompactSize(false);
				Size halfSize = new Size(pictureSize.Width / 2, pictureSize.Height / 2);
				ctrl.ScaleImage(0.5);
				_isZoomed = false;
			}
			chkToggleZoom.Checked = _isZoomed;
			RefreshViewer();
		}

		private void btnToggleView_Click(object sender, EventArgs e)
		{
			ToggleView();
		}

		private void chkToggleZoom_Click(object sender, EventArgs e)
		{
			ToggleZoom();
		}
	}
}
