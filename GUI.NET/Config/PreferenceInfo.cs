using System;
using System.Collections.Generic;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Config
{
	public class PreferenceInfo
	{
		public Language DisplayLanguage = Language.SystemDefault;

		public bool SingleInstance = true;
		public bool PauseWhenInBackground = false;
		public bool PauseWhenInMenusAndConfig = false;
		public bool PauseWhenInDebuggingTools = false;
		public bool AllowBackgroundInput = false;
		public bool AutoLoadIpsPatches = true;
		public bool AllowInvalidInput = false;
		public bool RemoveSpriteLimit = false;

		public bool DisplayMovieIcons = false;
		public bool HidePauseOverlay = false;
		public bool AutoHideMenu = false;
		public bool DisplayTitleBarInfo = false;

		public bool DisableMouseResize = false;

		public bool AutoSave = true;
		public Int32 AutoSaveDelay = 5;
		public bool AutoSaveNotify = false;
		public bool AllowMismatchingSaveStates = false;

		public bool FdsAutoLoadDisk = true;
		public bool FdsFastForwardOnLoad = false;
		public bool FdsAutoInsertDisk = false;

		public bool AssociateNesFiles = false;
		public bool AssociateFdsFiles = false;
		public bool AssociateMmoFiles = false;
		public bool AssociateNsfFiles = false;
		public bool AssociateMstFiles = false;
		public bool AssociateUnfFiles = false;

		public VsDualOutputOption VsDualVideoOutput = VsDualOutputOption.Both;
		public VsDualOutputOption VsDualAudioOutput = VsDualOutputOption.Both;

		public bool NsfEnableApuIrqs = false;
		public bool NsfMoveToNextTrackAfterTime = true;
		public Int32 NsfMoveToNextTrackTime = 120;
		public bool NsfAutoDetectSilence = true;
		public Int32 NsfAutoDetectSilenceDelay = 3000;
		public bool NsfRepeat = false;
		public bool NsfShuffle = false;

		public bool DisplayDebugInfo = false;

		public bool PauseOnMovieEnd = true;
		public bool AutomaticallyCheckForUpdates = true;

		public bool CloudSaveIntegration = false;
		public DateTime CloudLastSync = DateTime.MinValue;

		public bool DefaultsInitialized = false;
		public List<ShortcutKeyInfo> ShortcutKeys1;
		public List<ShortcutKeyInfo> ShortcutKeys2;

		public bool AlwaysOnTop = false;

		public bool DisableGameDatabase = false;
		public bool DisableHighResolutionTimer = false;
		public bool DisableOsd = false;

		public bool ShowFullPathInRecents = false;

		public bool ShowFrameCounter = false;
		public bool ShowGameTimer = false;
		public bool ShowVsConfigOnLoad = false;

		public bool DisableGameSelectionScreen = false;
		public bool GameSelectionScreenResetGame = false;

		public bool ConfirmExitResetPower = false;

		public bool DeveloperMode = false;

		public UInt32 RewindBufferSize = 300;

		public bool OverrideGameFolder = false;
		public bool OverrideAviFolder = false;
		public bool OverrideMovieFolder = false;
		public bool OverrideSaveDataFolder = false;
		public bool OverrideSaveStateFolder = false;
		public bool OverrideScreenshotFolder = false;
		public bool OverrideWaveFolder = false;

		public string GameFolder = "";
		public string AviFolder = "";
		public string MovieFolder = "";
		public string SaveDataFolder = "";
		public string SaveStateFolder = "";
		public string ScreenshotFolder = "";
		public string WaveFolder = "";

		public PreferenceInfo()
		{
		}

		public void InitializeDefaults()
		{
			if(!DefaultsInitialized) {
				ShortcutKeys1 = new List<ShortcutKeyInfo>();
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.FastForward, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Tab") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.Rewind, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Backspace") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.IncreaseSpeed, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("=") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.DecreaseSpeed, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("-") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.MaxSpeed, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F9") }));

				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.ToggleFps, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F10") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.ToggleFullscreen, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F11") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.ToggleKeyboardMode, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Scroll Lock") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.TakeScreenshot, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F12") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadRandomGame, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Ctrl"), Key2 = InteropEmu.GetKeyCode("Insert") }));

				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SwitchDiskSide, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Ctrl"), Key2 = InteropEmu.GetKeyCode("B") }));

				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.Reset, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Ctrl"), Key2 = InteropEmu.GetKeyCode("R") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.PowerCycle, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Ctrl"), Key2 = InteropEmu.GetKeyCode("T") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.Pause, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Esc") }));

				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SetScale1x, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Alt"), Key2 = InteropEmu.GetKeyCode("1") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SetScale2x, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Alt"), Key2 = InteropEmu.GetKeyCode("2") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SetScale3x, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Alt"), Key2 = InteropEmu.GetKeyCode("3") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SetScale4x, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Alt"), Key2 = InteropEmu.GetKeyCode("4") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SetScale5x, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Alt"), Key2 = InteropEmu.GetKeyCode("5") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SetScale6x, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Alt"), Key2 = InteropEmu.GetKeyCode("6") }));

				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.OpenFile, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Ctrl"), Key2 = InteropEmu.GetKeyCode("O") }));

				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SaveStateSlot1, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Shift"), Key2 = InteropEmu.GetKeyCode("F1") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SaveStateSlot2, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Shift"), Key2 = InteropEmu.GetKeyCode("F2") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SaveStateSlot3, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Shift"), Key2 = InteropEmu.GetKeyCode("F3") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SaveStateSlot4, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Shift"), Key2 = InteropEmu.GetKeyCode("F4") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SaveStateSlot5, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Shift"), Key2 = InteropEmu.GetKeyCode("F5") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SaveStateSlot6, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Shift"), Key2 = InteropEmu.GetKeyCode("F6") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SaveStateSlot7, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Shift"), Key2 = InteropEmu.GetKeyCode("F7") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.SaveStateToFile, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Ctrl"), Key2 = InteropEmu.GetKeyCode("S") }));

				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlot1, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F1") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlot2, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F2") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlot3, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F3") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlot4, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F4") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlot5, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F5") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlot6, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F6") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlot7, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F7") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateSlotAuto, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("F8") }));
				ShortcutKeys1.Add(new ShortcutKeyInfo(EmulatorShortcut.LoadStateFromFile, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Ctrl"), Key2 = InteropEmu.GetKeyCode("L") }));

				ShortcutKeys2 = new List<ShortcutKeyInfo>();
				ShortcutKeys2.Add(new ShortcutKeyInfo(EmulatorShortcut.FastForward, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Pad1 R2") }));
				ShortcutKeys2.Add(new ShortcutKeyInfo(EmulatorShortcut.Rewind, new KeyCombination() { Key1 = InteropEmu.GetKeyCode("Pad1 L2") }));
			}
			DefaultsInitialized = true;
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
				FileAssociationHelper.UpdateFileAssociation("nsfe", preferenceInfo.AssociateNsfFiles);
				FileAssociationHelper.UpdateFileAssociation("unf", preferenceInfo.AssociateUnfFiles);
			}

			InteropEmu.SetFlag(EmulationFlags.AllowInvalidInput, preferenceInfo.AllowInvalidInput);
			InteropEmu.SetFlag(EmulationFlags.RemoveSpriteLimit, preferenceInfo.RemoveSpriteLimit);
			InteropEmu.SetFlag(EmulationFlags.FdsAutoLoadDisk, preferenceInfo.FdsAutoLoadDisk);
			InteropEmu.SetFlag(EmulationFlags.FdsFastForwardOnLoad, preferenceInfo.FdsFastForwardOnLoad);
			InteropEmu.SetFlag(EmulationFlags.FdsAutoInsertDisk, preferenceInfo.FdsAutoInsertDisk);
			InteropEmu.SetFlag(EmulationFlags.PauseOnMovieEnd, preferenceInfo.PauseOnMovieEnd);
			InteropEmu.SetFlag(EmulationFlags.AllowBackgroundInput, preferenceInfo.AllowBackgroundInput);
			InteropEmu.SetFlag(EmulationFlags.PauseWhenInBackground, preferenceInfo.PauseWhenInBackground || preferenceInfo.PauseWhenInMenusAndConfig || preferenceInfo.PauseWhenInDebuggingTools);
			InteropEmu.SetFlag(EmulationFlags.DisableGameDatabase, preferenceInfo.DisableGameDatabase);
			InteropEmu.SetFlag(EmulationFlags.UseHighResolutionTimer, !preferenceInfo.DisableHighResolutionTimer);
			InteropEmu.SetFlag(EmulationFlags.DisableOsd, preferenceInfo.DisableOsd);
			InteropEmu.SetFlag(EmulationFlags.AllowMismatchingSaveStates, preferenceInfo.AllowMismatchingSaveStates);			

			InteropEmu.SetFlag(EmulationFlags.ShowFrameCounter, preferenceInfo.ShowFrameCounter);
			InteropEmu.SetFlag(EmulationFlags.ShowGameTimer, preferenceInfo.ShowGameTimer);

			InteropEmu.SetFlag(EmulationFlags.HidePauseOverlay, preferenceInfo.HidePauseOverlay);
			InteropEmu.SetFlag(EmulationFlags.DisplayMovieIcons, preferenceInfo.DisplayMovieIcons);
			InteropEmu.SetFlag(EmulationFlags.DisableGameSelectionScreen, preferenceInfo.DisableGameSelectionScreen);
			InteropEmu.SetFlag(EmulationFlags.ConfirmExitResetPower, preferenceInfo.ConfirmExitResetPower);

			InteropEmu.NsfSetNsfConfig(preferenceInfo.NsfAutoDetectSilence ? preferenceInfo.NsfAutoDetectSilenceDelay : 0, preferenceInfo.NsfMoveToNextTrackAfterTime ? preferenceInfo.NsfMoveToNextTrackTime : -1, !preferenceInfo.NsfEnableApuIrqs);
			InteropEmu.SetFlag(EmulationFlags.NsfRepeat, preferenceInfo.NsfRepeat);
			InteropEmu.SetFlag(EmulationFlags.NsfShuffle, preferenceInfo.NsfShuffle);

			InteropEmu.SetFlag(EmulationFlags.DisplayDebugInfo, preferenceInfo.DisplayDebugInfo);

			InteropEmu.SetFlag(EmulationFlags.VsDualMuteMaster, preferenceInfo.VsDualAudioOutput == VsDualOutputOption.SlaveOnly);
			InteropEmu.SetFlag(EmulationFlags.VsDualMuteSlave, preferenceInfo.VsDualAudioOutput == VsDualOutputOption.MasterOnly);

			InteropEmu.SetAutoSaveOptions(preferenceInfo.AutoSave ? (uint)preferenceInfo.AutoSaveDelay : 0, preferenceInfo.AutoSaveNotify);

			InteropEmu.ClearShortcutKeys();
			foreach(ShortcutKeyInfo shortcutInfo in preferenceInfo.ShortcutKeys1) {
				InteropEmu.SetShortcutKey(shortcutInfo.Shortcut, shortcutInfo.KeyCombination, 0);
			}
			foreach(ShortcutKeyInfo shortcutInfo in preferenceInfo.ShortcutKeys2) {
				InteropEmu.SetShortcutKey(shortcutInfo.Shortcut, shortcutInfo.KeyCombination, 1);
			}

			InteropEmu.SetRewindBufferSize(preferenceInfo.RewindBufferSize);

			InteropEmu.SetFolderOverrides(ConfigManager.SaveFolder, ConfigManager.SaveStateFolder, ConfigManager.ScreenshotFolder);
		}
	}

	public class ShortcutKeyInfo
	{
		public EmulatorShortcut Shortcut;
		public KeyCombination KeyCombination;

		public ShortcutKeyInfo() { }

		public ShortcutKeyInfo(EmulatorShortcut key, KeyCombination keyCombination)
		{
			Shortcut = key;
			KeyCombination = keyCombination;
		}
	}

	public enum VsDualOutputOption
	{
		Both = 0,
		MasterOnly = 1,
		SlaveOnly = 2
	}
}
