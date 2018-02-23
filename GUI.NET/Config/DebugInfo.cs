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
	
	public class DebugInfo
	{
		private const int MaxRecentScripts = 10;

		public DebugViewInfo LeftView;
		public DebugViewInfo RightView;

		public bool ShowOnlyDisassembledCode = true;
		public bool DisplayOpCodesInLowerCase = false;
		public bool ShowEffectiveAddresses = true;

		public bool DisassembleVerifiedData = false;
		public bool DisassembleUnidentifiedData = false;

		public bool ShowVerifiedData = false;
		public bool ShowUnidentifiedData = false;

		public bool SplitView = false;
		public bool HexDisplay = true;
		public bool ShowBreakpointLabels = true;

		public Size EventViewerSize = new Size(0, 0);
		public bool EventViewerShowPpuRegisterWrites = true;
		public bool EventViewerShowPpuRegisterReads = true;
		public bool EventViewerShowMapperRegisterWrites = true;
		public bool EventViewerShowMapperRegisterReads = true;
		public bool EventViewerShowNmi = true;
		public bool EventViewerShowIrq = true;
		public bool EventViewerShowSpriteZeroHit = true;
		public bool EventViewerShowMarkedBreakpoints = true;

		public XmlColor EventViewerMapperRegisterWriteColor = ColorTranslator.FromHtml("#007597");
		public XmlColor EventViewerMapperRegisterReadColor = ColorTranslator.FromHtml("#C92929");
		public XmlColor EventViewerNmiColor = ColorTranslator.FromHtml("#ABADAC");
		public XmlColor EventViewerIrqColor = ColorTranslator.FromHtml("#F9FEAC");
		public XmlColor EventViewerSpriteZeroHitColor = ColorTranslator.FromHtml("#9F93C6");
		public XmlColor EventViewerBreakpointColor = ColorTranslator.FromHtml("#1898E4");
		public XmlColor[] EventViewerPpuRegisterWriteColors = new XmlColor[8] {
			ColorTranslator.FromHtml("#FF5E5E"), ColorTranslator.FromHtml("#8E33FF"), Color.Red, ColorTranslator.FromHtml("#FF84E0"),
			ColorTranslator.FromHtml("#FAFF39"), ColorTranslator.FromHtml("#2EFF28"), ColorTranslator.FromHtml("#3D2DFF"), ColorTranslator.FromHtml("#FF060D")
		};
		public XmlColor[] EventViewerPpuRegisterReadColors = new XmlColor[8] {
			Color.Red, Color.Red, ColorTranslator.FromHtml("#FF8224"), Color.Red, ColorTranslator.FromHtml("#24A672"), Color.Red, Color.Red, ColorTranslator.FromHtml("#6AF0FF")
		};

		public bool PpuAutoRefresh = true;
		public bool PpuPartialDraw = false;
		public bool PpuShowPreviousFrame = false;
		public bool ShowPpuScrollOverlay = true;
		public bool ShowTileGrid = false;
		public bool ShowAttributeGrid = false;
		public bool HighlightChrTile = false;
		public bool NtViewerUseGrayscalePalette = false;

		public bool ChrViewerUseAutoPalette = true;
		public bool ChrViewerUseLargeSprites = false;

		public int PpuDisplayCycle = 0;
		public int PpuDisplayScanline = 241;

		public bool ShowCodePreview = true;
		public bool ShowOpCodeTooltips = true;

		public bool ShowToolbar = true;

		public bool ShowCpuMemoryMapping = true;
		public bool ShowPpuMemoryMapping = true;

		public bool ShowRightPanel = true;
		public bool ShowBottomPanel = true;
		public int LeftPanelWidth = 930;
		public int TopPanelHeight = 450;

		public XmlColor CodeVerifiedDataColor = Color.FromArgb(255, 252, 236);
		public XmlColor CodeUnidentifiedDataColor = Color.FromArgb(255, 242, 242);
		public XmlColor CodeUnexecutedCodeColor = Color.FromArgb(225, 244, 228);

		public XmlColor CodeExecBreakpointColor = Color.FromArgb(140, 40, 40);
		public XmlColor CodeWriteBreakpointColor = Color.FromArgb(40, 120, 80);
		public XmlColor CodeReadBreakpointColor = Color.FromArgb(40, 40, 200);
		public XmlColor CodeActiveStatementColor = Color.Yellow;
		public XmlColor CodeEffectiveAddressColor = Color.SteelBlue;

		public bool RamHighDensityTextMode = false;
		public bool RamEnablePerByteNavigation = false;
		public bool RamAutoRefresh = true;
		public RefreshSpeed RamAutoRefreshSpeed = RefreshSpeed.Normal;
		public bool RamIgnoreRedundantWrites = false;
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
		public bool RamHighlightLabelledBytes = false;
		public bool RamHighlightChrDrawnBytes = false;
		public bool RamHighlightChrReadBytes = false;
		public bool RamHighlightCodeBytes = false;
		public bool RamHighlightDataBytes = false;
		public bool RamHighlightDmcDataBytes = false;
		public XmlColor RamReadColor = Color.Blue;
		public XmlColor RamWriteColor = Color.Red;
		public XmlColor RamExecColor = Color.Green;
		public XmlColor RamLabelledByteColor = Color.LightPink;
		public XmlColor RamCodeByteColor = Color.DarkSeaGreen;
		public XmlColor RamDataByteColor = Color.LightSteelBlue;
		public XmlColor RamDmcDataByteColor = Color.Gold;
		public XmlColor RamChrDrawnByteColor = Color.DarkSeaGreen;
		public XmlColor RamChrReadByteColor = Color.LightSteelBlue;

		public Size MemoryViewerSize = new Size(0, 0);

		public int WindowWidth = -1;
		public int WindowHeight = -1;

		public int BreakInCount = 1;
		public BreakInMetric BreakInMetric = BreakInMetric.CpuCycles;

		public bool FindOccurrencesMatchCase = false;
		public bool FindOccurrencesMatchWholeWord = false;
		public string FindOccurrencesLastSearch = string.Empty;

		public bool AutoLoadDbgFiles = false;
		public bool AutoLoadCdlFiles = false;
		public bool DisableDefaultLabels = false;

		public bool RefreshWatchWhileRunning = false;
		public bool ShowMemoryValuesInCodeWindow = true;

		public bool BreakOnOpen = true;
		public bool BreakOnReset = true;
		public bool BreakOnUnofficialOpcodes = true;
		public bool BreakOnBrk = false;
		public bool BreakOnDebuggerFocus = false;
		public bool BreakOnCrash = false;

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
		public XmlColor AssemblerOpcodeColor = Color.FromArgb(22, 37, 37);
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

		static public void ApplyConfig()
		{
			InteropEmu.SetFlag(EmulationFlags.BreakOnCrash, ConfigManager.Config.DebugInfo.BreakOnCrash);
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

	public enum RefreshSpeed
	{
		Low = 0,
		Normal = 1,
		High = 2
	}

	public enum BreakInMetric
	{
		CpuCycles,
		CpuInstructions,
		PpuCycles,
		Scanlines,
		Frames
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
