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

			AddBinding("ShowFPS", chkShowFps);
			AddBinding("LimitFPS", chkLimitFps);
			AddBinding("OverscanLeft", nudOverscanLeft);
			AddBinding("OverscanRight", nudOverscanRight);
			AddBinding("OverscanTop", nudOverscanTop);
			AddBinding("OverscanBottom", nudOverscanBottom);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			VideoInfo.ApplyConfig();
		}
	}
}
