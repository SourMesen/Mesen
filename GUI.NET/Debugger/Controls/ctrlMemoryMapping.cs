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

			Action<int> addEmpty = (int size) => { regions.Add(new MemoryRegionInfo() { Name = "N/A", Size = size, Color = Color.FromArgb(222, 222, 222) }); };
			Action<int, int> addWorkRam = (int page, int size) => {
				string name = size >= 0x2000 ? ("Work RAM ($" + page.ToString("X2") + ")") : (size >= 0x800 ? ("$" + page.ToString("X2")) : "");
				regions.Add(new MemoryRegionInfo() { Name = name, Size = size, Color = Color.FromArgb(0xCD, 0xDC, 0xFA) });
			};
			Action<int, int> addSaveRam = (int page, int size) => {
				string name = size >= 0x2000 ? ("Save RAM ($" + page.ToString("X2") + ")") : (size >= 0x800 ? ("$" + page.ToString("X2")) : "");
				regions.Add(new MemoryRegionInfo() { Name = name, Size = size, Color = Color.FromArgb(0xFA, 0xDC, 0xCD) });
			};
			Action<int, int, Color> addPrgRom = (int page, int size, Color color) => { regions.Add(new MemoryRegionInfo() { Name = "$" + page.ToString("X2"), Size = size, Color = color }); };

			PrgMemoryType? memoryType = null;
			int currentSize = 0;
			int sizeOffset = -0x20;
			int startIndex = 0x40;
			bool alternateColor = true;
			
			Action<int> addSection = (int i) => {
				if(currentSize == 0) {
					return;
				}

				int size = currentSize + sizeOffset;
				if(memoryType == null) {
					addEmpty(size);
				} else if(memoryType == PrgMemoryType.PrgRom) {
					addPrgRom((int)(state.PrgMemoryOffset[startIndex] / state.PrgPageSize), size, alternateColor ? Color.FromArgb(0xC4, 0xE7, 0xD4) : Color.FromArgb(0xA4, 0xD7, 0xB4));
					alternateColor = !alternateColor;
				} else if(memoryType == PrgMemoryType.WorkRam) {
					addWorkRam((int)(state.PrgMemoryOffset[startIndex] / state.WorkRamPageSize), size);
				} else if(memoryType == PrgMemoryType.SaveRam) {
					if(state.HasBattery) {
						addSaveRam((int)(state.PrgMemoryOffset[startIndex] / state.SaveRamPageSize), size);
					} else {
						addWorkRam((int)(state.PrgMemoryOffset[startIndex] / state.SaveRamPageSize), size);
					}
				}
				sizeOffset = 0;
				currentSize = 0;
				startIndex = i;
			};

			for(int i = 0x40; i < 0x100; i++) {
				if(state.PrgMemoryAccess[i] != MemoryAccessType.NoAccess) {
					bool forceNewBlock = memoryType == PrgMemoryType.PrgRom && (i - startIndex) << 8 >= state.PrgPageSize;
					if(forceNewBlock || memoryType != state.PrgMemoryType[i] || state.PrgMemoryOffset[i] - state.PrgMemoryOffset[i-1] != 0x100) {
						addSection(i);
					}
					memoryType = state.PrgMemoryType[i];
				} else {
					if(memoryType != null) {
						addSection(i);
					}
					memoryType = null;
				}
				currentSize += 0x100;
			}
			addSection(-1);

			UpdateRegionArray(regions);
		}

		public void UpdatePpuRegions(CartridgeState state)
		{
			List<MemoryRegionInfo> regions = new List<MemoryRegionInfo>();

			ChrMemoryType? memoryType = null;
			int currentSize = 0;
			int startIndex = 0;
			bool alternateColor = true;

			Action<int> addSection = (int i) => {
				if(currentSize == 0) {
					return;
				}

				if(memoryType == null) {
					regions.Add(new MemoryRegionInfo() { Name = "N/A", Size = currentSize, Color = Color.FromArgb(222, 222, 222) });
				} else if(memoryType == ChrMemoryType.ChrRom || memoryType == ChrMemoryType.Default && state.ChrRomSize > 0) {
					int page = (int)(state.ChrMemoryOffset[startIndex] / state.ChrPageSize);
					Color color = alternateColor ? Color.FromArgb(0xC4, 0xE7, 0xD4) : Color.FromArgb(0xA4, 0xD7, 0xB4);
					alternateColor = !alternateColor;
					regions.Add(new MemoryRegionInfo() { Name = "$" + page.ToString("X2"), Size = currentSize, Color = color });
				} else if(memoryType == ChrMemoryType.ChrRam || memoryType == ChrMemoryType.Default && state.ChrRomSize == 0) {
					int page = (int)(state.ChrMemoryOffset[startIndex] / state.ChrPageSize);
					Color color = alternateColor ? Color.FromArgb(0xC4, 0xE0, 0xF4) : Color.FromArgb(0xB4, 0xD0, 0xE4);
					alternateColor = !alternateColor;
					regions.Add(new MemoryRegionInfo() { Name = "$" + page.ToString("X2"), Size = currentSize, Color = color });
				}
				currentSize = 0;
				startIndex = i;
			};

			for(int i = 0; i < 0x20; i++) {
				if(state.ChrMemoryAccess[i] != MemoryAccessType.NoAccess) {
					bool forceNewBlock = (i - startIndex) << 8 >= state.ChrPageSize;
					if(forceNewBlock || memoryType != state.ChrMemoryType[i] || state.ChrMemoryOffset[i] - state.ChrMemoryOffset[i - 1] != 0x100) {
						addSection(i);
					}
					memoryType = state.ChrMemoryType[i];
				} else {
					if(memoryType != null) {
						addSection(i);
					}
					memoryType = null;
				}
				currentSize += 0x100;
			}
			addSection(-1);

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
