using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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

			switch(breakpoint.AddressType) {
				case BreakpointAddressType.AnyAddress: radAnyAddress.Checked = true; break;
				case BreakpointAddressType.SingleAddress: radSpecificAddress.Checked = true; break;
				case BreakpointAddressType.AddressRange: radRange.Checked = true; break;
			}

			AddBinding("MemoryType", cboBreakpointType);
			AddBinding("Enabled", chkEnabled);
			AddBinding("Address", txtAddress);
			AddBinding("StartAddress", txtFrom);
			AddBinding("EndAddress", txtTo);
			AddBinding("BreakOnRead", chkRead);
			AddBinding("BreakOnWrite", chkWrite);
			AddBinding("BreakOnExec", chkExec);
			AddBinding("Condition", txtCondition);

			cboBreakpointType.Items.Clear();
			cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.CpuMemory));
			cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.PpuMemory));
			cboBreakpointType.Items.Add("-");

			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.PrgRom) > 0) {
				cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.PrgRom));
			}
			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.WorkRam) > 0) {
				cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.WorkRam));
			}
			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.SaveRam) > 0) {
				cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.SaveRam));
			}

			if(cboBreakpointType.Items.Count > 3) {
				cboBreakpointType.Items.Add("-");
			}

			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRom) > 0) {
				cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.ChrRom));
			}

			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRam) > 0) {
				cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.ChrRam));
			}

			cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.PaletteMemory));

			this.toolTip.SetToolTip(this.picExpressionWarning, "Condition contains invalid syntax or symbols.");
			this.toolTip.SetToolTip(this.picHelp, frmBreakpoint.GetConditionTooltip(false));
		}
		
		public static string GetConditionTooltip(bool forWatch)
		{
			string tooltip =
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
				"IsRead: True if the CPU is reading from a memory address" + Environment.NewLine +
				"IsWrite: True if the CPU is writing to a memory address" + Environment.NewLine;

			if(!forWatch) {
				tooltip +=
					"Address: Current CPU memory address being read/written" + Environment.NewLine +
					"RomAddress: Current ROM address being read/written" + Environment.NewLine;
			}

			tooltip +=
				"[<address>]: (Byte) Memory value at <address> (CPU)" + Environment.NewLine +
				"{<address>}: (Word) Memory value at <address> (CPU)" + Environment.NewLine + Environment.NewLine +

				"Examples:" + Environment.NewLine +
				"a == 10 || x == $23" + Environment.NewLine +
				"scanline == 10 && (cycle >= 55 && cycle <= 100)" + Environment.NewLine +
				"x == [$150] || y == [10]" + Environment.NewLine +
				"[[$15] + y]   -> Reads the value at address $15, adds Y to it and reads the value at the resulting address." + Environment.NewLine +
				"{$FFFA}  -> Returns the NMI handler's address.";

			return tooltip;
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
			if(txtCondition.Text.Trim().Length > 0) {
				EvalResultType resultType;
				InteropEmu.DebugEvaluateExpression(txtCondition.Text.Replace(Environment.NewLine, " "), out resultType);
				if(resultType == EvalResultType.Invalid) {
					picExpressionWarning.Visible = true;
					return false;
				}
			}
			picExpressionWarning.Visible = false;

			DebugMemoryType type = cboBreakpointType.GetEnumValue<DebugMemoryType>();
			

			int maxValue = InteropEmu.DebugGetMemorySize(type) - 1;

			if(radRange.Checked) {
				int start = 0, end = 0;
				int.TryParse(txtFrom.Text, NumberStyles.HexNumber, null, out start);
				int.TryParse(txtTo.Text, NumberStyles.HexNumber, null, out end);
				if(end < start || end > maxValue || start > maxValue) {
					return false;
				}
			}

			int address;
			if(int.TryParse(txtAddress.Text, NumberStyles.HexNumber, null, out address)) {
				if(address > maxValue) {
					return false;
				}
			}

			bool isCpuBreakpoint = new Breakpoint() { MemoryType = type }.IsCpuBreakpoint;
			return chkRead.Checked || chkWrite.Checked || (chkExec.Checked && isCpuBreakpoint) || txtCondition.Text.Length > 0;
		}

		private void txtAddress_Enter(object sender, EventArgs e)
		{
			radSpecificAddress.Checked = true;
		}

		private void txtFrom_Enter(object sender, EventArgs e)
		{
			radRange.Checked = true;
		}

		private void txtTo_Enter(object sender, EventArgs e)
		{
			radRange.Checked = true;
		}


		private void cboBreakpointType_SelectedIndexChanged(object sender, EventArgs e)
		{
			DebugMemoryType type = cboBreakpointType.GetEnumValue<DebugMemoryType>();

			bool isCpuBreakpoint = new Breakpoint() { MemoryType = type }.IsCpuBreakpoint;
			chkExec.Visible = isCpuBreakpoint;

			string maxValue = (InteropEmu.DebugGetMemorySize(type) - 1).ToString("X2");
			string minValue = "".PadLeft(maxValue.Length, '0');

			lblRange.Text = $"(range: ${minValue}-${maxValue})";
		}
	}
}
