using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlHexViewer : BaseScrollableTextboxUserControl
	{
		public event EventHandler ColumnCountChanged;

		private int _currentColumnCount;
		private byte[] _data;

		public ctrlHexViewer()
		{
			InitializeComponent();

			this.cboNumberColumns.SelectedIndex = ConfigManager.Config.DebugInfo.RamColumnCount;
		}

		protected override ctrlScrollableTextbox ScrollableTextbox
		{
			get { return this.ctrlDataViewer; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public byte[] Data
		{
			get { return this._data; }
			set 
			{
				if(value != null) {
					if(_currentColumnCount != this.ColumnCount) {
						_currentColumnCount = this.ColumnCount;
					}

					string[] hexContent, previousHexContent = null;
					int[] lineNumbers, previousLineNumbers;
					if(this._data != null && this._data.Length == value.Length) {
						this.ArrayToHex(this._data, out previousHexContent, out previousLineNumbers);
					}
					this.ArrayToHex(value, out hexContent, out lineNumbers);
					this.ctrlDataViewer.CompareLines = previousHexContent;
					this.ctrlDataViewer.TextLines = hexContent;

					this._data = value;
					this.ctrlDataViewer.LineNumbers = lineNumbers;
					if(this.ColumnCount == 16) {
						this.ctrlDataViewer.Header = "00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F";
					} else if(this.ColumnCount == 32) {
						this.ctrlDataViewer.Header = "00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F 10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F";
					} else if(this.ColumnCount == 64) {
						this.ctrlDataViewer.Header = "00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F 10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F 20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F 30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F";
					} else {
						this.ctrlDataViewer.Header = null;
					}
				}
			}
		}

		private int ColumnCount
		{
			get { return Int32.Parse(this.cboNumberColumns.Text); }
		}

		public int IdealWidth
		{
			get
			{
				return 135 + this.ColumnCount * 30;
			}
		}

		private string[] _lookupTable = new string[] {
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F",
			"60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F",
			"70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F",
			"80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F",
			"90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F",
			"A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF",
			"B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF",
			"C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF",
			"D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF",
			"E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF",
			"F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF",  
		};

		private void ArrayToHex(byte[] data, out string[] hexContent, out int[] lineNumbers)
		{
			int columnCount = this.ColumnCount;
			int lineNumber = 0;

			int rowCount = data.Length / columnCount;
			if(data.Length % columnCount != 0) {
				rowCount++;
			}

			hexContent = new string[rowCount];
			lineNumbers = new int[rowCount];

			StringBuilder sb = new StringBuilder(columnCount * 4);
			int columnCounter = 0;
			int lineCounter = 0;
			foreach(byte b in data) {
				sb.Append(_lookupTable[b]);
				sb.Append(" ");
				
				columnCounter++;
				if(columnCounter == columnCount) {
					hexContent[lineCounter] = sb.ToString();
					lineNumbers[lineCounter] = lineNumber;
					sb.Clear();
					columnCounter = 0;
					lineCounter++;
					lineNumber += columnCount;
				}
			}
			if(sb.Length > 0) {
				hexContent[lineCounter] = sb.ToString();
				lineNumbers[lineCounter] = lineNumber;
			}
		}

		private void cboNumberColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(this.ColumnCountChanged != null) {
				this.ColumnCountChanged(sender, e);
			}
			this.Data = _data;
			this.ctrlDataViewer.Focus();

			ConfigManager.Config.DebugInfo.RamColumnCount = this.cboNumberColumns.SelectedIndex;
			ConfigManager.ApplyChanges();
		}

		Point _previousLocation;
		private void ctrlDataViewer_MouseMove(object sender, MouseEventArgs e)
		{
			if(_previousLocation != e.Location) {
				string currentWord = this.GetWordUnderLocation(e.Location, false);
				string originalWord = this.GetWordUnderLocation(e.Location, true);

				if(currentWord != originalWord) {
					this.toolTip.Show("Previous Value: $" + originalWord + Environment.NewLine + "Current Value: $" + currentWord, this.ctrlDataViewer, e.Location.X + 5, e.Location.Y - 40, 3000);
				} else {
					this.toolTip.Hide(this.ctrlDataViewer);
				}
				_previousLocation = e.Location;
			}
		}

		private void ctrlDataViewer_FontSizeChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.RamFontSize = this.ctrlDataViewer.FontSize;
			ConfigManager.ApplyChanges();
		}
	}
}
