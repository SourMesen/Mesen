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
		[MinMax(0, 100)] public UInt32 OverscanLeft = 0;
		[MinMax(0, 100)] public UInt32 OverscanRight = 0;
		[MinMax(0, 100)] public UInt32 OverscanTop = 0;
		[MinMax(0, 100)] public UInt32 OverscanBottom = 0;
		[MinMax(0.1, 10.0)] public double VideoScale = 2;
		public VideoFilterType VideoFilter = VideoFilterType.None;
		public bool UseBilinearInterpolation = false;
		public VideoAspectRatio AspectRatio = VideoAspectRatio.NoStretching;
		public ScreenRotation ScreenRotation = ScreenRotation.None;
		[MinMax(0.1, 5.0)] public double CustomAspectRatio = 1.0;
		public bool VerticalSync = false;
		public bool UseHdPacks = true;
		public bool IntegerFpsMode = false;
		public string PaletteData;

		[MinMax(-100, 100)] public Int32 Brightness = 0;
		[MinMax(-100, 100)] public Int32 Contrast = 0;
		[MinMax(-100, 100)] public Int32 Hue = 0;
		[MinMax(-100, 100)] public Int32 Saturation = 0;
		[MinMax(0, 100)] public Int32 ScanlineIntensity = 0;

		[MinMax(-100, 100)] public Int32 NtscArtifacts = 0;
		[MinMax(-100, 100)] public Int32 NtscBleed = 0;
		[MinMax(-100, 100)] public Int32 NtscFringing = 0;
		[MinMax(-100, 100)] public Int32 NtscGamma = 0;
		[MinMax(-100, 100)] public Int32 NtscResolution = 0;
		[MinMax(-100, 100)] public Int32 NtscSharpness = 0;
		public bool NtscMergeFields = false;
		public bool NtscVerticalBlend = true;

		[MinMax(-50, 400)] public Int32 NtscYFilterLength = 0;
		[MinMax(0, 400)] public Int32 NtscIFilterLength = 50;
		[MinMax(0, 400)] public Int32 NtscQFilterLength = 50;

		public bool RemoveSpriteLimit = false;
		public bool AdaptiveSpriteLimit = true;
		public bool DisableBackground = false;
		public bool DisableSprites = false;
		public bool ForceBackgroundFirstColumn = false;
		public bool ForceSpritesFirstColumn = false;

		public bool FullscreenForceIntegerScale = false;
		public bool UseExclusiveFullscreen = false;
		public string FullscreenResolution = "";
		public VideoRefreshRates ExclusiveFullscreenRefreshRate = VideoRefreshRates._60;

		public bool UseCustomVsPalette = false;
		public bool ShowColorIndexes = true;

		public List<PaletteInfo> SavedPalettes = new List<PaletteInfo>();

		public VideoInfo()
		{
		}

		static public void ApplyOverscanConfig()
		{
			GameSpecificInfo gameSpecificInfo = GameSpecificInfo.GetGameSpecificInfo();
			if(gameSpecificInfo != null && gameSpecificInfo.OverrideOverscan) {
				InteropEmu.SetOverscanDimensions(gameSpecificInfo.OverscanLeft, gameSpecificInfo.OverscanRight, gameSpecificInfo.OverscanTop, gameSpecificInfo.OverscanBottom);
			} else {
				VideoInfo videoInfo = ConfigManager.Config.VideoInfo;
				InteropEmu.SetOverscanDimensions(videoInfo.OverscanLeft, videoInfo.OverscanRight, videoInfo.OverscanTop, videoInfo.OverscanBottom);
			}
		}

		static public void ApplyConfig()
		{
			VideoInfo videoInfo = ConfigManager.Config.VideoInfo;

			InteropEmu.SetFlag(EmulationFlags.ShowFPS, videoInfo.ShowFPS);
			InteropEmu.SetFlag(EmulationFlags.VerticalSync, videoInfo.VerticalSync);
			InteropEmu.SetFlag(EmulationFlags.UseHdPacks, videoInfo.UseHdPacks);
			InteropEmu.SetFlag(EmulationFlags.IntegerFpsMode, videoInfo.IntegerFpsMode);

			InteropEmu.SetFlag(EmulationFlags.RemoveSpriteLimit, videoInfo.RemoveSpriteLimit);
			InteropEmu.SetFlag(EmulationFlags.AdaptiveSpriteLimit, videoInfo.AdaptiveSpriteLimit);
			InteropEmu.SetFlag(EmulationFlags.DisableBackground, videoInfo.DisableBackground);
			InteropEmu.SetFlag(EmulationFlags.DisableSprites, videoInfo.DisableSprites);
			InteropEmu.SetFlag(EmulationFlags.ForceBackgroundFirstColumn, videoInfo.ForceBackgroundFirstColumn);
			InteropEmu.SetFlag(EmulationFlags.ForceSpritesFirstColumn, videoInfo.ForceSpritesFirstColumn);

			InteropEmu.SetFlag(EmulationFlags.UseCustomVsPalette, videoInfo.UseCustomVsPalette);

			VideoInfo.ApplyOverscanConfig();
			InteropEmu.SetScreenRotation((UInt32)videoInfo.ScreenRotation);

			InteropEmu.SetExclusiveRefreshRate((UInt32)videoInfo.ExclusiveFullscreenRefreshRate);

			InteropEmu.SetVideoFilter(videoInfo.VideoFilter);
			InteropEmu.SetVideoResizeFilter(videoInfo.UseBilinearInterpolation ? VideoResizeFilter.Bilinear : VideoResizeFilter.NearestNeighbor);
			InteropEmu.SetVideoScale(videoInfo.VideoScale <= 10 ? videoInfo.VideoScale : 2);
			InteropEmu.SetVideoAspectRatio(videoInfo.AspectRatio, videoInfo.CustomAspectRatio);

			InteropEmu.SetPictureSettings(videoInfo.Brightness / 100.0, videoInfo.Contrast / 100.0, videoInfo.Saturation / 100.0, videoInfo.Hue / 100.0, videoInfo.ScanlineIntensity / 100.0);
			InteropEmu.SetNtscFilterSettings(videoInfo.NtscArtifacts / 100.0, videoInfo.NtscBleed / 100.0, videoInfo.NtscFringing / 100.0, videoInfo.NtscGamma / 100.0, videoInfo.NtscResolution / 100.0, videoInfo.NtscSharpness / 100.0, videoInfo.NtscMergeFields, videoInfo.NtscYFilterLength / 100.0, videoInfo.NtscIFilterLength / 100.0, videoInfo.NtscQFilterLength / 100.0, videoInfo.NtscVerticalBlend);

			if(!string.IsNullOrWhiteSpace(videoInfo.PaletteData)) {
				try {
					byte[] palette = Convert.FromBase64String(videoInfo.PaletteData);
					if(palette.Length == 64*4 || palette.Length == 512*4) {
						InteropEmu.SetRgbPalette(palette, (UInt32)(palette.Length / 4));
					}
				} catch { }
			}
		}

		public bool IsFullColorPalette()
		{
			if(!string.IsNullOrWhiteSpace(this.PaletteData)) {
				byte[] palette = Convert.FromBase64String(this.PaletteData);
				return palette.Length == 512 * 4;
			}
			return false;
		}

		public void AddPalette(string paletteName, byte[] paletteData)
		{
			string base64Data = Convert.ToBase64String(paletteData);
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
					byte[] paletteData = Convert.FromBase64String(existingPalette.Palette);

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
