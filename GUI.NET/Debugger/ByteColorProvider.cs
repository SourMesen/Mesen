using System;
using System.Drawing;
using System.Linq;
using Be.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger
{
	public class ByteColorProvider : IByteColorProvider
	{
		DebugMemoryType _memoryType;
		AddressCounters[] _counts;
		bool[] _freezeState;
		byte[] _cdlData;
		ByteLabelState[] _hasLabel;
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

			_counts = InteropEmu.DebugGetMemoryAccessCounts((UInt32)firstByteIndex, (UInt32)visibleByteCount, _memoryType);
			if(_memoryType == DebugMemoryType.CpuMemory) {
				_freezeState = InteropEmu.DebugGetFreezeState((UInt16)firstByteIndex, (UInt16)visibleByteCount);
			}

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

			_hasLabel = new ByteLabelState[visibleByteCount];
			if(_highlightLabelledBytes) {
				if(_memoryType == DebugMemoryType.CpuMemory) {
					for(long i = 0; i < _hasLabel.Length; i++) {
						UInt16 addr = (UInt16)(i + firstByteIndex);
						CodeLabel label = LabelManager.GetLabel(addr);
						if(label == null) {
							label = LabelManager.GetLabel(addr, AddressType.Register);
						}

						if(label != null && !string.IsNullOrWhiteSpace(label.Label)) {
							if(label.Length > 1) {
								int relAddress = label.GetRelativeAddress();
								_hasLabel[i] = relAddress == addr ? ByteLabelState.LabelFirstByte : ByteLabelState.LabelExtraByte;
							} else {
								_hasLabel[i] = ByteLabelState.LabelFirstByte;
							}
						}
					}
				} else if(_memoryType == DebugMemoryType.PrgRom || _memoryType == DebugMemoryType.WorkRam || _memoryType == DebugMemoryType.SaveRam) {
					for(long i = 0; i < _hasLabel.Length; i++) {
						UInt32 addr = (UInt32)(i + firstByteIndex);
						CodeLabel label = LabelManager.GetLabel(addr, _memoryType.ToAddressType());
						if(label != null && !string.IsNullOrWhiteSpace(label.Label)) {
							_hasLabel[i] = label.Length == 1 || label.Address == addr ? ByteLabelState.LabelFirstByte : ByteLabelState.LabelExtraByte;
						}
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
			double framesSinceExec = (double)(_state.CPU.CycleCount - _counts[index].ExecStamp) / CyclesPerFrame;
			double framesSinceWrite = (double)(_state.CPU.CycleCount - _counts[index].WriteStamp) / CyclesPerFrame;
			double framesSinceRead = (double)(_state.CPU.CycleCount - _counts[index].ReadStamp) / CyclesPerFrame;

			bool isRead = _counts[index].ReadCount > 0;
			bool isWritten = _counts[index].WriteCount > 0;
			bool isExecuted = _counts[index].ExecCount > 0;
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

			//Labels/comments
			switch(_hasLabel[index]) {
				case ByteLabelState.LabelFirstByte: _colors.BackColor = ConfigManager.Config.DebugInfo.RamLabelledByteColor; break;
				case ByteLabelState.LabelExtraByte: _colors.BackColor = Color.FromArgb(180, ConfigManager.Config.DebugInfo.RamLabelledByteColor); break;
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
			} else if(_showExec && _counts[index].ExecStamp != 0 && framesSinceExec >= 0 && (framesSinceExec < _framesToFade || _framesToFade == 0)) {
				_colors.ForeColor = Color.FromArgb(alpha, DarkerColor(ConfigManager.Config.DebugInfo.RamExecColor, (_framesToFade - framesSinceExec) / _framesToFade));
			} else if(_showWrite && _counts[index].WriteStamp != 0 && framesSinceWrite >= 0 && (framesSinceWrite < _framesToFade || _framesToFade == 0)) {
				_colors.ForeColor = Color.FromArgb(alpha, DarkerColor(ConfigManager.Config.DebugInfo.RamWriteColor, (_framesToFade - framesSinceWrite) / _framesToFade));
			} else if(_showRead && _counts[index].ReadStamp != 0 && framesSinceRead >= 0 && (framesSinceRead < _framesToFade || _framesToFade == 0)) {
				_colors.ForeColor = Color.FromArgb(alpha, DarkerColor(ConfigManager.Config.DebugInfo.RamReadColor, (_framesToFade - framesSinceRead) / _framesToFade));
			} else {
				_colors.ForeColor = Color.FromArgb(alpha, Color.Black);
			}

			return _colors;
		}
	}

	enum ByteLabelState
	{
		NoLabel,
		LabelFirstByte,
		LabelExtraByte,
	}
}
