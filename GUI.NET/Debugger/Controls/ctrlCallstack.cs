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

		private Int32[] _absoluteCallstack;
		private Int32[] _relativeCallstack;
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
			int nmiHandler = InteropEmu.DebugGetMemoryValue(0xFFFA) | (InteropEmu.DebugGetMemoryValue(0xFFFB) << 8);
			int irqHandler = InteropEmu.DebugGetMemoryValue(0xFFFE) | (InteropEmu.DebugGetMemoryValue(0xFFFF) << 8);

			InteropEmu.DebugGetCallstack(out _absoluteCallstack, out _relativeCallstack);
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			_programCounter = state.CPU.DebugPC;

			int relDestinationAddr = -1, absDestinationAddr = -1;

			List<StackInfo> stack = new List<StackInfo>();
			for(int i = 0, len = _relativeCallstack.Length; i < len; i+=2) {
				if(_relativeCallstack[i] == -2) {
					break;
				}

				int relSubEntryAddr = i == 0 ? -1 : _relativeCallstack[i-1] & 0xFFFF;
				int absSubEntryAddr = i == 0 ? -1 : _absoluteCallstack[i-1];

				stack.Add(new StackInfo() {
					SubName = this.GetFunctionName(relSubEntryAddr, absSubEntryAddr, nmiHandler, irqHandler),
					IsMapped = (_relativeCallstack[i] & 0x10000) != 0x10000,
					CurrentRelativeAddr = _relativeCallstack[i] & 0xFFFF,
					CurrentAbsoluteAddr = _absoluteCallstack[i]
				});

				relDestinationAddr = _relativeCallstack[i+1] & 0xFFFF;
				absDestinationAddr = _absoluteCallstack[i+1];
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
			if(this.lstCallstack.Items.Count != stack.Count) {
				this.lstCallstack.Items.Clear();
				for(int i = 0, len = stack.Count; i < len; i++) {
					this.lstCallstack.Items.Add("").SubItems.AddRange(new string[] { "", "" });
				}
			}

			for(int i = 0, len = stack.Count; i < len; i++) {
				StackInfo stackInfo = stack[i];
				ListViewItem item = this.lstCallstack.Items[len - i - 1];

				item.Text = stackInfo.SubName;
				item.SubItems[1].Text = "@ $" + stackInfo.CurrentRelativeAddr.ToString("X4");
				item.SubItems[2].Text = "[$" + stackInfo.CurrentAbsoluteAddr.ToString("X4") + "]";

				if(!stackInfo.IsMapped && item.ForeColor != Color.Gray) {
					item.ForeColor = Color.Gray;
					item.Font = new Font(item.Font, FontStyle.Italic);
				} else if(stackInfo.IsMapped && item.ForeColor != Color.Black) {
					item.ForeColor = Color.Black;
					item.Font = new Font(item.Font, FontStyle.Regular);
				}
			}
		}

		private void UpdateList()
		{

		}

		private string GetFunctionName(int relSubEntryAddr, int absSubEntryAddr, int nmiHandler, int irqHandler)
		{
			if(relSubEntryAddr < 0) {
				return "[bottom of stack]";
			}

			string funcName;
			CodeLabel label = LabelManager.GetLabel((UInt32)absSubEntryAddr, AddressType.PrgRom);
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
					Int32 address = _relativeCallstack[(this.lstCallstack.Items.Count - 1 - this.lstCallstack.SelectedIndices[0]) * 2];
					if((address & 0x10000) == 0) {
						this.FunctionSelected?.Invoke(address & 0xFFFF, null);
					}
				}
			}
		}
	}
}
