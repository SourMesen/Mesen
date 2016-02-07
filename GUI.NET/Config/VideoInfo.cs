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
		public UInt32 EmulationSpeed = 100;
		public bool ShowFPS = false;
		public UInt32 OverscanLeft = 0;
		public UInt32 OverscanRight = 0;
		public UInt32 OverscanTop = 8;
		public UInt32 OverscanBottom = 8;
		public double VideoScale = 2;
		public VideoFilterType VideoFilter = VideoFilterType.None;
		public VideoAspectRatio AspectRatio = VideoAspectRatio.Auto;
		public bool VerticalSync = true;
		public bool UseHdPacks = false;
		public Int32[] Palette = new Int32[0];

		public VideoInfo()
		{
		}

		static public void ApplyConfig()
		{
			VideoInfo videoInfo = ConfigManager.Config.VideoInfo;

			InteropEmu.SetEmulationSpeed(videoInfo.EmulationSpeed);

			InteropEmu.SetFlag(EmulationFlags.ShowFPS, videoInfo.ShowFPS);
			InteropEmu.SetFlag(EmulationFlags.VerticalSync, videoInfo.VerticalSync);
			InteropEmu.SetFlag(EmulationFlags.UseHdPacks, videoInfo.UseHdPacks);

			InteropEmu.SetOverscanDimensions(videoInfo.OverscanLeft, videoInfo.OverscanRight, videoInfo.OverscanTop, videoInfo.OverscanBottom);

			InteropEmu.SetVideoFilter(videoInfo.VideoFilter);
			InteropEmu.SetVideoScale(videoInfo.VideoScale);
			InteropEmu.SetVideoAspectRatio(videoInfo.AspectRatio);

			if(videoInfo.Palette.Length == 64) {
				InteropEmu.SetRgbPalette(videoInfo.Palette);
			}
		}
	}
}
