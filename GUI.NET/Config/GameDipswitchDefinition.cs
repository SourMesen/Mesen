using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Mesen.GUI.Config
{
	public class GameDipswitchDefinition
	{
		public byte DefaultDipSwitches;
		public List<List<string>> DipSwitches;

		private static Dictionary<int, GameDipswitchDefinition> _gameConfigs = new Dictionary<int, GameDipswitchDefinition>();

		public static GameDipswitchDefinition GetDipswitchDefinition()
		{
			int prgCrc32 = (int)InteropEmu.GetRomInfo().PrgCrc32;
			if(_gameConfigs.ContainsKey(prgCrc32)) {
				return _gameConfigs[prgCrc32];
			} else {
				return null;
			}
		}

		static GameDipswitchDefinition()
		{
			XmlDocument config = new XmlDocument();
			config.Load(ResourceManager.GetZippedResource("DipswitchDefinitions.xml"));

			foreach(XmlNode gameNode in config.SelectNodes("/DipswitchDefinitions/Game")) {
				var gameDipswitches = new GameDipswitchDefinition();
				string crcValues = gameNode.Attributes["PrgCrc32"].Value;
				foreach(string crc in crcValues.Split(',')) {
					if(gameNode.Attributes["DefaultDip"] != null) {
						gameDipswitches.DefaultDipSwitches = (byte)Int32.Parse(gameNode.Attributes["DefaultDip"].Value);
					}
					gameDipswitches.DipSwitches = new List<List<string>>();

					foreach(XmlNode dipSwitch in gameNode.SelectNodes("DipSwitch")) {
						if(dipSwitch.Attributes["Localization"] != null) {
							var list = new List<string>();
							gameDipswitches.DipSwitches.Add(list);

							list.Add(dipSwitch.Attributes["Localization"].Value);
							foreach(XmlNode option in dipSwitch.SelectNodes("Option")) {
								list.Add(option.InnerText);
							}
						} else {
							var list = new List<string>();
							gameDipswitches.DipSwitches.Add(list);

							list.Add("Unknown");
							list.Add("Off");
							list.Add("On");
						}
					}
					_gameConfigs[int.Parse(crc, System.Globalization.NumberStyles.HexNumber)] = gameDipswitches;
				}
			}
		}
	}
}
