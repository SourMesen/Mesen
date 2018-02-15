using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	public class ctrlCodeScrollbar : Control
	{
		public event EventHandler ValueChanged;

		private IScrollbarColorProvider _colorProvider = null;
		public IScrollbarColorProvider ColorProvider
		{
			get { return _colorProvider; }
			set { _colorProvider = value; this.Invalidate(); }
		}

		public ctrlCodeScrollbar()
		{
			this.DoubleBuffered = true;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			int left = e.ClipRectangle.Left;
			int width = e.ClipRectangle.Width;
			e.Graphics.FillRectangle(Brushes.DimGray, e.ClipRectangle);

			float startPos = (float)this.Value / this.Maximum;
			if(this.ColorProvider != null) {
				Color prevBgColor = Color.White;
				int drawHeight = 0;
				for(int i = 0; i < this.Height; i++) {
					float top = e.ClipRectangle.Top + i;
					float position = (float)i / this.Height;
					Color bgColor = this.ColorProvider.GetBackgroundColor(position);
					if(bgColor != prevBgColor && i > 0) {
						using(Brush brush = new SolidBrush(prevBgColor)) {
							e.Graphics.FillRectangle(brush, left + 1, top - drawHeight, width - 1, drawHeight);
							drawHeight = 0;
						}
					}
					drawHeight++;

					if(bgColor != Color.Transparent) {
						prevBgColor = bgColor;
					}
				}
				using(Brush brush = new SolidBrush(prevBgColor)) {
					e.Graphics.FillRectangle(brush, left + 1, e.ClipRectangle.Bottom - drawHeight, width - 1, drawHeight);
				}
			} else {
				e.Graphics.FillRectangle(Brushes.White, left + 1, e.ClipRectangle.Top, width - 1, e.ClipRectangle.Height);
			}

			float highlightTop = e.ClipRectangle.Top + e.ClipRectangle.Height * startPos - HighlightOffset;
			using(SolidBrush brush = new SolidBrush(Color.FromArgb(120, 220, 220, 255))) {
				e.Graphics.FillRectangle(brush, left + 1, highlightTop, width, HighlightHeight);
				e.Graphics.DrawRectangle(Pens.DarkSlateGray, left + 1, highlightTop, width - 2, HighlightHeight);
				e.Graphics.DrawRectangle(Pens.Gray, left + 2, highlightTop + 1, width - 4, HighlightHeight - 2);
			}

			if(this.ColorProvider != null) {
				int selectedIndex = this.ColorProvider.GetSelectedLine();
				if(selectedIndex >= 0) {
					int selectedTop = selectedIndex * this.Height / this.Maximum;
					e.Graphics.FillRectangle(Brushes.LightBlue, left + 1, selectedTop - 1, width - 2, 3);
					e.Graphics.DrawRectangle(Pens.DimGray, left + 1, selectedTop - 1, width - 2, 3);
				}

				int activeIndex = this.ColorProvider.GetActiveLine();
				if(activeIndex >= 0) {
					int activeTop = activeIndex * this.Height / this.Maximum;
					e.Graphics.FillRectangle(Brushes.Yellow, left + 1, activeTop - 1, width - 2, 3);
					e.Graphics.DrawRectangle(Pens.DimGray, left + 1, activeTop - 1, width - 2, 3);
				}

				int linesPerPixel = (int)Math.Ceiling((float)this.Maximum / this.Height);
				for(int i = 0; i < e.ClipRectangle.Height; i++) {
					float top = e.ClipRectangle.Top + i;
					float position = (float)i / this.Height;
					Color markerColor = this.ColorProvider.GetMarkerColor(position, linesPerPixel);
					if(markerColor != Color.Transparent) {
						using(Brush brush = new SolidBrush(markerColor)) {
							e.Graphics.FillRectangle(brush, left + 3, top - 2, 5, 5);
						}
					}
				}
			}
		}

		private bool _mouseDown = false;
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.UpdatePosition(e.Y);
			this._mouseDown = true;

			if(_codeTooltip != null) {
				_codeTooltip.Close();
				_codeTooltip = null;
			}
			_lastPreviewScrollPosition = -1;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this._mouseDown = false;
		}

		frmCodeTooltip _codeTooltip = null;
		int _lastPreviewScrollPosition = -1;
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if(this._mouseDown) {
				this.UpdatePosition(e.Y);
			} else {
				if(this.ColorProvider != null) {
					int scrollPosition = Math.Max(0, (e.Y - this.Top) * this.Maximum / this.Height);
					if(_lastPreviewScrollPosition != scrollPosition) {
						Point p = this.PointToScreen(new Point(this.ClientRectangle.Right, e.Y));
						if(_codeTooltip == null) {
							_codeTooltip = this.ColorProvider.GetPreview(scrollPosition);
						} else {
							_codeTooltip.ScrollToLineIndex(scrollPosition);
						}
						if(_codeTooltip != null) {
							_codeTooltip.Left = p.X + 5;
							_codeTooltip.Top = p.Y;
							_codeTooltip.Show();
						}
						_lastPreviewScrollPosition = scrollPosition;
					}
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if(_codeTooltip != null) {
				_codeTooltip.Close();
				_codeTooltip = null;
			}
			_lastPreviewScrollPosition = -1;
		}

		private int HighlightHeight { get { return Math.Max(6, (int)(((float)this.VisibleLineCount / this.Maximum) * this.Height)); } }
		private int HighlightOffset
		{
			get
			{
				int highlightHeight = (int)(((float)this.VisibleLineCount / this.Maximum) * this.Height);
				if(HighlightHeight - highlightHeight > 0) {
					return (HighlightHeight - highlightHeight) / 2;
				} else {
					return 0;
				}
			}
		}

		private void UpdatePosition(int y)
		{
			this.Value = Math.Max(0 , y - HighlightHeight / 2 + HighlightOffset - this.Top) * this.Maximum / this.Height;
		}

		private int _value = 0;
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if(this._value != value) {
					this._value = value;
					this.ValueChanged?.Invoke(this, EventArgs.Empty);

					this.Invalidate();
				}
			}
		}

		private int _maximum = 1;
		public int Maximum
		{
			get { return Math.Max(1, this._maximum); }
			set { this._maximum = value; this.Invalidate(); }
		}

		public int VisibleLineCount { get; set; } = 1;
		public int LargeChange { get; set; } = 1;
	}

	public interface IScrollbarColorProvider
	{
		Color GetBackgroundColor(float position);
		Color GetMarkerColor(float position, int linesPerPixel);
		int GetActiveLine();
		int GetSelectedLine();
		frmCodeTooltip GetPreview(int lineIndex);
	}
}
