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
					SaveConfig();
				}
			}
		}

		private static void SaveConfig()
		{
			_config.Serialize(ConfigFile);
		}

		private static string ConfigFile
		{
			get
			{
				string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create), "Mesen");
				if(!Directory.Exists(configPath)) {
					Directory.CreateDirectory(configPath);
				}

				return Path.Combine(configPath, "settings.xml");
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
			_config = _dirtyConfig.Clone();
			SaveConfig();
		}

		public static void RejectChanges()
		{
			_dirtyConfig = _config.Clone();
		}
	}
}
