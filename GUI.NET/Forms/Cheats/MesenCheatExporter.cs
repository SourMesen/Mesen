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
			XmlWriter writer = XmlWriter.Create(filename, new XmlWriterSettings() { Indent = true });
			writer.WriteStartElement("cheats");
			writer.WriteAttributeString("MesenCheatFile", "true");
			writer.WriteAttributeString("version", "1.0");

			foreach(CheatInfo cheat in cheats) {
				writer.WriteStartElement("cheat");
				writer.WriteAttributeString("enabled", cheat.Enabled ? "1" : "0");
				writer.WriteAttributeString("game", "0x" + cheat.GameCrc);
				writer.WriteAttributeString("gameName", cheat.GameName);

				switch(cheat.CheatType) {
					case CheatType.GameGenie: writer.WriteElementString("genie", cheat.GameGenieCode); break;
					case CheatType.ProActionRocky: writer.WriteElementString("rocky", cheat.ProActionRockyCode.ToString("X8")); break;
					case CheatType.Custom:
						writer.WriteElementString("address", "0x" + cheat.Address.ToString("X4"));
						writer.WriteElementString("value", "0x" + cheat.Value.ToString("X2"));
						if(cheat.UseCompareValue) {
							writer.WriteElementString("compare", "0x" + cheat.CompareValue.ToString("X2"));
						}
						if(!cheat.IsRelativeAddress) {
							writer.WriteElementString("isPrgOffset", "true");
						}
						break;
				}
				writer.WriteElementString("description", cheat.CheatName);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();

			writer.Flush();
			writer.Close();
		}
	}
}
