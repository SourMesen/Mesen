﻿using System;
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
			AddBinding("MarkEvent", chkMarkOnEventViewer);
			AddBinding("ProcessDummyReadWrites", chkProcessDummyReadWrites);
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

			cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.NametableRam));
			cboBreakpointType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.PaletteMemory));

			this.toolTip.SetToolTip(this.picExpressionWarning, "Condition contains invalid syntax or symbols.");
			this.toolTip.SetToolTip(this.picHelp, frmBreakpoint.GetConditionTooltip(false));
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			Breakpoint bp = (Breakpoint)this.Entity;
			if(!BreakpointManager.Breakpoints.Contains(bp)) {
				//This is a new breakpoint, make sure address fields are empty instead of 0
				if(bp.Address == UInt32.MaxValue) {
					txtAddress.Text = "";
				}
				if(txtFrom.Text == "0") {
					txtFrom.Text = "";
				}
				if(txtTo.Text == "0") {
					txtTo.Text = "";
				}
			}
			if(bp.Address == 0 && bp.AddressType != BreakpointAddressType.SingleAddress) {
				txtAddress.Text = "";
			}
			if(bp.StartAddress == 0 && bp.EndAddress == 0 && bp.AddressType != BreakpointAddressType.AddressRange) {
				txtFrom.Text = "";
				txtTo.Text = "";
			}

			if(bp.AddressType == BreakpointAddressType.AddressRange) {
				txtFrom.Focus();
				txtFrom.SelectionStart = 0;
				txtFrom.SelectionLength = 0;
			} else if(bp.AddressType == BreakpointAddressType.SingleAddress) {
				txtAddress.Focus();
				txtAddress.SelectionStart = 0;
				txtAddress.SelectionLength = 0;
			}
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
				"PreviousOpPC: Address of the previous instruction's first byte" + Environment.NewLine +
				"Irq/Nmi/Sprite0Hit/SpriteOverflow/VerticalBlank: True if the corresponding flags are set" + Environment.NewLine +
				"Branched: True if the current instruction was reached by jumping or branching" + Environment.NewLine +
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
				InteropEmu.DebugEvaluateExpression(txtCondition.Text.Replace(Environment.NewLine, " "), out resultType, false);
				if(resultType == EvalResultType.Invalid) {
					picExpressionWarning.Visible = true;
					return false;
				}
			}
			picExpressionWarning.Visible = false;

			DebugMemoryType type = cboBreakpointType.GetEnumValue<DebugMemoryType>();
			int maxValue = InteropEmu.DebugGetMemorySize(type) - 1;

			if(radSpecificAddress.Checked) {
				if(ValidateAddress(txtAddress, maxValue) < 0) {
					return false;
				}
			} else if(radRange.Checked) {
				int start = ValidateAddress(txtFrom, maxValue);
				int end = ValidateAddress(txtTo, maxValue);
				
				if(start < 0 || end < 0 || end < start) {
					return false;
				}
			}

			return chkRead.Checked || chkWrite.Checked || (chkExec.Checked && Breakpoint.IsTypeCpuBreakpoint(type)) || txtCondition.Text.Length > 0;
		}

		private int ValidateAddress(TextBox field, int maxValue)
		{
			int value = -1;
			if(!int.TryParse(field.Text, NumberStyles.HexNumber, null, out value) || value > maxValue) {
				field.ForeColor = ThemeHelper.Theme.ErrorTextColor;
				value = -1;
			} else {
				field.ForeColor = ThemeHelper.Theme.TextBoxForeColor;
			}
			return value;
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

			chkExec.Visible = Breakpoint.IsTypeCpuBreakpoint(type);
			chkProcessDummyReadWrites.Visible = Breakpoint.IsTypeCpuBreakpoint(type);

			string maxValue = (InteropEmu.DebugGetMemorySize(type) - 1).ToString("X2");
			string minValue = "".PadLeft(maxValue.Length, '0');

			lblRange.Text = $"(range: ${minValue}-${maxValue})";
		}

		private void UpdateDummyReadWriteCheckbox()
		{
			if(chkRead.Checked || chkWrite.Checked) {
				chkProcessDummyReadWrites.Enabled = true;
			} else {
				chkProcessDummyReadWrites.Enabled = false;
				chkProcessDummyReadWrites.Checked = false;
			}
		}

		private void chkRead_CheckedChanged(object sender, EventArgs e)
		{
			UpdateDummyReadWriteCheckbox();
		}

		private void chkWrite_CheckedChanged(object sender, EventArgs e)
		{
			UpdateDummyReadWriteCheckbox();
		}
	}
}
