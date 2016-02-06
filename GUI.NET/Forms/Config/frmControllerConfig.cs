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
	public partial class frmControllerConfig : BaseConfigForm
	{
		public frmControllerConfig(ControllerInfo controllerInfo)
		{
			InitializeComponent();

			Entity = controllerInfo;

			AddBinding("TurboSpeed", trkTurboSpeed);
			ctrlStandardController0.Initialize(controllerInfo.Keys[0]);
			ctrlStandardController1.Initialize(controllerInfo.Keys[1]);
			ctrlStandardController2.Initialize(controllerInfo.Keys[2]);
			ctrlStandardController3.Initialize(controllerInfo.Keys[3]);
		}

		protected override void UpdateConfig()
		{
			ControllerInfo controllerInfo = (ControllerInfo)Entity;
			controllerInfo.Keys[0] = ctrlStandardController0.GetKeyMappings();
			controllerInfo.Keys[1] = ctrlStandardController1.GetKeyMappings();
			controllerInfo.Keys[2] = ctrlStandardController2.GetKeyMappings();
			controllerInfo.Keys[3] = ctrlStandardController3.GetKeyMappings();
			base.UpdateConfig();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			ControllerInfo controllerInfo = (ControllerInfo)Entity;
			if(tabMain.SelectedTab == tpgSet1) {
				ctrlStandardController0.ClearKeys();
			} else if(tabMain.SelectedTab == tpgSet2) {
				ctrlStandardController1.ClearKeys();
			} else if(tabMain.SelectedTab == tpgSet3) {
				ctrlStandardController2.ClearKeys();
			} else if(tabMain.SelectedTab == tpgSet4) {
				ctrlStandardController3.ClearKeys();
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			
		}
	}
}
