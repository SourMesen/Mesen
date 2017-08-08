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
		private void mnuPause_Click(object sender, EventArgs e)
		{
			PauseEmu();
		}

		private void mnuReset_Click(object sender, EventArgs e)
		{
			ResetEmu();
		}

		private void mnuPowerCycle_Click(object sender, EventArgs e)
		{
			InteropEmu.PowerCycle();
		}

		private void mnuPowerOff_Click(object sender, EventArgs e)
		{
			InteropEmu.Stop();
		}

		private void InitializeFdsDiskMenu()
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)(() => this.InitializeFdsDiskMenu()));
			} else {
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
		}

		private void mnuEjectDisk_Click(object sender, EventArgs e)
		{
			InteropEmu.FdsEjectDisk();
		}

		private void mnuSwitchDiskSide_Click(object sender, EventArgs e)
		{
			InteropEmu.FdsSwitchDiskSide();
		}
		
		private void InitializeVsSystemMenu()
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)(() => InitializeVsSystemMenu()));
			} else {
				sepVsSystem.Visible = InteropEmu.IsVsSystem();
				mnuInsertCoin1.Visible = InteropEmu.IsVsSystem();
				mnuInsertCoin2.Visible = InteropEmu.IsVsSystem();
				mnuVsGameConfig.Visible = InteropEmu.IsVsSystem();
			}
		}

		private void mnuInsertCoin1_Click(object sender, EventArgs e)
		{
			InteropEmu.VsInsertCoin(0);
		}

		private void mnuInsertCoin2_Click(object sender, EventArgs e)
		{
			InteropEmu.VsInsertCoin(1);
		}

		private void ShowVsGameConfig()
		{
			VsConfigInfo configInfo = VsConfigInfo.GetCurrentGameConfig(true);
			using(frmVsGameConfig frm = new frmVsGameConfig(configInfo)) {
				if(frm.ShowDialog(null, this) == DialogResult.OK) {
					VsConfigInfo.ApplyConfig();
				}
			}
		}

		private void mnuVsGameConfig_Click(object sender, EventArgs e)
		{
			ShowVsGameConfig();
		}
	}
}
