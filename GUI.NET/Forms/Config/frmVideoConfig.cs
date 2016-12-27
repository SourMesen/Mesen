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

			AddBinding("NtscYFilterLength", trkYFilterLength);
			AddBinding("NtscIFilterLength", trkIFilterLength);
			AddBinding("NtscQFilterLength", trkQFilterLength);

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
			VideoFilterType filter = ((VideoInfo)Entity).VideoFilter;
			if(filter == VideoFilterType.NTSC) {
				tlpNtscFilter1.Visible = true;
				tlpNtscFilter2.Visible = false;
				grpNtscFilter.Visible = true;
			} else if(filter == VideoFilterType.BisqwitNtsc || filter == VideoFilterType.BisqwitNtscHalfRes || filter == VideoFilterType.BisqwitNtscQuarterRes) {
				tlpNtscFilter1.Visible = false;
				tlpNtscFilter2.Visible = true;
				grpNtscFilter.Visible = true;
			} else {
				grpNtscFilter.Visible = false;
			}
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
					g.ScaleTransform(22, 22);
					g.DrawImageUnscaled(source, 0, 0);
				}
				this.picPalette.Image = target;
			} finally {
				handle.Free();
			}
		}

		private void picPalette_MouseDown(object sender, MouseEventArgs e)
		{
			int offset = (e.X / 22) + (e.Y / 22 * 16);

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
			ofd.SetFilter("Palette Files (*.pal)|*.pal|All Files (*.*)|*.*");
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
				0xFF6B6B6B, 0xFF001E87, 0xFF1F0B96, 0xFF3B0C87, 0xFF590D61,
				0xFF5E0528, 0xFF551100, 0xFF461B00, 0xFF303200, 0xFF0A4800,
				0xFF004E00, 0xFF004619, 0xFF00395A, 0xFF000000, 0xFF000000,
				0xFF000000, 0xFFB2B2B2, 0xFF1A53D1, 0xFF4835EE, 0xFF7123EC,
				0xFF9A1EB7, 0xFFA51E62, 0xFFA52D19, 0xFF874B00, 0xFF676900,
				0xFF298400, 0xFF038B00, 0xFF008240, 0xFF007096, 0xFF000000,
				0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF63ADFD, 0xFF908AFE,
				0xFFB977FC, 0xFFE771FE, 0xFFF76FC9, 0xFFF5836A, 0xFFDD9C29,
				0xFFBDB807, 0xFF84D107, 0xFF5BDC3B, 0xFF48D77D, 0xFF48C6D8,
				0xFF555555, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFC4E3FE,
				0xFFD7D5FE, 0xFFE6CDFE, 0xFFF9CAFE, 0xFFFEC9F0, 0xFFFED1C7,
				0xFFF7DCAC, 0xFFE8E89C, 0xFFD1F29D, 0xFFBFF4B1, 0xFFB7F5CD,
				0xFFB7EBF2, 0xFFBEBEBE, 0xFF000000, 0xFF000000
			}));

			RefreshPalette();
		}

		private void mnuPaletteYuv_Click(object sender, EventArgs e)
		{
			//YUV V3
			_paletteData = (Int32[])((object)(new UInt32[] {
				0xFF666666, 0xFF002A88, 0xFF1412A7, 0xFF3B00A4, 0xFF5C007E,
				0xFF6E0040, 0xFF6C0700, 0xFF561D00, 0xFF333500, 0xFF0C4800,
				0xFF005200, 0xFF004C18, 0xFF003E5B, 0xFF000000, 0xFF000000,
				0xFF000000, 0xFFADADAD, 0xFF155FD9, 0xFF4240FF, 0xFF7527FE,
				0xFFA01ACC, 0xFFB71E7B, 0xFFB53120, 0xFF994E00, 0xFF6B6D00,
				0xFF388700, 0xFF0D9300, 0xFF008C47, 0xFF007AA0, 0xFF000000,
				0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF64B0FF, 0xFF9290FF,
				0xFFC676FF, 0xFFF26AFF, 0xFFFF6ECC, 0xFFFF8170, 0xFFEA9E22,
				0xFFBCBE00, 0xFF88D800, 0xFF5CE430, 0xFF45E082, 0xFF48CDDE,
				0xFF4F4F4F, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFC0DFFF,
				0xFFD3D2FF, 0xFFE8C8FF, 0xFFFAC2FF, 0xFFFFC4EA, 0xFFFFCCC5,
				0xFFF7D8A5, 0xFFE4E594, 0xFFCFEF96, 0xFFBDF4AB, 0xFFB3F3CC,
				0xFFB5EBF2, 0xFFB8B8B8, 0xFF000000, 0xFF000000
			}));

			RefreshPalette();
		}

		private void mnuPaletteNestopiaRgb_Click(object sender, EventArgs e)
		{
			//Nestopia RGB
			_paletteData = (Int32[])((object)(new UInt32[] {
				0xFF6D6D6D, 0xFF002492, 0xFF0000DB, 0xFF6D49DB, 0xFF92006D,
				0xFFB6006D, 0xFFB62400, 0xFF924900, 0xFF6D4900, 0xFF244900,
				0xFF006D24, 0xFF009200, 0xFF004949, 0xFF000000, 0xFF000000,
				0xFF000000, 0xFFB6B6B6, 0xFF006DDB, 0xFF0049FF, 0xFF9200FF,
				0xFFB600FF, 0xFFFF0092, 0xFFFF0000, 0xFFDB6D00, 0xFF926D00,
				0xFF249200, 0xFF009200, 0xFF00B66D, 0xFF009292, 0xFF242424,
				0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF6DB6FF, 0xFF9292FF,
				0xFFDB6DFF, 0xFFFF00FF, 0xFFFF6DFF, 0xFFFF9200, 0xFFFFB600,
				0xFFDBDB00, 0xFF6DDB00, 0xFF00FF00, 0xFF49FFDB, 0xFF00FFFF,
				0xFF494949, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFB6DBFF,
				0xFFDBB6FF, 0xFFFFB6FF, 0xFFFF92FF, 0xFFFFB6B6, 0xFFFFDB92,
				0xFFFFFF49, 0xFFFFFF6D, 0xFFB6FF49, 0xFF92FF6D, 0xFF49FFDB,
				0xFF92DBFF, 0xFF929292, 0xFF000000, 0xFF000000,
			}));

			RefreshPalette();
		}


		private void mnuPaletteNostalgia_Click(object sender, EventArgs e)
		{
			_paletteData = (Int32[])((object)(new UInt32[] {
				0xFF656565, 0xFF00127D, 0xFF18008E, 0xFF360082,	0xFF56005D,
				0xFF5A0018, 0xFF4F0500, 0xFF381900, 0xFF1D3100, 0xFF003D00,
				0xFF004100, 0xFF003B17, 0xFF002E55, 0xFF000000, 0xFF000000,
				0xFF000000, 0xFFAFAFAF, 0xFF194EC8, 0xFF472FE3, 0xFF6B1FD7,
				0xFF931BAE, 0xFF9E1A5E, 0xFF973200, 0xFF7B4B00, 0xFF5B6700,
				0xFF267A00, 0xFF008200, 0xFF007A3E, 0xFF006E8A, 0xFF000000,
				0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF64A9FF, 0xFF8E89FF,
				0xFFB676FF, 0xFFE06FFF, 0xFFEF6CC4, 0xFFF0806A, 0xFFD8982C,
				0xFFB9B40A, 0xFF83CB0C, 0xFF5BD63F, 0xFF4AD17E, 0xFF4DC7CB,
				0xFF4C4C4C, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFC7E5FF,
				0xFFD9D9FF, 0xFFE9D1FF, 0xFFF9CEFF, 0xFFFFCCF1, 0xFFFFD4CB,
				0xFFF8DFB1,	0xFFEDEAA4, 0xFFD6F4A4, 0xFFC5F8B8, 0xFFBEF6D3,
				0xFFBFF1F1, 0xFFB9B9B9, 0xFF000000, 0xFF000000
			}));
					
			RefreshPalette();
		}

		private void mnuPaletteNesClassic_Click(object sender, EventArgs e)
		{
			_paletteData = (Int32[])((object)(new UInt32[] {
				0xFF606060, 0xFF000083, 0xFF1F069E, 0xFF380F7C, 0xFF560C62,
				0xFF5B0010, 0xFF530C00, 0xFF3A2308, 0xFF20350B, 0xFF0C410B,
				0xFF194516, 0xFF023E1E,	0xFF023154, 0xFF000000, 0xFF000000,
				0xFF000000,	0xFFA9A9A9, 0xFF104BBF, 0xFF4A1EE4, 0xFF690AD2,
				0xFF8E12B2, 0xFF9E0F4C, 0xFF8F3204, 0xFF735106, 0xFF5C6A12,
				0xFF187D10, 0xFF148109, 0xFF117547, 0xFF1D668F, 0xFF000000,
				0xFF000000, 0xFF000000,	0xFFFBFBFB, 0xFF6699F8, 0xFF8978FE,
				0xFFB262FF,	0xFFDE63FF, 0xFFEB69B3, 0xFFE38758, 0xFFC89F22,
				0xFFA7B103, 0xFF73C203, 0xFF5DD04F, 0xFF36C58D,	0xFF50C5CC,
				0xFF404040, 0xFF000000, 0xFF000000,	0xFFFBFBFB, 0xFFBFD4FA,
				0xFFCDCBFE, 0xFFD9C2FF,	0xFFECBEFF, 0xFFFAC2EB, 0xFFF7CAC3,
				0xFFE3CDA7,	0xFFD9DE9C, 0xFFC8E69E, 0xFFC0E6B8, 0xFFB5EDC7,
				0xFFB9E6EA, 0xFFB8B8B8, 0xFF000000, 0xFF000000
			}));

			RefreshPalette();
		}

		private void contextPaletteList_Opening(object sender, CancelEventArgs e)
		{
			for(int i = contextPaletteList.Items.Count - 1; i >= 7; i--) {
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
