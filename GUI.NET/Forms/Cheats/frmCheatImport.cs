using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms.Cheats
{
	public partial class frmCheatImport : BaseForm
	{
		private string _gameCrc = "";
		private string _gameName = "";
		private string _cheatFile = null;
		private bool _isMesenCheatFile = false;

		public List<CheatInfo> ImportedCheats { get; internal set; }

		public frmCheatImport()
		{
			InitializeComponent();

			UpdateImportButton();

			RomInfo romInfo = InteropEmu.GetRomInfo();
			_gameCrc = romInfo.GetPrgCrcString();
			_gameName = romInfo.GetRomName();
			txtGameName.Text = _gameName;
		}

		private void UpdateImportButton()
		{
			btnImport.Enabled = !string.IsNullOrWhiteSpace(_cheatFile) && (!string.IsNullOrWhiteSpace(_gameName) || _isMesenCheatFile);
		}

		private void LoadGame(string romPath)
		{
			ResourcePath resource = romPath;
			if(frmSelectRom.SelectRom(ref resource)) {
				RomInfo romInfo = InteropEmu.GetRomInfo(resource);
				_gameCrc = romInfo.GetPrgCrcString();
				_gameName = romInfo.GetRomName();
				txtGameName.Text = _gameName;
				UpdateImportButton();
			}
		}

		private void btnBrowseGame_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter(ResourceHelper.GetMessage("FilterRom"));
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				LoadGame(ofd.FileName);
			}
		}

		private void btnBrowseCheat_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter(ResourceHelper.GetMessage("FilterCheat"));
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				_cheatFile = ofd.FileName;
				_isMesenCheatFile = NestopiaCheatLoader.IsMesenCheatFile(_cheatFile);
				if(_isMesenCheatFile) {
					txtGameName.Text = "";
					_gameName = "";
					_gameCrc = "";
				}
				txtCheatFile.Text = Path.GetFileName(_cheatFile);
				UpdateImportButton();
			}
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			if(File.Exists(_cheatFile)) {
				switch(Path.GetExtension(_cheatFile).ToLowerInvariant().Substring(1)) {
					default:
					case "xml":
						ImportedCheats = NestopiaCheatLoader.Load(_cheatFile, _gameName, _gameCrc);
						break;

					case "cht":
						ImportedCheats = FceuxCheatLoader.Load(_cheatFile, _gameName, _gameCrc);
						break;
				}
			}

			if(ImportedCheats != null) {
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
