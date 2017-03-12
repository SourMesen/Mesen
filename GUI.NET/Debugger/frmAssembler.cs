using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Windows.Forms;
using Mesen.GUI.Controls;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmAssembler : BaseForm
	{
		private UInt16 _startAddress;
		private UInt16 _blockLength;
		private bool _hasParsingErrors = false;
		private bool _containedRtiRts = false;
		private bool _isEditMode = false;
		private bool _startAddressValid = true;

		public frmAssembler(string code = "", UInt16 startAddress = 0, UInt16 blockLength = 0)
		{
			InitializeComponent();

			if(!string.IsNullOrWhiteSpace(code)) {
				_isEditMode = true;
				_containedRtiRts = ContainsRtiOrRts(code);
			}

			_startAddress = startAddress;
			_blockLength = blockLength;

			txtCode.Font = new Font(BaseControl.MonospaceFontFamily, 10);
			ctrlHexBox.Font = new Font(BaseControl.MonospaceFontFamily, 10, FontStyle.Regular);
			ctrlHexBox.SelectionForeColor = Color.White;
			ctrlHexBox.SelectionBackColor = Color.FromArgb(31, 123, 205);
			ctrlHexBox.InfoBackColor = Color.FromArgb(235, 235, 235);
			ctrlHexBox.InfoForeColor = Color.Gray;
			ctrlHexBox.Width = ctrlHexBox.RequiredWidth;
			ctrlHexBox.ByteProvider = new StaticByteProvider(new byte[0]);

			txtStartAddress.Text = _startAddress.ToString("X4");
			txtCode.Text = code;
			txtCode.Select(0, 0);

			toolTip.SetToolTip(picSizeWarning, "Warning: The new code exceeds the original code's length." + Environment.NewLine + "Applying this modification will overwrite other portions of the code and potentially cause problems.");
			toolTip.SetToolTip(picStartAddressWarning, "Warning: Start address is invalid.  Must be a valid hexadecimal string.");

			UpdateWindow();
		}

		private bool ContainsRtiOrRts(string code)
		{
			return Regex.IsMatch(code, "^\\s*(RTI|RTS)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		private bool SizeExceeded
		{
			get { return ctrlHexBox.ByteProvider.Length > _blockLength; }
		}

		private bool NeedRtiRtsWarning
		{
			get { return _containedRtiRts && !ContainsRtiOrRts(txtCode.Text); }
		}

		private bool IsIdentical
		{
			get
			{
				if(ctrlHexBox.ByteProvider.Length != _blockLength) {
					return false;
				} else {
					for(int i = 0; i < ctrlHexBox.ByteProvider.Length; i++) {
						if(InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (UInt32)(_startAddress + i)) != ctrlHexBox.ByteProvider.ReadByte(i)) {
							return false;
						}
					}
					return true;
				}
			}
		}

		private void UpdateWindow()
		{
			short[] byteCode = InteropEmu.DebugAssembleCode(txtCode.Text, _startAddress);

			List<byte> convertedByteCode = new List<byte>();
			List<ErrorDetail> errorList = new List<ErrorDetail>();
			string[] codeLines = txtCode.Text.Replace("\r", "").Split('\n');
			int line = 1;
			foreach(short s in byteCode) {
				if(s >= 0) {
					convertedByteCode.Add((byte)s);
				} else if(s == (int)AssemblerSpecialCodes.EndOfLine) {
					line++;
				} else if(s < (int)AssemblerSpecialCodes.EndOfLine) {
					string message = "unknown error";
					switch((AssemblerSpecialCodes)s) {
						case AssemblerSpecialCodes.ParsingError: message = "Invalid syntax"; break;
						case AssemblerSpecialCodes.OutOfRangeJump: message = "Relative jump is out of range (-128 to 127)"; break;
						case AssemblerSpecialCodes.LabelRedefinition: message = "Cannot redefine an existing label"; break;
						case AssemblerSpecialCodes.MissingOperand: message = "Operand is missing"; break;
						case AssemblerSpecialCodes.OperandOutOfRange: message = "Operand is out of range (invalid value)"; break;
						case AssemblerSpecialCodes.InvalidHex: message = "Hexadecimal string is invalid"; break;
						case AssemblerSpecialCodes.InvalidSpaces: message = "Operand cannot contain spaces"; break;
						case AssemblerSpecialCodes.TrailingText: message = "Invalid text trailing at the end of line"; break;
						case AssemblerSpecialCodes.UnknownLabel: message = "Unknown label"; break;
						case AssemblerSpecialCodes.InvalidInstruction: message = "Invalid instruction"; break;
					}
					errorList.Add(new ErrorDetail() { Message = message + " - " + codeLines[line-1], LineNumber = line });
					line++;
				}
			}

			_hasParsingErrors = errorList.Count > 0;

			lstErrors.BeginUpdate();
			lstErrors.Items.Clear();
			lstErrors.Items.AddRange(errorList.ToArray());
			lstErrors.EndUpdate();

			ctrlHexBox.ByteProvider = new StaticByteProvider(convertedByteCode.ToArray());

			if(_isEditMode) {
				lblByteUsage.Text = ctrlHexBox.ByteProvider.Length.ToString() + " / " + _blockLength.ToString();
				lblByteUsage.ForeColor = SizeExceeded ? Color.Red : Color.Black;
				picSizeWarning.Visible = SizeExceeded;

				bool isIdentical = IsIdentical;
				lblNoChanges.Visible = isIdentical;
				btnOk.Image = _hasParsingErrors || NeedRtiRtsWarning || SizeExceeded ? Properties.Resources.Warning : null;
				btnOk.Enabled = !isIdentical && _startAddressValid && ctrlHexBox.ByteProvider.Length > 0;
			} else {
				lblNoChanges.Visible = false;
				btnOk.Image = _hasParsingErrors ? Properties.Resources.Warning : null;
				btnOk.Enabled = _startAddressValid && ctrlHexBox.ByteProvider.Length > 0;

				lblByteUsage.Text = ctrlHexBox.ByteProvider.Length.ToString();
			}

			btnExecute.Enabled = ctrlHexBox.ByteProvider.Length > 0 && ctrlHexBox.ByteProvider.Length <= 4000;

			picStartAddressWarning.Visible = !_startAddressValid;
		}

		private void txtCode_TextChanged(object sender, EventArgs e)
		{
			UpdateWindow();
		}

		private void txtCode_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Control && e.KeyCode == Keys.V || e.Shift && e.KeyCode == Keys.Insert) {
				txtCode.Paste(DataFormats.GetFormat("Text"));
				e.Handled = true;
			}
		}

		private void txtCode_SelectionChanged(object sender, EventArgs e)
		{
			lblLineNumber.Text = (txtCode.GetLineFromCharIndex(txtCode.GetFirstCharIndexOfCurrentLine()) + 1).ToString();
		}

		private void txtStartAddress_TextChanged(object sender, EventArgs e)
		{
			try {
				_startAddress = UInt16.Parse(txtStartAddress.Text, System.Globalization.NumberStyles.HexNumber);
				_startAddressValid = true;
			} catch {
				_startAddressValid = false;
			}
			UpdateWindow();
		}

		private void btnExecute_Click(object sender, EventArgs e)
		{
			WaitUntilBreak();

			List<string> warningMessages = new List<string>();
			if(_hasParsingErrors) {
				warningMessages.Add("Warning: The code contains parsing errors - lines with errors will be ignored.");
			}
			warningMessages.Add("Warning: The CPU will not resume its normal execution until a write to $3000 is performed (or the last line of code is reached)!");
			warningMessages.Add("This will assemble and execute the code in the $3000-$3FFF range, starting at address $3000." + Environment.NewLine + Environment.NewLine + "Execute?");

			if(MessageBox.Show(string.Join(Environment.NewLine+Environment.NewLine, warningMessages.ToArray()), "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK) {
				UInt16 originalAddress = _startAddress;
				_startAddress = 0x3000;
				UpdateWindow();
				List<byte> code = ((StaticByteProvider)ctrlHexBox.ByteProvider).Bytes;
				_startAddress = originalAddress;
				UpdateWindow();

				//Inject a STA $3000 instruction at the end - not flawless, but will work for typical scenarions
				code.Add(0x8D);
				code.Add(0x00);
				code.Add(0x30);

				InteropEmu.DebugStartCodeRunner(code.ToArray());
				InteropEmu.DebugRun();
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			WaitUntilBreak();

			List<string> warningMessages = new List<string>();
			if(_hasParsingErrors) {
				warningMessages.Add("Warning: The code contains parsing errors - lines with errors will be ignored.");
			}
			if(_isEditMode) {
				if(SizeExceeded) {
					warningMessages.Add("Warning: The new code exceeds the original code's length." + Environment.NewLine + "Applying this modification will overwrite other portions of the code and potentially cause problems.");
				}
				if(NeedRtiRtsWarning) {
					warningMessages.Add("Warning: The code originally contained an RTI/RTS instruction and it no longer does - this will probably cause problems.");
				}
			} else {
				warningMessages.Add($"Warning: The contents currently mapped to CPU memory addresses ${_startAddress.ToString("X4")} to ${(_startAddress+ctrlHexBox.ByteProvider.Length).ToString("X4")} will be overridden.");
			}

			if(warningMessages.Count == 0 || MessageBox.Show(string.Join(Environment.NewLine+Environment.NewLine, warningMessages.ToArray()) + Environment.NewLine + Environment.NewLine + "OK?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK) {
				byte lastByte = ctrlHexBox.ByteProvider.ReadByte(ctrlHexBox.ByteProvider.Length - 1);
				bool endsWithRtiRts = lastByte == 0x40 || lastByte == 0x60;
				int byteGap = (int)(_blockLength - ctrlHexBox.ByteProvider.Length);
				List<byte> bytes = new List<byte>(((StaticByteProvider)ctrlHexBox.ByteProvider).Bytes);
				if(byteGap > 0) {
					//Pad data with NOPs as needed
					int insertPoint = endsWithRtiRts ? bytes.Count - 1 : bytes.Count;
					for(int i = 0; i < byteGap; i++) {
						bytes.Insert(insertPoint, 0xEA); //0xEA = NOP
					}
				}

				InteropEmu.DebugSetMemoryValues(DebugMemoryType.CpuMemory, (UInt32)_startAddress, bytes.ToArray());

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void WaitUntilBreak()
		{
			while(!InteropEmu.DebugIsExecutionStopped()) {
				InteropEmu.DebugStep(1);
				System.Threading.Thread.Sleep(100);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		enum AssemblerSpecialCodes
		{
			OK = 0,
			EndOfLine = -1,
			ParsingError = -2,
			OutOfRangeJump = -3,
			LabelRedefinition = -4,
			MissingOperand = -5,
			OperandOutOfRange = -6,
			InvalidHex = -7,
			InvalidSpaces = -8,
			TrailingText = -9,
			UnknownLabel = -10,
			InvalidInstruction = -11,
		}

		private void lstErrors_DoubleClick(object sender, EventArgs e)
		{
			if(lstErrors.SelectedItem != null) {
				int lineNumber = (lstErrors.SelectedItem as ErrorDetail).LineNumber;
				int scrollIndex = txtCode.GetFirstCharIndexFromLine(Math.Max(0, lineNumber-1-txtCode.NumberOfVisibleLines/2));
				txtCode.SelectionStart = scrollIndex + 2;
				txtCode.ScrollToCaret();

				int errorIndex = txtCode.GetFirstCharIndexFromLine(lineNumber-1);
				txtCode.SelectionStart = errorIndex + 2;

				txtCode.Focus();
			}
		}

		private class ErrorDetail
		{
			public string Message { get; set; }
			public int LineNumber { get; set; }

			public override string ToString()
			{
				return "Line " + LineNumber.ToString() + ": " + this.Message;
			}
		}
	}

	public class ZoomlessRichTextBox : RichTextBox
	{
		public int NumberOfVisibleLines
		{
			get
			{
				int topIndex = this.GetCharIndexFromPosition(new Point(1, 1));
				int bottomIndex = this.GetCharIndexFromPosition(new Point(1, this.Height - 1));
				int topLine = this.GetLineFromCharIndex(topIndex);
				int bottomLine = this.GetLineFromCharIndex(bottomIndex);
				return bottomLine - topLine + 1;
			}
		}

		protected override void WndProc(ref Message m)
		{
			const int WM_SCROLLWHEEL = 0x20A;

			bool ctrl = Control.ModifierKeys.HasFlag(Keys.Control);
			bool wheel = m.Msg == WM_SCROLLWHEEL;

			if(!ctrl || !wheel) {
				//Block mouse wheel messages
				base.WndProc(ref m);
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			e.SuppressKeyPress = e.Control && e.Shift && (e.KeyValue == (int)Keys.Oemcomma || e.KeyValue == (int)Keys.OemPeriod);
			base.OnKeyDown(e);
		}
	}
}
