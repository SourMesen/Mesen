using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

			lblMesenVersion.Text = InteropEmu.GetMesenVersion();

#if AUTOBUILD
			string devVersion = ResourceManager.ReadZippedResource("DevBuild.txt");
			if(devVersion != null) {
				lblMesenVersion.Text = devVersion;
			}
#endif
		}

		private void lblLink_Click(object sender, EventArgs e)
		{
			Process.Start("http://www.mesen.ca");
		}

		private void picDonate_Click(object sender, EventArgs e)
		{
			Process.Start("http://www.mesen.ca/Donate.php?l=" + ResourceHelper.GetLanguageCode());
		}
	}
}
