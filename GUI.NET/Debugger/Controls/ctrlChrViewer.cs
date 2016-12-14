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

		public ctrlChrViewer()
		{
			InitializeComponent();

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				this.cboPalette.SelectedIndex = 0;
				this.cboHighlightType.SelectedIndex = 0;
			}
		}

		public void GetData()
		{
			for(int i = 0; i < 2; i++) {
				_chrPixelData[i] = InteropEmu.DebugGetChrBank(i + _chrSelection * 2, _selectedPalette, _useLargeSprites, _highlightType);
			}
		}

		public void RefreshViewer()
		{
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
					chrBanks[i].Image = target;
				} finally {
					handle.Free();
				}
			}
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
			}
		}

		private void cboChrSelection_DropDown(object sender, EventArgs e)
		{
			UpdateDropdown();
		}

		private void cboPalette_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._selectedPalette = this.cboPalette.SelectedIndex;
			this.RefreshViewer();
		}

		private void chkLargeSprites_Click(object sender, EventArgs e)
		{
			this._useLargeSprites = this.chkLargeSprites.Checked;
			this.RefreshViewer();
		}

		private void cboHighlightType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._highlightType = (CdlHighlightType)this.cboHighlightType.SelectedIndex;
			this.RefreshViewer();
		}

		private void cboChrSelection_SelectionChangeCommitted(object sender, EventArgs e)
		{
			this._chrSelection = this.cboChrSelection.SelectedIndex;
			this.RefreshViewer();
		}

		private void picChrBank_MouseMove(object sender, MouseEventArgs e)
		{
			List<PictureBox> chrBanks = new List<PictureBox>() { this.picChrBank1, this.picChrBank2 };
			int bankIndex = chrBanks.IndexOf((PictureBox)sender);
			int baseAddress = bankIndex == 0 ? 0x0000 : 0x1000;
			if(this.cboChrSelection.SelectedIndex > 1) {
				baseAddress += (this.cboChrSelection.SelectedIndex - 1) * 0x2000;
			}

			int tileX = Math.Min(e.X / 16, 15);
			int tileY = Math.Min(e.Y / 16, 15);

			int tileIndex = tileY * 16 + tileX;

			this.txtTileIndex.Text = tileIndex.ToString("X2");
			this.txtTileAddress.Text = (baseAddress + tileIndex * 16).ToString("X4");

			Bitmap tile = new Bitmap(64, 64);
			Bitmap tilePreview = new Bitmap(16, 16);
			using(Graphics g = Graphics.FromImage(tilePreview)) {
				g.DrawImage(((PictureBox)sender).Image, new Rectangle(0, 0, 16, 16), new Rectangle(tileX*16, tileY*16, 16, 16), GraphicsUnit.Pixel);
			}
			using(Graphics g = Graphics.FromImage(tile)) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.ScaleTransform(4, 4);
				g.DrawImageUnscaled(tilePreview, 0, 0);
			}
			this.picTile.Image = tile;
		}
	}
}
