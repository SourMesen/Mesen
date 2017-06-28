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
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlNametableViewer : BaseControl
	{
		private byte[][] _nametablePixelData = new byte[4][];
		private byte[][] _tileData = new byte[4][];
		private byte[][] _attributeData = new byte[4][];
		private Bitmap _gridOverlay;
		private Bitmap _nametableImage = new Bitmap(512, 480);
		private int _currentPpuAddress = -1;
		private int _tileX = 0;
		private int _tileY = 0;
		private int _nametableIndex = 0;

		public ctrlNametableViewer()
		{
			InitializeComponent();

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				chkShowPpuScrollOverlay.Checked = ConfigManager.Config.DebugInfo.ShowPpuScrollOverlay;
				chkShowTileGrid.Checked = ConfigManager.Config.DebugInfo.ShowTileGrid;
				chkShowAttributeGrid.Checked = ConfigManager.Config.DebugInfo.ShowAttributeGrid;
			}
		}

		public void GetData()
		{
			for(int i = 0; i < 4; i++) {
				InteropEmu.DebugGetNametable(i, out _nametablePixelData[i], out _tileData[i], out _attributeData[i]);
			}
		}

		public void RefreshViewer()
		{
			_currentPpuAddress = -1;

			int xScroll, yScroll;
			InteropEmu.DebugGetPpuScroll(out xScroll, out yScroll);

			Bitmap target = new Bitmap(512, 480);
			_nametableImage = new Bitmap(512, 480);

			using(Graphics gNametable = Graphics.FromImage(_nametableImage)) {
				for(int i = 0; i < 4; i++) {
					GCHandle handle = GCHandle.Alloc(_nametablePixelData[i], GCHandleType.Pinned);
					Bitmap source = new Bitmap(256, 240, 4*256, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
					try {
						gNametable.DrawImage(source, new Rectangle(i % 2 == 0 ? 0 : 256, i <= 1 ? 0 : 240, 256, 240), new Rectangle(0, 0, 256, 240), GraphicsUnit.Pixel);
					} finally {
						handle.Free();
					}
				}
			}

			if(this._gridOverlay == null && (chkShowTileGrid.Checked || chkShowAttributeGrid.Checked)) {
				this._gridOverlay = new Bitmap(512, 480);

				using(Graphics overlay = Graphics.FromImage(this._gridOverlay)) {
					if(chkShowTileGrid.Checked) {
						using(Pen pen = new Pen(Color.FromArgb(chkShowAttributeGrid.Checked ? 120 : 180, 240, 100, 120))) {
							if(chkShowAttributeGrid.Checked) {
								pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
							}
							DrawGrid(overlay, pen, 1);
						}
					}

					if(chkShowAttributeGrid.Checked) {
						using(Pen pen = new Pen(Color.FromArgb(180, 80, 130, 250))) {
							DrawGrid(overlay, pen, 2);
						}
					}
				}
			}

			using(Graphics g = Graphics.FromImage(target)) {
				g.DrawImage(_nametableImage, 0, 0);

				if(this._gridOverlay != null) {
					g.DrawImage(this._gridOverlay, 0, 0);
				}

				if(chkShowPpuScrollOverlay.Checked) {
					DrawScrollOverlay(xScroll, yScroll, g);
				}
			}
			this.picNametable.Image = target;
		}

		private static void DrawGrid(Graphics g, Pen pen, int factor)
		{
			for(int i = 0; i < 64 / factor; i++) {
				g.DrawLine(pen, i * 8 * factor - 1, 0, i * 8 * factor - 1, 479);
			}

			for(int i = 0; i < 60 / factor; i++) {
				g.DrawLine(pen, 0, i * 8 * factor - 1, 511, i * 8 * factor - 1);
			}
		}

		private static void DrawScrollOverlay(int xScroll, int yScroll, Graphics g)
		{
			using(Brush brush = new SolidBrush(Color.FromArgb(75, 100, 180, 215))) {
				g.FillRectangle(brush, xScroll, yScroll, 256, 240);
				if(xScroll + 256 >= 512) {
					g.FillRectangle(brush, 0, yScroll, xScroll - 256, 240);
				}
				if(yScroll + 240 >= 480) {
					g.FillRectangle(brush, xScroll, 0, 256, yScroll - 240);
				}
				if(xScroll + 256 >= 512 && yScroll + 240 >= 480) {
					g.FillRectangle(brush, 0, 0, xScroll - 256, yScroll - 240);
				}
			}
			using(Pen pen = new Pen(Color.FromArgb(230, 150, 150, 150), 2)) {
				g.DrawRectangle(pen, xScroll, yScroll, 256, 240);
				if(xScroll + 256 >= 512) {
					g.DrawRectangle(pen, 0, yScroll, xScroll - 256, 240);
				}
				if(yScroll + 240 >= 480) {
					g.DrawRectangle(pen, xScroll, 0, 256, yScroll - 240);
				}
				if(xScroll + 256 >= 512 && yScroll + 240 >= 480) {
					g.DrawRectangle(pen, 0, 0, xScroll - 256, yScroll - 240);
				}
			}
		}

		private void picNametable_MouseMove(object sender, MouseEventArgs e)
		{
			_nametableIndex = 0;
			if(e.X >= 256) {
				_nametableIndex++;
			}
			if(e.Y >= 240) {
				_nametableIndex+=2;
			}

			int baseAddress = 0x2000 + _nametableIndex * 0x400;

			_tileX = Math.Min(e.X / 8, 63);
			_tileY = Math.Min(e.Y / 8, 59);
			int shift = (_tileX & 0x02) | ((_tileY & 0x02) << 1);

			if(_nametableIndex % 2 == 1) {
				_tileX -= 32;
			}
			if(_nametableIndex >= 2) {
				_tileY -= 30;
			}

			int ppuAddress = (baseAddress + _tileX + _tileY * 32);
			if(_currentPpuAddress == ppuAddress) {
				return;
			}
			_currentPpuAddress = ppuAddress;

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			int bgAddr = state.PPU.ControlFlags.BackgroundPatternAddr;
			
			int tileIndex = _tileData[_nametableIndex][_tileY*32+_tileX];
			int attributeData = _attributeData[_nametableIndex][_tileY*32+_tileX];
			int attributeAddr = baseAddress + 960 + ((_tileY & 0xFC) << 1) + (_tileX >> 2);
			int paletteBaseAddr = ((attributeData >> shift) & 0x03) << 2;

			this.txtPpuAddress.Text = _currentPpuAddress.ToString("X4");
			this.txtNametable.Text = _nametableIndex.ToString();
			this.txtLocation.Text = _tileX.ToString() + ", " + _tileY.ToString();
			this.txtTileIndex.Text = tileIndex.ToString("X2");
			this.txtTileAddress.Text = (bgAddr + tileIndex * 16).ToString("X4");
			this.txtAttributeData.Text = attributeData.ToString("X2");
			this.txtAttributeAddress.Text = attributeAddr.ToString("X4");
			this.txtPaletteAddress.Text = (0x3F00 + paletteBaseAddr).ToString("X4");

			Bitmap tile = new Bitmap(64, 64);			
			Bitmap tilePreview = new Bitmap(8, 8);
			using(Graphics g = Graphics.FromImage(tilePreview)) {
				g.DrawImage(_nametableImage, new Rectangle(0, 0, 8, 8), new Rectangle(e.X/8*8, e.Y/8*8, 8, 8), GraphicsUnit.Pixel);
			}			
			using(Graphics g = Graphics.FromImage(tile)) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.ScaleTransform(8, 8);
				g.DrawImageUnscaled(tilePreview, 0, 0);
			}
			this.picTile.Image = tile;
		}

		private void chkShowScrollWindow_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowPpuScrollOverlay = chkShowPpuScrollOverlay.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkShowTileGrid_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowTileGrid = chkShowTileGrid.Checked;
			ConfigManager.ApplyChanges();
			this._gridOverlay = null;
			this.RefreshViewer();
		}

		private void chkShowAttributeGrid_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowAttributeGrid = chkShowAttributeGrid.Checked;
			ConfigManager.ApplyChanges();
			this._gridOverlay = null;
			this.RefreshViewer();
		}

		string _copyData;
		private void mnuCopyHdPack_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(_copyData);
		}

		private void ctxMenu_Opening(object sender, CancelEventArgs e)
		{
			int baseAddress = 0x2000 + _nametableIndex * 0x400;
			int tileIndex = _tileData[_nametableIndex][_tileY*32+_tileX];
			int attributeData = _attributeData[_nametableIndex][_tileY*32+_tileX];
			int shift = (_tileX & 0x02) | ((_tileY & 0x02) << 1);
			int paletteBaseAddr = ((attributeData >> shift) & 0x03) << 2;
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			int bgAddr = state.PPU.ControlFlags.BackgroundPatternAddr;
			int tileAddr = bgAddr + tileIndex * 16;

			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < 16; i++) {
				sb.Append(InteropEmu.DebugGetMemoryValue(DebugMemoryType.PpuMemory, (uint)(tileAddr + i)).ToString("X2"));
			}
			sb.Append(",");
			for(int i = 1; i < 4; i++) {
				sb.Append(InteropEmu.DebugGetMemoryValue(DebugMemoryType.PaletteMemory, (uint)(paletteBaseAddr + i)).ToString("X2"));
			}

			_copyData = sb.ToString();
			_copyData = _copyData.Substring(0, _copyData.Length - 1);
		}
	}
}
