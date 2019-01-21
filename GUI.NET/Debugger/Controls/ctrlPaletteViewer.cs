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
using System.Drawing.Drawing2D;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlPaletteViewer : BaseControl, ICompactControl
	{
		private byte[] _paletteRam;
		private int[] _palettePixelData;
		private int _paletteIndex = -1;

		public ctrlPaletteViewer()
		{
			InitializeComponent();
		}

		public Size GetCompactSize(bool includeMargins)
		{
			int margins = includeMargins ? (picPaletteBg.Margin.Right + picPaletteSprites.Margin.Left) : 0;
			return new Size(picPaletteBg.Width * 2 + margins, picPaletteBg.Height);
		}

		public void ScaleImage(double scale)
		{
			picPaletteBg.Size = new Size((int)(picPaletteBg.Width * scale), (int)(picPaletteBg.Height * scale));
			picPaletteSprites.Size = new Size((int)(picPaletteSprites.Width * scale), (int)(picPaletteSprites.Height * scale));
			picPaletteBg.InterpolationMode = scale > 1 ? InterpolationMode.NearestNeighbor : InterpolationMode.Default;
			picPaletteSprites.InterpolationMode = scale > 1 ? InterpolationMode.NearestNeighbor : InterpolationMode.Default;
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
				for(int i = 0; i < 2; i++) {
					Bitmap source = new Bitmap(4, 4, 4 * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject()+i*16*4);
					Bitmap target = new Bitmap(128, 128);

					using(Graphics g = Graphics.FromImage(target)) {
						g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
						g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
						g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
						g.ScaleTransform(32, 32);
						g.DrawImageUnscaled(source, 0, 0);

						g.ResetTransform();
						Font font = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize - 2, GraphicsUnit.Pixel);
						using(Brush bg = new SolidBrush(Color.FromArgb(150, Color.LightGray))) {
							for(int y = 0; y < 4; y++) {
								for(int x = 0; x < 4; x++) {
									g.DrawOutlinedString(_paletteRam[y * 4 + x + i * 16].ToString("X2"), font, Brushes.Black, bg, 32 * x + 14, 32 * y + 18);
								}
							}
						}
					}
					if(i == 0) {
						this.picPaletteBg.Image = target;
					} else {
						this.picPaletteSprites.Image = target;
					}
				}
			} finally {
				handle.Free();
			}

			this.picPaletteBg.Refresh();
			this.picPaletteSprites.Refresh();

			if(_paletteIndex == -1) {
				UpdateColorInformation(0);
			}
		}

		private void picPalette_MouseMove(object sender, MouseEventArgs e)
		{
			int tileX = Math.Max(0, Math.Min(e.X * 128 / (picPaletteBg.Width - 2) / 32, 3));
			int tileY = Math.Max(0, Math.Min(e.Y * 128 / (picPaletteBg.Height - 2) / 32, 3));

			int paletteIndex = tileY * 4 + tileX + (sender == picPaletteSprites ? 16 : 0);

			if(paletteIndex != _paletteIndex) {
				UpdateColorInformation(paletteIndex);
			}
		}

		private void UpdateColorInformation(int paletteIndex)
		{
			int tileX = paletteIndex % 4;
			int tileY = (paletteIndex / 4) % 4;
			_paletteIndex = paletteIndex;

			this.txtColor.Text = _paletteRam[paletteIndex].ToString("X2");
			this.txtPaletteAddress.Text = (0x3F00 + paletteIndex).ToString("X4");

			this.txtColorCodeHex.Text = GetHexColorString();
			this.txtColorCodeRgb.Text = GetRgbColorString();

			this.picColor.Image = PpuViewerHelper.GetPreview(new Point(tileX * 32, tileY * 32), new Size(32, 32), 2, paletteIndex >= 16 ? picPaletteSprites.Image : picPaletteBg.Image);
		}

		private void picPalette_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				using(frmSelectColor frm = new frmSelectColor()) {
					if(frm.ShowDialog(this) == DialogResult.OK) {
						int x = Math.Min(e.X * 128 / picPaletteBg.Width / 32, 31);
						int y = Math.Min(e.Y * 128 / picPaletteBg.Height / 32, 31);
						int colorAddress = y * 4 + x + (sender == picPaletteSprites ? 16 : 0);

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
