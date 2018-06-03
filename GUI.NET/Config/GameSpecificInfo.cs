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

		public static GameSpecificInfo GetGameSpecificInfo()
		{
			RomInfo romInfo = InteropEmu.GetRomInfo();
			GameSpecificInfo existingConfig = ConfigManager.Config.GameSpecificSettings.Find(gameConfig => gameConfig.GamePrgCrc32 == romInfo.GetPrgCrcString());
			return existingConfig;
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
