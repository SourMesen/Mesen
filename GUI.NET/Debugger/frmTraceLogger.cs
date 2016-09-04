using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmTraceLogger : BaseForm
	{
		private bool _loggingEnabled = false;

		public frmTraceLogger()
		{
			InitializeComponent();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if(_loggingEnabled) {
				InteropEmu.DebugStopTraceLogger();
			}
		}

		private void btnStartLogging_Click(object sender, EventArgs e)
		{
			TraceLoggerOptions options = new TraceLoggerOptions() {
				ShowByteCode = chkShowByteCode.Checked,
				ShowCpuCycles = chkShowCpuCycles.Checked,
				ShowExtraInfo = chkShowExtraInfo.Checked,
				ShowPpuCycles = chkShowPpuCycles.Checked,
				ShowPpuFrames = chkShowFrameCount.Checked,
				ShowPpuScanline = chkShowPpuScanline.Checked,
				ShowRegisters = chkShowRegisters.Checked,
				IndentCode = chkIndentCode.Checked
			};

			InteropEmu.DebugStartTraceLogger(options);

			btnStartLogging.Enabled = false;
			btnStopLogging.Enabled = true;
			btnOpenTrace.Enabled = true;

			_loggingEnabled = true;
		}

		private void btnStopLogging_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugStopTraceLogger();
			btnStartLogging.Enabled = true;
			btnStopLogging.Enabled = false;
		}

		private void btnOpenTrace_Click(object sender, EventArgs e)
		{
			try {
				System.Diagnostics.Process.Start(Path.Combine(ConfigManager.DebuggerFolder, "Trace - " + InteropEmu.GetRomInfo().GetRomName() + ".log"));
			} catch { }
		}
	}
}
