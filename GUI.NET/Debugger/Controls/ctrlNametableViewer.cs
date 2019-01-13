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
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlNametableViewer : BaseControl
	{
		public event EventHandler OnSelectChrTile;

		private byte[][] _nametablePixelData = new byte[4][];

		private byte[][] _prevTileData = new byte[4][];
		private byte[][] _prevAttributeData = new byte[4][];

		private byte[][] _tileData = new byte[4][];
		private byte[][] _attributeData = new byte[4][];
		private Bitmap _gridOverlay;
		private Bitmap _nametableImage = new Bitmap(512, 480);
		private int _currentPpuAddress = -1;
		private int _tileX = 0;
		private int _tileY = 0;
		private int _xScroll = 0;
		private int _yScroll = 0;
		private int _nametableIndex = 0;
		private ctrlChrViewer _chrViewer;
		private DebugState _state = new DebugState();
		private HdPackCopyHelper _hdCopyHelper = new HdPackCopyHelper();
		private bool _firstDraw = true;
		private bool[] _ntChanged = null;

		public ctrlNametableViewer()
		{
			InitializeComponent();

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				chkShowPpuScrollOverlay.Checked = ConfigManager.Config.DebugInfo.ShowPpuScrollOverlay;
				chkShowTileGrid.Checked = ConfigManager.Config.DebugInfo.ShowTileGrid;
				chkShowAttributeGrid.Checked = ConfigManager.Config.DebugInfo.ShowAttributeGrid;
				chkHighlightChrTile.Checked = ConfigManager.Config.DebugInfo.HighlightChrTile;
				chkUseGrayscalePalette.Checked = ConfigManager.Config.DebugInfo.NtViewerUseGrayscalePalette;
				chkHighlightTileUpdates.Checked = ConfigManager.Config.DebugInfo.NtViewerHighlightTileUpdates;
				chkHighlightAttributeUpdates.Checked = ConfigManager.Config.DebugInfo.NtViewerHighlightAttributeUpdates;
				chkIgnoreRedundantWrites.Checked = ConfigManager.Config.DebugInfo.NtViewerIgnoreRedundantWrites;

				UpdateIgnoreWriteCheckbox();
			}
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

		public void Connect(ctrlChrViewer chrViewer)
		{
			_chrViewer = chrViewer;
		}

		public void GetData()
		{
			InteropEmu.DebugGetPpuScroll(out _xScroll, out _yScroll);
			InteropEmu.DebugGetState(ref _state);

			_ntChanged = InteropEmu.DebugGetNametableChangedData();

			//Keep a copy of the previous frame to highlight modifications
			for(int i = 0; i < 4; i++) {
				_prevTileData[i] = _tileData[i] != null ? (byte[])_tileData[i].Clone() : null;
				_prevAttributeData[i] = _attributeData[i] != null ? (byte[])_attributeData[i].Clone() : null;
			}

			for(int i = 0; i < 4; i++) {
				InteropEmu.DebugGetNametable(i, ConfigManager.Config.DebugInfo.NtViewerUseGrayscalePalette, out _nametablePixelData[i], out _tileData[i], out _attributeData[i]);
			}

			_hdCopyHelper.RefreshData();
		}

		public void RefreshViewer()
		{
			int tileIndexOffset = _state.PPU.ControlFlags.BackgroundPatternAddr == 0x1000 ? 256 : 0;
			lblMirroringType.Text = ResourceHelper.GetEnumText(_state.Cartridge.Mirroring);

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

				for(int i = 0; i < 4; i++) {
					if(_chrViewer.SelectedTileIndex >= 0 && this.chkHighlightChrTile.Checked) {
						HighlightChrViewerTile(tileIndexOffset, g, i);
					}
				}

				if(this._gridOverlay != null) {
					g.DrawImage(this._gridOverlay, 0, 0);
				}

				if(chkShowPpuScrollOverlay.Checked) {
					DrawScrollOverlay(_xScroll, _yScroll, g);
				}
				
				if(chkHighlightAttributeUpdates.Checked || chkHighlightTileUpdates.Checked) {
					DrawEditHighlights(g);
				}
			}

			this.picNametable.Image = target;

			if(_firstDraw) {
				_currentPpuAddress = 0x2000;
				UpdateTileInformation(0, 0, 0x2000, 0);
				_firstDraw = false;
			}
		}

		private void DrawEditHighlights(Graphics g)
		{
			bool ignoreRedundantWrites = chkIgnoreRedundantWrites.Checked;
			using(Brush redBrush = new SolidBrush(Color.FromArgb(128, Color.Red))) {
				using(Brush yellowBrush = new SolidBrush(Color.FromArgb(128, Color.Yellow))) {
					for(int nt = 0; nt < 4; nt++) {
						if(_prevTileData[nt] == null || _prevAttributeData[nt] == null || _ntChanged == null) {
							continue;
						}

						int ntBaseAddress = nt * 0x400;
						for(int y = 0; y < 30; y++) {
							for(int x = 0; x < 32; x++) {
								int tileX = ((nt % 2 == 1) ? x + 32 : x) * 8;
								int tileY = ((nt >= 2) ? y + 30 : y) * 8;

								bool tileChanged = false;
								bool attrChanged = false;

								if(ignoreRedundantWrites) {
									tileChanged = _prevTileData[nt][y * 32 + x] != _tileData[nt][y * 32 + x];
									int shift = (x & 0x02) | ((y & 0x02) << 1);
									int attribute = (_attributeData[nt][y * 32 + x] >> shift) & 0x03;
									int prevAttribute = (_prevAttributeData[nt][y * 32 + x] >> shift) & 0x03;
									attrChanged = attribute != prevAttribute;
								} else {
									int tileAddress = ntBaseAddress + y * 32 + x;
									int attrAddress = ntBaseAddress + 32 * 30 + ((y & 0xFC) << 1) + (x >> 2);

									tileChanged = _ntChanged[tileAddress];
									attrChanged = _ntChanged[attrAddress];
								}

								if(chkHighlightTileUpdates.Checked && tileChanged) {
									g.FillRectangle(redBrush, tileX, tileY, 8, 8);
									g.DrawRectangle(Pens.Red, tileX, tileY, 8, 8);
								}

								if(chkHighlightAttributeUpdates.Checked && attrChanged) {
									g.FillRectangle(yellowBrush, tileX, tileY, 8, 8);
									g.DrawRectangle(Pens.Yellow, tileX, tileY, 8, 8);
								}
							}
						}
					}
				}
			}
		}

		private void HighlightChrViewerTile(int tileIndexOffset, Graphics dest, int nametableIndex)
		{
			int xOffset = nametableIndex % 2 == 0 ? 0 : 256;
			int yOffset = nametableIndex <= 1 ? 0 : 240;

			using(Pen pen = new Pen(Color.Red, 2)) {
				for(int j = 0; j < 960; j++) {
					if(_tileData[nametableIndex][j] + tileIndexOffset == _chrViewer.SelectedTileIndex) {
						dest.DrawRectangle(pen, new Rectangle(xOffset + (j%32)*8-1, yOffset + (j/32)*8-1, 10, 10));
					}
				}
			}
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
			int xPos = e.X * 512 / (picNametable.Width - 2);
			int yPos = e.Y * 480 / (picNametable.Height - 2);

			_nametableIndex = 0;
			if(xPos >= 256) {
				_nametableIndex++;
			}
			if(yPos >= 240) {
				_nametableIndex += 2;
			}

			int baseAddress = 0x2000 + _nametableIndex * 0x400;

			_tileX = Math.Min(xPos / 8, 63);
			_tileY = Math.Min(yPos / 8, 59);

			if(_nametableIndex % 2 == 1) {
				_tileX -= 32;
			}
			if(_nametableIndex >= 2) {
				_tileY -= 30;
			}

			int shift = (_tileX & 0x02) | ((_tileY & 0x02) << 1);
			int ppuAddress = (baseAddress + _tileX + _tileY * 32);
			if(_currentPpuAddress == ppuAddress) {
				return;
			}
			_currentPpuAddress = ppuAddress;

			UpdateTileInformation(xPos, yPos, baseAddress, shift);
		}

		private void UpdateTileInformation(int xPos, int yPos, int baseAddress, int shift)
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			int bgAddr = state.PPU.ControlFlags.BackgroundPatternAddr;

			int tileIndex = _tileData[_nametableIndex][_tileY * 32 + _tileX];
			int attributeData = _attributeData[_nametableIndex][_tileY * 32 + _tileX];
			int attributeAddr = baseAddress + 960 + ((_tileY & 0xFC) << 1) + (_tileX >> 2);
			int paletteBaseAddr = ((attributeData >> shift) & 0x03) << 2;

			this.ctrlTilePalette.SelectedPalette = (paletteBaseAddr >> 2);

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
				g.DrawImage(_nametableImage, new Rectangle(0, 0, 8, 8), new Rectangle(xPos / 8 * 8, yPos / 8 * 8, 8, 8), GraphicsUnit.Pixel);
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

		private void chkHighlightChrTile_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.HighlightChrTile = chkHighlightChrTile.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkUseGrayscalePalette_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.NtViewerUseGrayscalePalette = chkUseGrayscalePalette.Checked;
			ConfigManager.ApplyChanges();
			this.GetData();
			this.RefreshViewer();
		}

		private void chkHighlightTileUpdates_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.NtViewerHighlightTileUpdates = chkHighlightTileUpdates.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
			UpdateIgnoreWriteCheckbox();
		}

		private void chkHighlightAttributeUpdates_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.NtViewerHighlightAttributeUpdates = chkHighlightAttributeUpdates.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
			UpdateIgnoreWriteCheckbox();
		}

		private void chkIgnoreRedundantWrites_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.NtViewerIgnoreRedundantWrites = chkIgnoreRedundantWrites.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void UpdateIgnoreWriteCheckbox()
		{
			chkIgnoreRedundantWrites.Enabled = chkHighlightAttributeUpdates.Checked || chkHighlightTileUpdates.Checked;
		}

		string _copyData;
		private void mnuCopyHdPack_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(_copyData);
		}

		private string ToHdPackFormat(int nametableIndex, int nametableTileIndex)
		{
			int x = nametableTileIndex % 32;
			int y = nametableTileIndex / 32;

			int tileIndex = _tileData[_nametableIndex][nametableTileIndex];
			int attributeData = _attributeData[_nametableIndex][nametableTileIndex];
			int shift = (x & 0x02) | ((y & 0x02) << 1);
			int palette = (attributeData >> shift) & 0x03;
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			int bgAddr = state.PPU.ControlFlags.BackgroundPatternAddr;
			int tileAddr = bgAddr + tileIndex * 16;

			return _hdCopyHelper.ToHdPackFormat(tileAddr, palette, false, false);
		}

		private void ctxMenu_Opening(object sender, CancelEventArgs e)
		{
			mnuCopyNametableHdPack.Visible = Control.ModifierKeys == Keys.Shift;
			_copyData = ToHdPackFormat(_nametableIndex, _tileY * 32 + _tileX);
		}

		private void ShowInChrViewer()
		{
			int tileIndex = _tileData[_nametableIndex][_tileY*32+_tileX];
			int attributeData = _attributeData[_nametableIndex][_tileY*32+_tileX];
			int shift = (_tileX & 0x02) | ((_tileY & 0x02) << 1);
			int paletteIndex = ((attributeData >> shift) & 0x03);

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			int tileIndexOffset = state.PPU.ControlFlags.BackgroundPatternAddr == 0x1000 ? 256 : 0;

			if(!InteropEmu.DebugIsExecutionStopped() || ConfigManager.Config.DebugInfo.PpuRefreshOnBreak) {
				//Only change the palette if execution is not stopped (or if we're configured to refresh the viewer on break/pause)
				//Otherwise, the CHR viewer will refresh its data (and it might not match the data we loaded at the specified scanline/cycle anymore)
				_chrViewer.SelectedPaletteIndex = paletteIndex;
			}
			_chrViewer.SelectedTileIndex = tileIndex + tileIndexOffset;
			OnSelectChrTile?.Invoke(null, EventArgs.Empty);
		}

		private void picNametable_DoubleClick(object sender, EventArgs e)
		{
			ShowInChrViewer();
		}

		private void mnuShowInChrViewer_Click(object sender, EventArgs e)
		{
			ShowInChrViewer();
		}

		private void mnuCopyToClipboard_Click(object sender, EventArgs e)
		{
			CopyToClipboard();
		}

		public void CopyToClipboard()
		{
			Clipboard.SetImage(_nametableImage);
		}

		private void mnuCopyNametableHdPack_Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			for(int y = 0; y < 30; y++) {
				for(int x = 0; x < 32; x++) {
					sb.AppendLine(ToHdPackFormat(_nametableIndex, y*32+x) + "," + (x * 8).ToString() + "," + (y*8).ToString());
				}
			}
			Clipboard.SetText(sb.ToString());
		}

		private void mnuExportToPng_Click(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter("PNG files|*.png");
				if(sfd.ShowDialog() == DialogResult.OK) {
					_nametableImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
				}
			}
		}

		private void mnuEditInMemoryViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenMemoryViewer(_currentPpuAddress, DebugMemoryType.PpuMemory);
		}

		private void picNametable_MouseEnter(object sender, EventArgs e)
		{
			if(this.ParentForm.ContainsFocus) {
				this.Focus();
			}
		}
	}
}
