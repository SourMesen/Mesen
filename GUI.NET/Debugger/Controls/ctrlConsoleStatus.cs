using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class ctrlConsoleStatus : UserControl
	{
		public event EventHandler OnStateChanged;
		public event EventHandler OnGotoLocation;

		DebugState _lastState;

		EntityBinder _cpuBinder = new EntityBinder();
		EntityBinder _ppuControlBinder = new EntityBinder();
		EntityBinder _ppuStatusBinder = new EntityBinder();

		public ctrlConsoleStatus()
		{
			InitializeComponent();

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				tmrButton.Enabled = true;
			}

			_cpuBinder.Entity = new CPUState();
			_ppuControlBinder.Entity = new PPUControlFlags();
			_ppuStatusBinder.Entity = new PPUStatusFlags();

			_cpuBinder.AddBinding("A", txtA);
			_cpuBinder.AddBinding("X", txtX);
			_cpuBinder.AddBinding("Y", txtY);
			_cpuBinder.AddBinding("PC", txtPC);
			_cpuBinder.AddBinding("SP", txtSP);
			_cpuBinder.AddBinding("PS", txtStatus);
			_cpuBinder.AddBinding("CycleCount", txtCycleCount);
			_cpuBinder.AddBinding("NMIFlag", chkNMI);
			
			_ppuStatusBinder.AddBinding("VerticalBlank", chkVerticalBlank);
			_ppuStatusBinder.AddBinding("Sprite0Hit", chkSprite0Hit);
			_ppuStatusBinder.AddBinding("SpriteOverflow", chkSpriteOverflow);

			_ppuControlBinder.AddBinding("BackgroundEnabled", chkBGEnabled);
			_ppuControlBinder.AddBinding("SpritesEnabled", chkSpritesEnabled);
			_ppuControlBinder.AddBinding("BackgroundMask", chkDrawLeftBG);
			_ppuControlBinder.AddBinding("SpriteMask", chkDrawLeftSpr);
			_ppuControlBinder.AddBinding("VerticalWrite", chkVerticalWrite);
			_ppuControlBinder.AddBinding("VBlank", chkNMIOnBlank);
			_ppuControlBinder.AddBinding("LargeSprites", chkLargeSprites);
			_ppuControlBinder.AddBinding("Grayscale", chkGrayscale);
			_ppuControlBinder.AddBinding("IntensifyRed", chkIntensifyRed);
			_ppuControlBinder.AddBinding("IntensifyGreen", chkIntensifyGreen);
			_ppuControlBinder.AddBinding("IntensifyBlue", chkIntensifyBlue);
			_ppuControlBinder.AddBinding("BackgroundEnabled", chkBGEnabled);
			_ppuControlBinder.AddBinding("BackgroundEnabled", chkBGEnabled);

			_ppuControlBinder.AddBinding("BackgroundPatternAddr", txtBGAddr);
			_ppuControlBinder.AddBinding("SpritePatternAddr", txtSprAddr);
		}

		private void UpdateCPUStatus(ref CPUState state)
		{
			_cpuBinder.Entity = state;
			_cpuBinder.UpdateUI();

			UpdateCpuFlags();

			chkExternal.Checked = state.IRQFlag.HasFlag(IRQSource.External);
			chkFrameCounter.Checked = state.IRQFlag.HasFlag(IRQSource.FrameCounter);
			chkDMC.Checked = state.IRQFlag.HasFlag(IRQSource.DMC);
		}

		private void UpdatePPUStatus(ref PPUDebugState state)
		{
			_ppuControlBinder.Entity = state.ControlFlags;
			_ppuStatusBinder.Entity = state.StatusFlags;
			_ppuControlBinder.UpdateUI();
			_ppuStatusBinder.UpdateUI();

			txtCycle.Text = state.Cycle.ToString();
			txtScanline.Text = state.Scanline.ToString();

			txtVRAMAddr.Text = state.State.VideoRamAddr.ToString("X4");
			txtNTAddr.Text = (0x2000 | (state.State.VideoRamAddr & 0x0FFF)).ToString("X4");
		}

		private void UpdateStack(UInt16 stackPointer)
		{
			lstStack.Items.Clear();
			for(UInt32 i = (UInt32)0x100 + stackPointer; i <  0x200; i++) {
				lstStack.Items.Add("$" + InteropEmu.DebugGetMemoryValue(i).ToString("X2"));
			}
		}

		public void UpdateStatus(ref DebugState state)
		{
			_lastState = state;
			UpdateCPUStatus(ref state.CPU);
			UpdatePPUStatus(ref state.PPU);
			UpdateStack(state.CPU.SP);
			btnApplyChanges.Enabled = true;
		}

		private void btnApplyChanges_Click(object sender, EventArgs e)
		{
			_cpuBinder.UpdateObject();
			_ppuControlBinder.UpdateObject();
			_ppuStatusBinder.UpdateObject();

			DebugState state = _lastState;
			state.CPU = (CPUState)_cpuBinder.Entity;

			if(chkExternal.Checked) state.CPU.IRQFlag |= IRQSource.External;
			if(chkFrameCounter.Checked) state.CPU.IRQFlag |= IRQSource.FrameCounter;
			if(chkDMC.Checked) state.CPU.IRQFlag |= IRQSource.DMC;

			state.PPU.ControlFlags = (PPUControlFlags)_ppuControlBinder.Entity;
			state.PPU.StatusFlags = (PPUStatusFlags)_ppuStatusBinder.Entity;

			state.PPU.State.Mask = state.PPU.ControlFlags.GetMask();
			state.PPU.State.Control = state.PPU.ControlFlags.GetControl();
			state.PPU.State.Status = state.PPU.StatusFlags.GetStatus();

			UInt32 cycle = 0;
			UInt32.TryParse(txtCycle.Text, out cycle);
			state.PPU.Cycle = cycle;

			Int32 scanline = 0;
			Int32.TryParse(txtScanline.Text, out scanline);
			state.PPU.Scanline = scanline;

			UInt16 vramAddr = 0;
			UInt16.TryParse(txtVRAMAddr.Text, System.Globalization.NumberStyles.HexNumber, null, out vramAddr);
			state.PPU.State.VideoRamAddr = vramAddr;

			InteropEmu.DebugSetState(state);
			btnApplyChanges.Enabled = false;
			OnStateChanged?.Invoke(null, null);
		}

		private void chkCpuFlag_Click(object sender, EventArgs e)
		{
			int ps = 0;
			if(chkBreak.Checked) ps |= (int)PSFlags.Break;
			if(chkCarry.Checked) ps |= (int)PSFlags.Carry;
			if(chkDecimal.Checked) ps |= (int)PSFlags.Decimal;
			if(chkInterrupt.Checked) ps |= (int)PSFlags.Interrupt;
			if(chkNegative.Checked) ps |= (int)PSFlags.Negative;
			if(chkOverflow.Checked) ps |= (int)PSFlags.Overflow;
			if(chkReserved.Checked) ps |= (int)PSFlags.Reserved;
			if(chkZero.Checked) ps |= (int)PSFlags.Zero;
			txtStatus.Text = ps.ToString("X2");
		}

		private void UpdateCpuFlags()
		{
			PSFlags flags = (PSFlags)((CPUState)_cpuBinder.Entity).PS;
			chkBreak.Checked = flags.HasFlag(PSFlags.Break);
			chkCarry.Checked = flags.HasFlag(PSFlags.Carry);
			chkDecimal.Checked = flags.HasFlag(PSFlags.Decimal);
			chkInterrupt.Checked = flags.HasFlag(PSFlags.Interrupt);
			chkNegative.Checked = flags.HasFlag(PSFlags.Negative);
			chkOverflow.Checked = flags.HasFlag(PSFlags.Overflow);
			chkReserved.Checked = flags.HasFlag(PSFlags.Reserved);
			chkZero.Checked = flags.HasFlag(PSFlags.Zero);
		}

		private void txtStatus_TextChanged(object sender, EventArgs e)
		{
			if(!_cpuBinder.Updating) {
				_cpuBinder.UpdateObject();
				UpdateCpuFlags();
			}
		}

		private void tmrButton_Tick(object sender, EventArgs e)
		{
			btnApplyChanges.Enabled = InteropEmu.DebugIsExecutionStopped();
		}

		private void UpdateVectorAddresses()
		{
			int nmiHandler = InteropEmu.DebugGetMemoryValue(0xFFFA) | (InteropEmu.DebugGetMemoryValue(0xFFFB) << 8);
			int resetHandler = InteropEmu.DebugGetMemoryValue(0xFFFC) | (InteropEmu.DebugGetMemoryValue(0xFFFD) << 8);
			int irqHandler = InteropEmu.DebugGetMemoryValue(0xFFFE) | (InteropEmu.DebugGetMemoryValue(0xFFFF) << 8);

			mnuGoToNmiHandler.Text = "NMI Handler ($" + nmiHandler.ToString("X4") + ")";
			mnuGoToResetHandler.Text = "Reset Handler ($" + resetHandler.ToString("X4") + ")";
			mnuGoToIrqHandler.Text = "IRQ Handler ($" + irqHandler.ToString("X4") + ")";
		}


		private void btnGoto_Click(object sender, EventArgs e)
		{
			contextGoTo.Show(btnGoto.PointToScreen(new Point(0, btnGoto.Height-1)));
		}

		private void mnuGoToIrqHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(0xFFFF) << 8) | InteropEmu.DebugGetMemoryValue(0xFFFE);
			this.OnGotoLocation?.Invoke(address, null);
		}

		private void mnuGoToNmiHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(0xFFFB) << 8) | InteropEmu.DebugGetMemoryValue(0xFFFA);
			this.OnGotoLocation?.Invoke(address, null);
		}

		private void mnuGoToResetHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(0xFFFD) << 8) | InteropEmu.DebugGetMemoryValue(0xFFFC);
			this.OnGotoLocation?.Invoke(address, null);
		}

		private void contextGoTo_Opening(object sender, CancelEventArgs e)
		{
			UpdateVectorAddresses();
		}
	}
}
