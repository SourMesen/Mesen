using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mesen.GUI.Config;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Mesen.GUI
{
	class ResourceManager
	{
		public static string GetSha1Hash(byte[] fileData)
		{
			using(SHA1Managed sha1 = new SHA1Managed()) {
				byte[] hash = sha1.ComputeHash(fileData);

				var sb = new StringBuilder(hash.Length * 2);
				foreach(byte b in hash) {
					sb.Append(b.ToString("x2"));
				}
				return sb.ToString();
			}
		}

		private static void ExtractFile(ZipArchiveEntry entry, string outputFilename)
		{
			if(File.Exists(outputFilename)) {
				byte[] zipFileData = new byte[entry.Length];
				using(Stream fileStream = entry.Open()) {
					fileStream.Read(zipFileData, 0, (int)entry.Length);
				}

				string diskFileSha1 = GetSha1Hash(File.ReadAllBytes(outputFilename));
				string zipFileSha1 = GetSha1Hash(zipFileData);

				if(diskFileSha1 != zipFileSha1) {
					try {
						File.Delete(outputFilename);
					} catch { }
					try {
						File.WriteAllBytes(outputFilename, zipFileData);
					} catch { }
				}
			} else {
				try {
					//On Mono, using overwrite = true for ExtractToFile crashes/kills any currently running instance that uses the file.
					//This is probably a Mono bug?
					//Better to attempt a delete & then extract, like now (and like it used to be) 
					entry.ExtractToFile(outputFilename);
				} catch { }
			}
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

		public static string ReadZippedResource(string filename)
		{
			ZipArchive zip = new ZipArchive(Assembly.GetExecutingAssembly().GetManifestResourceStream("Mesen.GUI.Dependencies.Dependencies.zip"));
			foreach(ZipArchiveEntry entry in zip.Entries) {
				string entryFileName = Program.IsMono ? entry.Name.Substring(entry.Name.LastIndexOf('\\') + 1) : entry.Name;
				if(entryFileName == filename) {
					using(Stream stream = entry.Open()) {
						using(StreamReader reader = new StreamReader(stream)) {
							return reader.ReadToEnd();
						}
					}
				}
			}
			return null;
		}

		private static void CleanupOldFiles()
		{
			try {
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
				if(entry.Name.StartsWith("MesenCore") && !Program.IsMono && entry.Name.Contains(suffix)) {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, entry.Name.Replace(suffix, ""));
					ExtractFile(entry, outputFilename);					
				} else if(entry.Name.StartsWith("libMesenCore") && Program.IsMono && entry.Name.Contains(suffix)) {
					string outputFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), entry.Name.Replace(suffix, ""));
					ExtractFile(entry, outputFilename);
				} else if(entry.Name == "MesenUpdater.exe" || entry.Name == "MesenDB.txt") {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, entry.Name);
					ExtractFile(entry, outputFilename);
				} else if(entry.Name.StartsWith("Google.Apis") || entry.Name == "BouncyCastle.Crypto.dll" || entry.Name == "Zlib.Portable.dll" || entry.Name == "Newtonsoft.Json.dll") {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, "GoogleDrive", entry.Name);
					ExtractFile(entry, outputFilename);
				} else if(entry.Name == "Font.24.spritefont" || entry.Name == "Font.64.spritefont" || entry.Name == "LICENSE.txt" || entry.Name == "PixelFont.ttf") {
					string outputFilename = Path.Combine(ConfigManager.HomeFolder, "Resources", entry.Name);
					ExtractFile(entry, outputFilename);
				} else if(entry.Name == "DroidSansMono.ttf" && Program.IsMono) {
					string outputFilename = Path.Combine(ConfigManager.FontFolder, entry.Name);
					bool needRestart = !File.Exists(outputFilename);
					ExtractFile(entry, outputFilename);
					if(needRestart) {
						//If font is newly installed, restart Mesen (otherwise debugger will not be able to use the font and display incorrectly)
						ConfigManager.RestartMesen();
						return false;
					}
				}
			}

			return true;
		}
	}
}
