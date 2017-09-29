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
		private bool _allowSelection = false;
		private int _hoverColor = -1;
		private int _selectedColor = 0;
		
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

		public ctrlTilePalette()
		{
			InitializeComponent();
		}

		public void RefreshPalette()
		{
			if(_selectedPalette < 0) {
				return;
			}

			byte[] paletteRam = InteropEmu.DebugGetMemoryState(DebugMemoryType.PaletteMemory);
			int[] palette = InteropEmu.DebugGetPalette();
			int[] currentPalette = new int[16];
			Array.Copy(palette, _selectedPalette * 4, currentPalette, 0, 4);

			GCHandle handle = GCHandle.Alloc(currentPalette, GCHandleType.Pinned);
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
									g.DrawOutlinedString(paletteRam[_selectedPalette*4 + i].ToString("X2"), font, Brushes.Black, bg, 14+i*32, 18);
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
	}
}
