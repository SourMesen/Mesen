using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI
{
	public class InteropEmu
	{
		private const string DLLPath = "MesenCore.dll";
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool TestDll();
		[DllImport(DLLPath)] public static extern void InitDll();

		[DllImport(DLLPath, EntryPoint = "GetMesenVersion")] private static extern UInt32 GetMesenVersionWrapper();

		[DllImport(DLLPath)] public static extern void InitializeEmu([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string homeFolder, IntPtr windowHandle, IntPtr dxViewerHandle, [MarshalAs(UnmanagedType.I1)]bool noAudio, [MarshalAs(UnmanagedType.I1)]bool noVideo, [MarshalAs(UnmanagedType.I1)]bool noInput);
		[DllImport(DLLPath)] public static extern void Release();

		[DllImport(DLLPath)] public static extern void InitializeDualSystem(IntPtr windowHandle, IntPtr viewerHandle);
		[DllImport(DLLPath)] public static extern void ReleaseDualSystemAudioVideo();

		[DllImport(DLLPath)] public static extern void HistoryViewerInitialize(IntPtr windowHandle, IntPtr viewerHandle);
		[DllImport(DLLPath)] public static extern void HistoryViewerRelease();
		[DllImport(DLLPath)] public static extern void HistoryViewerRun();
		[DllImport(DLLPath)] public static extern void HistoryViewerStop();
		[DllImport(DLLPath)] public static extern UInt32 HistoryViewerGetHistoryLength();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool HistoryViewerSaveMovie([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string movieFile, UInt32 startPosition, UInt32 endPosition);
		[DllImport(DLLPath)] public static extern void HistoryViewerSetPosition(UInt32 seekPosition);
		[DllImport(DLLPath)] public static extern void HistoryViewerResumeGameplay(UInt32 seekPosition);
		[DllImport(DLLPath)] public static extern UInt32 HistoryViewerGetPosition();
		[DllImport(DLLPath, EntryPoint = "HistoryViewerGetSegments")] public static extern void HistoryViewerGetSegmentsWrapper(IntPtr segmentBuffer, ref UInt32 bufferSize);

		[DllImport(DLLPath)] public static extern void SetDisplayLanguage(Language lang);

		[DllImport(DLLPath)] public static extern void SetFullscreenMode([MarshalAs(UnmanagedType.I1)]bool fullscreen, IntPtr windowHandle, UInt32 monitorWidth, UInt32 monitorHeight);

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsRunning();
		[DllImport(DLLPath)] public static extern Int32 GetStopCode();

		[DllImport(DLLPath)] public static extern void LoadROM([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string patchFile);
		[DllImport(DLLPath)] public static extern void AddKnownGameFolder([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string folder);
		[DllImport(DLLPath)] public static extern void SetFolderOverrides([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string saveDataFolder, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string saveStateFolder, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string screenshotFolder);
		[DllImport(DLLPath)] public static extern void LoadRecentGame([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filepath, [MarshalAs(UnmanagedType.I1)]bool resetGame);

		[DllImport(DLLPath, EntryPoint = "GetArchiveRomList")] private static extern IntPtr GetArchiveRomListWrapper([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename);

		[DllImport(DLLPath)] public static extern void SetMousePosition(double x, double y);
		[DllImport(DLLPath)] public static extern void SetMouseMovement(Int16 x, Int16 y);
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool HasZapper();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool HasArkanoidPaddle();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool HasFourScore();

		[DllImport(DLLPath)] public static extern void SetControllerType(int port, ControllerType type);
		[DllImport(DLLPath)] public static extern void SetControllerKeys(int port, KeyMappingSet mapping);
		[DllImport(DLLPath)] public static extern void SetZapperDetectionRadius(UInt32 detectionRadius);
		[DllImport(DLLPath)] public static extern void SetExpansionDevice(ExpansionPortDevice device);
		[DllImport(DLLPath)] public static extern void SetConsoleType(ConsoleType type);
		[DllImport(DLLPath)] public static extern void SetMouseSensitivity(MouseDevice device, double sensitivity);
		[DllImport(DLLPath)] public static extern void ClearShortcutKeys();
		[DllImport(DLLPath)] public static extern void SetShortcutKey(EmulatorShortcut shortcut, KeyCombination keyCombination, int keySetIndex);

		[DllImport(DLLPath)] public static extern ControllerType GetControllerType(int port);
		[DllImport(DLLPath)] public static extern ExpansionPortDevice GetExpansionDevice();
		[DllImport(DLLPath)] public static extern ConsoleType GetConsoleType();

		[DllImport(DLLPath)] public static extern void UpdateInputDevices();

		[DllImport(DLLPath)] public static extern ConsoleFeatures GetAvailableFeatures();

		[DllImport(DLLPath, EntryPoint = "GetPressedKeys")] private static extern void GetPressedKeysWrapper(IntPtr keyBuffer);
		[DllImport(DLLPath)] public static extern UInt32 GetKeyCode([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string keyName);
		[DllImport(DLLPath, EntryPoint = "GetKeyName")] private static extern IntPtr GetKeyNameWrapper(UInt32 key);
		[DllImport(DLLPath)] public static extern void SetKeyState(Int32 scanCode, [MarshalAs(UnmanagedType.I1)]bool pressed);
		[DllImport(DLLPath)] public static extern void ResetKeyState();
		[DllImport(DLLPath)] public static extern void DisableAllKeys([MarshalAs(UnmanagedType.I1)]bool disabled);

		[DllImport(DLLPath)] public static extern void Run();
		[DllImport(DLLPath)] public static extern void Resume(ConsoleId consoleId = ConsoleId.Master);
		[DllImport(DLLPath)] public static extern void Pause(ConsoleId consoleId = ConsoleId.Master);
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsPaused(ConsoleId consoleId = ConsoleId.Master);
		[DllImport(DLLPath)] public static extern void Stop();

		[DllImport(DLLPath, EntryPoint = "GetRomInfo")] private static extern UInt32 GetRomInfoWrapper(ref InteropRomInfo romInfo, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename = "");

		[DllImport(DLLPath)] public static extern void PowerCycle();
		[DllImport(DLLPath)] public static extern void Reset();
		[DllImport(DLLPath)] public static extern void ResetLagCounter();

		[DllImport(DLLPath)] public static extern void StartServer(UInt16 port, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string password, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string hostPlayerName);
		[DllImport(DLLPath)] public static extern void StopServer();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsServerRunning();
		[DllImport(DLLPath)] public static extern void Connect([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string host, UInt16 port, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string password, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string playerName, [MarshalAs(UnmanagedType.I1)]bool spectator);
		[DllImport(DLLPath)] public static extern void Disconnect();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsConnected();

		[DllImport(DLLPath)] public static extern Int32 NetPlayGetAvailableControllers();
		[DllImport(DLLPath)] public static extern void NetPlaySelectController(Int32 controllerPort);
		[DllImport(DLLPath)] public static extern ControllerType NetPlayGetControllerType(Int32 controllerPort);
		[DllImport(DLLPath)] public static extern Int32 NetPlayGetControllerPort();

		[DllImport(DLLPath)] public static extern void TakeScreenshot();

		[DllImport(DLLPath)] public static extern IntPtr RegisterNotificationCallback(ConsoleId consoleId, NotificationListener.NotificationCallback callback);
		[DllImport(DLLPath)] public static extern void UnregisterNotificationCallback(IntPtr notificationListener);

		[DllImport(DLLPath)] public static extern void DisplayMessage([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string title, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string message, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string param1 = null);
		[DllImport(DLLPath, EntryPoint = "GetLog")] private static extern IntPtr GetLogWrapper();
		[DllImport(DLLPath)] public static extern void WriteLogEntry([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string message);

		[DllImport(DLLPath)] public static extern void MoviePlay([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void MovieRecord(ref RecordMovieOptions options);
		[DllImport(DLLPath)] public static extern void MovieStop();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool MoviePlaying();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool MovieRecording();

		[DllImport(DLLPath)] public static extern void AviRecord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename, VideoCodec codec, UInt32 compressionLevel);
		[DllImport(DLLPath)] public static extern void AviStop();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool AviIsRecording();

		[DllImport(DLLPath)] public static extern void WaveRecord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void WaveStop();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool WaveIsRecording();

		[DllImport(DLLPath)] public static extern Int32 RunRecordedTest([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern Int32 RunAutomaticTest([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void RomTestRecord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename, [MarshalAs(UnmanagedType.I1)]bool reset);
		[DllImport(DLLPath)] public static extern void RomTestRecordFromMovie([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string testFilename, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string movieFilename);
		[DllImport(DLLPath)] public static extern void RomTestRecordFromTest([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string newTestFilename, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string existingTestFilename);
		[DllImport(DLLPath)] public static extern void RomTestStop();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool RomTestRecording();

		[DllImport(DLLPath)] public static extern void SaveState(UInt32 stateIndex);
		[DllImport(DLLPath)] public static extern void LoadState(UInt32 stateIndex);
		[DllImport(DLLPath)] public static extern void SaveStateFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filepath);
		[DllImport(DLLPath)] public static extern void LoadStateFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filepath);
		[DllImport(DLLPath)] public static extern Int64 GetStateInfo(UInt32 stateIndex);

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsNsf();
		[DllImport(DLLPath)] public static extern void NsfSelectTrack(Byte trackNumber);
		[DllImport(DLLPath)] public static extern Int32 NsfGetCurrentTrack();
		[DllImport(DLLPath, EntryPoint = "NsfGetHeader")] private static extern void NsfGetHeaderWrapper(out NsfHeader header);
		[DllImport(DLLPath)] public static extern void NsfSetNsfConfig(Int32 autoDetectSilenceDelay, Int32 moveToNextTrackTime, [MarshalAs(UnmanagedType.I1)]bool disableApuIrqs);

		[DllImport(DLLPath)] public static extern UInt32 FdsGetSideCount();
		[DllImport(DLLPath)] public static extern void FdsEjectDisk();
		[DllImport(DLLPath)] public static extern void FdsInsertDisk(UInt32 diskNumber);
		[DllImport(DLLPath)] public static extern void FdsSwitchDiskSide();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool FdsIsAutoInsertDiskEnabled();

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsVsSystem();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsVsDualSystem();
		[DllImport(DLLPath)] public static extern void VsInsertCoin(UInt32 port);

		[DllImport(DLLPath)] public static extern void SetDipSwitches(UInt32 dipSwitches);

		[DllImport(DLLPath)] public static extern void InputBarcode(UInt64 barcode, Int32 digitCount);

		[DllImport(DLLPath)] public static extern void LoadTapeFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filepath);
		[DllImport(DLLPath)] public static extern void StartRecordingTapeFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filepath);
		[DllImport(DLLPath)] public static extern void StopRecordingTapeFile();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsRecordingTapeFile();

		[DllImport(DLLPath)] public static extern void SetCheats([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]InteropCheatInfo[] cheats, UInt32 length);

		[DllImport(DLLPath)] public static extern void SetOsdState([MarshalAs(UnmanagedType.I1)]bool enabled);
		[DllImport(DLLPath)] public static extern void SetGameDatabaseState([MarshalAs(UnmanagedType.I1)]bool enabled);

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool CheckFlag(EmulationFlags flag);
		[DllImport(DLLPath)] private static extern void SetFlags(EmulationFlags flags);
		[DllImport(DLLPath)] private static extern void ClearFlags(EmulationFlags flags);
		[DllImport(DLLPath)] public static extern void SetRamPowerOnState(RamPowerOnState state);
		[DllImport(DLLPath)] public static extern void SetMasterVolume(double volume, double volumeReduction, ConsoleId consoleId = ConsoleId.Master);
		[DllImport(DLLPath)] public static extern void SetChannelVolume(AudioChannel channel, double volume);
		[DllImport(DLLPath)] public static extern void SetChannelPanning(AudioChannel channel, double panning);
		[DllImport(DLLPath)] public static extern void SetEqualizerFilterType(EqualizerFilterType filter);
		[DllImport(DLLPath)] public static extern void SetEqualizerBands([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]double[] bands, UInt32 length);
		[DllImport(DLLPath)] public static extern void SetBandGain(int band, double gain);
		[DllImport(DLLPath)] public static extern void SetSampleRate(UInt32 sampleRate);
		[DllImport(DLLPath)] public static extern void SetAudioLatency(UInt32 msLatency);
		[DllImport(DLLPath)] public static extern void SetStereoFilter(StereoFilter stereoFilter);
		[DllImport(DLLPath)] public static extern void SetStereoDelay(Int32 delay);
		[DllImport(DLLPath)] public static extern void SetStereoPanningAngle(double angle);
		[DllImport(DLLPath)] public static extern void SetReverbParameters(double strength, double delay);
		[DllImport(DLLPath)] public static extern void SetCrossFeedRatio(UInt32 ratio);

		[DllImport(DLLPath)] public static extern NesModel GetNesModel();
		[DllImport(DLLPath)] public static extern void SetNesModel(NesModel model);
		[DllImport(DLLPath)] public static extern void SetEmulationSpeed(UInt32 emulationSpeed);
		[DllImport(DLLPath)] public static extern void IncreaseEmulationSpeed();
		[DllImport(DLLPath)] public static extern void DecreaseEmulationSpeed();
		[DllImport(DLLPath)] public static extern UInt32 GetEmulationSpeed();
		[DllImport(DLLPath)] public static extern void SetTurboRewindSpeed(UInt32 turboSpeed, UInt32 rewindSpeed);
		[DllImport(DLLPath)] public static extern void SetRewindBufferSize(UInt32 seconds);
		[DllImport(DLLPath)] public static extern void SetOverclockRate(UInt32 overclockRate, [MarshalAs(UnmanagedType.I1)]bool adjustApu);
		[DllImport(DLLPath)] public static extern void SetPpuNmiConfig(UInt32 extraScanlinesBeforeNmi, UInt32 extraScanlineAfterNmi);
		[DllImport(DLLPath)] public static extern void SetOverscanDimensions(UInt32 left, UInt32 right, UInt32 top, UInt32 bottom);
		[DllImport(DLLPath)] public static extern void SetVideoScale(double scale, ConsoleId consoleId = ConsoleId.Master);
		[DllImport(DLLPath)] public static extern void SetScreenRotation(UInt32 angle);
		[DllImport(DLLPath)] public static extern void SetExclusiveRefreshRate(UInt32 refreshRate);
		[DllImport(DLLPath)] public static extern void SetVideoAspectRatio(VideoAspectRatio aspectRatio, double customRatio);
		[DllImport(DLLPath)] public static extern void SetVideoFilter(VideoFilterType filter);
		[DllImport(DLLPath)] public static extern void SetVideoResizeFilter(VideoResizeFilter filter);
		[DllImport(DLLPath)] public static extern void SetRgbPalette(byte[] palette);
		[DllImport(DLLPath)] public static extern void SetPictureSettings(double brightness, double contrast, double saturation, double hue, double scanlineIntensity);
		[DllImport(DLLPath)] public static extern void SetNtscFilterSettings(double artifacts, double bleed, double fringing, double gamma, double resolution, double sharpness, [MarshalAs(UnmanagedType.I1)]bool mergeFields, double yFilterLength, double iFilterLength, double qFilterLength, [MarshalAs(UnmanagedType.I1)]bool verticalBlend);
		[DllImport(DLLPath)] public static extern void SetInputDisplaySettings(byte visiblePorts, InputDisplayPosition displayPosition, [MarshalAs(UnmanagedType.I1)]bool displayHorizontally);
		[DllImport(DLLPath)] public static extern void SetAutoSaveOptions(UInt32 delayInMinutes, [MarshalAs(UnmanagedType.I1)]bool showMessage);
		[DllImport(DLLPath)] public static extern void SetPauseScreenMessage([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string message);

		[DllImport(DLLPath, EntryPoint = "GetRgbPalette")] private static extern void GetRgbPaletteWrapper(IntPtr paletteBuffer);

		[DllImport(DLLPath, EntryPoint = "GetScreenSize")] private static extern void GetScreenSizeWrapper(ConsoleId consoleId, out ScreenSize size, [MarshalAs(UnmanagedType.I1)]bool ignoreScale);

		[DllImport(DLLPath, EntryPoint = "GetAudioDevices")] private static extern IntPtr GetAudioDevicesWrapper();
		[DllImport(DLLPath)] public static extern void SetAudioDevice(string audioDevice);

		[DllImport(DLLPath)] public static extern void DebugSetDebuggerConsole(ConsoleId consoleId);
		[DllImport(DLLPath)] public static extern void DebugInitialize();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugIsDebuggerRunning();
		[DllImport(DLLPath)] public static extern void DebugRelease();
		[DllImport(DLLPath)] public static extern void DebugSetFlags(DebuggerFlags flags);
		[DllImport(DLLPath)] public static extern void DebugGetState(ref DebugState state);
		[DllImport(DLLPath)] public static extern void DebugGetApuState(ref ApuState state);
		[DllImport(DLLPath)] public static extern void DebugSetState(DebugState state);
		[DllImport(DLLPath)] public static extern void DebugSetBreakpoints([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]InteropBreakpoint[] breakpoints, UInt32 length);
		[DllImport(DLLPath)] public static extern void DebugSetLabel(UInt32 address, AddressType addressType, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string label, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string comment);
		[DllImport(DLLPath)] public static extern void DebugDeleteLabels();
		[DllImport(DLLPath)] public static extern void DebugStep(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugPpuStep(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugStepCycles(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugStepOut();
		[DllImport(DLLPath)] public static extern void DebugStepOver();
		[DllImport(DLLPath)] public static extern void DebugStepBack();
		[DllImport(DLLPath)] public static extern void DebugBreakOnScanline(Int32 scanline);
		[DllImport(DLLPath)] public static extern void DebugRun();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugIsExecutionStopped();
		[DllImport(DLLPath)] public static extern Int32 DebugGetRelativeAddress(UInt32 absoluteAddr, AddressType type);
		[DllImport(DLLPath)] public static extern Int32 DebugFindSubEntryPoint(UInt16 relativeAddr);
		[DllImport(DLLPath)] public static extern Int32 DebugGetAbsoluteAddress(UInt32 relativeAddr);
		[DllImport(DLLPath)] public static extern Int32 DebugGetAbsoluteChrAddress(UInt32 relativeAddr);
		[DllImport(DLLPath)] public static extern Int32 DebugGetRelativeChrAddress(UInt32 absoluteAddr);
		[DllImport(DLLPath)] public static extern Int32 DebugGetMemorySize(DebugMemoryType type);
		[DllImport(DLLPath)] public static extern Byte DebugGetMemoryValue(DebugMemoryType type, UInt32 address);
		[DllImport(DLLPath)] public static extern void DebugSetMemoryValue(DebugMemoryType type, UInt32 address, byte value);
		[DllImport(DLLPath)] public static extern void DebugSetInputOverride(Int32 port, Int32 state);

		[DllImport(DLLPath)] public static extern Int32 DebugLoadScript([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string name, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string content, Int32 scriptId = -1);
		[DllImport(DLLPath)] public static extern void DebugRemoveScript(Int32 scriptId);
		[DllImport(DLLPath, EntryPoint = "DebugGetScriptLog")] private static extern IntPtr DebugGetScriptLogWrapper(Int32 scriptId);
		public static string DebugGetScriptLog(Int32 scriptId) { return PtrToStringUtf8(InteropEmu.DebugGetScriptLogWrapper(scriptId)).Replace("\n", Environment.NewLine); }

		[DllImport(DLLPath, EntryPoint = "DebugStartCodeRunner")] private static extern void DebugStartCodeRunnerWrapper(IntPtr byteCode, Int32 codeLength);
		public static void DebugStartCodeRunner(byte[] data)
		{
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				InteropEmu.DebugStartCodeRunnerWrapper(handle.AddrOfPinnedObject(), data.Length);
			} finally {
				handle.Free();
			}
		}

		[DllImport(DLLPath, EntryPoint = "DebugSetMemoryValues")] private static extern void DebugSetMemoryValuesWrapper(DebugMemoryType type, UInt32 address, IntPtr data, Int32 length);
		public static void DebugSetMemoryValues(DebugMemoryType type, UInt32 address, byte[] data)
		{
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				InteropEmu.DebugSetMemoryValuesWrapper(type, address, handle.AddrOfPinnedObject(), data.Length);
			} finally {
				handle.Free();
			}
		}

		[DllImport(DLLPath)] public static extern void DebugGetAbsoluteAddressAndType(UInt32 relativeAddr, AddressTypeInfo addressTypeInfo);
		[DllImport(DLLPath)] public static extern void DebugSetPpuViewerScanlineCycle(Int32 ppuViewerId, Int32 scanline, Int32 cycle);
		[DllImport(DLLPath)] public static extern void DebugClearPpuViewerSettings(Int32 ppuViewerId);

		[DllImport(DLLPath)] public static extern void DebugSetFreezeState(UInt16 address, [MarshalAs(UnmanagedType.I1)]bool frozen);

		[DllImport(DLLPath)] public static extern void DebugSetNextStatement(UInt16 addr);
		[DllImport(DLLPath)] public static extern Int32 DebugEvaluateExpression([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string expression, out EvalResultType resultType);

		[DllImport(DLLPath)] public static extern void DebugStartTraceLogger([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void DebugStopTraceLogger();
		[DllImport(DLLPath)] public static extern void DebugSetTraceOptions(InteropTraceLoggerOptions options);
		[DllImport(DLLPath, EntryPoint = "DebugGetExecutionTrace")] private static extern IntPtr DebugGetExecutionTraceWrapper(UInt32 lineCount);
		public static string DebugGetExecutionTrace(UInt32 lineCount) { return PtrToStringUtf8(InteropEmu.DebugGetExecutionTraceWrapper(lineCount)); }

		[DllImport(DLLPath)] public static extern void DebugMarkPrgBytesAs(UInt32 start, UInt32 end, CdlPrgFlags type);
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugLoadCdlFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string cdlFilepath);
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugSaveCdlFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string cdlFilepath);
		[DllImport(DLLPath)] public static extern void DebugGetCdlRatios(ref CdlRatios ratios);
		[DllImport(DLLPath)] public static extern void DebugResetCdlLog();
		[DllImport(DLLPath)] public static extern void DebugResetMemoryAccessCounts();
		[DllImport(DLLPath)] public static extern void DebugResetProfiler();

		[DllImport(DLLPath)] public static extern void DebugRevertPrgChrChanges();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugHasPrgChrChanges();

		[DllImport(DLLPath)] public static extern void DebugPerformUndo();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugHasUndoHistory();

		[DllImport(DLLPath, EntryPoint = "DebugGetNesHeader")] public static extern void DebugGetNesHeaderWrapper(IntPtr headerBuffer);
		public static byte[] DebugGetNesHeader()
		{
			byte[] header = new byte[16];
			GCHandle handle = GCHandle.Alloc(header, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetNesHeaderWrapper(handle.AddrOfPinnedObject());
			} finally {
				handle.Free();
			}
			return header;
		}

		[DllImport(DLLPath, EntryPoint = "DebugSaveRomToDisk")] public static extern void DebugSaveRomToDiskWrapper([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename, [MarshalAs(UnmanagedType.I1)]bool saveAsIps, IntPtr headerBuffer, CdlStripFlag cdlStripFlag);
		public static void DebugSaveRomToDisk(string filename, bool saveAsIps = false, byte[] header = null, CdlStripFlag cdlStripFlag = CdlStripFlag.StripNone)
		{
			if(header != null) {
				GCHandle handle = GCHandle.Alloc(header, GCHandleType.Pinned);
				try {
					InteropEmu.DebugSaveRomToDiskWrapper(filename, saveAsIps, handle.AddrOfPinnedObject(), cdlStripFlag);
				} finally {
					handle.Free();
				}
			} else {
				InteropEmu.DebugSaveRomToDiskWrapper(filename, saveAsIps, IntPtr.Zero, cdlStripFlag);
			}
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetCode")] private static extern IntPtr DebugGetCodeWrapper(ref UInt32 length);
		public static string DebugGetCode(bool forceRefresh)
		{
			UInt32 length = forceRefresh ? UInt32.MaxValue : 0;
			IntPtr ptrCodeString = InteropEmu.DebugGetCodeWrapper(ref length);
			if(ptrCodeString == IntPtr.Zero) {
				return null;
			} else {
				return PtrToStringUtf8(ptrCodeString, length);
			}
		}

		[DllImport(DLLPath, EntryPoint = "DebugAssembleCode")] private static extern UInt32 DebugAssembleCodeWrapper([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string code, UInt16 startAddress, IntPtr assembledCodeBuffer);
		public static Int16[] DebugAssembleCode(string code, UInt16 startAddress)
		{
			code = code.Replace(Environment.NewLine, "\n");
			int lineCount = code.Count(c => c == '\n');

			Int16[] assembledCode = new Int16[(lineCount + 1) * 4];
			UInt32 size = 0;

			GCHandle hAssembledCode = GCHandle.Alloc(assembledCode, GCHandleType.Pinned);
			try {
				size = InteropEmu.DebugAssembleCodeWrapper(code, startAddress, hAssembledCode.AddrOfPinnedObject());
			} finally {
				hAssembledCode.Free();
			}

			Array.Resize(ref assembledCode, (int)size);
			return assembledCode;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetMemoryState")] private static extern UInt32 DebugGetMemoryStateWrapper(DebugMemoryType type, IntPtr buffer);
		public static byte[] DebugGetMemoryState(DebugMemoryType type)
		{
			byte[] buffer = new byte[InteropEmu.DebugGetMemorySize(type)];
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				UInt32 memorySize = InteropEmu.DebugGetMemoryStateWrapper(type, handle.AddrOfPinnedObject());
				Array.Resize(ref buffer, (int)memorySize);
			} finally {
				handle.Free();
			}
			return buffer;
		}

		[DllImport(DLLPath, EntryPoint = "DebugSetMemoryState")] private static extern void DebugSetMemoryStateWrapper(DebugMemoryType type, IntPtr buffer);
		public static void DebugSetMemoryState(DebugMemoryType type, byte[] data)
		{
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				InteropEmu.DebugSetMemoryStateWrapper(type, handle.AddrOfPinnedObject());
			} finally {
				handle.Free();
			}
		}

		public static byte[] DebugGetInternalRam()
		{
			byte[] buffer = new byte[0x800];
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				UInt32 memorySize = InteropEmu.DebugGetMemoryStateWrapper(DebugMemoryType.InternalRam, handle.AddrOfPinnedObject());
				Array.Resize(ref buffer, (int)memorySize);
			} finally {
				handle.Free();
			}
			return buffer;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetNametable")] private static extern void DebugGetNametableWrapper(UInt32 nametableIndex, [MarshalAs(UnmanagedType.I1)]bool useGrayscalePalette, IntPtr frameBuffer, IntPtr tileData, IntPtr attributeData);
		public static void DebugGetNametable(int nametableIndex, bool useGrayscalePalette, out byte[] frameData, out byte[] tileData, out byte[] attributeData)
		{
			frameData = new byte[256*240*4];
			tileData = new byte[32*30];
			attributeData = new byte[32*30];

			GCHandle hFrameData = GCHandle.Alloc(frameData, GCHandleType.Pinned);
			GCHandle hTileData = GCHandle.Alloc(tileData, GCHandleType.Pinned);
			GCHandle hAttributeData = GCHandle.Alloc(attributeData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetNametableWrapper((UInt32)nametableIndex, useGrayscalePalette, hFrameData.AddrOfPinnedObject(), hTileData.AddrOfPinnedObject(), hAttributeData.AddrOfPinnedObject());
			} finally {
				hFrameData.Free();
				hTileData.Free();
				hAttributeData.Free();
			}
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetChrBank")] private static extern void DebugGetChrBankWrapper(UInt32 bankIndex, IntPtr frameBuffer, Byte palette, [MarshalAs(UnmanagedType.I1)]bool largeSprites, CdlHighlightType highlightType, [MarshalAs(UnmanagedType.I1)]bool useAutoPalette, [MarshalAs(UnmanagedType.I1)]bool showSingleColorTilesInGrayscale, IntPtr paletteBuffer);
		public static byte[] DebugGetChrBank(int bankIndex, int palette, bool largeSprites, CdlHighlightType highlightType, bool useAutoPalette, bool showSingleColorTilesInGrayscale, out UInt32[] paletteData)
		{
			byte[] frameData = new byte[128*128*4];
			paletteData = new UInt32[16*16];

			GCHandle hFrameData = GCHandle.Alloc(frameData, GCHandleType.Pinned);
			GCHandle hPaletteData = GCHandle.Alloc(paletteData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetChrBankWrapper((UInt32)bankIndex, hFrameData.AddrOfPinnedObject(), (Byte)palette, largeSprites, highlightType, useAutoPalette, showSingleColorTilesInGrayscale, hPaletteData.AddrOfPinnedObject());
			} finally {
				hFrameData.Free();
				hPaletteData.Free();
			}

			return frameData;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetSprites")] private static extern void DebugGetSpritesWrapper(IntPtr frameBuffer);
		public static byte[] DebugGetSprites()
		{
			byte[] frameData = new byte[64*128*4];

			GCHandle hFrameData = GCHandle.Alloc(frameData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetSpritesWrapper(hFrameData.AddrOfPinnedObject());
			} finally {
				hFrameData.Free();
			}

			return frameData;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetPalette")] private static extern void DebugGetPaletteWrapper(IntPtr frameBuffer);
		public static int[] DebugGetPalette()
		{
			int[] frameData = new int[4*8];

			GCHandle hFrameData = GCHandle.Alloc(frameData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetPaletteWrapper(hFrameData.AddrOfPinnedObject());
			} finally {
				hFrameData.Free();
			}

			return frameData;
		}

		[DllImport(DLLPath)] private static extern UInt32 DebugGetDebugEventCount([MarshalAs(UnmanagedType.I1)]bool returnPreviousFrameData);
		[DllImport(DLLPath, EntryPoint = "DebugGetDebugEvents")] private static extern void DebugGetDebugEventsWrapper(IntPtr frameBuffer, IntPtr infoArray, ref UInt32 maxEventCount, [MarshalAs(UnmanagedType.I1)]bool returnPreviousFrameData);
		public static void DebugGetDebugEvents(bool returnPreviousFrameData, out byte[] pictureData, out DebugEventInfo[] debugEvents)
		{
			pictureData = new byte[256 * 240 * 4];
			UInt32 maxEventCount = DebugGetDebugEventCount(returnPreviousFrameData);
			debugEvents = new DebugEventInfo[maxEventCount];

			GCHandle hPictureData = GCHandle.Alloc(pictureData, GCHandleType.Pinned);
			GCHandle hDebugEvents = GCHandle.Alloc(debugEvents, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetDebugEventsWrapper(hPictureData.AddrOfPinnedObject(), hDebugEvents.AddrOfPinnedObject(), ref maxEventCount, returnPreviousFrameData);
			} finally {
				hPictureData.Free();
				hDebugEvents.Free();
			}

			if(maxEventCount < debugEvents.Length) {
				//Remove the excess from the array if needed
				Array.Resize(ref debugEvents, (int)maxEventCount);
			}
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetProfilerData")] private static extern void DebugGetProfilerDataWrapper(IntPtr profilerData, ProfilerDataType dataType);
		public static Int64[] DebugGetProfilerData(ProfilerDataType dataType)
		{
			Int64[] profileData = new Int64[InteropEmu.DebugGetMemorySize(DebugMemoryType.PrgRom) + 2];

			GCHandle hProfilerData = GCHandle.Alloc(profileData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetProfilerDataWrapper(hProfilerData.AddrOfPinnedObject(), dataType);
			} finally {
				hProfilerData.Free();
			}

			return profileData;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetMemoryAccessCounts")] private static extern void DebugGetMemoryAccessCountsWrapper(AddressType type, MemoryOperationType operationType, IntPtr counts, [MarshalAs(UnmanagedType.I1)]bool forUninitReads);
		public static Int32[] DebugGetMemoryAccessCounts(AddressType type, MemoryOperationType operationType, bool forUninitReads)
		{
			int size = 0;
			switch(type) {
				case AddressType.InternalRam: size = 0x2000; break;
				case AddressType.PrgRom: size = InteropEmu.DebugGetMemorySize(DebugMemoryType.PrgRom); break;
				case AddressType.WorkRam: size = InteropEmu.DebugGetMemorySize(DebugMemoryType.WorkRam); break;
				case AddressType.SaveRam: size = InteropEmu.DebugGetMemorySize(DebugMemoryType.SaveRam); break;
			}

			Int32[] counts = new Int32[size];

			if(size > 0) {
				GCHandle hCounts = GCHandle.Alloc(counts, GCHandleType.Pinned);
				try {
					InteropEmu.DebugGetMemoryAccessCountsWrapper(type, operationType, hCounts.AddrOfPinnedObject(), forUninitReads);
				} finally {
					hCounts.Free();
				}
			}

			return counts;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetMemoryAccessStamps")] private static extern void DebugGetMemoryAccessStampsWrapper(UInt32 offset, UInt32 length, DebugMemoryType type, MemoryOperationType operationType, IntPtr stamps);
		public static Int32[] DebugGetMemoryAccessStamps(UInt32 offset, UInt32 length, DebugMemoryType type, MemoryOperationType operationType)
		{
			Int32[] stamps = new Int32[length];

			GCHandle hStamps = GCHandle.Alloc(stamps, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetMemoryAccessStampsWrapper(offset, length, type, operationType, hStamps.AddrOfPinnedObject());
			} finally {
				hStamps.Free();
			}

			return stamps;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetMemoryAccessCountsEx")] private static extern void DebugGetMemoryAccessCountsExWrapper(UInt32 offset, UInt32 length, DebugMemoryType type, MemoryOperationType operationType, IntPtr counts);
		public static Int32[] DebugGetMemoryAccessCountsEx(UInt32 offset, UInt32 length, DebugMemoryType type, MemoryOperationType operationType)
		{
			Int32[] counts = new Int32[length];

			GCHandle hResult = GCHandle.Alloc(counts, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetMemoryAccessCountsExWrapper(offset, length, type, operationType, hResult.AddrOfPinnedObject());
			} finally {
				hResult.Free();
			}

			return counts;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetFreezeState")] private static extern void DebugGetFreezeStateWrapper(UInt16 startAddress, UInt16 length, IntPtr freezeState);
		public static bool[] DebugGetFreezeState(UInt16 startAddress, UInt16 length)
		{
			bool[] freezeState = new bool[length];

			GCHandle hFreezeState = GCHandle.Alloc(freezeState, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetFreezeStateWrapper(startAddress, length, hFreezeState.AddrOfPinnedObject());
			} finally {
				hFreezeState.Free();
			}

			return freezeState;
		}

		[DllImport(DLLPath, EntryPoint = "DebugSetCdlData")] private static extern void DebugSetCdlDataWrapper(IntPtr cdlData, UInt32 length);
		public static void DebugSetCdlData(byte[] cdlData)
		{
			GCHandle hResult = GCHandle.Alloc(cdlData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugSetCdlDataWrapper(hResult.AddrOfPinnedObject(), (UInt32)cdlData.Length);
			} finally {
				hResult.Free();
			}
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetCdlData")] private static extern void DebugGetCdlDataWrapper(UInt32 offset, UInt32 length, DebugMemoryType type, IntPtr counts);
		public static byte[] DebugGetCdlData(UInt32 offset, UInt32 length, DebugMemoryType type)
		{
			byte[] cdlData = new byte[length];

			GCHandle hResult = GCHandle.Alloc(cdlData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetCdlDataWrapper(offset, length, type, hResult.AddrOfPinnedObject());
			} finally {
				hResult.Free();
			}

			return cdlData;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetCallstack")] private static extern void DebugGetCallstackWrapper(IntPtr callstackArray, ref UInt32 callstackSize);
		public static StackFrameInfo[] DebugGetCallstack()
		{
			StackFrameInfo[] callstack = new StackFrameInfo[512];
			UInt32 callstackSize = 0;

			GCHandle hCallstack = GCHandle.Alloc(callstack, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetCallstackWrapper(hCallstack.AddrOfPinnedObject(), ref callstackSize);
			} finally {
				hCallstack.Free();
			}
			Array.Resize(ref callstack, (int)callstackSize);

			return callstack;
		}

		[DllImport(DLLPath)] private static extern Int32 DebugGetFunctionEntryPointCount();
		[DllImport(DLLPath, EntryPoint = "DebugGetFunctionEntryPoints")] private static extern void DebugGetFunctionEntryPointsWrapper(IntPtr callstackAbsolute, Int32 maxCount);
		public static Int32[] DebugGetFunctionEntryPoints()
		{
			int maxCount = DebugGetFunctionEntryPointCount();
			Int32[] entryPoints = new Int32[maxCount+1];

			GCHandle hEntryPoints = GCHandle.Alloc(entryPoints, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetFunctionEntryPointsWrapper(hEntryPoints.AddrOfPinnedObject(), maxCount+1);
			} finally {
				hEntryPoints.Free();
			}

			return entryPoints;
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetPpuScroll")] private static extern UInt32 DebugGetPpuScrollWrapper();
		public static void DebugGetPpuScroll(out int xScroll, out int yScroll)
		{
			UInt32 ppuScroll = InteropEmu.DebugGetPpuScrollWrapper();
			xScroll = (int)ppuScroll & 0xFFFF;
			yScroll = (int)(ppuScroll >> 16) & 0xFFFF;
		}

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsHdPpu();

		[DllImport(DLLPath)] public static extern void HdBuilderStartRecording(
			[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string saveFolder,
			ScaleFilterType filterType,
			UInt32 scale,
			HdPackRecordFlags flags,
			UInt32 chrRamBankSize);

		[DllImport(DLLPath)] public static extern void HdBuilderStopRecording();

		[DllImport(DLLPath, EntryPoint = "HdBuilderGetBankPreview")] private static extern void HdBuilderGetBankPreviewWrapper(UInt32 bankNumber, UInt32 pageNumber, IntPtr rgbBuffer);
		public static byte[] HdBuilderGetBankPreview(UInt32 bankNumber, int scale, UInt32 pageNumber)
		{
			byte[] frameData = new byte[128*128*4*scale*scale];

			GCHandle hFrameData = GCHandle.Alloc(frameData, GCHandleType.Pinned);
			try {
				InteropEmu.HdBuilderGetBankPreviewWrapper(bankNumber, pageNumber, hFrameData.AddrOfPinnedObject());
			} finally {
				hFrameData.Free();
			}

			return frameData;
		}

		[DllImport(DLLPath, EntryPoint = "HdBuilderGetChrBankList")] private static extern void HdBuilderGetChrBankListWrapper(IntPtr bankList);
		public static UInt32[] HdBuilderGetChrBankList()
		{
			UInt32[] bankList = new UInt32[1024];
			GCHandle hBankList = GCHandle.Alloc(bankList, GCHandleType.Pinned);
			try {
				InteropEmu.HdBuilderGetChrBankListWrapper(hBankList.AddrOfPinnedObject());
				for(int i = 0; i < bankList.Length; i++) {
					if(bankList[i] == UInt32.MaxValue) {
						Array.Resize(ref bankList, i);
						break;
					}
				}
			} finally {
				hBankList.Free();
			}

			return bankList;
		}

		public static List<UInt32> GetPressedKeys()
		{
			UInt32[] keyBuffer = new UInt32[3];
			GCHandle handle = GCHandle.Alloc(keyBuffer, GCHandleType.Pinned);
			try {
				InteropEmu.GetPressedKeysWrapper(handle.AddrOfPinnedObject());
			} finally {
				handle.Free();
			}

			List<UInt32> keys = new List<UInt32>();
			for(int i = 0; i < 3; i++) {
				if(keyBuffer[i] != 0) {
					keys.Add(keyBuffer[i]);
				}
			}
			return keys;
		}

		public static NsfHeader NsfGetHeader()
		{
			NsfHeader header = new NsfHeader();
			NsfGetHeaderWrapper(out header);
			return header;
		}

		public static RomInfo GetRomInfo(string filename = "")
		{
			InteropRomInfo romInfo = new InteropRomInfo();
			romInfo.Sha1 = new byte[40];
			InteropEmu.GetRomInfoWrapper(ref romInfo, filename);
			return new RomInfo(romInfo);
		}

		public static ScreenSize GetScreenSize(bool ignoreScale, ConsoleId consoleId = ConsoleId.Master)
		{
			ScreenSize size;
			GetScreenSizeWrapper(consoleId, out size, ignoreScale);
			return size;
		}

		public static UInt32[] HistoryViewerGetSegments()
		{
			UInt32[] segmentBuffer = new UInt32[InteropEmu.HistoryViewerGetHistoryLength() / 30];
			UInt32 bufferSize = (UInt32)segmentBuffer.Length;

			GCHandle hSegmentBuffer = GCHandle.Alloc(segmentBuffer, GCHandleType.Pinned);
			try {
				InteropEmu.HistoryViewerGetSegmentsWrapper(hSegmentBuffer.AddrOfPinnedObject(), ref bufferSize);
			} finally {
				hSegmentBuffer.Free();
			}
			Array.Resize(ref segmentBuffer, (int)bufferSize);

			return segmentBuffer;
		}

		public static void SetFlag(EmulationFlags flag, bool value)
		{
			if(value) {
				InteropEmu.SetFlags(flag);
			} else {
				InteropEmu.ClearFlags(flag);
			}
		}

		public static string GetMesenVersion()
		{
			UInt32 version = GetMesenVersionWrapper();
			UInt32 revision = version & 0xFF;
			UInt32 minor = (version >> 8) & 0xFF;
			UInt32 major = (version >> 16) & 0xFFFF;
			return string.Format("{0}.{1}.{2}", major.ToString(), minor.ToString(), revision.ToString());
		}

		public static Int32[] GetRgbPalette()
		{
			Int32[] paleteData = new Int32[64];

			GCHandle hPaletteData = GCHandle.Alloc(paleteData, GCHandleType.Pinned);
			try {
				InteropEmu.GetRgbPaletteWrapper(hPaletteData.AddrOfPinnedObject());
			} finally {
				hPaletteData.Free();
			}

			return paleteData;
		}

		public static string GetLog() { return PtrToStringUtf8(InteropEmu.GetLogWrapper()).Replace("\n", Environment.NewLine); }
		public static string GetKeyName(UInt32 key) { return PtrToStringUtf8(InteropEmu.GetKeyNameWrapper(key)); }
		public static List<string> GetAudioDevices()
		{
			return new List<string>(PtrToStringUtf8(InteropEmu.GetAudioDevicesWrapper()).Split(new string[1] { "||" }, StringSplitOptions.RemoveEmptyEntries));
		}

		public class ArchiveRomEntry
		{
			public string Filename;
			public bool IsUtf8;

			public override string ToString()
			{
				return Filename;
			}
		}

		public static List<ArchiveRomEntry> GetArchiveRomList(string filename)
		{
			//Split the array on the [!|!] delimiter
			byte[] buffer = PtrToByteArray(InteropEmu.GetArchiveRomListWrapper(filename));
			List<List<byte>> filenames = new List<List<byte>>();
			List<byte> filenameBytes = new List<byte>();
			for(int i = 0; i < buffer.Length - 5; i++) {
				if(buffer[i] == '[' && buffer[i+1] == '!' && buffer[i+2] == '|' && buffer[i+3] == '!' && buffer[i+4] == ']') {
					filenames.Add(filenameBytes);
					filenameBytes = new List<byte>();
					i+=4;
				} else {
					filenameBytes.Add(buffer[i]);
				}
			}
			filenames.Add(filenameBytes);

			List<ArchiveRomEntry> entries = new List<ArchiveRomEntry>();

			//Check whether or not each string is a valid utf8 filename, if not decode it using the system's default encoding.
			//This is necessary because zip files do not have any rules when it comes to encoding filenames
			for(int i = 0; i < filenames.Count; i++) {
				byte[] originalBytes = filenames[i].ToArray();
				string utf8Filename = Encoding.UTF8.GetString(originalBytes);
				byte[] convertedBytes = Encoding.UTF8.GetBytes(utf8Filename);
				bool equal = true;
				if(originalBytes.Length == convertedBytes.Length) {
					for(int j = 0; j < convertedBytes.Length; j++) {
						if(convertedBytes[j] != originalBytes[j]) {
							equal = false;
							break;
						}
					}
				} else {
					equal = false;
				}

				if(!equal) {
					//String doesn't appear to be an utf8 string, use the system's default encoding
					entries.Add(new ArchiveRomEntry() { Filename = Encoding.Default.GetString(originalBytes), IsUtf8 = false });
				} else {
					entries.Add(new ArchiveRomEntry() { Filename = utf8Filename, IsUtf8 = true });
				}
			}

			return entries;
		}

		private static byte[] _codeByteArray = new byte[0];
		private static string PtrToStringUtf8(IntPtr ptr, UInt32 length = 0)
		{
			if(ptr == IntPtr.Zero) {
				return "";
			}

			int len = 0;
			if(length == 0) {
				while(System.Runtime.InteropServices.Marshal.ReadByte(ptr, len) != 0) {
					len++;
				}
			} else {
				len = (int)length;
			}

			if(len == 0) {
				return "";
			}

			if(length == 0) {
				byte[] array = new byte[len];
				System.Runtime.InteropServices.Marshal.Copy(ptr, array, 0, len);
				return System.Text.Encoding.UTF8.GetString(array);
			} else {
				//For the code window, reuse the same buffer to reduce allocations
				if(_codeByteArray.Length < len) {
					Array.Resize(ref _codeByteArray, len);
				}
				System.Runtime.InteropServices.Marshal.Copy(ptr, _codeByteArray, 0, len);
				return System.Text.Encoding.UTF8.GetString(_codeByteArray, 0, len);
			}
		}

		private static byte[] PtrToByteArray(IntPtr ptr)
		{
			if(ptr == IntPtr.Zero) {
				return new byte[0];
			}

			int len = 0;
			while(System.Runtime.InteropServices.Marshal.ReadByte(ptr, len) != 0) {
				len++;
			}

			byte[] array = new byte[len];
			System.Runtime.InteropServices.Marshal.Copy(ptr, array, 0, len);

			return array;
		}

		public enum ConsoleNotificationType
		{
			GameLoaded = 0,
			StateLoaded = 1,
			GameReset = 2,
			GamePaused = 3,
			GameResumed = 4,
			GameStopped = 5,
			CodeBreak = 6,
			CheatAdded = 7,
			CheatRemoved = 8,
			PpuFrameDone = 9,
			MovieEnded = 10,
			ResolutionChanged = 11,
			FdsBiosNotFound = 12,
			ConfigChanged = 13,
			DisconnectedFromServer = 14,
			PpuViewerDisplayFrame = 15,
			ExecuteShortcut = 16,
			EmulationStopped = 17,
			EventViewerDisplayFrame = 18,
			BeforeEmulationStop = 19,
			VsDualSystemStarted = 20,
			VsDualSystemStopped = 21,
		}

		public enum ControllerType
		{
			None = 0,
			StandardController = 1,
			Zapper = 2,
			ArkanoidController = 3,
			SnesController = 4,
			PowerPad = 5,
			SnesMouse = 6,
			SuborMouse = 7,
		}

		public enum ExpansionPortDevice
		{
			None = 0,
			Zapper = 1,
			FourPlayerAdapter = 2,
			ArkanoidController = 3,
			OekaKidsTablet = 4,
			FamilyTrainerMat = 5,
			KonamiHyperShot = 6,
			FamilyBasicKeyboard = 7,
			PartyTap = 8,
			Pachinko = 9,
			ExcitingBoxing = 10,
			JissenMahjong = 11,
			SuborKeyboard = 12,
			BarcodeBattler = 13,
			HoriTrack = 14,
			BandaiHyperShot = 15,
			AsciiTurboFile = 16,
			BattleBox = 17,
		}

		public enum MouseDevice
		{
			Unknown = 0,
			SnesMouse,
			SuborMouse,
			ArkanoidController,
			HoriTrack
		}

		public enum VsInputType
		{
			Default = 0,
			SwapControllers = 1,
			SwapAB = 2
		}

		public enum PpuModel
		{
			Ppu2C02 = 0,
			Ppu2C03 = 1,
			Ppu2C04A = 2,
			Ppu2C04B = 3,
			Ppu2C04C = 4,
			Ppu2C04D = 5,
			Ppu2C05A = 6,
			Ppu2C05B = 7,
			Ppu2C05C = 8,
			Ppu2C05D = 9,
			Ppu2C05E = 10,
		}

		public struct KeyMappingSet
		{
			public KeyMapping Mapping1;
			public KeyMapping Mapping2;
			public KeyMapping Mapping3;
			public KeyMapping Mapping4;
			public UInt32 TurboSpeed;

			[MarshalAs(UnmanagedType.I1)]
			public bool PowerpadUseSideA;
		}

		public struct KeyMapping
		{
			public UInt32 A;
			public UInt32 B;
			public UInt32 Up;
			public UInt32 Down;
			public UInt32 Left;
			public UInt32 Right;
			public UInt32 Start;
			public UInt32 Select;
			public UInt32 TurboA;
			public UInt32 TurboB;
			public UInt32 TurboStart;
			public UInt32 TurboSelect;
			public UInt32 Microphone;
			public UInt32 LButton;
			public UInt32 RButton;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
			public UInt32[] PowerPadButtons;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 72)]
			public UInt32[] FamilyBasicKeyboardButtons;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public UInt32[] PartyTapButtons;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public UInt32[] PachinkoButtons;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public UInt32[] ExcitingBoxingButtons;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
			public UInt32[] JissenMahjongButtons;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 99)]
			public UInt32[] SuborKeyboardButtons;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public UInt32[] BandaiMicrophoneButtons;
		}

		public enum StereoFilter
		{
			None = 0,
			Delay = 1,
			Panning = 2,
		}

		public struct ScreenSize
		{
			public Int32 Width;
			public Int32 Height;
			public double Scale;
		}

		public enum InputDisplayPosition
		{
			TopLeft = 0,
			TopRight = 1,
			BottomLeft = 2,
			BottomRight = 3
		}

		public class NotificationEventArgs
		{
			public ConsoleNotificationType NotificationType;
			public IntPtr Parameter;
		}

		public enum ConsoleId
		{
			Master = 0,
			Slave = 1,
			HistoryViewer = 2
		}

		public class NotificationListener : IDisposable
		{
			public delegate void NotificationCallback(int type, IntPtr parameter);
			public delegate void NotificationEventHandler(NotificationEventArgs e);
			public event NotificationEventHandler OnNotification;

			//Need to keep a reference to this callback, or it will get garbage collected (since the only reference to it is on the native side)
			NotificationCallback _callback;
			IntPtr _notificationListener;

			public NotificationListener(ConsoleId consoleId)
			{
				_callback = (int type, IntPtr parameter) => {
					this.ProcessNotification(type, parameter);
				};
				_notificationListener = InteropEmu.RegisterNotificationCallback(consoleId, _callback);
			}

			public void Dispose()
			{
				InteropEmu.UnregisterNotificationCallback(_notificationListener);
			}

			public void ProcessNotification(int type, IntPtr parameter)
			{
				if(this.OnNotification != null) {
					this.OnNotification(new NotificationEventArgs() {
						NotificationType = (ConsoleNotificationType)type,
						Parameter = parameter
					});
				}
			}
		}
	}

	public struct CdlRatios
	{
		public float CodeRatio;
		public float DataRatio;
		public float PrgRatio;

		public float ChrRatio;
		public float ChrReadRatio;
		public float ChrDrawnRatio;
	}

	public enum CdlHighlightType
	{
		None = 0,
		HighlightUsed = 1,
		HighlightUnused = 2
	}

	public enum CdlStripFlag
	{
		StripNone = 0,
		StripUnused = 1,
		StripUsed = 2
	}

	public enum CdlPrgFlags
	{
		None = 0x00,
		Code = 0x01,
		Data = 0x02,
		IndirectCode = 0x10,
		IndirectData = 0x20,
		PcmData = 0x40,
		SubEntryPoint = 0x80
	}

	public enum DebugEventType : byte
	{
		None = 0,
		PpuRegisterWrite,
		PpuRegisterRead,
		MapperRegisterWrite,
		MapperRegisterRead,
		Nmi,
		Irq,
		SpriteZeroHit,
		Breakpoint
	}

	public struct DebugEventInfo
	{
		public UInt16 Cycle;
		public Int16 Scanline;
		public UInt16 ProgramCounter;
		public UInt16 Address;
		public Int16 BreakpointId;
		public DebugEventType Type;
		public byte Value;
		public SByte PpuLatch;
	}

	public enum StackFrameFlags : byte
	{
		None = 0,
		Nmi = 1,
		Irq = 2
	}

	public struct StackFrameInfo
	{
		public Int32 JumpSourceAbsolute;
		public Int32 JumpTargetAbsolute;
		public UInt16 JumpSource;
		public UInt16 JumpTarget;
		public StackFrameFlags Flags;
	};

	public struct DebugState
	{
		public CPUState CPU;
		public PPUDebugState PPU;
		public CartridgeState Cartridge;
		public ApuState APU;
		public NesModel Model;
		public UInt32 ClockRate;
	}

	public struct CartridgeState
	{
		public UInt32 PrgRomSize;
		public UInt32 ChrRomSize;
		public UInt32 ChrRamSize;

		public UInt32 PrgPageCount;
		public UInt32 PrgPageSize;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public UInt32[] PrgSelectedPages;

		public UInt32 ChrPageCount;
		public UInt32 ChrPageSize;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public UInt32[] ChrSelectedPages;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public UInt32[] Nametables;

		public Int32 WorkRamStart;
		public Int32 WorkRamEnd;
		public Int32 SaveRamStart;
		public Int32 SaveRamEnd;

		public MirroringType Mirroring;
	}

	public enum MirroringType
	{
		Horizontal,
		Vertical,
		ScreenAOnly,
		ScreenBOnly,
		FourScreens
	}

	public struct PPUDebugState
	{
		public PPUControlFlags ControlFlags;
		public PPUStatusFlags StatusFlags;
		public PPUState State;
		public Int32 Scanline;
		public UInt32 Cycle;
		public UInt32 FrameCount;
		public UInt32 NmiScanline;
		public UInt32 ScanlineCount;
		public UInt32 SafeOamScanline;
	}

	public struct PPUState
	{
		public Byte Control;
		public Byte Mask;
		public Byte Status;
		public UInt32 SpriteRamAddr;
		public UInt16 VideoRamAddr;
		public Byte XScroll;
		public UInt16 TmpVideoRamAddr;

		[MarshalAs(UnmanagedType.I1)]
		public bool WriteToggle;

		public UInt16 HighBitShift;
		public UInt16 LowBitShift;
	}

	public struct PPUControlFlags
	{
		public Byte VerticalWrite;
		public UInt16 SpritePatternAddr;
		public UInt16 BackgroundPatternAddr;
		public Byte LargeSprites;
		public Byte VBlank;

		public Byte Grayscale;
		public Byte BackgroundMask;
		public Byte SpriteMask;
		public Byte BackgroundEnabled;
		public Byte SpritesEnabled;
		public Byte IntensifyRed;
		public Byte IntensifyGreen;
		public Byte IntensifyBlue;

		public Byte GetMask()
		{
			byte mask = 0;
			if(Grayscale != 0) mask |= 0x01;
			if(BackgroundMask != 0) mask |= 0x02;
			if(SpriteMask != 0) mask |= 0x04;
			if(BackgroundEnabled != 0) mask |= 0x08;
			if(SpritesEnabled != 0) mask |= 0x10;
			if(IntensifyBlue != 0) mask |= 0x80;
			if(IntensifyRed != 0) mask |= 0x20;
			if(IntensifyGreen != 0) mask |= 0x40;
			return mask;
		}

		public Byte GetControl()
		{
			byte control = 0;
			if(VerticalWrite != 0) control |= 0x04;
			if(SpritePatternAddr == 0x1000) control |= 0x08;
			if(BackgroundPatternAddr != 0x1000) control |= 0x10;
			if(LargeSprites != 0) control |= 0x20;
			if(VBlank != 0) control |= 0x80;
			return control;
		}
	}

	public struct PPUStatusFlags
	{
		public Byte SpriteOverflow;
		public Byte Sprite0Hit;
		public Byte VerticalBlank;

		public Byte GetStatus()
		{
			byte status = 0;
			if(SpriteOverflow != 0) status |= 0x20;
			if(Sprite0Hit != 0) status |= 0x40;
			if(VerticalBlank != 0) status |= 0x80;
			return status;
		}
	}

	public struct CPUState
	{
		public UInt16 PC;
		public Byte SP;
		public Byte A;
		public Byte X;
		public Byte Y;
		public Byte PS;
		public IRQSource IRQFlag;
		public Int32 CycleCount;

		[MarshalAs(UnmanagedType.I1)]
		public bool NMIFlag;

		public UInt16 DebugPC;
	}

	public struct ApuLengthCounterState
	{
		[MarshalAs(UnmanagedType.I1)]
		public bool Halt;
		public Byte Counter;
		public Byte ReloadValue;
	}

	public struct ApuEnvelopeState
	{
		[MarshalAs(UnmanagedType.I1)]
		public bool StartFlag;
		[MarshalAs(UnmanagedType.I1)]
		public bool Loop;
		[MarshalAs(UnmanagedType.I1)]
		public bool ConstantVolume;
		public Byte Divider;
		public Byte Counter;
		public Byte Volume;
	}

	public struct ApuSquareState
	{
		public Byte Duty;
		public Byte DutyPosition;
		public UInt16 Period;
		public UInt16 Timer;

		[MarshalAs(UnmanagedType.I1)]
		public bool SweepEnabled;
		[MarshalAs(UnmanagedType.I1)]
		public bool SweepNegate;
		public Byte SweepPeriod;
		public Byte SweepShift;

		[MarshalAs(UnmanagedType.I1)]
		public bool Enabled;
		public Byte OutputVolume;
		public double Frequency;

		public ApuLengthCounterState LengthCounter;
		public ApuEnvelopeState Envelope;
	}

	public struct ApuTriangleState
	{
		public UInt16 Period;
		public UInt16 Timer;
		public Byte SequencePosition;

		[MarshalAs(UnmanagedType.I1)]
		public bool Enabled;
		public double Frequency;
		public Byte OutputVolume;

		public ApuLengthCounterState LengthCounter;
	}

	public struct ApuNoiseState
	{
		public UInt16 Period;
		public UInt16 Timer;
		public UInt16 ShiftRegister;
		[MarshalAs(UnmanagedType.I1)]
		public bool ModeFlag;

		[MarshalAs(UnmanagedType.I1)]
		public bool Enabled;
		public double Frequency;
		public Byte OutputVolume;

		public ApuLengthCounterState LengthCounter;
		public ApuEnvelopeState Envelope;
	}

	public struct ApuDmcState
	{
		public double SampleRate;
		public UInt16 SampleAddr;
		public UInt16 SampleLength;

		[MarshalAs(UnmanagedType.I1)]
		public bool Loop;
		[MarshalAs(UnmanagedType.I1)]
		public bool IrqEnabled;
		public UInt16 Period;
		public UInt16 Timer;
		public UInt16 BytesRemaining;

		public Byte OutputVolume;
	}

	public struct ApuFrameCounterState
	{
		[MarshalAs(UnmanagedType.I1)]
		public bool FiveStepMode;
		public Byte SequencePosition;
		[MarshalAs(UnmanagedType.I1)]
		public bool IrqEnabled;
	}

	public struct ApuState
	{
		public ApuSquareState Square1;
		public ApuSquareState Square2;
		public ApuTriangleState Triangle;
		public ApuNoiseState Noise;
		public ApuDmcState Dmc;
		public ApuFrameCounterState FrameCounter;
	}

	[Serializable]
	public struct InteropTraceLoggerOptions
	{
		[MarshalAs(UnmanagedType.I1)] public bool ShowExtraInfo;
		[MarshalAs(UnmanagedType.I1)] public bool IndentCode;
		[MarshalAs(UnmanagedType.I1)] public bool UseLabels;
		[MarshalAs(UnmanagedType.I1)] public bool UseWindowsEol;
		[MarshalAs(UnmanagedType.I1)] public bool ExtendZeroPage;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
		public byte[] Condition;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
		public byte[] Format;
	}

	public enum ProfilerDataType
	{
		FunctionExclusive = 0,
		FunctionInclusive = 1,
		Instructions = 2,
		FunctionCallCount = 3,
	}

	[Flags]
	public enum IRQSource : uint
	{
		External = 1,
		FrameCounter = 2,
		DMC = 4,
	}

	[Flags]
	public enum PSFlags
	{
		Carry = 0x01,
		Zero = 0x02,
		Interrupt = 0x04,
		Decimal = 0x08,
		Break = 0x10,
		Reserved = 0x20,
		Overflow = 0x40,
		Negative = 0x80
	}

	[Flags]
	public enum EmulationFlags : UInt64
	{
		Paused = 0x01,
		ShowFPS = 0x02,
		VerticalSync = 0x04,
		AllowInvalidInput = 0x08,
		RemoveSpriteLimit = 0x10,
		UseHdPacks = 0x20,
		HasFourScore = 0x40,

		PauseOnMovieEnd = 0x0100,
		PauseWhenInBackground = 0x0200,
		AllowBackgroundInput = 0x0400,
		ReduceSoundInBackground = 0x0800,
		MuteSoundInBackground = 0x1000,

		FdsFastForwardOnLoad = 0x2000,
		FdsAutoLoadDisk = 0x4000,
		Mmc3IrqAltBehavior = 0x8000,

		SwapDutyCycles = 0x10000,

		AutoConfigureInput = 0x40000,

		ShowLagCounter = 0x80000,

		SilenceTriangleHighFreq = 0x100000,
		ReduceDmcPopping = 0x200000,

		DisableBackground = 0x400000,
		DisableSprites = 0x800000,
		ForceBackgroundFirstColumn = 0x1000000,
		ForceSpritesFirstColumn = 0x2000000,
		DisablePpu2004Reads = 0x4000000,
		DisableNoiseModeFlag = 0x8000000,
		DisablePaletteRead = 0x10000000,
		DisableOamAddrBug = 0x20000000,
		DisablePpuReset = 0x40000000,
		EnableOamDecay = 0x80000000,

		UseNes101Hvc101Behavior = 0x100000000,
		ShowFrameCounter = 0x200000000,
		ShowGameTimer = 0x400000000,

		FdsAutoInsertDisk = 0x800000000,

		Rewind = 0x1000000000,
		Turbo = 0x2000000000,
		InBackground = 0x4000000000,
		NsfPlayerEnabled = 0x8000000000,

		DisplayMovieIcons = 0x10000000000,
		HidePauseOverlay = 0x20000000000,

		UseCustomVsPalette = 0x40000000000,

		AdaptiveSpriteLimit = 0x80000000000,

		DisableGameSelectionScreen = 0x200000000000,

		ConfirmExitResetPower = 0x400000000000,

		NsfRepeat = 0x800000000000,
		NsfShuffle = 0x1000000000000,

		IntegerFpsMode = 0x2000000000000,

		DebuggerWindowEnabled = 0x4000000000000,
		BreakOnCrash = 0x8000000000000,

		AllowMismatchingSaveStates = 0x10000000000000,

		RandomizeMapperPowerOnState = 0x20000000000000,

		UseHighResolutionTimer = 0x40000000000000,
		DisplayDebugInfo = 0x80000000000000,

		ReduceSoundInFastForward = 0x100000000000000,

		VsDualMuteMaster = 0x200000000000000,
		VsDualMuteSlave = 0x400000000000000,

		ForceMaxSpeed = 0x4000000000000000,
		ConsoleMode = 0x8000000000000000,
	}

	[Flags]
	public enum DebuggerFlags
	{
		None = 0x00,

		PpuPartialDraw = 0x01,
		PpuShowPreviousFrame = 0x02,

		ShowEffectiveAddresses = 0x04,
		DisplayOpCodesInLowerCase = 0x08,
		BreakOnBrk = 0x10,
		BreakOnUnofficialOpCode = 0x20,
		BreakOnUninitMemoryRead = 0x40,

		DisassembleVerifiedData = 0x80,
		DisassembleUnidentifiedData = 0x100,
		ShowVerifiedData = 0x200,
		ShowUnidentifiedData = 0x400,

		IgnoreRedundantWrites = 0x800,

		HidePauseIcon = 0x1000,

		BreakOnDecayedOamRead = 0x2000,
	}

	public struct InteropRomInfo
	{
		public IntPtr RomNamePointer;
		public UInt32 Crc32;
		public UInt32 PrgCrc32;
		public RomFormat Format;

		[MarshalAs(UnmanagedType.I1)]
		public bool IsChrRam;

		public UInt16 MapperId;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
		public byte[] Sha1;
	}

	public enum RomFormat
	{
		Unknown = 0,
		iNes = 1,
		Unif = 2,
		Fds = 3,
		Nsf = 4,
	}

	public class RomInfo
	{
		public ResourcePath RomFile;
		public UInt32 Crc32;
		public UInt32 PrgCrc32;
		public RomFormat Format;
		public bool IsChrRam;
		public UInt16 MapperId;
		public string Sha1;

		public RomInfo(InteropRomInfo romInfo)
		{
			this.RomFile = (ResourcePath)UTF8Marshaler.GetStringFromIntPtr(romInfo.RomNamePointer);
			this.Crc32 = romInfo.Crc32;
			this.PrgCrc32 = romInfo.PrgCrc32;
			this.Format = romInfo.Format;
			this.IsChrRam = romInfo.IsChrRam;
			this.MapperId = romInfo.MapperId;
			this.Sha1 = Encoding.UTF8.GetString(romInfo.Sha1);
		}

		public string GetRomName()
		{
			return Path.GetFileNameWithoutExtension(this.RomFile.FileName);
		}

		public string GetCrcString()
		{
			return this.Crc32.ToString("X8");
		}

		public string GetPrgCrcString()
		{
			return this.PrgCrc32.ToString("X8");
		}
	};

	public struct KeyCombination
	{
		public UInt32 Key1;
		public UInt32 Key2;
		public UInt32 Key3;

		public bool IsEmpty { get { return Key1 == 0 && Key2 == 0 && Key3 == 0; } }

		public override string ToString()
		{
			if(IsEmpty) {
				return "";
			} else {
				return GetKeyNames();
			}
		}

		public KeyCombination(List<UInt32> scanCodes = null)
		{
			if(scanCodes != null) {
				if(scanCodes.Any(code => code > 0xFFFF)) {
					//If both keyboard & gamepad codes exist, only use the gamepad codes
					//This fixes an issue with Steam where Steam can remap gamepad buttons to send keyboard keys
					//See: Settings -> Controller Settings -> General Controller Settings -> Checking the Xbox/PS4/Generic/etc controller checkboxes will cause this
					scanCodes = scanCodes.Where(code => code > 0xFFFF).ToList();
				}

				Key1 = scanCodes.Count > 0 ? scanCodes[0] : 0;
				Key2 = scanCodes.Count > 1 ? scanCodes[1] : 0;
				Key3 = scanCodes.Count > 2 ? scanCodes[2] : 0;
			} else {
				Key1 = 0;
				Key2 = 0;
				Key3 = 0;
			}
		}

		private string GetKeyNames()
		{
			List<UInt32> scanCodes = new List<uint>() { Key1, Key2, Key3 };
			List<string> keyNames = scanCodes.Select((UInt32 scanCode) => InteropEmu.GetKeyName(scanCode)).Where((keyName) => !string.IsNullOrWhiteSpace(keyName)).ToList();
			keyNames.Sort((string a, string b) => {
				if(a == b) {
					return 0;
				}

				if(a == "Ctrl") {
					return -1;
				} else if(b == "Ctrl") {
					return 1;
				}

				if(a == "Alt") {
					return -1;
				} else if(b == "Alt") {
					return 1;
				}

				if(a == "Shift") {
					return -1;
				} else if(b == "Shift") {
					return 1;
				}

				return a.CompareTo(b);
			});

			return string.Join("+", keyNames);
		}
	}

	public enum EmulatorShortcut
	{
		FastForward,
		Rewind,
		RewindTenSecs,
		RewindOneMin,

		MoveToNextStateSlot,
		MoveToPreviousStateSlot,
		SaveState,
		LoadState,

		InsertNextDisk,
		VsServiceButton,
		VsServiceButton2,
		
		ToggleCheats,
		ToggleAudio,
		ToggleFastForward,
		ToggleRewind,
		ToggleKeyboardMode,

		RunSingleFrame,

		// Everything below this is handled UI-side
		SwitchDiskSide,
		EjectDisk,

		InsertCoin1,
		InsertCoin2,
		InsertCoin3,
		InsertCoin4,

		InputBarcode,

		TakeScreenshot,

		IncreaseSpeed,
		DecreaseSpeed,
		MaxSpeed,

		Pause,
		Reset,
		PowerCycle,
		PowerOff,
		Exit,

		SetScale1x,
		SetScale2x,
		SetScale3x,
		SetScale4x,
		SetScale5x,
		SetScale6x,
		ToggleFullscreen,
		ToggleFps,
		ToggleGameTimer,
		ToggleFrameCounter,
		ToggleLagCounter,
		ToggleOsd,
		ToggleAlwaysOnTop,
		ToggleSprites,
		ToggleBackground,
		ToggleDebugInfo,

		LoadRandomGame,
		SaveStateSlot1,
		SaveStateSlot2,
		SaveStateSlot3,
		SaveStateSlot4,
		SaveStateSlot5,
		SaveStateSlot6,
		SaveStateSlot7,
		SaveStateSlot8,
		SaveStateSlot9,
		SaveStateSlot10,
		SaveStateToFile,

		LoadStateSlot1,
		LoadStateSlot2,
		LoadStateSlot3,
		LoadStateSlot4,
		LoadStateSlot5,
		LoadStateSlot6,
		LoadStateSlot7,
		LoadStateSlot8,
		LoadStateSlot9,
		LoadStateSlot10,
		LoadStateSlotAuto,
		LoadStateFromFile,

		LoadLastSession,

		OpenFile,

		
		//Deprecated shortcuts
		OpenDebugger = 0xFFFF,
		OpenAssembler,
		OpenPpuViewer,
		OpenMemoryTools,
		OpenScriptWindow,
		OpenTraceLogger,
		OpenApuViewer,
		OpenEventViewer,
	}

	public struct InteropCheatInfo
	{
		public CheatType CheatType;
		public UInt32 ProActionRockyCode;
		public UInt32 Address;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
		public byte[] GameGenieCode;
		public byte Value;
		public byte CompareValue;
		[MarshalAs(UnmanagedType.I1)]
		public bool UseCompareValue;
		[MarshalAs(UnmanagedType.I1)]
		public bool IsRelativeAddress;
	}

	public struct InteropBreakpoint
	{
		public Int32 Id;
		public DebugMemoryType MemoryType;
		public BreakpointType Type;
		public Int32 StartAddress;
		public Int32 EndAddress;

		[MarshalAs(UnmanagedType.I1)]
		public bool Enabled;

		[MarshalAs(UnmanagedType.I1)]
		public bool MarkEvent;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
		public byte[] Condition;
	}

	public struct NsfHeader
	{
		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5)]
		public Byte[] Header;

		public Byte Version;
		public Byte TotalSongs;
		public Byte StartingSong;
		public UInt16 LoadAddress;
		public UInt16 InitAddress;
		public UInt16 PlayAddress;

		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256)]
		public Byte[] SongName;

		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256)]
		public Byte[] ArtistName;

		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256)]
		public Byte[] CopyrightHolder;

		public UInt16 PlaySpeedNtsc;

		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8)]
		public Byte[] BankSetup;

		public UInt16 PlaySpeedPal;
		public Byte Flags;
		public Byte SoundChips;

		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
		public Byte[] Padding;

		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256)]
		public Byte[] RipperName;

		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20000)]
		public Byte[] TrackName;

		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256)]
		public Int32[] TrackLength;

		[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256)]
		public Int32[] TrackFade;

		private string ConvertString(Byte[] input)
		{
			string output = Encoding.UTF8.GetString(input, 0, Array.IndexOf(input, (Byte)0));
			if(output.Length == 0 || output == "<?>") {
				return ResourceHelper.GetMessage("NsfUnknownField");
			}

			if(output[0] == 0xFFFD) {
				//Patch to convert an invalid character at index 0 to a copyright sign
				//This is usually the case for NSFe files (not sure what the encoding for NSF/NSFe is meant to be.  Is it properly defined?)
				return "©" + output.Substring(1);
			}

			return output;
		}

		public bool HasSongName { get { return this.GetSongName() != ResourceHelper.GetMessage("NsfUnknownField"); } }

		public string GetSongName()
		{
			return ConvertString(this.SongName);
		}

		public string GetArtistName()
		{
			return ConvertString(this.ArtistName);
		}

		public string GetCopyrightHolder()
		{
			return ConvertString(this.CopyrightHolder);
		}

		public string GetRipperName()
		{
			return ConvertString(this.RipperName);
		}

		public string[] GetTrackNames()
		{
			return Encoding.UTF8.GetString(this.TrackName, 0, Array.IndexOf(this.TrackName, (Byte)0)).Split(new string[] { "[!|!]" }, StringSplitOptions.None);
		}
	}

	public enum RecordMovieFrom
	{
		StartWithoutSaveData,
		StartWithSaveData,
		CurrentState
	}

	public struct RecordMovieOptions
	{
		private const int AuthorMaxSize = 250;
		private const int DescriptionMaxSize = 10000;
		private const int FilenameMaxSize = 2000;

		public RecordMovieOptions(string filename, string author, string description, RecordMovieFrom recordFrom)
		{
			Author = Encoding.UTF8.GetBytes(author);
			Array.Resize(ref Author, AuthorMaxSize);
			Author[AuthorMaxSize-1] = 0;

			Description = Encoding.UTF8.GetBytes(description.Replace("\r", ""));
			Array.Resize(ref Description, DescriptionMaxSize);
			Description[DescriptionMaxSize-1] = 0;

			Filename = Encoding.UTF8.GetBytes(filename);
			Array.Resize(ref Filename, FilenameMaxSize);
			Filename[FilenameMaxSize-1] = 0;

			RecordFrom = recordFrom;
		}

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = FilenameMaxSize)]
		public byte[] Filename;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = AuthorMaxSize)]
		public byte[] Author;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = DescriptionMaxSize)]
		public byte[] Description;

		public RecordMovieFrom RecordFrom;
	}

	[Flags]
	public enum BreakpointType
	{
		Global = 0,
		Execute = 1,
		Read = 2,
		Write = 4,
		ReadVram = 8,
		WriteVram = 16
	}

	public enum EvalResultType
	{
		Numeric = 0,
		Boolean = 1,
		Invalid = 2,
		DivideBy0 = 3
	}

	public enum NesModel
	{
		Auto = 0,
		NTSC = 1,
		PAL = 2,
		Dendy = 3,
	}

	public enum ConsoleType
	{
		Nes = 0,
		Famicom = 1
	}

	public enum RamPowerOnState
	{
		AllZeros = 0,
		AllOnes = 1,
		Random = 2
	}

	public enum AudioChannel
	{
		Square1 = 0,
		Square2 = 1,
		Triangle = 2,
		Noise = 3,
		DMC = 4,
		FDS = 5,
		MMC5 = 6,
		VRC6 = 7,
		VRC7 = 8,
		Namco163 = 9,
		Sunsoft5B = 10
	}

	public enum EqualizerFilterType
	{
		None = 0,
		Butterworth = 1,
		Chebyshev1 = 2,
		Chebyshev2 = 3
	}

	public enum VideoCodec
	{
		None = 0,
		ZMBV = 1,
		CSCD = 2,
	}

	public enum ScaleFilterType
	{
		xBRZ = 0,
		HQX = 1,
		Scale2x = 2,
		_2xSai = 3,
		Super2xSai = 4,
		SuperEagle = 5,
		Prescale = 6,
	}

	public enum VideoFilterType
	{
		None = 0,
		NTSC = 1,
		BisqwitNtscQuarterRes = 2,
		BisqwitNtscHalfRes = 3,
		BisqwitNtsc = 4,
		xBRZ2x = 5,
		xBRZ3x = 6,
		xBRZ4x = 7,
		xBRZ5x = 8,
		xBRZ6x = 9,
		HQ2x = 10,
		HQ3x = 11,
		HQ4x = 12,
		Scale2x = 13,
		Scale3x = 14,
		Scale4x = 15,
		_2xSai = 16,
		Super2xSai = 17,
		SuperEagle = 18,
		Prescale2x = 19,
		Prescale3x = 20,
		Prescale4x = 21,
		Prescale6x = 22,
		Prescale8x = 23,
		Prescale10x = 24,
	}

	public enum VideoResizeFilter
	{
		NearestNeighbor = 0,
		Bilinear = 1
	}

	public enum VideoAspectRatio
	{
		NoStretching = 0,
		Auto = 1,
		NTSC = 2,
		PAL = 3,
		Standard = 4,
		Widescreen = 5,
		Custom = 6
	}

	public enum VideoRefreshRates
	{
		_50 = 50,
		_60 = 60,
		_100 = 100,
		_120 = 120,
		_200 = 200,
		_240 = 240
	}

	[Flags]
	public enum ConsoleFeatures
	{
		None = 0,
		Fds = 1,
		Nsf = 2,
		VsSystem = 4,
		BarcodeReader = 8,
		TapeRecorder = 16,
		BandaiMicrophone = 32,
		DatachBarcodeReader = 64,
	}

	public enum ScreenRotation
	{
		None = 0,
		_90Degrees = 90,
		_180Degrees = 180,
		_270Degrees = 270
	}

	public enum DebugMemoryType
	{
		CpuMemory = 0,
		PpuMemory = 1,
		PaletteMemory = 2,
		SpriteMemory = 3,
		SecondarySpriteMemory = 4,
		PrgRom = 5,
		ChrRom = 6,
		ChrRam = 7,
		WorkRam = 8,
		SaveRam = 9,
		InternalRam = 10
	}

	public enum BreakSource
	{
		Break = 0,
		Pause = 1,
		BreakAfterSuspend = 2,
	}

	public enum AddressType
	{
		InternalRam = 0,
		PrgRom = 1,
		WorkRam = 2,
		SaveRam = 3,
		Register = 4
	}

	public static class AddressTypeExtensions
	{
		public static DebugMemoryType ToMemoryType(this AddressType type)
		{
			switch(type) {
				case AddressType.InternalRam: return DebugMemoryType.InternalRam;
				case AddressType.Register: return DebugMemoryType.CpuMemory;
				case AddressType.PrgRom: return DebugMemoryType.PrgRom;
				case AddressType.WorkRam: return DebugMemoryType.WorkRam;
				case AddressType.SaveRam: return DebugMemoryType.SaveRam;
			}
			return DebugMemoryType.CpuMemory;
		}

		public static AddressType ToAddressType(this DebugMemoryType type)
		{
			switch(type) {
				case DebugMemoryType.InternalRam: return AddressType.InternalRam;
				case DebugMemoryType.CpuMemory: return AddressType.Register;
				case DebugMemoryType.PrgRom: return AddressType.PrgRom;
				case DebugMemoryType.WorkRam: return AddressType.WorkRam;
				case DebugMemoryType.SaveRam: return AddressType.SaveRam;
			}
			return AddressType.Register;
		}
	}

	public enum MemoryOperationType
	{
		//Note: Not identical to the C++ enum
		Read = 0,
		Write = 1,
		Exec = 2,
	}

	[Flags]
	public enum HdPackRecordFlags
	{
		None = 0,
		UseLargeSprites = 1,
		SortByUsageFrequency = 2,
		GroupBlankTiles = 4,
		IgnoreOverscan = 8,
	}

	[StructLayout(LayoutKind.Sequential)]
	public class AddressTypeInfo
	{
		public Int32 Address;
		public AddressType Type;
	}
	
	public class MD5Helper
	{
		public static string GetMD5Hash(string filename)
		{
			if(File.Exists(filename)) {
				var md5 = System.Security.Cryptography.MD5.Create();
				return BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(filename))).Replace("-", "");
			}
			return null;
		}
	}

	public class UTF8Marshaler : ICustomMarshaler
	{
		static UTF8Marshaler _instance;

		public IntPtr MarshalManagedToNative(object managedObj)
		{
			if(managedObj == null) {
				return IntPtr.Zero;
			}
			if(!(managedObj is string)) {
				throw new MarshalDirectiveException("UTF8Marshaler must be used on a string.");
			}

			// not null terminated
			byte[] strbuf = Encoding.UTF8.GetBytes((string)managedObj);
			IntPtr buffer = Marshal.AllocHGlobal(strbuf.Length + 1);
			Marshal.Copy(strbuf, 0, buffer, strbuf.Length);

			// write the terminating null
			Marshal.WriteByte(buffer + strbuf.Length, 0);
			return buffer;
		}

		public object MarshalNativeToManaged(IntPtr pNativeData)
		{
			return GetStringFromIntPtr(pNativeData);
		}

		public void CleanUpNativeData(IntPtr pNativeData)
		{
			Marshal.FreeHGlobal(pNativeData);
		}

		public void CleanUpManagedData(object managedObj)
		{
		}

		public int GetNativeDataSize()
		{
			return -1;
		}

		public static ICustomMarshaler GetInstance(string cookie)
		{
			if(_instance == null) {
				return _instance = new UTF8Marshaler();
			}
			return _instance;
		}

		public static string GetStringFromIntPtr(IntPtr pNativeData)
		{
			int offset = 0;
			byte b = 0;
			do {
				b = Marshal.ReadByte(pNativeData, offset);
				offset++;
			} while(b != 0);

			int length = offset - 1;

			// should not be null terminated
			byte[] strbuf = new byte[length];
			// skip the trailing null
			Marshal.Copy((IntPtr)pNativeData, strbuf, 0, length);
			string data = Encoding.UTF8.GetString(strbuf);
			return data;
		}
	}
}
