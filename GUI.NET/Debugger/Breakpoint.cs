using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	public class Breakpoint
	{
		private bool _added = false;		

		public BreakpointType Type;
		public bool Enabled = true;
		public UInt32 Address;
		public bool IsAbsoluteAddress;

		public void Remove()
		{
			if(_added) {
				InteropEmu.DebugRemoveBreakpoint(Type, Address, false);
			}
		}

		public void Add()
		{
			InteropEmu.DebugAddBreakpoint(Type, Address, false, Enabled);
			_added = true;
		}

		public void SetEnabled(bool enabled)
		{
			Enabled = enabled;
			Remove();
			Add();
		}
	}
}
