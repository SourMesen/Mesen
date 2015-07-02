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

namespace Mesen.GUI
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

	public class PlayerProfile
	{
		public string PlayerName = "NewPlayer";
		public byte[] PlayerAvatar;

		public PlayerProfile()
		{
			SetAvatar(Properties.Resources.MesenLogo);
		}


		public void SetAvatar(Image image)
		{
			PlayerAvatar = image.ResizeImage(64, 64).ToByteArray(ImageFormat.Bmp);
		}

		public void SetAvatar(string filename)
		{
			PlayerAvatar = File.ReadAllBytes(filename);
		}

		public Image GetAvatarImage()
		{
			return Image.FromStream(new MemoryStream(PlayerAvatar));
		}
	}

	public class ClientConnectionInfo
	{
		public string Host = "localhost";
		public UInt16 Port = 8888;
	}
	
	public class ServerInfo
	{
		public string Name = "Default";
		public UInt16 Port = 8888;
		public string Password = null;
		public int MaxPlayers = 4;
		public bool AllowSpectators = true;
		public bool PublicServer = false;
	}

	public class Configuration
	{
		private const int MaxRecentFiles = 10;

		public PlayerProfile Profile;
		public ClientConnectionInfo ClientConnectionInfo;
		public ServerInfo ServerInfo;
		public List<string> RecentFiles;

		public Configuration()
		{
			Profile = new PlayerProfile();
			ClientConnectionInfo = new ClientConnectionInfo();
			ServerInfo = new ServerInfo();
			RecentFiles = new List<string>();
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
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
			using(TextReader textReader = new StreamReader(configFile)) {
				return (Configuration)xmlSerializer.Deserialize(textReader);
			}
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

	public static class ImageExtensions
	{
		public static byte[] ToByteArray(this Image image, ImageFormat format)
		{
			using(MemoryStream ms = new MemoryStream()) {
				image.Save(ms, format);
				return ms.ToArray();
			}
		}

		public static Image ResizeImage(this Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using(var graphics = Graphics.FromImage(destImage)) {
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using(var wrapMode = new ImageAttributes()) {
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}
