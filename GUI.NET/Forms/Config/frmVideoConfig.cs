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
			
			AddBinding("EmulationSpeed", nudEmulationSpeed);
			AddBinding("ShowFPS", chkShowFps);
			AddBinding("VerticalSync", chkVerticalSync);
			AddBinding("FullscreenMode", chkFullscreenMode);
			AddBinding("UseHdPacks", chkUseHdPacks);
			
			AddBinding("VideoScale", cboScale);
			AddBinding("AspectRatio", cboAspectRatio);
			AddBinding("VideoFilter", cboFilter);

			AddBinding("OverscanLeft", nudOverscanLeft);
			AddBinding("OverscanRight", nudOverscanRight);
			AddBinding("OverscanTop", nudOverscanTop);
			AddBinding("OverscanBottom", nudOverscanBottom);

			toolTip.SetToolTip(picHdNesTooltip, "This option allows Mesen to load HDNes-format HD packs if they are found." + Environment.NewLine + Environment.NewLine + "HD Packs should be placed in the \"HdPacks\" folder in a subfolder matching the name of the ROM." + Environment.NewLine + "e.g: MyRom.nes should have their HD Pack in \"HdPacks\\MyRom\\hires.txt\"." + Environment.NewLine + Environment.NewLine + "Note: Support for HD Packs is a work in progress and some limitations remain.");
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			VideoInfo.ApplyConfig();
		}
	}
}
