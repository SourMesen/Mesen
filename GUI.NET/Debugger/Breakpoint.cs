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

		public string GetAddressString(bool showLabel)
		{
			string addr = "";
			switch(AddressType) {
				case BreakpointAddressType.AnyAddress: return "<any>";
				case BreakpointAddressType.SingleAddress:
					if(IsAbsoluteAddress) {
						int relativeAddress = this.GetRelativeAddress();
						if(relativeAddress >= 0) {
							addr += "$" + this.GetRelativeAddress().ToString("X4") + " ";
						}
						addr += "[$" + Address.ToString("X4") + "]";
					} else {
						addr = "$" + Address.ToString("X4");
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

		public string GetAddressLabel()
		{
			UInt32 address = AddressType == BreakpointAddressType.SingleAddress ? this.Address : this.StartAddress;

			if(IsCpuBreakpoint) {
				CodeLabel label;
				if(this.IsAbsoluteAddress) {
					label = LabelManager.GetLabel(address, GUI.AddressType.PrgRom);
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
					return InteropEmu.DebugGetRelativeAddress(address, GUI.AddressType.PrgRom);
				} else {
					return InteropEmu.DebugGetRelativeChrAddress(address);
				}
			} else {
				return (int)this.Address;
			}
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