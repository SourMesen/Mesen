﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using Mesen.GUI.Forms;
using static Mesen.GUI.Forms.BaseForm;

namespace Mesen.GUI.Config
{
	public class DebuggerShortcutsConfig
	{
		//Shared
		[ShortcutName("Increase Font Size")]
		public XmlKeys IncreaseFontSize = Keys.Control | Keys.Oemplus;
		[ShortcutName("Decrease Font Size")]
		public XmlKeys DecreaseFontSize = Keys.Control | Keys.OemMinus;
		[ShortcutName("Reset Font Size")]
		public XmlKeys ResetFontSize = Keys.Control | Keys.D0;

		[ShortcutName("Go To...")]
		public XmlKeys GoTo = Keys.Control | Keys.G;

		[ShortcutName("Find")]
		public XmlKeys Find = Keys.Control | Keys.F;
		[ShortcutName("Find Next")]
		public XmlKeys FindNext = Keys.F3;
		[ShortcutName("Find Previous")]
		public XmlKeys FindPrev = Keys.Shift | Keys.F3;

		[ShortcutName("Undo")]
		public XmlKeys Undo = Keys.Control | Keys.Z;
		[ShortcutName("Copy")]
		public XmlKeys Copy = Keys.Control | Keys.C;
		[ShortcutName("Cut")]
		public XmlKeys Cut = Keys.Control | Keys.X;
		[ShortcutName("Paste")]
		public XmlKeys Paste = Keys.Control | Keys.V;
		[ShortcutName("Select All")]
		public XmlKeys SelectAll = Keys.Control | Keys.A;

		[ShortcutName("Refresh")]
		public XmlKeys Refresh = Keys.F5;

		[ShortcutName("Mark Selection as Code")]
		public XmlKeys MarkAsCode = Keys.Control | Keys.D1;
		[ShortcutName("Mark Selection as Data")]
		public XmlKeys MarkAsData = Keys.Control | Keys.D2;
		[ShortcutName("Mark Selection as Unidentified Code/Data")]
		public XmlKeys MarkAsUnidentified = Keys.Control | Keys.D3;

		[ShortcutName("Go to All")]
		public XmlKeys GoToAll = Keys.Control | Keys.Oemcomma;

		[ShortcutName("Zoom In")]
		public XmlKeys ZoomIn = Keys.Control | Keys.Oemplus;
		[ShortcutName("Zoom Out")]
		public XmlKeys ZoomOut = Keys.Control | Keys.OemMinus;

		[ShortcutName("PPU Viewer: Toggle View")]
		public XmlKeys PpuViewer_ToggleView = Keys.Control | Keys.Q;
		[ShortcutName("PPU Viewer: Toggle Zoom")]
		public XmlKeys PpuViewer_ToggleZoom = Keys.Control | Keys.W;

		[ShortcutName("Nametable Viewer: Add breakpoint (Tile)")]
		public XmlKeys PpuViewer_AddBreakpointTile = Keys.F9;
		[ShortcutName("Nametable Viewer: Add breakpoint (Attribute)")]
		public XmlKeys PpuViewer_AddBreakpointAttribute = Keys.F10;

		[ShortcutName("Edit in Memory Viewer")]
		public XmlKeys CodeWindow_EditInMemoryViewer = Keys.F1;
		[ShortcutName("View in disassembly")]
		public XmlKeys MemoryViewer_ViewInDisassembly = Keys.None;

		[ShortcutName("Open APU Viewer")]
		public XmlKeys OpenApuViewer = Keys.Control | Keys.U;
		[ShortcutName("Open Assembler")]
		public XmlKeys OpenAssembler = Keys.Control | Keys.K;
		[ShortcutName("Open Debugger")]
		public XmlKeys OpenDebugger = Keys.Control | Keys.D;
		[ShortcutName("Open Event Viewer")]
		public XmlKeys OpenEventViewer = Keys.Control | Keys.E;
		[ShortcutName("Open Memory Tools")]
		public XmlKeys OpenMemoryTools = Keys.Control | Keys.M;
		[ShortcutName("Open PPU Viewer")]
		public XmlKeys OpenPpuViewer = Keys.Control | Keys.P;
		[ShortcutName("Open Performance Profiler")]
		public XmlKeys OpenProfiler = Keys.Control | Keys.Y;
		[ShortcutName("Open Script Window")]
		public XmlKeys OpenScriptWindow = Keys.Control | Keys.N;
		[ShortcutName("Open Trace Logger")]
		public XmlKeys OpenTraceLogger = Keys.Control | Keys.J;
		[ShortcutName("Open Text Hooker")]
		public XmlKeys OpenTextHooker = Keys.Control | Keys.H;
		[ShortcutName("Open Watch Window")]
		public XmlKeys OpenWatchWindow = Keys.Control | Keys.W;

