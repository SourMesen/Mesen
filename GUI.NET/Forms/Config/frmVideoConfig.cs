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
			
			AddBinding("EmulationSpeed", nudEmulationSpeed);
			AddBinding("ShowFPS", chkShowFps);
			AddBinding("VerticalSync", chkVerticalSync);
			AddBinding("UseHdPacks", chkUseHdPacks);
			
			AddBinding("VideoScale", cboScale);
			AddBinding("AspectRatio", cboAspectRatio);
			AddBinding("VideoFilter", cboFilter);

			AddBinding("OverscanLeft", nudOverscanLeft);
			AddBinding("OverscanRight", nudOverscanRight);
			AddBinding("OverscanTop", nudOverscanTop);
			AddBinding("OverscanBottom", nudOverscanBottom);

			_paletteData = InteropEmu.GetRgbPalette();
			RefreshPalette();

			toolTip.SetToolTip(picHdNesTooltip, "This option allows Mesen to load HDNes-format HD packs if they are found." + Environment.NewLine + Environment.NewLine + "HD Packs should be placed in the \"HdPacks\" folder in a subfolder matching the name of the ROM." + Environment.NewLine + "e.g: MyRom.nes should have their HD Pack in \"HdPacks\\MyRom\\hires.txt\"." + Environment.NewLine + Environment.NewLine + "Note: Support for HD Packs is a work in progress and some limitations remain.");
		}
		
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(DialogResult == System.Windows.Forms.DialogResult.OK) {
				((VideoInfo)Entity).Palette = _paletteData;
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

		private void btnResetPalette_Click(object sender, EventArgs e)
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
					}
					paletteFile.Close();
				}
			}
		}
	}
}
