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

			int errorCount = 0;
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

				if(importLabel) {
					string addressString = rowData[1];
					uint address = 0;
					uint length = 1;
					if(addressString.Contains("-")) {
						uint addressEnd;
						string[] addressStartEnd = addressString.Split('-');
						if(UInt32.TryParse(addressStartEnd[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out address) &&
							UInt32.TryParse(addressStartEnd[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out addressEnd)) {
							if(addressEnd > address) {
								length = addressEnd - address;
							} else {
								//Invalid label (start < end)
								errorCount++;
								continue;
							}
						} else {
							//Invalid label (can't parse)
							errorCount++;
							continue;
						}
					} else {
						if(!UInt32.TryParse(rowData[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out address)) {
							//Invalid label (can't parse)
							errorCount++;
							continue;
						}
						length = 1;
					}

					string labelName = rowData[2];
					if(!string.IsNullOrEmpty(labelName) && !LabelManager.LabelRegex.IsMatch(labelName)) {
						//Reject labels that don't respect the label naming restrictions
						errorCount++;
						continue;
					}

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
					codeLabel.Label = labelName;
					codeLabel.Length = length;
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
				string message = $"Import completed with {labelCount} labels imported";
				if(errorCount > 0) {
					message += $" and {errorCount} error(s)";
				}
				MessageBox.Show(message, "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
