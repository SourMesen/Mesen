using System;
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

namespace Mesen.GUI.Forms.Config
{
	public partial class frmVideoConfig : BaseConfigForm
	{
		private Int32[] _paletteData;

		public frmVideoConfig()
		{
			InitializeComponent();

			Entity = ConfigManager.Config.VideoInfo;
			
			AddBinding("ShowFPS", chkShowFps);
			AddBinding("UseBilinearInterpolation", chkBilinearInterpolation);
			AddBinding("VerticalSync", chkVerticalSync);
			AddBinding("UseHdPacks", chkUseHdPacks);
			
			AddBinding("VideoScale", nudScale);
			AddBinding("AspectRatio", cboAspectRatio);
			AddBinding("VideoFilter", cboFilter);

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

			AddBinding("DisableBackground", chkDisableBackground);
			AddBinding("DisableSprites", chkDisableSprites);
			AddBinding("ForceBackgroundFirstColumn", chkForceBackgroundFirstColumn);
			AddBinding("ForceSpritesFirstColumn", chkForceSpritesFirstColumn);

			_paletteData = InteropEmu.GetRgbPalette();
			RefreshPalette();

			toolTip.SetToolTip(picHdNesTooltip, ResourceHelper.GetMessage("HDNesTooltip"));

			UpdateOverscanImage();

			ResourceHelper.ApplyResources(this, contextPaletteList);
			ResourceHelper.ApplyResources(this, contextPicturePresets);
		}

		private void UpdatePalette()
		{
			byte[] result = new byte[_paletteData.Length * sizeof(int)];
			Buffer.BlockCopy(_paletteData, 0, result, 0, result.Length);
			((VideoInfo)Entity).PaletteData = System.Convert.ToBase64String(result);
		}

		protected override bool ValidateInput()
		{
			UpdateObject();
			UpdatePalette();
			VideoInfo.ApplyConfig();
			return true;
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(DialogResult == System.Windows.Forms.DialogResult.OK) {
				UpdatePalette();
			}
			base.OnFormClosed(e);
			VideoInfo.ApplyConfig();
		}

