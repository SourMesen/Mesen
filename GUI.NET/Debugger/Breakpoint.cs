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
		public bool BreakOnRead = false;
		public bool BreakOnWrite = false;
		public bool BreakOnReadVram = false;
		public bool BreakOnWriteVram = false;
		public bool BreakOnExec = true;

		public bool Enabled = true;
		public UInt32 Address;
		public bool SpecificAddress = true;
		public bool IsAbsoluteAddress = false;
		public string Condition = "";

		public void SetEnabled(bool enabled)
		{
			Enabled = enabled;
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

		public InteropBreakpoint ToInteropBreakpoint()
		{
			InteropBreakpoint bp = new InteropBreakpoint() { Address = SpecificAddress ? (Int32)Address : -1, Type = Type, IsAbsoluteAddress = IsAbsoluteAddress };
			bp.Condition = new byte[1000];
			byte[] condition = Encoding.UTF8.GetBytes(Condition);
			Array.Copy(condition, bp.Condition, condition.Length);
			return bp;
		}
	}
}