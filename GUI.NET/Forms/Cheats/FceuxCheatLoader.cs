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
		private static string GetValue(List<string> data, int index)
		{
			if(data.Count > index) {
				return data[index];
			} else {
				return "";
			}
		}

		public static List<CheatInfo> Load(string filename, string gameName, string gameCrc)
		{
			List<CheatInfo> cheats = new List<CheatInfo>();

			foreach(string cheatLine in File.ReadAllLines(filename)) {
				try {
					if(!string.IsNullOrWhiteSpace(cheatLine)) {
						List<string> data = new List<string>(cheatLine.Split(':'));

						if(data.Count >= 3) {
							bool enabled = false;
							if(data[0].Length <= 2) {
								enabled = true;
								data[1] = data[0] + data[1];
								data.RemoveAt(0);
							}

							string address = data[0].Substring(data[0].Length - 4);
							string value = data[1];
							string compare = null;
							string description = null;
							if(data[0].StartsWith("SC") && data[0].Length >= 6 || (data[0].StartsWith("C") && data[0].Length >= 5)) {
								compare = data[2];
								description = GetValue(data, 3);
							} else {
								description = GetValue(data, 2);
							}

							var cheat = new CheatInfo() {
								GameCrc = gameCrc,
								GameName = gameName,
								CheatName = description,
								Enabled = enabled,
								CheatType = CheatType.Custom,
								IsRelativeAddress = true,
								Address = HexToInt(address),
								Value = (byte)HexToInt(value)
							};

							if(compare != null) {
								cheat.CompareValue = (byte)HexToInt(compare);
								cheat.UseCompareValue = true;
							}

							cheats.Add(cheat);
						}
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
