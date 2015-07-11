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
	public partial class frmInputConfig : BaseConfigForm
	{
		public frmInputConfig()
		{
			InitializeComponent();

			ctrlInputPortConfig1.Initialize(ConfigManager.Config.Controllers[0]);
			ctrlInputPortConfig2.Initialize(ConfigManager.Config.Controllers[1]);
			ctrlInputPortConfig3.Initialize(ConfigManager.Config.Controllers[2]);
			ctrlInputPortConfig4.Initialize(ConfigManager.Config.Controllers[3]);
		}

		protected override void UpdateConfig()
		{
			ctrlInputPortConfig1.UpdateConfig();
			ctrlInputPortConfig2.UpdateConfig();
			ctrlInputPortConfig3.UpdateConfig();
			ctrlInputPortConfig4.UpdateConfig();

			ControllerInfo.ApplyConfig();
		}
	}
}
