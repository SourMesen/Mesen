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
	public partial class frmPowerPadConfig : BaseInputConfigForm
	{
		public frmPowerPadConfig(ControllerInfo controllerInfo, int portNumber)
		{
			InitializeComponent();

			if(!this.DesignMode) {
				SetMainTab(this.tabMain);

				Entity = controllerInfo;

				ctrlPowerPadConfig0.Initialize(controllerInfo.Keys[0]);
				ctrlPowerPadConfig1.Initialize(controllerInfo.Keys[1]);
				ctrlPowerPadConfig2.Initialize(controllerInfo.Keys[2]);
				ctrlPowerPadConfig3.Initialize(controllerInfo.Keys[3]);

				this.Text += ": " + ResourceHelper.GetMessage("PlayerNumber", (portNumber + 1).ToString());
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			ClearCurrentTab();
		}

		private void btnSetDefault_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.PowerPad);
		}
	}
}
