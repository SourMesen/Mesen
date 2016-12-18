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

		private static void CleanupOldFiles()
		{
			try {
				if(Directory.Exists(Path.Combine(ConfigManager.HomeFolder, "Resources"))) {
					Directory.Delete(Path.Combine(ConfigManager.HomeFolder, "Resources"), true);
				}

				if(Directory.Exists(Path.Combine(ConfigManager.HomeFolder, "WinMesen"))) {
					Directory.Delete(Path.Combine(ConfigManager.HomeFolder, "WinMesen"), true);
				}
				if(File.Exists(Path.Combine(ConfigManager.HomeFolder, "WinMesen.dll"))) {
					File.Delete(Path.Combine(ConfigManager.HomeFolder, "WinMesen.dll"));
				}				
				if(File.Exists(Path.Combine(ConfigManager.HomeFolder, "NesNtsc.dll"))) {
					File.Delete(Path.Combine(ConfigManager.HomeFolder, "NesNtsc.dll"));
				}
				if(File.Exists(Path.Combine(ConfigManager.HomeFolder, "BlipBuffer.dll"))) {
					File.Delete(Path.Combine(ConfigManager.HomeFolder, "BlipBuffer.dll"));
				}
			} catch { }
		}

		public static bool ExtractResources()
		{
			CleanupOldFiles();

			Directory.CreateDirectory(Path.Combine(ConfigManager.HomeFolder, "Resources"));
			Directory.CreateDirectory(Path.Combine(ConfigManager.HomeFolder, "GoogleDrive"));

			ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("Mesen.GUI.Dependencies.Dependencies.zip"));
						
			//Extract all needed files
			string suffix = IntPtr.Size == 4 ? ".x86" : ".x64";
			foreach(ZipArchiveEntry entry in zip.Entries) {
				if(entry.Name.Contains(suffix)) {
					string baseFolder = Program.IsMono ? Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) : ConfigManager.HomeFolder; 
					string outputFilename = Path.Combine(baseFolder, entry.Name.Replace(suffix, ""));
					ExtractFile(entry, outputFilename);
				} else if(entry.Name == "MesenUpdater.exe" || entry.Name == "MesenDB.txt") {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, entry.Name);
					ExtractFile(entry, outputFilename);
				} else if(entry.Name.StartsWith("Google.Apis") || entry.Name == "BouncyCastle.Crypto.dll" || entry.Name == "Zlib.Portable.dll" || entry.Name == "Newtonsoft.Json.dll") {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, "GoogleDrive", entry.Name);
					ExtractFile(entry, outputFilename);
				} else if(entry.Name == "Font.24.spritefont" || entry.Name == "Font.64.spritefont" || entry.Name == "LICENSE.txt") {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, "Resources", entry.Name);
					ExtractFile(entry, outputFilename);
				} else if(entry.Name == "DroidSansMono.ttf") {
					if(Program.IsMono) {
						string outputFilename = Path.Combine(ConfigManager.FontFolder, entry.Name);
						bool needRestart = !File.Exists(outputFilename);
						ExtractFile(entry, outputFilename);
						if(needRestart) {
							//If font is newly installed, restart Mesen (otherwise debugger will not be able to use the font and display incorrectly)
							System.Diagnostics.Process.Start("mono", "\"" + Assembly.GetEntryAssembly().Location + "\" /delayrestart");
							return false;
						}
					}
				}
			}

			return true;
		}
	}
}
