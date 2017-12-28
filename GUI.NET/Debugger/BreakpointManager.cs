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

			//Don't refresh breakpoints, doing so will cause them to be enabled even if the debugger window is closed
			//RefreshBreakpoints();
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

		public static Breakpoint GetMatchingBreakpoint(int address)
		{
			return Breakpoints.FirstOrDefault<Breakpoint>((bp) => { return bp.Matches(address); });
		}

		public static Breakpoint GetMatchingBreakpoint(UInt32 startAddress, UInt32 endAddress, bool ppuBreakpoint)
		{
			bool isAddressRange = startAddress != endAddress;
			return Breakpoints.Where((bp) =>
					!bp.IsAbsoluteAddress &&
					((!isAddressRange && bp.Address == startAddress) || (isAddressRange && bp.StartAddress == startAddress && bp.EndAddress == endAddress)) &&
					((!ppuBreakpoint && (bp.BreakOnExec || bp.BreakOnRead || bp.BreakOnWrite)) ||
					 (ppuBreakpoint && (bp.BreakOnReadVram || bp.BreakOnWriteVram)))
				).FirstOrDefault();
		}

		public static void ToggleBreakpoint(int address, bool toggleEnabled)
		{
			if(address >= 0) {
				Breakpoint breakpoint = BreakpointManager.GetMatchingBreakpoint(address);
				if(breakpoint != null) {
					if(toggleEnabled) {
						breakpoint.SetEnabled(!breakpoint.Enabled);
					} else {
						BreakpointManager.RemoveBreakpoint(breakpoint);
					}
				} else {
					breakpoint = new Breakpoint() {
						BreakOnExec = true,
						Address = (UInt32)address,
						IsAbsoluteAddress = false,
						Enabled = true
					};
					BreakpointManager.AddBreakpoint(breakpoint);
				}
			}
		}

		public static void SetBreakpoints()
		{
			List<InteropBreakpoint> breakpoints = new List<InteropBreakpoint>();
			foreach(Breakpoint bp in Breakpoints) {
				if(bp.Enabled) {
					breakpoints.Add(bp.ToInteropBreakpoint());
				}
			}
			InteropEmu.DebugSetBreakpoints(breakpoints.ToArray(), (UInt32)breakpoints.Count);
		}
	}
}
