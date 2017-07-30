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
	public partial class frmZapperConfig : BaseConfigForm
	{
		public frmZapperConfig(ZapperInfo zapperInfo)
		{
			InitializeComponent();

			Entity = zapperInfo;
			AddBinding("DetectionRadius", trkRadius);
		}
	}
}
