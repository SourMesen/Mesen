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
using System.Text.RegularExpressions;
using System.Reflection;

namespace Mesen.GUI.Config
{
	class ConfigManager
	{
		private static Configuration _config;
		private static Configuration _dirtyConfig;
		private static bool? _portableMode = null;
		private static string _portablePath = null;
		public static bool DoNotSaveSettings { get; set; }

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

		private static void ApplySetting(Type type, object instance, string name, string value)
		{
			FieldInfo[] fields = type.GetFields();
			foreach(FieldInfo info in fields) {
				if(string.Compare(info.Name, name, true) == 0) {
					try {
						if(info.FieldType == typeof(int) || info.FieldType == typeof(uint) || info.FieldType == typeof(double)) {
							MinMaxAttribute minMaxAttribute = info.GetCustomAttribute(typeof(MinMaxAttribute)) as MinMaxAttribute;
							if(minMaxAttribute != null) {
								if(info.FieldType == typeof(int)) {
									int result;
									if(int.TryParse(value, out result)) {
										if(result >= (int)minMaxAttribute.Min && result <= (int)minMaxAttribute.Max) {
											info.SetValue(instance, result);
										}
									}
								} else if(info.FieldType == typeof(uint)) {
									uint result;
									if(uint.TryParse(value, out result)) {
										if(result >= (uint)(int)minMaxAttribute.Min && result <= (uint)(int)minMaxAttribute.Max) {
											info.SetValue(instance, result);
										}
									}
								} else if(info.FieldType == typeof(double)) {
									double result;
									if(double.TryParse(value, out result)) {
										if(result >= (double)minMaxAttribute.Min && result <= (double)minMaxAttribute.Max) {
											info.SetValue(instance, result);
										}
									}
								}
							} else {
								ValidValuesAttribute validValuesAttribute = info.GetCustomAttribute(typeof(ValidValuesAttribute)) as ValidValuesAttribute;
								if(validValuesAttribute != null) {
									uint result;
									if(uint.TryParse(value, out result)) {
										if(validValuesAttribute.ValidValues.Contains(result)) {
											info.SetValue(instance, result);
										}
									}
								}
							}
						} else if(info.FieldType == typeof(bool)) {
							if(string.Compare(value, "false", true) == 0) {
								info.SetValue(instance, false);
							} else if(string.Compare(value, "true", true) == 0) {
								info.SetValue(instance, true);
							}
						} else if(info.FieldType.IsEnum) {
							int indexOf = Enum.GetNames(info.FieldType).Select((enumValue) => enumValue.ToLower()).ToList().IndexOf(value.ToLower());
							if(indexOf >= 0) {
								info.SetValue(instance, indexOf);
							}
						}
					} catch {
					}
					break;
				}
			}
		}

		public static void ProcessSwitches(List<string> switches)
		{
			Regex regex = new Regex("/([a-z0-9_A-Z.]+)=([a-z0-9_A-Z.\\-]+)");
			foreach(string param in switches) {
				Match match = regex.Match(param);
				if(match.Success) {
					string switchName = match.Groups[1].Value;
					string switchValue = match.Groups[2].Value;

					ApplySetting(typeof(VideoInfo), Config.VideoInfo, switchName, switchValue);
					ApplySetting(typeof(AudioInfo), Config.AudioInfo, switchName, switchValue);
					ApplySetting(typeof(EmulationInfo), Config.EmulationInfo, switchName, switchValue);
					ApplySetting(typeof(Configuration), Config, switchName, switchValue);
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

		public static string FontFolder
		{
			get
			{
				string fontPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts, Environment.SpecialFolderOption.Create);
				if(!Directory.Exists(fontPath)) {
					Directory.CreateDirectory(fontPath);
				}
				return fontPath;
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

		public static string RecentGamesFolder
		{
			get
			{
				string recentGamesPath = Path.Combine(ConfigManager.HomeFolder, "RecentGames");
				if(!Directory.Exists(recentGamesPath)) {
					Directory.CreateDirectory(recentGamesPath);
				}
				return recentGamesPath;
			}
		}

		public static string WaveFolder
		{
			get
			{
				string waveFolder = Path.Combine(ConfigManager.HomeFolder, "Wave");
				if(!Directory.Exists(waveFolder)) {
					Directory.CreateDirectory(waveFolder);
				}
				return waveFolder;
			}
		}

		public static string AviFolder
		{
			get
			{
				string aviFolder = Path.Combine(ConfigManager.HomeFolder, "Avi");
				if(!Directory.Exists(aviFolder)) {
					Directory.CreateDirectory(aviFolder);
				}
				return aviFolder;
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
