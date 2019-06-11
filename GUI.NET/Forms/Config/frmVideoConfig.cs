﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	public partial class frmVideoConfig : BaseConfigForm
	{
		private Int32[] _paletteData;
		
		public frmVideoConfig()
		{
			InitializeComponent();

			ThemeHelper.FixMonoColors(contextPaletteList);
			ThemeHelper.FixMonoColors(contextPicturePresets);

			Entity = ConfigManager.Config.VideoInfo;
			
			AddBinding("ShowFPS", chkShowFps);
			AddBinding("UseBilinearInterpolation", chkBilinearInterpolation);
			AddBinding("VerticalSync", chkVerticalSync);
			AddBinding("UseHdPacks", chkUseHdPacks);
			AddBinding("IntegerFpsMode", chkIntegerFpsMode);
			AddBinding("FullscreenForceIntegerScale", chkFullscreenForceIntegerScale);
			AddBinding("UseExclusiveFullscreen", chkUseExclusiveFullscreen);
			AddBinding("ExclusiveFullscreenRefreshRate", cboRefreshRate);

			AddBinding("VideoScale", nudScale);
			AddBinding("AspectRatio", cboAspectRatio);
			AddBinding("CustomAspectRatio", nudCustomRatio);
			AddBinding("VideoFilter", cboFilter);

			AddBinding("ScreenRotation", cboScreenRotation);

			AddBinding("OverscanLeft", nudOverscanLeft);
			AddBinding("OverscanRight", nudOverscanRight);
			AddBinding("OverscanTop", nudOverscanTop);
			AddBinding("OverscanBottom", nudOverscanBottom);

			AddBinding("Brightness", trkBrightness);
			AddBinding("Contrast", trkContrast);
			AddBinding("Hue", trkHue);
			AddBinding("Saturation", trkSaturation);
			AddBinding("ScanlineIntensity", trkScanlines);

			AddBinding("NtscArtifacts", trkArtifacts);
			AddBinding("NtscBleed", trkBleed);
			AddBinding("NtscFringing", trkFringing);
			AddBinding("NtscGamma", trkGamma);
			AddBinding("NtscResolution", trkResolution);
			AddBinding("NtscSharpness", trkSharpness);
			AddBinding("NtscMergeFields", chkMergeFields);
			AddBinding("NtscVerticalBlend", chkVerticalBlend);

			AddBinding("NtscYFilterLength", trkYFilterLength);
			AddBinding("NtscIFilterLength", trkIFilterLength);
			AddBinding("NtscQFilterLength", trkQFilterLength);

			AddBinding("RemoveSpriteLimit", chkRemoveSpriteLimit);
			AddBinding("AdaptiveSpriteLimit", chkAdaptiveSpriteLimit);
			AddBinding("DisableBackground", chkDisableBackground);
			AddBinding("DisableSprites", chkDisableSprites);
			AddBinding("ForceBackgroundFirstColumn", chkForceBackgroundFirstColumn);
			AddBinding("ForceSpritesFirstColumn", chkForceSpritesFirstColumn);

			AddBinding("UseCustomVsPalette", chkUseCustomVsPalette);

			AddBinding("ShowColorIndexes", chkShowColorIndexes);

			_paletteData = InteropEmu.GetRgbPalette();
			if(!ConfigManager.Config.VideoInfo.IsFullColorPalette()) {
				Array.Resize(ref _paletteData, 64);
			}
			
			RefreshPalette();

			toolTip.SetToolTip(picHdNesTooltip, ResourceHelper.GetMessage("HDNesTooltip"));

			btnSelectPreset.Image = BaseControl.DownArrow;
			btnSelectPalette.Image = BaseControl.DownArrow;

			UpdateOverscanImage(picOverscan, (int)nudOverscanTop.Value, (int)nudOverscanBottom.Value, (int)nudOverscanLeft.Value, (int)nudOverscanRight.Value);
			UpdateOverscanImage(picGameSpecificOverscan, (int)nudGameSpecificOverscanTop.Value, (int)nudGameSpecificOverscanBottom.Value, (int)nudGameSpecificOverscanLeft.Value, (int)nudGameSpecificOverscanRight.Value);

			ResourceHelper.ApplyResources(this, contextPaletteList);
			ResourceHelper.ApplyResources(this, contextPicturePresets);

			if(Program.IsMono) {
				//Not available in the linux build (for now)
				chkUseExclusiveFullscreen.Visible = false;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			//Do this after we localize the form
			InitializeGameSpecificOverscanTab();
		}

		private void InitializeGameSpecificOverscanTab()
		{
			RomInfo romInfo = InteropEmu.GetRomInfo();
			if(romInfo.PrgCrc32 == 0) {
				chkEnableGameSpecificOverscan.Enabled = false;
			} else {
				chkEnableGameSpecificOverscan.Text += " (" + romInfo.GetRomName() + ")";

				GameSpecificInfo info = GameSpecificInfo.GetGameSpecificInfo();
				if(info != null) {
					chkEnableGameSpecificOverscan.Checked = info.OverrideOverscan;
					if(chkEnableGameSpecificOverscan.Checked) {
						tabOverscan.SelectedTab = tpgOverscanGameSpecific;
					}
					nudGameSpecificOverscanTop.Value = info.OverscanTop;
					nudGameSpecificOverscanBottom.Value = info.OverscanBottom;
					nudGameSpecificOverscanLeft.Value = info.OverscanLeft;
					nudGameSpecificOverscanRight.Value = info.OverscanRight;
				}
			}

			//Change name to share resources with global overscan tab
			lblGameSpecificOverscanBottom.Name = "lblBottom";
			lblGameSpecificOverscanLeft.Name = "lblLeft";
			lblGameSpecificOverscanRight.Name = "lblRight";
			lblGameSpecificOverscanTop.Name = "lblTop";
		}

		private void UpdatePalette()
		{
			byte[] result = new byte[_paletteData.Length * sizeof(int)];
			Buffer.BlockCopy(_paletteData, 0, result, 0, result.Length);
			((VideoInfo)Entity).PaletteData = Convert.ToBase64String(result);
		}

		protected override bool ValidateInput()
		{
			UpdateObject();
			UpdateCustomRatioVisibility();
			UpdatePalette();
			VideoFilterType filter = ((VideoInfo)Entity).VideoFilter;
			if(filter == VideoFilterType.NTSC) {
				tlpNtscFilter1.Visible = true;
				tlpNtscFilter2.Visible = false;
				chkMergeFields.Visible = true;
				grpNtscFilter.Visible = true;
			} else if(filter == VideoFilterType.BisqwitNtsc || filter == VideoFilterType.BisqwitNtscHalfRes || filter == VideoFilterType.BisqwitNtscQuarterRes) {
				tlpNtscFilter1.Visible = true;
				tlpNtscFilter2.Visible = true;
				chkMergeFields.Visible = false;
				grpNtscFilter.Visible = true;
			} else {
				grpNtscFilter.Visible = false;
			}

			VideoInfo.ApplyConfig();

			if(chkEnableGameSpecificOverscan.Checked) {
				InteropEmu.SetOverscanDimensions(
					(UInt32)nudGameSpecificOverscanLeft.Value,
					(UInt32)nudGameSpecificOverscanRight.Value,
					(UInt32)nudGameSpecificOverscanTop.Value,
					(UInt32)nudGameSpecificOverscanBottom.Value
				);
			}

			return true;
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(DialogResult == DialogResult.OK) {
				UpdatePalette();

				GameSpecificInfo.SetGameSpecificOverscan(
					chkEnableGameSpecificOverscan.Checked,
					(UInt32)nudGameSpecificOverscanTop.Value,
					(UInt32)nudGameSpecificOverscanBottom.Value,
					(UInt32)nudGameSpecificOverscanLeft.Value,
					(UInt32)nudGameSpecificOverscanRight.Value
				);
			}
			base.OnFormClosed(e);
			VideoInfo.ApplyConfig();
		}

		private void RefreshPalette()
		{
			this.ctrlPaletteDisplay.ShowColorIndexes = chkShowColorIndexes.Checked;
			this.ctrlPaletteDisplay.PaletteData = _paletteData;
		}

		private void ctrlPaletteDisplay_ColorClick(int colorIndex)
		{
			colorDialog.SolidColorOnly = true;
			colorDialog.AllowFullOpen = true;
			colorDialog.FullOpen = true;
			colorDialog.Color = Color.FromArgb(_paletteData[colorIndex]);
			if(colorDialog.ShowDialog() == DialogResult.OK) {
				_paletteData[colorIndex] = colorDialog.Color.ToArgb();
			}

			RefreshPalette();
		}

		private void btnLoadPalFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter("Palette Files (*.pal)|*.pal|All Files (*.*)|*.*");
			if(ofd.ShowDialog() == DialogResult.OK) {
				using(FileStream paletteFile = File.OpenRead(ofd.FileName)) {
					byte[] paletteFileData = new byte[512*3];
					int byteCount = paletteFile.Read(paletteFileData, 0, 512 * 3);
					if(byteCount == 64*3 || byteCount == 512*3) {
						_paletteData = new Int32[byteCount / 3];
						for(int i = 0; i < byteCount; i += 3) {
							_paletteData[i / 3] = (Int32)((UInt32)0xFF000000 | (UInt32)paletteFileData[i + 2] | (UInt32)(paletteFileData[i + 1] << 8) | (UInt32)(paletteFileData[i] << 16));
						}
						RefreshPalette();

						byte[] bytePalette = new byte[_paletteData.Length * sizeof(int)];
						Buffer.BlockCopy(_paletteData, 0, bytePalette, 0, bytePalette.Length);
						ConfigManager.Config.VideoInfo.AddPalette(Path.GetFileNameWithoutExtension(ofd.FileName), bytePalette);
					}
					paletteFile.Close();
				}
			}
		}

		private void UpdateOverscanImage(PictureBox picture, int top, int bottom, int left, int right)
		{
			Bitmap overscan = new Bitmap(picture.Width - 2, picture.Height - 2);

			using(Graphics g = Graphics.FromImage(overscan)) {
				g.Clear(Color.DarkGray);

				Rectangle fg = new Rectangle(left, top, 256 - left - right, 240 - top - bottom);
				g.ScaleTransform((float)overscan.Width / 256, (float)overscan.Height / 240);
				g.FillRectangle(Brushes.LightCyan, fg);
			}
			picture.Image = overscan;
		}

		private void nudOverscan_ValueChanged(object sender, EventArgs e)
		{
			UpdateOverscanImage(picOverscan, (int)nudOverscanTop.Value, (int)nudOverscanBottom.Value, (int)nudOverscanLeft.Value, (int)nudOverscanRight.Value);
		}

		private void nudGameSpecificOverscan_ValueChanged(object sender, EventArgs e)
		{
			UpdateOverscanImage(picGameSpecificOverscan, (int)nudGameSpecificOverscanTop.Value, (int)nudGameSpecificOverscanBottom.Value, (int)nudGameSpecificOverscanLeft.Value, (int)nudGameSpecificOverscanRight.Value);
		}

		private void btnResetPictureSettings_Click(object sender, EventArgs e)
		{
			cboFilter.SelectedIndex = 0;
			trkBrightness.Value = 0;
			trkContrast.Value = 0;
			trkHue.Value = 0;
			trkSaturation.Value = 0;

			trkScanlines.Value = 0;

			trkArtifacts.Value = 0;
			trkBleed.Value = 0;
			trkFringing.Value = 0;
			trkGamma.Value = 0;
			trkResolution.Value = 0;
			trkSharpness.Value = 0;
			chkMergeFields.Checked = false;
			chkVerticalBlend.Checked = true;

			trkYFilterLength.Value = 0;
			trkIFilterLength.Value = 50;
			trkQFilterLength.Value = 50;
		}

		private void btnSelectPreset_Click(object sender, EventArgs e)
		{
			contextPicturePresets.Show(btnSelectPreset.PointToScreen(new Point(0, btnSelectPreset.Height-1)));
		}

		private void mnuPresetComposite_Click(object sender, EventArgs e)
		{
			cboFilter.SelectedIndex = 1;
			trkHue.Value = 0;
			trkSaturation.Value = 0;
			trkContrast.Value = 0;
			trkBrightness.Value = 0;
			trkSharpness.Value = 0;
			trkGamma.Value = 0;
			trkResolution.Value = 0;
			trkArtifacts.Value = 0;
			trkFringing.Value = 0;
			trkBleed.Value = 0;
			chkMergeFields.Checked = false;

			trkScanlines.Value = 15;
		}

		private void mnuPresetSVideo_Click(object sender, EventArgs e)
		{
			cboFilter.SelectedIndex = 1;
			trkHue.Value = 0;
			trkSaturation.Value = 0;
			trkContrast.Value = 0;
			trkBrightness.Value = 0;
			trkSharpness.Value = 20;
			trkGamma.Value = 0;
			trkResolution.Value = 20;
			trkArtifacts.Value = -100;
			trkFringing.Value = -100;
			trkBleed.Value = 0;
			chkMergeFields.Checked = false;

			trkScanlines.Value = 15;
		}

		private void mnuPresetRgb_Click(object sender, EventArgs e)
		{
			cboFilter.SelectedIndex = 1;
			trkHue.Value = 0;
			trkSaturation.Value = 0;
			trkContrast.Value = 0;
			trkBrightness.Value = 0;
			trkSharpness.Value = 20;
			trkGamma.Value = 0;
			trkResolution.Value = 70;
			trkArtifacts.Value = -100;
			trkFringing.Value = -100;
			trkBleed.Value = -100;
			chkMergeFields.Checked = false;

			trkScanlines.Value = 15;
		}

		private void mnuPresetMonochrome_Click(object sender, EventArgs e)
		{
			cboFilter.SelectedIndex = 1;
			trkHue.Value = 0;
			trkSaturation.Value = -100;
			trkContrast.Value = 0;
			trkBrightness.Value = 0;
			trkSharpness.Value = 20;
			trkGamma.Value = 0;
			trkResolution.Value = 70;
			trkArtifacts.Value = -20;
			trkFringing.Value = -20;
			trkBleed.Value = -10;
			chkMergeFields.Checked = false;

			trkScanlines.Value = 15;
		}

		private void btnSelectPalette_Click(object sender, EventArgs e)
		{
			contextPaletteList.Show(btnSelectPalette.PointToScreen(new Point(0, btnSelectPalette.Height-1)));
		}

		private void UpdatePalette(UInt32[] newPalette)
		{
			_paletteData = new Int32[newPalette.Length];
			for(int i = 0; i < newPalette.Length; i++) {
				_paletteData[i] = (Int32)newPalette[i];
			}
			RefreshPalette();
		}

		private void mnuDefaultPalette_Click(object sender, EventArgs e)
		{
			UpdatePalette(new UInt32[] { 0xFF666666, 0xFF002A88, 0xFF1412A7, 0xFF3B00A4, 0xFF5C007E, 0xFF6E0040, 0xFF6C0600, 0xFF561D00, 0xFF333500, 0xFF0B4800, 0xFF005200, 0xFF004F08, 0xFF00404D, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFADADAD, 0xFF155FD9, 0xFF4240FF, 0xFF7527FE, 0xFFA01ACC, 0xFFB71E7B, 0xFFB53120, 0xFF994E00, 0xFF6B6D00, 0xFF388700, 0xFF0C9300, 0xFF008F32, 0xFF007C8D, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFF64B0FF, 0xFF9290FF, 0xFFC676FF, 0xFFF36AFF, 0xFFFE6ECC, 0xFFFE8170, 0xFFEA9E22, 0xFFBCBE00, 0xFF88D800, 0xFF5CE430, 0xFF45E082, 0xFF48CDDE, 0xFF4F4F4F, 0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFFC0DFFF, 0xFFD3D2FF, 0xFFE8C8FF, 0xFFFBC2FF, 0xFFFEC4EA, 0xFFFECCC5, 0xFFF7D8A5, 0xFFE4E594, 0xFFCFEF96, 0xFFBDF4AB, 0xFFB3F3CC, 0xFFB5EBF2, 0xFFB8B8B8, 0xFF000000, 0xFF000000 });
		}

		private void mnuPaletteUnsaturated_Click(object sender, EventArgs e)
		{
			UpdatePalette(new UInt32[] { 0xFF6B6B6B, 0xFF001E87, 0xFF1F0B96, 0xFF3B0C87, 0xFF590D61, 0xFF5E0528, 0xFF551100, 0xFF461B00, 0xFF303200, 0xFF0A4800, 0xFF004E00, 0xFF004619, 0xFF003A58, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFB2B2B2, 0xFF1A53D1, 0xFF4835EE, 0xFF7123EC, 0xFF9A1EB7, 0xFFA51E62, 0xFFA52D19, 0xFF874B00, 0xFF676900, 0xFF298400, 0xFF038B00, 0xFF008240, 0xFF007891, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF63ADFD, 0xFF908AFE, 0xFFB977FC, 0xFFE771FE, 0xFFF76FC9, 0xFFF5836A, 0xFFDD9C29, 0xFFBDB807, 0xFF84D107, 0xFF5BDC3B, 0xFF48D77D, 0xFF48CCCE, 0xFF555555, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFC4E3FE, 0xFFD7D5FE, 0xFFE6CDFE, 0xFFF9CAFE, 0xFFFEC9F0, 0xFFFED1C7, 0xFFF7DCAC, 0xFFE8E89C, 0xFFD1F29D, 0xFFBFF4B1, 0xFFB7F5CD, 0xFFB7F0EE, 0xFFBEBEBE, 0xFF000000, 0xFF000000 });
		}

		private void mnuPaletteYuv_Click(object sender, EventArgs e)
		{
			UpdatePalette(new UInt32[] { 0xFF666666, 0xFF002A88, 0xFF1412A7, 0xFF3B00A4, 0xFF5C007E, 0xFF6E0040, 0xFF6C0700, 0xFF561D00, 0xFF333500, 0xFF0C4800, 0xFF005200, 0xFF004C18, 0xFF003E5B, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFADADAD, 0xFF155FD9, 0xFF4240FF, 0xFF7527FE, 0xFFA01ACC, 0xFFB71E7B, 0xFFB53120, 0xFF994E00, 0xFF6B6D00, 0xFF388700, 0xFF0D9300, 0xFF008C47, 0xFF007AA0, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF64B0FF, 0xFF9290FF, 0xFFC676FF, 0xFFF26AFF, 0xFFFF6ECC, 0xFFFF8170, 0xFFEA9E22, 0xFFBCBE00, 0xFF88D800, 0xFF5CE430, 0xFF45E082, 0xFF48CDDE, 0xFF4F4F4F, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFC0DFFF, 0xFFD3D2FF, 0xFFE8C8FF, 0xFFFAC2FF, 0xFFFFC4EA, 0xFFFFCCC5, 0xFFF7D8A5, 0xFFE4E594, 0xFFCFEF96, 0xFFBDF4AB, 0xFFB3F3CC, 0xFFB5EBF2, 0xFFB8B8B8, 0xFF000000, 0xFF000000 });
		}

		private void mnuPaletteNestopiaRgb_Click(object sender, EventArgs e)
		{
			UpdatePalette(new UInt32[] { 0xFF6D6D6D, 0xFF002492, 0xFF0000DB, 0xFF6D49DB, 0xFF92006D, 0xFFB6006D, 0xFFB62400, 0xFF924900, 0xFF6D4900, 0xFF244900, 0xFF006D24, 0xFF009200, 0xFF004949, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFB6B6B6, 0xFF006DDB, 0xFF0049FF, 0xFF9200FF, 0xFFB600FF, 0xFFFF0092, 0xFFFF0000, 0xFFDB6D00, 0xFF926D00, 0xFF249200, 0xFF009200, 0xFF00B66D, 0xFF009292, 0xFF242424, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF6DB6FF, 0xFF9292FF, 0xFFDB6DFF, 0xFFFF00FF, 0xFFFF6DFF, 0xFFFF9200, 0xFFFFB600, 0xFFDBDB00, 0xFF6DDB00, 0xFF00FF00, 0xFF49FFDB, 0xFF00FFFF, 0xFF494949, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFB6DBFF, 0xFFDBB6FF, 0xFFFFB6FF, 0xFFFF92FF, 0xFFFFB6B6, 0xFFFFDB92, 0xFFFFFF49, 0xFFFFFF6D, 0xFFB6FF49, 0xFF92FF6D, 0xFF49FFDB, 0xFF92DBFF, 0xFF929292, 0xFF000000, 0xFF000000 });
		}

		private void mnuPaletteCompositeDirect_Click(object sender, EventArgs e)
		{
			UpdatePalette(new UInt32[] { 0xFF656565, 0xFF00127D, 0xFF18008E, 0xFF360082, 0xFF56005D, 0xFF5A0018, 0xFF4F0500, 0xFF381900, 0xFF1D3100, 0xFF003D00, 0xFF004100, 0xFF003B17, 0xFF002E55, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFAFAFAF, 0xFF194EC8, 0xFF472FE3, 0xFF6B1FD7, 0xFF931BAE, 0xFF9E1A5E, 0xFF993200, 0xFF7B4B00, 0xFF5B6700, 0xFF267A00, 0xFF008200, 0xFF007A3E, 0xFF006E8A, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF64A9FF, 0xFF8E89FF, 0xFFB676FF, 0xFFE06FFF, 0xFFEF6CC4, 0xFFF0806A, 0xFFD8982C, 0xFFB9B40A, 0xFF83CB0C, 0xFF5BD63F, 0xFF4AD17E, 0xFF4DC7CB, 0xFF4C4C4C, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFC7E5FF, 0xFFD9D9FF, 0xFFE9D1FF, 0xFFF9CEFF, 0xFFFFCCF1, 0xFFFFD4CB, 0xFFF8DFB1, 0xFFEDEAA4, 0xFFD6F4A4, 0xFFC5F8B8, 0xFFBEF6D3, 0xFFBFF1F1, 0xFFB9B9B9, 0xFF000000, 0xFF000000 });
		}

		private void mnuPaletteNesClassic_Click(object sender, EventArgs e)
		{
			UpdatePalette(new UInt32[] { 0xFF60615F, 0xFF000083, 0xFF1D0195, 0xFF340875, 0xFF51055E, 0xFF56000F, 0xFF4C0700, 0xFF372308, 0xFF203A0B, 0xFF0F4B0E, 0xFF194C16, 0xFF02421E, 0xFF023154, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFA9AAA8, 0xFF104BBF, 0xFF4712D8, 0xFF6300CA, 0xFF8800A9, 0xFF930B46, 0xFF8A2D04, 0xFF6F5206, 0xFF5C7114, 0xFF1B8D12, 0xFF199509, 0xFF178448, 0xFF206B8E, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFBFBFB, 0xFF6699F8, 0xFF8974F9, 0xFFAB58F8, 0xFFD557EF, 0xFFDE5FA9, 0xFFDC7F59, 0xFFC7A224, 0xFFA7BE03, 0xFF75D703, 0xFF60E34F, 0xFF3CD68D, 0xFF56C9CC, 0xFF414240, 0xFF000000, 0xFF000000, 0xFFFBFBFB, 0xFFBED4FA, 0xFFC9C7F9, 0xFFD7BEFA, 0xFFE8B8F9, 0xFFF5BAE5, 0xFFF3CAC2, 0xFFDFCDA7, 0xFFD9E09C, 0xFFC9EB9E, 0xFFC0EDB8, 0xFFB5F4C7, 0xFFB9EAE9, 0xFFABABAB, 0xFF000000, 0xFF000000 });
		}

		private void mnuPaletteOriginalHardware_Click(object sender, EventArgs e)
		{
			UpdatePalette(new UInt32[] { 0xFF6A6D6A, 0xFF00127D, 0xFF1E008A, 0xFF3B007D, 0xFF56005D, 0xFF5A0018, 0xFF4F0D00, 0xFF381E00, 0xFF203100, 0xFF003D00, 0xFF004000, 0xFF003B1E, 0xFF002E55, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFB9BCB9, 0xFF194EC8, 0xFF472FE3, 0xFF751FD7, 0xFF931EAD, 0xFF9E245E, 0xFF963800, 0xFF7B5000, 0xFF5B6700, 0xFF267A00, 0xFF007F00, 0xFF007842, 0xFF006E8A, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF69AEFF, 0xFF9798FF, 0xFFB687FF, 0xFFE278FF, 0xFFF279C7, 0xFFF58F6F, 0xFFDDA932, 0xFFBCB70D, 0xFF88D015, 0xFF60DB49, 0xFF4FD687, 0xFF50CACE, 0xFF515451, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFCCEAFF, 0xFFDEE2FF, 0xFFEEDAFF, 0xFFFAD7FD, 0xFFFDD7F6, 0xFFFDDCD0, 0xFFFAE8B6, 0xFFF2F1A9, 0xFFDBFBA9, 0xFFCAFFBD, 0xFFC3FBD8, 0xFFC4F6F6, 0xFFBEC1BE, 0xFF000000, 0xFF000000 });
		}

		private void mnuPalettePvmStyle_Click(object sender, EventArgs e)
		{
			UpdatePalette(new UInt32[] { 0xFF696964, 0xFF001774, 0xFF28007D, 0xFF3E006D, 0xFF560057, 0xFF5E0013, 0xFF531A00, 0xFF3B2400, 0xFF2A3000, 0xFF143A00, 0xFF003F00, 0xFF003B1E, 0xFF003050, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFB9B9B4, 0xFF1453B9, 0xFF4D2CDA, 0xFF7A1EC8, 0xFF98189C, 0xFF9D2344, 0xFFA03E00, 0xFF8D5500, 0xFF656D00, 0xFF2C7900, 0xFF008100, 0xFF007D42, 0xFF00788A, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF69A8FF, 0xFF9A96FF, 0xFFC28AFA, 0xFFEA7DFA, 0xFFF387B4, 0xFFF1986C, 0xFFE6B327, 0xFFD7C805, 0xFF90DF07, 0xFF64E53C, 0xFF45E27D, 0xFF48D5D9, 0xFF4B4B46, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFD2EAFF, 0xFFE2E2FF, 0xFFF2D8FF, 0xFFF8D2FF, 0xFFF8D9EA, 0xFFFADEB9, 0xFFF9E89B, 0xFFF3F28C, 0xFFD3FA91, 0xFFB8FCA8, 0xFFAEFACA, 0xFFCAF3F3, 0xFFBEBEB9, 0xFF000000, 0xFF000000 });
		}

		private void mnuPaletteSonyCxa2025As_Click(object sender, EventArgs e)
		{
			UpdatePalette(new UInt32[] { 0xFF585858, 0xFF00238C, 0xFF00139B, 0xFF2D0585, 0xFF5D0052, 0xFF7A0017, 0xFF7A0800, 0xFF5F1800, 0xFF352A00, 0xFF093900, 0xFF003F00, 0xFF003C22, 0xFF00325D, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFA1A1A1, 0xFF0053EE, 0xFF153CFE, 0xFF6028E4, 0xFFA91D98, 0xFFD41E41, 0xFFD22C00, 0xFFAA4400, 0xFF6C5E00, 0xFF2D7300, 0xFF007D06, 0xFF007852, 0xFF0069A9, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF1FA5FE, 0xFF5E89FE, 0xFFB572FE, 0xFFFE65F6, 0xFFFE6790, 0xFFFE773C, 0xFFFE9308, 0xFFC4B200, 0xFF79CA10, 0xFF3AD54A, 0xFF11D1A4, 0xFF06BFFE, 0xFF424242, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFA0D9FE, 0xFFBDCCFE, 0xFFE1C2FE, 0xFFFEBCFB, 0xFFFEBDD0, 0xFFFEC5A9, 0xFFFED18E, 0xFFE9DE86, 0xFFC7E992, 0xFFA8EEB0, 0xFF95ECD9, 0xFF91E4FE, 0xFFACACAC, 0xFF000000, 0xFF000000 });
		}

		private void contextPaletteList_Opening(object sender, CancelEventArgs e)
		{
			for(int i = contextPaletteList.Items.Count - 1; i >= 10; i--) {
				contextPaletteList.Items.RemoveAt(i);
			}
			
			if(ConfigManager.Config.VideoInfo.SavedPalettes.Count > 0) {
				contextPaletteList.Items.Add(new ToolStripSeparator());
				foreach(PaletteInfo info in ConfigManager.Config.VideoInfo.SavedPalettes) {
					ToolStripItem item = contextPaletteList.Items.Add(info.Name);
					item.Click += (object o, EventArgs args) => {
						_paletteData = ConfigManager.Config.VideoInfo.GetPalette(((ToolStripItem)o).Text);
						RefreshPalette();
					};
				}
			}
		}

		private void btnExportPalette_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.SetFilter("Palette Files (*.pal)|*.pal");
			if(sfd.ShowDialog() == DialogResult.OK) {
				List<byte>bytePalette = new List<byte>();
				foreach(int value in _paletteData) {
					bytePalette.Add((byte)(value >> 16 & 0xFF));
					bytePalette.Add((byte)(value >> 8 & 0xFF));
					bytePalette.Add((byte)(value & 0xFF));
				}
				File.WriteAllBytes(sfd.FileName, bytePalette.ToArray());
			}
		}

		private void cboAspectRatio_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateCustomRatioVisibility();
		}

		private void UpdateCustomRatioVisibility()
		{
			VideoAspectRatio ratio = cboAspectRatio.GetEnumValue<VideoAspectRatio>();
			lblCustomRatio.Visible = ratio == VideoAspectRatio.Custom;
			nudCustomRatio.Visible = ratio == VideoAspectRatio.Custom;
		}

		private void chkShowColorIndexes_CheckedChanged(object sender, EventArgs e)
		{
			this.RefreshPalette();
		}

		private void chkUseExclusiveFullscreen_CheckedChanged(object sender, EventArgs e)
		{
			flpRefreshRate.Visible = chkUseExclusiveFullscreen.Checked;
		}

		private void chkEnableGameSpecificOverscan_CheckedChanged(object sender, EventArgs e)
		{
			tpgOverscanGameSpecific.ImageIndex = chkEnableGameSpecificOverscan.Checked ? 0 : -1;
			nudGameSpecificOverscanBottom.Enabled = chkEnableGameSpecificOverscan.Checked;
			nudGameSpecificOverscanLeft.Enabled = chkEnableGameSpecificOverscan.Checked;
			nudGameSpecificOverscanRight.Enabled = chkEnableGameSpecificOverscan.Checked;
			nudGameSpecificOverscanTop.Enabled = chkEnableGameSpecificOverscan.Checked;
		}

		private void chkRemoveSpriteLimit_CheckedChanged(object sender, EventArgs e)
		{
			chkAdaptiveSpriteLimit.Enabled = chkRemoveSpriteLimit.Checked;
		}
	}
}
