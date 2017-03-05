using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Mesen.GUI.Debugger;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Config
{
	public enum ByteCodePosition
	{
		Hidden,
		Left,
		Below
	}

	public enum PrgAddressPosition
	{
		Hidden,
		Replace,
		Below
	}

	public class DebugViewInfo
	{
		public ByteCodePosition ByteCodePosition = ByteCodePosition.Hidden;
		public PrgAddressPosition PrgAddressPosition = PrgAddressPosition.Hidden;
		public float FontSize = BaseControl.DefaultFontSize;
	}

	public class DebugWorkspace
	{
		public List<Breakpoint> Breakpoints = new List<Breakpoint>();
		public List<string> WatchValues = new List<string>();
		public List<CodeLabel> Labels = new List<CodeLabel>();
		public List<string> TblMappings = null;
		private string _filePath;

		public static DebugWorkspace GetWorkspace()
		{
			RomInfo info = InteropEmu.GetRomInfo();
			return Deserialize(Path.Combine(ConfigManager.DebuggerFolder, info.GetRomName() + ".Workspace.xml"));
		}

		private static DebugWorkspace Deserialize(string path)
		{
			DebugWorkspace config = config = new DebugWorkspace();

			if(File.Exists(path)) {
				try {
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(DebugWorkspace));
					using(TextReader textReader = new StreamReader(path)) {
						config = (DebugWorkspace)xmlSerializer.Deserialize(textReader);
					}
				} catch { }
			} 

			config._filePath = path;

			return config;
		}

		public void Save()
		{
			try {
				XmlWriterSettings ws = new XmlWriterSettings();
				ws.NewLineHandling = NewLineHandling.Entitize;

				XmlSerializer xmlSerializer = new XmlSerializer(typeof(DebugWorkspace));
				using(XmlWriter xmlWriter = XmlWriter.Create(_filePath, ws)) {
					xmlSerializer.Serialize(xmlWriter, this);
				}
			} catch {
			}
		}
	}

	public enum DisassemblyType
	{
		VerifiedCode,
		Everything,
		EverythingButData
	}

	public class DebugInfo
	{
		public DebugViewInfo LeftView;
		public DebugViewInfo RightView;

		public bool ShowOnlyDisassembledCode = true;
		public bool DisplayOpCodesInLowerCase = false;
		public bool ShowEffectiveAddresses = true;
		public DisassemblyType DisassemblyType = DisassemblyType.VerifiedCode;

		public bool SplitView = false;
		public bool HexDisplay = true;

		public bool PpuAutoRefresh = true;
		public bool PpuPartialDraw = false;
		public bool ShowPpuScrollOverlay = true;
		public bool ShowTileGrid = false;
		public bool ShowAttributeGrid = false;
		public int PpuDisplayCycle = 0;
		public int PpuDisplayScanline = 241;

		public bool ShowCpuMemoryMapping = true;
		public bool ShowPpuMemoryMapping = true;

		public bool ShowRightPanel = true;
		public bool ShowBottomPanel = true;
		public int LeftPanelWidth = 930;
		public int TopPanelHeight = 450;

		public bool RamAutoRefresh = true;
		public int RamColumnCount = 2;
		public float RamFontSize = BaseControl.DefaultFontSize;
		public bool RamShowCharacters = true;
		public bool RamHighlightExecution = true;
		public bool RamHighlightWrites = true;
		public bool RamHighlightReads = true;
		public int RamFadeSpeed = 300;
		public bool RamHideUnusedBytes;
		public bool RamHideReadBytes;
		public bool RamHideWrittenBytes;
		public bool RamHideExecutedBytes;

		public int WindowWidth = -1;
		public int WindowHeight = -1;

		public int BreakInCount = 1;
		public bool BreakInPpuCycles = false;

		public bool HighlightUnexecutedCode = true;
		
		public bool FindOccurrencesMatchCase = false;
		public bool FindOccurrencesMatchWholeWord = false;
		public string FindOccurrencesLastSearch = string.Empty;

		public bool AutoLoadDbgFiles = false;
		public bool DisableDefaultLabels = false;

		public bool BreakOnOpen = true;
		public bool BreakOnReset = true;
		public bool BreakOnUnofficialOpcodes = true;
		public bool BreakOnBrk = false;

		public bool TraceAutoRefresh = true;
		public int TraceLineCount = 1000;
		public bool TraceIndentCode = false;
		public bool TraceShowByteCode = true;
		public bool TraceShowCpuCycles = false;
		public bool TraceShowEffectiveAddresses = true;
		public bool TraceShowExtraInfo = true;
		public bool TraceShowFrameCount = false;
		public bool TraceShowPpuCycles = true;
		public bool TraceShowPpuScanline = true;
		public bool TraceShowRegisters = true;
		public bool TraceUseLabels = false;

		public DebugInfo()
		{
			LeftView = new DebugViewInfo();
			RightView = new DebugViewInfo();
		}
	}
}
