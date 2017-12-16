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
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	public partial class frmPartytapConfig : BaseInputConfigForm
	{
		public frmPartytapConfig(ControllerInfo controllerInfo)
		{
			InitializeComponent();

			if(!this.DesignMode) {
				SetMainTab(this.tabMain);

				Entity = controllerInfo;

				ctrlPartytapConfig0.Initialize(controllerInfo.Keys[0]);
				ctrlPartytapConfig1.Initialize(controllerInfo.Keys[1]);
				ctrlPartytapConfig2.Initialize(controllerInfo.Keys[2]);
				ctrlPartytapConfig3.Initialize(controllerInfo.Keys[3]);
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			ClearCurrentTab();
		}

		private void btnSetDefault_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.PartyTap);
		}
	}
}
