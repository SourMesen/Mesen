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

		private static int _nextPpuViewerId = 0;
		private int _ppuViewerId = 0;

		public frmPpuViewer()
		{
			InitializeComponent();

			_ppuViewerId = GetNextPpuViewerId();

			this._selectedTab = this.tpgNametableViewer;
			this.mnuAutoRefresh.Checked = ConfigManager.Config.DebugInfo.PpuAutoRefresh;
			this.mnuRefreshOnBreak.Checked = ConfigManager.Config.DebugInfo.PpuRefreshOnBreak;
			this.ctrlNametableViewer.Connect(this.ctrlChrViewer);

			if(ConfigManager.Config.DebugInfo.PpuWindowLocation.HasValue) {
				this.StartPosition = FormStartPosition.Manual;
				this.Location = ConfigManager.Config.DebugInfo.PpuWindowLocation.Value;
			}
		}

		public static int GetNextPpuViewerId()
		{
			return _nextPpuViewerId++;
		}

		private void InitShortcuts()
		{
			mnuRefresh.InitShortcut(this, nameof(DebuggerShortcutsConfig.Refresh));
		}

		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);

			if(!this.DesignMode) {
				this._notifListener = new InteropEmu.NotificationListener();
				this._notifListener.OnNotification += this._notifListener_OnNotification;

				this.ctrlScanlineCycle.Initialize(_ppuViewerId, ConfigManager.Config.DebugInfo.PpuDisplayScanline, ConfigManager.Config.DebugInfo.PpuDisplayCycle);

				this.ctrlNametableViewer.GetData();
				this.ctrlChrViewer.GetData();
				this.ctrlSpriteViewer.GetData();
				this.ctrlPaletteViewer.GetData();

				this.ctrlNametableViewer.RefreshViewer();
				this.ctrlChrViewer.RefreshViewer();
				this.ctrlSpriteViewer.RefreshViewer();
				this.ctrlPaletteViewer.RefreshViewer();

				this.InitShortcuts();
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			this._notifListener.OnNotification -= this._notifListener_OnNotification;
			ConfigManager.Config.DebugInfo.PpuWindowLocation = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Location : this.Location;
			ConfigManager.Config.DebugInfo.PpuDisplayScanline = ctrlScanlineCycle.Scanline;
			ConfigManager.Config.DebugInfo.PpuDisplayCycle = ctrlScanlineCycle.Cycle;
			ConfigManager.ApplyChanges();
			InteropEmu.DebugClearPpuViewerSettings(_ppuViewerId);
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.CodeBreak:
				case InteropEmu.ConsoleNotificationType.GamePaused:
					if(ConfigManager.Config.DebugInfo.PpuRefreshOnBreak) {
						this.GetData();
						this.BeginInvoke((MethodInvoker)(() => this.RefreshViewers()));
					}
					break;

				case InteropEmu.ConsoleNotificationType.PpuViewerDisplayFrame:
					if(e.Parameter.ToInt32() == _ppuViewerId) {
						if(ConfigManager.Config.DebugInfo.PpuAutoRefresh && !_refreshing && (DateTime.Now - _lastUpdate).Milliseconds > 66) {
							//Update at 15 fps most
							this.GetData();
							this.BeginInvoke((MethodInvoker)(() => this.RefreshViewers()));
							_lastUpdate = DateTime.Now;
						}
					}
					break;

				case InteropEmu.ConsoleNotificationType.GameLoaded:
					//Configuration is lost when debugger is restarted (when switching game or power cycling)
					ctrlScanlineCycle.RefreshSettings();
					break;
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
			this.GetData();
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

		private void mnuRefreshOnBreak_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.PpuRefreshOnBreak = this.mnuRefreshOnBreak.Checked;
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

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(!this.ctrlScanlineCycle.ContainsFocus) {
				if(keyData == ConfigManager.Config.DebugInfo.Shortcuts.Copy) {
					if(this.tabMain.SelectedTab == tpgNametableViewer) {
						ctrlNametableViewer.CopyToClipboard();
					} else if(this.tabMain.SelectedTab == tpgChrViewer) {
						ctrlChrViewer.CopyToClipboard();
					} else if(this.tabMain.SelectedTab == tpgSpriteViewer) {
						ctrlSpriteViewer.CopyToClipboard();
					}
				} else if(this.tabMain.SelectedTab == tpgChrViewer) {
					bool shift = keyData.HasFlag(Keys.Shift);
					keyData &= ~Keys.Shift;

					if(keyData >= Keys.D1 && keyData <= Keys.D9) {
						if(shift) {
							this.ctrlChrViewer.SelectPalette(keyData - Keys.D1);
						} else {
							this.ctrlChrViewer.SelectColor((keyData - Keys.D1) % 4);
						}
						return true;
					}
					if(keyData >= Keys.NumPad1 && keyData <= Keys.NumPad9) {
						if(shift) {
							this.ctrlChrViewer.SelectPalette(keyData - Keys.NumPad1);
						} else {
							this.ctrlChrViewer.SelectColor((keyData - Keys.NumPad1) % 4);
						}
						return true;
					}
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