		[ShortcutName("Open Nametable Viewer (Compact)")]
		public XmlKeys OpenNametableViewer = Keys.None;
		[ShortcutName("Open CHR Viewer (Compact)")]
		public XmlKeys OpenChrViewer = Keys.None;
		[ShortcutName("Open Sprite Viewer (Compact)")]
		public XmlKeys OpenSpriteViewer = Keys.None;
		[ShortcutName("Open Palette Viewer (Compact)")]
		public XmlKeys OpenPaletteViewer = Keys.None;

		//Debugger window
		[ShortcutName("Reset")]
		public XmlKeys Reset = Keys.Control | Keys.R;
		[ShortcutName("Power Cycle")]
		public XmlKeys PowerCycle = Keys.Control | Keys.T;

		[ShortcutName("Continue")]
		public XmlKeys Continue = Keys.F5;
		[ShortcutName("Break")]
		public XmlKeys Break = Keys.Control | Keys.Alt | Keys.Cancel;
		[ShortcutName("Toggle Break/Continue")]
		public XmlKeys ToggleBreakContinue = Keys.Escape;
		[ShortcutName("Step Into")]
		public XmlKeys StepInto = Keys.F11;
		[ShortcutName("Step Over")]
		public XmlKeys StepOver = Keys.F10;
		[ShortcutName("Step Out")]
		public XmlKeys StepOut = Keys.Shift | Keys.F11;
		[ShortcutName("Step Back")]
		public XmlKeys StepBack = Keys.Shift | Keys.F10;

		[ShortcutName("Run one CPU Cycle")]
		public XmlKeys RunCpuCycle = Keys.None;
		[ShortcutName("Run one PPU Cycle")]
		public XmlKeys RunPpuCycle = Keys.F6;
		[ShortcutName("Run one scanline")]
		public XmlKeys RunPpuScanline = Keys.F7;
		[ShortcutName("Run one frame")]
		public XmlKeys RunPpuFrame = Keys.F8;

		[ShortcutName("Break In...")]
		public XmlKeys BreakIn = Keys.Control | Keys.B;
		[ShortcutName("Break On...")]
		public XmlKeys BreakOn = Keys.Alt | Keys.B;

		[ShortcutName("Find Occurrences")]
		public XmlKeys FindOccurrences = Keys.Control | Keys.Shift | Keys.F;
		[ShortcutName("Go To Program Counter")]
		public XmlKeys GoToProgramCounter = Keys.Alt | Keys.Multiply;

		[ShortcutName("Toggle Verified Data Display")]
		public XmlKeys ToggleVerifiedData = Keys.Alt | Keys.D1;
		[ShortcutName("Toggle Unidentified Code/Data Display")]
		public XmlKeys ToggleUnidentifiedCodeData = Keys.Alt | Keys.D2;

		[ShortcutName("Code Window: Set Next Statement")]
		public XmlKeys CodeWindow_SetNextStatement = Keys.Control | Keys.Shift | Keys.F10;
		[ShortcutName("Code Window: Edit Subroutine")]
		public XmlKeys CodeWindow_EditSubroutine = Keys.F4;
		[ShortcutName("Code Window: Edit Selected Code")]
		public XmlKeys CodeWindow_EditSelectedCode = Keys.None;
		[ShortcutName("Code Window: Edit Source File (Source View)")]
		public XmlKeys CodeWindow_EditSourceFile = Keys.F4;
		[ShortcutName("Code Window: Edit Label")]
		public XmlKeys CodeWindow_EditLabel = Keys.F2;
		[ShortcutName("Code Window: Navigate Back")]
		public XmlKeys CodeWindow_NavigateBack = Keys.Alt | Keys.Left;
		[ShortcutName("Code Window: Navigate Forward")]
		public XmlKeys CodeWindow_NavigateForward = Keys.Alt | Keys.Right;
		[ShortcutName("Code Window: Toggle Breakpoint")]
		public XmlKeys CodeWindow_ToggleBreakpoint = Keys.F9;
		[ShortcutName("Code Window: Disable/Enable Breakpoint")]
		public XmlKeys CodeWindow_DisableEnableBreakpoint = Keys.Control | Keys.F9;
		[ShortcutName("Code Window: Switch View (Disassembly / Source View)")]
		public XmlKeys CodeWindow_SwitchView = Keys.Control | Keys.Q;

		[ShortcutName("Function List: Edit Label")]
		public XmlKeys FunctionList_EditLabel = Keys.F2;
		[ShortcutName("Function List: Add Breakpoint")]
		public XmlKeys FunctionList_AddBreakpoint = Keys.None;
		[ShortcutName("Function List: Find Occurrences")]
		public XmlKeys FunctionList_FindOccurrences = Keys.None;

