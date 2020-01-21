using Mesen.GUI.Config;
using Mesen.GUI.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI
{
	public static class BiosHelper
	{
		private static string GetFileHash(string filename, out byte[] fileData)
		{
			using(SHA256 sha256Hash = SHA256.Create()) {
				// ComputeHash - returns byte array  
				byte[] data = File.ReadAllBytes(filename);
				if(data[0] == 'N' && data[1] == 'E' && data[2] == 'S' && data[3] == '\x1a') {
					//Ignore iNES header if one exists at the start of the file
					byte[] dataWithoutHeader = new byte[data.Length - 16];
					Array.Copy(data, 16, dataWithoutHeader, 0, dataWithoutHeader.Length);
					data = dataWithoutHeader;
				}
				fileData = data;

				byte[] bytes = sha256Hash.ComputeHash(data);

				// Convert byte array to a string   
				StringBuilder builder = new StringBuilder();
				for(int i = 0; i < bytes.Length; i++) {
					builder.Append(bytes[i].ToString("X2"));
				}
				return builder.ToString();
			}
		}

		private static List<string> GetExpectedHashes(RomFormat format)
		{
			switch(format) {
				case RomFormat.Fds: return new List<string> { "99C18490ED9002D9C6D999B9D8D15BE5C051BDFA7CC7E73318053C9A994B0178", "A0A9D57CBACE21BF9C85C2B85E86656317F0768D7772ACC90C7411AB1DBFF2BF" };
				case RomFormat.StudyBox: return new List<string> { "365F84C86F7F7C3AAA2042D78494D41448E998EC5A89AC1B5FECB452951D514C" };
			}
			throw new Exception("Unexpected format type");
		}

		public static bool RequestBiosFile(string fileName, int fileSize, RomFormat format)
		{
			if(MesenMsgBox.Show("BiosNotFound", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, fileName, fileSize.ToString()) == DialogResult.OK) {
				using(OpenFileDialog ofd = new OpenFileDialog()) {
					ofd.SetFilter(ResourceHelper.GetMessage("FilterAll"));
					if(ofd.ShowDialog(Application.OpenForms[0]) == DialogResult.OK) {
						byte[] fileData;
						string hash = GetFileHash(ofd.FileName, out fileData);
						if(!GetExpectedHashes(format).Contains(hash)) {
							if(MesenMsgBox.Show("BiosMismatch", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, GetExpectedHashes(format)[0], hash) != DialogResult.OK) {
								//Files don't match and user cancelled the action
								return false;
							}
						}
						File.WriteAllBytes(Path.Combine(ConfigManager.HomeFolder, fileName), fileData);
						return true;
					}
				}
			}
			return false;
		}
	}
}
