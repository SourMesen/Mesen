using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	public class BaseForm : Form
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!DesignMode) {
				Icon = Properties.Resources.MesenIcon;
			}
		}
	}
}
