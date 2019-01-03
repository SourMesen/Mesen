using System;
using System.Drawing;
using System.Linq;
using Be.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger
{
	public class ChrByteColorProvider : IByteColorProvider
	{
		DebugMemoryType _memoryType;
		bool _highlightDrawnBytes;
		bool _highlightReadBytes;
		bool _highlightBreakpoints;
		byte[] _cdlData;
		ByteColors _colors = new ByteColors();
		BreakpointType[] _breakpointTypes;

		public ChrByteColorProvider(DebugMemoryType memoryType, bool highlightDrawnBytes, bool highlightReadBytes, bool highlightBreakpoints)
		{
			_memoryType = memoryType;
			_highlightDrawnBytes = highlightDrawnBytes;
			_highlightReadBytes = highlightReadBytes;
			_highlightBreakpoints = highlightBreakpoints;

			_colors.ForeColor = Color.Black;
		}

		public void Prepare(long firstByteIndex, long lastByteIndex)
		{
			int visibleByteCount = (int)(lastByteIndex - firstByteIndex + 1);
			if(_highlightBreakpoints) {
				Breakpoint[] breakpoints = BreakpointManager.Breakpoints.ToArray();
				_breakpointTypes = new BreakpointType[visibleByteCount];

				for(int i = 0; i < visibleByteCount; i++) {
					int byteIndex = i + (int)firstByteIndex;
					foreach(Breakpoint bp in breakpoints) {
						if(bp.Enabled && !bp.IsCpuBreakpoint && bp.Matches(byteIndex, _memoryType)) {
							_breakpointTypes[i] = bp.BreakOnWrite ? BreakpointType.WriteVram : BreakpointType.ReadVram;
							break;
						}
					}
				}
			} else {
				_breakpointTypes = null;
			}

			if(_memoryType == DebugMemoryType.ChrRam && (_highlightDrawnBytes || _highlightReadBytes)) {
				_cdlData = InteropEmu.DebugGetCdlData((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType);
			} else {
				_cdlData = null;
			}
		}
		
		public ByteColors GetByteColor(long firstByteIndex, long byteIndex)
		{
			long index = byteIndex - firstByteIndex;

			_colors.BackColor = Color.Transparent;
			if(_cdlData != null) {
				if((_cdlData[index] & 0x01) != 0 && _highlightDrawnBytes) {
					//Drawn (CHR ROM)
					_colors.BackColor = ConfigManager.Config.DebugInfo.RamChrDrawnByteColor;
				} else if((_cdlData[index] & 0x02) != 0 && _highlightReadBytes) {
					//Read (CHR ROM)
					_colors.BackColor = ConfigManager.Config.DebugInfo.RamChrReadByteColor;
				}
			}

			_colors.BorderColor = Color.Empty;
			if(_breakpointTypes != null) {
				switch(_breakpointTypes[index]) {
					case BreakpointType.WriteVram: _colors.BorderColor =  ConfigManager.Config.DebugInfo.CodeWriteBreakpointColor; break;
					case BreakpointType.ReadVram: _colors.BorderColor =  ConfigManager.Config.DebugInfo.CodeReadBreakpointColor; break;
				}
			}

			return _colors;
		}
	}
}
