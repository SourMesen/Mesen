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
			return MessageBox.Show(ResourceHelper.GetMessage(text, args), "Mesen", buttons, icon);
		}
	}
}
