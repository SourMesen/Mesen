using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	class FontDialogHelper
	{
		public static Font SelectFont(Font currentFont)
		{
			using(FontDialog fd = new FontDialog()) {
				fd.Font = currentFont;
				fd.ShowApply = false;
				fd.ShowColor = false;
				fd.ShowEffects = false;
				fd.ShowHelp = false;
				if(fd.ShowDialog() == DialogResult.OK) {
					return fd.Font;
				} else {
					return currentFont;
				}
			}
		}
	}
}
