using System;
using System.Drawing;
using Be.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public class ByteColorProvider : IByteColorProvider
	{
		DebugMemoryType _memoryType;
		Int32[] _readStamps;
		Int32[] _writeStamps;
		Int32[] _execStamps;
		DebugState _state = new DebugState();
		bool _showExec;
		bool _showWrite;
		bool _showRead;
		int _framesToFade;

		public ByteColorProvider(DebugMemoryType memoryType, bool showExec, bool showWrite, bool showRead, int framesToFade)
		{
			_memoryType = memoryType;
			_showExec = showExec;
			_showWrite = showWrite;
			_showRead = showRead;
			_framesToFade = framesToFade;
		}

		public void Prepare(long firstByteIndex, long lastByteIndex)
		{
			_readStamps = InteropEmu.DebugGetMemoryAccessStamps((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType, MemoryOperationType.Read);
			_writeStamps = InteropEmu.DebugGetMemoryAccessStamps((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType, MemoryOperationType.Write);
			_execStamps = InteropEmu.DebugGetMemoryAccessStamps((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType, MemoryOperationType.Exec);
			InteropEmu.DebugGetState(ref _state);
		}

		public Color DarkerColor(Color input, double brightnessPercentage)
		{
			if(brightnessPercentage < 0.20) {
				brightnessPercentage *= 5;
			} else {
				brightnessPercentage = 1.0;
			}
			return Color.FromArgb((int)(input.R * brightnessPercentage), (int)(input.G * brightnessPercentage), (int)(input.B * brightnessPercentage));
		}

		public Color GetByteColor(long firstByteIndex, long byteIndex)
		{
			const int CyclesPerFrame = 29780;
			long index = byteIndex - firstByteIndex;
			double framesSinceExec = (double)(_state.CPU.CycleCount - _execStamps[index]) / CyclesPerFrame;
			double framesSinceWrite = (double)(_state.CPU.CycleCount - _writeStamps[index]) / CyclesPerFrame;
			double framesSinceRead = (double)(_state.CPU.CycleCount - _readStamps[index]) / CyclesPerFrame;

			if(_showExec && _execStamps[index] != 0 && framesSinceExec < _framesToFade) {
				return DarkerColor(Color.Green, (_framesToFade - framesSinceExec) / _framesToFade);
			} else if(_showWrite && _writeStamps[index] != 0 && framesSinceWrite < _framesToFade) {
				return DarkerColor(Color.Red, (_framesToFade - framesSinceWrite) / _framesToFade);
			} else if(_showRead && _readStamps[index] != 0 && framesSinceRead < _framesToFade) {
				return DarkerColor(Color.Blue, (_framesToFade - framesSinceRead) / _framesToFade);
			}

			return Color.Black;
		}
	}
}
