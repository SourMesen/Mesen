using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms.Config;

namespace Mesen.GUI.Forms
{
	public partial class frmMain
	{
		private void mnuGame_DropDownOpening(object sender, EventArgs e)
		{
			InitializeVsSystemMenu();
			InitializeFdsDiskMenu();

			bool hasBarcodeReader = InteropEmu.GetAvailableFeatures().HasFlag(ConsoleFeatures.BarcodeReader);
			mnuInputBarcode.Visible = hasBarcodeReader;

			bool hasTapeRecorder = InteropEmu.GetAvailableFeatures().HasFlag(ConsoleFeatures.TapeRecorder);
			mnuTapeRecorder.Visible = hasTapeRecorder;

			sepBarcode.Visible = hasBarcodeReader || hasTapeRecorder;
		}

		private void InitializeFdsDiskMenu()
		{
			UInt32 sideCount = InteropEmu.FdsGetSideCount();

			mnuSelectDisk.DropDownItems.Clear();

			if(sideCount > 0) {
				for(UInt32 i = 0; i < sideCount; i++) {
					UInt32 diskNumber = i;
					ToolStripItem item = mnuSelectDisk.DropDownItems.Add(ResourceHelper.GetMessage("FdsDiskSide", (diskNumber/2+1).ToString(), (diskNumber % 2 == 0 ? "A" : "B")));
					item.Click += (object sender, EventArgs args) => {
						InteropEmu.FdsInsertDisk(diskNumber);
					};
				}
				sepFdsDisk.Visible = true;
				mnuSelectDisk.Visible = true;
				mnuEjectDisk.Visible = true;
				mnuSwitchDiskSide.Visible = sideCount > 1;
			} else {
				sepFdsDisk.Visible = false;
				mnuSelectDisk.Visible = false;
				mnuEjectDisk.Visible = false;
				mnuSwitchDiskSide.Visible = false;
			}
		}

		private void InitializeVsSystemMenu()
		{
			sepVsSystem.Visible = InteropEmu.IsVsSystem() || InteropEmu.GetDipSwitchCount() > 0;
			mnuInsertCoin1.Visible = InteropEmu.IsVsSystem();
			mnuInsertCoin2.Visible = InteropEmu.IsVsSystem();
			mnuInsertCoin3.Visible = InteropEmu.IsVsDualSystem();
			mnuInsertCoin4.Visible = InteropEmu.IsVsDualSystem();
			mnuGameConfig.Visible = InteropEmu.GetDipSwitchCount() > 0;
		}

		private void ShowGameConfig()
		{
			GameSpecificInfo configInfo = GameSpecificInfo.GetGameSpecificInfo();
			if(configInfo == null) {
				configInfo = GameSpecificInfo.CreateGameSpecificConfig();
			}
			using(frmGameConfig frm = new frmGameConfig(configInfo)) {
				frm.ShowDialog(null, this);
			}
		}

		private void mnuGameConfig_Click(object sender, EventArgs e)
		{
			ShowGameConfig();
		}

		private void mnuLoadTapeFile_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterTapeFiles"));
				ofd.InitialDirectory = ConfigManager.SaveFolder;
				ofd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".fbt";
				if(ofd.ShowDialog(this) == DialogResult.OK) {
					InteropEmu.LoadTapeFile(ofd.FileName);
				}
			}
		}

		private void mnuStartRecordTapeFile_Click(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter(ResourceHelper.GetMessage("FilterTapeFiles"));
				sfd.InitialDirectory = ConfigManager.SaveFolder;
				sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".fbt";
				if(sfd.ShowDialog(this) == DialogResult.OK) {
					InteropEmu.StartRecordingTapeFile(sfd.FileName);
				}
			}
		}

		private void mnuStopRecordTapeFile_Click(object sender, EventArgs e)
		{
			InteropEmu.StopRecordingTapeFile();
		}
	}
}
