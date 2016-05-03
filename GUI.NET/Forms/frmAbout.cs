using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	partial class frmAbout : BaseForm
	{
		public frmAbout()
		{
			InitializeComponent();
		}

		private void lblLink_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.mesen.ca");
		}
	}
}
