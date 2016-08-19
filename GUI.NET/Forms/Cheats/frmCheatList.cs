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
		private List<CheatInfo> Cheats { get { return ConfigManager.Config.Cheats; } }
		private GameInfo _selectedItem = null;

		public frmCheatList()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			chkDisableCheats.Checked = ConfigManager.Config.DisableAllCheats;
			UpdateGameList();
		}
		
		protected override void UpdateConfig()
		{
			ConfigManager.Config.DisableAllCheats = chkDisableCheats.Checked;
		}

		private void UpdateGameList(string gameCrc = null)
		{
			RomInfo romInfo = InteropEmu.GetRomInfo();
			Dictionary<string, string> nameByCrc = new Dictionary<string, string>();
			if(!string.IsNullOrWhiteSpace(romInfo.RomName)) {
				nameByCrc[romInfo.GetPrgCrcString()] = romInfo.GetRomName();
			}
			foreach(CheatInfo cheat in this.Cheats) {
				if(!nameByCrc.ContainsKey(cheat.GameCrc)) {
					nameByCrc[cheat.GameCrc] = cheat.GameName;
				}
			}

			lstGameList.Items.Clear();
			List<GameInfo> gameList = new List<GameInfo>();
			foreach(KeyValuePair<string, string> kvp in nameByCrc) {
				gameList.Add(new GameInfo { Text = kvp.Value, Crc = kvp.Key });
			}
			lstGameList.Items.AddRange(gameList.ToArray());

			if(lstGameList.Items.Count > 0) {
				if(gameCrc == null) {
					if(_selectedItem == null && !string.IsNullOrWhiteSpace(romInfo.RomName)) {
						gameCrc = romInfo.GetPrgCrcString();
					} else {
						gameCrc = _selectedItem?.Crc;
					}
				}

				_selectedItem = null;

				if(gameCrc != null) {
					foreach(GameInfo info in lstGameList.Items) {
						if(info.Crc == gameCrc) {
							_selectedItem = info;
							info.Selected = true;
							break;
						}
					}
				}

				if(_selectedItem == null && lstGameList.Items.Count > 0) {
					_selectedItem = (GameInfo)lstGameList.Items[0];
					lstGameList.Items[0].Selected = true;
				}
			} else {
				_selectedItem = null;
			}

			btnDeleteGameCheats.Enabled = mnuDeleteGameCheats.Enabled = btnExportGame.Enabled = mnuExportGame.Enabled = _selectedItem != null;

			UpdateCheatList();
		}

		private void UpdateCheatList()
		{
			lstCheats.Sorting = SortOrder.Ascending;
			lstCheats.Items.Clear();
			if(_selectedItem != null) {
				string crc32 = _selectedItem.Crc;
				foreach(CheatInfo cheat in this.Cheats) {
					if(cheat.GameCrc == crc32) {
						ListViewItem item = lstCheats.Items.Add(cheat.CheatName);
						item.SubItems.Add(cheat.ToString());
						item.Tag = cheat;
						item.Checked = cheat.Enabled;
					}
				}
			}
		}

		private void lstGameList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(lstGameList.SelectedItems.Count > 0) {
				_selectedItem = (GameInfo)lstGameList.SelectedItems[0];
				UpdateCheatList();
			}
		}
		
		private void lstGameList_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			lstGameList.Sorting = lstGameList.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
		}

		private void mnuAddCheat_Click(object sender, EventArgs e)
		{
			CheatInfo newCheat = new CheatInfo {
				GameCrc = _selectedItem?.Crc,
				GameName = _selectedItem?.Text
			};

			frmCheat frm = new frmCheat(newCheat);
			if(frm.ShowDialog() == DialogResult.OK) {
				AddCheats(new List<CheatInfo>() { newCheat });
			}
		}

		private void lstCheats_DoubleClick(object sender, EventArgs e)
		{
			if(lstCheats.SelectedItems.Count == 1) {
				frmCheat frm = new frmCheat((CheatInfo)lstCheats.SelectedItems[0].Tag);
				if(frm.ShowDialog() == DialogResult.OK) {
					UpdateGameList();
				}
			}
		}

		private void lstCheats_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			if(e.Item.Tag is CheatInfo) {
				((CheatInfo)e.Item.Tag).Enabled = e.Item.Checked;
			}
		}

		private void lstCheats_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool enableActions = lstCheats.SelectedItems.Count > 0;
			mnuDeleteCheat.Enabled = btnDeleteCheat.Enabled = mnuExportSelectedCheats.Enabled = btnExportSelectedCheats.Enabled = enableActions;
		}

		private void DeleteSelectedCheats()
		{
			foreach(ListViewItem item in lstCheats.SelectedItems) {
				CheatInfo cheat = item.Tag as CheatInfo;
				this.Cheats.Remove(cheat);
			}
			UpdateGameList();
		}

		private void btnDeleteCheat_Click(object sender, EventArgs e)
		{
			DeleteSelectedCheats();
		}

		private void btnDeleteGameCheats_Click(object sender, EventArgs e)
		{
			foreach(var item in lstCheats.Items) {
				CheatInfo cheat = ((ListViewItem)item).Tag as CheatInfo;
				this.Cheats.Remove(cheat);
			}
			UpdateGameList();
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			var frm = new frmCheatImport();
			frm.FormClosing += (o, evt) => {
				if(frm.DialogResult == DialogResult.OK && frm.ImportedCheats != null) {
					AddCheats(frm.ImportedCheats);
				}
			};
			frm.ShowDialog(sender, this);
		}

		private void AddCheats(List<CheatInfo> cheats)
		{
			if(cheats.Count > 0) {
				HashSet<string> existingCheats = new HashSet<string>();
				foreach(CheatInfo cheat in this.Cheats) {
					existingCheats.Add(cheat.GameCrc + cheat.ToString());
				}

				foreach(CheatInfo cheat in cheats) {
					if(!existingCheats.Contains(cheat.GameCrc + cheat.ToString())) {
						this.Cheats.Add(cheat);
					}
				}

				UpdateGameList(cheats[0].GameCrc);
			}
		}

		private void btnDelete_ButtonClick(object sender, EventArgs e)
		{
			btnDelete.ShowDropDown();
		}

		private void ExportCheats(IEnumerable<CheatInfo> cheats, string defaultFilename)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.AddExtension = true;
			sfd.FileName = defaultFilename;
			sfd.Filter = "XML (*.xml)|*.xml";

			if(sfd.ShowDialog() == DialogResult.OK) {
				MesenCheatExporter.Export(sfd.FileName, cheats);
			}
		}

		private void btnExportAllCheats_Click(object sender, EventArgs e)
		{
			ExportCheats(this.Cheats, "MesenCheats.xml");
		}

		private void btnExport_ButtonClick(object sender, EventArgs e)
		{
			btnExport.ShowDropDown();
		}

		private void btnExportGame_Click(object sender, EventArgs e)
		{
			ExportCheats(this.Cheats.Where((c) => c.GameCrc == _selectedItem.Crc), _selectedItem.Text + "_Cheats.xml");
		}

		private void btnExportSelectedCheats_Click(object sender, EventArgs e)
		{
			List<CheatInfo> cheats = new List<CheatInfo>();
			foreach(ListViewItem item in lstCheats.SelectedItems) {
				cheats.Add(item.Tag as CheatInfo);
			}
			ExportCheats(cheats, _selectedItem.Text + "_Cheats.xml");
		}
	}

	public class GameInfo : ListViewItem
	{
		public string Crc;
	}
}
