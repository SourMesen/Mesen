using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms.Cheats
{
	public partial class frmCheat : BaseConfigForm
	{
		const int GGShortCodeLength = 6;
		const int GGLongCodeLength = 8;
		const int PARCodeLength = 8;

		private string _gameHash;

		CheatInfo _originalCheat;

		public frmCheat(CheatInfo cheat)
		{
			InitializeComponent();

			_originalCheat = cheat;
			if(cheat != null) {
				UpdateUI(cheat);
			}
			UpdateOKButton();
		}

		protected override bool ApplyChangesOnOK
		{
			get {	return false; }
		}

		protected override void UpdateConfig()
		{
			if(ConfigManager.Config.Cheats.Contains(_originalCheat)) {
				ConfigManager.Config.Cheats.Remove(_originalCheat);
			}
			ConfigManager.Config.Cheats.Add(GetCheatInfo());
		}

		private void UpdateOKButton()
		{
			btnOK.Enabled = this.IsValidInput();
		}

		private string GetMD5Hash(string filename)
		{
			var md5 = System.Security.Cryptography.MD5.Create();
			if(filename.EndsWith(".nes", StringComparison.InvariantCultureIgnoreCase)) {
				return BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(filename))).Replace("-", "");
			} else if(filename.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase)) {
				foreach(var entry in ZipFile.OpenRead(filename).Entries) {
					if(entry.Name.EndsWith(".nes", StringComparison.InvariantCultureIgnoreCase)) {
						return BitConverter.ToString(md5.ComputeHash(entry.Open())).Replace("-", "");
					}
				}
			}
			return null;
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "All supported formats (*.nes, *.zip)|*.NES;*.ZIP|NES Roms (*.nes)|*.NES|ZIP Archives (*.zip)|*.ZIP";
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				_gameHash = GetMD5Hash(ofd.FileName);
				if(_gameHash != null) {
					txtGameName.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
					UpdateOKButton();
				}
			}
		}

		public bool IsValidInput()
		{
			if(radCustom.Checked) {
				UInt32 val;
				Byte byteVal;
				if(!UInt32.TryParse(txtAddress.Text, System.Globalization.NumberStyles.AllowHexSpecifier, null, out val)) {
					return false;
				}

				if(!Byte.TryParse(txtValue.Text, System.Globalization.NumberStyles.AllowHexSpecifier, null, out byteVal)) {
					return false;
				}

				if(!Byte.TryParse(txtCompare.Text, System.Globalization.NumberStyles.AllowHexSpecifier, null, out byteVal)) {
					return false;
				}
			}

			CheatInfo cheat;
			try {
				cheat = this.GetCheatInfo();
			} catch {
				return false;
			}

			if(cheat.GameHash == null) {
				return false;
			}

			if(string.IsNullOrWhiteSpace(cheat.CheatName)) {
				return false;
			}

			if(cheat.CheatType == CheatType.GameGenie) {
				if(cheat.Code.Length != frmCheat.GGShortCodeLength && cheat.Code.Length != frmCheat.GGLongCodeLength) {
					return false;
				}
			} else if(cheat.CheatType == CheatType.ProActionRocky) {
				if(cheat.Code.Length != frmCheat.PARCodeLength) {
					return false;
				}
			}

			return true;
		}

		public CheatInfo GetCheatInfo()
		{
			return new CheatInfo() {
				Enabled = chkEnabled.Checked,
				CheatName = txtCheatName.Text,
				GameName = txtGameName.Text,
				GameHash = _gameHash,
				CheatType = radGameGenie.Checked ? CheatType.GameGenie : radProActionRocky.Checked ? CheatType.ProActionRocky : CheatType.Custom,
				Code = (radGameGenie.Checked ? txtGameGenie.Text : txtProActionRocky.Text).ToUpper(),
				Address = radCustom.Checked ? UInt32.Parse(txtAddress.Text, System.Globalization.NumberStyles.AllowHexSpecifier) : 0,
				Value = radCustom.Checked ? Byte.Parse(txtValue.Text, System.Globalization.NumberStyles.AllowHexSpecifier) : (byte)0,
				CompareValue = radCustom.Checked ? Byte.Parse(txtCompare.Text, System.Globalization.NumberStyles.AllowHexSpecifier) : (byte)0,
				IsRelativeAddress = radRelativeAddress.Checked
			};
		}

		private void UpdateUI(CheatInfo cheat)
		{
			chkEnabled.Checked = cheat.Enabled;
			txtCheatName.Text = cheat.CheatName;
			txtGameName.Text = cheat.GameName;
			_gameHash = cheat.GameHash;
			switch(cheat.CheatType) {
				case CheatType.GameGenie: 
					radGameGenie.Checked = true; 
					txtGameGenie.Text = cheat.Code;
					break;
				case CheatType.ProActionRocky: 
					radProActionRocky.Checked = true; 
					txtProActionRocky.Text = cheat.Code;
					break;
				case CheatType.Custom: 
					radCustom.Checked = true; 
					txtAddress.Text = cheat.Address.ToString("X");
					txtValue.Text = cheat.Value.ToString("X");
					txtCompare.Text = cheat.CompareValue.ToString("X");
					radAbsoluteAddress.Checked = !cheat.IsRelativeAddress;
					radRelativeAddress.Checked = cheat.IsRelativeAddress;
					break;
			}
		}

		private void txtBox_TextChanged(object sender, EventArgs e)
		{
			UpdateOKButton();
		}

		private void txtGameGenie_Enter(object sender, EventArgs e)
		{
			radGameGenie.Checked = true;
			UpdateOKButton();
		}

		private void txtProActionRocky_Enter(object sender, EventArgs e)
		{
			radProActionRocky.Checked = true;
			UpdateOKButton();
		}

		private void customField_Enter(object sender, EventArgs e)
		{
			radCustom.Checked = true;
			UpdateOKButton();
		}

		private void radType_CheckedChanged(object sender, EventArgs e)
		{
			UpdateOKButton();
		}
	}
}
