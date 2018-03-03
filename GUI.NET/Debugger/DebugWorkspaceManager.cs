using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger
{
	public class DebugWorkspaceManager
	{
		private static DebugWorkspace _workspace;
		private static string _romName;
		private static object _lock = new object();

		public static void SaveWorkspace()
		{
			if(_workspace != null) {
				lock(_lock) {
					if(_workspace != null) {
						_workspace.WatchValues = new List<string>(WatchManager.WatchEntries);
						_workspace.Labels = new List<CodeLabel>(LabelManager.GetLabels());
						_workspace.Breakpoints = new List<Breakpoint>(BreakpointManager.Breakpoints);
						_workspace.Save();
					}
				}
			}
		}

		public static void Clear()
		{
			lock(_lock) {
				_workspace = null;
				_romName = null;
			}
		}

		public static void ResetWorkspace()
		{
			if(_workspace != null) {
				lock(_lock) {
					if(_workspace != null) {
						_workspace.Breakpoints = new List<Breakpoint>();
						_workspace.Labels = new List<CodeLabel>();
						_workspace.WatchValues = new List<string>();
						LabelManager.ResetLabels();
						WatchManager.WatchEntries = _workspace.WatchValues;
						BreakpointManager.SetBreakpoints(_workspace.Breakpoints);
						_workspace.Save();
						Clear();
					}
				}
			}
		}

		public static void SetupWorkspace(bool saveCurrentWorkspace = true)
		{
			string romName = InteropEmu.GetRomInfo().GetRomName();
			lock(_lock) {
				if(_workspace != null && _romName == romName) {
					if(saveCurrentWorkspace) {
						SaveWorkspace();
					}

					//Setup labels
					if(_workspace.Labels.Count == 0) {
						LabelManager.ResetLabels();
						if(!ConfigManager.Config.DebugInfo.DisableDefaultLabels) {
							LabelManager.SetDefaultLabels(InteropEmu.GetRomInfo().MapperId);
						}
					} else {
						LabelManager.ResetLabels();
						LabelManager.SetLabels(_workspace.Labels, false);
					}

					//Load watch entries
					WatchManager.WatchEntries = _workspace.WatchValues;

					//Load breakpoints
					BreakpointManager.SetBreakpoints(_workspace.Breakpoints);
				} else {
					Clear();
				}
			}
		}

		public static DebugWorkspace GetWorkspace()
		{
			string romName = InteropEmu.GetRomInfo().GetRomName();
			if(_workspace == null || _romName != romName) {
				lock(_lock) {
					if(_workspace == null || _romName != romName) {
						if(_workspace != null) {
							SaveWorkspace();
						}
						_romName = InteropEmu.GetRomInfo().GetRomName();
						_workspace = DebugWorkspace.GetWorkspace();
						SetupWorkspace(false);						
					}
				}
			}
			return _workspace;
		}
		
		private static DebuggerFlags _flags = DebuggerFlags.None;
		public static void SetFlags(DebuggerFlags flags)
		{
			_flags |= flags;
			InteropEmu.DebugSetFlags(_flags);
		}

		public static void ClearFlags(DebuggerFlags flags = DebuggerFlags.None)
		{
			if(flags == DebuggerFlags.None) {
				_flags = DebuggerFlags.None;
			} else {
				_flags &= ~flags;
			}
			InteropEmu.DebugSetFlags(_flags);
		}
	}
}
