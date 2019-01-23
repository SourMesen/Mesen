using Mesen.GUI.Config;
using Mesen.GUI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	class CodeTooltipManager : IDisposable
	{
		public static Regex LabelArrayFormat = new Regex("(.*)\\+(\\d+)", RegexOptions.Compiled);

		private string _hoverLastWord = "";
		private int _hoverLastLineAddress = -1;
		private Point _previousLocation;
		private TooltipForm _codeTooltip = null;
		private Control _owner = null;
		private ctrlScrollableTextbox _codeViewer = null;

		public CodeInfo Code { get; set; }
		public Ld65DbgImporter SymbolProvider { get; set; }

		public CodeTooltipManager(Control owner, ctrlScrollableTextbox codeViewer)
		{
			_owner = owner;
			_codeViewer = codeViewer;

			_codeViewer.MouseMove += ctrlCodeViewer_MouseMove;
			_codeViewer.MouseLeave += ctrlCodeViewer_MouseLeave;
			_codeViewer.MouseDown += ctrlCodeViewer_MouseDown;
			_codeViewer.ScrollPositionChanged += ctrlCodeViewer_ScrollPositionChanged;
		}

		public void ShowTooltip(string word, Dictionary<string, string> values, int lineAddress, AddressTypeInfo previewAddress)
		{
			if(ConfigManager.Config.DebugInfo.OnlyShowTooltipsOnShift && Control.ModifierKeys != Keys.Shift) {
				return;
			}

			Form parentForm = _owner.FindForm();

			if(_hoverLastWord != word || _hoverLastLineAddress != lineAddress || _codeTooltip == null) {
				if(_codeTooltip != null) {
					_codeTooltip.Close();
					_codeTooltip = null;
				}

				if(ConfigManager.Config.DebugInfo.ShowOpCodeTooltips && frmOpCodeTooltip.IsOpCode(word)) {
					_codeTooltip = new frmOpCodeTooltip(parentForm, word, lineAddress);
				} else {
					_codeTooltip = new frmCodeTooltip(parentForm, values, previewAddress != null && previewAddress.Type == AddressType.PrgRom ? previewAddress : null, Code, SymbolProvider);
				}
				_codeTooltip.FormClosed += (s, e) => { _codeTooltip = null; };
			}
			_codeTooltip.SetFormLocation(new Point(Cursor.Position.X + 10, Cursor.Position.Y + 10), _owner);

			_hoverLastWord = word;
			_hoverLastLineAddress = lineAddress;
		}

		public void Close()
		{
			if(_codeTooltip != null) {
				bool restoreFocus = _codeTooltip.NeedRestoreFocus;
				_codeTooltip.Close();
				if(restoreFocus) {
					_owner.Focus();
				}
				_codeTooltip = null;
			}
		}

		public void DisplayAddressTooltip(string word, UInt32 address)
		{
			byte byteValue = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, address);
			UInt16 wordValue = (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, address + 1) << 8));

			var values = new Dictionary<string, string>() {
				{ "Address", "$" + address.ToString("X4") },
				{ "Value", $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" }
			};

			AddressTypeInfo addressInfo = new AddressTypeInfo();
			InteropEmu.DebugGetAbsoluteAddressAndType(address, addressInfo);
			this.ShowTooltip(word, values, -1, addressInfo);
		}

		private void DisplayLabelTooltip(string word, CodeLabel label, int? arrayIndex = null)
		{
			int relativeAddress = InteropEmu.DebugGetRelativeAddress((uint)(label.Address + (arrayIndex ?? 0)), label.AddressType);
			byte byteValue = relativeAddress >= 0 ? InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress) : (byte)0;
			UInt16 wordValue = relativeAddress >= 0 ? (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress + 1) << 8)) : (UInt16)0;

			var values = new Dictionary<string, string>() {
				{ "Label", label.Label + (arrayIndex != null ? $"+{arrayIndex.Value}" : "") },
				{ "Address", "$" + relativeAddress.ToString("X4") },
				{ "Value", (relativeAddress >= 0 ? $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" : "n/a") },
			};

			if(!string.IsNullOrWhiteSpace(label.Comment)) {
				values["Comment"] = label.Comment;
			}

			ShowTooltip(word, values, -1, new AddressTypeInfo() { Address = (int)label.Address, Type = label.AddressType });
		}

		private void DisplaySymbolTooltip(Ld65DbgImporter.SymbolInfo symbol, int? arrayIndex = null)
		{
			AddressTypeInfo addressInfo = SymbolProvider.GetSymbolAddressInfo(symbol);

			if(addressInfo != null && addressInfo.Address >= 0) {
				int relativeAddress = InteropEmu.DebugGetRelativeAddress((uint)(addressInfo.Address + (arrayIndex ?? 0)), addressInfo.Type);
				byte byteValue = relativeAddress >= 0 ? InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress) : (byte)0;
				UInt16 wordValue = relativeAddress >= 0 ? (UInt16)(byteValue | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)relativeAddress + 1) << 8)) : (UInt16)0;

				var values = new Dictionary<string, string>() {
					{ "Symbol", symbol.Name + (arrayIndex != null ? $"+{arrayIndex.Value}" : "") }
				};

				if(relativeAddress >= 0) {
					values["CPU Address"] = "$" + relativeAddress.ToString("X4");
				} else {
					values["CPU Address"] = "<out of scope>";
				}

				if(addressInfo.Type == AddressType.PrgRom) {
					values["PRG Offset"] = "$" + (addressInfo.Address + (arrayIndex ?? 0)).ToString("X4");
				}

				values["Value"] = (relativeAddress >= 0 ? $"${byteValue.ToString("X2")} (byte){Environment.NewLine}${wordValue.ToString("X4")} (word)" : "n/a");

				ShowTooltip(symbol.Name, values, -1, addressInfo);
			} else {
				var values = new Dictionary<string, string>() {
					{ "Symbol", symbol.Name },
					{ "Constant", symbol.Address.HasValue ? ("$" + symbol.Address.Value.ToString("X2")) : "<unknown>" }
				};
				ShowTooltip(symbol.Name, values, -1, addressInfo);
			}
		}

		public void ProcessMouseMove(Point location)
		{
			if(_previousLocation != location) {
				bool closeExistingPopup = true;

				_previousLocation = location;

				string word = _codeViewer.GetWordUnderLocation(location);
				if(word.StartsWith("$")) {
					try {
						UInt32 address = UInt32.Parse(word.Substring(1), NumberStyles.AllowHexSpecifier);

						AddressTypeInfo info = new AddressTypeInfo();
						InteropEmu.DebugGetAbsoluteAddressAndType(address, info);

						if(info.Address >= 0) {
							CodeLabel label = LabelManager.GetLabel((UInt32)info.Address, info.Type);
							if(label == null) {
								DisplayAddressTooltip(word, address);
								closeExistingPopup = false;
							} else {
								DisplayLabelTooltip(word, label, 0);
								closeExistingPopup = false;
							}
						} else {
							DisplayAddressTooltip(word, address);
							closeExistingPopup = false;
						}
					} catch { }
				} else {
					Match arrayMatch = LabelArrayFormat.Match(word);
					int? arrayIndex = null;
					if(arrayMatch.Success) {
						word = arrayMatch.Groups[1].Value;
						arrayIndex = Int32.Parse(arrayMatch.Groups[2].Value);
					}

					int address = _codeViewer.GetLineNumberAtPosition(location.Y);
					if(SymbolProvider != null) {
						int rangeStart, rangeEnd;
						if(_codeViewer.GetNoteRangeAtLocation(location.Y, out rangeStart, out rangeEnd)) {
							Ld65DbgImporter.SymbolInfo symbol = SymbolProvider.GetSymbol(word, rangeStart, rangeEnd);
							if(symbol != null) {
								DisplaySymbolTooltip(symbol, arrayIndex);
								return;
							}
						}
					} else {
						CodeLabel label = LabelManager.GetLabel(word);
						if(label != null) {
							DisplayLabelTooltip(word, label, arrayIndex);
							return;
						}
					}

					if(ConfigManager.Config.DebugInfo.ShowOpCodeTooltips && frmOpCodeTooltip.IsOpCode(word)) {
						ShowTooltip(word, null, address, new AddressTypeInfo() { Address = address, Type = AddressType.Register });
						closeExistingPopup = false;
					}
				}

				if(closeExistingPopup) {
					this.Close();
				}
			}
		}

		private void ctrlCodeViewer_MouseMove(object sender, MouseEventArgs e)
		{
			ProcessMouseMove(e.Location);
		}

		private void ctrlCodeViewer_MouseLeave(object sender, EventArgs e)
		{
			Close();
		}

		private void ctrlCodeViewer_MouseDown(object sender, MouseEventArgs e)
		{
			Close();
		}

		private void ctrlCodeViewer_ScrollPositionChanged(object sender, EventArgs e)
		{
			Close();
		}

		public void Dispose()
		{
			_codeViewer.MouseMove -= ctrlCodeViewer_MouseMove;
			_codeViewer.MouseLeave -= ctrlCodeViewer_MouseLeave;
			_codeViewer.MouseDown -= ctrlCodeViewer_MouseDown;
			_codeViewer.ScrollPositionChanged -= ctrlCodeViewer_ScrollPositionChanged;
		}
	}
}
