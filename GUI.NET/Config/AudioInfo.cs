using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public class AudioInfo
	{
		public string AudioDevice = "";
		public bool EnableAudio = true;
		public UInt32 AudioLatency = 100;
		public UInt32 MasterVolume = 100;
		public UInt32 Square1Volume = 100;
		public UInt32 Square2Volume = 100;
		public UInt32 TriangleVolume = 100;
		public UInt32 NoiseVolume = 100;
		public UInt32 DmcVolume = 100;
		public UInt32 SampleRate = 44100;

		public AudioInfo()
		{
		}

		static private double ConvertVolume(UInt32 volume)
		{
			if(ConfigManager.Config.AudioInfo.EnableAudio) {
				return ((double)volume / 100d);
			} else {
				return 0;
			}
		}

		static public void ApplyConfig()
		{
			AudioInfo audioInfo = ConfigManager.Config.AudioInfo;
			InteropEmu.SetAudioDevice(audioInfo.AudioDevice);
			InteropEmu.SetAudioLatency(audioInfo.AudioLatency);
			InteropEmu.SetMasterVolume(audioInfo.MasterVolume / 10d);
			InteropEmu.SetChannelVolume(0, ConvertVolume(audioInfo.Square1Volume));
			InteropEmu.SetChannelVolume(1, ConvertVolume(audioInfo.Square2Volume));
			InteropEmu.SetChannelVolume(2, ConvertVolume(audioInfo.TriangleVolume));
			InteropEmu.SetChannelVolume(3, ConvertVolume(audioInfo.NoiseVolume));
			InteropEmu.SetChannelVolume(4, ConvertVolume(audioInfo.DmcVolume));
			InteropEmu.SetSampleRate(audioInfo.SampleRate);
		}
	}
}
