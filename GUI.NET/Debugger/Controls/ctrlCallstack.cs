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
			InteropEmu.DebugGetCallstack(out _absoluteCallstack, out _relativeCallstack);
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			_programCounter = state.CPU.DebugPC;

			this.lstCallstack.Items.Clear();
			int subStartAddr = -1;
			for(int i = 0, len = _relativeCallstack.Length; i < len; i+=2) {
				int jsrAddr = _relativeCallstack[i];
				bool unmappedAddress = false;
				if(subStartAddr >= 0) {
					unmappedAddress = ((subStartAddr & 0x10000) == 0x10000);
					if(unmappedAddress) {
						subStartAddr &= 0xFFFF;
						jsrAddr &= 0xFFFF;
					}
				}

				string startAddr = subStartAddr >= 0 ? subStartAddr.ToString("X4") : "----";
				if(_relativeCallstack[i] == -2) {
					break;
				}
				subStartAddr = _relativeCallstack[i+1];
				ListViewItem item = this.lstCallstack.Items.Insert(0, "$" + startAddr + " @ $" + jsrAddr.ToString("X4") + "  [$" + _absoluteCallstack[i].ToString("X4") + "]");
				if(unmappedAddress) {
					item.ForeColor = Color.Gray;
					item.Font = new Font(item.Font, FontStyle.Italic);
				}
			}
			this.lstCallstack.Items.Insert(0, "$" + subStartAddr.ToString("X4") + " @ $" + _programCounter.ToString("X4"));
		}

		private void lstCallstack_DoubleClick(object sender, EventArgs e)
		{
			if(this.lstCallstack.SelectedIndices.Count > 0) {
				if(this.lstCallstack.SelectedIndices[0] == 0) {
					this.FunctionSelected(_programCounter, null);
				} else {
					Int32 address = _relativeCallstack[(this.lstCallstack.Items.Count - 1 - this.lstCallstack.SelectedIndices[0]) * 2];
					if(this.FunctionSelected != null) {
						this.FunctionSelected(address, null);
					}
				}
			}
		}
	}
}
