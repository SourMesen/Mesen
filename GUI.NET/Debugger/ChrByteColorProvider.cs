using System;
using System.Drawing;
using Be.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger
{
	public class ChrByteColorProvider : IByteColorProvider
	{
		DebugMemoryType _memoryType;
		bool _highlightDrawnBytes;
		bool _highlightReadBytes;
		byte[] _cdlData;

		public ChrByteColorProvider(DebugMemoryType memoryType, bool highlightDrawnBytes, bool highlightReadBytes)
		{
			_memoryType = memoryType;
			_highlightDrawnBytes = highlightDrawnBytes;
			_highlightReadBytes = highlightReadBytes;
		}

		public void Prepare(long firstByteIndex, long lastByteIndex)
		{
			if(_highlightDrawnBytes || _highlightReadBytes) {
				_cdlData = InteropEmu.DebugGetCdlData((UInt32)firstByteIndex, (UInt32)(lastByteIndex - firstByteIndex + 1), _memoryType);
			} else {
				_cdlData = null;
			}
		}
		
		public Color GetByteColor(long firstByteIndex, long byteIndex, out Color bgColor)
		{
			long index = byteIndex - firstByteIndex;

			bgColor = Color.Transparent;
			if(_cdlData != null) {
				if((_cdlData[index] & 0x01) != 0 && _highlightDrawnBytes) {
					//Drawn (CHR ROM)
					bgColor = ConfigManager.Config.DebugInfo.RamChrDrawnByteColor;
				} else if((_cdlData[index] & 0x02) != 0 && _highlightReadBytes) {
					//Read (CHR ROM)
					bgColor = ConfigManager.Config.DebugInfo.RamChrReadByteColor;
				}
			}

			return Color.Black;
		}
	}
}
