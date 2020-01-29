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
using System.Drawing.Imaging;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlEventViewerPpuView : BaseControl
	{
		private int _baseWidth = 341 * 2;
		private UInt32 _scanlineCount = 262;

		private Point _lastPos = new Point(-1, -1);
		private bool _needUpdate = false;
		private Bitmap _screenBitmap = null;
		private Bitmap _overlayBitmap = null;
		private Bitmap _displayBitmap = null;
		private UInt32[] _pictureData = null;
		private Font _overlayFont;
		
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
			EventViewerDisplayOptions options = GetInteropOptions();
			_scanlineCount = InteropEmu.TakeEventSnapshot(options);
		}

		public static bool ShowEvent(DebugEventInfo evt)
		{
			DebugInfo cfg = ConfigManager.Config.DebugInfo;
			switch(evt.Type) {
				case DebugEventType.PpuRegisterWrite: {
					int register = evt.Address & 0x07;
					switch(register) {
						case 0: return cfg.EventViewerShowPpuWrite2000;
						case 1: return cfg.EventViewerShowPpuWrite2001;
						case 3: return cfg.EventViewerShowPpuWrite2003;
						case 4: return cfg.EventViewerShowPpuWrite2004;
						case 5: return cfg.EventViewerShowPpuWrite2005;
						case 6: return cfg.EventViewerShowPpuWrite2006;
						case 7: return cfg.EventViewerShowPpuWrite2007;
						default: return false;
					}
				}

				case DebugEventType.PpuRegisterRead: {
					int register = evt.Address & 0x07;
					switch(register) {
						case 2: return cfg.EventViewerShowPpuRead2002;
						case 4: return cfg.EventViewerShowPpuRead2004;
						case 7: return cfg.EventViewerShowPpuRead2007;
						default: return false;
					}
				}

				case DebugEventType.MapperRegisterWrite: return cfg.EventViewerShowMapperRegisterWrites;
				case DebugEventType.MapperRegisterRead: return cfg.EventViewerShowMapperRegisterReads;
				case DebugEventType.Nmi: return cfg.EventViewerShowNmi;
				case DebugEventType.Irq: return cfg.EventViewerShowIrq;
				case DebugEventType.SpriteZeroHit: return cfg.EventViewerShowSpriteZeroHit;
				case DebugEventType.Breakpoint: return cfg.EventViewerShowMarkedBreakpoints;
				case DebugEventType.DmcDmaRead: return cfg.EventViewerShowDmcDmaReads;
			}
			return false;
		}

		public void RefreshViewer()
		{
			EventViewerDisplayOptions options = GetInteropOptions();
			_pictureData = InteropEmu.GetEventViewerOutput(_scanlineCount, options);

			int picHeight = (int)_scanlineCount * 2;
			if(_screenBitmap == null || _screenBitmap.Height != picHeight) {
				_screenBitmap = new Bitmap(_baseWidth, picHeight, PixelFormat.Format32bppPArgb);
				_overlayBitmap = new Bitmap(_baseWidth, picHeight, PixelFormat.Format32bppPArgb);
				_displayBitmap = new Bitmap(_baseWidth, picHeight, PixelFormat.Format32bppPArgb);
			}

			GCHandle handle = GCHandle.Alloc(this._pictureData, GCHandleType.Pinned);
			try {
				Bitmap source = new Bitmap(_baseWidth, (int)_scanlineCount * 2, _baseWidth * 4, PixelFormat.Format32bppPArgb, handle.AddrOfPinnedObject());
				using(Graphics g = Graphics.FromImage(_screenBitmap)) {
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
					g.DrawImageUnscaled(source, 0, 0);
				}
			} finally {
				handle.Free();
			}

			UpdateDisplay(true);
		}

		private void UpdateDisplay(bool forceUpdate)
		{
			if(!_needUpdate && !forceUpdate) {
				return;
			}

			using(Graphics g = Graphics.FromImage(_displayBitmap)) {
				g.DrawImage(_screenBitmap, 0, 0);
				g.DrawImage(_overlayBitmap, 0, 0);

				if(_lastPos.X >= 0) {
					string location = _lastPos.X + ", " + (_lastPos.Y - 1);
					SizeF size = g.MeasureString(location, _overlayFont);
					int x = _lastPos.X + 5;
					int y = _lastPos.Y - (int)size.Height / 2 - 5;

					if(x * 2 - picViewer.ScrollOffsets.X / picViewer.ImageScale + size.Width > (picViewer.Width / picViewer.ImageScale) - 5) {
						x -= (int)size.Width / 2 + 10;
					}
					if(y * 2 - picViewer.ScrollOffsets.Y / picViewer.ImageScale < size.Height + 5) {
						y = _lastPos.Y + 5;
					}

					g.DrawOutlinedString(location, _overlayFont, Brushes.Black, Brushes.White, x * 2, y * 2);
				}
			}

			picViewer.ImageSize = new Size(_baseWidth, (int)_scanlineCount * 2);
			picViewer.Image = _displayBitmap;
			_needUpdate = false;
		}
		
		private Point GetCycleScanline(Point location)
		{
			return new Point(
				((location.X & ~0x01) / picViewer.ImageScale) / 2,
				((location.Y & ~0x01) / picViewer.ImageScale) / 2
			);
		}

		private void UpdateOverlay(Point p)
		{
			Point pos = GetCycleScanline(p);

			if(_lastPos == pos) {
				//Same x,y location, no need to update
				return;
			}

			using(Graphics g = Graphics.FromImage(_overlayBitmap)) {
				g.Clear(Color.Transparent);
				using(Pen bg = new Pen(Color.FromArgb(128, Color.LightGray))) {
					g.DrawRectangle(bg, pos.X * 2 - 1, 0, 3, _overlayBitmap.Height);
					g.DrawRectangle(bg, 0, pos.Y * 2 - 1, _overlayBitmap.Width, 3);
				}
			}

			_needUpdate = true;
			_lastPos = pos;
		}

		private void ClearOverlay()
		{
			using(Graphics g = Graphics.FromImage(_overlayBitmap)) {
				g.Clear(Color.Transparent);
			}
			UpdateDisplay(false);
			_lastPos = new Point(-1, -1);
		}

		private EventViewerDisplayOptions GetInteropOptions()
		{
			DebugInfo cfg = ConfigManager.Config.DebugInfo;
			return new EventViewerDisplayOptions() {
				ShowPpuRegisterWrites = new byte[8] {
					(byte)(cfg.EventViewerShowPpuWrite2000 ? 1 : 0),
					(byte)(cfg.EventViewerShowPpuWrite2001 ? 1 : 0),
					0,
					(byte)(cfg.EventViewerShowPpuWrite2003 ? 1 : 0),
					(byte)(cfg.EventViewerShowPpuWrite2004 ? 1 : 0),
					(byte)(cfg.EventViewerShowPpuWrite2005 ? 1 : 0),
					(byte)(cfg.EventViewerShowPpuWrite2006 ? 1 : 0),
					(byte)(cfg.EventViewerShowPpuWrite2007 ? 1 : 0),
				},
				ShowPpuRegisterReads = new byte[8] {
					0,
					0,
					(byte)(cfg.EventViewerShowPpuRead2002 ? 1 : 0),
					0,
					(byte)(cfg.EventViewerShowPpuRead2004 ? 1 : 0),
					0,
					(byte)(cfg.EventViewerShowPpuRead2007 ? 1 : 0),
					0
				},
				ShowMapperRegisterWrites = cfg.EventViewerShowMapperRegisterWrites,
				ShowMapperRegisterReads = cfg.EventViewerShowMapperRegisterReads,
				MapperRegisterWriteColor = (uint)cfg.EventViewerMapperRegisterWriteColor.Color.ToArgb(),
				MapperRegisterReadColor = (uint)cfg.EventViewerMapperRegisterReadColor.Color.ToArgb(),
				ShowNmi = cfg.EventViewerShowNmi,
				ShowIrq = cfg.EventViewerShowIrq,
				ShowDmcDmaReads = cfg.EventViewerShowDmcDmaReads,
				ShowSpriteZeroHit = cfg.EventViewerShowSpriteZeroHit,
				ShowMarkedBreakpoints = cfg.EventViewerShowMarkedBreakpoints,
				ShowPreviousFrameEvents = cfg.EventViewerShowPreviousFrameEvents,
				ShowNtscBorders = cfg.EventViewerShowNtscBorders,
				IrqColor = (uint)cfg.EventViewerIrqColor.Color.ToArgb(),
				NmiColor = (uint)cfg.EventViewerNmiColor.Color.ToArgb(),
				DmcDmaReadColor = (uint)cfg.EventViewerDmcDmaReadColor.Color.ToArgb(),
				SpriteZeroHitColor = (uint)cfg.EventViewerSpriteZeroHitColor.Color.ToArgb(),
				BreakpointColor = (uint)cfg.EventViewerBreakpointColor.Color.ToArgb(),
				PpuRegisterWriteColor = new uint[8] {
					(uint)cfg.EventViewerPpuRegisterWrite2000Color.Color.ToArgb(),
					(uint)cfg.EventViewerPpuRegisterWrite2001Color.Color.ToArgb(),
					0,
					(uint)cfg.EventViewerPpuRegisterWrite2003Color.Color.ToArgb(),
					(uint)cfg.EventViewerPpuRegisterWrite2004Color.Color.ToArgb(),
					(uint)cfg.EventViewerPpuRegisterWrite2005Color.Color.ToArgb(),
					(uint)cfg.EventViewerPpuRegisterWrite2006Color.Color.ToArgb(),
					(uint)cfg.EventViewerPpuRegisterWrite2007Color.Color.ToArgb()
				},
				PpuRegisterReadColors = new uint[8] {
					0,
					0,
					(uint)cfg.EventViewerPpuRegisterRead2002Color.Color.ToArgb(),
					0,
					(uint)cfg.EventViewerPpuRegisterRead2004Color.Color.ToArgb(),
					0,
					(uint)cfg.EventViewerPpuRegisterRead2007Color.Color.ToArgb(),
					0
				},
			};
		}

		private frmCodeTooltip _tooltip = null;
		private void picViewer_MouseMove(object sender, MouseEventArgs e)
		{
			Point pos = GetCycleScanline(e.Location);
			if(_lastPos == pos) {
				return;
			}

			EventViewerDisplayOptions options = GetInteropOptions();
			DebugEventInfo debugEvent = new DebugEventInfo();
			InteropEmu.GetEventViewerEvent(ref debugEvent, (Int16)(pos.Y - 1), (UInt16)pos.X, options);
			if(debugEvent.ProgramCounter == 0xFFFFFFFF) {
				ResetTooltip();
				UpdateOverlay(e.Location);
				return;
			}

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

				case DebugEventType.DmcDmaRead:
					values["Address"] = "$" + debugEvent.Address.ToString("X4");
					values["Value"] = "$" + debugEvent.Value.ToString("X2");
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

			ResetTooltip();
			UpdateOverlay(new Point((int)(debugEvent.Cycle * 2 * picViewer.ImageScale), (int)((debugEvent.Scanline + 1) * 2 * picViewer.ImageScale)));

			Form parentForm = this.FindForm();
			_tooltip = new frmCodeTooltip(parentForm, values, null, null, null, 10);
			_tooltip.FormClosed += (s, evt) => { _tooltip = null; };
			Point location = Control.MousePosition;
			location.Offset(10, 10);
			_tooltip.SetFormLocation(location, this);
		}

		private void ResetTooltip()
		{
			if(_tooltip != null) {
				_tooltip.Close();
				_tooltip = null;
			}
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

		public void ZoomIn()
		{
			picViewer.ZoomIn();
		}

		public void ZoomOut()
		{
			picViewer.ZoomOut();
		}
	}
}
