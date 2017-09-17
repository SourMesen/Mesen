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
	public partial class ctrlStandardController : BaseControl
	{
		public event EventHandler OnChange;

		public enum MappedKeyType
		{
			None,
			Keyboard,
			Controller
		}

		public ctrlStandardController()
		{
			InitializeComponent();
			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime) {
				Initialize(new KeyMappings());
				UpdateBackground();
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

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			UpdateBackground();
		}

		private void UpdateBackground()
		{
			Rectangle rect = this.ClientRectangle;
			rect.Inflate(-2, -2);
			float xFactor = picBackground.Width / 585f;
			float yFactor = picBackground.Height / 210f;
			Bitmap bitmap = new Bitmap(picBackground.Width, picBackground.Height);
			using(Graphics g = Graphics.FromImage(bitmap)) {
				g.ScaleTransform(xFactor, yFactor);
				using(Pen pen = new Pen(Color.Black, 2f)) {
					g.DrawRectangle(pen, rect);
					g.DrawRectangle(pen, 226, 128, 159, 43);
					g.DrawPolygon(pen, _drawPoints);
				}
			}
			picBackground.Image = bitmap;
		}

		private void InitButton(Button btn, UInt32 scanCode)
		{
			btn.Text = InteropEmu.GetKeyName(scanCode);
			btn.Tag = scanCode;
		}

		public void Initialize(KeyMappings mappings)
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

			this.OnChange?.Invoke(this, null);
		}

		public MappedKeyType GetKeyType()
		{
			KeyMappings mappings = GetKeyMappings();
			MappedKeyType keyType = MappedKeyType.None;
			if(mappings.A > 0xFFFF || mappings.B > 0xFFFF || mappings.Down > 0xFFFF || mappings.Left > 0xFFFF || mappings.Right > 0xFFFF || mappings.Select > 0xFFFF ||
				mappings.Start > 0xFFFF || mappings.TurboA > 0xFFFF || mappings.TurboB > 0xFFFF || mappings.TurboSelect > 0xFFFF || mappings.TurboStart > 0xFFFF || mappings.Up > 0xFFFF) {
				keyType = MappedKeyType.Controller;
			} else if(mappings.A > 0 || mappings.B > 0 || mappings.Down > 0 || mappings.Left > 0 || mappings.Right > 0 || mappings.Select > 0 ||
				mappings.Start > 0 || mappings.TurboA > 0 || mappings.TurboB > 0 || mappings.TurboSelect > 0 || mappings.TurboStart > 0 || mappings.Up > 0) {
				keyType = MappedKeyType.Keyboard;
			}
			return keyType;
		}

		public void ClearKeys()
		{
			InitButton(btnA, 0);
			InitButton(btnB, 0);
			InitButton(btnStart, 0);
			InitButton(btnSelect, 0);
			InitButton(btnUp, 0);
			InitButton(btnDown, 0);
			InitButton(btnLeft, 0);
			InitButton(btnRight, 0);
			InitButton(btnTurboA, 0);
			InitButton(btnTurboB, 0);
			InitButton(btnMicrophone, 0);

			this.OnChange?.Invoke(this, null);
		}

		private void btnMapping_Click(object sender, EventArgs e)
		{
			using(frmGetKey frm = new frmGetKey(true)) {
				frm.ShowDialog();
				((Button)sender).Text = frm.ShortcutKey.ToString();
				((Button)sender).Tag = frm.ShortcutKey.Key1;
			}
			this.OnChange?.Invoke(this, null);
		}

		public KeyMappings GetKeyMappings()
		{
			KeyMappings mappings = new KeyMappings() {
				A = (UInt32)btnA.Tag,
				B = (UInt32)btnB.Tag,
				Start = (UInt32)btnStart.Tag,
				Select = (UInt32)btnSelect.Tag,
				Up = (UInt32)btnUp.Tag,
				Down = (UInt32)btnDown.Tag,
				Left = (UInt32)btnLeft.Tag,
				Right = (UInt32)btnRight.Tag,
				TurboA = (UInt32)btnTurboA.Tag,
				TurboB = (UInt32)btnTurboB.Tag,
				TurboSelect = 0,
				TurboStart = 0,
				Microphone = (UInt32)btnMicrophone.Tag,
			};
			return mappings;
		}
	}
}
