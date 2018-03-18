using Mesen.GUI.Config;
using Mesen.GUI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	class CodeTooltipManager
	{
		private bool _preventCloseTooltip = false;
		private string _hoverLastWord = "";
		private int _hoverLastLineAddress = -1;
		private Point _previousLocation;
		private Form _codeTooltip = null;
		private Control _owner = null;
		private ctrlScrollableTextbox _codeViewer = null;

		public string Code { get; set; }
		public Ld65DbgImporter SymbolProvider { get; set; }

		public CodeTooltipManager(Control owner, ctrlScrollableTextbox codeViewer)
		{
			_owner = owner;
			_codeViewer = codeViewer;
		}

		public void ShowTooltip(string word, Dictionary<string, string> values, int lineAddress, AddressTypeInfo? previewAddress)
		{
			if(_hoverLastWord != word || _hoverLastLineAddress != lineAddress || _codeTooltip == null) {
				if(!_preventCloseTooltip && _codeTooltip != null) {
					_codeTooltip.Close();
					_codeTooltip = null;
				}

				if(ConfigManager.Config.DebugInfo.ShowOpCodeTooltips && frmOpCodeTooltip.IsOpCode(word)) {
					_codeTooltip = new frmOpCodeTooltip(word, lineAddress);
				} else {
					_codeTooltip = new frmCodeTooltip(values, previewAddress.HasValue && previewAddress.Value.Type == AddressType.PrgRom ? previewAddress : null, Code, SymbolProvider);
				}
				_codeTooltip.Left = Cursor.Position.X + 10;
				_codeTooltip.Top = Cursor.Position.Y + 10;
				_codeTooltip.Show(_owner);
			}
			_codeTooltip.Left = Cursor.Position.X + 10;
			_codeTooltip.Top = Cursor.Position.Y + 10;

			_preventCloseTooltip = true;
			_hoverLastWord = word;
			_hoverLastLineAddress = lineAddress;
		}

		public void Close(bool forceClose = false)
		{
			if((!_preventCloseTooltip || forceClose) && _codeTooltip != null) {
				_codeTooltip.Close();
				_codeTooltip = null;
			}
			_preventCloseTooltip = false;
		}

		public void DisplayAddressTooltip(string word, UInt32 address)
		{
			byte byteValue = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, address);
			UInt16 wordValue = (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, address + 1) << 8));

			var values = new Dictionary<string, string>() {
				{ "Address", "$" + address.ToString("X4") },
				{ "Value", $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" }
			};

			this.ShowTooltip(word, values, -1, new AddressTypeInfo() { Address = (int)address, Type = AddressType.Register });
		}

		private void DisplayLabelTooltip(string word, CodeLabel label)
		{
			int relativeAddress = InteropEmu.DebugGetRelativeAddress(label.Address, label.AddressType);
			byte byteValue = relativeAddress >= 0 ? InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress) : (byte)0;
			UInt16 wordValue = relativeAddress >= 0 ? (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress + 1) << 8)) : (UInt16)0;

			var values = new Dictionary<string, string>() {
				{ "Label", label.Label },
				{ "Address", "$" + relativeAddress.ToString("X4") },
				{ "Value", (relativeAddress >= 0 ? $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" : "n/a") },
			};

			if(!string.IsNullOrWhiteSpace(label.Comment)) {
				values["Comment"] = label.Comment;
			}

			ShowTooltip(word, values, -1, new AddressTypeInfo() { Address = (int)label.Address, Type = label.AddressType });
		}

		private void DisplaySymbolTooltip(Ld65DbgImporter.SymbolInfo symbol)
		{
			int relativeAddress = symbol.Address.HasValue ? symbol.Address.Value : -1;
			byte byteValue = relativeAddress >= 0 ? InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress) : (byte)0;
			UInt16 wordValue = relativeAddress >= 0 ? (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress + 1) << 8)) : (UInt16)0;

			AddressTypeInfo? addressInfo = SymbolProvider.GetSymbolAddressInfo(symbol);

			if(addressInfo != null) {
				var values = new Dictionary<string, string>() {
					{ "Symbol", symbol.Name }
				};

				if(relativeAddress >= 0) {
					values["CPU Address"] = "$" + relativeAddress.ToString("X4");
				};

				if(addressInfo.Value.Type == AddressType.PrgRom) {
					values["PRG Offset"] = "$" + addressInfo.Value.Address.ToString("X4");
				}

				values["Value"] = (relativeAddress >= 0 ? $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" : "n/a");

				ShowTooltip(symbol.Name, values, -1, addressInfo);
			} else {
				var values = new Dictionary<string, string>() {
					{ "Symbol", symbol.Name },
					{ "Constant", "$" + relativeAddress.ToString("X2") }
				};
				ShowTooltip(symbol.Name, values, -1, addressInfo);
			}
		}

		public void ProcessMouseMove(Point location)
		{
			if(_previousLocation != location) {
				this.Close();

				_previousLocation = location;

				string word = _codeViewer.GetWordUnderLocation(location);
				if(word.StartsWith("$")) {
					try {
						UInt32 address = UInt32.Parse(word.Substring(1), NumberStyles.AllowHexSpecifier);

						AddressTypeInfo info = new AddressTypeInfo();
						InteropEmu.DebugGetAbsoluteAddressAndType(address, ref info);

						if(info.Address >= 0) {
							CodeLabel label = LabelManager.GetLabel((UInt32)info.Address, info.Type);
							if(label == null) {
								DisplayAddressTooltip(word, address);
							} else {
								DisplayLabelTooltip(word, label);
							}
						} else {
							DisplayAddressTooltip(word, address);
						}
					} catch { }
				} else {
					int address = _codeViewer.GetLineNumberAtPosition(location.Y);
					if(SymbolProvider != null) {
						int rangeStart, rangeEnd;
						if(_codeViewer.GetNoteRangeAtLocation(location.Y, out rangeStart, out rangeEnd)) {
							Ld65DbgImporter.SymbolInfo symbol = SymbolProvider.GetSymbol(word, rangeStart, rangeEnd);
							if(symbol != null) {
								DisplaySymbolTooltip(symbol);
								return;
							}
						}
					} else {
						CodeLabel label = LabelManager.GetLabel(word);
						if(label != null) {
							DisplayLabelTooltip(word, label);
							return;
						}
					}

					if(ConfigManager.Config.DebugInfo.ShowOpCodeTooltips && frmOpCodeTooltip.IsOpCode(word)) {
						ShowTooltip(word, null, -1, new AddressTypeInfo() { Address = address, Type = AddressType.Register });
					}
				}
			}
		}
	}
}
