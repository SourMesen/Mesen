using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms.Cheats
{
	class MesenCheatExporter
	{
		public static void Export(string filename, IEnumerable<CheatInfo> cheats)
		{
			//Exports to an XML format compatible with Nestopia's, but with an extra flag (isPrgOffset)
			string xml = "<cheats MesenCheatFile=\"true\" version=\"1.0\">";

			foreach(CheatInfo cheat in cheats) {
				string enabled = cheat.Enabled ? "1" : "0";
				string gameCrc = "0x" + cheat.GameCrc;
				string address = "0x" + cheat.Address.ToString("X4");
				string value = "0x" + cheat.Value.ToString("X2");
				string compare = "0x" + cheat.CompareValue.ToString("X2");
				string genie = cheat.GameGenieCode;
				string rocky = cheat.ProActionRockyCode.ToString("X8");

				string genieTag = cheat.CheatType == CheatType.GameGenie ? $"<genie>{genie}</genie>" : "";
				string rockyTag = cheat.CheatType == CheatType.ProActionRocky ? $"<rocky>{rocky}</rocky>" : "";
				string customTags = cheat.CheatType == CheatType.Custom ? $"<address>{address}</address><value>{value}</value><compare>{compare}</compare>" : "";

				string prgAddress = !cheat.IsRelativeAddress ? "<isPrgOffset>true</isPrgOffset>" : "";

				xml += $"<cheat enabled=\"{enabled}\" game=\"{gameCrc}\" gameName=\"{cheat.GameName}\">{genieTag}{rockyTag}{customTags}{prgAddress}<description>{cheat.CheatName}</description></cheat>";
			}
			xml += "</cheats>";

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);
			xmlDoc.Save(filename);
		}
	}
}
