using System;
using System.Drawing;
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
		private Size _originalSize;
		private bool _isCompact;

		private static int _nextPpuViewerId = 0;
		private int _ppuViewerId = 0;

		public frmPpuViewer()
		{
			InitializeComponent();

			_ppuViewerId = GetNextPpuViewerId();

			this._selectedTab = this.tpgNametableViewer;
			this.mnuAutoRefresh.Checked = ConfigManager.Config.DebugInfo.PpuAutoRefresh;
			this.mnuRefreshOnBreak.Checked = ConfigManager.Config.DebugInfo.PpuRefreshOnBreak;
			this.mnuShowInformationOverlay.Checked = ConfigManager.Config.DebugInfo.PpuShowInformationOverlay;
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
				this._notifListener = new InteropEmu.NotificationListener(ConfigManager.Config.DebugInfo.DebugConsoleId);
				this._notifListener.OnNotification += this._notifListener_OnNotification;

				this.ctrlScanlineCycle.Initialize(_ppuViewerId, ConfigManager.Config.DebugInfo.PpuDisplayScanline, ConfigManager.Config.DebugInfo.PpuDisplayCycle);

				GetData();
				
				this.ctrlNametableViewer.RefreshViewer();
				this.ctrlChrViewer.RefreshViewer();
				this.ctrlSpriteViewer.RefreshViewer();
				this.ctrlPaletteViewer.RefreshViewer();

				this.InitShortcuts();
				this.UpdateRefreshSpeedMenu();

				string toggleViewTooltip = "Toggle Compact/Normal View";
				if(ConfigManager.Config.DebugInfo.Shortcuts.PpuViewer_ToggleView != Keys.None) {
					toggleViewTooltip += " (" + DebuggerShortcutsConfig.GetShortcutDisplay(ConfigManager.Config.DebugInfo.Shortcuts.PpuViewer_ToggleView) + ")";
				}
				this.toolTip.SetToolTip(this.btnToggleView, toggleViewTooltip);

				_selectedTab = tabMain.SelectedTab;
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
						int refreshDelay = 60;
						switch(ConfigManager.Config.DebugInfo.PpuAutoRefreshSpeed) {
							case RefreshSpeed.Low: refreshDelay= 60; break;
							case RefreshSpeed.Normal: refreshDelay = 30; break;
							case RefreshSpeed.High: refreshDelay = 12; break;
						}
						if(ConfigManager.Config.DebugInfo.PpuAutoRefresh && !_refreshing && (DateTime.Now - _lastUpdate).Milliseconds > refreshDelay) {
							_lastUpdate = DateTime.Now;
							this.GetData();
							this.BeginInvoke((MethodInvoker)(() => this.RefreshViewers()));
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
			this.ctrlNametableViewer.GetData();
			this.ctrlChrViewer.GetData();
			this.ctrlSpriteViewer.GetData();
			this.ctrlPaletteViewer.GetData();
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

		private void mnuShowInformationOverlay_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.PpuShowInformationOverlay = this.mnuShowInformationOverlay.Checked;
			ConfigManager.ApplyChanges();
		}

		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._selectedTab = this.tabMain.SelectedTab;
			if(InteropEmu.DebugIsExecutionStopped()) {
				//Refresh data when changing tabs when not running
				this.RefreshViewers();
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == ConfigManager.Config.DebugInfo.Shortcuts.PpuViewer_ToggleView) {
				ToggleView();
				return true;
			}

			if(!this.ctrlScanlineCycle.ContainsFocus) {
				if(this.tabMain.SelectedTab == tpgChrViewer) {
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

		public void SelectChrTile(int tileIndex, int paletteIndex, bool allowOpenWindow)
		{
			if(_isCompact && allowOpenWindow) {
				//If in compact mode, don't move to the CHR tab, open or use another window instead
				frmPpuViewer otherPpuViewer = null;
				foreach(BaseForm frm in DebugWindowManager.GetWindows()) {
					if(frm != this && frm is frmPpuViewer && (!((frmPpuViewer)frm)._isCompact || ((frmPpuViewer)frm)._selectedTab == ((frmPpuViewer)frm).tpgChrViewer)) {
						//If a window exists and is either not in compact mode, or in compact mode and showing the CHR viewer, use it
						otherPpuViewer = frm as frmPpuViewer;
						break;
					}
				}
				if(otherPpuViewer == null) {
					//Open up a new viewer, in compact mode
					otherPpuViewer = (frmPpuViewer)DebugWindowManager.OpenDebugWindow(DebugWindow.PpuViewer);
					otherPpuViewer.SelectChrTile(tileIndex, paletteIndex, false);
					otherPpuViewer.ToggleView();
				} else {
					//Reuse an existing viewer that's not in compact mode
					otherPpuViewer.SelectChrTile(tileIndex, paletteIndex, false);
					otherPpuViewer.BringToFront();
				}
			} else {
				if(!InteropEmu.DebugIsExecutionStopped() || ConfigManager.Config.DebugInfo.PpuRefreshOnBreak) {
					//Only change the palette if execution is not stopped (or if we're configured to refresh the viewer on break/pause)
					//Otherwise, the CHR viewer will refresh its data (and it might not match the data we loaded at the specified scanline/cycle anymore)
					ctrlChrViewer.SelectedPaletteIndex = paletteIndex;
				}
				ctrlChrViewer.SelectedTileIndex = tileIndex;
				tabMain.SelectTab(tpgChrViewer);
				_selectedTab = tpgChrViewer;
			}
		}

		private void ctrlNametableViewer_OnSelectChrTile(int tileIndex, int paletteIndex)
		{
			SelectChrTile(tileIndex, paletteIndex, true);
		}

		private void ctrlSpriteViewer_OnSelectTilePalette(int tileIndex, int paletteIndex)
		{
			SelectChrTile(tileIndex, paletteIndex, true);
		}

		private void UpdateRefreshSpeedMenu()
		{
			mnuAutoRefreshLow.Checked = ConfigManager.Config.DebugInfo.PpuAutoRefreshSpeed == RefreshSpeed.Low;
			mnuAutoRefreshNormal.Checked = ConfigManager.Config.DebugInfo.PpuAutoRefreshSpeed == RefreshSpeed.Normal;
			mnuAutoRefreshHigh.Checked = ConfigManager.Config.DebugInfo.PpuAutoRefreshSpeed == RefreshSpeed.High;
		}

		private void mnuAutoRefreshSpeed_Click(object sender, EventArgs e)
		{
			if(sender == mnuAutoRefreshLow) {
				ConfigManager.Config.DebugInfo.PpuAutoRefreshSpeed = RefreshSpeed.Low;
			} else if(sender == mnuAutoRefreshNormal) {
				ConfigManager.Config.DebugInfo.PpuAutoRefreshSpeed = RefreshSpeed.Normal;
			} else if(sender == mnuAutoRefreshHigh) {
				ConfigManager.Config.DebugInfo.PpuAutoRefreshSpeed = RefreshSpeed.High;
			}
			ConfigManager.ApplyChanges();

			UpdateRefreshSpeedMenu();
		}

		private void ToggleCompactMode(ICompactControl control, TabPage tab, string title)
		{
			if(!_isCompact) {
				Point tabTopLeft = tabMain.PointToScreen(Point.Empty);
				Point tabContentTopLeft = tab.PointToScreen(Point.Empty);

				int heightGap = tabContentTopLeft.Y - tabTopLeft.Y + ctrlScanlineCycle.Height;

				_isCompact = true;
				_originalSize = this.Size;
				Size size = control.GetCompactSize();
				int widthDiff = ((Control)control).Width - size.Width;
				int heightDiff = ((Control)control).Height - size.Height;

				this.Controls.Add((Control)control);
				((Control)control).BringToFront();

				tabMain.Visible = false;
				ctrlScanlineCycle.Visible = false;
				this.Text = title;

				this.Size = new Size(this.Width - widthDiff, this.Height - heightDiff - heightGap + 3);
			} else {
				_isCompact = false;
				this.Size = _originalSize;
				tabMain.Visible = true;
				tab.Controls.Add((Control)control);
				ctrlScanlineCycle.Visible = true;
				this.Text = "PPU Viewer";
			}

			btnToggleView.Image = _isCompact ? Properties.Resources.Expand : Properties.Resources.Collapse;
		}

		private void ToggleView()
		{
			if(_selectedTab == tpgChrViewer) {
				ToggleCompactMode(ctrlChrViewer, tpgChrViewer, "CHR Viewer");
			} else if(_selectedTab == tpgPaletteViewer) {
				ToggleCompactMode(ctrlPaletteViewer, tpgPaletteViewer, "Palette Viewer");
			} else if(_selectedTab == tpgSpriteViewer) {
				ToggleCompactMode(ctrlSpriteViewer, tpgSpriteViewer, "Sprite Viewer");
			} else if(_selectedTab == tpgNametableViewer) {
				ToggleCompactMode(ctrlNametableViewer, tpgNametableViewer, "Nametable Viewer");
			}
		}

		private void btnToggleView_Click(object sender, EventArgs e)
		{
			ToggleView();
		}
	}

	public interface ICompactControl
	{
		Size GetCompactSize();
	}
}
