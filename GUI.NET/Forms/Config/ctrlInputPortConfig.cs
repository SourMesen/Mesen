using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms.Config
{
	public partial class ctrlInputPortConfig : UserControl
	{
		private ControllerInfo _controllerInfo;

		public ctrlInputPortConfig()
		{
			InitializeComponent();
		}

		public void Initialize(ControllerInfo controllerInfo)
		{
			_controllerInfo = controllerInfo;
			//ctrlStandardController.Initialize(controllerInfo.Keys);

			cboControllerType.SelectedIndex = (int)_controllerInfo.ControllerType;
		}

		public void UpdateConfig()
		{
			//_controllerInfo.Keys = ctrlStandardController.GetKeyMappings();
			_controllerInfo.ControllerType = (InteropEmu.ControllerType)cboControllerType.SelectedIndex;
		}

		private void cboControllerType_SelectedIndexChanged(object sender, EventArgs e)
		{
			ctrlStandardController.Visible = ((InteropEmu.ControllerType)cboControllerType.SelectedIndex == InteropEmu.ControllerType.StandardController);
		}
	}
}
