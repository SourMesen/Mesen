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
		AddressCounters[] _counts;
		DebugState _state = new DebugState();
		bool _showWrite;
		bool _showRead;
		int _framesToFade;
		bool _hideUnusedBytes;
		bool _hideReadBytes;
		bool _hideWrittenBytes;
		bool _highlightDrawnBytes;
		bool _highlightReadBytes;
		bool _highlightBreakpoints;
		byte[] _cdlData;
		ByteColors _colors = new ByteColors();
		BreakpointType[] _breakpointTypes;

		public ChrByteColorProvider(DebugMemoryType memoryType, bool showWrite, bool showRead, int framesToFade, bool hideUnusedBytes, bool hideReadBytes, bool hideWrittenBytes, bool highlightDrawnBytes, bool highlightReadBytes, bool highlightBreakpoints)
		{
			_memoryType = memoryType;
			_showWrite = showWrite;
			_showRead = showRead;
			_framesToFade = framesToFade;
			_hideUnusedBytes = hideUnusedBytes;
			_hideReadBytes = hideReadBytes;
			_hideWrittenBytes = hideWrittenBytes;
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

			InteropEmu.DebugGetState(ref _state);

			_counts = InteropEmu.DebugGetMemoryAccessCounts((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType);

			if(_memoryType == DebugMemoryType.ChrRom && (_highlightDrawnBytes || _highlightReadBytes)) {
				_cdlData = InteropEmu.DebugGetCdlData((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType);
			} else {
				_cdlData = null;
			}
		}
		
		public ByteColors GetByteColor(long firstByteIndex, long byteIndex)
		{
			const int CyclesPerFrame = 29780;
			long index = byteIndex - firstByteIndex;
			double framesSinceWrite = (double)(_state.CPU.CycleCount - _counts[index].WriteStamp) / CyclesPerFrame;
			double framesSinceRead = (double)(_state.CPU.CycleCount - _counts[index].ReadStamp) / CyclesPerFrame;

			bool isRead = _counts[index].ReadCount > 0;
			bool isWritten = _counts[index].WriteCount > 0;
			bool isUnused = !isRead && !isWritten;

			int alpha = 0;
			if(isRead && _hideReadBytes || isWritten && _hideWrittenBytes || isUnused && _hideUnusedBytes) {
				alpha = 128;
			}
			if(isRead && !_hideReadBytes || isWritten && !_hideWrittenBytes || isUnused && !_hideUnusedBytes) {
				alpha = 255;
			}

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

			if(_showWrite && _counts[index].WriteStamp != 0 && framesSinceWrite >= 0 && (framesSinceWrite < _framesToFade || _framesToFade == 0)) {
				_colors.ForeColor = Color.FromArgb(alpha, ByteColorProvider.DarkerColor(ConfigManager.Config.DebugInfo.RamWriteColor, (_framesToFade - framesSinceWrite) / _framesToFade));
			} else if(_showRead && _counts[index].ReadStamp != 0 && framesSinceRead >= 0 && (framesSinceRead < _framesToFade || _framesToFade == 0)) {
				_colors.ForeColor = Color.FromArgb(alpha, ByteColorProvider.DarkerColor(ConfigManager.Config.DebugInfo.RamReadColor, (_framesToFade - framesSinceRead) / _framesToFade));
			} else {
				_colors.ForeColor = Color.FromArgb(alpha, Color.Black);
			}

			return _colors;
		}
	}
}
