using Mesen.GUI.Config;
using Mesen.GUI.Debugger.Controls;
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
	public partial class frmEventViewer : BaseForm, IRefresh
	{
		private WindowRefreshManager _refreshManager;
		private InteropEmu.NotificationListener _notifListener;
		private EntityBinder _binder = new EntityBinder();
		private bool _inListViewTab = false;
		private DebugInfo _config;

		public ctrlScanlineCycleSelect ScanlineCycleSelect => null;

		public frmEventViewer()
		{
			InitializeComponent();
			_config = ConfigManager.Config.DebugInfo;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!this.DesignMode) {
				_binder.Entity = _config;

				mnuRefreshOnBreak.Checked = _config.EventViewerRefreshOnBreak;

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
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowDmcDmaReads), chkShowDmcDmaRead);
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
				_binder.AddBinding(nameof(DebugInfo.EventViewerDmcDmaReadColor), picDmcDmaRead);

				_binder.AddBinding(nameof(DebugInfo.EventViewerShowPreviousFrameEvents), chkShowPreviousFrameEvents);
				_binder.AddBinding(nameof(DebugInfo.EventViewerShowNtscBorders), chkShowNtscBorders);

				DebugWorkspaceManager.GetWorkspace();

				RestoreLocation(_config.EventViewerLocation, _config.EventViewerSize);

				this._notifListener = new InteropEmu.NotificationListener(_config.DebugConsoleId);
				this._notifListener.OnNotification += this._notifListener_OnNotification;

				_refreshManager = new WindowRefreshManager(this);
				_refreshManager.AutoRefresh = _config.EventViewerAutoRefresh;
				_refreshManager.AutoRefreshSpeed = _config.EventViewerAutoRefreshSpeed;
				mnuAutoRefresh.Checked = _config.EventViewerAutoRefresh;
				mnuAutoRefreshLow.Click += (s, evt) => _refreshManager.AutoRefreshSpeed = RefreshSpeed.Low;
				mnuAutoRefreshNormal.Click += (s, evt) => _refreshManager.AutoRefreshSpeed = RefreshSpeed.Normal;
				mnuAutoRefreshHigh.Click += (s, evt) => _refreshManager.AutoRefreshSpeed = RefreshSpeed.High;
				mnuAutoRefreshSpeed.DropDownOpening += (s, evt) => UpdateRefreshSpeedMenu();

				this.RefreshData();
				_binder.UpdateUI();
				this.RefreshViewer();

				InitShortcuts();
			}
		}

		private void InitShortcuts()
		{
			mnuRefresh.InitShortcut(this, nameof(DebuggerShortcutsConfig.Refresh));
			mnuZoomIn.InitShortcut(this, nameof(DebuggerShortcutsConfig.ZoomIn));
			mnuZoomOut.InitShortcut(this, nameof(DebuggerShortcutsConfig.ZoomOut));

			mnuZoomIn.Click += (s, evt) => ctrlEventViewerPpuView.ZoomIn();
			mnuZoomOut.Click += (s, evt) => ctrlEventViewerPpuView.ZoomOut();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			this._notifListener.OnNotification -= this._notifListener_OnNotification;
			_notifListener?.Dispose();
			_refreshManager?.Dispose();

			_binder.UpdateObject();
			_config.EventViewerAutoRefresh = _refreshManager.AutoRefresh;
			_config.EventViewerAutoRefreshSpeed = _refreshManager.AutoRefreshSpeed;
			_config.EventViewerLocation = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Location : this.Location;
			_config.EventViewerSize = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Size : this.Size;
			ConfigManager.Config.DebugInfo = _config;
			ConfigManager.ApplyChanges();
		}
		
		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.CodeBreak:
				case InteropEmu.ConsoleNotificationType.GamePaused:
					if(_config.EventViewerRefreshOnBreak) {
						this.RefreshData();
						this.BeginInvoke((MethodInvoker)(() => this.RefreshViewer()));
					}
					break;
			}
		}

		public void RefreshData()
		{
			if(_inListViewTab) {
				ctrlEventViewerListView.GetData();
			} else {
				ctrlEventViewerPpuView.GetData();
			}
		}

		public void RefreshViewer()
		{
			if(_binder.Updating) {
				return;
			}

			_binder.UpdateObject();
			ctrlEventViewerPpuView.RefreshViewer();
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			_inListViewTab = tabMain.SelectedTab == tpgListView;
			RefreshData();
		}

		private void mnuRefreshOnBreak_Click(object sender, EventArgs e)
		{
			_config.EventViewerRefreshOnBreak = this.mnuRefreshOnBreak.Checked;
		}
		
		private void chkShowPreviousFrameEvents_Click(object sender, EventArgs e)
		{
			_config.EventViewerShowPreviousFrameEvents = chkShowPreviousFrameEvents.Checked;
			if(InteropEmu.DebugIsExecutionStopped()) {
				this.RefreshData();
				this.RefreshViewer();
			}
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
			picDmcDmaRead.BackColor = ColorTranslator.FromHtml("#A9FEFC");
		}

		private void mnuRefresh_Click(object sender, EventArgs e)
		{
			RefreshData();
			RefreshViewer();
		}

		private void mnuAutoRefresh_CheckedChanged(object sender, EventArgs e)
		{
			_refreshManager.AutoRefresh = mnuAutoRefresh.Checked;
		}

		private void UpdateRefreshSpeedMenu()
		{
			mnuAutoRefreshLow.Checked = _refreshManager.AutoRefreshSpeed == RefreshSpeed.Low;
			mnuAutoRefreshNormal.Checked = _refreshManager.AutoRefreshSpeed == RefreshSpeed.Normal;
			mnuAutoRefreshHigh.Checked = _refreshManager.AutoRefreshSpeed == RefreshSpeed.High;
		}
	}
}
