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
	public partial class ctrlChrViewer : BaseControl
	{
		private byte[][] _chrPixelData = new byte[2][];
		private int _selectedPalette = 0;
		private int _chrSelection = 0;
		private CdlHighlightType _highlightType = CdlHighlightType.None;
		private bool _useLargeSprites = false;
		private Bitmap _tilePreview;
		private Bitmap[] _chrBanks = new Bitmap[2];

		private bool _bottomBank = false;
		private int _tileIndex = 0;

		private bool _hoverBottomBank = false;
		private int _hoverTileIndex = -1;

		private int _tilePosX = -1;
		private int _tilePosY = -1;

		private bool _forceChrRefresh = false;

		public ctrlChrViewer()
		{
			InitializeComponent();

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				this.cboPalette.SelectedIndex = 0;
				this.cboHighlightType.SelectedIndex = 0;

				this.picTile.Cursor = new Cursor(Properties.Resources.Pencil.GetHicon());

				this.toolTip.SetToolTip(this.picTileTooltip, "Click on the tile to draw with the selected color." + Environment.NewLine + "Right button draws the background color.");
				this.toolTip.SetToolTip(this.picColorTooltip, "Click on a color (or press 1-4) to select it.");
				this.toolTip.SetToolTip(this.picPaletteTooltip, "Press Shift-1 to Shift-8 to select the palette.");

				this.ctrlTilePalette.AllowSelection = true;
				this.ctrlTilePalette.HighlightMouseOver = true;
				this.ctrlTilePalette.DisplayIndexes = true;
			}
		}

		public void GetData()
		{
			for(int i = 0; i < 2; i++) {
				_chrPixelData[i] = InteropEmu.DebugGetChrBank(i + _chrSelection * 2, _selectedPalette, _useLargeSprites, _highlightType);
			}
		}

		public void RefreshViewer(bool refreshPreview = false)
		{
			_forceChrRefresh = true;

			UpdateDropdown();

			PictureBox[] chrBanks = new PictureBox[] { this.picChrBank1, this.picChrBank2 };

			for(int i = 0; i < 2; i++) {
				byte[] pixelData = _chrPixelData[i];

				GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
				try {
					Bitmap source = new Bitmap(128, 128, 4*128, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
					Bitmap target = new Bitmap(256, 256);

					using(Graphics g = Graphics.FromImage(target)) {
						g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
						g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
						g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
						g.ScaleTransform(2, 2);
						g.DrawImageUnscaled(source, 0, 0);
					}

					_chrBanks[i] = target;

					Bitmap chrBankImage = new Bitmap(256, 256);
					using(Graphics g = Graphics.FromImage(chrBankImage)) {
						g.DrawImage(_chrBanks[i], 0, 0);

						if((_bottomBank && i == 1) || (!_bottomBank && i == 0)) {
							int tileX = _tileIndex % 16;
							int tileY = _tileIndex / 16;
							using(Brush brush = new SolidBrush(Color.FromArgb(192, Color.White))) {
								g.FillRectangle(brush, tileX * 16, tileY * 16, 16, 16);
								g.DrawRectangle(Pens.Black, tileX * 16, tileY * 16, 16, 16);
							}
						}
						if(_hoverTileIndex >= 0) {
							if((_hoverBottomBank && i == 1) || (!_hoverBottomBank && i == 0)) {
								int tileX = _hoverTileIndex % 16;
								int tileY = _hoverTileIndex / 16;
								using(Brush brush = new SolidBrush(Color.FromArgb(192, Color.LightBlue))) {
									g.FillRectangle(brush, tileX * 16, tileY * 16, 16, 16);
									g.DrawRectangle(Pens.Black, tileX * 16 - 1, tileY * 16 - 1, 18, 18);
								}
							}
						}
					}
					chrBanks[i].Image = chrBankImage;
				} finally {
					handle.Free();
				}
			}

			this.RefreshPreview(_hoverTileIndex >= 0 ? _hoverTileIndex : _tileIndex, _hoverTileIndex >= 0 ? _hoverBottomBank : _bottomBank);
			ctrlTilePalette.RefreshPalette();
		}

		private UInt32 _chrSize;
		private void UpdateDropdown()
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			if(state.Cartridge.ChrRomSize == 0) {
				this.flpHighlight.Visible = false;
				this.cboHighlightType.SelectedIndex = 0;
			} else {
				this.flpHighlight.Visible = true;
			}

			UInt32 chrSize = state.Cartridge.ChrRomSize == 0 ? state.Cartridge.ChrRamSize : state.Cartridge.ChrRomSize;

			if(chrSize != _chrSize) {
				_chrSize = chrSize;

				int index = this.cboChrSelection.SelectedIndex;
				this.cboChrSelection.Items.Clear();
				this.cboChrSelection.Items.Add("PPU: $0000 - $1FFF");
				for(int i = 0; i < _chrSize / 0x2000; i++) {
					this.cboChrSelection.Items.Add("CHR: $" + (i * 0x2000).ToString("X4") + " - $" + (i * 0x2000 + 0x1FFF).ToString("X4"));
				}

				this.cboChrSelection.SelectedIndex = this.cboChrSelection.Items.Count > index && index >= 0 ? index : 0;
				this._chrSelection = this.cboChrSelection.SelectedIndex;
			}
		}

		private void cboChrSelection_DropDown(object sender, EventArgs e)
		{
			UpdateDropdown();
		}

		private void cboPalette_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._selectedPalette = this.cboPalette.SelectedIndex;
			this.ctrlTilePalette.SelectedPalette = this._selectedPalette;
			this.GetData();
			this.RefreshViewer();
		}

		private void chkLargeSprites_Click(object sender, EventArgs e)
		{
			this._useLargeSprites = this.chkLargeSprites.Checked;
			this.GetData();
			this.RefreshViewer();
		}

		private void cboHighlightType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._highlightType = (CdlHighlightType)this.cboHighlightType.SelectedIndex;
			this.GetData();
			this.RefreshViewer();
		}

		private void cboChrSelection_SelectionChangeCommitted(object sender, EventArgs e)
		{
			this._chrSelection = this.cboChrSelection.SelectedIndex;
			this.GetData();
			this.RefreshViewer();
		}

		private void picChrBank_MouseMove(object sender, MouseEventArgs e)
		{
			int tileX = Math.Min(e.X / 16, 15);
			int tileY = Math.Min(e.Y / 16, 15);

			bool bottomBank = sender == this.picChrBank2;
			int tileIndex = tileY * 16 + tileX;

			if(_forceChrRefresh || bottomBank != _hoverBottomBank || tileIndex != _hoverTileIndex) {
				_hoverBottomBank = sender == this.picChrBank2;
				_hoverTileIndex = tileY * 16 + tileX;
				RefreshViewer();
				RefreshPreview(_hoverTileIndex, _hoverBottomBank);
				_forceChrRefresh = false;
			}
		}

		private void picChrBank_MouseDown(object sender, MouseEventArgs e)
		{
			int tileX = Math.Min(e.X / 16, 15);
			int tileY = Math.Min(e.Y / 16, 15);

			_tileIndex = tileY * 16 + tileX;
			_bottomBank = sender == this.picChrBank2;
		}

		private void picChrBank_MouseLeave(object sender, EventArgs e)
		{
			_hoverTileIndex = -1;
			RefreshPreview(_tileIndex, _bottomBank);
		}

		private void RefreshPreview(int tileIndex, bool bottomBank)
		{
			int baseAddress = bottomBank ? 0x1000 : 0x0000;
			if(this.cboChrSelection.SelectedIndex > 1) {
				baseAddress += (this.cboChrSelection.SelectedIndex - 1) * 0x2000;
			}

			int tileX = tileIndex % 16;
			int tileY = tileIndex / 16;

			int realIndex = GetLargeSpriteIndex(tileIndex);
			this.txtTileIndex.Text = realIndex.ToString("X2");
			this.txtTileAddress.Text = (baseAddress + realIndex * 16).ToString("X4");

			_tilePreview = new Bitmap(128, 128);
			Bitmap source = new Bitmap(16, 16);
			using(Graphics g = Graphics.FromImage(source)) {
				g.DrawImage(bottomBank ? this._chrBanks[1]: this._chrBanks[0], new Rectangle(0, 0, 16, 16), new Rectangle(tileX*16, tileY*16, 16, 16), GraphicsUnit.Pixel);
			}
			using(Graphics g = Graphics.FromImage(_tilePreview)) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.ScaleTransform(8, 8);
				g.DrawImageUnscaled(source, 0, 0);
			}

			Bitmap tile = new Bitmap(128, 128);
			using(Graphics g = Graphics.FromImage(tile)) {
				g.DrawImageUnscaled(_tilePreview, 0, 0);
				using(Brush brush = new SolidBrush(Color.FromArgb(128, Color.White))) {
					g.FillRectangle(brush, _tilePosX*16, _tilePosY*16, 16, 16);
				}
			}
			this.picTile.Image = tile;
		}

		private void picTile_MouseMove(object sender, MouseEventArgs e)
		{
			int x = Math.Max(0, Math.Min(e.X / 16, 7));
			int y = Math.Max(0, Math.Min(e.Y / 16, 7));

			if(x != _tilePosX || y != _tilePosY) {
				_tilePosX = x;
				_tilePosY = y;

				RefreshPreview(_tileIndex, _bottomBank);

				if(_drawing) {
					DrawPixel(e.Button == MouseButtons.Left, _tilePosX, _tilePosY);
				}
			}
		}
		
		private void picTile_MouseLeave(object sender, EventArgs e)
		{
			_tilePosX = -1;
			_tilePosY = -1;
		}

		private int GetLargeSpriteIndex(int tileIndex)
		{
			if(chkLargeSprites.Checked) {
				int tileY = tileIndex / 16;
				int tileX = tileIndex % 16;

				int newX = (tileX * 2) % 16 + ((tileY & 0x01) == 0x01 ? 1 : 0);
				int newY = (tileY & 0xFE) + ((tileX >= 8) ? 1 : 0);
				return newY * 16 + newX;
			} else {
				return tileIndex;
			}
		}

		private void DrawPixel(bool leftButton, int x, int y)
		{
			int baseAddress = _bottomBank ? 0x1000 : 0x0000;
			bool ppuMemory = this.cboChrSelection.SelectedIndex == 0;
			if(this.cboChrSelection.SelectedIndex > 1) {
				baseAddress += (this.cboChrSelection.SelectedIndex - 1) * 0x2000;
			}

			int tileIndex = GetLargeSpriteIndex(_tileIndex);

			byte orgByte1 = InteropEmu.DebugGetMemoryValue(ppuMemory ? DebugMemoryType.PpuMemory : DebugMemoryType.ChrRom, (UInt32)(baseAddress + tileIndex * 16 + y));
			byte orgByte2 = InteropEmu.DebugGetMemoryValue(ppuMemory ? DebugMemoryType.PpuMemory : DebugMemoryType.ChrRom, (UInt32)(baseAddress + tileIndex * 16 + y + 8));

			byte byte1 = (byte)(orgByte1 & ~(0x80 >> x));
			byte byte2 = (byte)(orgByte2 & ~(0x80 >> x));

			byte value = 0;
			if(leftButton) {
				value = (byte)ctrlTilePalette.SelectedColor;
				byte1 |= (byte)(((value << 7) & 0x80) >> x);
				byte2 |= (byte)(((value << 6) & 0x80) >> x);
			}

			if(byte1 != orgByte1 || byte2 != orgByte2) {
				InteropEmu.DebugSetMemoryValue(ppuMemory ? DebugMemoryType.PpuMemory : DebugMemoryType.ChrRom, (UInt32)(baseAddress + tileIndex * 16 + y), byte1);
				InteropEmu.DebugSetMemoryValue(ppuMemory ? DebugMemoryType.PpuMemory : DebugMemoryType.ChrRom, (UInt32)(baseAddress + tileIndex * 16 + y + 8), byte2);

				GetData();
				RefreshViewer();
			}
		}

		private bool _drawing = false;
		private void picTile_MouseDown(object sender, MouseEventArgs e)
		{
			_drawing = true;

			int x = Math.Max(0, Math.Min(e.X / 16, 7));
			int y = Math.Max(0, Math.Min(e.Y / 16, 7));
			DrawPixel(e.Button == MouseButtons.Left, x, y);
		}

		private void picTile_MouseUp(object sender, MouseEventArgs e)
		{
			_drawing = false;
		}

		public void SelectPalette(int palette)
		{
			cboPalette.SelectedIndex = palette;
		}

		public void SelectColor(int color)
		{
			ctrlTilePalette.SelectedColor = color;
		}

		string _copyData;
		private void mnuCopyHdPack_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(_copyData);
		}

		private void ctxMenu_Opening(object sender, CancelEventArgs e)
		{
			int baseAddress = _bottomBank ? 0x1000 : 0x0000;
			bool ppuMemory = this.cboChrSelection.SelectedIndex == 0;
			if(this.cboChrSelection.SelectedIndex > 1) {
				baseAddress += (this.cboChrSelection.SelectedIndex - 1) * 0x2000;
			}

			int tileIndex = GetLargeSpriteIndex(_tileIndex);

			bool isChrRam = InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRom) == 0;
			StringBuilder sb = new StringBuilder();
			if(isChrRam) {
				for(int i = 0; i < 16; i++) {
					sb.Append(InteropEmu.DebugGetMemoryValue(ppuMemory ? DebugMemoryType.PpuMemory : DebugMemoryType.ChrRom, (UInt32)(baseAddress + tileIndex * 16 + i)).ToString("X2"));
				}
			} else {
				int absoluteTileIndex = ppuMemory ? InteropEmu.DebugGetAbsoluteChrAddress((uint)(baseAddress+tileIndex*16))/16 : (baseAddress / 16 + tileIndex);
				sb.Append(absoluteTileIndex.ToString());
			}
			sb.Append(",");
			for(int i = 0; i < 4; i++) {
				sb.Append(InteropEmu.DebugGetMemoryValue(DebugMemoryType.PaletteMemory, (uint)(this._selectedPalette * 4 + i)).ToString("X2"));
			}

			_copyData = sb.ToString();
		}
	}
}
