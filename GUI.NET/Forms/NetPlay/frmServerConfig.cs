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

namespace Mesen.GUI.Forms.NetPlay
{
	public partial class frmServerConfig : BaseConfigForm
	{
		public frmServerConfig()
		{
			InitializeComponent();

			this.txtServerName.Text = ConfigManager.Config.ServerInfo.Name;
			this.txtPort.Text = ConfigManager.Config.ServerInfo.Port.ToString();
			this.txtPassword.Text = string.Empty;
			this.nudNbPlayers.Value = ConfigManager.Config.ServerInfo.MaxPlayers;
			this.chkSpectator.Checked = ConfigManager.Config.ServerInfo.AllowSpectators;
			this.chkPublicServer.Checked = ConfigManager.Config.ServerInfo.PublicServer;
		}

		protected override void UpdateConfig()
		{
			ConfigManager.Config.ServerInfo = new ServerInfo() {
				Name = this.txtServerName.Text,
				Port = Convert.ToUInt16(this.txtPort.Text),
				Password = BitConverter.ToString(System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(this.txtPassword.Text))).Replace("-", ""),
				MaxPlayers = (int)this.nudNbPlayers.Value,
				AllowSpectators = this.chkSpectator.Checked,
				PublicServer = this.chkPublicServer.Checked
			};
		}

		private void Field_ValueChanged(object sender, EventArgs e)
		{
			UInt16 port;
			if(!UInt16.TryParse(this.txtPort.Text, out port)) {
				this.btnOK.Enabled = false;
			} else {
				this.btnOK.Enabled = !string.IsNullOrWhiteSpace(this.txtServerName.Text);
			}
		}
	}
}
