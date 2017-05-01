using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmBreakpoint : BaseConfigForm
	{
		public frmBreakpoint(Breakpoint breakpoint)
		{
			InitializeComponent();

			Entity = breakpoint;

			if(breakpoint.Type >= BreakpointType.ReadVram) {
				radPpu.Checked = true;
			} else {
				radCpu.Checked = true;
			}

			switch(breakpoint.AddressType) {
				case BreakpointAddressType.AnyAddress: radAnyAddress.Checked = true; break;
				case BreakpointAddressType.SingleAddress: radSpecificAddress.Checked = true; break;
				case BreakpointAddressType.AddressRange: radRange.Checked = true; break;
			}

			AddBinding("Enabled", chkEnabled);
			AddBinding("IsAbsoluteAddress", chkAbsolute);
			AddBinding("Address", txtAddress);
			AddBinding("StartAddress", txtFrom);
			AddBinding("EndAddress", txtTo);
			AddBinding("BreakOnRead", chkRead);
			AddBinding("BreakOnWrite", chkWrite);
			AddBinding("BreakOnExec", chkExec);
			AddBinding("BreakOnReadVram", chkReadVram);
			AddBinding("BreakOnWriteVram", chkWriteVram);
			AddBinding("Condition", txtCondition);

			this.toolTip.SetToolTip(this.picExpressionWarning, "Condition contains invalid syntax or symbols.");

			this.toolTip.SetToolTip(this.chkAbsolute, "Check to set an absolute address based on the exact address in PRG/CHR ROM (not CPU/PPU memory)");
			this.toolTip.SetToolTip(this.picHelp,
				"Most expressions/operators are accepted (C++ syntax)." + Environment.NewLine +
				"Note: Use the $ prefix to denote hexadecimal values." + Environment.NewLine + 
				"Note 2: Labels assigned to the code can be used (their value will match the label's address in CPU memory)." + Environment.NewLine + Environment.NewLine +
				"A/X/Y/PS/SP: Value of registers" + Environment.NewLine +
				"PC: Program Counter" + Environment.NewLine +
				"OpPC: Address of the current instruction's first byte" + Environment.NewLine +
				"Irq/Nmi: True if the Irq/Nmi flags are set" + Environment.NewLine +
				"Cycle/Scanline: Current cycle (0-340)/scanline(-1 to 260) of the PPU" + Environment.NewLine +
				"Frame: PPU frame number (since power on/reset)" + Environment.NewLine +
				"Value: Current value being read/written from/to memory" + Environment.NewLine +
				"Address: Current CPU memory address being read/written" + Environment.NewLine +
				"RomAddress: Current ROM address being read/written" + Environment.NewLine +
				"[<address>]: (Byte) Memory value at <address> (CPU)" + Environment.NewLine +
				"{<address>}: (Word) Memory value at <address> (CPU)" + Environment.NewLine + Environment.NewLine +

				"Examples:" + Environment.NewLine +
				"a == 10 || x == $23" + Environment.NewLine +
				"scanline == 10 && (cycle >= 55 && cycle <= 100)" + Environment.NewLine +
				"x == [$150] || y == [10]" + Environment.NewLine +
				"[[$15] + y]   -> Reads the value at address $15, adds Y to it and reads the value at the resulting address." + Environment.NewLine +
				"{$FFFA}  -> Returns the NMI handler's address."
			);
		}

		protected override void UpdateConfig()
		{
			base.UpdateConfig();

			if(radAnyAddress.Checked) {
				((Breakpoint)Entity).AddressType = BreakpointAddressType.AnyAddress;
			} else if(radSpecificAddress.Checked) {
				((Breakpoint)Entity).AddressType = BreakpointAddressType.SingleAddress;
			} else if(radRange.Checked) {
				((Breakpoint)Entity).AddressType = BreakpointAddressType.AddressRange;
			}
		}

		protected override bool ValidateInput()
		{
			if(txtCondition.Text.Length > 0) {
				EvalResultType resultType;
				InteropEmu.DebugEvaluateExpression(txtCondition.Text, out resultType);
				if(resultType == EvalResultType.Invalid) {
					picExpressionWarning.Visible = true;
					return false;
				}
			}
			picExpressionWarning.Visible = false;

			return chkRead.Checked || chkWrite.Checked || chkExec.Checked || chkReadVram.Checked || chkWriteVram.Checked || txtCondition.Text.Length > 0;
		}

		private void txtAddress_Enter(object sender, EventArgs e)
		{
			radSpecificAddress.Checked = true;
		}

		private void chkWriteVram_Enter(object sender, EventArgs e)
		{
			radPpu.Checked = true;
		}

		private void chkRead_Enter(object sender, EventArgs e)
		{
			radCpu.Checked = true;
		}

		private void radCpu_CheckedChanged(object sender, EventArgs e)
		{
			if(radCpu.Checked) {
				chkReadVram.Checked = false;
				chkWriteVram.Checked = false;
			}
		}

		private void radPpu_CheckedChanged(object sender, EventArgs e)
		{
			if(radPpu.Checked) {
				chkRead.Checked = false;
				chkWrite.Checked = false;
				chkExec.Checked = false;
			}
		}

		private void txtFrom_Enter(object sender, EventArgs e)
		{
			radRange.Checked = true;
		}

		private void txtTo_Enter(object sender, EventArgs e)
		{
			radRange.Checked = true;
		}
	}
}
