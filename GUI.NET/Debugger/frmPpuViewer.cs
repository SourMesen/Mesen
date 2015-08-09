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
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmPpuViewer : BaseForm
	{
		private InteropEmu.NotificationListener _notifListener;
		private int _autoRefreshCounter = 0;

		public frmPpuViewer()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);

			if(!this.DesignMode) {
				this._notifListener = new InteropEmu.NotificationListener();
				this._notifListener.OnNotification += this._notifListener_OnNotification;

				this.ctrlNametableViewer.RefreshViewer();
				this.ctrlChrViewer.RefreshViewer();
				this.ctrlSpriteViewer.RefreshViewer();
				this.ctrlPaletteViewer.RefreshViewer();
			}
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.CodeBreak) {
				this.BeginInvoke((MethodInvoker)(() => this.RefreshViewers()));
			} else if(e.NotificationType == InteropEmu.ConsoleNotificationType.PpuFrameDone) {
				this.BeginInvoke((MethodInvoker)(() => this.AutoRefresh()));
			}
		}

		private void RefreshViewers()
		{
			if(this.tabMain.SelectedTab == this.tpgNametableViewer) {
				this.ctrlNametableViewer.RefreshViewer();
			} else if(this.tabMain.SelectedTab == this.tpgChrViewer) {
				this.ctrlChrViewer.RefreshViewer();
			} else if(this.tabMain.SelectedTab == this.tpgSpriteViewer) {
				this.ctrlSpriteViewer.RefreshViewer();
			} else if(this.tabMain.SelectedTab == this.tpgPaletteViewer) {
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
	}
}
