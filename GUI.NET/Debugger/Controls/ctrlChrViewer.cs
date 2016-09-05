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
	public partial class ctrlChrViewer : UserControl
	{
		public ctrlChrViewer()
		{
			InitializeComponent();
		}
		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);
			if(!this.DesignMode) {
				this.UpdateDropdown();
				this.cboHighlightType.SelectedIndex = 0;
				this.cboPalette.SelectedIndex = 0;
			}
		}

		public void RefreshViewer()
		{
			PictureBox[] chrBanks = new PictureBox[] { this.picChrBank1, this.picChrBank2 };

			UpdateDropdown();

			for(int i = 0; i < 2; i++) {
				byte[] pixelData = InteropEmu.DebugGetChrBank(i + this.cboChrSelection.SelectedIndex * 2, this.cboPalette.SelectedIndex, this.chkLargeSprites.Checked, (CdlHighlightType)this.cboHighlightType.SelectedIndex);

				GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
				try {
					Bitmap source = new Bitmap(128, 128, 4*128, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
					Bitmap target = new Bitmap(256, 256);

					using(Graphics g = Graphics.FromImage(target)) {
						g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
						g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
						g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
						g.Clear(Color.Black);
						g.DrawImage(source, new Rectangle(0, 0, 256, 256), new Rectangle(0, 0, 128, 128), GraphicsUnit.Pixel);
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
			this.RefreshViewer();
		}

		private void chkLargeSprites_Click(object sender, EventArgs e)
		{
			this.RefreshViewer();
		}

		private void cboHighlightType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.RefreshViewer();
		}

		private void cboChrSelection_SelectionChangeCommitted(object sender, EventArgs e)
		{
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
			using(Graphics g = Graphics.FromImage(tile)) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.DrawImage(((PictureBox)sender).Image, new Rectangle(0, 0, 64, 64), new Rectangle(tileX*16, tileY*16, 16, 16), GraphicsUnit.Pixel);
			}
			this.picTile.Image = tile;
		}
	}
}
