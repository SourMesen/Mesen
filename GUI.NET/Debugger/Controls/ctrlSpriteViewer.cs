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
	public partial class ctrlSpriteViewer : UserControl
	{
		private byte[] _spriteRam;

		public ctrlSpriteViewer()
		{
			InitializeComponent();
		}
		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);
		}

		public void RefreshViewer()
		{
			_spriteRam = InteropEmu.DebugGetMemoryState(DebugMemoryType.SpriteMemory);

			byte[] pixelData = InteropEmu.DebugGetSprites();

			GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(64, 128, 4*64, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap target = new Bitmap(256, 512);

				using(Graphics g = Graphics.FromImage(target)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.DrawImage(source, new Rectangle(0, 0, 256, 512), new Rectangle(0, 0, 64, 128), GraphicsUnit.Pixel);
				}
				picSprites.Image = target;
			} finally {
				handle.Free();
			}
		}
		
		private void picSprites_MouseMove(object sender, MouseEventArgs e)
		{
			int tileX = Math.Min(e.X / 32, 31);
			int tileY = Math.Min(e.Y / 64, 63);

			int ramAddr = ((tileY << 3) + tileX) << 2;
			int spriteY = _spriteRam[ramAddr];
			int tileIndex = _spriteRam[ramAddr + 1];
			int attributes = _spriteRam[ramAddr + 2];
			int spriteX = _spriteRam[ramAddr + 3];

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			int tileAddr;
			if(state.PPU.ControlFlags.LargeSprites != 0) {
				tileAddr = ((tileIndex & 0x01) == 0x01 ? 0x1000 : 0x0000) + ((tileIndex & 0xFE) << 4);
			} else {
				tileAddr = state.PPU.ControlFlags.SpritePatternAddr + (tileIndex << 4);
			}

			int paletteAddr = 0x3F10 + ((attributes & 0x03) << 2);
			bool verticalMirror = (attributes & 0x80) == 0x80;
			bool horizontalMirror = (attributes & 0x40) == 0x40;
			bool backgroundPriority = (attributes & 0x20) == 0x20;

			this.txtTileIndex.Text = tileIndex.ToString("X2");
			this.txtTileAddress.Text = tileAddr.ToString("X4");
			this.txtPosition.Text = spriteX.ToString() + ", " + spriteY.ToString();
			this.txtPaletteAddress.Text = paletteAddr.ToString("X4");
			this.chkVerticalMirroring.Checked = verticalMirror;
			this.chkHorizontalMirroring.Checked = horizontalMirror;
			this.chkBackgroundPriority.Checked = backgroundPriority;

			Bitmap tile = new Bitmap(64, 128);
			using(Graphics g = Graphics.FromImage(tile)) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.DrawImage(((PictureBox)sender).Image, new Rectangle(0, 0, 64, 128), new Rectangle(tileX*32, tileY*64, 32, 64), GraphicsUnit.Pixel);
			}
			this.picTile.Image = tile;
		}
	}
}
