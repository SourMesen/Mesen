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
using Mesen.GUI.Config;
using System.Collections.Concurrent;
using System.Drawing.Imaging;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlCharacterMappings : BaseControl
	{
		private UInt32 _chrSize;
		private byte[] _chrPixelData;
		private int _chrSelection = 0;
		private ctrlCharacterMapping[] _mappings = new ctrlCharacterMapping[16];
		private byte[] _chrData = new byte[16*16*16];

		public ctrlCharacterMappings()
		{
			InitializeComponent();

			tlpTileMappings.SuspendLayout();
			for(int y = 0; y < 8; y++) {
				for(int x = 0; x < 2; x++) {
					ctrlCharacterMapping mapping = new ctrlCharacterMapping();
					tlpTileMappings.Controls.Add(mapping);
					tlpTileMappings.SetColumn(mapping, x);
					tlpTileMappings.SetRow(mapping, y + 1);
					mapping.picTiles.Image = new Bitmap(416, 20, PixelFormat.Format32bppPArgb);
					_mappings[(y << 1) + x] = mapping;
				}
			}
			tlpTileMappings.ResumeLayout();
		}

		public void GetData()
		{
			UInt32[] paletteData;
			_chrPixelData = InteropEmu.DebugGetChrBank(_chrSelection, 9, false, CdlHighlightType.None, false, false, out paletteData);

			bool isChrRam = InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRom) == 0;
			if(_chrSelection < 2) {
				byte[] chrData = InteropEmu.DebugGetMemoryState(DebugMemoryType.PpuMemory);
				Array.Copy(chrData, _chrSelection * 0x1000, _chrData, 0, 0x1000);
			} else {
				byte[] chrData = InteropEmu.DebugGetMemoryState(isChrRam ? DebugMemoryType.ChrRam : DebugMemoryType.ChrRom);

				int startIndex = (_chrSelection - 2) * 0x1000;
				if(startIndex >= chrData.Length) {
					//Can occur when switching games
					startIndex = 0;
				}

				Array.Copy(chrData, startIndex, _chrData, 0, 0x1000);
			}
		}

		public void RefreshViewer(bool refreshPreview = false)
		{
			UpdateChrBankDropdown();

			GCHandle handle = GCHandle.Alloc(_chrPixelData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(128, 128, 4*128, PixelFormat.Format32bppPArgb, handle.AddrOfPinnedObject());

				for(int y = 0; y < 16; y++) {
					using(Graphics g = Graphics.FromImage(_mappings[y].picTiles.Image)) {
						g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
						g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
						g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

						g.TranslateTransform(1, 0);
						for(int x = 0; x < 16; x++) {
							g.FillRectangle(Brushes.DarkGreen, 3 + x * 26, 1, 18, 18);
						}

						g.ScaleTransform(2, 2);
						for(int x = 0; x < 16; x++) {
							g.DrawImage(source, new Rectangle(2 + x * 13, 1, 8, 8), x * 8, y * 8, 8, 8, GraphicsUnit.Pixel);
						}
					}
					_mappings[y].picTiles.Invalidate();
					_mappings[y].UpdateData(_chrData, y);
				}
			} finally {
				handle.Free();
			}
		}

		public void SetCharacterMappings(ConcurrentDictionary<string, string> charMappings)
		{
			for(int i = 0; i < 16; i++) {
				_mappings[i].SetCharacterMappings(charMappings);
			}
		}

		private void UpdateChrBankDropdown()
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			
			UInt32 chrSize = state.Cartridge.ChrRomSize == 0 ? state.Cartridge.ChrRamSize : state.Cartridge.ChrRomSize;

			if(chrSize != _chrSize) {
				_chrSize = chrSize;

				int index = this.cboChrSelection.SelectedIndex;
				this.cboChrSelection.Items.Clear();
				this.cboChrSelection.Items.Add("PPU: $0000");
				this.cboChrSelection.Items.Add("PPU: $1000");
				for(int i = 0; i < _chrSize / 0x1000; i++) {
					this.cboChrSelection.Items.Add("CHR: $" + (i * 0x1000).ToString("X4"));
				}

				this.cboChrSelection.SelectedIndex = this.cboChrSelection.Items.Count > index && index >= 0 ? index : 0;
				this._chrSelection = this.cboChrSelection.SelectedIndex;
			}
		}

		private void cboChrSelection_DropDown(object sender, EventArgs e)
		{
			UpdateChrBankDropdown();
		}

		private void cboChrSelection_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._chrSelection = this.cboChrSelection.SelectedIndex;
			this.GetData();
			this.RefreshViewer();
		}
	}
}
