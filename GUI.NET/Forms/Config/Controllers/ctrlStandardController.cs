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
		private StandardControllerType _type = StandardControllerType.Default;

		public ctrlStandardController()
		{
			InitializeComponent();
			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime) {
				Type = StandardControllerType.Default;
				picBackground.Resize += picBackground_Resize;
				UpdateBackground();
			}
		}

		public int PortNumber { get; set; }

		private void picBackground_Resize(object sender, EventArgs e)
		{
			this.BeginInvoke((Action)(()=> UpdateBackground()));
		}

		public StandardControllerType Type
		{
			set
			{
				_type = value;

				lblStart.Text = "Start";
				lblSelect.Text = "Select";
				lblTurboA.Text = "Turbo A";
				lblTurboB.Text = "Turbo B";

				if(_type == StandardControllerType.SnesController) {
					lblTurboA.Text = "X";
					lblTurboB.Text = "Y";
				}

				if(_type == StandardControllerType.VsSystem) {
					lblStart.Visible = false;
					lblSelect.Visible = false;

					picStart.Visible = true;
					picSelect.Visible = true;
					picStart.Image = (PortNumber % 2 == 0) ? Properties.Resources.VsButton1 : Properties.Resources.VsButton2;
					picSelect.Image = (PortNumber % 2 == 0) ? Properties.Resources.VsButton3 : Properties.Resources.VsButton4;
				}

				bool isSnes = _type == StandardControllerType.SnesController;
				btnL.Visible = isSnes;
				lblL.Visible = isSnes;
				btnR.Visible = isSnes;
				lblR.Visible = isSnes;
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
	
		public override void UpdateKeyMappings(KeyMappings mappings)
		{
			mappings.A = (UInt32)btnA.Tag;
			mappings.B = (UInt32)btnB.Tag;
			mappings.Start = (UInt32)btnStart.Tag;
			mappings.Select = (UInt32)btnSelect.Tag;
			mappings.Up = (UInt32)btnUp.Tag;
			mappings.Down = (UInt32)btnDown.Tag;
			mappings.Left = (UInt32)btnLeft.Tag;
			mappings.Right = (UInt32)btnRight.Tag;
			mappings.TurboA = (UInt32)btnTurboA.Tag;
			mappings.TurboB = (UInt32)btnTurboB.Tag;
			mappings.TurboSelect = 0;
			mappings.TurboStart = 0;
			mappings.Microphone = (UInt32)btnMicrophone.Tag;
			mappings.LButton = (UInt32)btnL.Tag;
			mappings.RButton = (UInt32)btnR.Tag;
		}
	}

	public enum StandardControllerType
	{
		Default = 0,
		SnesController = 1,
		VsSystem = 2
	}
}
