using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	class TblLoader
	{
		public struct TblKey
		{
			public UInt64 Key;
			public int Length;
		}

		public static Dictionary<TblKey, string> ToDictionary(string filename)
		{
			try {
				Dictionary<TblKey, string> dict = new Dictionary<TblKey, string>();

				string[] lines = File.ReadAllLines(filename);
				for(int i = 0; i < lines.Length; i++) {
					if(!string.IsNullOrWhiteSpace(lines[i])) {
						string[] data = lines[i].Split('=');
						if(data.Length == 2) {
							data[0] = data[0].Replace(" ", "");
							if(data[0].Length % 2 == 0 && Regex.IsMatch(data[0], "[0-9A-Fa-f]+")) {
								TblKey key = new TblKey();

								List<byte> bytes = new List<byte>();
								for(int j = 0; j < data[0].Length; j+=2) {
									byte result = byte.Parse(data[0].Substring(j, 2), System.Globalization.NumberStyles.HexNumber);
									key.Key |= (UInt64)result << (8 * j / 2);
									key.Length++;
								}

								dict[key] = data[1];
							}
						}
					}
				}

				return dict;
			} catch {
				return null;
			}
		}
	}
}
