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
	public partial class ctrlHexViewer : BaseControl
	{
		private FindOptions _findOptions;
		private StaticByteProvider _byteProvider;
		private DebugMemoryType _memoryType;

		public ctrlHexViewer()
		{
			InitializeComponent();

			this.BaseFont = new Font(BaseControl.MonospaceFontFamily, 10, FontStyle.Regular);
			this.ctrlHexBox.ContextMenuStrip = this.ctxMenuStrip;
			this.ctrlHexBox.SelectionForeColor = Color.White;
			this.ctrlHexBox.SelectionBackColor = Color.FromArgb(31, 123, 205);
			this.ctrlHexBox.ShadowSelectionColor = Color.FromArgb(100, 60, 128, 200);
			this.ctrlHexBox.InfoBackColor = Color.FromArgb(235, 235, 235);
			this.ctrlHexBox.InfoForeColor = Color.Gray;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!IsDesignMode) {
				this.cboNumberColumns.SelectedIndex = ConfigManager.Config.DebugInfo.RamColumnCount;
				InitShortcuts();
			}
		}

		private void InitShortcuts()
		{
			mnuFreeze.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_Freeze));
			mnuUnfreeze.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_Unfreeze));

			mnuAddToWatch.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_AddToWatch));
			mnuEditBreakpoint.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_EditBreakpoint));
			mnuEditLabel.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_EditLabel));

			mnuUndo.InitShortcut(this, nameof(DebuggerShortcutsConfig.Undo));
			mnuCopy.InitShortcut(this, nameof(DebuggerShortcutsConfig.Copy));
			mnuPaste.InitShortcut(this, nameof(DebuggerShortcutsConfig.Paste));
			mnuSelectAll.InitShortcut(this, nameof(DebuggerShortcutsConfig.SelectAll));

			mnuMarkAsCode.InitShortcut(this, nameof(DebuggerShortcutsConfig.MarkAsCode));
			mnuMarkAsData.InitShortcut(this, nameof(DebuggerShortcutsConfig.MarkAsData));
			mnuMarkAsUnidentifiedData.InitShortcut(this, nameof(DebuggerShortcutsConfig.MarkAsUnidentified));
		}

		public byte[] GetData()
		{
			return this._byteProvider != null ? this._byteProvider.Bytes.ToArray() : new byte[0];
		}
		
		public void RefreshData(DebugMemoryType memoryType)
		{
			if(_memoryType != memoryType) {
				_memoryType = memoryType;
				_byteProvider = null;
			}

			byte[] data = InteropEmu.DebugGetMemoryState(this._memoryType);

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
					if(_byteProvider == null || _byteProvider.Length != data.Length) {
						_byteProvider = new StaticByteProvider(data);
						_byteProvider.ByteChanged += (int byteIndex, byte newValue, byte oldValue) => {
							InteropEmu.DebugSetMemoryValue(_memoryType, (UInt32)byteIndex, newValue);
						};
						_byteProvider.BytesChanged += (int byteIndex, byte[] values) => {
							InteropEmu.DebugSetMemoryValues(_memoryType, (UInt32)byteIndex, values);
						};
						this.ctrlHexBox.ByteProvider = _byteProvider;
					} else {
						_byteProvider.SetData(data);
					}
					this.ctrlHexBox.Refresh();
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

		private int _textZoom = 100;
		public int TextZoom
		{
			get { return _textZoom; }
			set
			{
				if(value >= 30 && value <= 500) {
					_textZoom = value;
					this.UpdateFont();
				}
			}
		}

		private Font _baseFont = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize, FontStyle.Regular); 
		public Font BaseFont {
			get { return _baseFont; }
			set
			{
				if(!value.Equals(_baseFont)) {
					_baseFont = value;
					this.UpdateFont();
				}
			}
		}

		public void UpdateFont()
		{
			this.ctrlHexBox.Font = new Font(BaseFont.FontFamily, BaseFont.Size * _textZoom / 100f, BaseFont.Style);
		}
		
		public void GoToAddress(int address)
		{
			this.ctrlHexBox.ScrollByteIntoView(GetData().Length - 1);
			this.ctrlHexBox.ScrollByteIntoView(address);
			this.ctrlHexBox.Select(address, 0);
			this.ctrlHexBox.Focus();
		}

		public void GoToAddress()
		{
			GoToAddress address = new GoToAddress();

			int currentAddr = (int)(this.ctrlHexBox.CurrentLine - 1) * this.ctrlHexBox.BytesPerLine;
			address.Address = (UInt32)currentAddr;

			frmGoToLine frm = new frmGoToLine(address, (_byteProvider.Length - 1).ToString("X").Length);
			frm.StartPosition = FormStartPosition.Manual;
			Point topLeft = this.PointToScreen(new Point(0, 0));
			frm.Location = new Point(topLeft.X + (this.Width - frm.Width) / 2, topLeft.Y + (this.Height - frm.Height) / 2);
			if(frm.ShowDialog() == DialogResult.OK) {
				GoToAddress((int)address.Address);
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
			this.UpdateActionAvailability();

			if(keyData == ConfigManager.Config.DebugInfo.Shortcuts.Find) {
				this.OpenSearchBox(true);
				return true;
			} else if(keyData == ConfigManager.Config.DebugInfo.Shortcuts.IncreaseFontSize) {
				this.TextZoom += 10;
				return true;
			} else if(keyData == ConfigManager.Config.DebugInfo.Shortcuts.DecreaseFontSize) {
				this.TextZoom -= 10;
				return true;
			} else if(keyData == ConfigManager.Config.DebugInfo.Shortcuts.ResetFontSize) {
				this.TextZoom = 100;
				return true;
			}

			if(this.cboSearch.Focused) {
				if(keyData == Keys.Escape) {
					this.CloseSearchBox();
					return true;
				}
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

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool HighDensityMode
		{
			get { return this.ctrlHexBox.HighDensityMode; }
			set { this.ctrlHexBox.HighDensityMode = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool EnablePerByteNavigation
		{
			get { return this.ctrlHexBox.EnablePerByteNavigation; }
			set { this.ctrlHexBox.EnablePerByteNavigation = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ByteEditingMode
		{
			get { return this.ctrlHexBox.ByteEditingMode; }
			set { this.ctrlHexBox.ByteEditingMode = value; }
		}

		public delegate void ByteMouseHoverHandler(int address);
		public event ByteMouseHoverHandler ByteMouseHover; 
		private void ctrlHexBox_MouseMove(object sender, MouseEventArgs e)
		{
			BytePositionInfo bpi = ctrlHexBox.GetHexBytePositionInfo(e.Location);
			ByteMouseHover?.Invoke((int)bpi.Index);
		}

		private void ctrlHexBox_MouseLeave(object sender, EventArgs e)
		{
			ByteMouseHover?.Invoke(-1);
		}

		private void UpdateLocationLabel()
		{
			if(ctrlHexBox.SelectionLength > 0) {
				this.lblLocation.Text = $"Selection: ${ctrlHexBox.SelectionStart.ToString("X4")} - ${(ctrlHexBox.SelectionStart + ctrlHexBox.SelectionLength - 1).ToString("X4")} ({ctrlHexBox.SelectionLength} bytes)";
			} else {
				this.lblLocation.Text = $"Location: ${ctrlHexBox.SelectionStart.ToString("X4")}";
			}
		}

		private void ctrlHexBox_SelectionStartChanged(object sender, EventArgs e)
		{
			UpdateLocationLabel();
		}

		private void ctrlHexBox_SelectionLengthChanged(object sender, EventArgs e)
		{
			UpdateLocationLabel();
		}

		private AddressType? GetAddressType()
		{
			switch(_memoryType) {
				case DebugMemoryType.InternalRam: return AddressType.InternalRam;
				case DebugMemoryType.WorkRam: return AddressType.WorkRam;
				case DebugMemoryType.SaveRam: return AddressType.SaveRam;
				case DebugMemoryType.PrgRom: return AddressType.PrgRom;
			}

			return null;
		}

		private int SelectionStartAddress { get { return (int)ctrlHexBox.SelectionStart; } }
		private int SelectionEndAddress { get { return (int)(ctrlHexBox.SelectionStart + (ctrlHexBox.SelectionLength == 0 ? 0 : (ctrlHexBox.SelectionLength - 1))); } }

		private void MarkSelectionAs(int start, int end, CdlPrgFlags type)
		{
			if(_memoryType == DebugMemoryType.CpuMemory) {
				start = InteropEmu.DebugGetAbsoluteAddress((UInt32)start);
				end = InteropEmu.DebugGetAbsoluteAddress((UInt32)end);
			}

			if(start >= 0 && end >= 0 && start <= end) {
				InteropEmu.DebugMarkPrgBytesAs((UInt32)start, (UInt32)end, type);
				frmDebugger debugger = DebugWindowManager.GetDebugger();
				if(debugger != null) {
					debugger.UpdateDebugger(false, false);
				}
			}
		}

		private void mnuAddToWatch_Click(object sender, EventArgs e)
		{
			string[] toAdd = Enumerable.Range(SelectionStartAddress, SelectionEndAddress - SelectionStartAddress + 1).Select((num) => $"[${num.ToString("X4")}]").ToArray();
			WatchManager.AddWatch(toAdd);
		}

		private void mnuEditBreakpoint_Click(object sender, EventArgs e)
		{
			UInt32 startAddress = (UInt32)SelectionStartAddress;
			UInt32 endAddress = (UInt32)SelectionEndAddress;
			BreakpointAddressType addressType = startAddress == endAddress ? BreakpointAddressType.SingleAddress : BreakpointAddressType.AddressRange;

			Breakpoint bp = BreakpointManager.GetMatchingBreakpoint(startAddress, endAddress, this._memoryType);
			if(bp == null) {
				bp = new Breakpoint() { Address = startAddress, MemoryType = this._memoryType, StartAddress = startAddress, EndAddress = endAddress, AddressType = addressType, BreakOnWrite = true, BreakOnRead = true };
				if(bp.IsCpuBreakpoint) {
					bp.BreakOnExec = true;
				}
			}
			BreakpointManager.EditBreakpoint(bp);
		}

		private void mnuEditLabel_Click(object sender, EventArgs e)
		{
			UInt32 address = (UInt32)ctrlHexBox.SelectionStart;
			if(this._memoryType == DebugMemoryType.CpuMemory) {
				AddressTypeInfo info = new AddressTypeInfo();
				InteropEmu.DebugGetAbsoluteAddressAndType(address, ref info);
				ctrlLabelList.EditLabel((UInt32)info.Address, info.Type);
			} else {
				ctrlLabelList.EditLabel(address, GetAddressType().Value);
			}
		}

		private void mnuFreeze_Click(object sender, EventArgs e)
		{
			for(int i = SelectionStartAddress, end = SelectionEndAddress; i <= end; i++) {
				InteropEmu.DebugSetFreezeState((UInt16)i, true);
			}
			this.ctrlHexBox.Invalidate();
		}

		private void mnuUnfreeze_Click(object sender, EventArgs e)
		{
			for(int i = SelectionStartAddress, end = SelectionEndAddress; i <= end; i++) {
				InteropEmu.DebugSetFreezeState((UInt16)i, false);
			}
			this.ctrlHexBox.Invalidate();
		}

		private void mnuMarkAsCode_Click(object sender, EventArgs e)
		{
			this.MarkSelectionAs(SelectionStartAddress, SelectionEndAddress, CdlPrgFlags.Code);
		}

		private void mnuMarkAsData_Click(object sender, EventArgs e)
		{
			this.MarkSelectionAs(SelectionStartAddress, SelectionEndAddress, CdlPrgFlags.Data);
		}

		private void mnuMarkAsUnidentifiedData_Click(object sender, EventArgs e)
		{
			this.MarkSelectionAs(SelectionStartAddress, SelectionEndAddress, CdlPrgFlags.None);
		}
		
		private void UpdateActionAvailability()
		{
			UInt32 startAddress = (UInt32)SelectionStartAddress;
			UInt32 endAddress = (UInt32)SelectionEndAddress;

			string address = "$" + startAddress.ToString("X4");
			string addressRange;
			if(startAddress != endAddress) {
				addressRange = "$" + startAddress.ToString("X4") + "-$" + endAddress.ToString("X4");
			} else {
				addressRange = address;
			}

			mnuEditLabel.Text = $"Edit Label ({address})";
			mnuEditBreakpoint.Text = $"Edit Breakpoint ({addressRange})";
			mnuAddToWatch.Text = $"Add to Watch ({addressRange})";

			if(this._memoryType == DebugMemoryType.CpuMemory) {
				bool[] freezeState = InteropEmu.DebugGetFreezeState((UInt16)startAddress, (UInt16)(endAddress - startAddress + 1));
				mnuFreeze.Enabled = !freezeState.All((frozen) => frozen);
				mnuUnfreeze.Enabled = freezeState.Any((frozen) => frozen);
				mnuFreeze.Text = $"Freeze ({addressRange})";
				mnuUnfreeze.Text = $"Unfreeze ({addressRange})";
			} else {
				mnuFreeze.Text = $"Freeze";
				mnuUnfreeze.Text = $"Unfreeze";
				mnuFreeze.Enabled = false;
				mnuUnfreeze.Enabled = false;
			}

			if(this._memoryType == DebugMemoryType.CpuMemory) {
				int absStart = InteropEmu.DebugGetAbsoluteAddress(startAddress);
				int absEnd = InteropEmu.DebugGetAbsoluteAddress(endAddress);

				if(absStart >= 0 && absEnd >= 0 && absStart <= absEnd) {
					mnuMarkSelectionAs.Text = "Mark selection as... (" + addressRange + ")";
					mnuMarkSelectionAs.Enabled = true;
				} else {
					mnuMarkSelectionAs.Text = "Mark selection as...";
					mnuMarkSelectionAs.Enabled = false;
				}
			} else if(this._memoryType == DebugMemoryType.PrgRom) {
				mnuMarkSelectionAs.Text = "Mark selection as... (" + addressRange + ")";
				mnuMarkSelectionAs.Enabled = true;
			} else {
				mnuMarkSelectionAs.Text = "Mark selection as...";
				mnuMarkSelectionAs.Enabled = false;
			}

			bool disableEditLabel = false;
			if(this._memoryType == DebugMemoryType.CpuMemory) {
				AddressTypeInfo info = new AddressTypeInfo();
				InteropEmu.DebugGetAbsoluteAddressAndType(startAddress, ref info);
				disableEditLabel = info.Address == -1;
			}

			mnuEditLabel.Enabled = !disableEditLabel && (this._memoryType == DebugMemoryType.CpuMemory || this.GetAddressType().HasValue);
			mnuEditBreakpoint.Enabled = DebugWindowManager.GetDebugger() != null && (
				this._memoryType == DebugMemoryType.CpuMemory ||
				this._memoryType == DebugMemoryType.PpuMemory ||
				this._memoryType == DebugMemoryType.PrgRom ||
				this._memoryType == DebugMemoryType.WorkRam ||
				this._memoryType == DebugMemoryType.SaveRam ||
				this._memoryType == DebugMemoryType.ChrRam ||
				this._memoryType == DebugMemoryType.ChrRom ||
				this._memoryType == DebugMemoryType.PaletteMemory
			);

			mnuAddToWatch.Enabled = this._memoryType == DebugMemoryType.CpuMemory;

			mnuCopy.Enabled = ctrlHexBox.CanCopy();
			mnuPaste.Enabled = ctrlHexBox.CanPaste();
			mnuSelectAll.Enabled = ctrlHexBox.CanSelectAll();
			mnuUndo.Enabled = InteropEmu.DebugHasUndoHistory();
		}

		private void ctxMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			this.UpdateActionAvailability();
		}

		private void mnuCopy_Click(object sender, EventArgs e)
		{
			ctrlHexBox.CopyHex();
		}

		private void mnuPaste_Click(object sender, EventArgs e)
		{
			ctrlHexBox.Paste();
		}

		private void mnuSelectAll_Click(object sender, EventArgs e)
		{
			ctrlHexBox.SelectAll();
		}

		private void mnuUndo_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugPerformUndo();
			this.RefreshData(_memoryType);
		}
	}
}
