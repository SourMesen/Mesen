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

		protected override bool ApplyChangesOnOK
		{
			get {	return false; }
		}

		protected override void UpdateConfig()
		{
			((CheatInfo)Entity).GameCrc = _gameCrc;
		}

		private void LoadGame(string romPath)
		{
			ResourcePath resource = romPath;
			if(frmSelectRom.SelectRom(ref resource)) {
				RomInfo romInfo = InteropEmu.GetRomInfo(resource);
				_gameCrc = romInfo.GetPrgCrcString();
				if(_gameCrc != null) {
					((CheatInfo)Entity).GameName = romInfo.GetRomName();
					txtGameName.Text = ((CheatInfo)Entity).GameName;
				}
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter(ResourceHelper.GetMessage("FilterRom"));
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
				string ggCode = txtGameGenie.Text.Trim();
				if(ggCode.Length != frmCheat.GGShortCodeLength && ggCode.Length != frmCheat.GGLongCodeLength) {
					return false;
				}
				if(ggCode.Count(c => !"APZLGITYEOXUKSVN".Contains(c.ToString().ToUpper())) > 0) {
					return false;
				}
			} else if(radProActionRocky.Checked) {
				string parCode = txtProActionRocky.Text.Trim();
				if(parCode.Length != frmCheat.PARCodeLength) {
					return false;
				}
				if(!UInt32.TryParse(parCode, System.Globalization.NumberStyles.AllowHexSpecifier, null, out val)) {
					return false;
				}
				if(parCode.Count(c => !"1234567890ABCDEF".Contains(c.ToString().ToUpper())) > 0) {
					return false;
				}
			} else {
				Byte byteVal;
				if(!UInt32.TryParse(txtAddress.Text.Trim(), System.Globalization.NumberStyles.AllowHexSpecifier, null, out val)) {
					return false;
				}
				if(radRelativeAddress.Checked && val > 0xFFFF) {
					//Do not allow cheats outside the 0-0xFFFF range in relative addressing mode
					return false;
				}

				if(!Byte.TryParse(txtValue.Text.Trim(), System.Globalization.NumberStyles.AllowHexSpecifier, null, out byteVal)) {
					return false;
				}

				if(!string.IsNullOrWhiteSpace(txtCompare.Text.Trim()) && !Byte.TryParse(txtCompare.Text.Trim(), System.Globalization.NumberStyles.AllowHexSpecifier, null, out byteVal)) {
					return false;
				}
			}

			ConvertCode();

			return true;
		}

		private void ConvertCode()
		{
			//Automatically update the other fields to match the active code (when possible)
			UpdateObject();

			CheatInfo cheat = (CheatInfo)this.Entity;
			if(cheat.CheatType == CheatType.GameGenie) {
				CheatData convertedData = ConvertGameGenieCode(cheat.GameGenieCode.ToUpper());
				txtProActionRocky.Text = ToProActionReplayCode(convertedData.Address, convertedData.Value, convertedData.CompareValue);
				txtAddress.Text = convertedData.Address.ToString("X4");
				txtValue.Text = convertedData.Value.ToString("X2");
				txtCompare.Text = convertedData.CompareValue >= 0 ? convertedData.CompareValue.ToString("X2") : "";
				chkCompareValue.Checked = convertedData.CompareValue >= 0;
			} else if(cheat.CheatType == CheatType.ProActionRocky) {
				CheatData convertedData = ConvertProActionReplayCode(cheat.ProActionRockyCode);
				txtGameGenie.Text = ToGameGenieCode(convertedData.Address, convertedData.Value, convertedData.CompareValue);
				txtAddress.Text = convertedData.Address.ToString("X4");
				txtValue.Text = convertedData.Value.ToString("X2");
				txtCompare.Text = convertedData.CompareValue.ToString("X2");
				radRelativeAddress.Checked = true;
				chkCompareValue.Checked = true;
			} else {
				if(cheat.IsRelativeAddress && cheat.Address >= 0x8000) {
					txtGameGenie.Text = ToGameGenieCode((int)cheat.Address, cheat.Value, cheat.UseCompareValue ? cheat.CompareValue : -1);
					txtProActionRocky.Text = ToProActionReplayCode((int)cheat.Address, cheat.Value, cheat.UseCompareValue ? cheat.CompareValue : -1);
				} else {
					txtProActionRocky.Text = "";
					txtGameGenie.Text = "";
				}
			}
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

		private int DecodeValue(int code, int[] bitIndexes)
		{
			int result = 0;
			for(int i = 0; i < bitIndexes.Length; i++) {
				result <<= 1;
				result |= (code >> bitIndexes[i]) & 0x01;
			}
			return result;
		}

		private int EncodeValue(int code, int[] bitIndexes)
		{
			int result = 0;
			for(int i = 0; i < bitIndexes.Length; i++) {
				result |= ((code >> i) & 0x01) << bitIndexes[bitIndexes.Length - i - 1];
			}
			return result;
		}

		private CheatData ConvertGameGenieCode(string ggCode)
		{
			string ggLetters = "APZLGITYEOXUKSVN";

			int rawCode = 0;
			for(int i = 0, len = ggCode.Length; i < len; i++) {
				rawCode |= ggLetters.IndexOf(ggCode[i]) << (i * 4);
			}

			int[] addressBits = { 14, 13, 12, 19, 22, 21, 20, 7, 10, 9, 8, 15, 18, 17, 16 };
			int[] valueBits = { 3, 6, 5, 4, 23, 2, 1, 0 };
			int compareValue = -1;
			if(ggCode.Length == 8) {
				//Bit 5 of the value is stored in a different location for 8-character codes
				valueBits[4] = 31;

				int[] compareValueBits = { 27, 30, 29, 28, 23, 26, 25, 24 };
				compareValue = DecodeValue(rawCode, compareValueBits);
			}
			int address = DecodeValue(rawCode, addressBits) + 0x8000;
			int value = DecodeValue(rawCode, valueBits);

			return new CheatData() {
				Address = address,
				Value = value,
				CompareValue = compareValue
			};
		}

		private string ToGameGenieCode(int address, int value, int compareValue)
		{
			string ggLetters = "APZLGITYEOXUKSVN";

			int[] addressBits = { 14, 13, 12, 19, 22, 21, 20, 7, 10, 9, 8, 15, 18, 17, 16 };
			int[] valueBits = { 3, 6, 5, 4, 23, 2, 1, 0 };
			int[] compareValueBits = { 27, 30, 29, 28, 23, 26, 25, 24 };

			if(compareValue >= 0) {
				//Bit 5 of the value is stored in a different location for 8-character codes
				valueBits[4] = 31;
			}

			UInt32 encodedAddress = (UInt32)(EncodeValue(address - 0x8000, addressBits) | 0x800);
			UInt32 encodedValue = (UInt32)EncodeValue(value, valueBits);

			UInt32 encodedCode = encodedValue | encodedAddress;
			if(compareValue >= 0) {
				UInt32 encodedCompareValue = (UInt32)EncodeValue(compareValue, compareValueBits);
				encodedCode |= encodedCompareValue;
			}

			int codeSize = compareValue >= 0 ? 8 : 6;
			string codeString = "";
			for(int i = 0; i < codeSize; i++) {
				codeString += ggLetters[(int)encodedCode & 0x0F];
				encodedCode >>= 4;
			}

			return codeString;
		}

		private CheatData ConvertProActionReplayCode(UInt32 parCode)
		{
			int[] shiftValues = {
				3, 13, 14, 1, 6, 9, 5, 0, 12, 7, 2, 8, 10, 11, 4,	//address
				19, 21, 23, 22, 20, 17, 16, 18,							//compare
				29, 31, 24, 26, 25, 30, 27, 28                     //value
			};
			UInt32 key = 0x7E5EE93A;
			UInt32 xorValue = 0x5C184B91;

			//Throw away bit 0, not used.
			parCode >>= 1;

			UInt32 result = 0;
			for(int i = 30; i >= 0; i--) {
				if((((key ^ parCode) >> 30) & 0x01) != 0) {
					result |= (UInt32)(0x01 << shiftValues[i]);
					key ^= xorValue;
				}
				parCode <<= 1;
				key <<= 1;
			}

			return new CheatData() {
				Address = (int)((result & 0x7fff) + 0x8000),
				Value = (int)(result >> 24) & 0xFF,
				CompareValue = (int)(result >> 16) & 0xFF
			};
		}

		private string ToProActionReplayCode(int address, int value, int compareValue)
		{
			if(compareValue < 0) {
				return "";
			}

			int[] shiftValues = {
				3, 13, 14, 1, 6, 9, 5, 0, 12, 7, 2, 8, 10, 11, 4,	//address
				19, 21, 23, 22, 20, 17, 16, 18,							//compare
				29, 31, 24, 26, 25, 30, 27, 28                     //value
			};

			int encodedValue = (address & 0x7FFF) | (compareValue << 16) | (value << 24);

			UInt32 key = 0x7E5EE93A;
			UInt32 xorValue = 0x5C184B91;

			UInt32 result = 0;
			for(int i = 30; i >= 0; i--) {
				int bit = (int)(encodedValue >> shiftValues[i]) & 0x01;
				if((((key >> 30) ^ bit) & 0x01) != 0) {
					result |= (UInt32)2 << i;
				}
				if((bit & 0x01) != 0) {
					key ^= xorValue;
				}

				key <<= 1;
			}

			return result.ToString("X8");
		}

		struct CheatData
		{
			public int Address;
			public int Value;
			public int CompareValue;
		}
	}
}
