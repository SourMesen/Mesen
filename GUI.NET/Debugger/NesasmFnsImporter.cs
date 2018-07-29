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
	public class NesasmFnsImporter
	{
		public static void Import(string path, bool silent = false)
		{
			//This only works reliably for NROM games with 32kb PRG
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			bool hasLargePrg = state.Cartridge.PrgRomSize != 0x8000;
			if(!silent && hasLargePrg) {
				if(MessageBox.Show($"Warning: Due to .fns file format limitations, imported labels are not reliable for games that have more than 32kb of PRG ROM.\n\nAre you sure you want to import these labels?", "Mesen", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
					return;
				}
			}
			Dictionary<UInt32, CodeLabel> labels = new Dictionary<uint, CodeLabel>();

			char[] separator = new char[1] { '=' };
			foreach(string row in File.ReadAllLines(path, Encoding.UTF8)) {
				string[] rowData = row.Split(separator);
				if(rowData.Length < 2) {
					//Invalid row
					continue;
				}

				uint address;
				if(UInt32.TryParse(rowData[1].Trim().Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out address)) {
					CodeLabel codeLabel;
					if(!labels.TryGetValue(address, out codeLabel)) {
						codeLabel = new CodeLabel();
						codeLabel.Address = hasLargePrg ? address : (address - 0x8000);
						codeLabel.AddressType = hasLargePrg ? AddressType.Register : AddressType.PrgRom;
						codeLabel.Label = "";
						codeLabel.Comment = "";
						labels[address] = codeLabel;
					}

					codeLabel.Label = rowData[0].Trim();
				}
			}

			LabelManager.SetLabels(labels.Values);

			if(!silent) {
				MessageBox.Show($"Import completed with {labels.Values.Count} labels imported.", "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
