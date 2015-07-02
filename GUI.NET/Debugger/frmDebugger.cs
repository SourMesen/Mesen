using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmDebugger : Form
	{
		InteropEmu.NotificationListener _notifListener;

		public frmDebugger()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			InteropEmu.DebugInitialize();
			InteropEmu.DebugStep(1);
		}

		void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.CodeBreak) {
				this.BeginInvoke((MethodInvoker)(() => UpdateDebugger()));
			}
		}

		private bool UpdateSplitView()
		{
			if(mnuSplitView.Checked) {
				tlpTop.ColumnStyles[1].SizeType = SizeType.Percent;
				tlpTop.ColumnStyles[1].Width = 50f;
				this.MinimumSize = new Size(1250, 650);
			} else {
				tlpTop.ColumnStyles[1].SizeType = SizeType.Absolute;
				tlpTop.ColumnStyles[1].Width = 0f;
				this.MinimumSize = new Size(1000, 650);
			}
			return mnuSplitView.Checked;
		}

		private void UpdateDebugger()
		{
			if(InteropEmu.DebugIsCodeChanged()) {
				string code = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(InteropEmu.DebugGetCode());
				ctrlDebuggerCode.Code = code;
				ctrlDebuggerCodeSplit.Code = code;
			}

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			if(UpdateSplitView()) {
				ctrlDebuggerCodeSplit.UpdateCode(true);
			}

			ctrlDebuggerCode.SelectActiveAddress(state.CPU.PC);
			ctrlConsoleStatus.UpdateStatus(ref state);
			ctrlWatch.UpdateWatch();
		}

		private void ToggleBreakpoint()
		{
			this.ctrlDebuggerCode.HighlightLine(this.ctrlDebuggerCode.GetCurrentLine(), Color.FromArgb(158, 84, 94), Color.White);

			//this.AddBreakpoint(this.GetLineAddress(lineIndex));
		}

		private void mnuContinue_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugRun();
			this.ctrlDebuggerCode.RemoveActiveHighlight();
		}

		private void frmDebugger_FormClosed(object sender, FormClosedEventArgs e)
		{
			InteropEmu.DebugRelease();
		}

		private void mnuToggleBreakpoint_Click(object sender, EventArgs e)
		{
			ToggleBreakpoint();
		}

		private void mnuBreak_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugStep(1);
		}

		private void mnuStepInto_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugStep(1);
		}

		private void mnuStepOut_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugStepOut();
		}
		
		private void mnuStepOver_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugStepOver();
		}

		private void mnuRunOneFrame_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugStepCycles(29780);
		}

		private void ctrlDebuggerCode_OnWatchAdded(WatchAddedEventArgs args)
		{
			this.ctrlWatch.AddWatch(args.Address);
		}

		private void mnuFind_Click(object sender, EventArgs e)
		{

		}

		private void mnuSplitView_Click(object sender, EventArgs e)
		{
			UpdateDebugger();
		}
	}
}
