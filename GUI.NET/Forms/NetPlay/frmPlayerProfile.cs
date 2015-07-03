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
			this.picAvatar.Image = ConfigManager.Config.Profile.GetAvatarImage();
		}

		private void picAvatar_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "All supported image files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				try {
					this.picAvatar.Image = Image.FromFile(ofd.FileName).ResizeImage(64, 64);
				} catch {
					MessageBox.Show("Invalid image format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		/*
		protected override void UpdateConfig()
		{
			PlayerProfile profile = new PlayerProfile();
			profile.PlayerName = this.txtPlayerName.Text;
			profile.SetAvatar(this.picAvatar.Image);
			ConfigManager.Config.Profile = profile;
		}*/
	}
}
