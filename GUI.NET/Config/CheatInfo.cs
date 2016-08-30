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

		public static void ClearCheats()
		{
			InteropEmu.SetCheats(new InteropCheatInfo[] { }, 0);
		}

		private InteropCheatInfo ToInterop()
		{
			byte[] ggCode = Encoding.UTF8.GetBytes(GameGenieCode ?? "");
			Array.Resize(ref ggCode, 9);

			return new InteropCheatInfo() {
				CheatType = CheatType,
				GameGenieCode = ggCode,
				ProActionRockyCode = ProActionRockyCode,
				Address = Address,
				Value = Value,
				CompareValue = CompareValue,
				UseCompareValue = UseCompareValue,
				IsRelativeAddress = IsRelativeAddress
			};
		}

		public static void ApplyCheats()
		{
			if(!ConfigManager.Config.DisableAllCheats) {
				string crc = InteropEmu.GetRomInfo().GetPrgCrcString();

				InteropCheatInfo[] cheats = ConfigManager.Config.Cheats.Where(c => c.GameCrc == crc && c.Enabled).Select(cheat => cheat.ToInterop()).ToArray();
				InteropEmu.SetCheats(cheats, (UInt32)cheats.Length);
								
				if(cheats.Length > 0) {
					InteropEmu.DisplayMessage("Cheats", cheats.Length > 1 ? "CheatsApplied" : "CheatApplied", cheats.Length.ToString());
				}
			} else {
				ClearCheats();
			}
		}
	}
}
