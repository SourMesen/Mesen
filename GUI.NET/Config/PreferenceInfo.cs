using System;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Config
{
	public class PreferenceInfo
	{
		public Language DisplayLanguage = Language.SystemDefault;

		public bool SingleInstance = true;
		public bool PauseWhenInBackground = false;
		public bool AllowBackgroundInput = false;
		public bool AutoLoadIpsPatches = true;
		public bool AllowInvalidInput = false;
		public bool RemoveSpriteLimit = false;

		public bool DisplayMovieIcons = false;
		public bool HidePauseOverlay = false;
		public bool AutoHideMenu = false;
		public bool DisplayTitleBarInfo = false;

		public bool AutoSave = true;
		public Int32 AutoSaveDelay = 5;
		public bool AutoSaveNotify = false;

		public bool FdsAutoLoadDisk = true;
		public bool FdsFastForwardOnLoad = false;

		public bool AssociateNesFiles = false;
		public bool AssociateFdsFiles = false;
		public bool AssociateMmoFiles = false;
		public bool AssociateMstFiles = false;
		public bool AssociateNsfFiles = false;
		public bool AssociateNsfeFiles = false;
		public bool AssociateUnfFiles = false;

		public bool NsfDisableApuIrqs = true;
		public bool NsfMoveToNextTrackAfterTime = true;
		public Int32 NsfMoveToNextTrackTime = 120;
		public bool NsfAutoDetectSilence = true;
		public Int32 NsfAutoDetectSilenceDelay = 3000;

		public bool PauseOnMovieEnd = true;
		public bool AutomaticallyCheckForUpdates = true;

		public bool CloudSaveIntegration = false;
		public DateTime CloudLastSync = DateTime.MinValue;

		public EmulatorKeyMappings? EmulatorKeySet1;
		public EmulatorKeyMappings? EmulatorKeySet2;

		public bool DisableGameDatabase = false;

		public PreferenceInfo()
		{
		}

		public void InitializeDefaults()
		{
			if(EmulatorKeySet1 == null) {
				EmulatorKeySet1 = new EmulatorKeyMappings() {
					FastForward = InteropEmu.GetKeyCode("Tab")
				};
			}

			if(EmulatorKeySet2 == null) {
				EmulatorKeySet2 = new EmulatorKeyMappings() {
					FastForward = InteropEmu.GetKeyCode("Pad1 R2")
				};
			}
		}

		static public void ApplyConfig()
		{
			PreferenceInfo preferenceInfo = ConfigManager.Config.PreferenceInfo;
			
			if(Program.IsMono) {
				FileAssociationHelper.ConfigureLinuxMimeTypes();
			} else {
				FileAssociationHelper.UpdateFileAssociation("nes", preferenceInfo.AssociateNesFiles);
				FileAssociationHelper.UpdateFileAssociation("fds", preferenceInfo.AssociateFdsFiles);
				FileAssociationHelper.UpdateFileAssociation("mmo", preferenceInfo.AssociateMmoFiles);
				FileAssociationHelper.UpdateFileAssociation("mst", preferenceInfo.AssociateMstFiles);
				FileAssociationHelper.UpdateFileAssociation("nsf", preferenceInfo.AssociateNsfFiles);
				FileAssociationHelper.UpdateFileAssociation("nsfe", preferenceInfo.AssociateNsfeFiles);
				FileAssociationHelper.UpdateFileAssociation("unf", preferenceInfo.AssociateUnfFiles);
			}

			InteropEmu.SetFlag(EmulationFlags.AllowInvalidInput, preferenceInfo.AllowInvalidInput);
			InteropEmu.SetFlag(EmulationFlags.RemoveSpriteLimit, preferenceInfo.RemoveSpriteLimit);
			InteropEmu.SetFlag(EmulationFlags.FdsAutoLoadDisk, preferenceInfo.FdsAutoLoadDisk);
			InteropEmu.SetFlag(EmulationFlags.FdsFastForwardOnLoad, preferenceInfo.FdsFastForwardOnLoad);
			InteropEmu.SetFlag(EmulationFlags.PauseOnMovieEnd, preferenceInfo.PauseOnMovieEnd);
			InteropEmu.SetFlag(EmulationFlags.AllowBackgroundInput, preferenceInfo.AllowBackgroundInput);
			InteropEmu.SetFlag(EmulationFlags.PauseWhenInBackground, preferenceInfo.PauseWhenInBackground);
			InteropEmu.SetFlag(EmulationFlags.DisableGameDatabase, preferenceInfo.DisableGameDatabase);

			InteropEmu.SetFlag(EmulationFlags.HidePauseOverlay, preferenceInfo.HidePauseOverlay);
			InteropEmu.SetFlag(EmulationFlags.DisplayMovieIcons, preferenceInfo.DisplayMovieIcons);

			InteropEmu.NsfSetNsfConfig(preferenceInfo.NsfAutoDetectSilence ? preferenceInfo.NsfAutoDetectSilenceDelay : 0, preferenceInfo.NsfMoveToNextTrackAfterTime ? preferenceInfo.NsfMoveToNextTrackTime : -1, preferenceInfo.NsfDisableApuIrqs);
			InteropEmu.SetAutoSaveOptions(preferenceInfo.AutoSave ? (uint)preferenceInfo.AutoSaveDelay : 0, preferenceInfo.AutoSaveNotify);
			InteropEmu.SetEmulatorKeys(new EmulatorKeyMappingSet() { KeySet1 = preferenceInfo.EmulatorKeySet1.Value, KeySet2 = preferenceInfo.EmulatorKeySet2.Value });
		}
	}
}
