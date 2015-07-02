using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	public class BaseConfigForm : Form
	{
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(this.DialogResult == System.Windows.Forms.DialogResult.OK) {
				UpdateConfig();
				ConfigManager.ApplyChanges();
			} else {
				ConfigManager.RejectChanges();
			}
			base.OnFormClosed(e);
		}

		protected virtual void UpdateConfig()
		{
		}
	}
}
