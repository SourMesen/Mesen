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
	public partial class frmImportSettings : BaseConfigForm
	{
		public frmImportSettings()
		{
			InitializeComponent();

			Entity = ConfigManager.Config.DebugInfo.ImportConfig;

			AddBinding("DbgImportRamLabels", chkDbgImportRamLabels);
			AddBinding("DbgImportPrgRomLabels", chkDbgImportPrgRomLabels);
			AddBinding("DbgImportComments", chkDbgImportComments);

			AddBinding("MlbImportInternalRamLabels", chkMlbImportInternalRamLabels);
			AddBinding("MlbImportWorkRamLabels", chkMlbImportWorkRamLabels);
			AddBinding("MlbImportSaveRamLabels", chkMlbImportSaveRamLabels);
			AddBinding("MlbImportRegisterLabels", chkDbgImportRegisterLabels);
			AddBinding("MlbImportPrgRomLabels", chkMlbImportPrgRomLabels);
			AddBinding("MlbImportComments", chkMlbImportComments);
		}
	}
}
