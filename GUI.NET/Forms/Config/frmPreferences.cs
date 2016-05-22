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
using Mesen.GUI.GoogleDriveIntegration;

namespace Mesen.GUI.Forms.Config
{
	public partial class frmPreferences : BaseConfigForm
	{
		public frmPreferences()
		{
			InitializeComponent();

			Entity = ConfigManager.Config.PreferenceInfo;

			AddBinding("DisplayLanguage", cboDisplayLanguage);

			AddBinding("AutomaticallyCheckForUpdates", chkAutomaticallyCheckForUpdates);
			AddBinding("SingleInstance", chkSingleInstance);
			AddBinding("AutoLoadIpsPatches", chkAutoLoadIps);
			AddBinding("AssociateNesFiles", chkNesFormat);
			AddBinding("AssociateFdsFiles", chkFdsFormat);
			AddBinding("AssociateMmoFiles", chkMmoFormat);
			AddBinding("AssociateMstFiles", chkMstFormat);

			AddBinding("UseAlternativeMmc3Irq", chkUseAlternativeMmc3Irq);
			AddBinding("AllowInvalidInput", chkAllowInvalidInput);
			AddBinding("RemoveSpriteLimit", chkRemoveSpriteLimit);

			AddBinding("FdsAutoLoadDisk", chkFdsAutoLoadDisk);
			AddBinding("FdsFastForwardOnLoad", chkFdsFastForwardOnLoad);

			AddBinding("PauseWhenInBackground", chkPauseWhenInBackground);
			AddBinding("AllowBackgroundInput", chkAllowBackgroundInput);

			AddBinding("PauseOnMovieEnd", chkPauseOnMovieEnd);

			UpdateCloudDisplay();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			PreferenceInfo.ApplyConfig();
		}

		private void chkPauseWhenInBackground_CheckedChanged(object sender, EventArgs e)
		{
			chkAllowBackgroundInput.Enabled = !chkPauseWhenInBackground.Checked;
			if(!chkAllowBackgroundInput.Enabled) {
				chkAllowBackgroundInput.Checked = false;
			}
		}

		private void btnOpenMesenFolder_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(ConfigManager.HomeFolder);
		}

		private void UpdateCloudDisplay()
		{
			if(!this.IsDisposed) {
				if(this.InvokeRequired) {
					this.BeginInvoke((MethodInvoker)(() => this.UpdateCloudDisplay()));
				} else {
					this.tlpCloudSaveDesc.Visible = !ConfigManager.Config.PreferenceInfo.CloudSaveIntegration;
					this.tlpCloudSaveEnabled.Visible = ConfigManager.Config.PreferenceInfo.CloudSaveIntegration;
				}
			}
		}

		private void TryEnableSync(bool retry = true)
		{
			if(CloudSyncHelper.EnableSync()) {
				if(!CloudSyncHelper.Sync()) {
					if(retry) {
						TryEnableSync(false);
					} else {
						MesenMsgBox.Show("GoogleDriveIntegrationError", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				} else {
					UpdateCloudDisplay();
				}
			} else {
				MesenMsgBox.Show("GoogleDriveIntegrationError", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnEnableIntegration_Click(object sender, EventArgs e)
		{
			Task.Run(() => TryEnableSync());
		}

		private void btnDisableIntegration_Click(object sender, EventArgs e)
		{
			Task.Run(() => {
				CloudSyncHelper.DisableSync();
				UpdateCloudDisplay();
			});
		}

		private void tmrSyncDateTime_Tick(object sender, EventArgs e)
		{
			btnDisableIntegration.Enabled = !CloudSyncHelper.Syncing;
			btnResync.Enabled = btnDisableIntegration.Enabled;

			if(ConfigManager.Config.PreferenceInfo.CloudLastSync != DateTime.MinValue) {
				lblLastSyncDateTime.Text = ConfigManager.Config.PreferenceInfo.CloudLastSync.ToLongDateString() + " " + ConfigManager.Config.PreferenceInfo.CloudLastSync.ToLongTimeString();
			} else {
				lblLastSyncDateTime.Text = "";
			}
		}

		private void btnResync_Click(object sender, EventArgs e)
		{
			Task.Run(() => CloudSyncHelper.Sync());
		}
	}
}
