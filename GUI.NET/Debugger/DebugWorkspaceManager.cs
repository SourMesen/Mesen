using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger
{
	public class DebugWorkspaceManager
	{
		private static DebugWorkspace _workspace;
		private static Ld65DbgImporter _symbolProvider;
		private static string _romName;
		private static object _lock = new object();

		public delegate void SymbolProviderChangedHandler(Ld65DbgImporter provider);
		public static event SymbolProviderChangedHandler SymbolProviderChanged;

		public static Ld65DbgImporter SymbolProvider
		{
			get { return _symbolProvider; }
			private set
			{
				if(_symbolProvider != value) {
					_symbolProvider = value;
					SymbolProviderChanged?.Invoke(value);
				}
			}
		}

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
				SymbolProvider = null;
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

		public static DebugWorkspace GetWorkspace()
		{
			string romName = InteropEmu.GetRomInfo().GetRomName();
			if(_workspace != null) {
				SaveWorkspace();
			}

			if(_workspace == null || _romName != romName) {
				SymbolProvider = null;
				lock(_lock) {
					if(_workspace == null || _romName != romName) {
						_romName = InteropEmu.GetRomInfo().GetRomName();
						_workspace = DebugWorkspace.GetWorkspace();

						//Load watch entries
						WatchManager.WatchEntries = _workspace.WatchValues;

						//Setup labels
						if(_workspace.Labels.Count == 0 && !ConfigManager.Config.DebugInfo.DisableDefaultLabels) {
							LabelManager.SetDefaultLabels(InteropEmu.GetRomInfo().MapperId);
						}
					}
				}
			}

			//Send breakpoints & labels to emulation core (even if the same game is running)
			BreakpointManager.SetBreakpoints(_workspace.Breakpoints);
			LabelManager.RefreshLabels();

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

		public static void ResetLabels()
		{
			LabelManager.ResetLabels();
			if(!ConfigManager.Config.DebugInfo.DisableDefaultLabels) {
				LabelManager.SetDefaultLabels(InteropEmu.GetRomInfo().MapperId);
			}
			SaveWorkspace();
			GetWorkspace();
		}

		public static void AutoLoadDbgFiles(bool silent)
		{
			if(ConfigManager.Config.DebugInfo.AutoLoadDbgFiles) {
				RomInfo info = InteropEmu.GetRomInfo();
				string dbgPath = Path.Combine(info.RomFile.Folder, info.GetRomName() + ".dbg");
				if(File.Exists(dbgPath)) {
					DateTime lastDbgUpdate = File.GetLastWriteTime(dbgPath);
					if(lastDbgUpdate != SymbolProvider?.DbgFileStamp) {
						ImportDbgFile(dbgPath, silent);
					} else {
						//Currently loaded symbol provider is still valid
						return;
					}
				} else {
					string mlbPath = Path.Combine(info.RomFile.Folder, info.GetRomName() + ".mlb");
					if(File.Exists(mlbPath)) {
						ImportMlbFile(mlbPath, silent);
					} else {
						string fnsPath = Path.Combine(info.RomFile.Folder, info.GetRomName() + ".fns");
						if(File.Exists(fnsPath)) {
							ImportNesasmFnsFile(fnsPath, silent);
						}
					}
				}
			}
		}

		public static void ImportNesasmFnsFile(string fnsPath, bool silent = false)
		{
			if(ConfigManager.Config.DebugInfo.ImportConfig.ResetLabelsOnImport) {
				ResetLabels();
			}
			NesasmFnsImporter.Import(fnsPath, silent);
		}

		public static void ImportMlbFile(string mlbPath, bool silent = false)
		{
			if(ConfigManager.Config.DebugInfo.ImportConfig.ResetLabelsOnImport) {
				ResetLabels();
			}
			MesenLabelFile.Import(mlbPath, silent);
		}

		public static void ImportDbgFile(string dbgPath, bool silent)
		{
			if(ConfigManager.Config.DebugInfo.ImportConfig.ResetLabelsOnImport) {
				ResetLabels();
			}

			Ld65DbgImporter dbgImporter = new Ld65DbgImporter();
			dbgImporter.Import(dbgPath, silent);

			DebugWorkspaceManager.SymbolProvider = dbgImporter;
		}
	}
}
