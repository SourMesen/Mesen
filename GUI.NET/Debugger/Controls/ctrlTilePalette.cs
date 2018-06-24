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
using Mesen.GUI.Forms;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlTilePalette : BaseControl
	{
		private int _selectedPalette = -1;
		private UInt32? _paletteColors = null;
		private bool _allowSelection = false;
		private int _hoverColor = -1;
		private int _selectedColor = 0;
		private int[] _currentPalette = new int[4];

		public bool HighlightMouseOver { get; set; }
		public bool DisplayIndexes { get; set; }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedColor {
			get
			{
				return _selectedColor;
			}
			set
			{
				_selectedColor = value;
				this.RefreshPalette();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AllowSelection
		{
			get
			{
				return _allowSelection;
			}
			set
			{
				if(value) {
					this.picPaletteSelection.Cursor = new Cursor(Properties.Resources.Pipette.GetHicon());
				} else {
					this.picPaletteSelection.Cursor = Cursors.Default;
				}
				_allowSelection = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedPalette
		{
			get
			{
				return _selectedPalette;
			}
			set
			{
				_selectedPalette = value;
				this.RefreshPalette();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UInt32? PaletteColors
		{
			get
			{
				return _paletteColors;
			}
			set
			{
				_paletteColors = value;
				this.RefreshPalette();
			}
		}


		public ctrlTilePalette()
		{
			InitializeComponent();
		}

		public void RefreshPalette()
		{
			if(_selectedPalette < 0 && !_paletteColors.HasValue) {
				return;
			}

			int[] paletteColorCodes = new int[4];
			if(_paletteColors.HasValue) {
				int[] paletteData = InteropEmu.GetRgbPalette();
				for(int i = 0; i < 4; i++) {
					paletteColorCodes[i] = (int)(_paletteColors.Value >> (8 * i)) & 0x3F;
					_currentPalette[i] = paletteData[paletteColorCodes[i]];
				}
			} else {
				byte[] paletteRam = InteropEmu.DebugGetMemoryState(DebugMemoryType.PaletteMemory);
				int[] palette = InteropEmu.DebugGetPalette();
				Array.Copy(palette, _selectedPalette * 4, _currentPalette, 0, 4);
				for(int i = 0; i < 4; i++) {
					paletteColorCodes[i] = paletteRam[_selectedPalette * 4 + i];
				}
			}

			GCHandle handle = GCHandle.Alloc(_currentPalette, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(4, 1, 4*4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap target = new Bitmap(128, 32);

				using(Graphics g = Graphics.FromImage(target)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.ScaleTransform(32, 32);
					g.DrawImageUnscaled(source, 0, 0);

					g.ResetTransform();

					using(Font font = new Font(BaseControl.MonospaceFontFamily, 10, GraphicsUnit.Pixel)) {
						using(Brush bg = new SolidBrush(Color.FromArgb(150, Color.LightGray))) {
							for(int i = 0; i < 4; i++) {
								if(this.DisplayIndexes) {
									g.DrawOutlinedString(i.ToString(), font, Brushes.Black, bg, 5+i*32, 2);
								} else {
									g.DrawOutlinedString(paletteColorCodes[i].ToString("X2"), font, Brushes.Black, bg, 14+i*32, 18);
								}
							}
						}
					}
					if(this.AllowSelection) {
						using(Pen pen = new Pen(Color.LightBlue, 3)) {
							g.DrawRectangle(pen, this.SelectedColor * 32 + 2, 2, 29, 29);
						}
					}
					if(this.HighlightMouseOver && _hoverColor >= 0) {
						using(Pen pen = new Pen(Color.DarkGray, 3)) {
							g.DrawRectangle(pen, _hoverColor * 32 + 2, 2, 29, 29);
						}
					}
				}
				this.picPaletteSelection.Image = target;
			} finally {
				handle.Free();
			}
		}
		
		private void picPaletteSelection_MouseMove(object sender, MouseEventArgs e)
		{
			_hoverColor = e.X * 128 / (this.Width - 2) / 32;
			RefreshPalette();
		}

		private void picPaletteSelection_MouseDown(object sender, MouseEventArgs e)
		{
			this.SelectedColor = e.X * 128 / (this.Width - 2) / 32;
			RefreshPalette();
		}

		private void picPaletteSelection_MouseLeave(object sender, EventArgs e)
		{
			_hoverColor = -1;
			RefreshPalette();
		}

		private string GetHexColorString()
		{
			return "#" + _currentPalette[this.SelectedColor].ToString("X8").Substring(2, 6);
		}

		private string GetRgbColorString()
		{
			Color selectedColor = Color.FromArgb(_currentPalette[this.SelectedColor]);
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
