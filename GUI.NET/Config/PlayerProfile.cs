using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public class PlayerProfile
	{
		public string PlayerName = "NewPlayer";

		[NonSerialized]
		public byte[] PlayerAvatar;

		public PlayerProfile()
		{
			//SetAvatar(Properties.Resources.MesenLogo);
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
}
