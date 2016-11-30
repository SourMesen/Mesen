using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmPpuViewer : BaseForm
	{
		private InteropEmu.NotificationListener _notifListener;
		private int _autoRefreshCounter = 0;
		private TabPage _selectedTab;

		public frmPpuViewer()
		{
			InitializeComponent();

			this._selectedTab = this.tpgNametableViewer;
			this.mnuAutoRefresh.Checked = ConfigManager.Config.DebugInfo.PpuAutoRefresh;
		}

		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);

			if(!this.DesignMode) {
				this._notifListener = new InteropEmu.NotificationListener();
				this._notifListener.OnNotification += this._notifListener_OnNotification;

				InteropEmu.DebugSetPpuViewerScanlineCycle((int)this.nudScanline.Value, (int)this.nudCycle.Value);

				this.ctrlNametableViewer.GetData();
				this.ctrlChrViewer.GetData();
				this.ctrlSpriteViewer.GetData();
				this.ctrlPaletteViewer.GetData();

				this.ctrlNametableViewer.RefreshViewer();
				this.ctrlChrViewer.RefreshViewer();
				this.ctrlSpriteViewer.RefreshViewer();
				this.ctrlPaletteViewer.RefreshViewer();
			}
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.CodeBreak) {
				this.GetData();
				this.BeginInvoke((MethodInvoker)(() => this.RefreshViewers()));
			} else if(e.NotificationType == InteropEmu.ConsoleNotificationType.PpuViewerDisplayFrame) {
				if(_autoRefreshCounter % 4 == 0) {
					this.GetData();
				}
				this.BeginInvoke((MethodInvoker)(() => this.AutoRefresh()));
			}
		}

		private void GetData()
		{
			if(_selectedTab == this.tpgNametableViewer) {
				this.ctrlNametableViewer.GetData();
			} else if(_selectedTab == this.tpgChrViewer) {
				this.ctrlChrViewer.GetData();
			} else if(_selectedTab == this.tpgSpriteViewer) {
				this.ctrlSpriteViewer.GetData();
			} else if(_selectedTab == this.tpgPaletteViewer) {
				this.ctrlPaletteViewer.GetData();
			}
		}

		private void RefreshViewers()
		{
			if(_selectedTab == this.tpgNametableViewer) {
				this.ctrlNametableViewer.RefreshViewer();
			} else if(_selectedTab == this.tpgChrViewer) {
				this.ctrlChrViewer.RefreshViewer();
			} else if(_selectedTab == this.tpgSpriteViewer) {
				this.ctrlSpriteViewer.RefreshViewer();
			} else if(_selectedTab == this.tpgPaletteViewer) {
				this.ctrlPaletteViewer.RefreshViewer();
			}
		}

		private void AutoRefresh()
		{
			if(_autoRefreshCounter % 4 == 0 && this.mnuAutoRefresh.Checked) {
				this.RefreshViewers();
			}
			_autoRefreshCounter++;
		}
		
		private void mnuRefresh_Click(object sender, EventArgs e)
		{
			this.RefreshViewers();
		}
		
		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void mnuAutoRefresh_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.PpuAutoRefresh = this.mnuAutoRefresh.Checked;
			ConfigManager.ApplyChanges();
		}

		private void nudScanlineCycle_ValueChanged(object sender, EventArgs e)
		{
			InteropEmu.DebugSetPpuViewerScanlineCycle((int)this.nudScanline.Value, (int)this.nudCycle.Value);
		}

		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._selectedTab = this.tabMain.SelectedTab;
			if(InteropEmu.DebugIsExecutionStopped()) {
				//Refresh data when changing tabs when not running
				this.GetData();
				this.RefreshViewers();
			}
		}
	}
}
