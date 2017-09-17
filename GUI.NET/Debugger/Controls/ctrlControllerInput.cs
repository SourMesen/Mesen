using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	class ctrlControllerInput : Control
	{
		[Flags]
		private enum Buttons
		{
			None = 0x00,
			A = 0x01,
			B = 0x02,
			Select = 0x04,
			Start = 0x08,
			Up = 0x10,
			Down = 0x20,
			Left = 0x40,
			Right = 0x80
		}

		private Buttons _buttonState = 0;
		private Buttons _highlightedButtons = 0;

		public ctrlControllerInput()
		{
			this.DoubleBuffered = true;
			this.ResizeRedraw = true;
		}
		
		public int PlayerNumber { get; set; }

		private Buttons ButtonState
		{
			get { return _buttonState; }
			set
			{
				_buttonState = value;
				this.Invalidate();
				InteropEmu.DebugSetInputOverride(this.PlayerNumber, (int)_buttonState);
			}
		}

		public void UpdateButtons(byte buttonState)
		{
			if((byte)this.ButtonState != buttonState) {
				this.ButtonState = (Buttons)buttonState;
				this.Invalidate();
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			this.ButtonState = ToggleButtonState(ButtonState, e.Location);
			this.Invalidate();
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			this.ButtonState = ToggleButtonState(ButtonState, e.Location);
			this.Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			_highlightedButtons = Buttons.None;
			_highlightedButtons = ToggleButtonState(_highlightedButtons, e.Location);
			this.Cursor = _highlightedButtons != Buttons.None ? Cursors.Hand : Cursors.Default;
			this.Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			_highlightedButtons = Buttons.None;
			this.Cursor = Cursors.Default;
			this.Invalidate();
		}

		private Buttons ToggleButtonState(Buttons state, Point mouseLoc)
		{
			Point location = new Point((int)(mouseLoc.X/_xFactor), (int)(mouseLoc.Y/_yFactor));
			
			Rectangle upButton = new Rectangle(6, 2, 4, 4);
			Rectangle downButton = new Rectangle(6, 10, 4, 4);
			Rectangle leftButton = new Rectangle(2, 6, 4, 4);
			Rectangle rightButton = new Rectangle(10, 6, 4, 4);

			Rectangle selectButton = new Rectangle(16, 8, 5, 5);
			Rectangle startButton = new Rectangle(23, 8, 5, 5);
			Rectangle aButton = new Rectangle(37, 7, 5, 5);
			Rectangle bButton = new Rectangle(30, 7, 5, 5);

			if(upButton.Contains(location)) {
				state ^= Buttons.Up;
			} else if(downButton.Contains(location)) {
				state ^= Buttons.Down;
			} else if(leftButton.Contains(location)) {
				state ^= Buttons.Left;
			} else if(rightButton.Contains(location)) {
				state ^= Buttons.Right;
			}else if(selectButton.Contains(location)) {
				state ^= Buttons.Select;
			}else if(startButton.Contains(location)) {
				state ^= Buttons.Start;
			}else if(aButton.Contains(location)) {
				state ^= Buttons.A;
			}else if(bButton.Contains(location)) {
				state ^= Buttons.B;
			}
			return state;
		}
		
		private Brush GetButtonColor(Buttons button)
		{
			if(_highlightedButtons.HasFlag(button)) {
				if(this.ButtonState.HasFlag(button)) {
					return Brushes.LightGreen;
				} else {
					return Brushes.White;
				}
			} else {
				if(this.ButtonState.HasFlag(button)) {
					return Brushes.LightGreen;
				} else {
					return Brushes.DarkSlateGray;
				}
			}
		}

		float _xFactor = 1;
		float _yFactor = 1;
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			_xFactor = (float)this.Width / 44;
			_yFactor = (float)this.Height / 16;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.Clear(Color.LightGray);
			
			e.Graphics.ScaleTransform(_xFactor, _yFactor);

			e.Graphics.DrawRectangle(Pens.DarkSlateGray, 0, 0, 44, 16);
			e.Graphics.FillRectangle(Brushes.DarkSlateGray, 6, 6, 4, 4);

			if(PlayerNumber == 0) {
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 22, 2, 1, 5);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 21, 6, 3, 1);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 21, 3, 1, 1);
			} else if(PlayerNumber == 1) {
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 21, 2, 3, 1);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 23, 3, 1, 1);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 22, 4, 1, 1);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 21, 5, 1, 1);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 21, 6, 3, 1);
			} else if(PlayerNumber == 2) {
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 21, 2, 3, 1);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 22, 4, 2, 1);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 21, 6, 3, 1);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 23, 2, 1, 5);
			} else if(PlayerNumber == 3) {
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 21, 2, 1, 3);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 23, 2, 1, 5);
				e.Graphics.FillRectangle(Brushes.DarkSlateGray, 22, 4, 1, 1);
			}

			e.Graphics.FillRectangle(GetButtonColor(Buttons.Up), 6, 2, 4, 4);
			e.Graphics.FillRectangle(GetButtonColor(Buttons.Down), 6, 10, 4, 4);

			e.Graphics.FillRectangle(GetButtonColor(Buttons.Left), 2, 6, 4, 4);
			e.Graphics.FillRectangle(GetButtonColor(Buttons.Right), 10, 6, 4, 4);

			e.Graphics.FillRectangle(GetButtonColor(Buttons.Select), 16, 9, 5, 3);
			e.Graphics.FillRectangle(GetButtonColor(Buttons.Start), 23, 9, 5, 3);

			e.Graphics.FillRectangle(GetButtonColor(Buttons.B), 30, 8, 5, 3);
			e.Graphics.FillRectangle(GetButtonColor(Buttons.B), 31, 7, 3, 5);

			e.Graphics.FillRectangle(GetButtonColor(Buttons.A), 37, 8, 5, 3);
			e.Graphics.FillRectangle(GetButtonColor(Buttons.A), 38, 7, 3, 5);
		}
	}
}
