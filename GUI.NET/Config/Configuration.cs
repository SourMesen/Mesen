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

		public string MesenVersion = "0.3.2";
		public PreferenceInfo PreferenceInfo;
		public AudioInfo AudioInfo;
		public VideoInfo VideoInfo;
		public InputInfo InputInfo;
		public EmulationInfo EmulationInfo;
		public List<RecentItem> RecentFiles;
		public List<VsConfigInfo> VsConfig;
		public List<CheatInfo> Cheats;
		public bool ShowOnlyCheatsForCurrentGame;
		public NesModel Region;
		public ClientConnectionInfo ClientConnectionInfo;
		public ServerInfo ServerInfo;
		public PlayerProfile Profile;
		public DebugInfo DebugInfo;

		public Configuration()
		{
			Profile = new PlayerProfile();
			ClientConnectionInfo = new ClientConnectionInfo();
			ServerInfo = new ServerInfo();
			AudioInfo = new AudioInfo();
			VideoInfo = new VideoInfo();
			PreferenceInfo = new PreferenceInfo();
			EmulationInfo = new EmulationInfo();
			RecentFiles = new List<RecentItem>();
			InputInfo = new InputInfo();
			Cheats = new List<CheatInfo>();
			VsConfig = new List<VsConfigInfo>();
			DebugInfo = new DebugInfo();
		}

		public void ApplyConfig()
		{
			InputInfo.ApplyConfig();
			VideoInfo.ApplyConfig();
			AudioInfo.ApplyConfig();
			PreferenceInfo.ApplyConfig();
			EmulationInfo.ApplyConfig();

			InteropEmu.SetNesModel(Region);
		}

		public void InitializeDefaults()
		{
			InputInfo.InitializeDefaults();
		}
		
		public void AddRecentFile(string filepath, string romName, int archiveFileIndex)
		{
			RecentItem existingItem = RecentFiles.Where((item)  => item.Path == filepath && item.ArchiveFileIndex == archiveFileIndex).FirstOrDefault();
			if(existingItem != null) {
				RecentFiles.Remove(existingItem);
			}
			RecentItem recentItem = new RecentItem { RomName = romName, Path = filepath, ArchiveFileIndex = archiveFileIndex };

			RecentFiles.Insert(0, recentItem);
			if(RecentFiles.Count > Configuration.MaxRecentFiles) {
				RecentFiles.RemoveAt(Configuration.MaxRecentFiles);
			}
			ConfigManager.ApplyChanges();
		}

		public static Configuration Deserialize(string configFile)
		{
			Configuration config;

			try {
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
				using(TextReader textReader = new StreamReader(configFile)) {
					config = (Configuration)xmlSerializer.Deserialize(textReader);
				}
			} catch {
				config = new Configuration();
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

	public class RecentItem
	{
		public string Path;
		public string RomName;
		public int ArchiveFileIndex;
	}
}
