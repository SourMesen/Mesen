using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public class GameSpecificInfo
	{
		public string GameName;
		public string GamePrgCrc32;

		public bool OverrideOverscan;
		public UInt32 OverscanLeft;
		public UInt32 OverscanRight;
		public UInt32 OverscanTop;
		public UInt32 OverscanBottom;

		public UInt32 DipSwitches = 0;

		public static GameSpecificInfo GetGameSpecificInfo()
		{
			RomInfo romInfo = InteropEmu.GetRomInfo();
			GameSpecificInfo existingConfig = ConfigManager.Config.GameSpecificSettings.Find(gameConfig => gameConfig.GamePrgCrc32 == romInfo.GetPrgCrcString());
			return existingConfig;
		}

		public static void AddGameSpecificConfig(GameSpecificInfo info)
		{
			if(!ConfigManager.Config.GameSpecificSettings.Contains(info)) {
				ConfigManager.Config.GameSpecificSettings.Add(info);
			}
		}

		public static GameSpecificInfo CreateGameSpecificConfig()
		{
			RomInfo romInfo = InteropEmu.GetRomInfo();
			GameSpecificInfo info = new GameSpecificInfo();
			info.GameName = romInfo.GetRomName();
			info.GamePrgCrc32 = romInfo.GetPrgCrcString();
			return info;
		}

		public static void ApplyGameSpecificConfig()
		{
			GameSpecificInfo existingConfig = GetGameSpecificInfo();
			if(existingConfig != null) {
				InteropEmu.SetDipSwitches(existingConfig.DipSwitches);
			} else {
				GameDipswitchDefinition dipswitchDefinition = GameDipswitchDefinition.GetDipswitchDefinition();
				if(dipswitchDefinition != null) {
					InteropEmu.SetDipSwitches(dipswitchDefinition.DefaultDipSwitches);
				} else {
					InteropEmu.SetDipSwitches(0);
				}
			}
		}

		public static void SetDipswitches(UInt32 dipswitches)
		{
			GameSpecificInfo existingConfig = GetGameSpecificInfo();

			if(existingConfig != null) {
				existingConfig.DipSwitches = dipswitches;
			} else {
				GameSpecificInfo info = new GameSpecificInfo();
				info.DipSwitches = dipswitches;
				ConfigManager.Config.GameSpecificSettings.Add(info);
			}
			ApplyGameSpecificConfig();

			ConfigManager.ApplyChanges();
		}

		public static void SetGameSpecificOverscan(bool overrideOverscan, UInt32 top, UInt32 bottom, UInt32 left, UInt32 right)
		{
			RomInfo romInfo = InteropEmu.GetRomInfo();
			if(romInfo.PrgCrc32 == 0) {
				return;
			}

			GameSpecificInfo existingConfig = ConfigManager.Config.GameSpecificSettings.Find(gameConfig => gameConfig.GamePrgCrc32 == romInfo.GetPrgCrcString());

			if(overrideOverscan || existingConfig != null) {
				//Only add if the config already exists, or if override setting is turned on
				GameSpecificInfo info = existingConfig ?? new GameSpecificInfo();
				info.GameName = romInfo.GetRomName();
				info.GamePrgCrc32 = romInfo.GetPrgCrcString();
				info.OverrideOverscan = overrideOverscan;
				info.OverscanTop = top;
				info.OverscanBottom = bottom;
				info.OverscanLeft = left;
				info.OverscanRight = right;

				if(existingConfig == null) {
					ConfigManager.Config.GameSpecificSettings.Add(info);
				}
			}
		}
	}
}
