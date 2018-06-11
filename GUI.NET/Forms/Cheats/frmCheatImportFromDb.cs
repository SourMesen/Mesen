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

namespace Mesen.GUI.Forms.Cheats
{
	public partial class frmCheatImportFromDb : BaseForm
	{
		private List<CheatInfo> _cheats;
		private Dictionary<string, GameInfo> _gamesByCrc;
		public List<CheatInfo> ImportedCheats { get; internal set; }
		private string _previousSearch = "";

		private class GameInfo
		{
			public string Name;
			public string Crc;
			public int CheatCount;

			public override string ToString()
			{
				return Name;
			}
		}

		public frmCheatImportFromDb()
		{
			InitializeComponent();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			_cheats = NestopiaCheatLoader.Load(ResourceManager.GetZippedResource("MesenCheatDb.xml"), "", "");

			_gamesByCrc = new Dictionary<string, GameInfo>();
			foreach(CheatInfo cheat in _cheats) {
				if(_gamesByCrc.ContainsKey(cheat.GameName)) {
					_gamesByCrc[cheat.GameCrc].CheatCount++;
				} else {
					_gamesByCrc[cheat.GameCrc] = new GameInfo() { Name = cheat.GameName, Crc = cheat.GameCrc, CheatCount = 1 };
				}
			}

			lblCheatCount.Text = ResourceHelper.GetMessage("CheatsFound", _gamesByCrc.Count.ToString(), _cheats.Count.ToString());
			lstGames.Sorted = true;

			txtSearch.Focus();
			UpdateList();

			RomInfo info = InteropEmu.GetRomInfo();
			if(!string.IsNullOrWhiteSpace(info.GetRomName())) {
				string loadedGameCrc = info.GetPrgCrcString();
				for(int i = 0, len = lstGames.Items.Count; i < len; i++) {
					if(((GameInfo)lstGames.Items[i]).Crc == loadedGameCrc) {
						lstGames.TopIndex = Math.Max(0, i - 10);
						lstGames.SelectedIndex = i;
						break;
					}
				}
			}
		}
	
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(txtSearch.Focused) {
				if(keyData == Keys.Down || keyData == Keys.PageDown || keyData == Keys.Up || keyData == Keys.PageUp) {
					lstGames.Focus();
					if(lstGames.Items.Count > 0) {
						lstGames.SelectedIndex = 0;
					}
					return true;
				}
			} else if(lstGames.Focused && lstGames.SelectedIndex <= 0) {
				if(keyData == Keys.Up || keyData == Keys.PageUp) {
					txtSearch.Focus();
					txtSearch.SelectAll();
					return true;
				}
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}		

		private void UpdateList()
		{
			lstGames.Items.Clear();
			if(string.IsNullOrWhiteSpace(_previousSearch)) {
				lstGames.Items.AddRange(_gamesByCrc.Values.ToArray());
			} else {
				lstGames.Items.AddRange(_gamesByCrc.Where(c => c.Value.Name.IndexOf(_previousSearch, StringComparison.InvariantCultureIgnoreCase) >= 0).Select(c => c.Value).ToArray());
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
		}

		void SetImportedCheats()
		{
			string crc = ((GameInfo)lstGames.SelectedItem).Crc;
			this.ImportedCheats = new List<CheatInfo>(_cheats.Where(c => c.GameCrc == crc).ToArray());
			this.DialogResult = DialogResult.OK;
		}

		private void lstRoms_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnOK.Enabled = lstGames.SelectedItems.Count > 0;
		}

		private void lstRoms_DoubleClick(object sender, EventArgs e)
		{
			SetImportedCheats();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SetImportedCheats();
		}

		private void tmrSearch_Tick(object sender, EventArgs e)
		{
			if(txtSearch.Text.Trim() != _previousSearch) {
				_previousSearch = txtSearch.Text.Trim();
				UpdateList();
			}
		}
	}
}
