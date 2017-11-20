using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	public partial class ctrlStandardController : BaseInputConfigControl
	{
		private bool _isSnesController = false;

		public ctrlStandardController(KeyMappings mappings, int portNumber, ConsoleType consoleType, InteropEmu.ControllerType controllerType) : base(mappings)
		{
			InitializeComponent();
			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime) {
				picBackground.Resize += picBackground_Resize;
				UpdateBackground();

				IsSnesController = controllerType == InteropEmu.ControllerType.SnesController;
				ShowMicrophone = portNumber == 1 && consoleType == ConsoleType.Famicom;

				Initialize(mappings);
			}
		}

		private void picBackground_Resize(object sender, EventArgs e)
		{
			this.BeginInvoke((Action)(()=> UpdateBackground()));
		}

		public bool IsSnesController 
		{
			set
			{
				_isSnesController = value;

				if(value) {
					lblTurboA.Text = _isSnesController ? "X" : "Turbo A";
					lblTurboB.Text = _isSnesController ? "Y" : "Turbo B";
				}

				btnL.Visible = value;
				lblL.Visible = value;
				btnR.Visible = value;
				lblR.Visible = value;
			}
		}

		public bool ShowMicrophone
		{
			set
			{
				btnMicrophone.Visible = value;
				lblMicrophone.Visible = value;
			}
		}

		private Point[] _drawPoints = new Point[13] {
			new Point(56, 29), new Point(56, 85), new Point(22, 85),
			new Point(22, 130), new Point(56, 130), new Point(56, 181),
			new Point(145, 181), new Point(145, 130), new Point(179, 130),
			new Point(179, 85), new Point(145, 85), new Point(145, 29),
			new Point(56, 29)
		};
		
		public void UpdateBackground()
		{
			float xFactor = picBackground.Width / 585f;
			float yFactor = picBackground.Height / 210f;
			Bitmap bitmap = new Bitmap(picBackground.Width, picBackground.Height);
			using(Graphics g = Graphics.FromImage(bitmap)) {
				g.ScaleTransform(xFactor, yFactor);
				using(Pen pen = new Pen(Color.Black, 2f)) {
					g.DrawRectangle(pen, 1, 1, 585 - 4, 210 - 4);
					g.DrawRectangle(pen, 226, 128, 159, 43);
					g.DrawPolygon(pen, _drawPoints);
				}
			}
			picBackground.Image = bitmap;
		}

		public override void Initialize(KeyMappings mappings)
		{
			InitButton(btnA, mappings.A);
			InitButton(btnB, mappings.B);
			InitButton(btnStart, mappings.Start);
			InitButton(btnSelect, mappings.Select);
			InitButton(btnUp, mappings.Up);
			InitButton(btnDown, mappings.Down);
			InitButton(btnLeft, mappings.Left);
			InitButton(btnRight, mappings.Right);
			InitButton(btnTurboA, mappings.TurboA);
			InitButton(btnTurboB, mappings.TurboB);
			InitButton(btnMicrophone, mappings.Microphone);
			InitButton(btnL, mappings.LButton);
			InitButton(btnR, mappings.RButton);

			this.OnChange();
		}
		
		public override void UpdateKeyMappings()
		{
			_mappings.A = (UInt32)btnA.Tag;
			_mappings.B = (UInt32)btnB.Tag;
			_mappings.Start = (UInt32)btnStart.Tag;
			_mappings.Select = (UInt32)btnSelect.Tag;
			_mappings.Up = (UInt32)btnUp.Tag;
			_mappings.Down = (UInt32)btnDown.Tag;
			_mappings.Left = (UInt32)btnLeft.Tag;
			_mappings.Right = (UInt32)btnRight.Tag;
			_mappings.TurboA = (UInt32)btnTurboA.Tag;
			_mappings.TurboB = (UInt32)btnTurboB.Tag;
			_mappings.TurboSelect = 0;
			_mappings.TurboStart = 0;
			_mappings.Microphone = (UInt32)btnMicrophone.Tag;
			_mappings.LButton = (UInt32)btnL.Tag;
			_mappings.RButton = (UInt32)btnR.Tag;
		}
	}
}
