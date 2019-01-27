using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlCharacterMapping : UserControl
	{
		private Color _activeBgColor = Color.FromArgb(180, 250, 200);
		private TextBox[] _txtMappings;
		private byte[][] _tileData = new byte[16][];
		private ConcurrentDictionary<string, string> _charMappings;
		private static readonly uint[] _hexLookup = InitializeHexLookup();

		public ctrlCharacterMapping()
		{
			InitializeComponent();

			for(int i = 0; i < 16; i++) {
				_tileData[i] = new byte[16];
			}

			_txtMappings = new TextBox[16] {
				txtMapping0, txtMapping1, txtMapping2, txtMapping3, txtMapping4, txtMapping5, txtMapping6, txtMapping7,
				txtMapping8, txtMapping9, txtMapping10, txtMapping11, txtMapping12, txtMapping13, txtMapping14, txtMapping15
			};

			if(Program.IsMono) {
				this.SuspendLayout();
				tlpTiles.SuspendLayout();
				//this.picTiles.Margin = new Padding(0, 5, 0, 1);
				for(int i = 0; i < 16; i++) {
					_txtMappings[i].Width -= 4;
				}
				this.Width -= 72;
				this.Height -= 4;
				tlpTiles.ResumeLayout();
				this.ResumeLayout();
			}
		}

		private static uint[] InitializeHexLookup()
		{
			var result = new uint[256];
			for(int i = 0; i < 256; i++) {
				string s = i.ToString("X2");
				result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
			}
			return result;
		}

		private static string ToHexString(byte[] bytes)
		{
			char[] result = new char[bytes.Length * 2];
			for(int i = 0; i < bytes.Length; i++) {
				var val = _hexLookup[bytes[i]];
				result[2 * i] = (char)val;
				result[2 * i + 1] = (char)(val >> 16);
			}
			return new string(result);
		}

		public static string GetColorIndependentKey(byte[] tileData)
		{
			sbyte nextColor = 0;
			byte[] colorKey = new byte[16];
			sbyte[] lookupTable = new sbyte[4] { -1, -1, -1, -1 };
			for(int y = 0; y < 8; y++) {
				byte lowByte = tileData[y];
				byte highByte = tileData[y + 8];

				for(int x = 0; x < 8; x++) {
					byte color = (byte)((lowByte & 0x01) | ((highByte << 1) & 0x02));
					lowByte >>= 1;
					highByte >>= 1;
					if(lookupTable[color] == -1) {
						lookupTable[color] = nextColor;
						nextColor++;
					}

					colorKey[(y << 1) + x / 4] |= (byte)(lookupTable[color] << ((x & 0x03) << 1));
				}
			}
			
			return ToHexString(colorKey);
		}

		public void UpdateData(byte[] chrData, int rowIndex)
		{
			for(int i = 0; i < 16; i++) {
				Array.Copy(chrData, rowIndex * 256 + i * 16, _tileData[i], 0, 16);

				string key = GetColorIndependentKey(_tileData[i]);
				string mapping;
				if(_charMappings.TryGetValue(key, out mapping)) {
					_txtMappings[i].Text = mapping;
					_txtMappings[i].BackColor = _activeBgColor;
				} else {
					_txtMappings[i].Text = "";
					_txtMappings[i].BackColor = Color.White;
				}
				_txtMappings[i].ForeColor = Color.Black;
			}
		}

		private void txtMapping_TextChanged(object sender, EventArgs e)
		{
			int i = Array.IndexOf(_txtMappings, sender);
			string key = GetColorIndependentKey(_tileData[i]);

			string text = ((TextBox)sender).Text;
			if(string.IsNullOrWhiteSpace(text)) {
				_charMappings.TryRemove(key, out text);
				((TextBox)sender).BackColor = Color.White;
			} else {
				_charMappings[key] = text;
				((TextBox)sender).BackColor = _activeBgColor;
			}
		}

		public void SetCharacterMappings(ConcurrentDictionary<string, string> charMappings)
		{
			_charMappings = charMappings;
		}
	}
}
