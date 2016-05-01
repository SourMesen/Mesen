using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mesen.GUI.Config
{
	public class VsConfigInfo
	{
		public string GameID;
		public string GameHash;
		public InteropEmu.PpuModel PpuModel;
		public byte DipSwitches;

		public static VsConfigInfo GetCurrentGameConfig(bool createNew)
		{
			string hash = MD5Helper.GetMD5Hash(InteropEmu.GetROMPath());
			foreach(VsConfigInfo config in ConfigManager.Config.VsConfig) {
				if(config.GameHash == hash) {
					return config;
				}
			}

			if(createNew) {
				VsConfigInfo newConfig = new VsConfigInfo();
				newConfig.GameHash = hash;
				ConfigManager.Config.VsConfig.Add(newConfig);
				return newConfig;
			} else {
				return null;
			}			
		}

		public static void ApplyConfig()
		{
			VsConfigInfo configInfo = GetCurrentGameConfig(false);
			if(configInfo != null) {
				InteropEmu.VsSetGameConfig(configInfo.PpuModel, configInfo.DipSwitches);
			}
		}
	}

	public class VsGameConfig
	{
		public string GameName;
		public List<List<string>> DipSwitches;

		private static Dictionary<string, VsGameConfig> _gameConfigs = new Dictionary<string, VsGameConfig>();

		public static string GetGameID(string romName)
		{
			romName = romName.ToLowerInvariant().Replace(" ", "");
			foreach(KeyValuePair<string, VsGameConfig> kvp in _gameConfigs) {
				if(romName.Contains(kvp.Key.ToLowerInvariant().Replace(" ", "")) || romName.Contains(kvp.Value.GameName.ToLowerInvariant().Replace(" ", ""))) {
					return kvp.Key;
				}
			}
			return "Unknown";
		}

		public static VsGameConfig GetGameConfig(string gameID)
		{
			if(gameID != null && _gameConfigs.ContainsKey(gameID)) {
				return _gameConfigs[gameID];
			} else {
				return null;
			}
		}

		public static Dictionary<string, VsGameConfig> GetGameConfigs()
		{
			return _gameConfigs;
		}

		static VsGameConfig()
		{
			XmlDocument config = new XmlDocument();
			config.Load(ResourceManager.GetZippedResource("VsSystem.xml"));

			foreach(XmlNode gameNode in config.SelectNodes("/VsSystemGames/Game")) {
				var gameConfig = new VsGameConfig();
				gameConfig.GameName = gameNode.Attributes["Localization"].Value;
				gameConfig.DipSwitches = new List<List<string>>();
				foreach(XmlNode dipSwitch in gameNode.SelectNodes("DipSwitch")) {
					if(dipSwitch.Attributes["Localization"] != null) {
						var list = new List<string>();
						gameConfig.DipSwitches.Add(list);

						list.Add(dipSwitch.Attributes["Localization"].Value);
						foreach(XmlNode option in dipSwitch.SelectNodes("Option")) {
							list.Add(option.InnerText);
						}
					} else {
						var list = new List<string>();
						gameConfig.DipSwitches.Add(list);

						list.Add("Unknown");
						list.Add("Off");
						list.Add("On");
					}
				}
				_gameConfigs[gameNode.Attributes["ID"].Value] = gameConfig;
			}
		}
	}



}
