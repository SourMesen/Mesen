using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI
{
	public class InteropEmu
	{
		private const string DLLPath = "WinMesen.dll";
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool TestDll();

		[DllImport(DLLPath, EntryPoint = "GetMesenVersion")] private static extern UInt32 GetMesenVersionWrapper();

		[DllImport(DLLPath)] public static extern void InitializeEmu([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string homeFolder, IntPtr windowHandle, IntPtr dxViewerHandle);
		[DllImport(DLLPath)] public static extern void Release();

		[DllImport(DLLPath)] public static extern void SetDisplayLanguage(Language lang);

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsRunning();

		[DllImport(DLLPath)] public static extern void LoadROM([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename, Int32 archiveFileIndex = -1);
		[DllImport(DLLPath)] public static extern void ApplyIpsPatch([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void AddKnowGameFolder([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string folder);

		[DllImport(DLLPath, EntryPoint = "GetArchiveRomList")] private static extern IntPtr GetArchiveRomListWrapper([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename);
		public static List<string> GetArchiveRomList(string filename) { return new List<string>(PtrToStringUtf8(InteropEmu.GetArchiveRomListWrapper(filename)).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)); }

		[DllImport(DLLPath)] public static extern void SetMousePosition(double x, double y);
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool HasZapper();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool HasArkanoidPaddle();

		[DllImport(DLLPath)] public static extern void SetControllerType(int port, ControllerType type);
		[DllImport(DLLPath)] public static extern void SetControllerKeys(int port, KeyMappingSet mapping);
		[DllImport(DLLPath)] public static extern void SetExpansionDevice(ExpansionPortDevice device);
		[DllImport(DLLPath)] public static extern void SetConsoleType(ConsoleType type);

		[DllImport(DLLPath)] public static extern UInt32 GetPressedKey();
		[DllImport(DLLPath)] public static extern UInt32 GetKeyCode([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string keyName);
		[DllImport(DLLPath, EntryPoint="GetKeyName")] private static extern IntPtr GetKeyNameWrapper(UInt32 key);

		
		[DllImport(DLLPath)] public static extern void Run();
		[DllImport(DLLPath)] public static extern void Pause();
		[DllImport(DLLPath)] public static extern void Resume();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsPaused();
		[DllImport(DLLPath)] public static extern void Stop();

		[DllImport(DLLPath, EntryPoint = "GetRomInfo")] private static extern UInt32 GetRomInfoWrapper(ref InteropRomInfo romInfo, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string filename = "", Int32 archiveFileIndex = -1);

		[DllImport(DLLPath)] public static extern void Reset();
		[DllImport(DLLPath)] public static extern void StartServer(UInt16 port, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string hostPlayerName);
		[DllImport(DLLPath)] public static extern void StopServer();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsServerRunning();
		[DllImport(DLLPath)] public static extern void Connect([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string host, UInt16 port, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string playerName, byte[] avatarData, UInt32 avatarSize, [MarshalAs(UnmanagedType.I1)]bool spectator);
		[DllImport(DLLPath)] public static extern void Disconnect();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsConnected();

		[DllImport(DLLPath)] public static extern Int32 NetPlayGetAvailableControllers();
		[DllImport(DLLPath)] public static extern void NetPlaySelectController(Int32 controllerPort);
		[DllImport(DLLPath)] public static extern ControllerType NetPlayGetControllerType(Int32 controllerPort);
		[DllImport(DLLPath)] public static extern Int32 NetPlayGetControllerPort();

		[DllImport(DLLPath)] public static extern void TakeScreenshot();

		[DllImport(DLLPath)] public static extern IntPtr RegisterNotificationCallback(NotificationListener.NotificationCallback callback);
		[DllImport(DLLPath)] public static extern void UnregisterNotificationCallback(IntPtr notificationListener);

		[DllImport(DLLPath)] public static extern void DisplayMessage([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string title, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string message, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]string param1 = null);
		[DllImport(DLLPath, EntryPoint= "GetLog")] private static extern IntPtr GetLogWrapper();

		[DllImport(DLLPath)] public static extern void MoviePlay([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void MovieRecord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename, [MarshalAs(UnmanagedType.I1)]bool reset);
		[DllImport(DLLPath)] public static extern void MovieStop();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool MoviePlaying();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool MovieRecording();

		[DllImport(DLLPath)] public static extern void WaveRecord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void WaveStop();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool WaveIsRecording();

		[DllImport(DLLPath)] public static extern Int32 RomTestRun([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void RomTestRecord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename, [MarshalAs(UnmanagedType.I1)]bool reset);
		[DllImport(DLLPath)] public static extern void RomTestRecordFromMovie([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string testFilename, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string movieFilename);
		[DllImport(DLLPath)] public static extern void RomTestRecordFromTest([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string newTestFilename, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string existingTestFilename);
		[DllImport(DLLPath)] public static extern void RomTestStop();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool RomTestRecording();

		[DllImport(DLLPath)] public static extern void SaveState(UInt32 stateIndex);
		[DllImport(DLLPath)] public static extern void LoadState(UInt32 stateIndex);
		[DllImport(DLLPath)] public static extern Int64 GetStateInfo(UInt32 stateIndex);

		[DllImport(DLLPath)] public static extern UInt32 FdsGetSideCount();
		[DllImport(DLLPath)] public static extern void FdsEjectDisk();
		[DllImport(DLLPath)] public static extern void FdsInsertDisk(UInt32 diskNumber);
		[DllImport(DLLPath)] public static extern void FdsSwitchDiskSide();

		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool IsVsSystem();
		[DllImport(DLLPath)] public static extern void VsInsertCoin(UInt32 port);
		[DllImport(DLLPath)] public static extern void VsSetGameConfig(PpuModel model, byte dipSwitches);

		[DllImport(DLLPath)] public static extern void CheatAddCustom(UInt32 address, Byte value, Int32 compareValue, [MarshalAs(UnmanagedType.I1)]bool isRelativeAddress);
		[DllImport(DLLPath)] public static extern void CheatAddGameGenie([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string code);
		[DllImport(DLLPath)] public static extern void CheatAddProActionRocky(UInt32 code);
		[DllImport(DLLPath)] public static extern void CheatClear();

		[DllImport(DLLPath)] private static extern void SetFlags(EmulationFlags flags);
		[DllImport(DLLPath)] private static extern void ClearFlags(EmulationFlags flags);
		[DllImport(DLLPath)] public static extern void SetMasterVolume(double volume);
		[DllImport(DLLPath)] public static extern void SetChannelVolume(AudioChannel channel, double volume);
		[DllImport(DLLPath)] public static extern void SetSampleRate(UInt32 sampleRate);
		[DllImport(DLLPath)] public static extern void SetAudioLatency(UInt32 msLatency);
		[DllImport(DLLPath)] public static extern void SetStereoFilter(StereoFilter stereoFilter);
		[DllImport(DLLPath)] public static extern void SetStereoDelay(Int32 delay);
		[DllImport(DLLPath)] public static extern void SetStereoPanningAngle(double angle);
		[DllImport(DLLPath)] public static extern void SetReverbParameters(double strength, double delay);
		[DllImport(DLLPath)] public static extern void SetNesModel(NesModel model);
		[DllImport(DLLPath)] public static extern void SetEmulationSpeed(UInt32 emulationSpeed);
		[DllImport(DLLPath)] public static extern void SetOverclockRate(UInt32 overclockRate, [MarshalAs(UnmanagedType.I1)]bool adjustApu);
		[DllImport(DLLPath)] public static extern void SetPpuNmiConfig(UInt32 extraScanlinesBeforeNmi, UInt32 extraScanlineAfterNmi);
		[DllImport(DLLPath)] public static extern void SetOverscanDimensions(UInt32 left, UInt32 right, UInt32 top, UInt32 bottom);
		[DllImport(DLLPath)] public static extern void SetVideoScale(double scale);
		[DllImport(DLLPath)] public static extern void SetVideoAspectRatio(VideoAspectRatio aspectRatio);
		[DllImport(DLLPath)] public static extern void SetVideoFilter(VideoFilterType filter);
		[DllImport(DLLPath)] public static extern void SetVideoResizeFilter(VideoResizeFilter filter);
		[DllImport(DLLPath)] public static extern void SetRgbPalette(byte[] palette);
		[DllImport(DLLPath)] public static extern void SetPictureSettings(double brightness, double contrast, double saturation, double hue, double scanlineIntensity);
		[DllImport(DLLPath)] public static extern void SetNtscFilterSettings(double artifacts, double bleed, double fringing, double gamma, double resolution, double sharpness, [MarshalAs(UnmanagedType.I1)]bool mergeFields);

		[DllImport(DLLPath, EntryPoint="GetRgbPalette")] private static extern void GetRgbPaletteWrapper(IntPtr paletteBuffer);
		
		[DllImport(DLLPath, EntryPoint="GetScreenSize")] private static extern void GetScreenSizeWrapper(out ScreenSize size, [MarshalAs(UnmanagedType.I1)]bool ignoreScale);

		[DllImport(DLLPath, EntryPoint= "GetAudioDevices")] private static extern IntPtr GetAudioDevicesWrapper();
		[DllImport(DLLPath)] public static extern void SetAudioDevice(string audioDevice);

		[DllImport(DLLPath)] public static extern void DebugInitialize();
		[DllImport(DLLPath)] public static extern void DebugRelease();
		[DllImport(DLLPath)] public static extern void DebugSetFlags(DebuggerFlags flags);
		[DllImport(DLLPath)] public static extern void DebugGetState(ref DebugState state);
		[DllImport(DLLPath)] public static extern void DebugSetBreakpoints([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)]InteropBreakpoint[] breakpoints, UInt32 length);
		[DllImport(DLLPath)] public static extern void DebugStep(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugPpuStep(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugStepCycles(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugStepOut();
		[DllImport(DLLPath)] public static extern void DebugStepOver();
		[DllImport(DLLPath)] public static extern void DebugRun();
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugIsCodeChanged();
		[DllImport(DLLPath)] public static extern IntPtr DebugGetCode();
		[DllImport(DLLPath)] public static extern Byte DebugGetMemoryValue(UInt32 addr);
		[DllImport(DLLPath)] public static extern UInt32 DebugGetRelativeAddress(UInt32 addr);
		[DllImport(DLLPath)] public static extern void DebugSetNextStatement(UInt16 addr);
		[DllImport(DLLPath)] public static extern Int32 DebugEvaluateExpression([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string expression, out EvalResultType resultType);
		
		[DllImport(DLLPath)] public static extern void DebugStartTraceLogger(TraceLoggingOptions options);
		[DllImport(DLLPath)] public static extern void DebugStopTraceLogger();
		
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugLoadCdlFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string cdlFilepath);
		[DllImport(DLLPath)] [return: MarshalAs(UnmanagedType.I1)] public static extern bool DebugSaveCdlFile([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string cdlFilepath);
		[DllImport(DLLPath)] public static extern void DebugGetCdlRatios(ref CdlRatios ratios);
		[DllImport(DLLPath)] public static extern void DebugResetCdlLog();

		[DllImport(DLLPath, EntryPoint="DebugGetMemoryState")] private static extern UInt32 DebugGetMemoryStateWrapper(DebugMemoryType type, IntPtr buffer);
		public static byte[] DebugGetMemoryState(DebugMemoryType type)
		{
			byte[] buffer = new byte[10485760]; //10mb buffer
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try {
				UInt32 memorySize = InteropEmu.DebugGetMemoryStateWrapper(type, handle.AddrOfPinnedObject());
				Array.Resize(ref buffer, (int)memorySize);
			} finally {
				handle.Free();
			}
			return buffer;
		}

		[DllImport(DLLPath, EntryPoint="DebugGetNametable")] private static extern void DebugGetNametableWrapper(UInt32 nametableIndex, IntPtr frameBuffer, IntPtr tileData, IntPtr attributeData);
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

		[DllImport(DLLPath, EntryPoint="DebugGetChrBank")] private static extern void DebugGetChrBankWrapper(UInt32 bankIndex, IntPtr frameBuffer, Byte palette);
		public static byte[] DebugGetChrBank(int bankIndex, int palette)
		{
			byte[] frameData = new byte[128*128*4];

			GCHandle hFrameData = GCHandle.Alloc(frameData, GCHandleType.Pinned);
			try {
				InteropEmu.DebugGetChrBankWrapper((UInt32)bankIndex, hFrameData.AddrOfPinnedObject(), (Byte)palette);
			} finally {
				hFrameData.Free();
			}

			return frameData;
		}

		[DllImport(DLLPath, EntryPoint="DebugGetSprites")] private static extern void DebugGetSpritesWrapper(IntPtr frameBuffer);
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

		[DllImport(DLLPath, EntryPoint="DebugGetPalette")] private static extern void DebugGetPaletteWrapper(IntPtr frameBuffer);
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

		[DllImport(DLLPath, EntryPoint="DebugGetCallstack")] private static extern void DebugGetCallstackWrapper(IntPtr callstackAbsolute, IntPtr callstackRelative);
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

		public static RomInfo GetRomInfo(string filename = "", Int32 archiveFileIndex = -1)
		{
			InteropRomInfo romInfo = new InteropRomInfo();
			InteropEmu.GetRomInfoWrapper(ref romInfo, filename, archiveFileIndex);
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
			return new List<string>(PtrToStringUtf8(InteropEmu.GetAudioDevicesWrapper()).Split(new string[1] { "||" }, StringSplitOptions.RemoveEmptyEntries ));
		}

		private static string PtrToStringUtf8(IntPtr ptr)
		{
			if(ptr == IntPtr.Zero) {
				return "";
			}
			
			int len = 0;
			while(System.Runtime.InteropServices.Marshal.ReadByte(ptr, len) != 0) {
				len++;
			}

			if(len == 0) {
				return "";
			}

			byte[] array = new byte[len];
			System.Runtime.InteropServices.Marshal.Copy(ptr, array, 0, len);
			return System.Text.Encoding.UTF8.GetString(array);
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
			DisconnectedFromServer = 15
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

	public struct DebugState
	{
		public CPUState CPU;
		public PPUDebugState PPU;
	}

	public struct PPUDebugState
	{
		public PPUControlFlags ControlFlags;
		public PPUStatusFlags StatusFlags;
		public PPUState State;
		public Int32 Scanline;
		public UInt32 Cycle;
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
	}

	public struct PPUStatusFlags
	{
		public Byte SpriteOverflow;
		public Byte Sprite0Hit;
		public Byte VerticalBlank;
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

	public struct TraceLoggingOptions
	{


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
	public enum EmulationFlags
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

		InBackground = 0x40000000,
	}

	[Flags]
	public enum DebuggerFlags
	{
		None = 0,
		PpuPartialDraw = 1
	}

	public struct InteropRomInfo
	{
		public IntPtr RomNamePointer;
		public UInt32 Crc32;
	}

	public class RomInfo
	{
		public string RomName;
		public UInt32 Crc32;

		public RomInfo(InteropRomInfo romInfo)
		{
			this.RomName = UTF8Marshaler.GetStringFromIntPtr(romInfo.RomNamePointer);
			this.Crc32 = romInfo.Crc32;
		}

		public string GetRomName()
		{
			return Path.GetFileNameWithoutExtension(this.RomName);
		}

		public string GetCrcString()
		{
			return this.Crc32.ToString("X8");
		}
	};

	public struct InteropBreakpoint
	{
		public BreakpointType Type;
		public Int32 Address;
		
		[MarshalAs(UnmanagedType.I1)]
		public bool IsAbsoluteAddress;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
		public byte[] Condition;
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
		Famicom = 1,
		//VsSystem = 2,
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

	public enum VideoFilterType
	{
		None = 0,
		NTSC = 1,
		xBRZ2x = 2,
		xBRZ3x = 3,
		xBRZ4x = 4,
		xBRZ5x = 5,
		xBRZ6x = 6,
		HQ2x = 7,
		HQ3x = 8,
		HQ4x = 9,
		Scale2x = 10,
		Scale3x = 11,
		Scale4x = 12,
		_2xSai = 13,
		Super2xSai = 14,
		SuperEagle = 15
	}

	public enum VideoResizeFilter
	{
		NearestNeighbor = 0,
		Bilinear = 1
	}

	public enum VideoAspectRatio
	{
		Auto = 0,
		NTSC = 1,
		PAL = 2,
		Standard = 3,
		Widescreen = 4
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
