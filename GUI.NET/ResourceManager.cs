using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mesen.GUI.Config;

namespace Mesen.GUI
{
	class ResourceManager
	{
		private static void ExtractResource(string resourceName, string filename)
		{
			if(!System.IO.File.Exists(filename)) {
				System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
				using(Stream s = a.GetManifestResourceStream(resourceName)) {
					byte[] buffer = new byte[s.Length];
					s.Read(buffer, 0, (int)s.Length);
					File.WriteAllBytes(Path.Combine(ConfigManager.HomeFolder, filename), buffer);
				}
			}
		}

		public static void ExtractResources()
		{
			Directory.CreateDirectory(Path.Combine(ConfigManager.HomeFolder, "Resources"));

			//Extract all needed files
			ExtractResource("Mesen.GUI.Dependencies.WinMesen.dll", "WinMesen.dll");
			ExtractResource("Mesen.GUI.Dependencies.BlipBuffer.dll", "BlipBuffer.dll");
			ExtractResource("Mesen.GUI.Dependencies.NesNtsc.dll", "NesNtsc.dll");

			ExtractResource("Mesen.GUI.Dependencies.MesenIcon.bmp", Path.Combine("Resources", "MesenIcon.bmp"));
			ExtractResource("Mesen.GUI.Dependencies.Roboto.12.spritefont", Path.Combine("Resources", "Roboto.12.spritefont"));
			ExtractResource("Mesen.GUI.Dependencies.Roboto.9.spritefont", Path.Combine("Resources", "Roboto.9.spritefont"));
			ExtractResource("Mesen.GUI.Dependencies.Toast.dds", Path.Combine("Resources", "Toast.dds"));
		}
	}
}
