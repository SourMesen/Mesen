using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlCallstack : BaseControl
	{
		private class StackInfo
		{
			public string SubName;
			public bool IsMapped;
			public int CurrentRelativeAddr;
			public int CurrentAbsoluteAddr;
		}

		public event EventHandler FunctionSelected;

		private StackFrameInfo[] _stackFrames;
		private Int32 _programCounter;

		public ctrlCallstack()
		{
			InitializeComponent();
		}

		public void UpdateCallstack()
		{
			List<StackInfo> stack = GetStackInfo();
			this.UpdateList(stack);
		}

		private List<StackInfo> GetStackInfo()
		{
			int nmiHandler = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFA) | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFB) << 8);
			int irqHandler = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFE) | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFF) << 8);

			_stackFrames = InteropEmu.DebugGetCallstack();
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			_programCounter = state.CPU.DebugPC;

			int relDestinationAddr = -1, absDestinationAddr = -1;

			List<StackInfo> stack = new List<StackInfo>();
			for(int i = 0, len = _stackFrames.Length; i < len; i++) {
				int relSubEntryAddr = i == 0 ? -1 : _stackFrames[i-1].JumpTarget;
				int absSubEntryAddr = i == 0 ? -1 : _stackFrames[i-1].JumpTargetAbsolute;

				stack.Add(new StackInfo() {
					SubName = this.GetFunctionName(relSubEntryAddr, absSubEntryAddr, nmiHandler, irqHandler),
					IsMapped = _stackFrames[i].JumpSourceAbsolute < 0 ? false : InteropEmu.DebugGetRelativeAddress((uint)_stackFrames[i].JumpSourceAbsolute, AddressType.PrgRom) >= 0,
					CurrentRelativeAddr = _stackFrames[i].JumpSource,
					CurrentAbsoluteAddr = _stackFrames[i].JumpSourceAbsolute
				});

				relDestinationAddr = _stackFrames[i].JumpTarget;
				absDestinationAddr = _stackFrames[i].JumpTargetAbsolute;
			}

			//Add current location
			stack.Add(new StackInfo() {
				SubName = this.GetFunctionName(relDestinationAddr, absDestinationAddr, nmiHandler, irqHandler),
				IsMapped = true,
				CurrentRelativeAddr = _programCounter,
				CurrentAbsoluteAddr = InteropEmu.DebugGetAbsoluteAddress((UInt32)_programCounter)
			});

			return stack;
		}

		private void UpdateList(List<StackInfo> stack)
		{
			this.lstCallstack.BeginUpdate();
			while(this.lstCallstack.Items.Count > stack.Count) {
				this.lstCallstack.Items.RemoveAt(this.lstCallstack.Items.Count - 1);
			}
			while(this.lstCallstack.Items.Count < stack.Count) {
				this.lstCallstack.Items.Add("").SubItems.AddRange(new string[] { "", "" });
			}

			for(int i = 0, len = stack.Count; i < len; i++) {
				StackInfo stackInfo = stack[i];
				ListViewItem item = this.lstCallstack.Items[len - i - 1];

				item.Text = stackInfo.SubName;
				item.SubItems[1].Text = "@ $" + stackInfo.CurrentRelativeAddr.ToString("X4");
				item.SubItems[2].Text = "[$" + stackInfo.CurrentAbsoluteAddr.ToString("X4") + "]";

				if(!stackInfo.IsMapped && item.ForeColor != ThemeHelper.Theme.LabelDisabledForeColor) {
					item.ForeColor = ThemeHelper.Theme.LabelDisabledForeColor;
					item.Font = new Font(item.Font, FontStyle.Italic);
				} else if(stackInfo.IsMapped && item.ForeColor != ThemeHelper.Theme.LabelForeColor) {
					item.ForeColor = ThemeHelper.Theme.LabelForeColor;
					item.Font = new Font(item.Font, FontStyle.Regular);
				}
			}
			this.lstCallstack.EndUpdate();
		}
		
		private string GetFunctionName(int relSubEntryAddr, int absSubEntryAddr, int nmiHandler, int irqHandler)
		{
			if(relSubEntryAddr < 0) {
				return "[bottom of stack]";
			}

			string funcName;
			CodeLabel label = absSubEntryAddr >= 0 ? LabelManager.GetLabel((UInt32)absSubEntryAddr, AddressType.PrgRom) : null;
			if(label != null) {
				funcName = label.Label + (relSubEntryAddr >= 0 ? (" ($" + relSubEntryAddr.ToString("X4") + ")") : "");
			} else {
				funcName = (relSubEntryAddr >= 0 ? ("$" + relSubEntryAddr.ToString("X4")) : "n/a");
			}

			if(relSubEntryAddr == nmiHandler) {
				funcName = "[nmi] " + funcName;
			} else if(relSubEntryAddr == irqHandler) {
				funcName = "[irq] " + funcName;
			}
			return funcName;
		}

		private void lstCallstack_DoubleClick(object sender, EventArgs e)
		{
			if(this.lstCallstack.SelectedIndices.Count > 0) {
				if(this.lstCallstack.SelectedIndices[0] == 0) {
					this.FunctionSelected(_programCounter, null);
				} else {
					StackFrameInfo stackFrameInfo = _stackFrames[(this.lstCallstack.Items.Count - 1 - this.lstCallstack.SelectedIndices[0])];
					this.FunctionSelected?.Invoke((int)stackFrameInfo.JumpSource, null);
				}
			}
		}
	}
}
