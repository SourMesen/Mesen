using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmMemoryViewer : BaseForm
	{
		private InteropEmu.NotificationListener _notifListener;
		private int _autoRefreshCounter = 0;

		public frmMemoryViewer()
		{
			InitializeComponent();

			this.mnuAutoRefresh.Checked = ConfigManager.Config.DebugInfo.RamAutoRefresh;
			this.ctrlHexViewer.FontSize = ConfigManager.Config.DebugInfo.RamFontSize;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.cboMemoryType.SelectedIndex = 0;
			this.Size = new Size(this.ctrlHexViewer.IdealWidth, this.Height);

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;
		}

		void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.CodeBreak) {
				this.BeginInvoke((MethodInvoker)(() => this.RefreshData()));
			} else if(e.NotificationType == InteropEmu.ConsoleNotificationType.PpuFrameDone) {
				this.BeginInvoke((MethodInvoker)(() => {
					if(_autoRefreshCounter % 4 == 0 && this.mnuAutoRefresh.Checked) {
						this.RefreshData();
					}
					_autoRefreshCounter++;
				}));
			}
		}
		
		private void cboMemoryType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.RefreshData();
			this.ctrlHexViewer.ScrollToTop();
		}

		private void mnuRefresh_Click(object sender, EventArgs e)
		{
			this.RefreshData();
		}

		private void RefreshData()
		{
			this.ctrlHexViewer.Data = InteropEmu.DebugGetMemoryState((DebugMemoryType)this.cboMemoryType.SelectedIndex);
		}

		private void ctrlHexViewer_ColumnCountChanged(object sender, EventArgs e)
		{
			this.Size = new Size(this.ctrlHexViewer.IdealWidth, this.Height);
		}

		private void mnuFind_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.OpenSearchBox();
		}

		private void mnuFindNext_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.FindNext();
		}

		private void mnuFindPrev_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.FindPrevious();
		}

		private void mnuGoTo_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.GoToAddress();
		}

		private void mnuIncreaseFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.FontSize++;
			this.UpdateConfig();
		}

		private void mnuDecreaseFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.FontSize--;
			this.UpdateConfig();
		}

		private void mnuResetFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.FontSize = 13;
			this.UpdateConfig();
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void UpdateConfig()
		{
			ConfigManager.Config.DebugInfo.RamAutoRefresh = this.mnuAutoRefresh.Checked;
			ConfigManager.Config.DebugInfo.RamFontSize = this.ctrlHexViewer.FontSize;
			ConfigManager.ApplyChanges();
		}

		private void mnuAutoRefresh_Click(object sender, EventArgs e)
		{
			this.UpdateConfig();
		}
	}
}
