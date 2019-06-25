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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlNametableViewer : BaseControl, ICompactControl
	{
		public delegate void OnSelectChrTileHandler(int tileIndex, int paletteIndex);
		public event OnSelectChrTileHandler OnSelectChrTile;

		private byte[][] _nametablePixelData = new byte[4][];

		private byte[][] _prevTileData = new byte[4][];
		private byte[][] _prevAttributeData = new byte[4][];

		private byte[][] _tileData = new byte[4][];
		private byte[][] _attributeData = new byte[4][];
		private Bitmap _gridOverlay;
		private Bitmap _nametableImage = new Bitmap(512, 480, PixelFormat.Format32bppPArgb);
		private Bitmap _finalImage = new Bitmap(512, 480, PixelFormat.Format32bppPArgb);
		private Bitmap _hudImage = new Bitmap(512, 480, PixelFormat.Format32bppPArgb);
		private TileInfo _tileInfo;
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
		private bool _showAttributeColorsOnly = false;

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

				chkShowAttributeColorsOnly.Checked = ConfigManager.Config.DebugInfo.ShowAttributeColorsOnly;
				_showAttributeColorsOnly = ConfigManager.Config.DebugInfo.ShowAttributeColorsOnly;
				chkUseGrayscalePalette.Enabled = !_showAttributeColorsOnly;

				UpdateIgnoreWriteCheckbox();
			}
		}

		public Size GetCompactSize(bool includeMargins)
		{
			return new Size(picNametable.Width, picNametable.Height);
		}

		public void ScaleImage(double scale)
		{
			picNametable.Size = new Size((int)(picNametable.Width * scale), (int)(picNametable.Height * scale));
			picNametable.InterpolationMode = scale > 1 ? InterpolationMode.NearestNeighbor : InterpolationMode.Default;
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
				mnuAddBreakpoint.InitShortcut(this, nameof(DebuggerShortcutsConfig.PpuViewer_AddBreakpointTile));
				mnuAddBreakpointAttribute.InitShortcut(this, nameof(DebuggerShortcutsConfig.PpuViewer_AddBreakpointAttribute));
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

			NametableDisplayMode mode;
			if(_showAttributeColorsOnly) {
				mode = NametableDisplayMode.AttributeView;
			} else if(ConfigManager.Config.DebugInfo.NtViewerUseGrayscalePalette) {
				mode = NametableDisplayMode.Grayscale;
			} else {
				mode = NametableDisplayMode.Normal;
			}

			for(int i = 0; i < 4; i++) {
				InteropEmu.DebugGetNametable(i, mode, out _nametablePixelData[i], out _tileData[i], out _attributeData[i]);
			}

			_hdCopyHelper.RefreshData();
		}

		public void RefreshViewer()
		{
			int tileIndexOffset = _state.PPU.ControlFlags.BackgroundPatternAddr == 0x1000 ? 256 : 0;
			lblMirroringType.Text = ResourceHelper.GetEnumText(_state.Cartridge.Mirroring);

			using(Graphics gNametable = Graphics.FromImage(_nametableImage)) {
				for(int i = 0; i < 4; i++) {
					GCHandle handle = GCHandle.Alloc(_nametablePixelData[i], GCHandleType.Pinned);
					Bitmap source = new Bitmap(256, 240, 4*256, PixelFormat.Format32bppPArgb, handle.AddrOfPinnedObject());
					try {
						gNametable.DrawImage(source, new Rectangle(i % 2 == 0 ? 0 : 256, i <= 1 ? 0 : 240, 256, 240), new Rectangle(0, 0, 256, 240), GraphicsUnit.Pixel);
					} finally {
						handle.Free();
					}
				}
			}

			if(this._gridOverlay == null && (chkShowTileGrid.Checked || chkShowAttributeGrid.Checked)) {
				this._gridOverlay = new Bitmap(512, 480, PixelFormat.Format32bppPArgb);

				using(Graphics overlay = Graphics.FromImage(this._gridOverlay)) {
					if(chkShowTileGrid.Checked) {
						using(Pen pen = new Pen(Color.FromArgb(chkShowAttributeGrid.Checked ? 120 : 180, 240, 100, 120))) {
							if(chkShowAttributeGrid.Checked) {
								pen.DashStyle = DashStyle.Dot;
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

			using(Graphics g = Graphics.FromImage(_finalImage)) {
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

			if(_firstDraw) {
				_currentPpuAddress = 0x2000;
				UpdateTileInformation(0, 0, 0x2000, 0);
				_firstDraw = false;
			}

			this.DrawHud();
		}

		private void DrawHud()
		{
			using(Graphics g = Graphics.FromImage(_hudImage)) {
				g.DrawImage(_finalImage, 0, 0);

				if(_currentPpuAddress >= 0) {
					//Draw overlay over current tile
					int x = _tileX + ((_nametableIndex & 0x01) == 1 ? 32 : 0);
					int y = _tileY + (_nametableIndex >= 2 ? 30 : 0);
					using(SolidBrush brush = new SolidBrush(Color.FromArgb(100, 255, 255, 255))) {
						g.FillRectangle(brush, x * 8, y * 8, 8, 8);
					}
					g.DrawRectangle(Pens.White, x * 8, y * 8, 7, 7);

					if(ConfigManager.Config.DebugInfo.PpuShowInformationOverlay) {
						//Draw tooltip box with information
						string tooltipText = (
							"Tile:      $" + _tileInfo.PpuAddress.ToString("X4") + " = $" + _tileInfo.TileIndex.ToString("X2") + Environment.NewLine +
							"Position:  " + _tileInfo.TileX.ToString() + ", " + _tileInfo.TileY.ToString() + Environment.NewLine +
							"Attribute: $" + _tileInfo.AttributeAddress.ToString("X4") + " = $" + _tileInfo.AttributeData.ToString("X2") + Environment.NewLine +
							"Palette:   " + (_tileInfo.PaletteAddress >> 2).ToString() + " ($" + (0x3F00 + _tileInfo.PaletteAddress).ToString("X4") + ")" + Environment.NewLine
						);

						PpuViewerHelper.DrawOverlayTooltip(_hudImage, tooltipText, picTile.Image, _tileInfo.PaletteAddress >> 2, _nametableIndex >= 2, g);
					}
				}
			}

			picNametable.Image = _hudImage;
			picNametable.Refresh();
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

		private void picNametable_MouseLeave(object sender, EventArgs e)
		{
			_currentPpuAddress = -1;
			DrawHud();
		}

		private void picNametable_MouseMove(object sender, MouseEventArgs e)
		{
			int xPos = Math.Max(0, e.X * 512 / (picNametable.Width - 2));
			int yPos = Math.Max(0, e.Y * 480 / (picNametable.Height - 2));

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
			DrawHud();
		}

		private void UpdateTileInformation(int xPos, int yPos, int baseAddress, int shift)
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			int bgAddr = state.PPU.ControlFlags.BackgroundPatternAddr;

			byte tileIndex = _tileData[_nametableIndex][_tileY * 32 + _tileX];
			byte attributeData = _attributeData[_nametableIndex][_tileY * 32 + _tileX];

			_tileInfo = new TileInfo() {
				PpuAddress = _currentPpuAddress,
				TileIndex = tileIndex,
				AttributeData = attributeData,
				AttributeAddress = baseAddress + 960 + ((_tileY & 0xFC) << 1) + (_tileX >> 2),
				PaletteAddress = ((attributeData >> shift) & 0x03) << 2,
				TileX = _tileX,
				TileY = _tileY,
				Nametable = _nametableIndex,
				TileAddress = bgAddr + tileIndex * 16
			};

			this.ctrlTilePalette.SelectedPalette = (_tileInfo.PaletteAddress >> 2);

			this.txtPpuAddress.Text = _tileInfo.PpuAddress.ToString("X4");
			this.txtNametable.Text = _tileInfo.Nametable.ToString();
			this.txtLocation.Text = _tileInfo.TileX.ToString() + ", " + _tileInfo.TileY.ToString();
			this.txtTileIndex.Text = _tileInfo.TileIndex.ToString("X2");
			this.txtTileAddress.Text = _tileInfo.TileAddress.ToString("X4");
			this.txtAttributeData.Text = _tileInfo.AttributeData.ToString("X2");
			this.txtAttributeAddress.Text = _tileInfo.AttributeAddress.ToString("X4");
			this.txtPaletteAddress.Text = (0x3F00 + _tileInfo.PaletteAddress).ToString("X4");

			picTile.Image = PpuViewerHelper.GetPreview(new Point(xPos / 8 * 8, yPos / 8 * 8), new Size(8, 8), 8, _nametableImage);
		}

		private void chkShowScrollWindow_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowPpuScrollOverlay = chkShowPpuScrollOverlay.Checked;
			ConfigManager.ApplyChanges();
			this.RefreshViewer();
		}

		private void chkShowAttributeColorsOnly_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowAttributeColorsOnly = chkShowAttributeColorsOnly.Checked;
			ConfigManager.ApplyChanges();

			_showAttributeColorsOnly = chkShowAttributeColorsOnly.Checked;
			chkUseGrayscalePalette.Enabled = !chkShowAttributeColorsOnly.Checked;
			this.GetData();
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
			mnuAddBreakpoint.Text = "Add breakpoint (Tile - $" + _tileInfo.PpuAddress.ToString("X4") + ")";
			mnuAddBreakpointAttribute.Text = "Add breakpoint (Attribute - $" + _tileInfo.AttributeAddress.ToString("X4") + ")";
			mnuEditInMemoryViewer.Text = "Edit in Memory Viewer ($" + _currentPpuAddress.ToString("X4") + ")";
			mnuAddBreakpoint.Enabled = DebugWindowManager.GetDebugger() != null;
			mnuAddBreakpointAttribute.Enabled = DebugWindowManager.GetDebugger() != null;
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

			OnSelectChrTile?.Invoke(tileIndex + tileIndexOffset, paletteIndex);
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
			DebugWindowManager.OpenMemoryViewer(_tileInfo.PpuAddress, DebugMemoryType.PpuMemory);
		}

		private void AddBreakpoint(int address)
		{
			PpuAddressTypeInfo addressInfo = InteropEmu.DebugGetPpuAbsoluteAddressAndType((uint)address);

			BreakpointManager.EditBreakpoint(new Breakpoint() {
				MemoryType = addressInfo.Type.ToMemoryType(),
				BreakOnExec = false,
				BreakOnRead = true,
				BreakOnWrite = true,
				Address = (UInt32)addressInfo.Address,
				StartAddress = (UInt32)addressInfo.Address,
				EndAddress = (UInt32)addressInfo.Address,
				AddressType = BreakpointAddressType.SingleAddress
			});
		}

		private void mnuAddBreakpointAttribute_Click(object sender, EventArgs e)
		{
			if(DebugWindowManager.GetDebugger() == null) {
				return;
			}
			AddBreakpoint(_tileInfo.AttributeAddress);
		}

		private void mnuAddBreakpoint_Click(object sender, EventArgs e)
		{
			if(DebugWindowManager.GetDebugger() == null) {
				return;
			}
			AddBreakpoint(_tileInfo.PpuAddress);
		}

		private void picNametable_MouseEnter(object sender, EventArgs e)
		{
			if(this.ParentForm.ContainsFocus) {
				this.Focus();
			}
		}

		private class TileInfo
		{
			public int PpuAddress;
			public byte TileIndex;
			public int TileAddress;
			public byte AttributeData;
			public int AttributeAddress;
			public int PaletteAddress;
			public int TileX;
			public int TileY;
			public int Nametable;
		}
	}
}
