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

namespace Mesen.GUI.Config
{
	public class DebugViewInfo
	{
		public bool ShowByteCode = false;
		public bool ShowPrgAddresses = false;
		public float FontSize = 13;
	}

	public class DebugWorkspace
	{
		public List<Breakpoint> Breakpoints = new List<Breakpoint>();
		public List<string> WatchValues = new List<string>();
		public List<CodeLabel> Labels = new List<CodeLabel>();
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
		public DebugViewInfo LeftView;
		public DebugViewInfo RightView;

		public bool ShowOnlyDisassembledCode = true;

		public bool SplitView = false;
		public bool HexDisplay = true;

		public bool PpuAutoRefresh = true;
		public bool PpuPartialDraw = false;
		public bool ShowPpuScrollOverlay = true;
		public int PpuDisplayCycle = 0;
		public int PpuDisplayScanline = 241;

		public bool ShowEffectiveAddresses = true;

		public bool ShowCpuMemoryMapping = true;
		public bool ShowPpuMemoryMapping = true;
		public bool ShowFunctionLabelLists = true;

		public bool RamAutoRefresh = true;
		public int RamColumnCount = 2;
		public float RamFontSize = 13;

		public int WindowWidth = -1;
		public int WindowHeight = -1;
		public int BottomPanelHeight = -1;

		public int BreakInCount = 1;
		public bool BreakInPpuCycles = false;

		public bool HighlightUnexecutedCode = true;
		
		public bool FindOccurrencesMatchCase = false;
		public bool FindOccurrencesMatchWholeWord = false;
		public string FindOccurrencesLastSearch = string.Empty;

		public bool AutoLoadDbgFiles = false;

		public DebugInfo()
		{
			LeftView = new DebugViewInfo();
			RightView = new DebugViewInfo();
		}
	}
}
