using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	public class CodeLabel
	{
		public UInt32 Address;
		public AddressType AddressType;
		public string Label;
		public string Comment;
	}

	public class LabelManager
	{
		private static Dictionary<string, CodeLabel> _labels = new Dictionary<string, CodeLabel>();
		private static Dictionary<string, CodeLabel> _reverseLookup = new Dictionary<string, CodeLabel>();

		public static event EventHandler OnLabelUpdated;

		public static void ResetLabels()
		{
			_labels.Clear();
			_reverseLookup.Clear();
		}

		public static CodeLabel GetLabel(UInt32 address, AddressType type)
		{
			return _labels.ContainsKey(GetKey(address, type)) ? _labels[GetKey(address, type)] : null;
		}

		public static CodeLabel GetLabel(string label)
		{
			return _reverseLookup.ContainsKey(label) ? _reverseLookup[label] : null;
		}

		public static void SetLabels(List<CodeLabel> labels)
		{
			ResetLabels();
			foreach(CodeLabel label in labels) {
				SetLabel(label.Address, label.AddressType, label.Label, label.Comment, false);
			}
			OnLabelUpdated?.Invoke(null, null);
		}

		public static List<CodeLabel> GetLabels()
		{
			return _labels.Values.ToList<CodeLabel>();
		}

		private static string GetKey(UInt32 address, AddressType addressType)
		{
			return address.ToString() + addressType.ToString();
		}

		public static bool SetLabel(UInt32 address, AddressType type, string label, string comment, bool raiseEvent = true)
		{
			if(_labels.ContainsKey(GetKey(address, type))) {
				_reverseLookup.Remove(_labels[GetKey(address, type)].Label);
			}

			_labels[GetKey(address, type)] = new CodeLabel() { Address = address, AddressType = type, Label = label, Comment = comment };
			if(label.Length > 0) {
				_reverseLookup[label] = _labels[GetKey(address, type)];
			}

			InteropEmu.DebugSetLabel(address, type, label, comment);
			if(raiseEvent) {
				OnLabelUpdated?.Invoke(null, null);
			}

			return true;
		}

		public static void DeleteLabel(UInt32 address, AddressType type, bool raiseEvent)
		{
			if(_labels.ContainsKey(GetKey(address, type))) {
				_reverseLookup.Remove(_labels[GetKey(address, type)].Label);
			}
			if(_labels.Remove(GetKey(address, type))) {
				InteropEmu.DebugSetLabel(address, type, string.Empty, string.Empty);
				if(raiseEvent) {
					OnLabelUpdated?.Invoke(null, null);
				}
			}
		}

		public static void SetDefaultLabels(bool forFDS)
		{
			LabelManager.SetLabel(0x2000, AddressType.Register, "PpuControl_2000", string.Empty);
			LabelManager.SetLabel(0x2001, AddressType.Register, "PpuMask_2001", string.Empty);
			LabelManager.SetLabel(0x2002, AddressType.Register, "PpuStatus_2002", string.Empty);
			LabelManager.SetLabel(0x2003, AddressType.Register, "OamAddr_2003", string.Empty);
			LabelManager.SetLabel(0x2004, AddressType.Register, "OamData_2004", string.Empty);
			LabelManager.SetLabel(0x2005, AddressType.Register, "PpuScroll_2005", string.Empty);
			LabelManager.SetLabel(0x2006, AddressType.Register, "PpuAddr_2006", string.Empty);
			LabelManager.SetLabel(0x2007, AddressType.Register, "PpuData_2007", string.Empty);

			LabelManager.SetLabel(0x4000, AddressType.Register, "Sq1Duty_4000", string.Empty);
			LabelManager.SetLabel(0x4001, AddressType.Register, "Sq1Sweep_4001", string.Empty);
			LabelManager.SetLabel(0x4002, AddressType.Register, "Sq1Timer_4002", string.Empty);
			LabelManager.SetLabel(0x4003, AddressType.Register, "Sq1Length_4003", string.Empty);

			LabelManager.SetLabel(0x4004, AddressType.Register, "Sq1Duty_4004", string.Empty);
			LabelManager.SetLabel(0x4005, AddressType.Register, "Sq1Sweep_4005", string.Empty);
			LabelManager.SetLabel(0x4006, AddressType.Register, "Sq1Timer_4006", string.Empty);
			LabelManager.SetLabel(0x4007, AddressType.Register, "Sq1Length_4007", string.Empty);

			LabelManager.SetLabel(0x4008, AddressType.Register, "TrgLinear_4008", string.Empty);
			LabelManager.SetLabel(0x400A, AddressType.Register, "TrgTimer_400A", string.Empty);
			LabelManager.SetLabel(0x400B, AddressType.Register, "TrgLength_400B", string.Empty);

			LabelManager.SetLabel(0x400C, AddressType.Register, "NoiseVolume_400C", string.Empty);
			LabelManager.SetLabel(0x400E, AddressType.Register, "NoisePeriod_400E", string.Empty);
			LabelManager.SetLabel(0x400F, AddressType.Register, "NoiseLength_400F", string.Empty);

			LabelManager.SetLabel(0x4010, AddressType.Register, "DmcFreq_4010", string.Empty);
			LabelManager.SetLabel(0x4011, AddressType.Register, "DmcCounter_4011", string.Empty);
			LabelManager.SetLabel(0x4012, AddressType.Register, "DmcAddress_4012", string.Empty);
			LabelManager.SetLabel(0x4013, AddressType.Register, "DmcLength_4013", string.Empty);

			LabelManager.SetLabel(0x4014, AddressType.Register, "SpriteDma_4014", string.Empty);

			LabelManager.SetLabel(0x4015, AddressType.Register, "ApuStatus_4015", string.Empty);

			LabelManager.SetLabel(0x4016, AddressType.Register, "Ctrl1_4016", string.Empty);
			LabelManager.SetLabel(0x4017, AddressType.Register, "Ctrl2_FrameCtr_4017", string.Empty);

			if(forFDS) {
				LabelManager.SetLabel(0x01F8, AddressType.PrgRom, "LoadFiles", "Input: Pointer to Disk ID, Pointer to File List" + Environment.NewLine + "Output: A = error #, Y = # of files loaded" + Environment.NewLine + "Desc: Loads files specified by DiskID into memory from disk. Load addresses are decided by the file's header.");
				LabelManager.SetLabel(0x0237, AddressType.PrgRom, "AppendFile", "Input: Pointer to Disk ID, Pointer to File Header" + Environment.NewLine + "Output: A = error #" + Environment.NewLine + "Desc: Appends the file data given by DiskID to the disk. This means that the file is tacked onto the end of the disk, and the disk file count is incremented. The file is then read back to verify the write. If an error occurs during verification, the disk's file count is decremented (logically hiding the written file).");
				LabelManager.SetLabel(0x0239, AddressType.PrgRom, "WriteFile", "Input: Pointer to Disk ID, Pointer to File Header, A = file #" + Environment.NewLine + "Output: A = error #" + Environment.NewLine + "Desc: Same as \"Append File\", but instead of writing the file to the end of the disk, A specifies the sequential position on the disk to write the file (0 is the first). This also has the effect of setting the disk's file count to the A value, therefore logically hiding any other files that may reside after the written one.");
				LabelManager.SetLabel(0x02B7, AddressType.PrgRom, "CheckFileCount", "Input: Pointer to Disk ID, A = # to set file count to" + Environment.NewLine + "Output: A = error #" + Environment.NewLine + "Desc: Reads in disk's file count, compares it to A, then sets the disk's file count to A.");
				LabelManager.SetLabel(0x02BB, AddressType.PrgRom, "AdjustFileCount", "Input: Pointer to Disk ID, A = number to reduce current file count by" + Environment.NewLine + "Output: A = error #" + Environment.NewLine + "Desc: Reads in disk's file count, decrements it by A, then writes the new value back.");
				LabelManager.SetLabel(0x0301, AddressType.PrgRom, "SetFileCount1", "Input: Pointer to Disk ID, A = file count minus one = # of the last file" + Environment.NewLine + "Output: A = error #" + Environment.NewLine + "Desc: Set the file count to A + 1");
				LabelManager.SetLabel(0x0305, AddressType.PrgRom, "SetFileCount", "Input: Pointer to Disk ID, A = file count" + Environment.NewLine + "Output: A = error #" + Environment.NewLine + "Desc: Set the file count to A");
				LabelManager.SetLabel(0x032A, AddressType.PrgRom, "GetDiskInfo", "Input: Pointer to Disk Info" + Environment.NewLine + "Output: A = error #" + Environment.NewLine + "Desc: Fills DiskInfo up with data read off the current disk.");

				LabelManager.SetLabel(0x0445, AddressType.PrgRom, "CheckDiskHeader", "Input: Pointer to 10 byte string at $00 " + Environment.NewLine + "Output:  " + Environment.NewLine + "Desc: Compares the first 10 bytes on the disk coming after the FDS string, to 10 bytes pointed to by Ptr($00). To bypass the checking of any byte, a -1 can be placed in the equivelant place in the compare string.  Otherwise, if the comparison fails, an appropriate error will be generated.");
				LabelManager.SetLabel(0x0484, AddressType.PrgRom, "GetNumFiles", "Input:  " + Environment.NewLine + "Output:  " + Environment.NewLine + "Desc: Reads number of files stored on disk, stores the result in $06");
				LabelManager.SetLabel(0x0492, AddressType.PrgRom, "SetNumFiles", "Input:  " + Environment.NewLine + "Output: A = number of files " + Environment.NewLine + "Desc: Writes new number of files to disk header.");
				LabelManager.SetLabel(0x04A0, AddressType.PrgRom, "FileMatchTest", "Input: Pointer to FileID list at $02 " + Environment.NewLine + "Output:   " + Environment.NewLine + "Desc: Uses a byte string pointed at by Ptr($02) to tell the disk system which files to load.  The file ID's number is searched for in the string.  If an exact match is found, [$09] is 0'd, and [$0E] is incremented.  If no matches are found after 20 bytes, or a -1 entry is encountered, [$09] is set to -1.  If the first byte in the string is -1, the BootID number is used for matching files (any FileID that is not greater than the BootID qualifies as a match).");
				LabelManager.SetLabel(0x04DA, AddressType.PrgRom, "SkipFiles", "Input: Number of files to skip in $06 " + Environment.NewLine + "Output:  " + Environment.NewLine + "Desc: Skips over specified number of files.");

				LabelManager.SetLabel(0x0149, AddressType.PrgRom, "Delay132", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: " + Environment.NewLine + "Desc: 132 clock cycle delay");
				LabelManager.SetLabel(0x0153, AddressType.PrgRom, "Delayms", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: X, Y " + Environment.NewLine + "Desc: Delay routine, Y = delay in ms (approximate)");
				LabelManager.SetLabel(0x0161, AddressType.PrgRom, "DisPFObj", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, $fe " + Environment.NewLine + "Desc: Disable sprites and background");
				LabelManager.SetLabel(0x016B, AddressType.PrgRom, "EnPFObj", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, $fe " + Environment.NewLine + "Desc: Enable sprites and background");
				LabelManager.SetLabel(0x0171, AddressType.PrgRom, "DisObj", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, $fe " + Environment.NewLine + "Desc: Disable sprites");
				LabelManager.SetLabel(0x0178, AddressType.PrgRom, "EnObj", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, $fe " + Environment.NewLine + "Desc: Enable sprites");
				LabelManager.SetLabel(0x017E, AddressType.PrgRom, "DisPF", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, $fe " + Environment.NewLine + "Desc: Disable background");
				LabelManager.SetLabel(0x0185, AddressType.PrgRom, "EnPF", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, $fe " + Environment.NewLine + "Desc: Enable background");
				LabelManager.SetLabel(0x01B2, AddressType.PrgRom, "VINTWait", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: $ff " + Environment.NewLine + "Desc: Wait until next VBlank NMI fires, and return (for programs that does it the \"everything in main\" way). NMI vector selection at $100 is preserved, but further VBlanks are disabled.");
				LabelManager.SetLabel(0x07BB, AddressType.PrgRom, "VRAMStructWrite", "Input: Pointer to VRAM buffer to be written " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, Y, $00, $01, $ff " + Environment.NewLine + "Desc: Set VRAM increment to 1 (clear [[PPUCTRL]]/$ff bit 2), and write a VRAM buffer to VRAM. Read below for information on the structure.");
				LabelManager.SetLabel(0x0844, AddressType.PrgRom, "FetchDirectPtr", "Input: " + Environment.NewLine + "Output: $00, $01 = pointer fetched " + Environment.NewLine + "Affects: A, X, Y, $05, $06 " + Environment.NewLine + "Desc: Fetch a direct pointer from the stack (the pointer should be placed after the return address of the routine that calls this one (see \"important notes\" above)), save the pointer at ($00) and fix the return address.");
				LabelManager.SetLabel(0x086A, AddressType.PrgRom, "WriteVRAMBuffer", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, Y, $301, $302 " + Environment.NewLine + "Desc: Write the VRAM Buffer at $302 to VRAM. Read below for information on the structure.");
				LabelManager.SetLabel(0x08B3, AddressType.PrgRom, "ReadVRAMBuffer", "Input: X = start address of read buffer, Y = # of bytes to read " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, Y " + Environment.NewLine + "Desc: Read individual bytes from VRAM to the VRAMBuffer. (see notes below)");
				LabelManager.SetLabel(0x08D2, AddressType.PrgRom, "PrepareVRAMString", "Input: A = High VRAM address, X = Low VRAM address, Y = string length, Direct Pointer = data to be written to VRAM " + Environment.NewLine + "Output: A = $ff : no error, A = $01 : string didn't fit in buffer " + Environment.NewLine + "Affects: A, X, Y, $00, $01, $02, $03, $04, $05, $06 " + Environment.NewLine + "Desc: This routine copies pointed data into the VRAM buffer.");
				LabelManager.SetLabel(0x08E1, AddressType.PrgRom, "PrepareVRAMStrings", "Input: A = High VRAM address, X = Low VRAM address, Direct pointer = data to be written to VRAM " + Environment.NewLine + "Output: A = $ff : no error, A = $01 : data didn't fit in buffer " + Environment.NewLine + "Affects: A, X, Y, $00, $01, $02, $03, $04, $05, $06 " + Environment.NewLine + "Desc: This routine copies a 2D string into the VRAM buffer. The first byte of the data determines the width and height of the following string (in tiles): Upper nybble = height, lower nybble = width.");
				LabelManager.SetLabel(0x094F, AddressType.PrgRom, "GetVRAMBufferByte", "Input: X = starting index of read buffer, Y = # of address to compare (starting at 1), $00, $01 = address to read from " + Environment.NewLine + "Output: carry clear : a previously read byte was returned, carry set : no byte was read, should wait next call to ReadVRAMBuffer " + Environment.NewLine + "Affects: A, X, Y " + Environment.NewLine + "Desc: This routine was likely planned to be used in order to avoid useless latency on a VRAM reads (see notes below). It compares the VRAM address in ($00) with the Yth (starting at 1) address of the read buffer. If both addresses match, the corresponding data byte is returned exit with c clear. If the addresses are different, the buffer address is overwritten by the address in ($00) and the routine exit with c set.");
				LabelManager.SetLabel(0x097D, AddressType.PrgRom, "Pixel2NamConv", "Input: $02 = Pixel X cord, $03 = Pixel Y cord " + Environment.NewLine + "Output: $00 = High nametable address, $01 = Low nametable address " + Environment.NewLine + "Affects: A " + Environment.NewLine + "Desc: This routine convert pixel screen coordinates to corresponding nametable address (assumes no scrolling, and points to first nametable at $2000-$23ff).");
				LabelManager.SetLabel(0x0997, AddressType.PrgRom, "Nam2PixelConv", "Input: $00 = High nametable address, $01 = low nametable address " + Environment.NewLine + "Output: $02 = Pixel X cord, $03 = Pixel Y cord " + Environment.NewLine + "Affects: A " + Environment.NewLine + "Desc: This routine convert a nametable address to corresponding pixel coordinates (assume no scrolling).");
				LabelManager.SetLabel(0x09B1, AddressType.PrgRom, "Random", "Input: X = Zero Page address where the random bytes are placed, Y = # of shift register bytes (normally $02) " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, Y, $00 " + Environment.NewLine + "Desc: This is a shift-register based random number generator, normally takes 2 bytes (using more won't affect random sequence). On reset you are supposed to write some non-zero values here (BIOS uses writes $d0, $d0), and call this routine several times before the data is actually random. Each call of this routine will shift the bytes ''right''.");
				LabelManager.SetLabel(0x09C8, AddressType.PrgRom, "SpriteDMA", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A " + Environment.NewLine + "Desc: This routine does sprite DMA from RAM $200-$2ff");
				LabelManager.SetLabel(0x09D3, AddressType.PrgRom, "CounterLogic", "Input: A, Y = end Zeropage address of counters, X = start zeropage address of counters " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, $00 " + Environment.NewLine + "Desc: This decrements several counters in Zeropage. The first counter is a decimal counter 9 -> 8 -> 7 -> ... -> 1 -> 0 -> 9 -> ... Counters 1...A are simply decremented and stays at 0. Counters A+1...Y are decremented when the first counter does a 0 -> 9 transition, and stays at 0.");
				LabelManager.SetLabel(0x09EB, AddressType.PrgRom, "ReadPads", "Input: " + Environment.NewLine + "Output: $f5 = Joypad #1 data, $f6 = Joypad #2 data " + Environment.NewLine + "Affects: A, X, $00, $01, " + Environment.NewLine + "Desc: This read hardwired famicom joypads.");
				LabelManager.SetLabel(0x0A1A, AddressType.PrgRom, "ReadDownPads", "Input: " + Environment.NewLine + "Output: $f5 = Joypad #1 up->down transitions, $f6 = Joypad #2 up->down transitions $f7 = Joypad #1 data, $f8 = Joypad #2 data " + Environment.NewLine + "Affects: A, X, $00, $01 " + Environment.NewLine + "Desc: This reads hardwired famicom joypads, and detect up->down button transitions");
				LabelManager.SetLabel(0x0A1F, AddressType.PrgRom, "ReadOrDownPads", "Input: " + Environment.NewLine + "Output: $f5 = Joypad #1 up->down transitions, $f6 = Joypad #2 up->down transitions $f7 = Joypad #1 data, $f8 = Joypad #2 data " + Environment.NewLine + "Affects: A, X, $00, $01 " + Environment.NewLine + "Desc: This read both hardwired famicom and expansion port joypads and detect up->down button transitions.");
				LabelManager.SetLabel(0x0A36, AddressType.PrgRom, "ReadDownVerifyPads", "Input: " + Environment.NewLine + "Output: $f5 = Joypad #1 up->down transitions, $f6 = Joypad #2 up->down transitions $f7 = Joypad #1 data, $f8 = Joypad #2 data " + Environment.NewLine + "Affects: A, X, $00, $01 " + Environment.NewLine + "Desc: This reads hardwired Famicom joypads, and detect up->down button transitions. Data is read until two consecutive read matches to work around the DMC reading glitches.");
				LabelManager.SetLabel(0x0A4C, AddressType.PrgRom, "ReadOrDownVerifyPads", "Input: " + Environment.NewLine + "Output: $f5 = Joypad #1 up->down transitions, $f6 = Joypad #2 up->down transitions $f7 = Joypad #1 data, $f8 = Joypad #2 data " + Environment.NewLine + "Affects: A, X, $00, $01 " + Environment.NewLine + "Desc: This read both hardwired famicom and expansion port joypads and detect up->down button transitions. Data is read until two consecutive read matches to work around the DMC reading glitches.");
				LabelManager.SetLabel(0x0A68, AddressType.PrgRom, "ReadDownExpPads", "Input: $f1-$f4 = up->down transitions, $f5-$f8 = Joypad data in the order : Pad1, Pad2, Expansion1, Expansion2 " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, $00, $01 " + Environment.NewLine + "Desc: This read both hardwired famicom and expansion port joypad, but stores their data separately instead of ORing them together like the other routines does. This routine is NOT DMC fortified.");
				LabelManager.SetLabel(0x0A84, AddressType.PrgRom, "VRAMFill", "Input: A = High VRAM Address (aka tile row #), X = Fill value, Y = # of tile rows OR attribute fill data " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, Y, $00, $01, $02 " + Environment.NewLine + "Desc: This routine does 2 things : If A < $20, it fills pattern table data with the value in X for 16 * Y tiles. If A >= $20, it fills the corresponding nametable with the value in X and attribute table with the value in Y.");
				LabelManager.SetLabel(0x0Ad2, AddressType.PrgRom, "MemFill", "Input: A = fill value, X = first page #, Y = last page # " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, Y, $00, $01 " + Environment.NewLine + "Desc: This routines fills RAM pages with specified value.");
				LabelManager.SetLabel(0x0AEA, AddressType.PrgRom, "SetScroll", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A " + Environment.NewLine + "Desc: This routine set scroll registers according to values in $fc, $fd and $ff. Should typically be called in VBlank after VRAM updates");
				LabelManager.SetLabel(0x0AFD, AddressType.PrgRom, "JumpEngine", "Input: A = Jump table entry " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, Y, $00, $01 " + Environment.NewLine + "Desc: The instruction calling this is supposed to be followed by a jump table (16-bit pointers little endian, up to 128 pointers). A is the entry # to jump to, return address on stack is used to get jump table entries.");
				LabelManager.SetLabel(0x0B13, AddressType.PrgRom, "ReadKeyboard", "Input: " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: " + Environment.NewLine + "Desc: Read Family Basic Keyboard expansion (detail is under analysis)");
				LabelManager.SetLabel(0x0B66, AddressType.PrgRom, "LoadTileset", "Input: A = Low VRAM Address & Flags, Y = Hi VRAM Address, X = # of tiles to transfer to/from VRAM " + Environment.NewLine + "Output: " + Environment.NewLine + "Affects: A, X, Y, $00, $01, $02, $03, $04 " + Environment.NewLine + "Desc: This routine can read and write 2BP and 1BP tilesets to/from VRAM. See appendix below about the flags.");
				LabelManager.SetLabel(0x0C22, AddressType.PrgRom, "unk_EC22", "Some kind of logic that some games use. (detail is under analysis)");
			}
		}
	}
}
