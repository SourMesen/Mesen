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
		private List<frmMemoryViewer> _memoryViewers = new List<frmMemoryViewer>();
		private InteropEmu.NotificationListener _notifListener;
		private ctrlDebuggerCode _lastCodeWindow;

		public frmDebugger()
		{
			InitializeComponent();

			_lastCodeWindow = ctrlDebuggerCode;
		}

		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);

			if(!DesignMode) {
				Icon = Properties.Resources.MesenIcon;
			}

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			InteropEmu.DebugInitialize();
			
			//Pause a few frames later to give the debugger a chance to disassemble some code
			InteropEmu.DebugStep(100000);
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
				tlpTop.ColumnStyles[0].Width = 50f;
				tlpTop.ColumnStyles[1].Width = 50f;
				this.MinimumSize = new Size(1250, 650);
			} else {
				tlpTop.ColumnStyles[1].SizeType = SizeType.Absolute;
				tlpTop.ColumnStyles[1].Width = 0f;
				this.MinimumSize = new Size(1000, 650);
			}
			ctrlDebuggerCodeSplit.Visible = mnuSplitView.Checked;
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
			} else {
				_lastCodeWindow = ctrlDebuggerCode;
			}

			ctrlDebuggerCode.SelectActiveAddress(state.CPU.DebugPC);
			ctrlDebuggerCodeSplit.SetActiveAddress(state.CPU.DebugPC);
			RefreshBreakpoints();

			ctrlConsoleStatus.UpdateStatus(ref state);
			ctrlWatch.UpdateWatch();
		}

		private void ClearActiveStatement()
		{
			ctrlDebuggerCode.ClearActiveAddress();
			RefreshBreakpoints();
		}

		private void ToggleBreakpoint()
		{
			ctrlBreakpoints.ToggleBreakpoint(_lastCodeWindow.GetCurrentLine());
		}
		
		private void RefreshBreakpoints()
		{
			ctrlDebuggerCodeSplit.HighlightBreakpoints(ctrlBreakpoints.GetBreakpoints());
			ctrlDebuggerCode.HighlightBreakpoints(ctrlBreakpoints.GetBreakpoints());
		}

		private void mnuContinue_Click(object sender, EventArgs e)
		{
			ClearActiveStatement();
			InteropEmu.DebugRun();
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
			_lastCodeWindow.OpenSearchBox();
		}
		
		private void mnuFindNext_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FindNext();
		}

		private void mnuFindPrev_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FindPrevious();
		}

		private void mnuSplitView_Click(object sender, EventArgs e)
		{
			UpdateDebugger();
		}

		private void mnuMemoryViewer_Click(object sender, EventArgs e)
		{
			frmMemoryViewer frm = new frmMemoryViewer();
			this._memoryViewers.Add(frm);
			frm.FormClosed += (obj, args) => {
				this._memoryViewers.Remove((frmMemoryViewer)obj);
			};
			frm.Show();
		}

		private void ctrlBreakpoints_BreakpointChanged(object sender, EventArgs e)
		{
			RefreshBreakpoints();
		}

		private void ctrlDebuggerCode_Enter(object sender, EventArgs e)
		{
			_lastCodeWindow = ctrlDebuggerCode;
		}

		private void ctrlDebuggerCodeSplit_Enter(object sender, EventArgs e)
		{
			_lastCodeWindow = ctrlDebuggerCodeSplit;
		}

		private void mnuGoTo_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.GoToAddress();
		}

		private void mnuIncreaseFontSize_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FontSize++;
		}

		private void mnuDecreaseFontSize_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FontSize--;
		}

		private void mnuResetFontSize_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FontSize = 13;
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			foreach(frmMemoryViewer frm in this._memoryViewers.ToArray()) {
				frm.Close();
			}
			base.OnFormClosed(e);
		}
	}
}
