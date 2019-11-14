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
		private EntityBinder _binder = new EntityBinder();
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
				_binder.Entity = ConfigManager.Config.DebugInfo;

				mnuRefreshOnBreak.Checked = ConfigManager.Config.DebugInfo.EventViewerRefreshOnBreak;

				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuWrite2000), chkWrite2000);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuWrite2001), chkWrite2001);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuWrite2003), chkWrite2003);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuWrite2004), chkWrite2004);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuWrite2005), chkWrite2005);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuWrite2006), chkWrite2006);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuWrite2007), chkWrite2007);

				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuRead2002), chkRead2002);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuRead2004), chkRead2004);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPpuRead2007), chkRead2007);

				_binder.AddBinding(nameof(DebugInfo.EventViewerShowIrq), chkShowIrq);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowNmi), chkShowNmi);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowSpriteZeroHit), chkShowSpriteZero);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowMapperRegisterWrites), chkShowMapperRegisterWrites);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowMapperRegisterReads), chkShowMapperRegisterReads);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowMarkedBreakpoints), chkBreakpoints);

				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterWrite2000Color), picWrite2000);
				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterWrite2001Color), picWrite2001);
				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterWrite2003Color), picWrite2003);
				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterWrite2004Color), picWrite2004);
				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterWrite2005Color), picWrite2005);
				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterWrite2006Color), picWrite2006);
				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterWrite2007Color), picWrite2007);

				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterRead2002Color), picRead2002);
				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterRead2004Color), picRead2004);
				_binder.AddBinding(nameof(DebugInfo.EventViewerPpuRegisterRead2007Color), picRead2007);

				_binder.AddBinding(nameof(DebugInfo.EventViewerMapperRegisterWriteColor), picMapperWrite);
				_binder.AddBinding(nameof(DebugInfo.EventViewerMapperRegisterReadColor), picMapperRead);
				_binder.AddBinding(nameof(DebugInfo.EventViewerNmiColor), picNmi);
				_binder.AddBinding(nameof(DebugInfo.EventViewerIrqColor), picIrq);
				_binder.AddBinding(nameof(DebugInfo.EventViewerSpriteZeroHitColor), picSpriteZeroHit);
				_binder.AddBinding(nameof(DebugInfo.EventViewerBreakpointColor), picBreakpoint);

				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPreviousFrameEvents), chkShowPreviousFrameEvents);

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

				_binder.UpdateUI();

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

			_binder.UpdateObject();
			ConfigManager.Config.DebugInfo.EventViewerLocation = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Location : this.Location;
			ConfigManager.ApplyChanges();
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
			if(_binder.Updating) {
				return;
			}

			_refreshing = true;
			_binder.UpdateObject();
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
		
		private void chkShowPreviousFrameEvents_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.EventViewerShowPreviousFrameEvents = chkShowPreviousFrameEvents.Checked;
			ConfigManager.ApplyChanges();
			if(InteropEmu.DebugIsExecutionStopped()) {
				this.GetData();
				this.RefreshViewer();
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

		private void picColor_BackColorChanged(object sender, EventArgs e)
		{
			RefreshViewer();
		}

		private void chkShowHide_Click(object sender, EventArgs e)
		{
			RefreshViewer();
		}

		private void mnuResetColors_Click(object sender, EventArgs e)
		{
			picWrite2000.BackColor = ColorTranslator.FromHtml("#FF5E5E");
			picWrite2001.BackColor = ColorTranslator.FromHtml("#8E33FF");
			picWrite2003.BackColor = ColorTranslator.FromHtml("#FF84E0");
			picWrite2004.BackColor = ColorTranslator.FromHtml("#FAFF39");
			picWrite2005.BackColor = ColorTranslator.FromHtml("#2EFF28");
			picWrite2006.BackColor = ColorTranslator.FromHtml("#3D2DFF");
			picWrite2007.BackColor = ColorTranslator.FromHtml("#FF060D");

			picRead2002.BackColor = ColorTranslator.FromHtml("#FF8224");
			picRead2004.BackColor = ColorTranslator.FromHtml("#24A672");
			picRead2007.BackColor = ColorTranslator.FromHtml("#6AF0FF");

			picMapperRead.BackColor = ColorTranslator.FromHtml("#C92929");
			picMapperWrite.BackColor = ColorTranslator.FromHtml("#007597");

			picNmi.BackColor = ColorTranslator.FromHtml("#ABADAC");
			picIrq.BackColor = ColorTranslator.FromHtml("#F9FEAC");
			picSpriteZeroHit.BackColor = ColorTranslator.FromHtml("#9F93C6");
			picBreakpoint.BackColor = ColorTranslator.FromHtml("#1898E4");
		}
	}
}
