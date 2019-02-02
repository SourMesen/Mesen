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
using System.Collections.ObjectModel;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlEventViewerPpuView : BaseControl, ICompactControl
	{
		public event EventHandler OnPictureResized;

		private DebugState _state = new DebugState();
		private Point _lastPos = new Point(-1, -1);
		private bool _needUpdate = false;
		private Bitmap _screenBitmap = null;
		private Bitmap _eventBitmap = null;
		private Bitmap _overlayBitmap = null;
		private Bitmap _displayBitmap = null;
		private byte[] _pictureData = null;
		private Dictionary<int, List<DebugEventInfo>> _debugEventsByCycle = new Dictionary<int, List<DebugEventInfo>>();
		private List<DebugEventInfo> _debugEvents = new List<DebugEventInfo>();
		private Font _overlayFont;
		private double _scale = 1;
		
		public ctrlEventViewerPpuView()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!IsDesignMode) {
				tmrOverlay.Start();
				_overlayFont = new Font(BaseControl.MonospaceFontFamily, 10);
			}
		}

		public void GetData()
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			DebugEventInfo[] eventInfoArray;
			DebugEventInfo[] prevEventInfoArray = new DebugEventInfo[0];

			InteropEmu.DebugGetDebugEvents(false, out _pictureData, out eventInfoArray);
			if(ConfigManager.Config.DebugInfo.EventViewerShowPreviousFrameEvents && (state.PPU.Scanline != -1 || state.PPU.Cycle != 0)) {
				//Get the previous frame's data, too
				InteropEmu.DebugGetDebugEvents(true, out _pictureData, out prevEventInfoArray);
			}

			int currentCycle = (int)((state.PPU.Scanline + 1) * 341 + state.PPU.Cycle);
			var debugEvents = new Dictionary<int, List<DebugEventInfo>>();

			List<DebugEventInfo> eventList = new List<DebugEventInfo>(eventInfoArray.Length+prevEventInfoArray.Length);
			Action<DebugEventInfo> addEvent = (DebugEventInfo eventInfo) => {
				int frameCycle = (eventInfo.Scanline + 1) * 341 + eventInfo.Cycle;

				List<DebugEventInfo> infoList;
				if(!debugEvents.TryGetValue(frameCycle, out infoList)) {
					infoList = new List<DebugEventInfo>();
					debugEvents[frameCycle] = infoList;
				}
				infoList.Add(eventInfo);
				eventList.Add(eventInfo);
			};

			for(int i = 0; i < eventInfoArray.Length; i++) {
				addEvent(eventInfoArray[i]);
			}

			//Show events from the previous frame, too
			for(int i = 0; i < prevEventInfoArray.Length; i++) {
				int frameCycle = (prevEventInfoArray[i].Scanline + 1) * 341 + prevEventInfoArray[i].Cycle;
				if(frameCycle > currentCycle) {
					addEvent(prevEventInfoArray[i]);
				}
			}

			_debugEvents = eventList;
			_debugEventsByCycle = debugEvents;
			_state = state;
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
				int picHeight = (int)_state.PPU.ScanlineCount * 2;
				if(_eventBitmap == null || _eventBitmap.Height != picHeight) {
					_screenBitmap = new Bitmap(682, picHeight);
					_eventBitmap = new Bitmap(682, picHeight);
					_overlayBitmap = new Bitmap(682, picHeight);
					_displayBitmap = new Bitmap(682, picHeight);
				}

				Size picSize = new Size((int)((_eventBitmap.Width * _scale) + 2), (int)((_eventBitmap.Height * _scale) + 2));
				if(picSize != this.picPicture.Size) {
					this.picPicture.Size = picSize;
					this.OnPictureResized?.Invoke(this, EventArgs.Empty);
				}

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

				using(Graphics g = Graphics.FromImage(_screenBitmap)) {
					g.Clear(Color.Gray);
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
					g.ScaleTransform(2, 2);
					g.DrawImageUnscaled(source, 1, 1);

					g.ResetTransform();

					int nmiStart = (int)(_state.PPU.NmiScanline + 1) * 2 + 1;
					int nmiEnd = (int)(_state.PPU.SafeOamScanline + 1) * 2 + 2;
					g.FillRectangle(Brushes.DimGray, 0, nmiStart, 682, nmiEnd - nmiStart);

					g.DrawLine(Pens.Blue, 0, nmiStart, 682, nmiStart);
					g.DrawLine(Pens.Red, 0, nmiEnd, 682, nmiEnd);

					if(_state.PPU.Scanline > 0) {
						int currentScanline = (_state.PPU.Scanline + 1) * 2 + 1;
						g.FillRectangle(Brushes.Yellow, 0, currentScanline, 682, 2);
					}
				}
				
				using(Graphics g = Graphics.FromImage(_eventBitmap)) {
					g.Clear(Color.Transparent);
					DrawEvents(g, colors);
				}
				UpdateDisplay(true);
			} finally {
				handle.Free();
			}
		}

		private void UpdateDisplay(bool forceUpdate)
		{
			if(!_needUpdate && !forceUpdate) {
				return;
			}

			using(Graphics g = Graphics.FromImage(_displayBitmap)) {
				g.DrawImage(_screenBitmap, 0, 0);
				g.DrawImage(_overlayBitmap, 0, 0);
				g.DrawImage(_eventBitmap, 0, 0);

				if(_lastPos.X >= 0) {
					string location = _lastPos.X / 2 + ", " + ((_lastPos.Y / 2) - 1);
					SizeF size = g.MeasureString(location, _overlayFont);
					int x = _lastPos.X + 15;
					int y = _lastPos.Y - (int)size.Height - 5;
					if(x + size.Width > _displayBitmap.Width - 5) {
						x -= (int)size.Width + 20;
					}
					if(y < size.Height + 5) {
						y = _lastPos.Y + 5;
					}

					g.DrawOutlinedString(location, _overlayFont, Brushes.White, Brushes.Black, x, y);
				}
			}

			picPicture.Image = _displayBitmap;
			_needUpdate = false;
		}

		private void UpdateOverlay(Point p)
		{
			int x = (int)(p.X / 2 * 2 / _scale);
			int y = (int)(p.Y / 2 * 2 / _scale);

			if(_lastPos.X == x && _lastPos.Y == y) {
				//Same x,y location, no need to update
				return;
			}

			using(Graphics g = Graphics.FromImage(_overlayBitmap)) {
				g.Clear(Color.Transparent);

				using(Pen bg = new Pen(Color.FromArgb(128, Color.Black))) {
					g.DrawRectangle(bg, x - 1, 0, 3, _overlayBitmap.Height);
					g.DrawRectangle(bg, 0, y - 1, _overlayBitmap.Width, 3);
				}
				using(Pen fg = new Pen(Color.FromArgb(230, Color.Orange))) {
					g.DrawRectangle(fg, x, 0, 1, _overlayBitmap.Height);
					g.DrawRectangle(fg, 0, y, _overlayBitmap.Width, 1);
				}
			}

			_needUpdate = true;
			_lastPos = new Point(x, y);
		}

		private void ClearOverlay()
		{
			using(Graphics g = Graphics.FromImage(_overlayBitmap)) {
				g.Clear(Color.Transparent);
			}
			UpdateDisplay(false);
			_lastPos = new Point(-1, -1);
		}

		private void DrawEvents(Graphics g, List<List<Color>> colors)
		{
			var enumValues = Enum.GetValues(typeof(DebugEventType));
			IGrouping<int, DebugEventInfo>[][] groupedEvents = new IGrouping<int, DebugEventInfo>[enumValues.Length][];
			foreach(DebugEventType eventType in Enum.GetValues(typeof(DebugEventType))) {
				if(ShowEvent(eventType)) {
					List<Color> colorList = colors[(int)eventType];
					groupedEvents[(int)eventType] = _debugEvents.Where((v) => v.Type == eventType).GroupBy((v) => v.Address % colorList.Count).ToArray();
				} else {
					groupedEvents[(int)eventType] = null;
				}
			}

			DrawEvents(g, colors, false, groupedEvents);
			DrawEvents(g, colors, true, groupedEvents);
		}

		private static void DrawEvents(Graphics g, List<List<Color>> colors, bool drawFg, IGrouping<int, DebugEventInfo>[][] groupedEvents)
		{
			int size = drawFg ? 2 : 6;
			int offset = drawFg ? 0 : 2;
			foreach(DebugEventType eventType in Enum.GetValues(typeof(DebugEventType))) {
				if(groupedEvents[(int)eventType] != null) {
					foreach(var eventGroup in groupedEvents[(int)eventType]) {
						List<Rectangle> rects = new List<Rectangle>(eventGroup.Count());
						foreach(DebugEventInfo evt in eventGroup) {
							rects.Add(new Rectangle(evt.Cycle * 2 - offset, evt.Scanline * 2 - offset + 2, size, size));
						}

						List<Color> colorList = colors[(int)eventType];
						Color color = colorList[eventGroup.Key];
						using(Brush b = new SolidBrush(drawFg ? color : ControlPaint.Dark(color))) {
							g.FillRectangles(b, rects.ToArray());
						}
					}
				}
			}
		}

		private int _lastKey = -1;
		private frmCodeTooltip _tooltip = null;
		private void picPicture_MouseMove(object sender, MouseEventArgs e)
		{
			int cycle = e.X * 341 / (picPicture.Width - 2);
			int scanline = e.Y * (int)_state.PPU.ScanlineCount / (picPicture.Height - 2) - 1;
			
			int[] offsets = new int[3] { 0, -1, 1 };

			for(int y = 0; y < 3; y++) {
				for(int x = 0; x < 3; x++) {
					int key = (scanline + offsets[y] + 1) * 341 + cycle + offsets[x];

					List<DebugEventInfo> eventList;
					if(_debugEventsByCycle.TryGetValue(key, out eventList)) {
						foreach(DebugEventInfo debugEvent in eventList) {
							if(ShowEvent(debugEvent.Type)) {
								if(key != _lastKey) {
									ResetTooltip();

									Dictionary<string, string> values = new Dictionary<string, string>() {
									{ "Type", ResourceHelper.GetEnumText(debugEvent.Type) },
									{ "Scanline", debugEvent.Scanline.ToString() },
									{ "Cycle", debugEvent.Cycle.ToString() },
									{ "PC", "$" + debugEvent.ProgramCounter.ToString("X4") },
								};

									switch(debugEvent.Type) {
										case DebugEventType.MapperRegisterRead:
										case DebugEventType.MapperRegisterWrite:
										case DebugEventType.PpuRegisterRead:
										case DebugEventType.PpuRegisterWrite:
											values["Register"] = "$" + debugEvent.Address.ToString("X4");
											values["Value"] = "$" + debugEvent.Value.ToString("X2");

											if(debugEvent.PpuLatch >= 0) {
												values["2nd Write"] = debugEvent.PpuLatch == 0 ? "false" : "true";
											}
											break;

										case DebugEventType.Breakpoint:
											ReadOnlyCollection<Breakpoint> breakpoints = BreakpointManager.Breakpoints;
											if(debugEvent.BreakpointId >= 0 && debugEvent.BreakpointId < breakpoints.Count) {
												Breakpoint bp = breakpoints[debugEvent.BreakpointId];
												values["BP Type"] = bp.ToReadableType();
												values["BP Addresses"] = bp.GetAddressString(true);
												if(bp.Condition.Length > 0) {
													values["BP Condition"] = bp.Condition;
												}
											}
											break;
									}

									UpdateOverlay(new Point((int)(debugEvent.Cycle * 2 * _scale), (int)((debugEvent.Scanline + 1) * 2 * _scale)));

									Form parentForm = this.FindForm();
									_tooltip = new frmCodeTooltip(parentForm, values, null, null, null, 10);
									_tooltip.FormClosed += (s, evt) => { _tooltip = null; };
									Point location = PointToScreen(e.Location);
									location.Offset(10, 10);
									_tooltip.SetFormLocation(location, this);
									_lastKey = key;
								}

								//Found a matching write to display, stop processing
								return;
							}
						}
					}
				}
			}

			UpdateOverlay(e.Location);

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
			ClearOverlay();
		}

		private void tmrOverlay_Tick(object sender, EventArgs e)
		{
			UpdateDisplay(false);
		}

		public Size GetCompactSize(bool includeMargins)
		{
			return picPicture.Size + picPicture.Margin.Size;
		}

		public void ScaleImage(double scale)
		{
			_scale *= scale;
			picPicture.Size = new Size((int)(picPicture.Width * scale), (int)(picPicture.Height * scale));
		}
	}
}
