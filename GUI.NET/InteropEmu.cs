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

		[DllImport(DLLPath, EntryPoint = "GetMesenVersion")] private static extern UInt32 GetMesenVersionWrapper();

		[DllImport(DLLPath)] public static extern void InitializeEmu([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string homeFolder, IntPtr windowHandle, IntPtr dxViewerHandle, [MarshalAs(UnmanagedType.I1)]bool noAudio, [MarshalAs(UnmanagedType.I1)]bool noVideo, [MarshalAs(UnmanagedType.I1)]bool noInput);
		[DllImport(DLLPath)] public static extern void Release();

		[DllImport(DLLPath)] public static extern void SetDisplayLanguage(Language lang);

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsRunning();

		[DllImport(DLLPath)] public static extern void LoadROM([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string patchFile);
		[DllImport(DLLPath)] public static extern void AddKnownGameFolder([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string folder);
		[DllImport(DLLPath)] public static extern void SetFolderOverrides([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string saveDataFolder, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string saveStateFolder, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string screenshotFolder);
		[DllImport(DLLPath)] public static extern void LoadRecentGame([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filepath, [MarshalAs(UnmanagedType.I1)]bool resetGame);

		[DllImport(DLLPath, EntryPoint = "GetArchiveRomList")] private static extern IntPtr GetArchiveRomListWrapper([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename);
		public static List<string> GetArchiveRomList(string filename) { return new List<string>(PtrToStringUtf8(InteropEmu.GetArchiveRomListWrapper(filename)).Split(new string[] { "[!|!]" }, StringSplitOptions.RemoveEmptyEntries)); }

		[DllImport(DLLPath)] public static extern void SetMousePosition(double x, double y);
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool HasZapper();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool HasArkanoidPaddle();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool HasFourScore();

		[DllImport(DLLPath)] public static extern void SetControllerType(int port, ControllerType type);
		[DllImport(DLLPath)] public static extern void SetControllerKeys(int port, KeyMappingSet mapping);
		[DllImport(DLLPath)] public static extern void SetZapperDetectionRadius(UInt32 detectionRadius);
		[DllImport(DLLPath)] public static extern void SetExpansionDevice(ExpansionPortDevice device);
		[DllImport(DLLPath)] public static extern void SetConsoleType(ConsoleType type);
		[DllImport(DLLPath)] public static extern void SetEmulatorKeys(EmulatorKeyMappingSet mappings);

		[DllImport(DLLPath)] public static extern ControllerType GetControllerType(int port);
		[DllImport(DLLPath)] public static extern ExpansionPortDevice GetExpansionDevice();
		[DllImport(DLLPath)] public static extern ConsoleType GetConsoleType();

		[DllImport(DLLPath)] public static extern void UpdateInputDevices();
		[DllImport(DLLPath)] public static extern UInt32 GetPressedKey();
		[DllImport(DLLPath)] public static extern UInt32 GetKeyCode([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string keyName);
		[DllImport(DLLPath, EntryPoint = "GetKeyName")] private static extern IntPtr GetKeyNameWrapper(UInt32 key);
		[DllImport(DLLPath)] public static extern void SetKeyState(Int32 scanCode, [MarshalAs(UnmanagedType.I1)]bool pressed);
		[DllImport(DLLPath)] public static extern void ResetKeyState();

		[DllImport(DLLPath)] public static extern void Run();
		[DllImport(DLLPath)] public static extern void Pause();
		[DllImport(DLLPath)] public static extern void Resume();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsPaused();
		[DllImport(DLLPath)] public static extern void Stop();

		[DllImport(DLLPath, EntryPoint = "GetRomInfo")] private static extern UInt32 GetRomInfoWrapper(ref InteropRomInfo romInfo, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename = "");

		[DllImport(DLLPath)] public static extern void PowerCycle();
		[DllImport(DLLPath)] public static extern void Reset();
		[DllImport(DLLPath)] public static extern void ResetLagCounter();

		[DllImport(DLLPath)] public static extern void StartServer(UInt16 port, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string hostPlayerName);
		[DllImport(DLLPath)] public static extern void StopServer();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsServerRunning();
		[DllImport(DLLPath)] public static extern void Connect([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string host, UInt16 port, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string playerName, [MarshalAs(UnmanagedType.I1)]bool spectator);
		[DllImport(DLLPath)] public static extern void Disconnect();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsConnected();

		[DllImport(DLLPath)] public static extern Int32 NetPlayGetAvailableControllers();
		[DllImport(DLLPath)] public static extern void NetPlaySelectController(Int32 controllerPort);
		[DllImport(DLLPath)] public static extern ControllerType NetPlayGetControllerType(Int32 controllerPort);
		[DllImport(DLLPath)] public static extern Int32 NetPlayGetControllerPort();

		[DllImport(DLLPath)] public static extern void TakeScreenshot();

		[DllImport(DLLPath)] public static extern IntPtr RegisterNotificationCallback(NotificationListener.NotificationCallback callback);
		[DllImport(DLLPath)] public static extern void UnregisterNotificationCallback(IntPtr notificationListener);

		[DllImport(DLLPath)] public static extern void DisplayMessage([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string title, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string message, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string param1 = null);
		[DllImport(DLLPath, EntryPoint = "GetLog")] private static extern IntPtr GetLogWrapper();

		[DllImport(DLLPath)] public static extern void MoviePlay([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void MovieRecord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename, [MarshalAs(UnmanagedType.I1)]bool reset);
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
		[DllImport(DLLPath)] public static extern void VsInsertCoin(UInt32 port);
		[DllImport(DLLPath)] public static extern void VsSetGameConfig(PpuModel model, VsInputType inputType, byte dipSwitches);

		[DllImport(DLLPath)] public static extern void SetCheats([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]InteropCheatInfo[] cheats, UInt32 length);

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool CheckFlag(EmulationFlags flag);
		[DllImport(DLLPath)] private static extern void SetFlags(EmulationFlags flags);
		[DllImport(DLLPath)] private static extern void ClearFlags(EmulationFlags flags);
		[DllImport(DLLPath)] public static extern void SetRamPowerOnState(RamPowerOnState state);
		[DllImport(DLLPath)] public static extern void SetMasterVolume(double volume);
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
		[DllImport(DLLPath)] public static extern void SetVideoScale(double scale);
		[DllImport(DLLPath)] public static extern void SetVideoAspectRatio(VideoAspectRatio aspectRatio, double customRatio);
		[DllImport(DLLPath)] public static extern void SetVideoFilter(VideoFilterType filter);
		[DllImport(DLLPath)] public static extern void SetVideoResizeFilter(VideoResizeFilter filter);
		[DllImport(DLLPath)] public static extern void SetRgbPalette(byte[] palette);
		[DllImport(DLLPath)] public static extern void SetPictureSettings(double brightness, double contrast, double saturation, double hue, double scanlineIntensity);
		[DllImport(DLLPath)] public static extern void SetNtscFilterSettings(double artifacts, double bleed, double fringing, double gamma, double resolution, double sharpness, [MarshalAs(UnmanagedType.I1)]bool mergeFields, double yFilterLength, double iFilterLength, double qFilterLength);
		[DllImport(DLLPath)] public static extern void SetInputDisplaySettings(byte visiblePorts, InputDisplayPosition displayPosition, [MarshalAs(UnmanagedType.I1)]bool displayHorizontally);
		[DllImport(DLLPath)] public static extern void SetAutoSaveOptions(UInt32 delayInMinutes, [MarshalAs(UnmanagedType.I1)]bool showMessage);

		[DllImport(DLLPath, EntryPoint = "GetRgbPalette")] private static extern void GetRgbPaletteWrapper(IntPtr paletteBuffer);

		[DllImport(DLLPath, EntryPoint = "GetScreenSize")] private static extern void GetScreenSizeWrapper(out ScreenSize size, [MarshalAs(UnmanagedType.I1)]bool ignoreScale);

		[DllImport(DLLPath, EntryPoint = "GetAudioDevices")] private static extern IntPtr GetAudioDevicesWrapper();
		[DllImport(DLLPath)] public static extern void SetAudioDevice(string audioDevice);

		[DllImport(DLLPath)] public static extern void DebugInitialize();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugIsDebuggerRunning();
		[DllImport(DLLPath)] public static extern void DebugRelease();
		[DllImport(DLLPath)] public static extern void DebugSetFlags(DebuggerFlags flags);
		[DllImport(DLLPath)] public static extern void DebugGetState(ref DebugState state);
		[DllImport(DLLPath)] public static extern void DebugSetState(DebugState state);
		[DllImport(DLLPath)] public static extern void DebugSetBreakpoints([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]InteropBreakpoint[] breakpoints, UInt32 length);
		[DllImport(DLLPath)] public static extern void DebugSetLabel(UInt32 address, AddressType addressType, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string label, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string comment);
		[DllImport(DLLPath)] public static extern void DebugStep(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugPpuStep(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugStepCycles(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugStepOut();
		[DllImport(DLLPath)] public static extern void DebugStepOver();
		[DllImport(DLLPath)] public static extern void DebugStepBack();
		[DllImport(DLLPath)] public static extern void DebugRun();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugIsExecutionStopped();
		[DllImport(DLLPath)] public static extern Int32 DebugGetRelativeAddress(UInt32 absoluteAddr, AddressType type);
		[DllImport(DLLPath)] public static extern Int32 DebugFindSubEntryPoint(UInt16 relativeAddr);
		[DllImport(DLLPath)] public static extern Int32 DebugGetAbsoluteAddress(UInt32 relativeAddr);
		[DllImport(DLLPath)] public static extern Int32 DebugGetMemorySize(DebugMemoryType type);
		[DllImport(DLLPath)] public static extern Byte DebugGetMemoryValue(DebugMemoryType type, UInt32 address);
		[DllImport(DLLPath)] public static extern void DebugSetMemoryValue(DebugMemoryType type, UInt32 address, byte value);
		[DllImport(DLLPath)] public static extern void DebugSetInputOverride(Int32 port, Int32 state);

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

		[DllImport(DLLPath)] public static extern void DebugGetAbsoluteAddressAndType(UInt32 relativeAddr, ref AddressTypeInfo addressTypeInfo);
		[DllImport(DLLPath)] public static extern void DebugSetPpuViewerScanlineCycle(Int32 scanline, Int32 cycle);

		[DllImport(DLLPath)] public static extern void DebugSetFreezeState(UInt16 address, [MarshalAs(UnmanagedType.I1)]bool frozen);

		[DllImport(DLLPath)] public static extern void DebugSetNextStatement(UInt16 addr);
		[DllImport(DLLPath)] public static extern Int32 DebugEvaluateExpression([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string expression, out EvalResultType resultType);

		[DllImport(DLLPath)] public static extern void DebugStartTraceLogger([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void DebugStopTraceLogger();
		[DllImport(DLLPath)] public static extern void DebugSetTraceOptions(TraceLoggerOptions options);
		[DllImport(DLLPath, EntryPoint = "DebugGetExecutionTrace")] private static extern IntPtr DebugGetExecutionTraceWrapper(UInt32 lineCount);
		public static string DebugGetExecutionTrace(UInt32 lineCount) { return PtrToStringUtf8(InteropEmu.DebugGetExecutionTraceWrapper(lineCount)); }

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugLoadCdlFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string cdlFilepath);
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugSaveCdlFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string cdlFilepath);
		[DllImport(DLLPath)] public static extern void DebugGetCdlRatios(ref CdlRatios ratios);
		[DllImport(DLLPath)] public static extern void DebugResetCdlLog();
		[DllImport(DLLPath)] public static extern void DebugResetMemoryAccessCounts();
		[DllImport(DLLPath)] public static extern void DebugResetProfiler();

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

		[DllImport(DLLPath, EntryPoint = "DebugSaveRomToDisk")] public static extern void DebugSaveRomToDiskWrapper([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename, IntPtr headerBuffer);
		public static void DebugSaveRomToDisk(string filename, byte[] header = null)
		{
			if(header != null) {
				GCHandle handle = GCHandle.Alloc(header, GCHandleType.Pinned);
				try {
					InteropEmu.DebugSaveRomToDiskWrapper(filename, handle.AddrOfPinnedObject());
				} finally {
					handle.Free();
				}
			} else {
				InteropEmu.DebugSaveRomToDiskWrapper(filename, IntPtr.Zero);
			}
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetCode")] private static extern IntPtr DebugGetCodeWrapper(out UInt32 length);
		public static string DebugGetCode()
		{
			UInt32 length;
			IntPtr ptrCodeString = InteropEmu.DebugGetCodeWrapper(out length);
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

		[DllImport(DLLPath, EntryPoint = "DebugGetNametable")] private static extern void DebugGetNametableWrapper(UInt32 nametableIndex, IntPtr frameBuffer, IntPtr tileData, IntPtr attributeData);
		public static void DebugGetNametable(int nametableIndex, out byte[] frameData, out byte[] tileData, out byte[] attributeData)
		{
			frameData = new byte[256*240*4];
			tileData = new byte[32*30];
			attributeData = new byte[32*30];

			GCHandle hFrameData = GCHandle.Alloc(frameData, GCHandleType.Pinned);
			GCHandle hTileData = GCHandle.Alloc(tileData, GCHandleType.Pinned);
			GCHandle hAttributeData = GCHandle.Alloc(attributeData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetNametableWrapper((UInt32)nametableIndex, hFrameData.AddrOfPinnedObject(), hTileData.AddrOfPinnedObject(), hAttributeData.AddrOfPinnedObject());
			} finally {
				hFrameData.Free();
				hTileData.Free();
				hAttributeData.Free();
			}
		}

		[DllImport(DLLPath, EntryPoint = "DebugGetChrBank")] private static extern void DebugGetChrBankWrapper(UInt32 bankIndex, IntPtr frameBuffer, Byte palette, [MarshalAs(UnmanagedType.I1)]bool largeSprites, CdlHighlightType highlightType);
		public static byte[] DebugGetChrBank(int bankIndex, int palette, bool largeSprites, CdlHighlightType highlightType)
		{
			byte[] frameData = new byte[128*128*4];

			GCHandle hFrameData = GCHandle.Alloc(frameData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetChrBankWrapper((UInt32)bankIndex, hFrameData.AddrOfPinnedObject(), (Byte)palette, largeSprites, highlightType);
			} finally {
				hFrameData.Free();
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
		public static byte[] DebugGetPalette()
		{
			byte[] frameData = new byte[4*8*4];

			GCHandle hFrameData = GCHandle.Alloc(frameData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetPaletteWrapper(hFrameData.AddrOfPinnedObject());
			} finally {
				hFrameData.Free();
			}

			return frameData;
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

		[DllImport(DLLPath, EntryPoint = "DebugGetCallstack")] private static extern void DebugGetCallstackWrapper(IntPtr callstackAbsolute, IntPtr callstackRelative);
		public static void DebugGetCallstack(out Int32[] callstackAbsolute, out Int32[] callstackRelative)
		{
			callstackAbsolute = new Int32[1024];
			callstackRelative = new Int32[1024];

			GCHandle hAbsolute = GCHandle.Alloc(callstackAbsolute, GCHandleType.Pinned);
			GCHandle hRelative = GCHandle.Alloc(callstackRelative, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetCallstackWrapper(hAbsolute.AddrOfPinnedObject(), hRelative.AddrOfPinnedObject());
			} finally {
				hAbsolute.Free();
				hRelative.Free();
			}
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

		public static NsfHeader NsfGetHeader()
		{
			NsfHeader header = new NsfHeader();
			NsfGetHeaderWrapper(out header);
			return header;
		}

		public static RomInfo GetRomInfo(string filename = "")
		{
			InteropRomInfo romInfo = new InteropRomInfo();
			InteropEmu.GetRomInfoWrapper(ref romInfo, filename);
			return new RomInfo(romInfo);
		}

		public static ScreenSize GetScreenSize(bool ignoreScale)
		{
			ScreenSize size;
			GetScreenSizeWrapper(out size, ignoreScale);
			return size;
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
			FdsDiskChanged = 12,
			FdsBiosNotFound = 13,
			ConfigChanged = 14,
			DisconnectedFromServer = 15,
			PpuViewerDisplayFrame = 16,
			RequestExit = 17,
			ToggleCheats = 18,
			ToggleAudio = 19,
			RequestReset = 20,
			RequestPowerCycle = 21,
		}

		public enum ControllerType
		{
			None = 0,
			StandardController = 1,
			Zapper = 2,
			ArkanoidController = 3,
		}

		public enum ExpansionPortDevice
		{
			None = 0,
			Zapper = 1,
			FourPlayerAdapter = 2,
			ArkanoidController = 3,
			OekaKidsTablet = 4,
		}

		public enum VsInputType
		{
			Default = 0,
			TypeA = 1,
			TypeB = 2,
			TypeC = 3,
			TypeD = 4,
			TypeE = 5
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
		}

		public class NotificationListener : IDisposable
		{
			public delegate void NotificationCallback(int type);
			public delegate void NotificationEventHandler(NotificationEventArgs e);
			public event NotificationEventHandler OnNotification;

			//Need to keep a reference to this callback, or it will get garbage collected (since the only reference to it is on the native side)
			NotificationCallback _callback;
			IntPtr _notificationListener;

			public NotificationListener()
			{
				_callback = (int type) => {
					this.ProcessNotification(type);
				};
				_notificationListener = InteropEmu.RegisterNotificationCallback(_callback);
			}

			public void Dispose()
			{
				InteropEmu.UnregisterNotificationCallback(_notificationListener);
			}

			public void ProcessNotification(int type)
			{
				if(this.OnNotification != null) {
					this.OnNotification(new NotificationEventArgs() { NotificationType = (ConsoleNotificationType)type });
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

	public struct DebugState
	{
		public CPUState CPU;
		public PPUDebugState PPU;
		public CartridgeState Cartridge;
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

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public UInt32[] Nametables;
	}

	public struct PPUDebugState
	{
		public PPUControlFlags ControlFlags;
		public PPUStatusFlags StatusFlags;
		public PPUState State;
		public Int32 Scanline;
		public UInt32 Cycle;
		public UInt32 FrameCount;
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
		public Byte WriteToggle;

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

	public enum StatusFlagFormat
	{
		Hexadecimal = 0,
		Text = 1,
		CompactText = 2
	}

	[Serializable]
	public struct TraceLoggerOptions
	{
		[MarshalAs(UnmanagedType.I1)] public bool ShowByteCode;
		[MarshalAs(UnmanagedType.I1)] public bool ShowRegisters;
		[MarshalAs(UnmanagedType.I1)] public bool ShowCpuCycles;
		[MarshalAs(UnmanagedType.I1)] public bool ShowPpuCycles;
		[MarshalAs(UnmanagedType.I1)] public bool ShowPpuScanline;
		[MarshalAs(UnmanagedType.I1)] public bool ShowPpuFrames;
		[MarshalAs(UnmanagedType.I1)] public bool ShowExtraInfo;
		[MarshalAs(UnmanagedType.I1)] public bool IndentCode;
		[MarshalAs(UnmanagedType.I1)] public bool ShowEffectiveAddresses;
		[MarshalAs(UnmanagedType.I1)] public bool UseLabels;
		public StatusFlagFormat StatusFormat;

		[NonSerialized]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
		public byte[] Condition;
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

		DisableGameDatabase = 0x20000,
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

		Turbo = 0x2000000000,
		InBackground = 0x4000000000,
		NsfPlayerEnabled = 0x8000000000,

		DisplayMovieIcons = 0x10000000000,
		HidePauseOverlay = 0x20000000000,

		UseCustomVsPalette = 0x40000000000,

		AdaptiveSpriteLimit = 0x80000000000,

		DisableOsd = 0x100000000000,
		DisableGameSelectionScreen = 0x200000000000,

		ConfirmExitResetPower = 0x400000000000,

		ForceMaxSpeed = 0x4000000000000000,
		ConsoleMode = 0x8000000000000000,
	}

	[Flags]
	public enum DebuggerFlags
	{
		None = 0x00,
		PpuPartialDraw = 0x01,
		ShowEffectiveAddresses = 0x02,
		ShowOnlyDisassembledCode = 0x04,
		DisplayOpCodesInLowerCase = 0x08,
		DisassembleEverything = 0x10,
		DisassembleEverythingButData = 0x20,
		BreakOnBrk = 0x40,
		BreakOnUnofficialOpCode = 0x80,
	}

	public struct InteropRomInfo
	{
		public IntPtr RomNamePointer;
		public UInt32 Crc32;
		public UInt32 PrgCrc32;
		public RomFormat Format;

		[MarshalAs(UnmanagedType.I1)]
		public bool IsChrRam;
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
		public string RomName;
		public UInt32 Crc32;
		public UInt32 PrgCrc32;
		public RomFormat Format;
		public bool IsChrRam;

		public RomInfo(InteropRomInfo romInfo)
		{
			this.RomName = UTF8Marshaler.GetStringFromIntPtr(romInfo.RomNamePointer);
			this.Crc32 = romInfo.Crc32;
			this.PrgCrc32 = romInfo.PrgCrc32;
			this.Format = romInfo.Format;
			this.IsChrRam = romInfo.IsChrRam;
		}

		public string GetRomName()
		{
			return Path.GetFileNameWithoutExtension(this.RomName);
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

	public struct EmulatorKeyMappingSet
	{
		public EmulatorKeyMappings KeySet1;
		public EmulatorKeyMappings KeySet2;
	}

	public struct EmulatorKeyMappings
	{
		public UInt32 FastForward;

		public UInt32 Rewind;
		public UInt32 RewindTenSecs;
		public UInt32 RewindOneMin;

		public UInt32 Pause;
		public UInt32 Reset;
		public UInt32 PowerCycle;
		public UInt32 PowerOff;
		public UInt32 Exit;

		public UInt32 MoveToNextStateSlot;
		public UInt32 MoveToPreviousStateSlot;
		public UInt32 SaveState;
		public UInt32 LoadState;

		public UInt32 SwitchDiskSide;
		public UInt32 InsertNextDisk;

		public UInt32 InsertCoin1;
		public UInt32 InsertCoin2;
		public UInt32 VsServiceButton;

		public UInt32 TakeScreenshot;
		public UInt32 IncreaseSpeed;
		public UInt32 DecreaseSpeed;

		public UInt32 ToggleCheats;
		public UInt32 ToggleAudio;

		public UInt32 RunSingleFrame;
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
		public BreakpointType Type;
		public Int32 StartAddress;
		public Int32 EndAddress;

		[MarshalAs(UnmanagedType.I1)]
		public bool IsAbsoluteAddress;

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
		Invalid = 2
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

	public enum AddressType
	{
		InternalRam = 0,
		PrgRom = 1,
		WorkRam = 2,
		SaveRam = 3,
		Register = 4
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
	}

	public struct AddressTypeInfo
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
