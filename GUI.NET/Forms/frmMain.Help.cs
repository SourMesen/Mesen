using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms
{
	public partial class frmMain
	{
		void PerformUpgrade()
		{
			Version newVersion = new Version(InteropEmu.GetMesenVersion());
			Version oldVersion = new Version(ConfigManager.Config.MesenVersion);
			if(oldVersion < newVersion) {
				//Upgrade
				if(oldVersion <= new Version("0.4.1")) {
					//Version 0.4.1-
					//Remove all old cheats (Game matching/CRC logic has been changed and no longer compatible)
					ConfigManager.Config.Cheats = new List<CheatInfo>();
				}

				if(oldVersion <= new Version("0.5.3")) {
					//Version 0.5.3-
					//Reduce sound latency if still using default
					if(ConfigManager.Config.AudioInfo.AudioLatency == 100) {
						//50ms is a fairly safe number - seems to work fine as low as 20ms (varies by computer)
						ConfigManager.Config.AudioInfo.AudioLatency = 50;
					}
				}

				if(oldVersion <= new Version("0.9.0")) {
					//Version 0.9.0-
					if(ConfigManager.Config.VideoInfo.AspectRatio == VideoAspectRatio.Auto) {
						//0.9.0's "Auto" has been renamed to "NoStretching"
						ConfigManager.Config.VideoInfo.AspectRatio = VideoAspectRatio.NoStretching;
					}

					ConfigManager.Config.RecentFiles.Clear();
				}

				if(oldVersion <= new Version("0.9.1")) {
					//Version 0.9.1-
					//Remove all old cheats with a CRC value of 0 (bugged FDS cheats)
					ConfigManager.Config.Cheats = ConfigManager.Config.Cheats.Where((cheat) => cheat.GameCrc != "00000000" && cheat.GameCrc.Length == 8).ToList();
				}
				
				if(oldVersion <= new Version("0.9.4")) {
					ShortcutKeyInfo oldLoadAutoSlot1 = ConfigManager.Config.PreferenceInfo.ShortcutKeys1.Where(o => o.Shortcut == EmulatorShortcut.LoadStateSlot8).FirstOrDefault();
					ShortcutKeyInfo oldLoadAutoSlot2 = ConfigManager.Config.PreferenceInfo.ShortcutKeys2.Where(o => o.Shortcut == EmulatorShortcut.LoadStateSlot8).FirstOrDefault();
					ShortcutKeyInfo newLoadAutoSlot1 = ConfigManager.Config.PreferenceInfo.ShortcutKeys1.Where(o => o.Shortcut == EmulatorShortcut.LoadStateSlotAuto).FirstOrDefault();
					ShortcutKeyInfo newLoadAutoSlot2 = ConfigManager.Config.PreferenceInfo.ShortcutKeys2.Where(o => o.Shortcut == EmulatorShortcut.LoadStateSlotAuto).FirstOrDefault();

					//Copy old "Load State - Slot 8" shortcut to "Load State - Auto Save Slot"
					if(oldLoadAutoSlot1 != null && !oldLoadAutoSlot1.KeyCombination.IsEmpty && newLoadAutoSlot1 == null) {
						ConfigManager.Config.PreferenceInfo.ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlotAuto, oldLoadAutoSlot1.KeyCombination));
						oldLoadAutoSlot1.KeyCombination = new KeyCombination();
					}
					if(oldLoadAutoSlot2 != null && !oldLoadAutoSlot2.KeyCombination.IsEmpty && newLoadAutoSlot2 == null) {
						ConfigManager.Config.PreferenceInfo.ShortcutKeys2.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlotAuto, oldLoadAutoSlot2.KeyCombination));
						oldLoadAutoSlot2.KeyCombination = new KeyCombination();
					}
				}

				if(oldVersion <= new Version("0.9.5")) {
					//Version 0.9.5-
					//Reset the default keys for the new controller types (to reduce settings.xml size)
					KeyPresets presets = new KeyPresets();
					if(ConfigManager.Config.InputInfo.Controllers.Count > 0 && ConfigManager.Config.InputInfo.Controllers[0].Keys.Count > 0) {
						ConfigManager.Config.InputInfo.Controllers[0].Keys[0].ExcitingBoxingButtons = presets.ExcitingBoxing.ExcitingBoxingButtons;
						ConfigManager.Config.InputInfo.Controllers[0].Keys[0].FamilyBasicKeyboardButtons = presets.FamilyBasic.FamilyBasicKeyboardButtons;
						ConfigManager.Config.InputInfo.Controllers[0].Keys[0].JissenMahjongButtons = presets.JissenMahjong.JissenMahjongButtons;
						ConfigManager.Config.InputInfo.Controllers[0].Keys[0].PachinkoButtons = presets.Pachinko.PachinkoButtons;
						ConfigManager.Config.InputInfo.Controllers[0].Keys[0].PartyTapButtons = presets.PartyTap.PartyTapButtons;
						ConfigManager.Config.InputInfo.Controllers[0].Keys[0].PowerPadButtons = presets.PowerPad.PowerPadButtons;
						if(ConfigManager.Config.InputInfo.Controllers.Count > 1 && ConfigManager.Config.InputInfo.Controllers[1].Keys.Count > 0) {
							ConfigManager.Config.InputInfo.Controllers[1].Keys[0].PowerPadButtons = presets.PowerPad.PowerPadButtons;
						}
						ConfigManager.Config.InputInfo.Controllers[0].Keys[0].SuborKeyboardButtons = presets.SuborKeyboard.SuborKeyboardButtons;
						ConfigManager.Config.InputInfo.Controllers[0].Keys[0].BandaiMicrophoneButtons = presets.BandaiMicrophone.BandaiMicrophoneButtons;
					}

					//Set the audio latency setting back to a sane default (since the way the code uses the value has changed)
					if(ConfigManager.Config.AudioInfo.AudioLatency < 60) {
						ConfigManager.Config.AudioInfo.AudioLatency = 60;
					}

					//No reason to keep this disabled by default - enabling it by default makes it easier for new users to install/use HD packs
					ConfigManager.Config.VideoInfo.UseHdPacks = true;
				}

				ConfigManager.Config.MesenVersion = InteropEmu.GetMesenVersion();
				ConfigManager.ApplyChanges();

				_showUpgradeMessage = true;
			}
		}

		private void CheckForUpdates(bool displayResult)
		{
			Task.Run(() => {
				try {
					using(var client = new WebClient()) {
						XmlDocument xmlDoc = new XmlDocument();

						string platform = Program.IsMono ? "linux" : "win";
						xmlDoc.LoadXml(client.DownloadString("http://www.mesen.ca/Services/GetLatestVersion.php?v=" + InteropEmu.GetMesenVersion() + "&p=" + platform + "&l=" + ResourceHelper.GetLanguageCode()));
						Version currentVersion = new Version(InteropEmu.GetMesenVersion());
						Version latestVersion = new Version(xmlDoc.SelectSingleNode("VersionInfo/LatestVersion").InnerText);
						string changeLog = xmlDoc.SelectSingleNode("VersionInfo/ChangeLog").InnerText;
						string fileHash = xmlDoc.SelectSingleNode("VersionInfo/Sha1Hash").InnerText;
						string donateText = xmlDoc.SelectSingleNode("VersionInfo/DonateText")?.InnerText;

						if(latestVersion > currentVersion) {
							this.BeginInvoke((MethodInvoker)(() => {
								using(frmUpdatePrompt frmUpdate = new frmUpdatePrompt(currentVersion, latestVersion, changeLog, fileHash, donateText)) {
									if(frmUpdate.ShowDialog(null, this) == DialogResult.OK) {
										Application.Exit();
									}
								}
							}));
						} else if(displayResult) {
							MesenMsgBox.Show("MesenUpToDate", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
				} catch(Exception ex) {
					if(displayResult) {
						MesenMsgBox.Show("ErrorWhileCheckingUpdates", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
					}
				}
			});
		}

		private void mnuAbout_Click(object sender, EventArgs e)
		{
			using(frmAbout frm = new frmAbout()) {
				frm.ShowDialog(this);
			}
		}

		private void mnuCheckForUpdates_Click(object sender, EventArgs e)
		{
			CheckForUpdates(true);
		}
		
		private void mnuOnlineHelp_Click(object sender, EventArgs e)
		{
			string platform = Program.IsMono ? "linux" : "win";
			Process.Start("http://www.mesen.ca/docs/?v=" + InteropEmu.GetMesenVersion() + "&p=" + platform + "&l=" + ResourceHelper.GetLanguageCode());
		}

		private void mnuReportBug_Click(object sender, EventArgs e)
		{
			Process.Start("http://www.mesen.ca/ReportBug.php");
		}

		private void mnuHelpWindow_Click(object sender, EventArgs e)
		{
			using(frmHelp frm = new frmHelp()) {
				frm.ShowDialog(sender, this);
			}
		}
	}
}
