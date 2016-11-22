using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mesen.GUI.Debugger;

namespace Mesen.GUI.Config
{
	public class DebugViewInfo
	{
		public bool ShowByteCode = false;
		public bool ShowPrgAddresses = false;
		public float FontSize = 13;
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

		public bool ShowEffectiveAddresses = true;

		public bool ShowCpuMemoryMapping = true;
		public bool ShowPpuMemoryMapping = true;

		public bool RamAutoRefresh = true;
		public int RamColumnCount = 2;
		public float RamFontSize = 13;

		public List<Breakpoint> Breakpoints;
		public List<string> WatchValues;

		public DebugInfo()
		{
			LeftView = new DebugViewInfo();
			RightView = new DebugViewInfo();
		}
	}
}
