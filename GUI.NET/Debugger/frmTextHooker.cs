using System;
using System.Collections.Concurrent;
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
	public partial class frmTextHooker : BaseForm
	{
		private static Dictionary<string, string> _defaultCharMappings = new Dictionary<string, string>();
		private ConcurrentDictionary<string, string> _charMappings = new ConcurrentDictionary<string, string>();

		private DateTime _lastUpdate = DateTime.MinValue;
		private InteropEmu.NotificationListener _notifListener;
		private TabPage _selectedTab;
		private bool _refreshing = false;
		private int _ppuViewerId = 0;

		public frmTextHooker()
		{
			InitializeComponent();

			_ppuViewerId = frmPpuViewer.GetNextPpuViewerId();

			this._selectedTab = this.tpgTextHooker;
			this.mnuAutoRefresh.Checked = ConfigManager.Config.DebugInfo.TextHookerAutoRefresh;
			this.mnuRefreshOnBreak.Checked = ConfigManager.Config.DebugInfo.TextHookerRefreshOnBreak;

			if(ConfigManager.Config.DebugInfo.TextHookerWindowLocation.HasValue) {
				this.StartPosition = FormStartPosition.Manual;
				this.Location = ConfigManager.Config.DebugInfo.TextHookerWindowLocation.Value;
			}
		}

		static frmTextHooker()
		{
			char[] separator = new char[] { ',' };
			foreach(string mappingRow in ResourceManager.ReadZippedResource("CharacterMappings.txt").Replace("\r", "").Split('\n')) {
				string[] parts = mappingRow.Split(separator, 2);
				if(parts.Length == 2) {
					_defaultCharMappings[parts[0]] = parts[1];
				}
			}
		}

		private void InitShortcuts()
		{
			mnuRefresh.InitShortcut(this, nameof(DebuggerShortcutsConfig.Refresh));
		}
		
		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);

			if(!this.DesignMode) {
				this._charMappings = new ConcurrentDictionary<string, string>();

				foreach(KeyValuePair<string, string> kvp in _defaultCharMappings) {
					this._charMappings[kvp.Key] = kvp.Value;
				}

				foreach(CharMap mapping in ConfigManager.Config.DebugInfo.TextHookerCharMappings) {
					this._charMappings[mapping.Key] = mapping.Value;
				}

				this.ctrlScanlineCycle.Initialize(_ppuViewerId, ConfigManager.Config.DebugInfo.TextHookerDisplayScanline, ConfigManager.Config.DebugInfo.TextHookerDisplayCycle);
				this.ctrlTextHooker.SetCharacterMappings(_charMappings);
				this.ctrlCharacterMappings.SetCharacterMappings(_charMappings);

				this._notifListener = new InteropEmu.NotificationListener();
				this._notifListener.OnNotification += this._notifListener_OnNotification;

				this.ctrlTextHooker.GetData();
				this.ctrlCharacterMappings.GetData();

				this.ctrlTextHooker.RefreshViewer();
				this.ctrlCharacterMappings.RefreshViewer();

				this.InitShortcuts();
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			this._notifListener.OnNotification -= this._notifListener_OnNotification;
			InteropEmu.DebugClearPpuViewerSettings(_ppuViewerId);

			List<CharMap> mappings = new List<CharMap>();
			foreach(KeyValuePair<string, string> kvp in _charMappings) {
				if(!string.IsNullOrWhiteSpace(kvp.Value)) {
					string defaultMapping;
					if(_defaultCharMappings.TryGetValue(kvp.Key, out defaultMapping)) {
						if(defaultMapping == kvp.Value) {
							//Don't save mapping in user's profile if it matches the built-in mappings
							continue;
						}
					}

					mappings.Add(new CharMap() { Key = kvp.Key, Value = kvp.Value });
				}
			}
			ConfigManager.Config.DebugInfo.TextHookerCharMappings = mappings;
			ConfigManager.Config.DebugInfo.TextHookerWindowLocation = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Location : this.Location;
			ConfigManager.Config.DebugInfo.TextHookerDisplayScanline = ctrlScanlineCycle.Scanline;
			ConfigManager.Config.DebugInfo.TextHookerDisplayCycle = ctrlScanlineCycle.Cycle;
			ConfigManager.ApplyChanges();
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.CodeBreak:
				case InteropEmu.ConsoleNotificationType.GamePaused:
					if(ConfigManager.Config.DebugInfo.TextHookerRefreshOnBreak) {
						this.GetData();
						this.BeginInvoke((MethodInvoker)(() => this.RefreshViewers()));
					}
					break;

				case InteropEmu.ConsoleNotificationType.PpuViewerDisplayFrame:
					if(e.Parameter.ToInt32() == _ppuViewerId) {
						if(ConfigManager.Config.DebugInfo.TextHookerAutoRefresh && !_refreshing && (DateTime.Now - _lastUpdate).Milliseconds > 66) {
							//Update at 15 fps at most
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
			if(_selectedTab == this.tpgTextHooker) {
				this.ctrlTextHooker.GetData();
			} else if(_selectedTab == this.tpgCharacterMappings) {
				this.ctrlCharacterMappings.GetData();
			}
		}

		private void RefreshViewers()
		{
			_refreshing = true;
			if(_selectedTab == this.tpgTextHooker) {
				this.ctrlTextHooker.RefreshViewer();
			} else if(_selectedTab == this.tpgCharacterMappings) {
				this.ctrlCharacterMappings.RefreshViewer();
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
			ConfigManager.Config.DebugInfo.TextHookerAutoRefresh = this.mnuAutoRefresh.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuRefreshOnBreak_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.TextHookerRefreshOnBreak = this.mnuRefreshOnBreak.Checked;
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
	}
}
