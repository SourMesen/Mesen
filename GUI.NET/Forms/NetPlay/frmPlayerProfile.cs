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
	public partial class frmPlayerProfile : BaseConfigForm
	{
		public frmPlayerProfile()
		{
			InitializeComponent();

			this.txtPlayerName.Text = ConfigManager.Config.Profile.PlayerName;
		}
		
		protected override void UpdateConfig()
		{
			PlayerProfile profile = new PlayerProfile();
			profile.PlayerName = this.txtPlayerName.Text;
			ConfigManager.Config.Profile = profile;
		}
	}
}
