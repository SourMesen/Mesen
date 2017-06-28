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
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlSpriteViewer : BaseControl
	{
		private byte[] _spriteRam;
		private byte[] _spritePixelData;
		private int _selectedSprite = -1;
		private bool _largeSprites;
		private int _spritePatternAddr;
		private bool _forceRefresh;

		public ctrlSpriteViewer()
		{
			InitializeComponent();
		}
		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);
		}

		public void GetData()
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			_largeSprites = state.PPU.ControlFlags.LargeSprites != 0;
			_spritePatternAddr = state.PPU.ControlFlags.SpritePatternAddr;

			_spriteRam = InteropEmu.DebugGetMemoryState(DebugMemoryType.SpriteMemory);
			_spritePixelData = InteropEmu.DebugGetSprites();
		}

		public void RefreshViewer()
		{
			_forceRefresh = true;

			GCHandle handle = GCHandle.Alloc(_spritePixelData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(64, 128, 4*64, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap target = new Bitmap(256, 512);

				using(Graphics g = Graphics.FromImage(target)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					
					g.ScaleTransform(4, 4);
					g.DrawImageUnscaled(source, 0, 0);
				}
				picSprites.Image = target;
			} finally {
				handle.Free();
			}

			CreateScreenPreview();
		}

		private void CreateScreenPreview()
		{
			GCHandle handle = GCHandle.Alloc(_spritePixelData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(64, 128, 4*64, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap screenPreview = new Bitmap(256, 240);

				using(Graphics g = Graphics.FromImage(screenPreview)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.FillRectangle(Brushes.Black, 0, 0, 256, 240);

					for(int i = 63; i >= 0; i--) {
						if(i != _selectedSprite) {
							DrawSprite(source, g, i);
						}
					}

					if(_selectedSprite >= 0) {
						DrawSprite(source, g, _selectedSprite);
					}
				}
				picPreview.Image = screenPreview;
			} finally {
				handle.Free();
			}
		}

		private void DrawSprite(Bitmap source, Graphics g, int i)
		{
			int spriteY = _spriteRam[i*4];
			int spriteX = _spriteRam[i*4+3];

			if(spriteY < 240) {
				g.DrawImage(source, new Rectangle(spriteX, spriteY, 8, _largeSprites ? 16 : 8), new Rectangle((i % 8) * 8, (i / 8) * 16, 8, _largeSprites ? 16 : 8), GraphicsUnit.Pixel);
			}

			if(_selectedSprite == i) {
				using(Pen pen = new Pen(Color.Red, 2)) {
					g.DrawRectangle(pen, new Rectangle(spriteX - 1, spriteY - 1, 10, _largeSprites ? 18 : 10));
				}
			}
		}

		private void picSprites_MouseMove(object sender, MouseEventArgs e)
		{
			int tileX = Math.Min(e.X / 32, 31);
			int tileY = Math.Min(e.Y / 64, 63);

			int ramAddr = ((tileY << 3) + tileX) << 2;

			if(ramAddr / 4 == _selectedSprite && !_forceRefresh) {
				return;
			}

			_forceRefresh = false;
			_selectedSprite = ramAddr / 4;

			int spriteY = _spriteRam[ramAddr];
			int tileIndex = _spriteRam[ramAddr + 1];
			int attributes = _spriteRam[ramAddr + 2];
			int spriteX = _spriteRam[ramAddr + 3];

			int tileAddr;
			if(_largeSprites) {
				tileAddr = ((tileIndex & 0x01) == 0x01 ? 0x1000 : 0x0000) + ((tileIndex & 0xFE) << 4);
			} else {
				tileAddr = _spritePatternAddr + (tileIndex << 4);
			}

			int paletteAddr = 0x3F10 + ((attributes & 0x03) << 2);
			bool verticalMirror = (attributes & 0x80) == 0x80;
			bool horizontalMirror = (attributes & 0x40) == 0x40;
			bool backgroundPriority = (attributes & 0x20) == 0x20;

			this.txtSpriteIndex.Text = _selectedSprite.ToString("X2");
			this.txtTileIndex.Text = tileIndex.ToString("X2");
			this.txtTileAddress.Text = tileAddr.ToString("X4");
			this.txtPosition.Text = spriteX.ToString() + ", " + spriteY.ToString();
			this.txtPaletteAddress.Text = paletteAddr.ToString("X4");
			this.chkVerticalMirroring.Checked = verticalMirror;
			this.chkHorizontalMirroring.Checked = horizontalMirror;
			this.chkBackgroundPriority.Checked = backgroundPriority;

			Bitmap tile = new Bitmap(64, 128);
			Bitmap tilePreview = new Bitmap(8, 16);
			using(Graphics g = Graphics.FromImage(tilePreview)) {
				g.DrawImage(((PictureBox)sender).Image, new Rectangle(0, 0, 8, 16), new Rectangle(tileX*32, tileY*64, 32, 64), GraphicsUnit.Pixel);
			}			
			using(Graphics g = Graphics.FromImage(tile)) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.ScaleTransform(8, 8);
				g.DrawImageUnscaled(tilePreview, 0, 0);
			}
			this.picTile.Image = tile;

			this.CreateScreenPreview();
		}

		private void picSprites_MouseLeave(object sender, EventArgs e)
		{
			this._selectedSprite = -1;
			this.CreateScreenPreview();
		}

		string _copyData;
		private void mnuCopyHdPack_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(_copyData);
		}

		private void ctxMenu_Opening(object sender, CancelEventArgs e)
		{
			int ramAddr = _selectedSprite * 4;
			int spriteY = _spriteRam[ramAddr];
			int tileIndex = _spriteRam[ramAddr + 1];
			int palette = (_spriteRam[ramAddr + 2] & 0x03) + 4;
			int spriteX = _spriteRam[ramAddr + 3];

			int tileAddr;
			if(_largeSprites) {
				tileAddr = ((tileIndex & 0x01) == 0x01 ? 0x1000 : 0x0000) + ((tileIndex & 0xFE) << 4);
			} else {
				tileAddr = _spritePatternAddr + (tileIndex << 4);
			}

			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < 16; i++) {
				sb.Append(InteropEmu.DebugGetMemoryValue(DebugMemoryType.PpuMemory, (UInt32)(tileAddr + i)).ToString("X2"));
			}
			sb.Append(",");
			for(int i = 1; i < 4; i++) {
				sb.Append(InteropEmu.DebugGetMemoryValue(DebugMemoryType.PaletteMemory, (uint)(palette * 4 + i)).ToString("X2"));
			}

			_copyData = sb.ToString();
			_copyData = _copyData.Substring(0, _copyData.Length - 1);
		}
	}
}
