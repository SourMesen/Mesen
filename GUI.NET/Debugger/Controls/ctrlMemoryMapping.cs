using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	class MemoryRegionInfo
	{
		public string Name { get; set; }
		public int Size { get; set; }
		public Color Color { get; set; }
	}

	class ctrlMemoryMapping : Control
	{
		List<MemoryRegionInfo> _regions = new List<MemoryRegionInfo>();

		public ctrlMemoryMapping()
		{
			this.DoubleBuffered = true;
			this.ResizeRedraw = true;
		}

		private void UpdateRegionArray(List<MemoryRegionInfo> regions)
		{
			if(regions.Count != _regions.Count) {
				_regions = regions;
				this.Invalidate();
			} else {
				for(int i = 0; i < regions.Count; i++) {
					if(_regions[i].Color != regions[i].Color || _regions[i].Name != regions[i].Name || _regions[i].Size != regions[i].Size) {
						_regions = regions;
						this.Invalidate();
						return;
					}
				}
			}
		}

		public void UpdateCpuRegions(CartridgeState state)
		{
			List<MemoryRegionInfo> regions = new List<MemoryRegionInfo>();

			regions.Add(new MemoryRegionInfo() { Name = "Internal RAM", Size = 0x2000, Color = Color.FromArgb(222, 222, 222) });
			regions.Add(new MemoryRegionInfo() { Name = "CPU Registers", Size = 0x2020, Color = Color.FromArgb(222, 222, 222) });
			regions.Add(new MemoryRegionInfo() { Name = "N/A", Size = 0x1FE0, Color = Color.FromArgb(222, 222, 222) });
			regions.Add(new MemoryRegionInfo() { Name = "Work RAM", Size = 0x2000, Color = Color.FromArgb(0xCD, 0xDC, 0xFA) });

			for(int i = 0; i < 0x8000 / state.PrgPageSize; i++) {
				string text = state.PrgSelectedPages[i] == 0xEEEEEEEE ? "N/A" : ("$" + state.PrgSelectedPages[i].ToString("X2"));
				regions.Add(new MemoryRegionInfo() { Name = text, Size = (int)state.PrgPageSize, Color = i % 2 == 0 ? Color.FromArgb(0xC4, 0xE7, 0xD4) : Color.FromArgb(0xA4, 0xD7, 0xB4) });
			}

			UpdateRegionArray(regions);
		}

		public void UpdatePpuRegions(CartridgeState state)
		{
			List<MemoryRegionInfo> regions = new List<MemoryRegionInfo>();

			for(int i = 0; i < 0x2000 / state.ChrPageSize; i++) {
				string text = state.ChrSelectedPages[i] == 0xEEEEEEEE ? "N/A" : ("$" + state.ChrSelectedPages[i].ToString("X2"));
				regions.Add(new MemoryRegionInfo() { Name = text, Size = (int)state.ChrPageSize, Color = i % 2 == 0 ? Color.FromArgb(0xC4, 0xE0, 0xF4) : Color.FromArgb(0xB4, 0xD0, 0xE4) });
			}

			for(int i = 0; i < 4; i++) {
				regions.Add(new MemoryRegionInfo() { Name = "NT " + state.Nametables[i].ToString(), Size = 0x400, Color = i % 2 == 0 ? Color.FromArgb(0xF4, 0xC7, 0xD4) : Color.FromArgb(0xD4, 0xA7, 0xB4) });
			}

			UpdateRegionArray(regions);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.Clear(Color.LightGray);

			if(_regions.Count > 0) {
				Rectangle rect = Rectangle.Inflate(this.ClientRectangle, -2, -1);

				int totalSize = 0;
				foreach(MemoryRegionInfo region in _regions) {
					totalSize += region.Size;
				}

				float pixelsPerByte = (float)rect.Width / totalSize;

				StringFormat verticalFormat = new StringFormat(StringFormatFlags.DirectionVertical);

				float currentPosition = 1;
				int byteOffset = 0;
				foreach(MemoryRegionInfo region in _regions) {
					float length = pixelsPerByte * region.Size;
					using(Brush brush = new SolidBrush(region.Color)) {
						e.Graphics.FillRectangle(brush, currentPosition, 0, length, rect.Height);
					}
					e.Graphics.DrawRectangle(Pens.Black, currentPosition, 0, length, rect.Height);
					e.Graphics.RotateTransform(-90);
					SizeF textSize = e.Graphics.MeasureString(byteOffset.ToString("X4"), new Font("Arial", 8));
					e.Graphics.DrawString(byteOffset.ToString("X4"), new Font("Arial", 8), Brushes.Black, -rect.Height + (rect.Height - textSize.Width) / 2, currentPosition + 3);
					e.Graphics.ResetTransform();

					textSize = e.Graphics.MeasureString(region.Name, new Font("Arial", 9));
					e.Graphics.DrawString(region.Name, new Font("Arial", 9), Brushes.Black, currentPosition + 12 + ((length - 12) / 2 - textSize.Width / 2), rect.Height / 2 - 7);

					currentPosition += length;
					byteOffset += region.Size;
				}
			}
		}
	}
}
