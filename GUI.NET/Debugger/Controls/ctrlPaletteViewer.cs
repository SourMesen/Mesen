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
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlPaletteViewer : BaseControl
	{
		private byte[] _paletteRam;
		private int[] _palettePixelData;
		private int _paletteIndex = -1;

		public ctrlPaletteViewer()
		{
			InitializeComponent();
		}
		
		public void GetData()
		{
			this._paletteRam = InteropEmu.DebugGetMemoryState(DebugMemoryType.PaletteMemory);
			this._palettePixelData = InteropEmu.DebugGetPalette();
		}

		public void RefreshViewer()
		{
			GCHandle handle = GCHandle.Alloc(this._palettePixelData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(4, 8, 4*4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap target = new Bitmap(128, 256);

				using(Graphics g = Graphics.FromImage(target)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.ScaleTransform(32, 32);
					g.DrawImageUnscaled(source, 0, 0);

					g.ResetTransform();
					Font font = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize - 2, GraphicsUnit.Pixel);
					using(Brush bg = new SolidBrush(Color.FromArgb(150, Color.LightGray))) {
						for(int y = 0; y < 8; y++) {
							for(int x = 0; x < 4; x++) {
								g.DrawOutlinedString(_paletteRam[y*4+x].ToString("X2"), font, Brushes.Black, bg, 32*x+14, 32*y+18);
							}
						}
					}
				}
				this.picPalette.Image = target;
			} finally {
				handle.Free();
			}

			this.picPalette.Refresh();

			if(_paletteIndex == -1) {
				UpdateColorInformation(0);
			}
		}

		private void picPalette_MouseMove(object sender, MouseEventArgs e)
		{
			int tileX = Math.Min(e.X * 128 / (picPalette.Width - 2) / 32, 31);
			int tileY = Math.Min(e.Y * 256 / (picPalette.Height - 2) / 32, 31);

			int paletteIndex = tileY * 4 + tileX;

			if(paletteIndex != _paletteIndex) {
				UpdateColorInformation(paletteIndex);
			}
		}

		private void UpdateColorInformation(int paletteIndex)
		{
			int tileX = paletteIndex % 4;
			int tileY = paletteIndex / 4;
			_paletteIndex = paletteIndex;

			this.txtColor.Text = _paletteRam[paletteIndex].ToString("X2");
			this.txtPaletteAddress.Text = (0x3F00 + paletteIndex).ToString("X4");

			this.txtColorCodeHex.Text = GetHexColorString();
			this.txtColorCodeRgb.Text = GetRgbColorString();

			Bitmap tile = new Bitmap(64, 64);
			using(Graphics g = Graphics.FromImage(tile)) {
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.DrawImage(picPalette.Image, new Rectangle(0, 0, 64, 64), new Rectangle(tileX * 32, tileY * 32, 32, 32), GraphicsUnit.Pixel);
			}
			this.picColor.Image = tile;
		}

		private void picPalette_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				using(frmSelectColor frm = new frmSelectColor()) {
					if(frm.ShowDialog(this) == DialogResult.OK) {
						int x = Math.Min(e.X * 128 / picPalette.Width / 32, 31);
						int y = Math.Min(e.Y * 256 / picPalette.Height / 32, 31);
						int colorAddress = y * 4 + x;

						InteropEmu.DebugSetMemoryValue(DebugMemoryType.PaletteMemory, (uint)colorAddress, (byte)frm.ColorIndex);
						this.GetData();
						this.RefreshViewer();
						this.UpdateColorInformation(this._paletteIndex);
					}
				}
			}
		}

		private string GetHexColorString()
		{
			return "#" + _palettePixelData[_paletteIndex].ToString("X8").Substring(2, 6);
		}

		private string GetRgbColorString()
		{
			Color selectedColor = Color.FromArgb(_palettePixelData[_paletteIndex]);
			return "rgb(" + selectedColor.R.ToString() + ", " + selectedColor.G.ToString() + ", " + selectedColor.B.ToString() + ")";
		}

		private void ctxMenu_Opening(object sender, CancelEventArgs e)
		{
			mnuCopyHexColor.Text = "Copy Hex Color - " + GetHexColorString();
			mnuCopyRgbColor.Text = "Copy RGB Color - " + GetRgbColorString();
		}

		private void mnuCopyHexColor_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(GetHexColorString());
		}

		private void mnuCopyRgbColor_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(GetRgbColorString());
		}
	}
}
