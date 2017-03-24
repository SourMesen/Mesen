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
using Be.Windows.Forms;
using Mesen.GUI.Controls;
using static Be.Windows.Forms.DynamicByteProvider;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlHexViewer : UserControl
	{
		private FindOptions _findOptions;
		private StaticByteProvider _byteProvider;

		public ctrlHexViewer()
		{
			InitializeComponent();

			this.ctrlHexBox.Font = new Font(BaseControl.MonospaceFontFamily, 10, FontStyle.Regular);
			this.ctrlHexBox.SelectionForeColor = Color.White;
			this.ctrlHexBox.SelectionBackColor = Color.FromArgb(31, 123, 205);
			this.ctrlHexBox.ShadowSelectionColor = Color.FromArgb(100, 60, 128, 200);
			this.ctrlHexBox.InfoBackColor = Color.FromArgb(235, 235, 235);
			this.ctrlHexBox.InfoForeColor = Color.Gray;

			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime) {
				this.cboNumberColumns.SelectedIndex = ConfigManager.Config.DebugInfo.RamColumnCount;
			}
		}

		public byte[] GetData()
		{
			return this._byteProvider != null ? this._byteProvider.Bytes.ToArray() : new byte[0];
		}
		
		public void SetData(byte[] data)
		{
			if(data != null) {
				bool changed = true;
				if(this._byteProvider != null && data.Length == this._byteProvider.Bytes.Count) {
					changed = false;
					for(int i = 0; i < this._byteProvider.Bytes.Count; i++) {
						if(this._byteProvider.Bytes[i] != data[i]) {
							changed = true;
							break;
						}
					}
				}

				if(changed) {
					bool needInit = _byteProvider == null;
					_byteProvider = new StaticByteProvider(data);
					_byteProvider.ByteChanged += (int byteIndex, byte newValue, byte oldValue) => {
						ByteChanged?.Invoke(byteIndex, newValue, oldValue);
					};

					this.ctrlHexBox.ByteProvider = _byteProvider;

					if(needInit) {
						InitializeContextMenu?.Invoke(this.ctrlHexBox, EventArgs.Empty);
					}
				}
			}
		}

		private int ColumnCount
		{
			get { return Int32.Parse(this.cboNumberColumns.Text); }
		}

		public int RequiredWidth
		{
			get { return this.ctrlHexBox.RequiredWidth;	}
		}
				
		private void cboNumberColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ctrlHexBox.Focus();

			this.ctrlHexBox.BytesPerLine = this.ColumnCount;
			this.ctrlHexBox.UseFixedBytesPerLine = true;

			ConfigManager.Config.DebugInfo.RamColumnCount = this.cboNumberColumns.SelectedIndex;
			ConfigManager.ApplyChanges();
		}

		public Font HexFont
		{
			get { return this.ctrlHexBox.Font; }
		}

		public void IncreaseFontSize()
		{
			this.SetFontSize(Math.Min(40, this.ctrlHexBox.Font.Size + 1));
		}

		public void DecreaseFontSize()
		{
			this.SetFontSize(Math.Max(6, this.ctrlHexBox.Font.Size - 1));
		}

		public void SetFontSize(float size)
		{
			this.ctrlHexBox.Font = new Font(BaseControl.MonospaceFontFamily, size);
		}

		public void ResetFontSize()
		{
			this.SetFontSize(BaseControl.DefaultFontSize);
		}

		public void GoToAddress()
		{
			GoToAddress address = new GoToAddress();

			int currentAddr = (int)(this.ctrlHexBox.CurrentLine - 1) * this.ctrlHexBox.BytesPerLine;
			address.Address = (UInt32)currentAddr;

			frmGoToLine frm = new frmGoToLine(address);
			frm.StartPosition = FormStartPosition.Manual;
			Point topLeft = this.PointToScreen(new Point(0, 0));
			frm.Location = new Point(topLeft.X + (this.Width - frm.Width) / 2, topLeft.Y + (this.Height - frm.Height) / 2);
			if(frm.ShowDialog() == DialogResult.OK) {
				this.ctrlHexBox.ScrollByteIntoView((int)address.Address);
				this.ctrlHexBox.Focus();
			}
		}

		public void OpenSearchBox(bool forceFocus = false)
		{
			this._findOptions = new Be.Windows.Forms.FindOptions();
			this._findOptions.Type = chkTextSearch.Checked ? FindType.Text : FindType.Hex;
			this._findOptions.MatchCase = false;
			this._findOptions.Text = this.cboSearch.Text;
			this._findOptions.WrapSearch = true;

			bool focus = !this.panelSearch.Visible;
			this.panelSearch.Visible = true;

			if(Program.IsMono) {
				//Mono doesn't resize the TLP properly for some reason when set to autosize
				this.tlpMain.RowStyles[2].SizeType = System.Windows.Forms.SizeType.Absolute;
				this.tlpMain.RowStyles[2].Height = 30;
			}
			if(focus || forceFocus) {
				this.cboSearch.Focus();
				this.cboSearch.SelectAll();
			}
		}

		private void CloseSearchBox()
		{
			this.panelSearch.Visible = false;
			if(Program.IsMono) {
				//Mono doesn't resize the TLP properly for some reason when set to autosize
				this.tlpMain.RowStyles[2].SizeType = System.Windows.Forms.SizeType.Absolute;
				this.tlpMain.RowStyles[2].Height = 0;
			}			
			this.Focus();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch(keyData) {
				case Keys.Control | Keys.F: this.OpenSearchBox(true); return true;
				case Keys.Escape: this.CloseSearchBox(); return true;
				case Keys.Control | Keys.Oemplus: this.IncreaseFontSize(); return true;
				case Keys.Control | Keys.OemMinus: this.DecreaseFontSize(); return true;
				case Keys.Control | Keys.D0: this.ResetFontSize(); return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		public void FindNext()
		{
			this.OpenSearchBox();
			if(this.UpdateSearchOptions()) {
				if(this.ctrlHexBox.Find(this._findOptions, HexBox.eSearchDirection.Next) == -1) {
					this.lblSearchWarning.Text = "No matches found!";
				}
			}
		}

		public void FindPrevious()
		{
			this.OpenSearchBox();
			if(this.UpdateSearchOptions()) {
				if(this.ctrlHexBox.Find(this._findOptions, HexBox.eSearchDirection.Previous) == -1) {
					this.lblSearchWarning.Text = "No matches found!";
				}
			}
		}

		private void picCloseSearch_Click(object sender, EventArgs e)
		{
			this.CloseSearchBox();
		}

		private void picSearchPrevious_MouseUp(object sender, MouseEventArgs e)
		{
			this.FindPrevious();
		}

		private void picSearchNext_MouseUp(object sender, MouseEventArgs e)
		{
			this.FindNext();
		}

		private byte[] GetByteArray(string hexText, ref bool hasWildcard)
		{
			hexText = hexText.Replace(" ", "");

			try {
				List<byte> bytes = new List<byte>(hexText.Length/2);
				for(int i = 0; i < hexText.Length; i+=2) {
					if(i == hexText.Length - 1) {
						bytes.Add((byte)(Convert.ToByte(hexText.Substring(i, 1), 16) << 4));
						hasWildcard = true;
					} else {
						bytes.Add(Convert.ToByte(hexText.Substring(i, 2), 16));
					}
				}
				return bytes.ToArray();
			} catch {
				return new byte[0];
			}
		}

		private bool UpdateSearchOptions()
		{
			bool invalidSearchString = false;

			this._findOptions.MatchCase = this.chkMatchCase.Checked;

			if(this.chkTextSearch.Checked) {
				this._findOptions.Type = FindType.Text;
				this._findOptions.Text = this.cboSearch.Text;
				this._findOptions.HasWildcard = false;
			} else {
				this._findOptions.Type = FindType.Hex;
				bool hasWildcard = false;
				this._findOptions.Hex = this.GetByteArray(this.cboSearch.Text, ref hasWildcard);
				this._findOptions.HasWildcard = hasWildcard;
				invalidSearchString = this._findOptions.Hex.Length == 0 && this.cboSearch.Text.Trim().Length > 0;
			}

			this.lblSearchWarning.Text = "";

			bool emptySearch = this._findOptions.Text.Length == 0 || (!this.chkTextSearch.Checked && (this._findOptions.Hex == null || this._findOptions.Hex.Length == 0));
			if(invalidSearchString) {
				this.lblSearchWarning.Text = "Invalid search string";
			} else if(!emptySearch) {
				return true;
			}
			return false;
		}

		private void cboSearch_TextUpdate(object sender, EventArgs e)
		{
			if(this.UpdateSearchOptions()) {
				if(this.ctrlHexBox.Find(this._findOptions, HexBox.eSearchDirection.Incremental) == -1) {
					this.lblSearchWarning.Text = "No matches found!";
				}
			}
		}

		private void cboSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter) {
				this.FindNext();
				if(this.cboSearch.Items.Contains(this.cboSearch.Text)) {
					this.cboSearch.Items.Remove(this.cboSearch.Text);
				}
				this.cboSearch.Items.Insert(0, this.cboSearch.Text);

				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void chkTextSearch_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateSearchOptions();
		}
		
		public event EventHandler RequiredWidthChanged
		{
			add { this.ctrlHexBox.RequiredWidthChanged += value; }
			remove { this.ctrlHexBox.RequiredWidthChanged -= value; }
		}

		public event EventHandler InitializeContextMenu;
		public event ByteChangedHandler ByteChanged;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IByteCharConverter ByteCharConverter
		{
			get { return this.ctrlHexBox.ByteCharConverter; }
			set { this.ctrlHexBox.ByteCharConverter = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IByteColorProvider ByteColorProvider
		{
			get { return this.ctrlHexBox.ByteColorProvider; }
			set { this.ctrlHexBox.ByteColorProvider = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool StringViewVisible
		{
			get { return this.ctrlHexBox.StringViewVisible; }
			set { this.ctrlHexBox.StringViewVisible = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ReadOnly
		{
			get { return this.ctrlHexBox.ReadOnly; }
			set { this.ctrlHexBox.ReadOnly = value; }
		}
	}
}
