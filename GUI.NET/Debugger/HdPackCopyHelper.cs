using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	public class HdPackCopyHelper
	{
		private byte[] _ppuMemory = new byte[0];
		private int[] _absoluteTileIndexes = new int[512];
		private byte[] _paletteRam = new byte[0];

		public void RefreshData()
		{
			//Data needed for HD Pack copy
			bool isChrRam = InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRom) == 0;
			if(isChrRam) {
				_ppuMemory = InteropEmu.DebugGetMemoryState(DebugMemoryType.PpuMemory);
			} else {
				for(int i = 0; i < 512; i++) {
					_absoluteTileIndexes[i] = InteropEmu.DebugGetAbsoluteChrAddress((uint)i * 16) / 16;
				}
			}
			_paletteRam = InteropEmu.DebugGetMemoryState(DebugMemoryType.PaletteMemory);
		}

		public string ToHdPackFormat(int tileAddr, int palette, bool forSprite, bool isAbsoluteAddress)
		{
			bool isChrRam = InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRom) == 0;
			StringBuilder sb = new StringBuilder();

			if(isAbsoluteAddress) {
				if(isChrRam) {
					for(int i = 0; i < 16; i++) {
						sb.Append(InteropEmu.DebugGetMemoryValue(DebugMemoryType.ChrRam, (uint)(tileAddr + i)).ToString("X2"));
					}
				} else {
					sb.Append((tileAddr / 16).ToString());
				}
			} else {
				if(isChrRam) {
					for(int i = 0; i < 16; i++) {
						sb.Append(_ppuMemory[tileAddr + i].ToString("X2"));
					}
				} else {
					sb.Append(_absoluteTileIndexes[tileAddr / 16].ToString());
				}
			}

			if(forSprite) {
				sb.Append(",FF");
				for(int i = 1; i < 4; i++) {
					sb.Append(_paletteRam[palette * 4 + i].ToString("X2"));
				}
			} else {
				sb.Append(",");
				for(int i = 0; i < 4; i++) {
					sb.Append(_paletteRam[palette * 4 + i].ToString("X2"));
				}
			}

			return sb.ToString();
		}
	}
}
