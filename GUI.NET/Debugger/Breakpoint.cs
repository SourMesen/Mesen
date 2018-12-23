using Mesen.GUI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	public class Breakpoint
	{
		private DebugMemoryType _memoryType = DebugMemoryType.CpuMemory;
		private bool _isCpuBreakpoint = true;
		private AddressType _equivalentAddressType = GUI.AddressType.InternalRam;

		public DebugMemoryType MemoryType
		{
			get { return _memoryType; }
			set
			{
				_memoryType = value;
				_isCpuBreakpoint = IsTypeCpuBreakpoint(value);
				if(_isCpuBreakpoint) {
					_equivalentAddressType = value.ToAddressType();
				}
			}
		}
		public bool BreakOnRead = false;
		public bool BreakOnWrite = false;
		public bool BreakOnExec = true;

		public bool Enabled = true;
		public bool MarkEvent = false;
		public UInt32 Address;
		public UInt32 StartAddress;
		public UInt32 EndAddress;
		public BreakpointAddressType AddressType = BreakpointAddressType.SingleAddress;
		public string Condition = "";

		public string GetAddressString(bool showLabel)
		{
			string addr = "";
			switch(AddressType) {
				case BreakpointAddressType.AnyAddress: return "<any>";
				case BreakpointAddressType.SingleAddress:
					if(IsAbsoluteAddress) {
						int relativeAddress = this.GetRelativeAddress();
						if(relativeAddress >= 0) {
							addr += $"${relativeAddress.ToString("X4")} ";
						} else {
							addr += "N/A ";
						}
						addr += $"[${Address.ToString("X4")}]";
					} else {
						addr = $"${Address.ToString("X4")}";
					}
					break;

				case BreakpointAddressType.AddressRange:
					if(IsAbsoluteAddress) {
						addr = $"[${StartAddress.ToString("X4")}] - [${EndAddress.ToString("X4")}]";
					} else {
						addr = $"${StartAddress.ToString("X4")} - ${EndAddress.ToString("X4")}";
					}
					break;
			}

			string label = GetAddressLabel();
			if(showLabel && !string.IsNullOrWhiteSpace(label)) {
				addr = label + $", {addr}";
			}
			return addr;
		}

		public static bool IsTypeCpuBreakpoint(DebugMemoryType type)
		{
			return (
				type == DebugMemoryType.CpuMemory ||
				type == DebugMemoryType.WorkRam ||
				type == DebugMemoryType.SaveRam ||
				type == DebugMemoryType.PrgRom
			);
		}

		public void SetEnabled(bool enabled)
		{
			Enabled = enabled;
			BreakpointManager.RefreshBreakpoints(this);
		}

		public void SetMarked(bool marked)
		{
			MarkEvent = marked;
			BreakpointManager.RefreshBreakpoints(this);
		}

		public bool IsAbsoluteAddress { get { return MemoryType != DebugMemoryType.CpuMemory && MemoryType != DebugMemoryType.PpuMemory; } }

		public bool IsCpuBreakpoint { get { return this._isCpuBreakpoint; } }

		private BreakpointTypeFlags Type
		{
			get
			{
				BreakpointTypeFlags type = BreakpointTypeFlags.Global;
				if(BreakOnRead) {
					type |= IsCpuBreakpoint ? BreakpointTypeFlags.Read : BreakpointTypeFlags.ReadVram;
				}
				if(BreakOnWrite) {
					type |= IsCpuBreakpoint ? BreakpointTypeFlags.Write : BreakpointTypeFlags.WriteVram;
				}
				if(BreakOnExec && IsCpuBreakpoint) {
					type |= BreakpointTypeFlags.Execute;
				}
				return type;
			}
		}

		public string ToReadableType()
		{
			string type;

			switch(MemoryType) {
				default:
				case DebugMemoryType.CpuMemory: type = "CPU"; break;
				case DebugMemoryType.PpuMemory: type = "PPU"; break;
				case DebugMemoryType.PrgRom: type = "PRG"; break;
				case DebugMemoryType.WorkRam: type = "WRAM"; break;
				case DebugMemoryType.SaveRam: type = "SRAM"; break;
				case DebugMemoryType.ChrRam: type = "CHR"; break;
				case DebugMemoryType.ChrRom: type = "CHR"; break;
				case DebugMemoryType.PaletteMemory: type = "PAL"; break;
			}

			type += ":";
			type += BreakOnRead ? "R" : "‒";
			type += BreakOnWrite ? "W" : "‒";
			if(IsCpuBreakpoint) {
				type += BreakOnExec ? "X" : "‒";
			}
			return type;
		}

		public string GetAddressLabel()
		{
			UInt32 address = AddressType == BreakpointAddressType.SingleAddress ? this.Address : this.StartAddress;

			if(IsCpuBreakpoint) {
				CodeLabel label;
				if(this.IsAbsoluteAddress) {
					label = LabelManager.GetLabel(address, this.MemoryType.ToAddressType());
				} else {
					label = LabelManager.GetLabel((UInt16)address);
				}
				if(label != null) {
					return label.Label;
				}
			}
			return string.Empty;
		}

		public int GetRelativeAddress()
		{
			UInt32 address = AddressType == BreakpointAddressType.SingleAddress ? this.Address : this.StartAddress;
			if(this.IsAbsoluteAddress) {
				if(IsCpuBreakpoint) {
					return InteropEmu.DebugGetRelativeAddress(address, this.MemoryType.ToAddressType());
				} else {
					return InteropEmu.DebugGetRelativeChrAddress(address);
				}
			} else {
				return -1;
			}
		}

		public bool Matches(int relativeAddress, AddressTypeInfo info)
		{
			if(this.IsCpuBreakpoint && this.AddressType == BreakpointAddressType.SingleAddress) {
				if(this.MemoryType == DebugMemoryType.CpuMemory) {
					return relativeAddress == this.Address;
				} else {
					return _equivalentAddressType == info.Type && info.Address == this.Address;
				}
			}
			return false;
		}

		public InteropBreakpoint ToInteropBreakpoint(int breakpointId)
		{
			InteropBreakpoint bp = new InteropBreakpoint() {
				Id = breakpointId,
				MemoryType = MemoryType,
				Type = Type,
				MarkEvent = MarkEvent,
				Enabled = Enabled
			};
			switch(AddressType) {
				case BreakpointAddressType.AnyAddress:
					bp.StartAddress = -1;
					bp.EndAddress = -1;
					break;

				case BreakpointAddressType.SingleAddress:
					bp.StartAddress = (Int32)Address;
					bp.EndAddress = -1;
					break;

				case BreakpointAddressType.AddressRange:
					bp.StartAddress = (Int32)StartAddress;
					bp.EndAddress = (Int32)EndAddress;
					break;
			}

			bp.Condition = new byte[1000];
			byte[] condition = Encoding.UTF8.GetBytes(Condition.Replace(Environment.NewLine, " "));
			Array.Copy(condition, bp.Condition, condition.Length);
			return bp;
		}
	}

	public enum BreakpointAddressType
	{
		AnyAddress,
		SingleAddress,
		AddressRange,
	}
}