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
		public bool EnableAudio = true;
		public UInt32 AudioLatency = 150;
		public UInt32 MasterVolume = 50;
		public UInt32 Square1Volume = 50;
		public UInt32 Square2Volume = 50;
		public UInt32 TriangleVolume = 50;
		public UInt32 NoiseVolume = 50;
		public UInt32 DmcVolume = 50;

		public AudioInfo()
		{
		}

		static private double ConvertVolume(UInt32 volume)
		{
			if(ConfigManager.Config.AudioInfo.EnableAudio) {
				return ((double)volume / 100d) * (double)ConfigManager.Config.AudioInfo.MasterVolume * 2 / 100d;
			} else {
				return 0;
			}
		}

		static public void ApplyConfig()
		{
			AudioInfo audioInfo = ConfigManager.Config.AudioInfo;
			InteropEmu.SetAudioLatency(audioInfo.AudioLatency);
			InteropEmu.SetChannelVolume(0, ConvertVolume(audioInfo.Square1Volume));
			InteropEmu.SetChannelVolume(1, ConvertVolume(audioInfo.Square2Volume));
			InteropEmu.SetChannelVolume(2, ConvertVolume(audioInfo.TriangleVolume));
			InteropEmu.SetChannelVolume(3, ConvertVolume(audioInfo.NoiseVolume));
			InteropEmu.SetChannelVolume(4, ConvertVolume(audioInfo.DmcVolume));
		}
	}
}
