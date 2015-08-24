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
		public Int32 FpsLimit = -1;
		public bool ShowFPS = false;
		public UInt32 OverscanLeft = 0;
		public UInt32 OverscanRight = 0;
		public UInt32 OverscanTop = 8;
		public UInt32 OverscanBottom = 8;

		public VideoInfo()
		{
		}

		static public void ApplyConfig()
		{
			VideoInfo videoInfo = ConfigManager.Config.VideoInfo;
			
			InteropEmu.SetFpsLimit(videoInfo.FpsLimit);

			if(ConfigManager.Config.VideoInfo.ShowFPS) {
				InteropEmu.SetFlags((UInt32)EmulationFlags.ShowFPS);
			} else {
				InteropEmu.ClearFlags((UInt32)EmulationFlags.ShowFPS);
			}

			InteropEmu.SetOverscanDimensions(videoInfo.OverscanLeft, videoInfo.OverscanRight, videoInfo.OverscanTop, videoInfo.OverscanBottom);
		}

		static public Size GetViewerSize()
		{
			VideoInfo videoInfo = ConfigManager.Config.VideoInfo;
			return new Size((int)(256-videoInfo.OverscanLeft-videoInfo.OverscanRight)*4, (int)(240-videoInfo.OverscanTop-videoInfo.OverscanBottom)*4);
		}
	}
}
