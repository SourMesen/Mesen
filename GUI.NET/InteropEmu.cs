using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI
{
	public class InteropEmu
	{
		private const string DLLPath = "WinMesen.dll";
		[DllImport(DLLPath)] public static extern void InitializeEmu([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string homeFolder, IntPtr windowHandle, IntPtr dxViewerHandle);
		[DllImport(DLLPath)] public static extern void Release();

		[DllImport(DLLPath)] public static extern void LoadROM([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void AddKnowGameFolder([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string folder);

		[DllImport(DLLPath)] public static extern void AddKeyMappings(int port, KeyMapping mapping);
		[DllImport(DLLPath)] public static extern void ClearKeyMappings(int port);
		[DllImport(DLLPath)] public static extern UInt32 GetPressedKey();
		[DllImport(DLLPath)] public static extern UInt32 GetKeyCode([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string keyName);
		[DllImport(DLLPath, EntryPoint="GetKeyName")] private static extern IntPtr GetKeyNameWrapper(UInt32 key);
		
		[DllImport(DLLPath)] public static extern void Run();
		[DllImport(DLLPath)] public static extern void Pause();
		[DllImport(DLLPath)] public static extern void Resume();
		[DllImport(DLLPath)] public static extern bool IsPaused();
		[DllImport(DLLPath)] public static extern void Stop();
		[DllImport(DLLPath, EntryPoint="GetROMPath")] private static extern IntPtr GetROMPathWrapper();
		[DllImport(DLLPath)] public static extern void Reset();
		[DllImport(DLLPath)] public static extern void SetFlags(int flags);
		[DllImport(DLLPath)] public static extern void ClearFlags(int flags);
		[DllImport(DLLPath)] public static extern void StartServer(UInt16 port);
		[DllImport(DLLPath)] public static extern void StopServer();
		[DllImport(DLLPath)] public static extern bool IsServerRunning();
		[DllImport(DLLPath)] public static extern void Connect(string host, UInt16 port, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string playerName, byte[] avatarData, UInt32 avatarSize);
		[DllImport(DLLPath)] public static extern void Disconnect();
		[DllImport(DLLPath)] public static extern bool IsConnected();
		
		[DllImport(DLLPath)] public static extern void Render();
		[DllImport(DLLPath)] public static extern void TakeScreenshot();

		[DllImport(DLLPath)] public static extern IntPtr RegisterNotificationCallback(NotificationListener.NotificationCallback callback);
		[DllImport(DLLPath)] public static extern void UnregisterNotificationCallback(IntPtr notificationListener);

		[DllImport(DLLPath)] public static extern void DisplayMessage([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string title, [MarshalAs(UnmanagedType.LPWStr)]string message);

		[DllImport(DLLPath)] public static extern void MoviePlay([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename);
		[DllImport(DLLPath)] public static extern void MovieRecord([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(UTF8Marshaler))]string filename, bool reset);
		[DllImport(DLLPath)] public static extern void MovieStop();
		[DllImport(DLLPath)] public static extern bool MoviePlaying();
		[DllImport(DLLPath)] public static extern bool MovieRecording();

		[DllImport(DLLPath)] public static extern void SaveState(UInt32 stateIndex);
		[DllImport(DLLPath)] public static extern void LoadState(UInt32 stateIndex);
		[DllImport(DLLPath)] public static extern Int64 GetStateInfo(UInt32 stateIndex);

		[DllImport(DLLPath)] public static extern void CheatAddCustom(UInt32 address, Byte value, Int32 compareValue, bool isRelativeAddress);
		[DllImport(DLLPath)] public static extern void CheatAddGameGenie(string code);
		[DllImport(DLLPath)] public static extern void CheatAddProActionRocky(UInt32 code);
		[DllImport(DLLPath)] public static extern void CheatClear();

		[DllImport(DLLPath)] public static extern void DebugInitialize();
		[DllImport(DLLPath)]	public static extern void DebugRelease();
		[DllImport(DLLPath)] public static extern void DebugGetState(ref DebugState state);
		//[DllImport(DLLPath)] public static extern void DebugSetBreakpoints();
		[DllImport(DLLPath)] public static extern void DebugStep(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugStepCycles(UInt32 count);
		[DllImport(DLLPath)] public static extern void DebugStepOut();
		[DllImport(DLLPath)] public static extern void DebugStepOver();
		[DllImport(DLLPath)] public static extern void DebugRun();
		[DllImport(DLLPath)] public static extern bool DebugIsCodeChanged();
		[DllImport(DLLPath)] public static extern IntPtr DebugGetCode();
		[DllImport(DLLPath)] public static extern Byte DebugGetMemoryValue(UInt32 addr);
		[DllImport(DLLPath)] public static extern UInt32 DebugGetRelativeAddress(UInt32 addr);


		public static string GetROMPath() { return PtrToStringUtf8(InteropEmu.GetROMPathWrapper()); }
		public static string GetKeyName(UInt32 key) { return PtrToStringUtf8(InteropEmu.GetKeyNameWrapper(key)); }

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
			public UInt32 TurboSpeed;
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
		public bool NMIFlag;
	};

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

	public class MD5Helper
	{
		public static string GetMD5Hash(string filename)
		{
			var md5 = System.Security.Cryptography.MD5.Create();
			if(filename.EndsWith(".nes", StringComparison.InvariantCultureIgnoreCase)) {
				return BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(filename))).Replace("-", "");
			} else if(filename.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase)) {
				foreach(var entry in ZipFile.OpenRead(filename).Entries) {
					if(entry.Name.EndsWith(".nes", StringComparison.InvariantCultureIgnoreCase)) {
						return BitConverter.ToString(md5.ComputeHash(entry.Open())).Replace("-", "");
					}
				}
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

		public unsafe object MarshalNativeToManaged(IntPtr pNativeData)
		{
			byte* walk = (byte*)pNativeData;

			// find the end of the string
			while(*walk != 0) {
				walk++;
			}
			int length = (int)(walk - (byte*)pNativeData);

			// should not be null terminated
			byte[] strbuf = new byte[length];
			// skip the trailing null
			Marshal.Copy((IntPtr)pNativeData, strbuf, 0, length);
			string data = Encoding.UTF8.GetString(strbuf);
			return data;
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
	}
}
