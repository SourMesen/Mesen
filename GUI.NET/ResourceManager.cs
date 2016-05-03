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
				try {
					File.WriteAllBytes(Path.Combine(ConfigManager.HomeFolder, filename), buffer);
				} catch { }
			}
		}

		private static void ExtractFile(ZipArchiveEntry entry, string outputFilename)
		{
			if(File.Exists(outputFilename)) {
				try {
					File.Delete(outputFilename);
				} catch { }
			}
			try {
				entry.ExtractToFile(outputFilename);
			} catch { }
		}

		public static Stream GetZippedResource(string filename)
		{
			ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("Mesen.GUI.Dependencies.Dependencies.zip"));
			foreach(ZipArchiveEntry entry in zip.Entries) {
				if(entry.Name == filename) {
					return entry.Open();
				}
			}
			return null;
		}

		public static void ExtractResources()
		{
			Directory.CreateDirectory(Path.Combine(ConfigManager.HomeFolder, "Resources"));
			Directory.CreateDirectory(Path.Combine(ConfigManager.HomeFolder, "WinMesen"));

			ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("Mesen.GUI.Dependencies.Dependencies.zip"));
						
			//Extract all needed files
			string suffix = IntPtr.Size == 4 ? ".x86" : ".x64";
			foreach(ZipArchiveEntry entry in zip.Entries) {
				if(entry.Name == "MSVCx64.zip" && IntPtr.Size == 8 || entry.Name == "MSVCx86.zip" && IntPtr.Size == 4) {
					using(Stream stream = entry.Open()) {
						ZipArchive msvcZip = new ZipArchive(stream);
						foreach(ZipArchiveEntry msvcEntry in msvcZip.Entries) {
							ExtractFile(msvcEntry, Path.Combine(ConfigManager.HomeFolder, "WinMesen", msvcEntry.Name));
						}
					}
				} else if(entry.Name.Contains(suffix)) {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, "WinMesen", entry.Name.Replace(suffix, ""));
					ExtractFile(entry, outputFilename);
				} else if(entry.Name == "MesenUpdater.exe") {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, entry.Name.Replace(suffix, ""));
					ExtractFile(entry, outputFilename);
				} else if(entry.Name == "Font.24.spritefont" || entry.Name == "Font.64.spritefont" || entry.Name == "LICENSE.txt") {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, "Resources", entry.Name.Replace(suffix, ""));
					ExtractFile(entry, outputFilename);
				}
			}
		}
	}
}
