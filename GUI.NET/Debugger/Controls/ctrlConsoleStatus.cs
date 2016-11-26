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

namespace Mesen.GUI.Debugger
{
	public partial class ctrlConsoleStatus : UserControl
	{
		public ctrlConsoleStatus()
		{
			InitializeComponent();
		}

		private void UpdateCPUStatus(ref DebugState state)
		{
			txtA.Text = state.CPU.A.ToString("X2");
			txtX.Text = state.CPU.X.ToString("X2");
			txtY.Text = state.CPU.Y.ToString("X2");
			txtPC.Text = state.CPU.PC.ToString("X4");
			txtSP.Text = state.CPU.SP.ToString("X2");
			txtStatus.Text = state.CPU.PS.ToString("X2");
			txtCycleCount.Text = state.CPU.CycleCount.ToString();

			PSFlags flags = (PSFlags)state.CPU.PS;
			chkBreak.Checked = flags.HasFlag(PSFlags.Break);
			chkCarry.Checked = flags.HasFlag(PSFlags.Carry);
			chkDecimal.Checked = flags.HasFlag(PSFlags.Decimal);
			chkInterrupt.Checked = flags.HasFlag(PSFlags.Interrupt);
			chkNegative.Checked = flags.HasFlag(PSFlags.Negative);
			chkOverflow.Checked = flags.HasFlag(PSFlags.Overflow);
			chkReserved.Checked = flags.HasFlag(PSFlags.Reserved);
			chkZero.Checked = flags.HasFlag(PSFlags.Zero);

			chkExternal.Checked = state.CPU.IRQFlag.HasFlag(IRQSource.External);
			chkFrameCounter.Checked = state.CPU.IRQFlag.HasFlag(IRQSource.FrameCounter);
			chkDMC.Checked = state.CPU.IRQFlag.HasFlag(IRQSource.DMC);

			chkNMI.Checked = state.CPU.NMIFlag;
		}

		private void UpdatePPUStatus(ref DebugState state)
		{
			chkVerticalBlank.Checked = Convert.ToBoolean(state.PPU.StatusFlags.VerticalBlank);
			chkSprite0Hit.Checked = Convert.ToBoolean(state.PPU.StatusFlags.Sprite0Hit);
			chkSpriteOverflow.Checked = Convert.ToBoolean(state.PPU.StatusFlags.SpriteOverflow);

			chkBGEnabled.Checked = Convert.ToBoolean(state.PPU.ControlFlags.BackgroundEnabled);
			chkSpritesEnabled.Checked = Convert.ToBoolean(state.PPU.ControlFlags.SpritesEnabled);
			chkDrawLeftBG.Checked = Convert.ToBoolean(state.PPU.ControlFlags.BackgroundMask);
			chkDrawLeftSpr.Checked = Convert.ToBoolean(state.PPU.ControlFlags.SpriteMask);
			chkVerticalWrite.Checked = Convert.ToBoolean(state.PPU.ControlFlags.VerticalWrite);
			chkNMIOnBlank.Checked = Convert.ToBoolean(state.PPU.ControlFlags.VBlank);
			chkLargeSprites.Checked = Convert.ToBoolean(state.PPU.ControlFlags.LargeSprites);
			chkGrayscale.Checked = Convert.ToBoolean(state.PPU.ControlFlags.Grayscale);
			chkIntensifyRed.Checked = Convert.ToBoolean(state.PPU.ControlFlags.IntensifyRed);
			chkIntensifyGreen.Checked = Convert.ToBoolean(state.PPU.ControlFlags.IntensifyGreen);
			chkIntensifyBlue.Checked = Convert.ToBoolean(state.PPU.ControlFlags.IntensifyBlue);

			txtBGAddr.Text = state.PPU.ControlFlags.BackgroundPatternAddr.ToString("X4");
			txtSprAddr.Text = state.PPU.ControlFlags.SpritePatternAddr.ToString("X4");

			txtVRAMAddr.Text = state.PPU.State.VideoRamAddr.ToString("X4");
			txtCycle.Text = state.PPU.Cycle.ToString();
			txtScanline.Text = state.PPU.Scanline.ToString();
			txtNTAddr.Text = (0x2000 | (state.PPU.State.VideoRamAddr & 0x0FFF)).ToString("X4");
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
			UpdateCPUStatus(ref state);
			UpdatePPUStatus(ref state);
			UpdateStack(state.CPU.SP);
		}
	}
}
