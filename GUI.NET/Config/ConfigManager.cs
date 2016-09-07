using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Mesen.GUI.Config
{
	class ConfigManager
	{
		private static Configuration _config;
		private static Configuration _dirtyConfig;
		private static bool? _portableMode = null;
		private static string _portablePath = null;

		private static void LoadConfig()
		{
			if(_config == null) {
				if(File.Exists(ConfigFile)) {
					_config = Configuration.Deserialize(ConfigFile);
					_dirtyConfig = Configuration.Deserialize(ConfigFile);
				} else {
					//Create new config file and save it to disk
					_config = new Configuration();
					_dirtyConfig = new Configuration();
					_config.Save();
				}
			}
		}

		public static void SaveConfig()
		{
			_config.Save();
		}

		public static string HomeFolder
		{
			get
			{
				if(_portableMode == null) {
					_portableMode = System.Reflection.Assembly.GetEntryAssembly().Location.EndsWith("_P.exe", StringComparison.InvariantCultureIgnoreCase);
					_portablePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
				}

				if(_portableMode.Value) {
					return Path.Combine(_portablePath, "Mesen");
				} else {
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create), "Mesen");
				}
			}
		}

		public static string MovieFolder
		{
			get
			{
				string movieFolder = Path.Combine(ConfigManager.HomeFolder, "Movies");
				if(!Directory.Exists(movieFolder)) {
					Directory.CreateDirectory(movieFolder);
				}
				return movieFolder;
			}
		}

		public static string WaveFolder
		{
			get
			{
				string waveFoler = Path.Combine(ConfigManager.HomeFolder, "Wave");
				if(!Directory.Exists(waveFoler)) {
					Directory.CreateDirectory(waveFoler);
				}
				return waveFoler;
			}
		}

		public static string SaveFolder
		{
			get
			{
				string movieFolder = Path.Combine(ConfigManager.HomeFolder, "Saves");
				if(!Directory.Exists(movieFolder)) {
					Directory.CreateDirectory(movieFolder);
				}
				return movieFolder;
			}
		}

		public static string SaveStateFolder
		{
			get
			{
				string movieFolder = Path.Combine(ConfigManager.HomeFolder, "SaveStates");
				if(!Directory.Exists(movieFolder)) {
					Directory.CreateDirectory(movieFolder);
				}
				return movieFolder;
			}
		}

		public static string DebuggerFolder
		{
			get
			{
				string debuggerFolder = Path.Combine(ConfigManager.HomeFolder, "Debugger");
				if(!Directory.Exists(debuggerFolder)) {
					Directory.CreateDirectory(debuggerFolder);
				}
				return debuggerFolder;
			}
		}

		public static string DownloadFolder
		{
			get
			{
				string downloadFolder = Path.Combine(ConfigManager.HomeFolder, "Downloads");
				if(!Directory.Exists(downloadFolder)) {
					Directory.CreateDirectory(downloadFolder);
				}
				return downloadFolder;
			}
		}

		public static string BackupFolder
		{
			get
			{
				string backupFolder = Path.Combine(ConfigManager.HomeFolder, "Backups");
				if(!Directory.Exists(backupFolder)) {
					Directory.CreateDirectory(backupFolder);
				}
				return backupFolder;
			}
		}

		public static string TestFolder
		{
			get
			{
				string testFolder = Path.Combine(ConfigManager.HomeFolder, "Tests");
				if(!Directory.Exists(testFolder)) {
					Directory.CreateDirectory(testFolder);
				}
				return testFolder;
			}
		}

		public static string ConfigFile
		{
			get
			{
				if(!Directory.Exists(HomeFolder)) {
					Directory.CreateDirectory(HomeFolder);
				}

				return Path.Combine(HomeFolder, "settings.xml");
			}
		}

		public static Configuration Config
		{
			get 
			{
				LoadConfig();
				return _dirtyConfig;
			}
		}

		public static void ApplyChanges()
		{
			_config.NeedToSave = false;
			_config = _dirtyConfig.Clone();
			_config.NeedToSave = true;
			_config.Save();
		}

		public static void RejectChanges()
		{
			_dirtyConfig = _config.Clone();
		}
	}
}
