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
		private EntityBinder _entityBinder = new EntityBinder();
		private int _previousCycleCount;
		private string _previousTrace;
		private volatile bool _refreshRunning;

		public frmTraceLogger()
		{
			InitializeComponent();

			DebugInfo debugInfo = ConfigManager.Config.DebugInfo;
			if(!debugInfo.TraceLoggerSize.IsEmpty) {
				this.Size = debugInfo.TraceLoggerSize;
			}

			mnuAutoRefresh.Checked = debugInfo.TraceAutoRefresh;
			_lineCount = debugInfo.TraceLineCount;

			_entityBinder.Entity = debugInfo.TraceLoggerOptions;
			_entityBinder.AddBinding("ShowByteCode", chkShowByteCode);
			_entityBinder.AddBinding("ShowCpuCycles", chkShowCpuCycles);
			_entityBinder.AddBinding("ShowEffectiveAddresses", chkShowEffectiveAddresses);
			_entityBinder.AddBinding("ShowExtraInfo", chkShowExtraInfo);
			_entityBinder.AddBinding("ShowPpuFrames", chkShowFrameCount);
			_entityBinder.AddBinding("ShowPpuCycles", chkShowPpuCycles);
			_entityBinder.AddBinding("ShowPpuScanline", chkShowPpuScanline);
			_entityBinder.AddBinding("ShowRegisters", chkShowRegisters);
			_entityBinder.AddBinding("IndentCode", chkIndentCode);
			_entityBinder.AddBinding("UseLabels", chkUseLabels);
			_entityBinder.AddBinding("StatusFormat", cboStatusFlagFormat);
			_entityBinder.UpdateUI();

			UpdateMenu();
			txtTraceLog.FontSize = 10;
			tmrUpdateLog.Start();
			RefreshLog(true);

			this.toolTip.SetToolTip(this.picExpressionWarning, "Condition contains invalid syntax or symbols.");
			this.toolTip.SetToolTip(this.picHelp, "When a condition is given, instructions will only be logged by the trace logger if the condition returns a value not equal to 0 or false." + Environment.NewLine + Environment.NewLine + frmBreakpoint.GetConditionTooltip(false));
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			tmrUpdateLog.Stop();
			while(_refreshRunning) {
				System.Threading.Thread.Sleep(50);
			}

			base.OnFormClosing(e);

			DebugInfo debugInfo = ConfigManager.Config.DebugInfo;
			debugInfo.TraceAutoRefresh = mnuAutoRefresh.Checked;
			debugInfo.TraceLineCount = _lineCount;
			debugInfo.TraceIndentCode = chkIndentCode.Checked;
			debugInfo.TraceLoggerSize = this.WindowState == FormWindowState.Maximized ? this.RestoreBounds.Size : this.Size;
			_entityBinder.Entity = debugInfo.TraceLoggerOptions;
			_entityBinder.UpdateObject();
			debugInfo.TraceLoggerOptions = (TraceLoggerOptions)_entityBinder.Entity;

			ConfigManager.ApplyChanges();

			if(_loggingEnabled) {
				InteropEmu.DebugStopTraceLogger();
			}
		}

		private void SetOptions()
		{
			_entityBinder.Entity = ConfigManager.Config.DebugInfo.TraceLoggerOptions;
			_entityBinder.UpdateObject();
			TraceLoggerOptions options = (TraceLoggerOptions)_entityBinder.Entity;
			options.Condition = Encoding.UTF8.GetBytes(txtCondition.Text);
			Array.Resize(ref options.Condition, 1000);
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


		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch(keyData) {
				case Keys.C | Keys.Control:
					if(_previousTrace != null) {
						string[] lines = _previousTrace.Split('\n');
						StringBuilder sb = new StringBuilder();
						for(int i = txtTraceLog.SelectionStart, end = txtTraceLog.SelectionStart + txtTraceLog.SelectionLength; i <= end; i++) {
							sb.AppendLine(lines[i]);
						}
						Clipboard.SetText(sb.ToString());
					}
					return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void RefreshLog(bool scrollToBottom)
		{
			if(_refreshRunning) {
				return;
			}

			//Make sure labels are up to date
			DebugWorkspaceManager.GetWorkspace();

			_refreshRunning = true;
			SetOptions();
			Task.Run(() => {
				//Update trace log in another thread for performance
				DebugState state = new DebugState();
				InteropEmu.DebugGetState(ref state);
				if(_previousCycleCount != state.CPU.CycleCount) {
					string newTrace = InteropEmu.DebugGetExecutionTrace((UInt32)_lineCount);
					_previousCycleCount = state.CPU.CycleCount;
					_previousTrace = newTrace;

					int index = 0;
					string line = null;
					Func<bool> readLine = () => {
						if(index < newTrace.Length) {
							int endOfLineIndex = newTrace.IndexOf('\n', index);
							line = newTrace.Substring(index, endOfLineIndex - index);
							index = endOfLineIndex + 1;
							return true;
						} else {
							return false;
						}
					};

					List<int> programCounter = new List<int>(30000);
					List<string> byteCode = new List<string>(30000);
					List<string> lineContent = new List<string>(30000);
					List<int> indent = new List<int>(30000);

					char[] splitter = new char[] { ' ' };
					while(readLine()) {
						programCounter.Add(Int32.Parse(line.Substring(0, 4), System.Globalization.NumberStyles.HexNumber));
						byteCode.Add(line.Substring(6, 11));
						lineContent.Add(line.Substring(19));
						indent.Add(6);
					}
					this.BeginInvoke((Action)(() => {
						txtTraceLog.ShowContentNotes = chkShowByteCode.Checked;
						txtTraceLog.ShowSingleContentLineNotes = chkShowByteCode.Checked;

						txtTraceLog.LineIndentations = indent.ToArray();
						txtTraceLog.TextLines = lineContent.ToArray();
						txtTraceLog.LineNumbers = programCounter.ToArray();
						txtTraceLog.TextLineNotes = byteCode.ToArray();

						if(scrollToBottom) {
							txtTraceLog.ScrollToLineIndex(txtTraceLog.LineCount - 1);
						}
					}));
				}
				_refreshRunning = false;
			});
		}

		private void UpdateMenu()
		{
			mnu30000Lines.Checked = _lineCount == 30000;
			mnu10000Lines.Checked = _lineCount == 10000;
			mnu1000Lines.Checked = _lineCount == 1000;
			mnu100Lines.Checked = _lineCount == 100;

			if(_lineCount >= 1000) {
				tmrUpdateLog.Interval = 250;
			} else {
				tmrUpdateLog.Interval = 150;
			}
		}

		private void tmrUpdateLog_Tick(object sender, EventArgs e)
		{
			if(txtCondition.Text.Length > 0) {
				EvalResultType resultType;
				InteropEmu.DebugEvaluateExpression(txtCondition.Text, out resultType);
				picExpressionWarning.Visible = (resultType == EvalResultType.Invalid);
			} else {
				picExpressionWarning.Visible = false;
			}

			if(mnuAutoRefresh.Checked) {
				RefreshLog(false);
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
			RefreshLog(false);
		}
	}
}