		[ShortcutName("Label List: Add Label")]
		public XmlKeys LabelList_Add = Keys.Insert;
		[ShortcutName("Label List: Edit Label")]
		public XmlKeys LabelList_Edit = Keys.F2;
		[ShortcutName("Label List: Delete Label")]
		public XmlKeys LabelList_Delete = Keys.Delete;
		[ShortcutName("Label List: Add Breakpoint")]
		public XmlKeys LabelList_AddBreakpoint = Keys.None;
		[ShortcutName("Label List: Add to Watch")]
		public XmlKeys LabelList_AddToWatch = Keys.None;
		[ShortcutName("Label List: Find Occurrences")]
		public XmlKeys LabelList_FindOccurrences = Keys.None;
		[ShortcutName("Label List: View in CPU Memory")]
		public XmlKeys LabelList_ViewInCpuMemory = Keys.None;
		[ShortcutName("Label List: View in [memory type]")]
		public XmlKeys LabelList_ViewInMemoryType = Keys.None;

		[ShortcutName("Breakpoint List: Add Breakpoint")]
		public XmlKeys BreakpointList_Add = Keys.Insert;
		[ShortcutName("Breakpoint List: Edit Breakpoint")]
		public XmlKeys BreakpointList_Edit = Keys.F2;
		[ShortcutName("Breakpoint List: Go To Location")]
		public XmlKeys BreakpointList_GoToLocation = Keys.None;
		[ShortcutName("Breakpoint List: Delete Breakpoint")]
		public XmlKeys BreakpointList_Delete = Keys.Delete;

		[ShortcutName("Watch List: Delete")]
		public XmlKeys WatchList_Delete = Keys.Delete;
		[ShortcutName("Watch List: Move Up")]
		public XmlKeys WatchList_MoveUp = Keys.Control | Keys.Up;
		[ShortcutName("Watch List: Move Down")]
		public XmlKeys WatchList_MoveDown = Keys.Control | Keys.Down;

		[ShortcutName("Save Rom")]
		public XmlKeys SaveRom = Keys.Control | Keys.S;
		[ShortcutName("Save Rom As...")]
		public XmlKeys SaveRomAs = Keys.None;
		[ShortcutName("Save edits as IPS patch...")]
		public XmlKeys SaveEditAsIps = Keys.None;
		[ShortcutName("Revert PRG/CHR changes")]
		public XmlKeys RevertPrgChrChanges = Keys.None;

		//Memory Tools
		[ShortcutName("Freeze")]
		public XmlKeys MemoryViewer_Freeze = Keys.Control | Keys.Q;
		[ShortcutName("Unfreeze")]
		public XmlKeys MemoryViewer_Unfreeze = Keys.Control | Keys.W;
		[ShortcutName("Add to Watch")]
		public XmlKeys MemoryViewer_AddToWatch = Keys.None;
		[ShortcutName("Edit Breakpoint")]
		public XmlKeys MemoryViewer_EditBreakpoint = Keys.None;
		[ShortcutName("Edit Label")]
		public XmlKeys MemoryViewer_EditLabel = Keys.None;
		[ShortcutName("Import")]
		public XmlKeys MemoryViewer_Import = Keys.Control | Keys.O;
		[ShortcutName("Export")]
		public XmlKeys MemoryViewer_Export = Keys.Control | Keys.S;
		[ShortcutName("View in CPU/PPU Memory")]
		public XmlKeys MemoryViewer_ViewInCpuMemory = Keys.None;
		[ShortcutName("View in [memory type]")]
		public XmlKeys MemoryViewer_ViewInMemoryType = Keys.None;

		//Script Window
		[ShortcutName("Open Script")]
		public XmlKeys ScriptWindow_OpenScript = Keys.Control | Keys.N;
		[ShortcutName("Save Script")]
		public XmlKeys ScriptWindow_SaveScript = Keys.Control | Keys.S;
		[ShortcutName("Run Script")]
		public XmlKeys ScriptWindow_RunScript = Keys.F5;
		[ShortcutName("Stop Script")]
		public XmlKeys ScriptWindow_StopScript = Keys.Escape;

		public static string GetShortcutDisplay(Keys keys)
		{
			if(keys == Keys.None) {
				return "";
			} else {
				string keyString = new KeysConverter().ConvertToString(keys);
				return keyString.Replace("+None", "").Replace("Oemcomma", ",").Replace("Oemplus", "+").Replace("Oemtilde", "Tilde").Replace("OemMinus", "-").Replace("Cancel", "Break").Replace("Escape", "Esc");
			}
		}
		
