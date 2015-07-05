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
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms.Cheats
{
	public partial class frmCheatList : BaseConfigForm
	{
		public frmCheatList()
		{
			InitializeComponent();
			
			chkCurrentGameOnly.Checked = ConfigManager.Config.ShowOnlyCheatsForCurrentGame;
			if(!chkCurrentGameOnly.Checked) {
				UpdateCheatList();
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Location = new Point(Owner.Location.X + (Owner.Width - Width) / 2, Owner.Location.Y + (Owner.Height - Height) / 2);
		}

		protected override void UpdateConfig()
		{
			ConfigManager.Config.ShowOnlyCheatsForCurrentGame = chkCurrentGameOnly.Checked;
		}

		private void UpdateCheatList()
		{
			string md5hash = MD5Helper.GetMD5Hash(InteropEmu.GetROMPath());
			lstCheats.Items.Clear();
			foreach(CheatInfo cheat in ConfigManager.Config.Cheats) {
				if(!chkCurrentGameOnly.Checked || cheat.GameHash == md5hash) {
					ListViewItem item = lstCheats.Items.Add(cheat.GameName);
					item.SubItems.AddRange(new string[] { cheat.CheatName, cheat.ToString() });
					item.Tag = cheat;
					item.Checked = cheat.Enabled;
				}
			}
		}

		private void mnuAddCheat_Click(object sender, EventArgs e)
		{
			CheatInfo newCheat = new CheatInfo();
			frmCheat frm = new frmCheat(newCheat);
			if(frm.ShowDialog() == DialogResult.OK) {
				ConfigManager.Config.Cheats.Add(newCheat);
				UpdateCheatList();
			}
		}

		private void lstCheats_DoubleClick(object sender, EventArgs e)
		{
			if(lstCheats.SelectedItems.Count == 1) {
				frmCheat frm = new frmCheat((CheatInfo)lstCheats.SelectedItems[0].Tag);
				if(frm.ShowDialog() == DialogResult.OK) {
					UpdateCheatList();
				}
			}
		}

		private void lstCheats_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if(e.Item.Tag is CheatInfo) {
				((CheatInfo)e.Item.Tag).Enabled = e.Item.Checked;
			}
		}

		private void chkCurrentGameOnly_CheckedChanged(object sender, EventArgs e)
		{
			UpdateCheatList();
		}
	}
}
