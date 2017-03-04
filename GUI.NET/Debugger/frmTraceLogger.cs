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
using Mesen.GUI.Controls;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmTraceLogger : BaseForm
	{
		private int _lineCount;
		private bool _loggingEnabled = false;
		private string _lastFilename;

		public frmTraceLogger()
		{
			InitializeComponent();

			DebugInfo debugInfo = ConfigManager.Config.DebugInfo;
			mnuAutoRefresh.Checked = debugInfo.TraceAutoRefresh;
			_lineCount = debugInfo.TraceLineCount;
			chkIndentCode.Checked = debugInfo.TraceIndentCode;
			chkShowByteCode.Checked = debugInfo.TraceShowByteCode;
			chkShowCpuCycles.Checked = debugInfo.TraceShowCpuCycles;
			chkShowEffectiveAddresses.Checked = debugInfo.TraceShowEffectiveAddresses;
			chkShowExtraInfo.Checked = debugInfo.TraceShowExtraInfo;
			chkShowFrameCount.Checked = debugInfo.TraceShowFrameCount;
			chkShowPpuCycles.Checked = debugInfo.TraceShowPpuCycles;
			chkShowPpuScanline.Checked = debugInfo.TraceShowPpuScanline;
			chkShowRegisters.Checked = debugInfo.TraceShowRegisters;
			chkUseLabels.Checked = debugInfo.TraceUseLabels;

			UpdateMenu();
			txtTraceLog.Font = new Font(BaseControl.MonospaceFontFamily, 10);
			tmrUpdateLog.Start();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			DebugInfo debugInfo = ConfigManager.Config.DebugInfo;
			debugInfo.TraceAutoRefresh = mnuAutoRefresh.Checked;
			debugInfo.TraceLineCount = _lineCount;
			debugInfo.TraceIndentCode = chkIndentCode.Checked;
			debugInfo.TraceShowByteCode = chkShowByteCode.Checked;
			debugInfo.TraceShowCpuCycles = chkShowCpuCycles.Checked;
			debugInfo.TraceShowEffectiveAddresses = chkShowEffectiveAddresses.Checked;
			debugInfo.TraceShowExtraInfo = chkShowExtraInfo.Checked;
			debugInfo.TraceShowFrameCount = chkShowFrameCount.Checked;
			debugInfo.TraceShowPpuCycles = chkShowPpuCycles.Checked;
			debugInfo.TraceShowPpuScanline = chkShowPpuScanline.Checked;
			debugInfo.TraceShowRegisters = chkShowRegisters.Checked;
			debugInfo.TraceUseLabels = chkUseLabels.Checked;
			ConfigManager.ApplyChanges();

			if(_loggingEnabled) {
				InteropEmu.DebugStopTraceLogger();
			}
		}

		private void SetOptions()
		{
			TraceLoggerOptions options = new TraceLoggerOptions() {
				ShowByteCode = chkShowByteCode.Checked,
				ShowCpuCycles = chkShowCpuCycles.Checked,
				ShowExtraInfo = chkShowExtraInfo.Checked,
				ShowPpuCycles = chkShowPpuCycles.Checked,
				ShowPpuFrames = chkShowFrameCount.Checked,
				ShowPpuScanline = chkShowPpuScanline.Checked,
				ShowRegisters = chkShowRegisters.Checked,
				IndentCode = chkIndentCode.Checked,
				ShowEffectiveAddresses = chkShowEffectiveAddresses.Checked,
				UseLabels = chkUseLabels.Checked
			};

			InteropEmu.DebugSetTraceOptions(options);
		}

		private void btnStartLogging_Click(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.FileName = "Trace logs (*.txt)|*.txt";
				sfd.FileName = "Trace - " + InteropEmu.GetRomInfo().GetRomName() + ".txt";
				sfd.InitialDirectory = ConfigManager.DebuggerFolder;
				if(sfd.ShowDialog() == DialogResult.OK) {
					_lastFilename = sfd.FileName;
					SetOptions();
					InteropEmu.DebugStartTraceLogger(sfd.FileName);

					btnStartLogging.Enabled = false;
					btnStopLogging.Enabled = true;
					btnOpenTrace.Enabled = false;

					_loggingEnabled = true;
				}
			}
		}

		private void btnStopLogging_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugStopTraceLogger();
			btnStartLogging.Enabled = true;
			btnStopLogging.Enabled = false;
			btnOpenTrace.Enabled = true;
		}

		private void btnOpenTrace_Click(object sender, EventArgs e)
		{
			try {
				System.Diagnostics.Process.Start(_lastFilename);
			} catch { }
		}

		private void RefreshLog()
		{
			SetOptions();
			string newTrace = InteropEmu.DebugGetExecutionTrace((UInt32)_lineCount).Replace("\n", Environment.NewLine);

			if(newTrace != txtTraceLog.Text) {
				txtTraceLog.Text = newTrace;
				txtTraceLog.SelectionStart = txtTraceLog.TextLength;
				txtTraceLog.ScrollToCaret();
			}
		}

		private void UpdateMenu()
		{
			mnu30000Lines.Checked = _lineCount == 30000;
			mnu10000Lines.Checked = _lineCount == 10000;
			mnu1000Lines.Checked = _lineCount == 1000;
			mnu100Lines.Checked = _lineCount == 100;

			if(_lineCount >= 10000) {
				mnuAutoRefresh.Checked = false;
			} else if(_lineCount == 1000) {
				tmrUpdateLog.Interval = 250;
			} else {
				tmrUpdateLog.Interval = 150;
			}

			mnuAutoRefresh.Enabled = _lineCount < 10000;
		}

		private void tmrUpdateLog_Tick(object sender, EventArgs e)
		{
			if(mnuAutoRefresh.Checked) {
				RefreshLog();
			}
		}

		private void mnu30000Lines_Click(object sender, EventArgs e)
		{
			_lineCount = 30000;
			UpdateMenu();
		}

		private void mnu10000Lines_Click(object sender, EventArgs e)
		{
			_lineCount = 10000;
			UpdateMenu();
		}

		private void mnu1000Lines_Click(object sender, EventArgs e)
		{
			_lineCount = 1000;
			UpdateMenu();
		}

		private void mnu100Lines_Click(object sender, EventArgs e)
		{
			_lineCount = 100;
			UpdateMenu();
		}

		private void mnuRefresh_Click(object sender, EventArgs e)
		{
			RefreshLog();
		}
	}
}
