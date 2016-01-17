using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mesen.GUI.Config;
using System.IO.Compression;

namespace Mesen.GUI
{
	class ResourceManager
	{
		private static void ExtractResource(string resourceName, string filename)
		{
			if(File.Exists(filename)) {
				try {
					File.Delete(filename);
				} catch { } 
			}
			Assembly a = Assembly.GetExecutingAssembly();
			using(Stream s = a.GetManifestResourceStream(resourceName)) {
				byte[] buffer = new byte[s.Length];
				s.Read(buffer, 0, (int)s.Length);
				File.WriteAllBytes(Path.Combine(ConfigManager.HomeFolder, filename), buffer);
			}
		}

		public static void ExtractResources()
		{
			Directory.CreateDirectory(Path.Combine(ConfigManager.HomeFolder, "Resources"));

			ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("Mesen.GUI.Dependencies.Dependencies.zip"));
						
			//Extract all needed files
			string suffix = IntPtr.Size == 4 ? ".x86" : ".x64";
			foreach(ZipArchiveEntry entry in zip.Entries) {
				if(entry.Name.Contains(suffix)) {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, entry.Name.Replace(suffix, ""));

					if(File.Exists(outputFilename)) {
						try {
							File.Delete(outputFilename);
						} catch { }
					}
					entry.ExtractToFile(outputFilename);
				}
			}

			ExtractResource("Mesen.GUI.Dependencies.MesenIcon.bmp", Path.Combine("Resources", "MesenIcon.bmp"));
			ExtractResource("Mesen.GUI.Dependencies.Roboto.12.spritefont", Path.Combine("Resources", "Roboto.12.spritefont"));
			ExtractResource("Mesen.GUI.Dependencies.Roboto.9.spritefont", Path.Combine("Resources", "Roboto.9.spritefont"));
			ExtractResource("Mesen.GUI.Dependencies.Toast.dds", Path.Combine("Resources", "Toast.dds"));
		}
	}
}
