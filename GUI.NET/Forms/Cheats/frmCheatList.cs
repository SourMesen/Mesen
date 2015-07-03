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
			UpdateCheatList();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Location = new Point(Owner.Location.X + (Owner.Width - Width) / 2, Owner.Location.Y + (Owner.Height - Height) / 2);
		}

		private void UpdateCheatList()
		{
			lstCheats.Items.Clear();
			foreach(CheatInfo cheat in ConfigManager.Config.Cheats) {
				ListViewItem item = lstCheats.Items.Add(cheat.GameName);
				item.SubItems.AddRange(new string[] { cheat.CheatName, cheat.ToString() });
				item.Tag = cheat;
				item.Checked = cheat.Enabled;
			}
		}

		private void mnuAddCheat_Click(object sender, EventArgs e)
		{
			frmCheat frm = new frmCheat(null);
			if(frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				UpdateCheatList();
			}
		}

		private void lstCheats_DoubleClick(object sender, EventArgs e)
		{
			if(lstCheats.SelectedItems.Count == 1) {
				frmCheat frm = new frmCheat((CheatInfo)lstCheats.SelectedItems[0].Tag);
				if(frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
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
	}
}
