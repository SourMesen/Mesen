using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlNametableViewer : UserControl
	{
		private List<byte[]> _tileData = new List<byte[]>() { null, null, null, null };
		private List<byte[]> _attributeData = new List<byte[]>() { null, null, null, null };

		public ctrlNametableViewer()
		{
			InitializeComponent();
		}

		public void RefreshViewer()
		{
			PictureBox[] nametables = new PictureBox[] { this.picNametable1, this.picNametable2, this.picNametable3, this.picNametable4 };

			for(int i = 0; i < 4; i++) {
				byte[] nametablePixelData, tileData, attributeData;
				InteropEmu.DebugGetNametable(i, out nametablePixelData, out tileData, out attributeData);
				_tileData[i] = tileData;
				_attributeData[i] = attributeData;

				GCHandle handle = GCHandle.Alloc(nametablePixelData, GCHandleType.Pinned);
				try {
					Bitmap source = new Bitmap(256, 240, 4*256, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
					Bitmap target = new Bitmap(256, 240);
					using(Graphics g = Graphics.FromImage(target)) {
						g.DrawImage(source, new Rectangle(0, 0, 256, 240), new Rectangle(0, 0, 256, 240), GraphicsUnit.Pixel);
					}
					nametables[i].Image = target;
				} finally {
					handle.Free();
				}
			}
		}

		private void picNametable_MouseMove(object sender, MouseEventArgs e)
		{
			List<PictureBox> nametables = new List<PictureBox>() { this.picNametable1, this.picNametable2, this.picNametable3, this.picNametable4 };
			int nametableIndex = nametables.IndexOf((PictureBox)sender);

			int baseAddress = 0x2000 + nametableIndex * 0x400;

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			int bgAddr = state.PPU.ControlFlags.BackgroundPatternAddr;

			int tileX = Math.Min(e.X / 8, 31);
			int tileY = Math.Min(e.Y / 8, 29);
			int shift = (tileX & 0x02) | ((tileY & 0x02) << 1);

			int tileIndex = _tileData[nametableIndex][tileY*32+tileX];
			int attributeData = _attributeData[nametableIndex][tileY*32+tileX];
			int attributeAddr = baseAddress + 960 + ((tileY & 0xFC) << 1) + (tileX >> 2);
			int paletteBaseAddr = ((attributeData >> shift) & 0x03) << 2;

			this.txtTileIndex.Text = tileIndex.ToString("X2");
			this.txtTileAddress.Text = (bgAddr + tileIndex * 16).ToString("X4");
			this.txtAttributeData.Text = attributeData.ToString("X2");
			this.txtAttributeAddress.Text = attributeAddr.ToString("X4");
			this.txtPaletteAddress.Text = (0x3F00 + paletteBaseAddr).ToString("X4");

			Bitmap tile = new Bitmap(64, 64);
			using(Graphics g = Graphics.FromImage(tile)) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.DrawImage(((PictureBox)sender).Image, new Rectangle(0, 0, 64, 64), new Rectangle(tileX*8, tileY*8, 8, 8), GraphicsUnit.Pixel);
			}
			this.picTile.Image = tile;
		}

	}
}
