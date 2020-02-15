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
	public partial class frmProfiler : BaseForm
	{
		public frmProfiler()
		{
			InitializeComponent();

			if(!DesignMode) {
				DebugWorkspaceManager.AutoLoadDbgFiles(true);

				ctrlProfiler.RefreshData();
				ctrlProfiler.RefreshList();
				tmrRefresh.Start();

				RestoreLocation(ConfigManager.Config.DebugInfo.ProfilerLocation, ConfigManager.Config.DebugInfo.ProfilerSize);
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			ConfigManager.Config.DebugInfo.ProfilerSize = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Size : this.Size;
			ConfigManager.Config.DebugInfo.ProfilerLocation = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Location : this.Location;
			ConfigManager.ApplyChanges();
		}

		private void tmrRefresh_Tick(object sender, EventArgs e)
		{
			ctrlProfiler.RefreshData();
			ctrlProfiler.RefreshList();
		}
	}
}
