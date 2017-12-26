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
		private const int MaxRecentScripts = 10;

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
		public bool HighlightChrTile = false;
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
		public bool RamShowLabelInfo = true;
		public bool RamHighlightExecution = true;
		public bool RamHighlightWrites = true;
		public bool RamHighlightReads = true;
		public int RamFadeSpeed = 300;
		public bool RamHideUnusedBytes = false;
		public bool RamHideReadBytes = false;
		public bool RamHideWrittenBytes = false;
		public bool RamHideExecutedBytes = false;
		public bool RamHighlightChrDrawnBytes = false;
		public bool RamHighlightChrReadBytes = false;
		public bool RamHighlightCodeBytes = false;
		public bool RamHighlightDataBytes = false;
		public XmlColor RamReadColor = Color.Blue;
		public XmlColor RamWriteColor = Color.Red;
		public XmlColor RamExecColor = Color.Green;
		public XmlColor RamCodeByteColor = Color.DarkSeaGreen;
		public XmlColor RamDataByteColor = Color.LightSteelBlue;
		public XmlColor RamChrDrawnByteColor = Color.DarkSeaGreen;
		public XmlColor RamChrReadByteColor = Color.LightSteelBlue;

		public Size MemoryViewerSize = new Size(0, 0);

		public int WindowWidth = -1;
		public int WindowHeight = -1;

		public int BreakInCount = 1;
		public bool BreakInPpuCycles = false;

		public bool HighlightUnexecutedCode = true;

		public bool FindOccurrencesMatchCase = false;
		public bool FindOccurrencesMatchWholeWord = false;
		public string FindOccurrencesLastSearch = string.Empty;

		public bool AutoLoadDbgFiles = false;
		public bool AutoLoadCdlFiles = false;
		public bool DisableDefaultLabels = false;

		public bool RefreshWatchWhileRunning = false;

		public bool BreakOnOpen = true;
		public bool BreakOnReset = true;
		public bool BreakOnUnofficialOpcodes = true;
		public bool BreakOnBrk = false;
		public bool BreakOnDebuggerFocus = false;

		public TraceLoggerOptions TraceLoggerOptions;
		public bool TraceAutoRefresh = true;
		public int TraceLineCount = 1000;
		public bool TraceIndentCode = false;
		public Size TraceLoggerSize = new Size(0, 0);

		public Size ScriptWindowSize = new Size(0, 0);
		public int ScriptCodeWindowHeight = 0;
		public List<string> RecentScripts = new List<string>();
		public bool SaveScriptBeforeRun = true;
		public ScriptStartupBehavior ScriptStartupBehavior = ScriptStartupBehavior.ShowTutorial;
		public bool AutoLoadLastScript = true;
		public int ScriptZoom = 100;

		public bool AssemblerCodeHighlighting = true;
		public XmlColor AssemblerOpcodeColor = Color.DarkSlateGray;
		public XmlColor AssemblerLabelDefinitionColor = Color.Blue;
		public XmlColor AssemblerImmediateColor = Color.Chocolate;
		public XmlColor AssemblerAddressColor = Color.DarkRed;
		public XmlColor AssemblerCommentColor = Color.Green;
		public Size AssemblerSize = new Size(0, 0);
		public int AssemblerZoom = 100;

		public DebugInfo()
		{
			LeftView = new DebugViewInfo();
			RightView = new DebugViewInfo();
			TraceLoggerOptions = new TraceLoggerOptions() {
				ShowByteCode = true,
				ShowCpuCycles = false,
				ShowEffectiveAddresses = true,
				ShowExtraInfo = true,
				ShowPpuFrames = false,
				ShowPpuCycles = true,
				ShowPpuScanline = true,
				ShowRegisters = true,
				UseLabels = false,
				StatusFormat = StatusFlagFormat.Hexadecimal
			};
		}

		public void AddRecentScript(string scriptFile)
		{
			string existingItem = RecentScripts.Where((file) => file == scriptFile).FirstOrDefault();
			if(existingItem != null) {
				RecentScripts.Remove(existingItem);
			}

			RecentScripts.Insert(0, scriptFile);
			if(RecentScripts.Count > DebugInfo.MaxRecentScripts) {
				RecentScripts.RemoveAt(DebugInfo.MaxRecentScripts);
			}
			ConfigManager.ApplyChanges();
		}
	}

	public enum ScriptStartupBehavior
	{
		ShowTutorial = 0,
		ShowBlankWindow = 1,
		LoadLastScript = 2
	}


	public class XmlColor
	{
		private Color _color = Color.Black;

		public XmlColor() { }
		public XmlColor(Color c) { _color = c; }

		[XmlIgnore]
		public Color Color
		{
			get { return _color; }
			set { _color = value; }
		}
		
		public static implicit operator Color(XmlColor x)
		{
			return x.Color;
		}

		public static implicit operator XmlColor(Color c)
		{
			return new XmlColor(c);
		}

		[XmlAttribute]
		public string ColorString
		{
			get { return ColorTranslator.ToHtml(_color); }
			set
			{
				try {
					_color = ColorTranslator.FromHtml(value);
				} catch(Exception) {
					_color = Color.Black;
				}
			}
		}
	}
}
