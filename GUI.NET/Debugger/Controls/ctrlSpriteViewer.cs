﻿using System;
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
using System.Drawing.Imaging;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlSpriteViewer : BaseControl
	{
		public delegate void SelectTilePaletteHandler(int tileIndex, int paletteIndex);
		public event SelectTilePaletteHandler OnSelectTilePalette;
		private byte[] _spriteRam;
		private byte[] _spritePixelData;
		private int _selectedSprite = -1;
		private bool _largeSprites;
		private int _spritePatternAddr;
		private bool _forceRefresh;
		private Point? _previewMousePosition = null;
		private int _contextMenuSpriteIndex = -1;
		private bool _copyPreview = false;
		private Bitmap _imgSprites;
		private Bitmap _screenPreview = new Bitmap(256, 240, PixelFormat.Format32bppArgb);
		private HdPackCopyHelper _hdCopyHelper = new HdPackCopyHelper();
		private bool _firstDraw = true;

		public ctrlSpriteViewer()
		{
			InitializeComponent();

			picPreview.Image = new Bitmap(256, 240, PixelFormat.Format32bppArgb);
			picSprites.Image = new Bitmap(256, 512, PixelFormat.Format32bppArgb);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(ctxMenu.ProcessCommandKey(ref msg, keyData)) {
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(!IsDesignMode) {
				mnuCopyToClipboard.InitShortcut(this, nameof(DebuggerShortcutsConfig.Copy));
				mnuEditInMemoryViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.CodeWindow_EditInMemoryViewer));
			}
		}

		public void GetData()
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			_largeSprites = state.PPU.ControlFlags.LargeSprites != 0;
			_spritePatternAddr = state.PPU.ControlFlags.SpritePatternAddr;

			_spriteRam = InteropEmu.DebugGetMemoryState(DebugMemoryType.SpriteMemory);
			_spritePixelData = InteropEmu.DebugGetSprites();

			_hdCopyHelper.RefreshData();
		}

		public void RefreshViewer()
		{
			_forceRefresh = true;

			GCHandle handle = GCHandle.Alloc(_spritePixelData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(64, 128, 4*64, PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());

				Bitmap sprites = new Bitmap(64, 128, PixelFormat.Format32bppArgb);
				using(Graphics g = Graphics.FromImage(sprites)) {
					g.DrawImage(source, 0, 0);
				}
				_imgSprites = sprites;

				using(Graphics g = Graphics.FromImage(picSprites.Image)) {
					g.Clear(Color.FromArgb(64, 64, 64));
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					
					g.ScaleTransform(4, 4);
					g.DrawImageUnscaled(sprites, 0, 0);
				}
				picSprites.Invalidate();
			} finally {
				handle.Free();
			}

			if(_previewMousePosition.HasValue) {
				SelectSpriteUnderCursor();
			}
			CreateScreenPreview();

			if(_firstDraw) {
				//Update the UI with the first sprite when showing for the first time
				UpdateTileInfo(0);
				_selectedSprite = -1;
				_firstDraw = false;
			}
		}

		private void CreateScreenPreview()
		{
			GCHandle handle = GCHandle.Alloc(_spritePixelData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(64, 128, 4*64, PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());

				using(Graphics g = Graphics.FromImage(_screenPreview)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.Clear(Color.Transparent);

					for(int i = 63; i >= 0; i--) {
						if(i != _selectedSprite) {
							DrawSprite(source, g, i);
						}
					}

					if(_selectedSprite >= 0) {
						DrawSprite(source, g, _selectedSprite);
					}
				}

				using(Graphics g = Graphics.FromImage(picPreview.Image)) {
					g.Clear(Color.FromArgb(64, 64, 64));
					g.DrawImage(_screenPreview, 0, 0);
				}
				picPreview.Invalidate();
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
			_previewMousePosition = null;

			int tileX = Math.Min(e.X * 256 / (picSprites.Width - 2) / 32, 31);
			int tileY = Math.Min(e.Y * 512 / (picSprites.Height - 2) / 64, 63);
			int ramAddr = ((tileY << 3) + tileX) << 2;

			if(ramAddr / 4 == _selectedSprite && !_forceRefresh) {
				return;
			}

			UpdateTileInfo(ramAddr);
		}

		private void UpdateTileInfo(int ramAddr)
		{
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

			this.ctrlTilePalette.SelectedPalette = (attributes & 0x03) + 4;

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

			int tileX = _selectedSprite % 8;
			int tileY = _selectedSprite / 8;

			Bitmap tile = new Bitmap(64, 128);
			Bitmap tilePreview = new Bitmap(8, 16);
			using(Graphics g = Graphics.FromImage(tilePreview)) {
				g.DrawImage(picSprites.Image, new Rectangle(0, 0, 8, 16), new Rectangle(tileX*32, tileY*64, 32, 64), GraphicsUnit.Pixel);
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
			mnuCopyAllSpritesHdPack.Visible = Control.ModifierKeys == Keys.Shift;
			
			if(_selectedSprite < 0) {
				_contextMenuSpriteIndex = -1;
				return;
			}

			_contextMenuSpriteIndex = _selectedSprite;
			_copyData = ToHdPackFormat(_selectedSprite);
		}

		private string ToHdPackFormat(int spriteIndex)
		{
			int ramAddr = spriteIndex * 4;
			int tileIndex = _spriteRam[ramAddr + 1];
			int palette = (_spriteRam[ramAddr + 2] & 0x03) + 4;

			int tileAddr;
			if(_largeSprites) {
				tileAddr = ((tileIndex & 0x01) == 0x01 ? 0x1000 : 0x0000) + ((tileIndex & 0xFE) << 4);
			} else {
				tileAddr = _spritePatternAddr + (tileIndex << 4);
			}

			return _hdCopyHelper.ToHdPackFormat(tileAddr, palette, true, false);
		}

		private void picPreview_MouseMove(object sender, MouseEventArgs e)
		{
			_previewMousePosition = e.Location;
			SelectSpriteUnderCursor();
		}

		private void SelectSpriteUnderCursor()
		{
			Point p = _previewMousePosition.Value;
			int xPos = p.X * 256 / (picPreview.Width - 2);
			int yPos = p.Y * 240 / (picPreview.Height - 2);
			int prevSprite = _selectedSprite;
			_selectedSprite = -1;
			for(int i = 0x100 - 4; i >= 0; i-=4) {
				int spriteY = _spriteRam[i];
				int spriteX = _spriteRam[i + 3];

				if(xPos >= spriteX && xPos < spriteX + 8 && yPos >= spriteY && yPos < spriteY + (_largeSprites ? 16 : 8)) {
					_selectedSprite = i / 4;
					break;
				}
			}

			if(prevSprite != _selectedSprite) {
				if(_selectedSprite >= 0) {
					UpdateTileInfo(_selectedSprite * 4);
				}
				CreateScreenPreview();
			}
		}

		private void picPreview_MouseLeave(object sender, EventArgs e)
		{
			CreateScreenPreview();
		}

		private void ShowInChrViewer()
		{
			if(_selectedSprite < 0 && _contextMenuSpriteIndex < 0) {
				return;
			}

			int ramAddr = (_selectedSprite >= 0 ? _selectedSprite : _contextMenuSpriteIndex) * 4;
			int tileIndex = _spriteRam[ramAddr + 1];
			int palette = (_spriteRam[ramAddr + 2] & 0x03) + 4;

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			if(_largeSprites) {
				if(tileIndex % 2 == 1) {
					tileIndex += 256;
					tileIndex--;
				}
				OnSelectTilePalette?.Invoke(tileIndex, palette);
			} else {
				int tileIndexOffset = state.PPU.ControlFlags.SpritePatternAddr == 0x1000 ? 256 : 0;
				OnSelectTilePalette?.Invoke(tileIndex+tileIndexOffset, palette);
			}
		}

		private void picSprites_DoubleClick(object sender, EventArgs e)
		{
			ShowInChrViewer();
		}

		private void mnuShowInChrViewer_Click(object sender, EventArgs e)
		{
			ShowInChrViewer();
		}

		private void picSprites_MouseEnter(object sender, EventArgs e)
		{
			_copyPreview = false;
			if(this.ParentForm.ContainsFocus) {
				this.Focus();
			}
		}

		private void picPreview_MouseEnter(object sender, EventArgs e)
		{
			_copyPreview = true;
			if(this.ParentForm.ContainsFocus) {
				this.Focus();
			}
		}

		private void mnuCopyToClipboard_Click(object sender, EventArgs e)
		{
			CopyToClipboard();
		}

		private Bitmap GetCopyBitmap()
		{
			Bitmap src = _copyPreview ? _screenPreview : _imgSprites;
			Bitmap target = new Bitmap(src.Width, src.Height);
			using(Graphics g = Graphics.FromImage(target)) {
				g.Clear(Color.FromArgb(64, 64, 64));
				g.DrawImage(src, 0, 0);
			}
			return target;
		}

		public void CopyToClipboard()
		{
			using(Bitmap target = GetCopyBitmap()) {
				Clipboard.SetImage(target);
			}
		}

		private void mnuExportToPng_Click(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter("PNG files|*.png");
				if(sfd.ShowDialog() == DialogResult.OK) {
					using(Bitmap target = GetCopyBitmap()) {
						target.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
					}
				}
			}
		}

		private void mnuCopyAllSpritesHdPack_Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < 64; i++) {
				int ramAddr = i * 4;
				int spriteY = _spriteRam[ramAddr];
				int spriteX = _spriteRam[ramAddr+3];
				int attributes = _spriteRam[ramAddr+2];
				bool horizontalMirror = (attributes & 0x40) == 0x40;
				bool verticalMirror = (attributes & 0x80) == 0x80;

				if(spriteY >= 0 && spriteY < 240) {
					sb.AppendLine(
						ToHdPackFormat(i) + "," +
						spriteX.ToString() + "," +
						spriteY.ToString() + "," +
						(horizontalMirror ? "Y" : "N") + "," +
						(verticalMirror ? "Y" : "N")
					);
				}
			}
			if(sb.Length > 0) {
				Clipboard.SetText(sb.ToString());
			} else {
				Clipboard.Clear();
			}
		}

		private void mnuEditInMemoryViewer_Click(object sender, EventArgs e)
		{
			if(_selectedSprite < 0 && _contextMenuSpriteIndex < 0) {
				return;
			}

			int ramAddr = (_selectedSprite >= 0 ? _selectedSprite : _contextMenuSpriteIndex) * 4;
			int tileIndex = _spriteRam[ramAddr + 1];
			
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			int tileIndexOffset = (!_largeSprites && state.PPU.ControlFlags.SpritePatternAddr == 0x1000) ? 256 : 0;
			DebugWindowManager.OpenMemoryViewer((tileIndex + tileIndexOffset) * 16, DebugMemoryType.PpuMemory);
		}
	}
}
