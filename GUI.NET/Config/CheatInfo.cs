using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public enum CheatType
	{
		GameGenie = 0,
		ProActionRocky = 1,
		Custom = 2
	}

	public class CheatInfo
	{
		public bool Enabled = true;
		public string CheatName;
		public string GameName;
		public string GameCrc;
		public CheatType CheatType;
		public string GameGenieCode;
		public UInt32 ProActionRockyCode;
		public UInt32 Address;
		public Byte Value;
		public Byte CompareValue;
		public bool UseCompareValue;
		public bool IsRelativeAddress = true;

		public override string ToString()
		{
			switch(CheatType) {
				case CheatType.Custom:
					return (!IsRelativeAddress ? "#" : "") + string.Format("{0:X4}:{1:X2}" + (UseCompareValue ? ":{2:X2}" : ""), Address, Value, CompareValue);
				case Config.CheatType.GameGenie:
					return "GG: " + GameGenieCode.ToUpperInvariant();
				case Config.CheatType.ProActionRocky:
					return "PRA: " + ProActionRockyCode.ToString("X");
			}
			return string.Empty;
		}

		public void ApplyCheat()
		{
			switch(CheatType) {
				case CheatType.Custom:
					InteropEmu.CheatAddCustom(Address, Value, UseCompareValue ? CompareValue : -1, IsRelativeAddress);
					break;

				case Config.CheatType.GameGenie:
					InteropEmu.CheatAddGameGenie(GameGenieCode);
					break;

				case Config.CheatType.ProActionRocky:
					InteropEmu.CheatAddProActionRocky(ProActionRockyCode);
					break;
			}
		}

		public static void ClearCheats()
		{
			InteropEmu.CheatClear();
		}

		public static void ApplyCheats()
		{
			InteropEmu.CheatClear();

			if(!ConfigManager.Config.DisableAllCheats) {
				string crc = InteropEmu.GetRomInfo().GetPrgCrcString();
				int cheatCount = 0;
				foreach(CheatInfo cheat in ConfigManager.Config.Cheats.Where(c => c.GameCrc == crc)) {
					if(cheat.Enabled) {
						cheat.ApplyCheat();
						cheatCount++;
					}
				}

				if(cheatCount > 0) {
					InteropEmu.DisplayMessage("Cheats", cheatCount > 1 ? "CheatsApplied" : "CheatApplied", cheatCount.ToString());
				}
			}
		}
	}
}
