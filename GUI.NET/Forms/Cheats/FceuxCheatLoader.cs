using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms.Cheats
{
	class FceuxCheatLoader
	{
		public static List<CheatInfo> Load(string filename, string gameName, string gameCrc)
		{
			List<CheatInfo> cheats = new List<CheatInfo>();

			foreach(string cheatLine in File.ReadAllLines(filename)) {
				try {
					string[] data = cheatLine.Split(':');

					if(data.Length >= 3) {
						string address = null;
						string value = null;
						string compare = null;
						string description = null;

						if(data[0] == "C" || data[0] == "S" || data[0] == "CS" || data[0] == "SC" || string.IsNullOrWhiteSpace(data[0])) {
							address = data[1];
							value = data[2];
							compare = data[0].Contains("C") ? data[3] : null;
							description = data[0].Contains("C") ? data[4] : data[3];
						} else {
							address = data[0];
							value = data[1];
							description = data[2];
						}

						var cheat = new CheatInfo();
						cheat.GameCrc = gameCrc;
						cheat.GameName = gameName;
						cheat.CheatName = description;
						cheat.Enabled = false;

						cheat.CheatType = CheatType.Custom;
						cheat.IsRelativeAddress = true;
						cheat.Address = HexToInt(address);
						cheat.Value = (byte)HexToInt(value);
						if(compare != null) {
							cheat.CompareValue = (byte)HexToInt(compare);
							cheat.UseCompareValue = true;
						}
						cheats.Add(cheat);
					}
				} catch { }
			}
			return cheats;
		}

		private static UInt32 HexToInt(string hex)
		{
			if(string.IsNullOrWhiteSpace(hex)) {
				return 0;
			} else {
				return UInt32.Parse(hex.Trim().Replace("$", "").Replace("0x", ""), System.Globalization.NumberStyles.HexNumber);
			}
		}
	}
}
