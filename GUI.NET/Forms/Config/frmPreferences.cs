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
			AddBinding("DeveloperMode", chkDeveloperMode);

			AddBinding("AssociateNesFiles", chkNesFormat);
			AddBinding("AssociateFdsFiles", chkFdsFormat);
			AddBinding("AssociateMmoFiles", chkMmoFormat);
			AddBinding("AssociateNsfFiles", chkNsfFormat);
			AddBinding("AssociateMstFiles", chkMstFormat);
			AddBinding("AssociateUnfFiles", chkUnfFormat);

			AddBinding("NsfAutoDetectSilence", chkNsfAutoDetectSilence);
			AddBinding("NsfMoveToNextTrackAfterTime", chkNsfMoveToNextTrackAfterTime);
			AddBinding("NsfMoveToNextTrackTime", nudNsfMoveToNextTrackTime);
			AddBinding("NsfAutoDetectSilenceDelay", nudNsfAutoDetectSilenceDelay);
			AddBinding("NsfDisableApuIrqs", chkNsfDisableApuIrqs);

			AddBinding("FdsAutoLoadDisk", chkFdsAutoLoadDisk);
			AddBinding("FdsFastForwardOnLoad", chkFdsFastForwardOnLoad);
			AddBinding("FdsAutoInsertDisk", chkFdsAutoInsertDisk);

			AddBinding("PauseWhenInBackground", chkPauseWhenInBackground);
			AddBinding("PauseWhenInMenusAndConfig", chkPauseInMenuAndConfig);
			AddBinding("PauseWhenInDebuggingTools", chkPauseInDebugger);

			AddBinding("AllowBackgroundInput", chkAllowBackgroundInput);
			AddBinding("ConfirmExitResetPower", chkConfirmExitResetPower);			

			AddBinding("PauseOnMovieEnd", chkPauseOnMovieEnd);

			AddBinding("AlwaysOnTop", chkAlwaysOnTop);

			AddBinding("DisableGameDatabase", chkDisableGameDatabase);
			AddBinding("DisableHighResolutionTimer", chkDisableHighResolutionTimer);
			AddBinding("DisableOsd", chkDisableOsd);

			AddBinding("AutoSave", chkAutoSave);
			AddBinding("AutoSaveDelay", nudAutoSave);
			AddBinding("AutoSaveNotify", chkAutoSaveNotify);
			AddBinding("AllowMismatchingSaveStates", chkAllowMismatchingSaveStates);

			AddBinding("DisplayMovieIcons", chkDisplayMovieIcons);
			AddBinding("HidePauseOverlay", chkHidePauseOverlay);

			AddBinding("AutoHideMenu", chkAutoHideMenu);
			AddBinding("DisplayTitleBarInfo", chkDisplayTitleBarInfo);

			AddBinding("ShowFullPathInRecents", chkShowFullPathInRecents);

			AddBinding("ShowFrameCounter", chkShowFrameCounter);
			AddBinding("ShowGameTimer", chkShowGameTimer);

			AddBinding("DisableGameSelectionScreen", chkDisableGameSelectionScreen);
			AddBinding("GameSelectionScreenResetGame", chkGameSelectionScreenResetGame);

			AddBinding("RewindBufferSize", nudRewindBufferSize);

			AddBinding("ShowVsConfigOnLoad", chkShowVsConfigOnLoad);

			AddBinding("GameFolder", psGame);
			AddBinding("AviFolder", psAvi);
			AddBinding("MovieFolder", psMovies);
			AddBinding("SaveDataFolder", psSaveData);
			AddBinding("SaveStateFolder", psSaveStates);
			AddBinding("ScreenshotFolder", psScreenshots);
			AddBinding("WaveFolder", psWave);

			AddBinding("OverrideGameFolder", chkGameOverride);
			AddBinding("OverrideAviFolder", chkAviOverride);
			AddBinding("OverrideMovieFolder", chkMoviesOverride);
			AddBinding("OverrideSaveDataFolder", chkSaveDataOverride);
			AddBinding("OverrideSaveStateFolder", chkSaveStatesOverride);
			AddBinding("OverrideScreenshotFolder", chkScreenshotsOverride);
			AddBinding("OverrideWaveFolder", chkWaveOverride);

			radStorageDocuments.Checked = ConfigManager.HomeFolder == ConfigManager.DefaultDocumentsFolder;
			radStoragePortable.Checked = !radStorageDocuments.Checked;

			UpdateLocationText();
			UpdateFolderOverrideUi();
			UpdateCloudDisplay();

			if(Program.IsMono) {
				//This option does nothing on Linux, hide it.
				chkDisableHighResolutionTimer.Visible = false;
			}
		}

		protected override void UpdateConfig()
		{
			base.UpdateConfig();
			ctrlEmulatorShortcuts.UpdateConfig();
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

		private void chkAutoSave_CheckedChanged(object sender, EventArgs e)
		{
			nudAutoSave.Enabled = chkAutoSave.Checked;
			chkAutoSaveNotify.Enabled = chkAutoSave.Checked;
		}

		private void btnResetSettings_Click(object sender, EventArgs e)
		{
			if(MesenMsgBox.Show("ResetSettingsConfirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
				ConfigManager.ResetSettings();
				this.Close();
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if(this.DialogResult == DialogResult.OK) {
				if(!ValidateFolderSettings()) {
					e.Cancel = true;
					return;
				}

				if(radStorageDocuments.Checked != (ConfigManager.HomeFolder == ConfigManager.DefaultDocumentsFolder)) {
					//Need to copy files and display confirmation
					string targetFolder = radStorageDocuments.Checked ? ConfigManager.DefaultDocumentsFolder : ConfigManager.DefaultPortableFolder;
					if(MesenMsgBox.Show("CopyMesenDataPrompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, ConfigManager.HomeFolder, targetFolder) == DialogResult.OK) {
						try {
							MigrateData(ConfigManager.HomeFolder, targetFolder);
						} catch(Exception ex) {
							MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
							e.Cancel = true;
						}
					} else {
						e.Cancel = true;
						return;
					}
				}
			} else {
				base.OnFormClosing(e);
			}
		}

		private bool MigrateData(string source, string target)
		{
			using(frmCopyFiles frm = new frmCopyFiles(source, target)) {
				frm.ShowDialog(this);
				if(frm.Exception != null) {
					throw frm.Exception;
				}
			}
			if(File.Exists(Path.Combine(source, "settings.backup.xml"))) {
				File.Delete(Path.Combine(source, "settings.backup.xml"));
			}
			File.Move(Path.Combine(source, "settings.xml"), Path.Combine(source, "settings.backup.xml"));

			ConfigManager.InitHomeFolder();
			ConfigManager.SaveConfig();

			ConfigManager.RestartMesen(true);
			Application.Exit();

			return true;
		}

		private bool ValidateFolderSettings()
		{
			bool result = true;
			List<string> invalidFolders = new List<string>();
			try {
				if(chkGameOverride.Checked && !CheckFolderPermissions(psGame.Text, false)) {
					invalidFolders.Add(chkGameOverride.Text.Replace(":", "").Trim());
				}
				if(chkAviOverride.Checked && !CheckFolderPermissions(psAvi.Text)) {
					invalidFolders.Add(chkAviOverride.Text.Replace(":", "").Trim());
				}
				if(chkMoviesOverride.Checked && !CheckFolderPermissions(psMovies.Text)) {
					invalidFolders.Add(chkMoviesOverride.Text.Replace(":", "").Trim());
				}
				if(chkSaveDataOverride.Checked && !CheckFolderPermissions(psSaveData.Text)) {
					invalidFolders.Add(chkSaveDataOverride.Text.Replace(":", "").Trim());
				}
				if(chkSaveStatesOverride.Checked && !CheckFolderPermissions(psSaveStates.Text)) {
					invalidFolders.Add(chkSaveStatesOverride.Text.Replace(":", "").Trim());
				}
				if(chkScreenshotsOverride.Checked && !CheckFolderPermissions(psScreenshots.Text)) {
					invalidFolders.Add(chkScreenshotsOverride.Text.Replace(":", "").Trim());
				}
				if(chkWaveOverride.Checked && !CheckFolderPermissions(psWave.Text)) {
					invalidFolders.Add(chkWaveOverride.Text.Replace(":", "").Trim());
				}

				result = invalidFolders.Count == 0;
			} catch {
				result = false;
			}
			if(!result) {
				MesenMsgBox.Show("InvalidPaths", MessageBoxButtons.OK, MessageBoxIcon.Error, string.Join(Environment.NewLine, invalidFolders));
			}
			return result;
		}

		private bool CheckFolderPermissions(string folder, bool checkWritePermission = true)
		{
			if(!Directory.Exists(folder)) {
				try {
					if(string.IsNullOrWhiteSpace(folder)) {
						return false;
					}
					Directory.CreateDirectory(folder);
				} catch {
					return false;
				}
			}
			if(checkWritePermission) {
				try {
					File.WriteAllText(Path.Combine(folder, "test.txt"), "");
					File.Delete(Path.Combine(folder, "test.txt"));
				} catch {
					return false;
				}
			}
			return true;
		}

		private void UpdateFolderOverrideUi()
		{
			psGame.Enabled = chkGameOverride.Checked;
			psAvi.Enabled = chkAviOverride.Checked;
			psMovies.Enabled = chkMoviesOverride.Checked;
			psSaveData.Enabled = chkSaveDataOverride.Checked;
			psSaveStates.Enabled = chkSaveStatesOverride.Checked;
			psScreenshots.Enabled = chkScreenshotsOverride.Checked;
			psWave.Enabled = chkWaveOverride.Checked;

			psGame.DisabledText = ResourceHelper.GetMessage("LastFolderUsed");
			psAvi.DisabledText = ConfigManager.DefaultAviFolder;
			psMovies.DisabledText = ConfigManager.DefaultMovieFolder;
			psSaveData.DisabledText = ConfigManager.DefaultSaveDataFolder;
			psSaveStates.DisabledText = ConfigManager.DefaultSaveStateFolder;
			psScreenshots.DisabledText = ConfigManager.DefaultScreenshotFolder;
			psWave.DisabledText = ConfigManager.DefaultWaveFolder;
		}

		private void UpdateLocationText()
		{
			lblLocation.Text = radStorageDocuments.Checked ? ConfigManager.DefaultDocumentsFolder : ConfigManager.DefaultPortableFolder;
		}

		private void chkOverride_CheckedChanged(object sender, EventArgs e)
		{
			UpdateFolderOverrideUi();
		}

		private void radStorageDocuments_CheckedChanged(object sender, EventArgs e)
		{
			UpdateLocationText();
		}
	}
}
