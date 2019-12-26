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

		private int GetHandlerTarget(uint address)
		{
			return InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, address) | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, address+1) << 8);
		}

		private void UpdateVectorAddresses()
		{
			RomFormat format = InteropEmu.GetRomInfo().Format;
			if(format == RomFormat.Nsf) {
				NsfHeader header = InteropEmu.NsfGetHeader();
				mnuGoToInitHandler.ShortcutKeyDisplayString = "$" + header.InitAddress.ToString("X4");
				mnuGoToPlayHandler.ShortcutKeyDisplayString = "$" + header.PlayAddress.ToString("X4");
			} else {
				mnuGoToNmiHandler.ShortcutKeyDisplayString = "$" + GetHandlerTarget(0xFFFA).ToString("X4");
				mnuGoToResetHandler.ShortcutKeyDisplayString = "$" + GetHandlerTarget(0xFFFC).ToString("X4");
				mnuGoToIrqHandler.ShortcutKeyDisplayString = "$" + GetHandlerTarget(0xFFFE).ToString("X4");

				if(format == RomFormat.Fds) {
					mnuFdsIrqHandler.ShortcutKeyDisplayString = "$" + GetHandlerTarget(0xDFFE).ToString("X4");
					mnuFdsNmiHandler1.ShortcutKeyDisplayString = "$" + GetHandlerTarget(0xDFF6).ToString("X4");
					mnuFdsNmiHandler2.ShortcutKeyDisplayString = "$" + GetHandlerTarget(0xDFF8).ToString("X4");
					mnuFdsNmiHandler3.ShortcutKeyDisplayString = "$" + GetHandlerTarget(0xDFFA).ToString("X4");
					mnuFdsResetHandler.ShortcutKeyDisplayString = "$" + GetHandlerTarget(0xDFFC).ToString("X4");
				}
			}

			mnuGoToInitHandler.Tag = mnuGoToInitHandler.Visible = format == RomFormat.Nsf;
			mnuGoToPlayHandler.Tag = mnuGoToPlayHandler.Visible = format == RomFormat.Nsf;
			mnuGoToIrqHandler.Tag = mnuGoToIrqHandler.Visible = format != RomFormat.Nsf;
			mnuGoToNmiHandler.Tag = mnuGoToNmiHandler.Visible = format != RomFormat.Nsf;
			mnuGoToResetHandler.Tag = mnuGoToResetHandler.Visible = format != RomFormat.Nsf;

			sepFds.Tag = sepFds.Visible = format == RomFormat.Fds;
			mnuFdsIrqHandler.Tag = mnuFdsIrqHandler.Visible = format == RomFormat.Fds;
			mnuFdsNmiHandler1.Tag = mnuFdsNmiHandler1.Visible = format == RomFormat.Fds;
			mnuFdsNmiHandler2.Tag = mnuFdsNmiHandler2.Visible = format == RomFormat.Fds;
			mnuFdsNmiHandler3.Tag = mnuFdsNmiHandler3.Visible = format == RomFormat.Fds;
			mnuFdsResetHandler.Tag = mnuFdsResetHandler.Visible = format == RomFormat.Fds;
		}

		private void btnGoto_Click(object sender, EventArgs e)
		{
			contextGoTo.Show(btnGoto.PointToScreen(new Point(0, btnGoto.Height-1)));
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

		public void CloneGoToMenu(ToolStripMenuItem parent)
		{
			parent.DropDownItems.Clear();
			UpdateVectorAddresses();
			List<ToolStripItem> items = new List<ToolStripItem>();
			foreach(ToolStripItem item in contextGoTo.Items) {
				if(item.Tag as bool? == false) {
					continue;
				}

				if(item is ToolStripMenuItem) {
					ToolStripMenuItem copy = new ToolStripMenuItem(item.Text);
					copy.ShortcutKeyDisplayString = (item as ToolStripMenuItem).ShortcutKeyDisplayString;
					copy.Click += (s, e) => item.PerformClick();
					items.Add(copy);
				} else if(item is ToolStripSeparator) {
					items.Add(new ToolStripSeparator());
				}
			}
			parent.DropDownItems.AddRange(items.ToArray());
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

		private void mnuGoToHandler_Click(object sender, EventArgs e)
		{
			int address = int.Parse((sender as ToolStripMenuItem).ShortcutKeyDisplayString.Substring(1), System.Globalization.NumberStyles.HexNumber);
			this.OnGotoLocation?.Invoke(address, EventArgs.Empty);
		}
	}
}
