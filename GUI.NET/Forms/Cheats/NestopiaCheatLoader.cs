using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms.Cheats
{
	class NestopiaCheatLoader
	{
		public static bool IsMesenCheatFile(string filename)
		{
			try {
				XmlDocument xml = new XmlDocument();
				xml.Load(filename);
				XmlNode node = xml.SelectSingleNode("/cheats");
				return node != null && node.Attributes["MesenCheatFile"] != null && node.Attributes["MesenCheatFile"].Value == "true";
			} catch {
				return false;
			}
		}

		public static List<CheatInfo> Load(string filepath, string gameName, string gameCrc)
		{
			try {
				List<CheatInfo> cheats = new List<CheatInfo>();

				XmlDocument xml = new XmlDocument();
				xml.Load(filepath);
				bool validFile = false;
				bool hasMatchingCheats = false;
				foreach(XmlNode node in xml.SelectNodes("/cheats/cheat")) {
					try {
						if(node.Attributes["game"] != null) {
							validFile = true;
							var nodeGameName = node.Attributes["gameName"]?.Value;
							if(nodeGameName != null && string.IsNullOrWhiteSpace(gameCrc) || node.Attributes["game"].Value.Contains(gameCrc)) {
								hasMatchingCheats = true;
								var crc = node.Attributes["game"].Value.Replace("0x", "").ToUpper();
								var genie = node.SelectSingleNode("genie");
								var rocky = node.SelectSingleNode("rocky");
								var description = node.SelectSingleNode("description");
								var address = node.SelectSingleNode("address");
								var value = node.SelectSingleNode("value");
								var compare = node.SelectSingleNode("compare");
								bool isPrgOffset = node.SelectSingleNode("isPrgOffset")?.Value == "true";

								var cheat = new CheatInfo();
								cheat.GameCrc = crc;
								cheat.GameName = nodeGameName ?? gameName;
								cheat.CheatName = description?.InnerXml;
								cheat.Enabled = node.Attributes["enabled"] != null && node.Attributes["enabled"].Value == "1" ? true : false;
								if(genie != null) {
									cheat.CheatType = CheatType.GameGenie;
									cheat.GameGenieCode = genie.InnerText.ToUpper();
								} else if(rocky != null) {
									cheat.CheatType = CheatType.ProActionRocky;
									cheat.ProActionRockyCode = HexToInt(rocky.InnerText);
								} else {
									cheat.CheatType = CheatType.Custom;
									cheat.IsRelativeAddress = !isPrgOffset;
									cheat.Address = HexToInt(address?.InnerText);
									cheat.Value = (byte)HexToInt(value?.InnerText);
									if(compare != null) {
										cheat.CompareValue = (byte)HexToInt(compare.InnerText);
										cheat.UseCompareValue = true;
									}
								}

								cheats.Add(cheat);
							}
						}
					} catch { }
				}

				if(!validFile) {
					//Valid xml file, but invalid content
					MesenMsgBox.Show("InvalidCheatFile", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, Path.GetFileName(filepath));
					return null;
				} else if(!hasMatchingCheats) {
					//Valid cheat file, but no cheats match selected game
					MesenMsgBox.Show("NoMatchingCheats", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, Path.GetFileName(filepath));
					return null;
				} else {
					return cheats;
				}
			} catch {
				//Invalid xml file
				MesenMsgBox.Show("InvalidXmlFile", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, Path.GetFileName(filepath));
				return null;
			}
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
