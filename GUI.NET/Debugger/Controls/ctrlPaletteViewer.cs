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
	public partial class ctrlPaletteViewer : UserControl
	{
		private byte[] _paletteRam;

		public ctrlPaletteViewer()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);
		}

		public void RefreshViewer()
		{
			this._paletteRam = InteropEmu.DebugGetMemoryState(DebugMemoryType.PaletteMemory);
			byte[] pixelData = InteropEmu.DebugGetPalette();

			GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(4, 8, 4*4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap target = new Bitmap(128, 256);

				using(Graphics g = Graphics.FromImage(target)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.DrawImage(source, new Rectangle(0, 0, 128, 256), new Rectangle(0, 0, 4, 8), GraphicsUnit.Pixel);
				}
				this.picPalette.Image = target;
			} finally {
				handle.Free();
			}
		}

		private void picPalette_MouseMove(object sender, MouseEventArgs e)
		{
			int tileX = Math.Min(e.X / 32, 31);
			int tileY = Math.Min(e.Y / 32, 31);
			int tileIndex = tileY * 4 + tileX;

			this.txtColor.Text = _paletteRam[tileIndex].ToString("X2");
			this.txtPaletteAddress.Text = (0x3F00 + tileIndex).ToString("X4");

			Bitmap tile = new Bitmap(64, 64);
			using(Graphics g = Graphics.FromImage(tile)) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.DrawImage(picPalette.Image, new Rectangle(0, 0, 64, 64), new Rectangle(tileX*32, tileY*32, 32, 32), GraphicsUnit.Pixel);
			}
			this.picColor.Image = tile;
		}
	}
}