		private static Dictionary<WeakReference<ToolStripMenuItem>, string> _bindings = new Dictionary<WeakReference<ToolStripMenuItem>, string>();
		private static Dictionary<WeakReference<ToolStripMenuItem>, WeakReference<Control>> _parents = new Dictionary<WeakReference<ToolStripMenuItem>, WeakReference<Control>>();
		public static void RegisterMenuItem(ToolStripMenuItem item, Control parent, string fieldName)
		{
			var weakRef = new WeakReference<ToolStripMenuItem>(item);
			_bindings[weakRef] = fieldName;
			_parents[weakRef] = new WeakReference<Control>(parent);

			//Remove old references
			var dictCopy = new Dictionary<WeakReference<ToolStripMenuItem>, string>(_bindings);

			//Iterate on a copy to avoid "collection was modified" error
			foreach(var kvp in dictCopy) {
				ToolStripMenuItem menuItem;
				if(!kvp.Key.TryGetTarget(out menuItem)) {
					_bindings.Remove(kvp.Key);
					_parents.Remove(kvp.Key);
				}
			}
		}

		public static void UpdateMenus()
		{
			foreach(WeakReference<ToolStripMenuItem> itemRef in _bindings.Keys) {
				ToolStripMenuItem item;
				if(itemRef.TryGetTarget(out item)) {
					string fieldName = _bindings[itemRef];
					Control parent;
					_parents[itemRef].TryGetTarget(out parent);
					if(parent != null) {
						UpdateShortcutItem(item, parent, fieldName);
					}
				}
			}
		}

		public static void ClearProcessCmdKeyHandler(ToolStripMenuItem item, Control parent)
		{
			Form parentForm = parent.FindForm();
			if(parentForm is BaseForm) {
				(parentForm as BaseForm).OnProcessCmdKey -= ((ShortcutInfo)item.Tag).KeyHandler;
			}
			((ShortcutInfo)item.Tag).KeyHandler = null;
		}

		public static void UpdateShortcutItem(ToolStripMenuItem item, Control parent, string fieldName)
		{
			if(item.Tag == null) {
				item.Tag = new ShortcutInfo() { KeyHandler = null, ShortcutKey = fieldName };
			} else if(((ShortcutInfo)item.Tag).KeyHandler != null) {
				ClearProcessCmdKeyHandler(item, parent);
			}

			Keys keys = (XmlKeys)typeof(DebuggerShortcutsConfig).GetField(fieldName).GetValue(ConfigManager.Config.DebugInfo.Shortcuts);
			if((keys != Keys.None && !ToolStripManager.IsValidShortcut(keys)) || Program.IsMono) {
				//Support normally invalid shortcut keys as a shortcut
				item.ShortcutKeys = Keys.None;
				item.ShortcutKeyDisplayString = GetShortcutDisplay(keys);

				Form parentForm = parent.FindForm();
				if(parentForm is BaseForm) {
					ProcessCmdKeyHandler onProcessCmdKeyHandler = (Keys keyData, ref bool processed) => {
						if(!processed && item.Enabled && parent.ContainsFocus && keyData == keys) {
							item.PerformClick();
							processed = true;
						}
					};

					((ShortcutInfo)item.Tag).KeyHandler = onProcessCmdKeyHandler;
					(parentForm as BaseForm).OnProcessCmdKey += onProcessCmdKeyHandler;
				}
			} else {
				item.ShortcutKeys = keys;
				item.ShortcutKeyDisplayString = GetShortcutDisplay(keys);
			}
		}
	}

	public static class ToolStripMenuItemExtensions
	{
		public static void InitShortcut(this ToolStripMenuItem item, Control parent, string fieldName)
		{
			DebuggerShortcutsConfig.UpdateShortcutItem(item, parent, fieldName);
			DebuggerShortcutsConfig.RegisterMenuItem(item, parent, fieldName);
		}
	}

	public class ShortcutInfo
	{
		public string ShortcutKey;
		public ProcessCmdKeyHandler KeyHandler;
	}

	public class XmlKeys
	{
		private Keys _keys = Keys.None;

		public XmlKeys() { }
		public XmlKeys(Keys k) { _keys = k; }

		public static implicit operator Keys(XmlKeys k)
		{
			return k._keys;
		}

		public static implicit operator XmlKeys(Keys k)
		{
			return new XmlKeys(k);
		}

		[XmlAttribute]
		public string Value
		{
			get { return _keys.ToString(); }
			set
			{
				try {
					Enum.TryParse<Keys>(value, out _keys);
				} catch(Exception) {
					_keys = Keys.None;
				}
			}
		}
	}

	public class ShortcutNameAttribute : Attribute
	{
		public string Name { get; private set; }

		public ShortcutNameAttribute(string name)
		{
			this.Name = name;
		}
	}
}