		private void RefreshPalette()
		{
			GCHandle handle = GCHandle.Alloc(_paletteData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(16, 4, 16*4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap target = new Bitmap(384, 96);

				using(Graphics g = Graphics.FromImage(target)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.DrawImage(source, new Rectangle(0, 0, 384, 96), new Rectangle(0, 0, 16, 4), GraphicsUnit.Pixel);
				}
				this.picPalette.Image = target;
			} finally {
				handle.Free();
			}
		}

		private void picPalette_MouseDown(object sender, MouseEventArgs e)
		{
			int offset = (e.X / 24) + (e.Y / 24 * 16);

			colorDialog.SolidColorOnly = true;
			colorDialog.AllowFullOpen = true;
			colorDialog.FullOpen = true;
			colorDialog.Color = Color.FromArgb(_paletteData[offset]);
			if(colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				_paletteData[offset] = colorDialog.Color.ToArgb();
			}

			RefreshPalette();
		}

		private void btnLoadPalFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Palette Files (*.pal)|*.pal|All Files (*.*)|*.*";
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				using(FileStream paletteFile = File.OpenRead(ofd.FileName)) {
					byte[] paletteFileData = new byte[64*3];
					if(paletteFile.Read(paletteFileData, 0, 64*3) == 64*3) {
						for(int i = 0; i < 64; i++) {
							int fileOffset = i * 3;
							_paletteData[i] = (Int32)((UInt32)0xFF000000 | (UInt32)paletteFileData[fileOffset+2] | (UInt32)(paletteFileData[fileOffset+1] << 8) | (UInt32)(paletteFileData[fileOffset] << 16));
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

		private void UpdateOverscanImage()
		{
			Bitmap overscan = new Bitmap(256, 240);

			using(Graphics g = Graphics.FromImage(overscan)) {
				Rectangle bg = new Rectangle(0, 0, 256, 240);
				g.FillRectangle(Brushes.DarkGray, bg);

				Rectangle fg = new Rectangle((int)nudOverscanLeft.Value, (int)nudOverscanTop.Value, 256 - (int)nudOverscanLeft.Value - (int)nudOverscanRight.Value, 240 - (int)nudOverscanTop.Value - (int)nudOverscanBottom.Value);
				g.FillRectangle(Brushes.LightCyan, fg);
			}
			picOverscan.Image = overscan;
		}

		private void nudOverscan_ValueChanged(object sender, EventArgs e)
		{
			UpdateOverscanImage();
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

		private void mnuDefaultPalette_Click(object sender, EventArgs e)
		{
			_paletteData = (Int32[])((object)(new UInt32[] {
				0xFF666666, 0xFF002A88, 0xFF1412A7, 0xFF3B00A4, 0xFF5C007E,
				0xFF6E0040, 0xFF6C0600, 0xFF561D00, 0xFF333500, 0xFF0B4800,
				0xFF005200, 0xFF004F08, 0xFF00404D, 0xFF000000, 0xFF000000,
				0xFF000000, 0xFFADADAD, 0xFF155FD9, 0xFF4240FF, 0xFF7527FE,
				0xFFA01ACC, 0xFFB71E7B, 0xFFB53120, 0xFF994E00, 0xFF6B6D00,
				0xFF388700, 0xFF0C9300, 0xFF008F32, 0xFF007C8D, 0xFF000000,
				0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFF64B0FF, 0xFF9290FF,
				0xFFC676FF, 0xFFF36AFF, 0xFFFE6ECC, 0xFFFE8170, 0xFFEA9E22,
				0xFFBCBE00, 0xFF88D800, 0xFF5CE430, 0xFF45E082, 0xFF48CDDE,
				0xFF4F4F4F, 0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFFC0DFFF,
				0xFFD3D2FF, 0xFFE8C8FF, 0xFFFBC2FF, 0xFFFEC4EA, 0xFFFECCC5,
				0xFFF7D8A5, 0xFFE4E594, 0xFFCFEF96, 0xFFBDF4AB, 0xFFB3F3CC,
				0xFFB5EBF2, 0xFFB8B8B8, 0xFF000000, 0xFF000000
			}));

			RefreshPalette();
		}

		private void mnuPaletteUnsaturated_Click(object sender, EventArgs e)
		{
			//Unsaturated-V5
			_paletteData = (Int32[])((object)(new UInt32[] {
				0XFF6B6B6B, 0XFF001E87, 0XFF1F0B96, 0XFF3B0C87, 0XFF590D61,
				0XFF5E0528, 0XFF551100, 0XFF461B00, 0XFF303200, 0XFF0A4800,
				0XFF004E00, 0XFF004619, 0XFF00395A, 0XFF000000, 0XFF000000,
				0XFF000000, 0XFFB2B2B2, 0XFF1A53D1, 0XFF4835EE, 0XFF7123EC,
				0XFF9A1EB7, 0XFFA51E62, 0XFFA52D19, 0XFF874B00, 0XFF676900,
				0XFF298400, 0XFF038B00, 0XFF008240, 0XFF007096, 0XFF000000,
				0XFF000000, 0XFF000000, 0XFFFFFFFF, 0XFF63ADFD, 0XFF908AFE,
				0XFFB977FC, 0XFFE771FE, 0XFFF76FC9, 0XFFF5836A, 0XFFDD9C29,
				0XFFBDB807, 0XFF84D107, 0XFF5BDC3B, 0XFF48D77D, 0XFF48C6D8,
				0XFF555555, 0XFF000000, 0XFF000000, 0XFFFFFFFF, 0XFFC4E3FE,
				0XFFD7D5FE, 0XFFE6CDFE, 0XFFF9CAFE, 0XFFFEC9F0, 0XFFFED1C7,
				0XFFF7DCAC, 0XFFE8E89C, 0XFFD1F29D, 0XFFBFF4B1, 0XFFB7F5CD,
				0XFFB7EBF2, 0XFFBEBEBE, 0XFF000000, 0XFF000000
			}));

			RefreshPalette();
		}

		private void mnuPaletteYuv_Click(object sender, EventArgs e)
		{
			//YUV V3
			_paletteData = (Int32[])((object)(new UInt32[] {
				0XFF666666, 0XFF002A88, 0XFF1412A7, 0XFF3B00A4, 0XFF5C007E,
				0XFF6E0040, 0XFF6C0700, 0XFF561D00, 0XFF333500, 0XFF0C4800,
				0XFF005200, 0XFF004C18, 0XFF003E5B, 0XFF000000, 0XFF000000,
				0XFF000000, 0XFFADADAD, 0XFF155FD9, 0XFF4240FF, 0XFF7527FE,
				0XFFA01ACC, 0XFFB71E7B, 0XFFB53120, 0XFF994E00, 0XFF6B6D00,
				0XFF388700, 0XFF0D9300, 0XFF008C47, 0XFF007AA0, 0XFF000000,
				0XFF000000, 0XFF000000, 0XFFFFFFFF, 0XFF64B0FF, 0XFF9290FF,
				0XFFC676FF, 0XFFF26AFF, 0XFFFF6ECC, 0XFFFF8170, 0XFFEA9E22,
				0XFFBCBE00, 0XFF88D800, 0XFF5CE430, 0XFF45E082, 0XFF48CDDE,
				0XFF4F4F4F, 0XFF000000, 0XFF000000, 0XFFFFFFFF, 0XFFC0DFFF,
				0XFFD3D2FF, 0XFFE8C8FF, 0XFFFAC2FF, 0XFFFFC4EA, 0XFFFFCCC5,
				0XFFF7D8A5, 0XFFE4E594, 0XFFCFEF96, 0XFFBDF4AB, 0XFFB3F3CC,
				0XFFB5EBF2, 0XFFB8B8B8, 0XFF000000, 0XFF000000
			}));

			RefreshPalette();
		}

		private void mnuPaletteNestopiaRgb_Click(object sender, EventArgs e)
		{
			//Nestopia RGB
			_paletteData = (Int32[])((object)(new UInt32[] {
				0XFF6D6D6D, 0XFF002492, 0XFF0000DB, 0XFF6D49DB, 0XFF92006D,
				0XFFB6006D, 0XFFB62400, 0XFF924900, 0XFF6D4900, 0XFF244900,
				0XFF006D24, 0XFF009200, 0XFF004949, 0XFF000000, 0XFF000000,
				0XFF000000, 0XFFB6B6B6, 0XFF006DDB, 0XFF0049FF, 0XFF9200FF,
				0XFFB600FF, 0XFFFF0092, 0XFFFF0000, 0XFFDB6D00, 0XFF926D00,
				0XFF249200, 0XFF009200, 0XFF00B66D, 0XFF009292, 0XFF242424,
				0XFF000000, 0XFF000000, 0XFFFFFFFF, 0XFF6DB6FF, 0XFF9292FF,
				0XFFDB6DFF, 0XFFFF00FF, 0XFFFF6DFF, 0XFFFF9200, 0XFFFFB600,
				0XFFDBDB00, 0XFF6DDB00, 0XFF00FF00, 0XFF49FFDB, 0XFF00FFFF,
				0XFF494949, 0XFF000000, 0XFF000000, 0XFFFFFFFF, 0XFFB6DBFF,
				0XFFDBB6FF, 0XFFFFB6FF, 0XFFFF92FF, 0XFFFFB6B6, 0XFFFFDB92,
				0XFFFFFF49, 0XFFFFFF6D, 0XFFB6FF49, 0XFF92FF6D, 0XFF49FFDB,
				0XFF92DBFF, 0XFF929292, 0XFF000000, 0XFF000000,
			}));

			RefreshPalette();
		}

		private void contextPaletteList_Opening(object sender, CancelEventArgs e)
		{
			for(int i = contextPaletteList.Items.Count - 1; i >= 5; i--) {
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
			sfd.Filter = "Palette Files (*.pal)|*.pal";
			if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				List<byte>bytePalette = new List<byte>();
				foreach(int value in _paletteData) {
					bytePalette.Add((byte)(value >> 16 & 0xFF));
					bytePalette.Add((byte)(value >> 8 & 0xFF));
					bytePalette.Add((byte)(value & 0xFF));
				}
				File.WriteAllBytes(sfd.FileName, bytePalette.ToArray());
			}
		}
	}
}
