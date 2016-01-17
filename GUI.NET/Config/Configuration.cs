using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mesen.GUI.Config
{
	public class Configuration
	{
		private const int MaxRecentFiles = 10;

		public PlayerProfile Profile;
		public ClientConnectionInfo ClientConnectionInfo;
		public ServerInfo ServerInfo;
		public AudioInfo AudioInfo;
		public VideoInfo VideoInfo;
		public PreferenceInfo PreferenceInfo;
		public List<string> RecentFiles;
		public List<CheatInfo> Cheats;
		public List<ControllerInfo> Controllers;
		public bool ShowOnlyCheatsForCurrentGame;
		public bool AutoLoadIpsPatches;
		public NesModel Region;

		public Configuration()
		{
			Profile = new PlayerProfile();
			ClientConnectionInfo = new ClientConnectionInfo();
			ServerInfo = new ServerInfo();
			AudioInfo = new AudioInfo();
			VideoInfo = new VideoInfo();
			PreferenceInfo = new PreferenceInfo();
			RecentFiles = new List<string>();
			Controllers = new List<ControllerInfo>();
			Cheats = new List<CheatInfo>();
		}

		public void ApplyConfig()
		{
			ControllerInfo.ApplyConfig();
			VideoInfo.ApplyConfig();
			AudioInfo.ApplyConfig();
			PreferenceInfo.ApplyConfig();

			InteropEmu.SetNesModel(Region);
		}

		public void InitializeDefaults()
		{
			while(Controllers.Count < 4) {
				var controllerInfo = new ControllerInfo();
				controllerInfo.ControllerType = Controllers.Count < 2 ? ControllerType.StandardController : ControllerType.None;
				Controllers.Add(controllerInfo);
			}
		}
		
		public void AddRecentFile(string filepath)
		{
			if(RecentFiles.Contains(filepath)) {
				RecentFiles.Remove(filepath);
			}
			RecentFiles.Insert(0, filepath);
			if(RecentFiles.Count > Configuration.MaxRecentFiles) {
				RecentFiles.RemoveAt(Configuration.MaxRecentFiles);
			}
			ConfigManager.ApplyChanges();
		}

		public static Configuration Deserialize(string configFile)
		{
			Configuration config;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
			using(TextReader textReader = new StreamReader(configFile)) {
				config = (Configuration)xmlSerializer.Deserialize(textReader);
			}

			config.InitializeDefaults();
			return config;
		}

		public void Serialize(string configFile)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
			using(TextWriter textWriter = new StreamWriter(configFile)) {
				xmlSerializer.Serialize(textWriter, this);
			}
		}

		public Configuration Clone()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, this);

			StringReader stringReader = new StringReader(stringWriter.ToString());
			return (Configuration)xmlSerializer.Deserialize(stringReader);
		}
	}

}
