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
		private KeyPresets _presets = new KeyPresets();

		public frmControllerConfig(ControllerInfo controllerInfo, int portNumber, ConsoleType consoleType)
		{
			InitializeComponent();

			Entity = controllerInfo;

			AddBinding("TurboSpeed", trkTurboSpeed);
			ctrlStandardController0.Initialize(controllerInfo.Keys[0]);
			ctrlStandardController1.Initialize(controllerInfo.Keys[1]);
			ctrlStandardController2.Initialize(controllerInfo.Keys[2]);
			ctrlStandardController3.Initialize(controllerInfo.Keys[3]);

			if(portNumber == 1 && consoleType == ConsoleType.Famicom) {
				ctrlStandardController0.ShowMicrophone = true;
				ctrlStandardController1.ShowMicrophone = true;
				ctrlStandardController2.ShowMicrophone = true;
				ctrlStandardController3.ShowMicrophone = true;
			}

			ResourceHelper.ApplyResources(this, mnuStripPreset);

			this.Text += ": " + ResourceHelper.GetMessage("PlayerNumber", (portNumber + 1).ToString());
		}

		private ctrlStandardController GetControllerControl()
		{
			if(tabMain.SelectedTab == tpgSet1) {
				return ctrlStandardController0;
			} else if(tabMain.SelectedTab == tpgSet2) {
				return ctrlStandardController1;
			} else if(tabMain.SelectedTab == tpgSet3) {
				return ctrlStandardController2;
			} else if(tabMain.SelectedTab == tpgSet4) {
				return ctrlStandardController3;
			}

			return ctrlStandardController0;
		}

		private void UpdateTabIcons()
		{
			tpgSet1.ImageIndex = (int)ctrlStandardController0.GetKeyType() - 1;
			tpgSet2.ImageIndex = (int)ctrlStandardController1.GetKeyType() - 1;
			tpgSet3.ImageIndex = (int)ctrlStandardController2.GetKeyType() - 1;
			tpgSet4.ImageIndex = (int)ctrlStandardController3.GetKeyType() - 1;
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

		private void btnSelectPreset_Click(object sender, EventArgs e)
		{
			mnuStripPreset.Show(btnSelectPreset.PointToScreen(new Point(0, btnSelectPreset.Height-1)));
		}

		private void mnuWasdLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.WasdLayout);
		}

		private void mnuArrowLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.ArrowLayout);
		}

		private void mnuFceuxLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.FceuxLayout);
		}

		private void mnuNestopiaLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.NestopiaLayout);
		}

		private void mnuXboxLayout1_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.XboxLayout1);
		}

		private void mnuXboxLayout2_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.XboxLayout1);
		}

		private void mnuPs4Layout1_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.Ps4Layout1);
		}

		private void mnuPs4Layout2_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.Ps4Layout2);
		}

		private void mnuSnes30Layout1_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.Snes30Layout1);
		}

		private void mnuSnes30Layout2_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.Snes30Layout2);
		}

		private void ctrlStandardController_OnChange(object sender, EventArgs e)
		{
			UpdateTabIcons();
		}
	}
}
