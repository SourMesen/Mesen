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

		private string _gameCrc;

		public frmCheat(CheatInfo cheat)
		{
			InitializeComponent();

			Entity = cheat;

			_gameCrc = cheat.GameCrc;

			radGameGenie.Tag = CheatType.GameGenie;
			radProActionRocky.Tag = CheatType.ProActionRocky;
			radCustom.Tag = CheatType.Custom;
			radRelativeAddress.Tag = true;
			radAbsoluteAddress.Tag = false;

			AddBinding("Enabled", chkEnabled);
			AddBinding("CheatName", txtCheatName);
			AddBinding("GameName", txtGameName);
			AddBinding("CheatType", radGameGenie.Parent);
			AddBinding("GameGenieCode", txtGameGenie);
			AddBinding("ProActionRockyCode", txtProActionRocky);
			AddBinding("Address", txtAddress);
			AddBinding("Value", txtValue);
			AddBinding("UseCompareValue", chkCompareValue);
			AddBinding("CompareValue", txtCompare);
			AddBinding("IsRelativeAddress", radRelativeAddress.Parent);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			txtCheatName.Focus();
		}

		protected override Type BindedType
		{
			get { return typeof(CheatInfo); }
		}

		protected override bool ApplyChangesOnOK
		{
			get {	return false; }
		}

		protected override void UpdateConfig()
		{
			UpdateObject();
			((CheatInfo)Entity).GameCrc = _gameCrc;
		}

		private void LoadGame(string romPath)
		{
			int archiveFileIndex = -1;
			if(frmSelectRom.SelectRom(romPath, ref archiveFileIndex)) {
				RomInfo romInfo = InteropEmu.GetRomInfo(romPath, archiveFileIndex);
				_gameCrc = romInfo.GetPrgCrcString();
				if(_gameCrc != null) {
					((CheatInfo)Entity).GameName = Path.GetFileNameWithoutExtension(romInfo.RomName);
					txtGameName.Text = ((CheatInfo)Entity).GameName;
				}
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = ResourceHelper.GetMessage("FilterRom");
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				LoadGame(ofd.FileName);
			}
		}

		protected override bool ValidateInput()
		{
			UInt32 val;
			if(_gameCrc == null) {
				return false;
			}

			if(string.IsNullOrWhiteSpace(txtGameName.Text)) {
				return false;
			}

			if(string.IsNullOrWhiteSpace(txtCheatName.Text)) {
				return false;
			}

			if(radGameGenie.Checked) {
				if(txtGameGenie.Text.Length != frmCheat.GGShortCodeLength && txtGameGenie.Text.Length != frmCheat.GGLongCodeLength) {
					return false;
				}
				if(txtGameGenie.Text.Count(c => !"APZLGITYEOXUKSVN".Contains(c.ToString().ToUpper())) > 0) {
					return false;
				}
			} else if(radProActionRocky.Checked) {
				if(txtProActionRocky.Text.Length != frmCheat.PARCodeLength) {
					return false;
				}
				if(!UInt32.TryParse(txtProActionRocky.Text, System.Globalization.NumberStyles.AllowHexSpecifier, null, out val)) {
					return false;
				}
				if(txtProActionRocky.Text.Count(c => !"1234567890ABCDEF".Contains(c.ToString().ToUpper())) > 0) {
					return false;
				}
			} else {
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

			return true;
		}

		private void txtGameGenie_Enter(object sender, EventArgs e)
		{
			radGameGenie.Checked = true;
		}

		private void txtProActionRocky_Enter(object sender, EventArgs e)
		{
			radProActionRocky.Checked = true;
		}

		private void customField_Enter(object sender, EventArgs e)
		{
			radCustom.Checked = true;
		}

		private void chkCompareValue_CheckedChanged(object sender, EventArgs e)
		{
			txtCompare.Enabled = chkCompareValue.Checked;
		}
	}
}
