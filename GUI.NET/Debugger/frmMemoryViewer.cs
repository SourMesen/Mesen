using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmMemoryViewer : BaseForm
	{
		private InteropEmu.NotificationListener _notifListener;

		public frmMemoryViewer()
		{
			InitializeComponent();

			this.cboMemoryType.SelectedIndex = 0;
			this.Size = new Size(this.ctrlHexViewer.IdealWidth, this.Height);

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;
		}

		void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.CodeBreak) {
				this.BeginInvoke((MethodInvoker)(() => this.RefreshData()));
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
		}

		private void mnuDecreaseFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.FontSize--;
		}

		private void mnuResetFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.FontSize = 13;
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
