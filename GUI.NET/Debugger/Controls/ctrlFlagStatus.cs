using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlFlagStatus : BaseControl
	{
		public ctrlFlagStatus()
		{
			InitializeComponent();
			if(Program.IsMono) {
				lblLetter.Location = new Point(-1, -1);
			}
		}

		public bool Active
		{
			set
			{
				panelBorder.BackColor = value ? Color.Red : Color.LightGray;
				panelBg.BackColor = value ? Color.White : Color.DarkGray;
				lblLetter.ForeColor = Color.Black;
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
