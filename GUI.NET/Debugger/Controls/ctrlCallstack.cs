using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlCallstack : UserControl
	{
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
			int nmiHandler = InteropEmu.DebugGetMemoryValue(0xFFFA) | (InteropEmu.DebugGetMemoryValue(0xFFFB) << 8);
			int irqHandler = InteropEmu.DebugGetMemoryValue(0xFFFE) | (InteropEmu.DebugGetMemoryValue(0xFFFF) << 8);

			InteropEmu.DebugGetCallstack(out _absoluteCallstack, out _relativeCallstack);
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			_programCounter = state.CPU.DebugPC;

			this.lstCallstack.BeginUpdate();
			this.lstCallstack.Items.Clear();
			int relSubEntryAddr = -1, absSubEntryAddr = -1, relCurrentAddr = -1, relDestinationAddr = -1, absCurrentAddr = -1, absDestinationAddr = -1;
			ListViewItem item;
			for(int i = 0, len = _relativeCallstack.Length; i < len; i+=2) {
				if(_relativeCallstack[i] == -2) {
					break;
				}

				relSubEntryAddr = i == 0 ? -1 : _relativeCallstack[i-1] & 0xFFFF;
				absSubEntryAddr = i == 0 ? -1 : _absoluteCallstack[i-1];

				bool currentAddrUnmapped = (_relativeCallstack[i] & 0x10000) == 0x10000;
				relCurrentAddr = _relativeCallstack[i] & 0xFFFF;
				relDestinationAddr = _relativeCallstack[i+1] & 0xFFFF;
				absCurrentAddr = _absoluteCallstack[i];
				absDestinationAddr = _absoluteCallstack[i+1];

				item = this.lstCallstack.Items.Insert(0, this.GetFunctionName(relSubEntryAddr, absSubEntryAddr, nmiHandler, irqHandler));
				item.SubItems.Add("@ $" + relCurrentAddr.ToString("X4"));
				item.SubItems.Add("[$" + absCurrentAddr.ToString("X4") + "]");

				if(currentAddrUnmapped) {
					item.ForeColor = Color.Gray;
					item.Font = new Font(item.Font, FontStyle.Italic);
				}
			}

			item = this.lstCallstack.Items.Insert(0, this.GetFunctionName(relDestinationAddr, absDestinationAddr, nmiHandler, irqHandler));
			item.SubItems.Add("@ $" + _programCounter.ToString("X4"));
			item.SubItems.Add("[$" + InteropEmu.DebugGetAbsoluteAddress((UInt32)_programCounter).ToString("X4") + "]");
			if((relDestinationAddr & 0x10000) == 0x10000) {
				item.ForeColor = Color.Gray;
				item.Font = new Font(item.Font, FontStyle.Italic);
			}

			this.lstCallstack.EndUpdate();
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
