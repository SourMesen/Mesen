using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmOpCodeTooltip : Form
	{
		static private Dictionary<string, OpCodeDesc> _descriptions;
		private Form _parentForm;

		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		static frmOpCodeTooltip()
		{
			//Descriptions taken from http://www.obelisk.me.uk/6502/reference.html

			_descriptions = new Dictionary<string, OpCodeDesc>();

			_descriptions.Add("adc", new OpCodeDesc("ADC - Add with Carry", "Add the value at the specified memory address to the accumulator + the carry bit. On overflow, the carry bit is set.", CpuFlag.Carry | CpuFlag.Zero | CpuFlag.Overflow | CpuFlag.Negative));
			_descriptions.Add("and", new OpCodeDesc("AND - Bitwise AND", "Perform an AND operation between the accumulator and the value at the specified memory address.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("asl", new OpCodeDesc("ASL - Arithmetic Shift Left", "Shifts all the bits of the accumulator (or the byte at the specified memory address) by 1 bit to the left. Bit 0 will be set to 0 and the carry flag will take the value of bit 7 (before the shift).", CpuFlag.Carry | CpuFlag.Zero | CpuFlag.Negative));

			_descriptions.Add("bcc", new OpCodeDesc("BCC - Branch if Carry Clear", "If the carry flag is clear, jump to location specified."));
			_descriptions.Add("bcs", new OpCodeDesc("BCS - Branch if Carry Set", "If the carry flag is set, jump to the location specified."));
			_descriptions.Add("beq", new OpCodeDesc("BEQ - Branch if Equal", "If the zero flag is set, jump to the location specified."));
			_descriptions.Add("bit", new OpCodeDesc("BIT - Bit Test", "Bits 6 and 7 of the byte at the specified memory address are copied to the N and V flags. If the accumulator's value ANDed with that byte is 0, the zero flag is set (otherwise it is cleared).", CpuFlag.Zero | CpuFlag.Overflow | CpuFlag.Negative));
			_descriptions.Add("bmi", new OpCodeDesc("BMI - Branch if Minus", "If the negative flag is set, jump to the location specified."));
			_descriptions.Add("bne", new OpCodeDesc("BNE - Branch if Not Equal", "If the zero flag is clear, jump to the location specified."));
			_descriptions.Add("bpl", new OpCodeDesc("BPL - Branch if Positive", "If the negative flag is clear, jump to the location specified."));
			_descriptions.Add("brk", new OpCodeDesc("BRK - Break", "The BRK instruction causes the CPU to jump to its IRQ vector, as if an interrupt had occurred. The PC and status flags are pushed on the stack."));
			_descriptions.Add("bvc", new OpCodeDesc("BVC - Branch if Overflow Clear", "If the overflow flag is clear, jump to the location specified."));
			_descriptions.Add("bvs", new OpCodeDesc("BVS - Branch if Overflow Set", "If the overflow flag is set then, jump to the location specified."));

			_descriptions.Add("clc", new OpCodeDesc("CLC - Clear Carry Flag", "Clears the carry flag.", CpuFlag.Carry));
			_descriptions.Add("cld", new OpCodeDesc("CLD - Clear Decimal Mode", "Clears the decimal mode flag.", CpuFlag.Decimal));
			_descriptions.Add("cli", new OpCodeDesc("CLI - Clear Interrupt Disable", "Clears the interrupt disable flag.", CpuFlag.Interrupt));
			_descriptions.Add("clv", new OpCodeDesc("CLV - Clear Overflow Flag", "Clears the overflow flag.", CpuFlag.Overflow));
			_descriptions.Add("cmp", new OpCodeDesc("CMP - Compare", "Compares the accumulator with the byte at the specified memory address..", CpuFlag.Zero | CpuFlag.Carry | CpuFlag.Negative));
			_descriptions.Add("cpx", new OpCodeDesc("CPX - Compare X Register", "Compares the X register with the byte at the specified memory address.", CpuFlag.Zero | CpuFlag.Carry | CpuFlag.Negative));
			_descriptions.Add("cpy", new OpCodeDesc("CPY - Compare Y Register", "Compares the Y register with the byte at the specified memory address.", CpuFlag.Zero | CpuFlag.Carry | CpuFlag.Negative));

			_descriptions.Add("dec", new OpCodeDesc("DEC - Decrement Memory", "Subtracts one from the byte at the specified memory address.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("dex", new OpCodeDesc("DEX - Decrement X Register", "Subtracts one from the X register.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("dey", new OpCodeDesc("DEY - Decrement Y Register", "Subtracts one from the Y register.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("eor", new OpCodeDesc("EOR - Exclusive OR", "Performs an exclusive OR operation between the accumulator and the byte at the specified memory address.", CpuFlag.Zero | CpuFlag.Negative));

			_descriptions.Add("inc", new OpCodeDesc("INC - Increment Memory", "Adds one to the the byte at the specified memory address.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("inx", new OpCodeDesc("INX - Increment X Register", "Adds one to the X register.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("iny", new OpCodeDesc("INY - Increment Y Register", "Adds one to the Y register.", CpuFlag.Zero | CpuFlag.Negative));

			_descriptions.Add("jmp", new OpCodeDesc("JMP - Jump", "Jumps to the specified location (alters the program counter)"));
			_descriptions.Add("jsr", new OpCodeDesc("JSR - Jump to Subroutine", "Pushes the address (minus one) of the next instruction to the stack and then jumps to the target address."));

			_descriptions.Add("lda", new OpCodeDesc("LDA - Load Accumulator", "Loads a byte from the specified memory address into the accumulator.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("ldx", new OpCodeDesc("LDX - Load X Register", "Loads a byte from the specified memory address into the X register.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("ldy", new OpCodeDesc("LDY - Load Y Register", "Loads a byte from the specified memory address into the Y register.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("lsr", new OpCodeDesc("LSR - Logical Shift Right", "Shifts all the bits of the accumulator (or the byte at the specified memory address) by 1 bit to the right. Bit 7 will be set to 0 and the carry flag will take the value of bit 0 (before the shift).", CpuFlag.Carry | CpuFlag.Zero | CpuFlag.Negative));

			_descriptions.Add("nop", new OpCodeDesc("NOP - No Operation", "Performs no operation other than delaying execution of the next instruction by 2 cycles."));

			_descriptions.Add("ora", new OpCodeDesc("ORA - Inclusive OR", "Performs an inclusive OR operation between the accumulator and the byte at the specified memory address.", CpuFlag.Zero | CpuFlag.Negative));

			_descriptions.Add("pha", new OpCodeDesc("PHA - Push Accumulator", "Pushes the value of the accumulator to the stack."));
			_descriptions.Add("php", new OpCodeDesc("PHP - Push Processor Status", "Pushes the value of the status flags to the stack."));
			_descriptions.Add("pla", new OpCodeDesc("PLA - Pull Accumulator", "Pulls a byte from the stack and stores it into the accumulator.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("plp", new OpCodeDesc("PLP - Pull Processor Status", "Pulls a byte from the stack and stores it into the processor flags.  The flags will be modified based on the value pulled.", CpuFlag.Carry | CpuFlag.Decimal | CpuFlag.Interrupt | CpuFlag.Negative | CpuFlag.Overflow | CpuFlag.Zero));

			_descriptions.Add("rol", new OpCodeDesc("ROL - Rotate Left", "Shifts all bits 1 position to the left. The right-most bit takes the current value of the carry flag. The left-most bit is stored into the carry flag.", CpuFlag.Carry | CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("ror", new OpCodeDesc("ROR - Rotate Right", "Shifts all bits 1 position to the right. The left-most bit takes the current value of the carry flag. The right-most bit is stored into the carry flag.", CpuFlag.Carry | CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("rti", new OpCodeDesc("RTI - Return from Interrupt", "The RTI instruction is used at the end of the interrupt handler to return execution to its original location.  It pulls the status flags and program counter from the stack."));
			_descriptions.Add("rts", new OpCodeDesc("RTS - Return from Subroutine", "The RTS instruction is used at the end of a subroutine to return execution to the calling function. It pulls the status flags and program counter (minus 1) from the stack."));

			_descriptions.Add("sbc", new OpCodeDesc("SBC - Subtract with Carry", "Substracts the byte at the specified memory address from the value of the accumulator (affected by the carry flag).", CpuFlag.Carry | CpuFlag.Zero | CpuFlag.Overflow | CpuFlag.Negative));
			_descriptions.Add("sec", new OpCodeDesc("SEC - Set Carry Flag", "Sets the carry flag.", CpuFlag.Carry));
			_descriptions.Add("sed", new OpCodeDesc("SED - Set Decimal Flag", "Sets the decimal flag.", CpuFlag.Decimal));
			_descriptions.Add("sei", new OpCodeDesc("SEI - Set Interrupt Disable", "Sets the interrupt disable flag.", CpuFlag.Interrupt));
			_descriptions.Add("sta", new OpCodeDesc("STA - Store Accumulator", "Stores the value of the accumulator into memory."));
			_descriptions.Add("stx", new OpCodeDesc("STX - Store X Register", "Stores the value of the X register into memory."));
			_descriptions.Add("sty", new OpCodeDesc("STY - Store Y Register", "Stores the value of the Y register into memory."));
			_descriptions.Add("tax", new OpCodeDesc("TAX - Transfer A to X", "Copies the accumulator into the X register.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("tay", new OpCodeDesc("TAY - Transfer A to Y", "Copies the accumulator into the Y register.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("tsx", new OpCodeDesc("TSX - Transfer SP to X", "Copies the stack pointer into the X register.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("txa", new OpCodeDesc("TXA - Transfer X to A", "Copies the X register into the accumulator.", CpuFlag.Zero | CpuFlag.Negative));
			_descriptions.Add("txs", new OpCodeDesc("TXS - Transfer X to SP", "Copies the X register into the stack pointer."));
			_descriptions.Add("tya", new OpCodeDesc("TYA - Transfer Y to A", "Copies the Y register into the accumulator.", CpuFlag.Zero | CpuFlag.Negative));
		}

		public static bool IsOpCode(string text)
		{
			OpCodeDesc desc;
			return _descriptions.TryGetValue(text.ToLowerInvariant(), out desc);
		}

		public frmOpCodeTooltip(Form parent, string opcode, int relativeAddress)
		{
			_parentForm = parent;
			InitializeComponent();

			OpCodeDesc desc = _descriptions[opcode.ToLowerInvariant()];

			if(relativeAddress >= 0) {
				byte opCode = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress);

				int opSize = 1;
				switch(AddressingModes[opCode]) {
					case AddrMode.Abs: case AddrMode.AbsX: case AddrMode.AbsXW: case AddrMode.AbsY: case AddrMode.AbsYW: case AddrMode.Ind:
						opSize = 3;
						break;

					case AddrMode.Imm: case AddrMode.IndX: case AddrMode.IndY: case AddrMode.IndYW: case AddrMode.Rel: case AddrMode.Zero: case AddrMode.ZeroX: case AddrMode.ZeroY:
						opSize = 2;
						break;
				}

				string byteCode = "";
				for(int i = 0; i < opSize; i++) {
					byte value = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)(relativeAddress + i));
					byteCode += "$" + value.ToString("X2") + " ";
				}

				lblByteCode.Text = byteCode;
				lblAddressingMode.Text = ResourceHelper.GetEnumText(AddressingModes[opCode]);
				lblCycleCount.Text = OpCycles[opCode].ToString();
				if(OpCycles[opCode] != OpCyclesCrossed[opCode]) {
					lblCycleCount.Text += $" ({OpCyclesCrossed[opCode]} if page crossed)";
				}
			} else {
				tlpOpCodeInfo.Visible = false;
			}

			ctrlFlagCarry.Letter = "C";
			ctrlFlagDecimal.Letter = "D";
			ctrlFlagInterrupt.Letter = "I";
			ctrlFlagNegative.Letter = "N";
			ctrlFlagOverflow.Letter = "O";
			ctrlFlagZero.Letter = "Z";

			lblName.Text = desc.Name;
			lblOpCodeDescription.Text = desc.Description;

			ctrlFlagCarry.Active = desc.Flags.HasFlag(CpuFlag.Carry);
			ctrlFlagDecimal.Active = desc.Flags.HasFlag(CpuFlag.Decimal);
			ctrlFlagInterrupt.Active = desc.Flags.HasFlag(CpuFlag.Interrupt);
			ctrlFlagNegative.Active = desc.Flags.HasFlag(CpuFlag.Negative);
			ctrlFlagOverflow.Active = desc.Flags.HasFlag(CpuFlag.Overflow);
			ctrlFlagZero.Active = desc.Flags.HasFlag(CpuFlag.Zero);

			this.TopLevel = false;
			this.Parent = _parentForm;
			_parentForm.Controls.Add(this);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.Width = this.tlpMain.Width;
			this.Height = this.tlpMain.Height;
			this.BringToFront();
		}

		[Flags]
		enum CpuFlag { None = 0, Carry = 1, Decimal = 2, Interrupt = 4, Negative = 8, Overflow = 16, Zero = 32 }

		class OpCodeDesc
		{
			internal string Name { get; set; }
			internal string Description { get; set; }
			internal CpuFlag Flags { get; set; }

			internal OpCodeDesc(string name, string desc, CpuFlag flags = CpuFlag.None)
			{
				Name = name;
				Description = desc;
				Flags = flags;
			}
		}

		enum AddrMode
		{
			None, Acc, Imp, Imm, Rel,
			Zero, Abs, ZeroX, ZeroY,
			Ind, IndX, IndY, IndYW,
			AbsX, AbsXW, AbsY, AbsYW
		}

		private AddrMode[] AddressingModes = new AddrMode[256] {
			AddrMode.Imp,  AddrMode.IndX,    AddrMode.None, AddrMode.IndX,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Imp,  AddrMode.Imm,  AddrMode.Acc,  AddrMode.Imm,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,
			AddrMode.Rel,  AddrMode.IndY,    AddrMode.None, AddrMode.IndYW,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.Imp,  AddrMode.AbsY, AddrMode.Imp,  AddrMode.AbsYW,AddrMode.AbsX, AddrMode.AbsX, AddrMode.AbsXW,AddrMode.AbsXW,
			AddrMode.Abs,  AddrMode.IndX,    AddrMode.None, AddrMode.IndX,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Imp,  AddrMode.Imm,  AddrMode.Acc,  AddrMode.Imm,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,
			AddrMode.Rel,  AddrMode.IndY,    AddrMode.None, AddrMode.IndYW,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.Imp,  AddrMode.AbsY, AddrMode.Imp,  AddrMode.AbsYW,AddrMode.AbsX, AddrMode.AbsX, AddrMode.AbsXW,AddrMode.AbsXW,
			AddrMode.Imp,  AddrMode.IndX,    AddrMode.None, AddrMode.IndX,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Imp,  AddrMode.Imm,  AddrMode.Acc,  AddrMode.Imm,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,
			AddrMode.Rel,  AddrMode.IndY,    AddrMode.None, AddrMode.IndYW,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.Imp,  AddrMode.AbsY, AddrMode.Imp,  AddrMode.AbsYW,AddrMode.AbsX, AddrMode.AbsX, AddrMode.AbsXW,AddrMode.AbsXW,
			AddrMode.Imp,  AddrMode.IndX,    AddrMode.None, AddrMode.IndX,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Imp,  AddrMode.Imm,  AddrMode.Acc,  AddrMode.Imm,  AddrMode.Ind,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,
			AddrMode.Rel,  AddrMode.IndY,    AddrMode.None, AddrMode.IndYW,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.Imp,  AddrMode.AbsY, AddrMode.Imp,  AddrMode.AbsYW,AddrMode.AbsX, AddrMode.AbsX, AddrMode.AbsXW,AddrMode.AbsXW,
			AddrMode.Imm,  AddrMode.IndX,    AddrMode.Imm,  AddrMode.IndX,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Imp,  AddrMode.Imm,  AddrMode.Imp,  AddrMode.Imm,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,
			AddrMode.Rel,  AddrMode.IndYW,   AddrMode.None, AddrMode.IndYW,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroY,   AddrMode.ZeroY,   AddrMode.Imp,  AddrMode.AbsYW,AddrMode.Imp,  AddrMode.AbsYW,AddrMode.AbsXW,AddrMode.AbsXW,AddrMode.AbsYW,AddrMode.AbsYW,
			AddrMode.Imm,  AddrMode.IndX,    AddrMode.Imm,  AddrMode.IndX,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Imp,  AddrMode.Imm,  AddrMode.Imp,  AddrMode.Imm,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,
			AddrMode.Rel,  AddrMode.IndY,    AddrMode.None, AddrMode.IndY,    AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroY,   AddrMode.ZeroY,   AddrMode.Imp,  AddrMode.AbsY, AddrMode.Imp,  AddrMode.AbsY, AddrMode.AbsX, AddrMode.AbsX, AddrMode.AbsY, AddrMode.AbsY,
			AddrMode.Imm,  AddrMode.IndX,    AddrMode.Imm,  AddrMode.IndX,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Imp,  AddrMode.Imm,  AddrMode.Imp,  AddrMode.Imm,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,
			AddrMode.Rel,  AddrMode.IndY,    AddrMode.None, AddrMode.IndYW,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.Imp,  AddrMode.AbsY, AddrMode.Imp,  AddrMode.AbsYW,AddrMode.AbsX, AddrMode.AbsX, AddrMode.AbsXW,AddrMode.AbsXW,
			AddrMode.Imm,  AddrMode.IndX,    AddrMode.Imm,  AddrMode.IndX,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Zero,    AddrMode.Imp,  AddrMode.Imm,  AddrMode.Imp,  AddrMode.Imm,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,  AddrMode.Abs,
			AddrMode.Rel,  AddrMode.IndY,    AddrMode.None, AddrMode.IndYW,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.ZeroX,   AddrMode.Imp,  AddrMode.AbsY, AddrMode.Imp,  AddrMode.AbsYW,AddrMode.AbsX, AddrMode.AbsX, AddrMode.AbsXW,AddrMode.AbsXW,
		};

		private int[] OpCycles = new int[256] {
			7, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 4, 4, 6, 6,
			2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
			6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 4, 4, 6, 6,
			2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
			6, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 3, 4, 6, 6,
			2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
			6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 5, 4, 6, 6,
			2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
			2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,
			2, 6, 2, 6, 4, 4, 4, 4, 2, 5, 2, 5, 5, 5, 5, 5,
			2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,
			2, 5, 2, 5, 4, 4, 4, 4, 2, 4, 2, 4, 4, 4, 4, 4,
			2, 6, 2, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,
			2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7,
			2, 6, 3, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,
			2, 5, 2, 8, 4, 4, 6, 6, 2, 4, 2, 7, 4, 4, 7, 7
		};

		private int[] OpCyclesCrossed = new int[256] {
			7, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 4, 4, 6, 6,
			3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
			6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 4, 4, 6, 6,
			3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
			6, 6, 2, 8, 3, 3, 5, 5, 3, 2, 2, 2, 3, 4, 6, 6,
			3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
			6, 6, 2, 8, 3, 3, 5, 5, 4, 2, 2, 2, 5, 4, 6, 6,
			3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
			2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,
			3, 6, 2, 6, 4, 4, 4, 4, 2, 5, 2, 5, 5, 5, 5, 5,
			2, 6, 2, 6, 3, 3, 3, 3, 2, 2, 2, 2, 4, 4, 4, 4,
			3, 6, 2, 5, 4, 4, 4, 4, 2, 5, 2, 5, 5, 5, 5, 5,
			2, 6, 2, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,
			3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
			2, 6, 3, 8, 3, 3, 5, 5, 2, 2, 2, 2, 4, 4, 6, 6,
			3, 6, 2, 8, 4, 4, 6, 6, 2, 5, 2, 7, 5, 5, 7, 7,
		};
	}
}
