using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms.NetPlay
{
	public partial class frmClientConfig : BaseConfigForm
	{
		public frmClientConfig()
		{
			InitializeComponent();

			this.txtHost.Text = ConfigManager.Config.ClientConnectionInfo.Host;
			this.txtPort.Text = ConfigManager.Config.ClientConnectionInfo.Port.ToString();
		}

		protected override void UpdateConfig()
		{
			ConfigManager.Config.ClientConnectionInfo = new ClientConnectionInfo() { Host = this.txtHost.Text, Port = Convert.ToUInt16(this.txtPort.Text) };
		}

		private void Field_TextChanged(object sender, EventArgs e)
		{
			UInt16 port;
			if(!UInt16.TryParse(this.txtPort.Text, out port)) {
				this.btnOK.Enabled = false;
			} else {
				this.btnOK.Enabled = !string.IsNullOrWhiteSpace(this.txtHost.Text);
			}
		}
	}
}
