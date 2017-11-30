using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	class MesenMsgBox
	{
		public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon, params string[] args)
		{
			string resourceText = ResourceHelper.GetMessage(text, args);

			if(resourceText.StartsWith("[[")) {
				if(args != null && args.Length > 0) {
					return MessageBox.Show(string.Format("Critical error (" + text + ") {0}", args), "Mesen", buttons, icon);
				} else {
					return MessageBox.Show(string.Format("Critical error (" + text + ")"), "Mesen", buttons, icon);
				}
			} else {
				return MessageBox.Show(Application.OpenForms[0], ResourceHelper.GetMessage(text, args), "Mesen", buttons, icon);
			}
		}
	}
}
