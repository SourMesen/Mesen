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
	public partial class ctrlPpuWriteViewer : BaseControl
	{
		private byte[] _pictureData = null;
		private Dictionary<int, PpuRegisterWriteInfo> _ppuRegisterWrites = new Dictionary<int, PpuRegisterWriteInfo>();

		public ctrlPpuWriteViewer()
		{
			InitializeComponent();
		}
		
		public void GetData()
		{
			PpuRegisterWriteInfo[] writeInfoArray;
			InteropEmu.DebugGetPpuRegisterWriteData(out _pictureData, out writeInfoArray);

			var writes = new Dictionary<int, PpuRegisterWriteInfo>();
			for(int i = 0; i < writeInfoArray.Length; i++) {
				writes[(writeInfoArray[i].Scanline + 1) * 341 + writeInfoArray[i].Cycle] = writeInfoArray[i];
			}
			_ppuRegisterWrites = writes;
		}

		public void RefreshViewer()
		{
			GCHandle handle = GCHandle.Alloc(this._pictureData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(256, 240, 256*4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap target = new Bitmap(682, 524);

				Brush[] brushes = new Brush[8] { Brushes.MediumPurple, Brushes.LightBlue, Brushes.Transparent, Brushes.Orange, Brushes.Yellow, Brushes.LightPink, Brushes.LightGreen, Brushes.Cyan };
				using(Graphics g = Graphics.FromImage(target)) {
					g.Clear(Color.DimGray);
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.ScaleTransform(2, 2);
					g.DrawImageUnscaled(source, 1, 0);

					g.ResetTransform();
					foreach(var kvp in _ppuRegisterWrites) {
						using(Brush b = new SolidBrush(ControlPaint.Dark((brushes[kvp.Value.Address & 0x07] as SolidBrush).Color))) {
							int x = kvp.Value.Cycle * 2;
							int y = kvp.Value.Scanline >= 0 ? kvp.Value.Scanline * 2 : 261 * 2;
							g.FillRectangle(b, x - 2, y - 2, 6, 6);
						}
					}
					foreach(var kvp in _ppuRegisterWrites) {
						int x = kvp.Value.Cycle * 2;
						int y = kvp.Value.Scanline >= 0 ? kvp.Value.Scanline * 2 : 261 * 2;
						g.FillRectangle(brushes[kvp.Value.Address & 0x07], x, y, 2, 2);
					}
				}
				this.picPicture.Image = target;
			} finally {
				handle.Free();
			}
		}

		private int _lastKey = -1;
		private frmCodeTooltip _tooltip = null;
		private void picPicture_MouseMove(object sender, MouseEventArgs e)
		{
			int cycle = e.X * 341 / picPicture.Image.Width;
			int scanline = e.Y * 262 / picPicture.Image.Height;
			if(scanline == 261) {
				scanline = -1;
			}

			int[] offsets = new int[3] { 0, -1, 1 };

			for(int y = 0; y < 3; y++) {
				for(int x = 0; x < 3; x++) {
					int key = (scanline + offsets[y] + 1) * 341 + cycle + offsets[x];

					PpuRegisterWriteInfo writeInfo;
					if(_ppuRegisterWrites.TryGetValue(key, out writeInfo)) {
						if(key != _lastKey) {
							ResetTooltip();

							Dictionary<string, string> values = new Dictionary<string, string>() {
								{ "Register", "$" + (0x2000 + writeInfo.Address).ToString("X4") },
								{ "Value", "$" + writeInfo.Value.ToString("X2") },
								{ "Scanline", writeInfo.Scanline.ToString() },
								{ "Cycle", writeInfo.Cycle.ToString() },
							};

							_tooltip = new frmCodeTooltip(values);
							Point location = PointToScreen(e.Location);
							location.Offset(10, 10);
							_tooltip.Location = location;
							_tooltip.Show();
							_lastKey = key;
						}

						//Found a matching write to display, stop processing
						return;
					}
				}
			}

			//No match found, make sure any existing tooltip is closed
			ResetTooltip();
		}

		private void ResetTooltip()
		{
			if(_tooltip != null) {
				_tooltip.Close();
				_tooltip = null;
			}
			_lastKey = -1;
		}

		private void picPicture_MouseLeave(object sender, EventArgs e)
		{
			ResetTooltip();
		}
	}
}
