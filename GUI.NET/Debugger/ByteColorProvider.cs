﻿using System;
using System.Drawing;
using System.Linq;
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
		bool[] _hasLabel;
		DebugState _state = new DebugState();
		bool _showExec;
		bool _showWrite;
		bool _showRead;
		int _framesToFade;
		bool _hideUnusedBytes;
		bool _hideReadBytes;
		bool _hideWrittenBytes;
		bool _hideExecutedBytes;
		bool _highlightDmcDataBytes;
		bool _highlightDataBytes;
		bool _highlightCodeBytes;
		bool _highlightLabelledBytes;
		bool _highlightBreakpoints;
		ByteColors _colors = new ByteColors();
		BreakpointType[] _breakpointTypes;

		public ByteColorProvider(DebugMemoryType memoryType, bool showExec, bool showWrite, bool showRead, int framesToFade, bool hideUnusedBytes, bool hideReadBytes, bool hideWrittenBytes, bool hideExecutedBytes, bool highlightDmcDataBytes, bool highlightDataBytes, bool highlightCodeBytes, bool highlightLabelledBytes, bool highlightBreakpoints)
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
			_highlightDmcDataBytes = highlightDmcDataBytes;
			_highlightDataBytes = highlightDataBytes;
			_highlightCodeBytes = highlightCodeBytes;
			_highlightLabelledBytes = highlightLabelledBytes;
			_highlightBreakpoints = highlightBreakpoints;
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
						if(bp.Enabled && bp.IsCpuBreakpoint && bp.Matches(byteIndex, _memoryType)) {
							_breakpointTypes[i] = bp.BreakOnExec ? BreakpointType.Execute : (bp.BreakOnWrite ? BreakpointType.WriteRam : BreakpointType.ReadRam);
							break;
						}
					}
				}
			} else {
				_breakpointTypes = null;
			}

			_readStamps = InteropEmu.DebugGetMemoryAccessStamps((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType, MemoryOperationType.Read);
			_writeStamps = InteropEmu.DebugGetMemoryAccessStamps((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType, MemoryOperationType.Write);
			_execStamps = InteropEmu.DebugGetMemoryAccessStamps((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType, MemoryOperationType.Exec);
			if(_memoryType == DebugMemoryType.CpuMemory) {
				_freezeState = InteropEmu.DebugGetFreezeState((UInt16)firstByteIndex, (UInt16)visibleByteCount);
			}

			_readCounts = InteropEmu.DebugGetMemoryAccessCounts((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType, MemoryOperationType.Read);
			_writeCounts = InteropEmu.DebugGetMemoryAccessCounts((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType, MemoryOperationType.Write);
			_execCounts = InteropEmu.DebugGetMemoryAccessCounts((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType, MemoryOperationType.Exec);

			_cdlData = null;
			if(_highlightDmcDataBytes || _highlightDataBytes || _highlightCodeBytes) {
				switch(_memoryType) {
					case DebugMemoryType.ChrRom:
					case DebugMemoryType.PpuMemory:
					case DebugMemoryType.CpuMemory:
					case DebugMemoryType.PrgRom:
						_cdlData = InteropEmu.DebugGetCdlData((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType);
						break;
				}
			}

			_hasLabel = new bool[visibleByteCount];
			if(_highlightLabelledBytes) {
				if(_memoryType == DebugMemoryType.CpuMemory) {
					for(long i = 0; i < _hasLabel.Length; i++) {
						_hasLabel[i] = (
							!string.IsNullOrWhiteSpace(LabelManager.GetLabel((UInt16)(i + firstByteIndex))?.Label) ||
							!string.IsNullOrWhiteSpace(LabelManager.GetLabel((uint)(i + firstByteIndex), AddressType.Register)?.Label)
						);
					}
				} else if(_memoryType == DebugMemoryType.PrgRom || _memoryType == DebugMemoryType.WorkRam || _memoryType == DebugMemoryType.SaveRam) {
					for(long i = 0; i < _hasLabel.Length; i++) {
						_hasLabel[i] = !string.IsNullOrWhiteSpace(LabelManager.GetLabel((uint)(firstByteIndex  + i), _memoryType.ToAddressType())?.Label);
					}
				}
			}

			InteropEmu.DebugGetState(ref _state);
		}

		public static Color DarkerColor(Color input, double brightnessPercentage)
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

		public ByteColors GetByteColor(long firstByteIndex, long byteIndex)
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

			_colors.BackColor = Color.Transparent;
			if(_cdlData != null) {
				if((_cdlData[index] & (byte)CdlPrgFlags.Code) != 0 && _highlightCodeBytes) {
					//Code
					_colors.BackColor = ConfigManager.Config.DebugInfo.RamCodeByteColor;
				} else if((_cdlData[index] & (byte)CdlPrgFlags.PcmData) != 0 && _highlightDmcDataBytes) {
					//DMC channel Data
					_colors.BackColor = ConfigManager.Config.DebugInfo.RamDmcDataByteColor;
				} else if((_cdlData[index] & (byte)CdlPrgFlags.Data) != 0 && _highlightDataBytes) {
					//Data
					_colors.BackColor = ConfigManager.Config.DebugInfo.RamDataByteColor;
				}
			}

			if(_hasLabel[index]) {
				//Labels/comments
				_colors.BackColor = ConfigManager.Config.DebugInfo.RamLabelledByteColor;
			}

			_colors.BorderColor = Color.Empty;
			if(_breakpointTypes != null) {
				switch(_breakpointTypes[index]) {
					case BreakpointType.Execute: _colors.BorderColor = ConfigManager.Config.DebugInfo.CodeExecBreakpointColor; break;
					case BreakpointType.WriteRam: _colors.BorderColor =  ConfigManager.Config.DebugInfo.CodeWriteBreakpointColor; break;
					case BreakpointType.ReadRam: _colors.BorderColor =  ConfigManager.Config.DebugInfo.CodeReadBreakpointColor; break;
				}
			}

			if(_freezeState != null && _freezeState[index]) {
				_colors.ForeColor = Color.Magenta;
			} else if(_showExec && _execStamps[index] != 0 && framesSinceExec >= 0 && (framesSinceExec < _framesToFade || _framesToFade == 0)) {
				_colors.ForeColor = Color.FromArgb(alpha, DarkerColor(ConfigManager.Config.DebugInfo.RamExecColor, (_framesToFade - framesSinceExec) / _framesToFade));
			} else if(_showWrite && _writeStamps[index] != 0 && framesSinceWrite >= 0 && (framesSinceWrite < _framesToFade || _framesToFade == 0)) {
				_colors.ForeColor = Color.FromArgb(alpha, DarkerColor(ConfigManager.Config.DebugInfo.RamWriteColor, (_framesToFade - framesSinceWrite) / _framesToFade));
			} else if(_showRead && _readStamps[index] != 0 && framesSinceRead >= 0 && (framesSinceRead < _framesToFade || _framesToFade == 0)) {
				_colors.ForeColor = Color.FromArgb(alpha, DarkerColor(ConfigManager.Config.DebugInfo.RamReadColor, (_framesToFade - framesSinceRead) / _framesToFade));
			} else {
				_colors.ForeColor = Color.FromArgb(alpha, Color.Black);
			}

			return _colors;
		}
	}
}
