using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlFlagStatus : UserControl
	{
		public ctrlFlagStatus()
		{
			InitializeComponent();
			if(Program.IsMono) {
				lblLetter.Padding = Padding.Empty;
				lblLetter.Width = 16;
			}
		}

		public bool Active
		{
			set
			{
				panel2.BackColor = value ? Color.Red : Color.LightGray;
				lblLetter.BackColor = value ? Color.White : Color.DarkGray;
			}
		}

		public string Letter
		{
			set
			{
				lblLetter.Text = value;
			}
		}
	}
}
