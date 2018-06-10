using Mesen.GUI.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

			DebugImportConfig config = ConfigManager.Config.DebugInfo.ImportConfig;

			char[] separator = new char[1] { ':' };
			foreach(string row in File.ReadAllLines(path, Encoding.UTF8)) {
				string[] rowData = row.Split(separator, 4);
				if(rowData.Length < 3) {
					//Invalid row
					continue;
				}
				AddressType type;
				bool importLabel = false;
				switch(rowData[0][0]) {
					case 'G': type = AddressType.Register; importLabel = config.MlbImportRegisterLabels; break;
					case 'R': type = AddressType.InternalRam; importLabel = config.MlbImportInternalRamLabels; break;
					case 'P': type = AddressType.PrgRom; importLabel = config.MlbImportPrgRomLabels; break;
					case 'S': type = AddressType.SaveRam; importLabel = config.MlbImportSaveRamLabels; break;
					case 'W': type = AddressType.WorkRam; importLabel = config.MlbImportWorkRamLabels; break;
					default: continue;
				}

				uint address;
				if(importLabel && UInt32.TryParse(rowData[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out address)) {
					CodeLabel codeLabel;
					if(!labels[type].TryGetValue(address, out codeLabel)) {
						codeLabel = new CodeLabel();
						codeLabel.Address = address;
						codeLabel.AddressType = type;
						codeLabel.Label = "";
						codeLabel.Comment = "";
						labels[type][address] = codeLabel;
					}

					if(rowData.Length > 3 && config.MlbImportComments) {
						codeLabel.Comment = rowData[3].Replace("\\n", "\n");
					}
					codeLabel.Label = rowData[2].Replace("\\n", "\n").Replace("\n", "");
				}
			}

			int labelCount = 0;
			foreach(KeyValuePair<AddressType, Dictionary<UInt32, CodeLabel>> kvp in labels) {
				labelCount += kvp.Value.Values.Count;
			}
			List<CodeLabel> codeLabels = new List<CodeLabel>();
			foreach(KeyValuePair<AddressType, Dictionary<UInt32, CodeLabel>> kvp in labels) {
				codeLabels.AddRange(kvp.Value.Values);
			}
			LabelManager.SetLabels(codeLabels);

			if(!silent) {
				MessageBox.Show($"Import completed with {labelCount} labels imported.", "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
