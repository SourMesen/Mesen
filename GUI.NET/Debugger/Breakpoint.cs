using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	public enum BreakpointAddressType
	{
		AnyAddress,
		SingleAddress,
		AddressRange,
	}
	public class Breakpoint
	{
		public bool BreakOnRead = false;
		public bool BreakOnWrite = false;
		public bool BreakOnReadVram = false;
		public bool BreakOnWriteVram = false;
		public bool BreakOnExec = true;

		public bool Enabled = true;
		public UInt32 Address;
		public UInt32 StartAddress;
		public UInt32 EndAddress;
		public BreakpointAddressType AddressType = BreakpointAddressType.SingleAddress;
		public bool IsAbsoluteAddress = false;
		public string Condition = "";

		public string GetAddressString()
		{
			switch(AddressType) {
				case BreakpointAddressType.AnyAddress: return "<any>";
				case BreakpointAddressType.SingleAddress:
					if(IsAbsoluteAddress) {
						int relativeAddress = this.GetRelativeAddress();
						string addr = "";
						if(relativeAddress >= 0) {
							addr += "$" + this.GetRelativeAddress().ToString("X4") + " ";
						}
						addr += "[$" + Address.ToString("X4") + "]";
						return addr;
					} else {
						return "$" + Address.ToString("X4");
					}

				case BreakpointAddressType.AddressRange:
					if(IsAbsoluteAddress) {
						return $"[${StartAddress.ToString("X4")} - [${EndAddress.ToString("X4")}]";
					} else {
						return $"${StartAddress.ToString("X4")} - ${EndAddress.ToString("X4")}";
					}
			}
			return string.Empty;
		}

		public void SetEnabled(bool enabled)
		{
			Enabled = enabled;
			BreakpointManager.RefreshBreakpoints(this);
		}

		public bool IsCpuBreakpoint
		{
			get
			{
				return 
					Type.HasFlag(BreakpointType.Read) ||
					Type.HasFlag(BreakpointType.Write) ||
					Type.HasFlag(BreakpointType.Execute);
			}
		}

		public BreakpointType Type
		{
			get
			{
				BreakpointType type = BreakpointType.Global;
				if(BreakOnRead) {
					type |= BreakpointType.Read;
				}
				if(BreakOnWrite) {
					type |= BreakpointType.Write;
				}
				if(BreakOnExec) {
					type |= BreakpointType.Execute;
				}
				if(BreakOnReadVram) {
					type |= BreakpointType.ReadVram;
				}
				if(BreakOnWriteVram) {
					type |= BreakpointType.WriteVram;
				}
				return type;
			}
		}

		public int GetRelativeAddress()
		{
			if(this.IsCpuBreakpoint) {
				if(this.IsAbsoluteAddress) {
					return InteropEmu.DebugGetRelativeAddress((uint)this.Address, GUI.AddressType.PrgRom);
				} else {
					return (int)this.Address;
				}
			}
			return 0;
		}

		public bool Matches(int relativeAddress)
		{
			if(this.IsCpuBreakpoint) {
				if(this.IsAbsoluteAddress) {
					AddressTypeInfo addressTypeInfo = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType((uint)relativeAddress, ref addressTypeInfo);

					return addressTypeInfo.Type == GUI.AddressType.PrgRom && addressTypeInfo.Address == this.Address;
				} else {
					return relativeAddress == this.Address;
				}
			}
			return false;
		}

		public InteropBreakpoint ToInteropBreakpoint()
		{
			InteropBreakpoint bp = new InteropBreakpoint() {
				Type = Type,
				IsAbsoluteAddress = IsAbsoluteAddress
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
			byte[] condition = Encoding.UTF8.GetBytes(Condition);
			Array.Copy(condition, bp.Condition, condition.Length);
			return bp;
		}
	}
}