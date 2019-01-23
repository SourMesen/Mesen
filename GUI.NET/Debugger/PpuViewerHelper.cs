using Mesen.GUI.Config;
using Mesen.GUI.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	public class PpuViewerHelper
	{
		private static Font _font = new Font(BaseControl.MonospaceFontFamily, 10);

		public static void DrawPalettePreview(int paletteIndex, Graphics g)
		{
			int[] palette = InteropEmu.DebugGetPalette();
			GCHandle handle = GCHandle.Alloc(palette, GCHandleType.Pinned);
			Bitmap source = new Bitmap(4, 1, 4 * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject() + paletteIndex * 16);

			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.SmoothingMode = SmoothingMode.None;
			g.PixelOffsetMode = PixelOffsetMode.Half;

			Matrix transform = g.Transform;
			g.ScaleTransform(16, 16);
			g.DrawImageUnscaled(source, 0, 0);

			g.Transform = transform;
			g.DrawRectangle(Pens.White, 1, 1, 63, 15);
		}

		public static void DrawOverlayTooltip(Image dest, string tooltipText, Image tilePreview, int paletteIndex, bool topRightAnchor, Graphics g)
		{

			int palettePreviewHeight = paletteIndex >= 0 ? 16 : 0;
			SizeF requiredSize = g.MeasureString(tooltipText, _font);
			requiredSize = new SizeF(requiredSize.Width + tilePreview.Width + 13, Math.Max(requiredSize.Height, tilePreview.Height + 10 + palettePreviewHeight));

			if(topRightAnchor) {
				g.TranslateTransform(dest.Width - requiredSize.Width - 5, 5);
			} else {
				g.TranslateTransform(dest.Width - requiredSize.Width - 5, dest.Height - requiredSize.Height - 5);
			}

			using(SolidBrush brush = new SolidBrush(Color.FromArgb(180, 0, 0, 0))) {
				g.FillRectangle(brush, 0, 0, requiredSize.Width, requiredSize.Height);
			}
			g.DrawRectangle(Pens.White, 0, 0, requiredSize.Width, requiredSize.Height);
			g.DrawString(tooltipText, _font, Brushes.White, 5, 5, StringFormat.GenericTypographic);
			g.DrawImage(tilePreview, requiredSize.Width - tilePreview.Width - 3, 5);
			g.DrawRectangle(Pens.White, requiredSize.Width - tilePreview.Width - 3, 5, tilePreview.Width - 1, tilePreview.Height - 1);

			if(paletteIndex >= 0) {
				g.TranslateTransform(requiredSize.Width - tilePreview.Width - 3, tilePreview.Height + 7);
				PpuViewerHelper.DrawPalettePreview(paletteIndex, g);
			}
		}

		public static Bitmap GetPreview(Point originalPos, Size originalSize, int scale, Image source)
		{
			Bitmap tile = new Bitmap(originalSize.Width * scale, originalSize.Height * scale);
			Bitmap tilePreview = new Bitmap(originalSize.Width, originalSize.Height);
			using(Graphics g = Graphics.FromImage(tilePreview)) {
				g.DrawImage(source, 0, 0, new Rectangle(originalPos.X, originalPos.Y, originalSize.Width, originalSize.Height), GraphicsUnit.Pixel);
			}
			using(Graphics g = Graphics.FromImage(tile)) {
				g.InterpolationMode = InterpolationMode.NearestNeighbor;
				g.SmoothingMode = SmoothingMode.None;
				g.PixelOffsetMode = PixelOffsetMode.Half;
				g.ScaleTransform(scale, scale);
				g.DrawImageUnscaled(tilePreview, 0, 0);
			}
			return tile;
		}
	}
}
