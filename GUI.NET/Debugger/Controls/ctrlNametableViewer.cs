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
			int nametableIndex = 0;
			if(e.X >= 256) {
				nametableIndex++;
			}
			if(e.Y >= 240) {
				nametableIndex+=2;
			}

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
	}
}
