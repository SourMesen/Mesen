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

			AddBinding("SpecificAddress", radSpecificAddress, radAnyAddress);
			AddBinding("Enabled", chkEnabled);
			AddBinding("IsAbsoluteAddress", chkAbsolute);
			AddBinding("Address", txtAddress);
			AddBinding("BreakOnRead", chkRead);
			AddBinding("BreakOnWrite", chkWrite);
			AddBinding("BreakOnExec", chkExec);
			AddBinding("BreakOnReadVram", chkReadVram);
			AddBinding("BreakOnWriteVram", chkWriteVram);
			AddBinding("Condition", txtCondition);

			this.toolTip.SetToolTip(this.chkAbsolute, "Check to set an absolute address based on the exact address in PRG/CHR ROM (not CPU/PPU memory)");
			this.toolTip.SetToolTip(this.picHelp,
				"Most expressions/operators are accepted (C++ syntax)." + Environment.NewLine +
				"Note: Use the $ prefix to denote hexadecimal values." + Environment.NewLine + Environment.NewLine +
				"A/X/Y/PS/SP: Value of registers" + Environment.NewLine +
				"Irq/Nmi: True if the Irq/Nmi flags are set" + Environment.NewLine +
				"Cycle/Scanline: Current cycle (0-340)/scanline(-1 to 260) of the PPU" + Environment.NewLine +
				"Value: Current value being read/written from/to memory" + Environment.NewLine +
				"Address: Current CPU memory address being read/written" + Environment.NewLine +
				"RomAddress: Current ROM address being read/written" + Environment.NewLine +
				"[<address>]: Value at address (CPU)" + Environment.NewLine + Environment.NewLine +

				"Examples:" + Environment.NewLine +
				"a == 10 || x == $23" + Environment.NewLine +
				"scanline == 10 && (cycle >= 55 && cycle <= 100)" + Environment.NewLine +
				"x == [$150] || y == [10]" + Environment.NewLine +
				"[[$15] + y]   -> Reads the value at address $15, adds Y to it and reads the value at the resulting address."
			);
		}

		protected override bool ValidateInput()
		{
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
	}
}
