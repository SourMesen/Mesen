using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	public class MesenLabelFile
	{
		public static void Import(string path, bool silent = false)
		{
			Dictionary<AddressType, Dictionary<UInt32, CodeLabel>> labels = new Dictionary<AddressType, Dictionary<UInt32, CodeLabel>>() {
				{ AddressType.InternalRam, new Dictionary<uint, CodeLabel>() },
				{ AddressType.PrgRom, new Dictionary<uint, CodeLabel>() },
				{ AddressType.Register, new Dictionary<uint, CodeLabel>() },
				{ AddressType.SaveRam, new Dictionary<uint, CodeLabel>() },
				{ AddressType.WorkRam, new Dictionary<uint, CodeLabel>() }
			};

			char[] separator = new char[1] { ':' };
			foreach(string row in File.ReadAllLines(path, Encoding.UTF8)) {
				string[] rowData = row.Split(separator, 4);

				AddressType type;
				switch(rowData[0][0]) {
					case 'G': type = AddressType.Register; break;
					case 'R': type = AddressType.InternalRam; break;
					case 'P': type = AddressType.PrgRom; break;
					case 'S': type = AddressType.SaveRam; break;
					case 'W': type = AddressType.WorkRam; break;
					default: continue;
				}

				uint address;
				if(UInt32.TryParse(rowData[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out address)) {
					CodeLabel codeLabel;
					if(!labels[type].TryGetValue(address, out codeLabel)) {
						codeLabel = new CodeLabel();
						codeLabel.Address = address;
						codeLabel.AddressType = type;
						codeLabel.Label = "";
						codeLabel.Comment = "";
						labels[type][address] = codeLabel;
					}

					if(rowData.Length > 3) {
						codeLabel.Comment = rowData[3].Replace("\\n", "\n");
					}
					codeLabel.Label = rowData[2].Replace("\\n", "\n").Replace("\n", "");
				}
			}

			foreach(KeyValuePair<AddressType, Dictionary<UInt32, CodeLabel>> kvp in labels) {
				LabelManager.SetLabels(kvp.Value.Values);
			}
		}

		public static void Export(string path)
		{
			List<CodeLabel> labels = new List<CodeLabel>(LabelManager.GetLabels());
			labels.Sort((CodeLabel a, CodeLabel b) => {
				int result = a.AddressType.CompareTo(b.AddressType);
				if(result == 0) {
					return a.Address.CompareTo(b.Address);
				} else {
					return result;
				}
			});

			StringBuilder sb = new StringBuilder();
			foreach(CodeLabel label in labels) {
				sb.Append(label.ToString() + "\n");
			}
			File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
		}
	}
}
