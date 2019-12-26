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
	public partial class frmWatchWindow : BaseForm
	{
		private InteropEmu.NotificationListener _notifListener;

		public frmWatchWindow()
		{
			InitializeComponent();

			if(!DesignMode) {
				RestoreLocation(ConfigManager.Config.DebugInfo.WatchWindowLocation, ConfigManager.Config.DebugInfo.WatchWindowSize);
				this.toolTip.SetToolTip(picWatchHelp, ctrlWatch.GetTooltipText());
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!DesignMode) {
				DebugWorkspaceManager.GetWorkspace();
				DebugWorkspaceManager.AutoLoadDbgFiles(true);
				_notifListener = new InteropEmu.NotificationListener(ConfigManager.Config.DebugInfo.DebugConsoleId);
				_notifListener.OnNotification += _notifListener_OnNotification;
				ctrlWatch.UpdateWatch(true);
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			ConfigManager.Config.DebugInfo.WatchWindowSize = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Size : this.Size;
			ConfigManager.Config.DebugInfo.WatchWindowLocation = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Location : this.Location;
			ConfigManager.ApplyChanges();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			if(_notifListener != null) {
				_notifListener.Dispose();
				_notifListener = null;
			}
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.PpuFrameDone:
					this.BeginInvoke((MethodInvoker)(() => {
						ctrlWatch.UpdateWatch(false);
					}));
					break;

				case InteropEmu.ConsoleNotificationType.CodeBreak:
					this.BeginInvoke((MethodInvoker)(() => {
						ctrlWatch.UpdateWatch(false);
					}));
					break;
			}
		}
	}
}
