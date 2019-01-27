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
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger
{
	public partial class ctrlConsoleStatus : BaseControl
	{
		public event EventHandler OnStateChanged;
		public event EventHandler OnGotoLocation;

		private bool _dirty = false;
		private bool _preventDirty = false;
		private DebugState _lastState;
		private EntityBinder _cpuBinder = new EntityBinder();
		private EntityBinder _ppuControlBinder = new EntityBinder();
		private EntityBinder _ppuStatusBinder = new EntityBinder();

		public ctrlConsoleStatus()
		{
			InitializeComponent();
			ThemeHelper.FixMonoColors(contextGoTo);

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				tmrButton.Enabled = true;
			}

			if(Program.IsMono) {
				this.Font = new Font("Microsoft Sans Serif", 7.75F);
			}

			btnGoto.Image = BaseControl.DownArrow;

			_cpuBinder.Entity = new CPUState();
			_ppuControlBinder.Entity = new PPUControlFlags();
			_ppuStatusBinder.Entity = new PPUStatusFlags();

			_cpuBinder.AddBinding("A", txtA);
			_cpuBinder.AddBinding("X", txtX);
			_cpuBinder.AddBinding("Y", txtY);
			_cpuBinder.AddBinding("PC", txtPC);
			_cpuBinder.AddBinding("SP", txtSP);
			_cpuBinder.AddBinding("PS", txtStatus);
			_cpuBinder.AddBinding("CycleCount", txtCycleCount, eNumberFormat.Decimal);
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
			txtFrameCount.Text = state.FrameCount.ToString();

			txtVRAMAddr.Text = state.State.VideoRamAddr.ToString("X4");
			txtTmpAddr.Text = state.State.TmpVideoRamAddr.ToString("X4");
			txtNTAddr.Text = (0x2000 | (state.State.VideoRamAddr & 0x0FFF)).ToString("X4");

			chkWriteToggle.Checked = state.State.WriteToggle;
			txtXScroll.Text = state.State.XScroll.ToString();
		}

		private void UpdateStack(UInt16 stackPointer)
		{
			StringBuilder sb = new StringBuilder();
			for(UInt32 i = (UInt32)0x100 + stackPointer + 1; i < 0x200; i++) {
				sb.Append("$");
				sb.Append(InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, i).ToString("X2"));
				sb.Append(", ");
			}
			string stack = sb.ToString();
			if(stack.Length > 2) {
				stack = stack.Substring(0, stack.Length - 2);
			}
			txtStack.Text = stack;
		}

		public void UpdateStatus(ref DebugState state)
		{
			this._preventDirty = true;
			_lastState = state;
			UpdateCPUStatus(ref state.CPU);
			UpdatePPUStatus(ref state.PPU);
			UpdateStack(state.CPU.SP);

			btnUndo.Enabled = false;
			this._dirty = false;
			this._preventDirty = false;
		}

		public void ApplyChanges()
		{
			if(this._dirty) {
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
				
				UInt32 frameCount = 0;
				UInt32.TryParse(txtFrameCount.Text, out frameCount);
				state.PPU.FrameCount = frameCount;

				Int32 xScroll = 0;
				Int32.TryParse(txtXScroll.Text, out xScroll);
				state.PPU.State.XScroll = (byte)Math.Max(0, Math.Min(xScroll, 7));

				UInt16 vramAddr = 0;
				UInt16.TryParse(txtVRAMAddr.Text, System.Globalization.NumberStyles.HexNumber, null, out vramAddr);
				state.PPU.State.VideoRamAddr = vramAddr;

				UInt16 tmpVramAddr = 0;
				UInt16.TryParse(txtTmpAddr.Text, System.Globalization.NumberStyles.HexNumber, null, out tmpVramAddr);
				state.PPU.State.TmpVideoRamAddr = tmpVramAddr;

				state.PPU.State.WriteToggle = chkWriteToggle.Checked;

				InteropEmu.DebugSetState(state);
				_lastState = state;
				_dirty = false;
				btnUndo.Enabled = false;
				OnStateChanged?.Invoke(null, null);
			}
		}

		private void chkCpuFlag_Click(object sender, EventArgs e)
		{
			this.OnOptionChanged(sender, e);

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
			this.OnOptionChanged(sender, e);
			if(!_cpuBinder.Updating) {
				_cpuBinder.UpdateObject();
				UpdateCpuFlags();
			}
		}

		private void tmrButton_Tick(object sender, EventArgs e)
		{
			btnUndo.Enabled = this._dirty && InteropEmu.DebugIsExecutionStopped();
		}

		private void UpdateVectorAddresses()
		{
			bool isNsf = InteropEmu.GetRomInfo().Format == RomFormat.Nsf;
			if(isNsf) {
				NsfHeader header = InteropEmu.NsfGetHeader();
				mnuGoToInitHandler.Text = "Init Handler ($" + header.InitAddress.ToString("X4") + ")";
				mnuGoToPlayHandler.Text = "Play Handler ($" + header.PlayAddress.ToString("X4") + ")";
			} else {
				int nmiHandler = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFA) | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFB) << 8);
				int resetHandler = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFC) | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFD) << 8);
				int irqHandler = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFE) | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFF) << 8);

				mnuGoToNmiHandler.Text = "NMI Handler ($" + nmiHandler.ToString("X4") + ")";
				mnuGoToResetHandler.Text = "Reset Handler ($" + resetHandler.ToString("X4") + ")";
				mnuGoToIrqHandler.Text = "IRQ Handler ($" + irqHandler.ToString("X4") + ")";
			}

			mnuGoToInitHandler.Visible = isNsf;
			mnuGoToPlayHandler.Visible = isNsf;
			mnuGoToIrqHandler.Visible = !isNsf;
			mnuGoToNmiHandler.Visible = !isNsf;
			mnuGoToResetHandler.Visible = !isNsf;
		}

		private void btnGoto_Click(object sender, EventArgs e)
		{
			contextGoTo.Show(btnGoto.PointToScreen(new Point(0, btnGoto.Height-1)));
		}

		private void mnuGoToIrqHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFF) << 8) | InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFE);
			this.OnGotoLocation?.Invoke(address, null);
		}

		private void mnuGoToNmiHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFB) << 8) | InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFA);
			this.OnGotoLocation?.Invoke(address, null);
		}

		private void mnuGoToResetHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFD) << 8) | InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFC);
			this.OnGotoLocation?.Invoke(address, null);
		}

		private void mnuGoToInitHandler_Click(object sender, EventArgs e)
		{
			this.OnGotoLocation?.Invoke((int)InteropEmu.NsfGetHeader().InitAddress, null);
		}

		private void mnuGoToPlayHandler_Click(object sender, EventArgs e)
		{
			this.OnGotoLocation?.Invoke((int)InteropEmu.NsfGetHeader().PlayAddress, null);
		}
		
		private void mnuGoToProgramCounter_Click(object sender, EventArgs e)
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			this.OnGotoLocation((int)state.CPU.DebugPC, null);
		}

		private void contextGoTo_Opening(object sender, CancelEventArgs e)
		{
			UpdateVectorAddresses();
		}

		private void OnOptionChanged(object sender, EventArgs e)
		{
			if(InteropEmu.DebugIsExecutionStopped()) {
				if(!this._preventDirty) {
					this._dirty = true;
					this.btnUndo.Enabled = true;
				}
			} else {
				this.UpdateStatus(ref _lastState);
			}
		}

		private void btnUndo_Click(object sender, EventArgs e)
		{
			if(this._dirty) {
				this.UpdateStatus(ref _lastState);
				this.btnUndo.Enabled = false;
			}
		}
	}
}
