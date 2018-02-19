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
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlEventViewerPpuView : BaseControl
	{
		private UInt32 _scanlineCount = 262;
		private byte[] _pictureData = null;
		private Dictionary<int, DebugEventInfo> _debugEvents = new Dictionary<int, DebugEventInfo>();

		public ctrlEventViewerPpuView()
		{
			InitializeComponent();
		}
		
		public void GetData()
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			_scanlineCount = state.PPU.ScanlineCount;

			DebugEventInfo[] eventInfoArray;
			InteropEmu.DebugGetDebugEvents(out _pictureData, out eventInfoArray);
			var debugEvents = new Dictionary<int, DebugEventInfo>();
			for(int i = 0; i < eventInfoArray.Length; i++) {
				debugEvents[(eventInfoArray[i].Scanline + 1) * 341 + eventInfoArray[i].Cycle] = eventInfoArray[i];
			}
			_debugEvents = debugEvents;
		}

		private bool ShowEvent(DebugEventType type)
		{
			switch(type) {
				case DebugEventType.PpuRegisterWrite: return ConfigManager.Config.DebugInfo.EventViewerShowPpuRegisterWrites;
				case DebugEventType.PpuRegisterRead: return ConfigManager.Config.DebugInfo.EventViewerShowPpuRegisterReads;
				case DebugEventType.MapperRegisterWrite: return ConfigManager.Config.DebugInfo.EventViewerShowMapperRegisterWrites;
				case DebugEventType.MapperRegisterRead: return ConfigManager.Config.DebugInfo.EventViewerShowMapperRegisterReads;
				case DebugEventType.Nmi: return ConfigManager.Config.DebugInfo.EventViewerShowNmi;
				case DebugEventType.Irq: return ConfigManager.Config.DebugInfo.EventViewerShowIrq;
				case DebugEventType.SpriteZeroHit: return ConfigManager.Config.DebugInfo.EventViewerShowSpriteZeroHit;
				case DebugEventType.Breakpoint: return ConfigManager.Config.DebugInfo.EventViewerShowMarkedBreakpoints;
			}
			return false;
		}

		public void RefreshViewer()
		{
			GCHandle handle = GCHandle.Alloc(this._pictureData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(256, 240, 256*4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
				Bitmap target = new Bitmap(682, (int)_scanlineCount * 2);
				this.picPicture.Width = target.Width + 2;
				this.picPicture.Height = target.Height + 2;
				this.Width = this.picPicture.Width + 2;
				this.Height = this.picPicture.Height + 2;

				var d = ConfigManager.Config.DebugInfo;
				
				List<List<Color>> colors = new List<List<Color>> {
					null, //None
					new List<Color>(d.EventViewerPpuRegisterWriteColors.Select((c) => (Color)c)), //PpuRegisterWrite
					new List<Color>(d.EventViewerPpuRegisterReadColors.Select((c) => (Color)c)), //PpuRegisterRead
					new List<Color> { d.EventViewerMapperRegisterWriteColor }, //MapperRegisterWrite
					new List<Color> { d.EventViewerMapperRegisterReadColor }, //MapperRegisterRead
					new List<Color> { d.EventViewerNmiColor }, //Nmi
					new List<Color> { d.EventViewerIrqColor }, //Irq
					new List<Color> { d.EventViewerSpriteZeroHitColor }, //SpriteZeroHit
					new List<Color> { d.EventViewerBreakpointColor }, //Breakpoint
				};
				
				using(Graphics g = Graphics.FromImage(target)) {
					g.Clear(Color.DimGray);
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.ScaleTransform(2, 2);
					g.DrawImageUnscaled(source, 1, 1);

					g.ResetTransform();
					foreach(var kvp in _debugEvents) {
						if(ShowEvent(kvp.Value.Type)) {
							List<Color> colorList = colors[(int)kvp.Value.Type];
							Color color = colorList[kvp.Value.Address % colorList.Count];
							using(Brush b = new SolidBrush(ControlPaint.Dark(color))) {
								g.FillRectangle(b, kvp.Value.Cycle * 2 - 2, kvp.Value.Scanline * 2, 6, 6);
							}
						}
					}
					foreach(var kvp in _debugEvents) {
						if(ShowEvent(kvp.Value.Type)) {
							List<Color> colorList = colors[(int)kvp.Value.Type];
							Color color = colorList[kvp.Value.Address % colorList.Count];
							using(Brush b = new SolidBrush(color)) {
								g.FillRectangle(b, kvp.Value.Cycle * 2, kvp.Value.Scanline * 2 + 2, 2, 2);
							}
						}
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
			int cycle = e.X * 341 / (picPicture.Width - 2);
			int scanline = e.Y * (int)_scanlineCount / (picPicture.Height - 2) - 1;
			
			int[] offsets = new int[3] { 0, -1, 1 };

			for(int y = 0; y < 3; y++) {
				for(int x = 0; x < 3; x++) {
					int key = (scanline + offsets[y] + 1) * 341 + cycle + offsets[x];

					DebugEventInfo debugEvent;
					if(_debugEvents.TryGetValue(key, out debugEvent)) {
						if(ShowEvent(debugEvent.Type)) {
							if(key != _lastKey) {
								ResetTooltip();

								Dictionary<string, string> values = new Dictionary<string, string>() {
								{ "Type", ResourceHelper.GetEnumText(debugEvent.Type) },
								{ "Scanline", debugEvent.Scanline.ToString() },
								{ "Cycle", debugEvent.Cycle.ToString() },
							};

								switch(debugEvent.Type) {
									case DebugEventType.MapperRegisterRead:
									case DebugEventType.MapperRegisterWrite:
									case DebugEventType.PpuRegisterRead:
									case DebugEventType.PpuRegisterWrite:
										values["Register"] = "$" + debugEvent.Address.ToString("X4");
										values["Value"] = "$" + debugEvent.Value.ToString("X2");
										break;
								}

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
