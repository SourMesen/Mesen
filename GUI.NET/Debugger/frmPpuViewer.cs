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
		private DateTime _lastUpdate = DateTime.MinValue;
		private InteropEmu.NotificationListener _notifListener;
		private TabPage _selectedTab;
		private bool _refreshing = false;

		public frmPpuViewer()
		{
			InitializeComponent();

			this._selectedTab = this.tpgNametableViewer;
			this.mnuAutoRefresh.Checked = ConfigManager.Config.DebugInfo.PpuAutoRefresh;
			this.ctrlNametableViewer.Connect(this.ctrlChrViewer);
		}

		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);

			if(!this.DesignMode) {
				this._notifListener = new InteropEmu.NotificationListener();
				this._notifListener.OnNotification += this._notifListener_OnNotification;

				this.nudScanline.Value = ConfigManager.Config.DebugInfo.PpuDisplayScanline;
				this.nudCycle.Value = ConfigManager.Config.DebugInfo.PpuDisplayCycle;

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

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			this._notifListener.OnNotification -= this._notifListener_OnNotification;
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.CodeBreak) {
				this.GetData();
				this.BeginInvoke((MethodInvoker)(() => this.RefreshViewers()));
			} else if(e.NotificationType == InteropEmu.ConsoleNotificationType.PpuViewerDisplayFrame) {
				if(ConfigManager.Config.DebugInfo.PpuAutoRefresh && !_refreshing && (DateTime.Now - _lastUpdate).Milliseconds > 66) {
					//Update at 15 fps most
					this.GetData();
					this.BeginInvoke((MethodInvoker)(() => this.RefreshViewers()));
					_lastUpdate = DateTime.Now;
				}
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
			_refreshing = true;
			if(_selectedTab == this.tpgNametableViewer) {
				this.ctrlNametableViewer.RefreshViewer();
			} else if(_selectedTab == this.tpgChrViewer) {
				this.ctrlChrViewer.RefreshViewer();
			} else if(_selectedTab == this.tpgSpriteViewer) {
				this.ctrlSpriteViewer.RefreshViewer();
			} else if(_selectedTab == this.tpgPaletteViewer) {
				this.ctrlPaletteViewer.RefreshViewer();
			}
			_refreshing = false;
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
			ConfigManager.Config.DebugInfo.PpuDisplayScanline = (int)this.nudScanline.Value;
			ConfigManager.Config.DebugInfo.PpuDisplayCycle = (int)this.nudCycle.Value;
			ConfigManager.ApplyChanges();
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

		private void btnReset_Click(object sender, EventArgs e)
		{
			this.nudScanline.Value = 241;
			this.nudCycle.Value = 0;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(this.tabMain.SelectedTab == tpgChrViewer && !this.nudScanline.Focused && !this.nudCycle.Focused) {
				bool shift = keyData.HasFlag(Keys.Shift);
				keyData &= ~Keys.Shift;

				if(keyData >= Keys.D1 && keyData <= Keys.D8) {
					if(shift) {
						this.ctrlChrViewer.SelectPalette(keyData - Keys.D1);
					} else {
						this.ctrlChrViewer.SelectColor((keyData - Keys.D1) % 4);
					}
					return true;
				}
				if(keyData >= Keys.NumPad1 && keyData <= Keys.NumPad8) {
					if(shift) {
						this.ctrlChrViewer.SelectPalette(keyData - Keys.NumPad1);
					} else {
						this.ctrlChrViewer.SelectColor((keyData - Keys.NumPad1) % 4);
					}
					return true;
				}
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void ctrlNametableViewer_OnSelectChrTile(object sender, EventArgs e)
		{
			tabMain.SelectTab(tpgChrViewer);
		}

		private void ctrlSpriteViewer_OnSelectTilePalette(int tileIndex, int paletteIndex)
		{
			ctrlChrViewer.SelectedTileIndex = tileIndex;
			ctrlChrViewer.SelectedPaletteIndex = paletteIndex;
			tabMain.SelectTab(tpgChrViewer);
		}
	}
}
