using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public class VideoInfo
	{
		public bool LimitFPS = true;
		public bool ShowFPS = false;

		public VideoInfo()
		{
		}

		static public void ApplyConfig()
		{
			if(ConfigManager.Config.VideoInfo.LimitFPS) {
				InteropEmu.SetFlags((UInt32)EmulationFlags.LimitFPS);
			} else {
				InteropEmu.ClearFlags((UInt32)EmulationFlags.LimitFPS);
			}

			if(ConfigManager.Config.VideoInfo.ShowFPS) {
				InteropEmu.SetFlags((UInt32)EmulationFlags.ShowFPS);
			} else {
				InteropEmu.ClearFlags((UInt32)EmulationFlags.ShowFPS);
			}
		}
	}
}
