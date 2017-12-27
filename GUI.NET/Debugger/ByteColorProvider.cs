using System;
using System.Drawing;
using Be.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger
{
	public class ByteColorProvider : IByteColorProvider
	{
		DebugMemoryType _memoryType;
		Int32[] _readStamps;
		Int32[] _writeStamps;
		Int32[] _execStamps;
		Int32[] _readCounts;
		Int32[] _writeCounts;
		Int32[] _execCounts;
		bool[] _freezeState;
		byte[] _cdlData;
		DebugState _state = new DebugState();
		bool _showExec;
		bool _showWrite;
		bool _showRead;
		int _framesToFade;
		bool _hideUnusedBytes;
		bool _hideReadBytes;
		bool _hideWrittenBytes;
		bool _hideExecutedBytes;
		bool _highlightDataBytes;
		bool _highlightCodeBytes;
		bool _highlightLabelledBytes;

		public ByteColorProvider(DebugMemoryType memoryType, bool showExec, bool showWrite, bool showRead, int framesToFade, bool hideUnusedBytes, bool hideReadBytes, bool hideWrittenBytes, bool hideExecutedBytes, bool highlightDataBytes, bool highlightCodeBytes, bool highlightLabelledBytes)
		{
			_memoryType = memoryType;
			_showExec = showExec;
			_showWrite = showWrite;
			_showRead = showRead;
			_framesToFade = framesToFade;
			_hideUnusedBytes = hideUnusedBytes;
			_hideReadBytes = hideReadBytes;
			_hideWrittenBytes = hideWrittenBytes;
			_hideExecutedBytes = hideExecutedBytes;
			_highlightDataBytes = highlightDataBytes;
			_highlightCodeBytes = highlightCodeBytes;
			_highlightLabelledBytes = highlightLabelledBytes;
		}

		public void Prepare(long firstByteIndex, long lastByteIndex)
		{
			_readStamps = InteropEmu.DebugGetMemoryAccessStamps((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType, MemoryOperationType.Read);
			_writeStamps = InteropEmu.DebugGetMemoryAccessStamps((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType, MemoryOperationType.Write);
			_execStamps = InteropEmu.DebugGetMemoryAccessStamps((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType, MemoryOperationType.Exec);
			if(_memoryType == DebugMemoryType.CpuMemory) {
				_freezeState = InteropEmu.DebugGetFreezeState((UInt16)firstByteIndex, (UInt16)(lastByteIndex - firstByteIndex + 1));
			}

			_readCounts = InteropEmu.DebugGetMemoryAccessCountsEx((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType, MemoryOperationType.Read);
			_writeCounts = InteropEmu.DebugGetMemoryAccessCountsEx((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType, MemoryOperationType.Write);
			_execCounts = InteropEmu.DebugGetMemoryAccessCountsEx((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType, MemoryOperationType.Exec);

			_cdlData = null;
			if(_highlightDataBytes || _highlightCodeBytes || _highlightLabelledBytes) {
				switch(_memoryType) {
					case DebugMemoryType.ChrRom:
					case DebugMemoryType.PpuMemory:
					case DebugMemoryType.CpuMemory:
					case DebugMemoryType.PrgRom:
						_cdlData = InteropEmu.DebugGetCdlData((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType);
						break;
				}
			}

			InteropEmu.DebugGetState(ref _state);
		}

		public Color DarkerColor(Color input, double brightnessPercentage)
		{
			if(double.IsInfinity(brightnessPercentage)) {
				brightnessPercentage = 1.0;
			}
			if(brightnessPercentage < 0.20) {
				brightnessPercentage *= 5;
			} else {
				brightnessPercentage = 1.0;
			}
			return Color.FromArgb((int)(input.R * brightnessPercentage), (int)(input.G * brightnessPercentage), (int)(input.B * brightnessPercentage));
		}

		public Color GetByteColor(long firstByteIndex, long byteIndex, out Color bgColor)
		{
			const int CyclesPerFrame = 29780;
			long index = byteIndex - firstByteIndex;
			double framesSinceExec = (double)(_state.CPU.CycleCount - _execStamps[index]) / CyclesPerFrame;
			double framesSinceWrite = (double)(_state.CPU.CycleCount - _writeStamps[index]) / CyclesPerFrame;
			double framesSinceRead = (double)(_state.CPU.CycleCount - _readStamps[index]) / CyclesPerFrame;

			bool isRead = _readCounts[index] > 0;
			bool isWritten = _writeCounts[index] > 0;
			bool isExecuted = _execCounts[index] > 0;
			bool isUnused = !isRead && !isWritten && !isExecuted;

			int alpha = 0;
			if(isRead && _hideReadBytes || isWritten && _hideWrittenBytes || isExecuted && _hideExecutedBytes || isUnused && _hideUnusedBytes) {
				alpha = 128;
			}
			if(isRead && !_hideReadBytes || isWritten && !_hideWrittenBytes || isExecuted && !_hideExecutedBytes || isUnused && !_hideUnusedBytes) {
				alpha = 255;
			}

			bgColor = Color.Transparent;
			if(_cdlData != null) {
				if((_cdlData[index] & 0x04) != 0 && _highlightLabelledBytes) {
					//Labels/comments
					bgColor = ConfigManager.Config.DebugInfo.RamLabelledByteColor;
				} else if((_cdlData[index] & 0x01) != 0 && _highlightCodeBytes) {
					//Code
					bgColor = ConfigManager.Config.DebugInfo.RamCodeByteColor;
				} else if((_cdlData[index] & 0x02) != 0 && _highlightDataBytes) {
					//Data
					bgColor = ConfigManager.Config.DebugInfo.RamDataByteColor;
				}
			}

			if(_freezeState != null && _freezeState[index]) {
				return Color.Magenta;
			} else if(_showExec && _execStamps[index] != 0 && framesSinceExec >= 0 && (framesSinceExec < _framesToFade || _framesToFade == 0)) {
				return Color.FromArgb(alpha, DarkerColor(ConfigManager.Config.DebugInfo.RamExecColor, (_framesToFade - framesSinceExec) / _framesToFade));
			} else if(_showWrite && _writeStamps[index] != 0 && framesSinceWrite >= 0 && (framesSinceWrite < _framesToFade || _framesToFade == 0)) {
				return Color.FromArgb(alpha, DarkerColor(ConfigManager.Config.DebugInfo.RamWriteColor, (_framesToFade - framesSinceWrite) / _framesToFade));
			} else if(_showRead && _readStamps[index] != 0 && framesSinceRead >= 0 && (framesSinceRead < _framesToFade || _framesToFade == 0)) {
				return Color.FromArgb(alpha, DarkerColor(ConfigManager.Config.DebugInfo.RamReadColor, (_framesToFade - framesSinceRead) / _framesToFade));
			}

			return Color.FromArgb(alpha, Color.Black);
		}
	}
}
