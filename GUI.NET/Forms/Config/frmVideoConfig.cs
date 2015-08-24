using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms.Config
{
	public partial class frmVideoConfig : BaseConfigForm
	{
		public frmVideoConfig()
		{
			InitializeComponent();

			Entity = ConfigManager.Config.VideoInfo;

			if(ConfigManager.Config.VideoInfo.FpsLimit == -1) {
				cboFpsLimit.SelectedIndex = 0;
			} else if(ConfigManager.Config.VideoInfo.FpsLimit == 0) {
				cboFpsLimit.SelectedIndex = 1;
			} else {
				cboFpsLimit.SelectedIndex = cboFpsLimit.Items.IndexOf(ConfigManager.Config.VideoInfo.FpsLimit.ToString());
			}

			AddBinding("ShowFPS", chkShowFps);
			AddBinding("OverscanLeft", nudOverscanLeft);
			AddBinding("OverscanRight", nudOverscanRight);
			AddBinding("OverscanTop", nudOverscanTop);
			AddBinding("OverscanBottom", nudOverscanBottom);
		}

		protected override void UpdateConfig()
		{
			int fpsLimit;
			if(cboFpsLimit.SelectedIndex == 0) {
				fpsLimit = -1;
			} else if(cboFpsLimit.SelectedIndex == 1) {
				fpsLimit = 0;
			} else {
				fpsLimit = Int32.Parse(cboFpsLimit.SelectedItem.ToString());
			}

			((VideoInfo)Entity).FpsLimit = fpsLimit;
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			VideoInfo.ApplyConfig();
		}
	}
}
