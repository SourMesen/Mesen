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
	public partial class frmControllerConfig : BaseInputConfigForm
	{
		public frmControllerConfig(ControllerInfo controllerInfo, int portNumber, ConsoleType consoleType, InteropEmu.ControllerType controllerType)
		{
			InitializeComponent();

			if(!this.DesignMode) {
				SetMainTab(this.tabMain);

				Entity = controllerInfo;

				AddBinding("TurboSpeed", trkTurboSpeed);

				ctrlStandardController0.Initialize(controllerInfo.Keys[0]);
				ctrlStandardController1.Initialize(controllerInfo.Keys[1]);
				ctrlStandardController2.Initialize(controllerInfo.Keys[2]);
				ctrlStandardController3.Initialize(controllerInfo.Keys[3]);

				ctrlStandardController0.PortNumber = portNumber;
				ctrlStandardController1.PortNumber = portNumber;
				ctrlStandardController2.PortNumber = portNumber;
				ctrlStandardController3.PortNumber = portNumber;

				if(portNumber == 1 && consoleType == ConsoleType.Famicom) {
					ctrlStandardController0.ShowMicrophone = true;
					ctrlStandardController1.ShowMicrophone = true;
					ctrlStandardController2.ShowMicrophone = true;
					ctrlStandardController3.ShowMicrophone = true;
				}

				if(controllerType == InteropEmu.ControllerType.SnesController) {
					ctrlStandardController0.Type = StandardControllerType.SnesController;
					ctrlStandardController1.Type = StandardControllerType.SnesController;
					ctrlStandardController2.Type = StandardControllerType.SnesController;
					ctrlStandardController3.Type = StandardControllerType.SnesController;
				}

				if(InteropEmu.IsVsSystem()) {
					ctrlStandardController0.Type = StandardControllerType.VsSystem;
					ctrlStandardController1.Type = StandardControllerType.VsSystem;
					ctrlStandardController2.Type = StandardControllerType.VsSystem;
					ctrlStandardController3.Type = StandardControllerType.VsSystem;
				}

				this.btnSelectPreset.Image = BaseControl.DownArrow;

				ResourceHelper.ApplyResources(this, mnuStripPreset);
				this.Text += ": " + ResourceHelper.GetMessage("PlayerNumber", (portNumber + 1).ToString());
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			ClearCurrentTab();
		}

		private void btnSelectPreset_Click(object sender, EventArgs e)
		{
			mnuStripPreset.Show(btnSelectPreset.PointToScreen(new Point(0, btnSelectPreset.Height-1)));
		}

		private void mnuWasdLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.WasdLayout);
		}

		private void mnuArrowLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.ArrowLayout);
		}

		private void mnuFceuxLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.FceuxLayout);
		}

		private void mnuNestopiaLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.NestopiaLayout);
		}

		private void mnuXboxLayout1_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.XboxLayout1);
		}

		private void mnuXboxLayout2_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.XboxLayout2);
		}

		private void mnuPs4Layout1_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.Ps4Layout1);
		}

		private void mnuPs4Layout2_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.Ps4Layout2);
		}

		private void mnuSnes30Layout1_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.Snes30Layout1);
		}

		private void mnuSnes30Layout2_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(Presets.Snes30Layout2);
		}
	}
}
