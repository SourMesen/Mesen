using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public class BreakpointManager
	{
		private static object _lock = new object();
		private static List<Breakpoint> _breakpoints = new List<Breakpoint>();

		public static event EventHandler BreakpointsChanged;
		public static ReadOnlyCollection<Breakpoint> Breakpoints {
			get {
				lock(_lock) {
					return _breakpoints.ToList().AsReadOnly();
				}
			}
		}

		public static void RefreshBreakpoints(Breakpoint bp = null)
		{
			if(BreakpointsChanged != null) {
				BreakpointsChanged(bp, null);
			}

			SetBreakpoints();
		}

		public static void SetBreakpoints(List<Breakpoint> breakpoints)
		{
			lock(_lock) {
				_breakpoints = breakpoints.ToList();
			}

			RefreshBreakpoints();
		}

		public static void EditBreakpoint(Breakpoint bp)
		{
			if(new frmBreakpoint(bp).ShowDialog() == DialogResult.OK) {
				lock(_lock) {
					if(!_breakpoints.Contains(bp)) {
						_breakpoints.Add(bp);
					}
				}
				RefreshBreakpoints(bp);
			}
		}

		public static void RemoveBreakpoint(Breakpoint bp)
		{
			lock(_lock) {
				_breakpoints.Remove(bp);
			}
			RefreshBreakpoints(bp);
		}

		public static void AddBreakpoint(Breakpoint bp)
		{
			lock(_lock) {
				_breakpoints.Add(bp);
			}
			RefreshBreakpoints(bp);
		}

		public static Breakpoint GetMatchingBreakpoint(int relativeAddress, AddressTypeInfo info)
		{
			return Breakpoints.Where((bp) => bp.Matches(relativeAddress, info)).FirstOrDefault();
		}

		public static Breakpoint GetMatchingBreakpoint(UInt32 startAddress, UInt32 endAddress, DebugMemoryType memoryType)
		{
			bool isAddressRange = startAddress != endAddress;
			return Breakpoints.Where((bp) =>
					bp.MemoryType == memoryType &&
					((!isAddressRange && bp.Address == startAddress) || (isAddressRange && bp.StartAddress == startAddress && bp.EndAddress == endAddress))					
				).FirstOrDefault();
		}

		public static void ToggleBreakpoint(int relativeAddress, AddressTypeInfo info, bool toggleEnabled)
		{
			if(relativeAddress >= 0 || info.Address >= 0) {
				Breakpoint breakpoint = BreakpointManager.GetMatchingBreakpoint(relativeAddress, info);
				if(breakpoint != null) {
					if(toggleEnabled) {
						breakpoint.SetEnabled(!breakpoint.Enabled);
					} else {
						BreakpointManager.RemoveBreakpoint(breakpoint);
					}
				} else {
					if(info.Address < 0 || info.Type == AddressType.InternalRam) {
						breakpoint = new Breakpoint() {
							MemoryType = DebugMemoryType.CpuMemory,
							BreakOnExec = true,
							BreakOnRead = true,
							BreakOnWrite = true,
							Address = (UInt32)relativeAddress,
							Enabled = true
						};
					} else {
						breakpoint = new Breakpoint() {
							Enabled = true,
							BreakOnExec = true,
							Address = (UInt32)info.Address
						};

						if(info.Type != AddressType.PrgRom) {
							breakpoint.BreakOnRead = true;
							breakpoint.BreakOnWrite = true;
						}

						breakpoint.MemoryType = info.Type.ToMemoryType();
					}
					BreakpointManager.AddBreakpoint(breakpoint);
				}
			}
		}

		public static void SetBreakpoints()
		{
			List<InteropBreakpoint> breakpoints = new List<InteropBreakpoint>();
			for(int i = 0; i < Breakpoints.Count; i++) {
				breakpoints.Add(Breakpoints[i].ToInteropBreakpoint(i));
			}
			InteropEmu.DebugSetBreakpoints(breakpoints.ToArray(), (UInt32)breakpoints.Count);
		}
	}
}
