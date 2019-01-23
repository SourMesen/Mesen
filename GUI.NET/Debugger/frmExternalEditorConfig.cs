using Mesen.GUI.Config;
using Mesen.GUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmExternalEditorConfig : BaseConfigForm
	{
		public frmExternalEditorConfig()
		{
			InitializeComponent();

			Entity = ConfigManager.Config.DebugInfo;

			AddBinding("ExternalEditorPath", txtPath);
			AddBinding("ExternalEditorArguments", txtArguments);
		}
	}
}
