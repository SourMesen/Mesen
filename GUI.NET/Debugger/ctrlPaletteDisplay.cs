using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;
using System.Runtime.InteropServices;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger
{
	public partial class ctrlPaletteDisplay : UserControl
	{
		private int[] _paletteData;
		private bool _showColorIndexes;

		public delegate void PaletteClickHandler(int colorIndex);
		public event PaletteClickHandler ColorClick;

		public ctrlPaletteDisplay()
		{
			InitializeComponent();
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ShowColorIndexes
		{
			get
			{
				return _showColorIndexes;
			}
			set
			{
				_showColorIndexes = value;
				this.RefreshPalette();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int[] PaletteData
	{
			get
			{
				return _paletteData;
			}
			set
			{
				_paletteData = new int[value.Length];
				Array.Copy(value, _paletteData, value.Length);
				this.RefreshPalette();
			}
		}

		private void RefreshPalette()
		{
			if(this._paletteData == null) {
				return;
			}

			GCHandle handle = GCHandle.Alloc(this.PaletteData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(16, 4, 16*4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap target = new Bitmap(336, 336);

				Font font = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize - 2, GraphicsUnit.Pixel);
				using(Graphics g = Graphics.FromImage(target)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.ScaleTransform(42, 42);
					g.DrawImageUnscaled(source, 0, 0);
					g.DrawImageUnscaled(source, -8, 4);

					if(this.ShowColorIndexes) {
						g.ScaleTransform(1f/42, 1f/42);
						using(Brush bg = new SolidBrush(Color.FromArgb(150, Color.LightGray))) {
							for(int y = 0; y < 8; y++) {
								for(int x = 0; x < 8; x++) {
									int index = y * 16 + x;
									if(y >= 4) {
										index = (y - 4) * 16 + x + 8;
									}
									g.DrawOutlinedString(index.ToString("X2"), font, Brushes.Black, bg, 42*x + 22, 42*y + 26);
								}
							}
						}
					}
				}
				picPalette.Image = target;
			} finally {
				handle.Free();
			}
		}

		private void picPalette_MouseDown(object sender, MouseEventArgs e)
		{
			float xPos = (float)e.X / picPalette.Image.Width;
			float yPos = (float)e.Y / picPalette.Image.Height;

			float y = yPos < 0.5 ? yPos : (yPos - 0.5f);
			float x = yPos < 0.5 ? xPos : (xPos + 1);

			int offset = (int)(x * 8) + (int)(y * 8) * 16;

			ColorClick?.Invoke(offset);
		}
	}
}
