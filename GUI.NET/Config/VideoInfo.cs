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
		public bool ShowFPS = false;
		public UInt32 OverscanLeft = 0;
		public UInt32 OverscanRight = 0;
		public UInt32 OverscanTop = 8;
		public UInt32 OverscanBottom = 8;
		public double VideoScale = 2;
		public VideoFilterType VideoFilter = VideoFilterType.None;
		public bool UseBilinearInterpolation = false;
		public VideoAspectRatio AspectRatio = VideoAspectRatio.Auto;
		public bool VerticalSync = false;
		public bool UseHdPacks = false;
		public string PaletteData;

		public Int32 Brightness = 0;
		public Int32 Contrast = 0;
		public Int32 Hue = 0;
		public Int32 Saturation = 0;
		public Int32 ScanlineIntensity = 0;

		public Int32 NtscArtifacts = 0;
		public Int32 NtscBleed = 0;
		public Int32 NtscFringing = 0;
		public Int32 NtscGamma = 0;
		public Int32 NtscResolution = 0;
		public Int32 NtscSharpness = 0;
		public bool NtscMergeFields = false;

		public bool DisableBackground = false;
		public bool DisableSprites = false;

		public List<PaletteInfo> SavedPalettes = new List<PaletteInfo>();

		public VideoInfo()
		{
		}

		static public void ApplyConfig()
		{
			VideoInfo videoInfo = ConfigManager.Config.VideoInfo;

			InteropEmu.SetFlag(EmulationFlags.ShowFPS, videoInfo.ShowFPS);
			InteropEmu.SetFlag(EmulationFlags.VerticalSync, videoInfo.VerticalSync);
			InteropEmu.SetFlag(EmulationFlags.UseHdPacks, videoInfo.UseHdPacks);

			InteropEmu.SetFlag(EmulationFlags.DisableBackground, videoInfo.DisableBackground);
			InteropEmu.SetFlag(EmulationFlags.DisableSprites, videoInfo.DisableSprites);

			InteropEmu.SetOverscanDimensions(videoInfo.OverscanLeft, videoInfo.OverscanRight, videoInfo.OverscanTop, videoInfo.OverscanBottom);

			InteropEmu.SetVideoFilter(videoInfo.VideoFilter);
			InteropEmu.SetVideoResizeFilter(videoInfo.UseBilinearInterpolation ? VideoResizeFilter.Bilinear : VideoResizeFilter.NearestNeighbor);
			InteropEmu.SetVideoScale(videoInfo.VideoScale);
			InteropEmu.SetVideoAspectRatio(videoInfo.AspectRatio);

			InteropEmu.SetPictureSettings(videoInfo.Brightness / 100.0, videoInfo.Contrast / 100.0, videoInfo.Saturation / 100.0, videoInfo.Hue / 100.0, videoInfo.ScanlineIntensity / 100.0);
			InteropEmu.SetNtscFilterSettings(videoInfo.NtscArtifacts / 100.0, videoInfo.NtscBleed / 100.0, videoInfo.NtscFringing / 100.0, videoInfo.NtscGamma / 100.0, videoInfo.NtscResolution / 100.0, videoInfo.NtscSharpness / 100.0, videoInfo.NtscMergeFields);

			if(!string.IsNullOrWhiteSpace(videoInfo.PaletteData)) {
				try {
					byte[] palette = System.Convert.FromBase64String(videoInfo.PaletteData);
					if(palette.Length == 64*4) {
						InteropEmu.SetRgbPalette(palette);
					}
				} catch { }
			}
		}

		public void AddPalette(string paletteName, byte[] paletteData)
		{
			string base64Data = System.Convert.ToBase64String(paletteData);
			foreach(PaletteInfo existingPalette in this.SavedPalettes) {
				if(existingPalette.Name == paletteName) {
					//Update existing palette
					existingPalette.Palette = base64Data;
					return;
				}
			}

			if(this.SavedPalettes.Count >= 5) {
				//Remove oldest palette
				this.SavedPalettes.RemoveAt(0);
			}

			PaletteInfo palette = new PaletteInfo();
			palette.Name = paletteName;
			palette.Palette = base64Data;
			this.SavedPalettes.Add(palette);
		}

		public Int32[] GetPalette(string paletteName)
		{
			foreach(PaletteInfo existingPalette in this.SavedPalettes) {
				if(existingPalette.Name == paletteName) {
					byte[] paletteData = System.Convert.FromBase64String(existingPalette.Palette);

					int[] result = new int[paletteData.Length / sizeof(int)];
					Buffer.BlockCopy(paletteData, 0, result, 0, paletteData.Length);
					return result;
				}
			}

			return null;
		}
	}

	public class PaletteInfo
	{
		public string Name;
		public string Palette; //Base64
	}
}
