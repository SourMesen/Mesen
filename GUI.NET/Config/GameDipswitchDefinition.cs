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
		public string GameName;
		public string GameID;
		public byte DefaultDipSwitches;

		public List<List<string>> DipSwitches;

		private static Dictionary<string, GameDipswitchDefinition> _gameConfigs = new Dictionary<string, GameDipswitchDefinition>();

		public static string GetGameIdByCrc(UInt32 prgCrc32)
		{
			switch(prgCrc32) {
				case 0xEB2DBA63: case 0x98CFE016: return "TKOBoxing";
				case 0x135ADF7C: return "RBIBaseball";
				case 0xED588F00: return "DuckHunt";
				case 0x16D3F469: return "NinjaJajamaruKun";
				case 0x8850924B: return "Tetris";
				case 0x8C0C2DF5: return "TopGun";
				case 0x70901B25: return "Slalom";
				case 0xCF36261E: return "SuperSkyKid";
				case 0xE1AA8214: return "StarLuster";
				case 0xD5D7EAC4: return "DrMario";
				case 0xFFBEF374: return "Castlevania";
				case 0xE2C0A2BE: return "Platoon";
				case 0x29155E0C: return "ExciteBike";
				case 0xCBE85490: return "ExciteBikeB";
				case 0x07138C06: return "CluCluLand";
				case 0x43A357EF: return "IceClimber";
				case 0xD4EB5923: return "IceClimberB";
				case 0x737DD1BF: case 0x4BF3972D: case 0x8B60CC58: case 0x8192C804: return "SuperMarioBros";
				case 0xE528F651: return "Pinball";
				case 0xEC461DB9: return "PinballB";
				case 0xAE8063EF: return "MachRiderFightingCourse";
				case 0x0B65A917: case 0x8A6A9848: return "MachRider";
				case 0x46914E3E: return "Soccer";
				case 0x70433F2C: return "BattleCity";
				case 0xD99A2087: return "Gradius";
				case 0x1E438D52: return "Goonies";
				case 0xFF5135A3: return "HoganAlley";
				case 0x17AE56BE: return "FreedomForce";
				case 0xC99EC059: return "RaidBungelingBay";
				case 0xF9D3B0A3: case 0x66BB838F: case 0x9924980A: return "SuperXevious";
				case 0xA93A5AEE: return "Golf";
				case 0xCC2C4B5D: case 0x86167220: return "GolfB";
				case 0xCA85E56D: return "MightyBombJack";
				case 0xFE446787: return "Gumshoe";
			}

			return null;
		}

		public static string GetGameID()
		{
			string gameID = GetGameIdByCrc(InteropEmu.GetRomInfo().PrgCrc32);

			if(gameID != null) {
				 return gameID;
			}

			return "Unknown";
		}

		public static GameDipswitchDefinition GetDipswitchDefinition()
		{
			string gameID = GetGameID();
			if(gameID != null && _gameConfigs.ContainsKey(gameID)) {
				return _gameConfigs[gameID];
			} else {
				return null;
			}
		}

		static GameDipswitchDefinition()
		{
			XmlDocument config = new XmlDocument();
			config.Load(ResourceManager.GetZippedResource("VsSystem.xml"));

			foreach(XmlNode gameNode in config.SelectNodes("/VsSystemGames/Game")) {
				var gameDipswitches = new GameDipswitchDefinition();
				gameDipswitches.GameID = gameNode.Attributes["ID"].Value;
				gameDipswitches.GameName = gameNode.Attributes["Localization"].Value;
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
				_gameConfigs[gameNode.Attributes["ID"].Value] = gameDipswitches;
			}
		}
	}



}
