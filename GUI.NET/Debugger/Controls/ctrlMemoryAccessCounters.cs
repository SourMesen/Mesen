using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;
using Mesen.GUI.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlMemoryAccessCounters : BaseControl
	{
		private MemoryCountData[] _data;
		private DebugMemoryType _memoryType = DebugMemoryType.InternalRam;
		private SortType _sortType = SortType.Address;

		public ctrlMemoryAccessCounters()
		{
			InitializeComponent();
			
			this.toolTip.SetToolTip(chkHighlightUninitRead, "The uninitialized memory reads highlight will only be accurate if the debugger was active when the game was loaded (or if the game has been power cycled since)");
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(!IsDesignMode) {
				InitMemoryTypeDropdown();
				cboSort.SelectedIndex = 0;
				InitShortcuts();
			}
		}

		private void InitShortcuts()
		{
			mnuCopy.InitShortcut(this, nameof(DebuggerShortcutsConfig.Copy));
			mnuSelectAll.InitShortcut(this, nameof(DebuggerShortcutsConfig.SelectAll));
		}

		public void InitMemoryTypeDropdown()
		{
			cboMemoryType.SelectedIndexChanged -= cboMemoryType_SelectedIndexChanged;

			DebugMemoryType originalValue = cboMemoryType.GetEnumValue<DebugMemoryType>();

			cboMemoryType.BeginUpdate();
			cboMemoryType.Items.Clear();

			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.InternalRam));
			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.PrgRom) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.PrgRom));
			}
			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.WorkRam) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.WorkRam));
			}
			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.SaveRam) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.SaveRam));
			}

			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRom) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.ChrRom));
			}
			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRam) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.ChrRam));
			}
			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.PaletteMemory));
			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.NametableRam));

			cboMemoryType.SelectedIndex = 0;
			cboMemoryType.SetEnumValue(originalValue);
			cboMemoryType.SelectedIndexChanged += cboMemoryType_SelectedIndexChanged;
			cboMemoryType.EndUpdate();

			UpdateMemoryType();
		}

		public Font BaseFont { get { return ctrlScrollableTextbox.BaseFont; } set { ctrlScrollableTextbox.BaseFont = value; } }
		public int TextZoom { get { return ctrlScrollableTextbox.TextZoom; } set { ctrlScrollableTextbox.TextZoom = value; } }

		public void RefreshData()
		{
			bool isPpu = (
				_memoryType == DebugMemoryType.ChrRom ||
				_memoryType == DebugMemoryType.ChrRam ||
				_memoryType == DebugMemoryType.PaletteMemory ||
				_memoryType == DebugMemoryType.NametableRam
			);

			int[] readCounts = InteropEmu.DebugGetMemoryAccessCounts(_memoryType, MemoryOperationType.Read);
			int[] writeCounts = InteropEmu.DebugGetMemoryAccessCounts(_memoryType, MemoryOperationType.Write);
			int[] execCounts = isPpu ? new int[readCounts.Length] : InteropEmu.DebugGetMemoryAccessCounts(_memoryType, MemoryOperationType.Exec);

			int[] uninitReads = isPpu ? new int[readCounts.Length] : InteropEmu.DebugGetUninitMemoryReads(_memoryType);

			List<int> addresses = new List<int>(readCounts.Length);
			List<string> content = new List<string>(readCounts.Length);

			if(_data == null || _data.Length != readCounts.Length) {
				_data = new MemoryCountData[readCounts.Length];
				for(int i = 0; i < readCounts.Length; i++) {
					_data[i] = new MemoryCountData();
				}
			}

			for(int i = 0; i < readCounts.Length; i++) {
				_data[i].Address = i;
				_data[i].ReadCount = readCounts[i];
				_data[i].WriteCount = writeCounts[i];
				_data[i].ExecCount = execCounts[i];
				_data[i].UninitRead = uninitReads[i] > 0;
				_data[i].IsPpu = isPpu;
			}

			MemoryCountData[] data = new MemoryCountData[readCounts.Length];
			Array.Copy(_data, data, readCounts.Length);

			switch(_sortType) {
				case SortType.Address: break;
				case SortType.Read: Array.Sort(data.Select((e) => -e.ReadCount).ToArray<int>(), data); break;
				case SortType.Write: Array.Sort(data.Select((e) => -e.WriteCount).ToArray<int>(), data); break;
				case SortType.Exec: Array.Sort(data.Select((e) => -e.ExecCount).ToArray<int>(), data); break;
				case SortType.UninitRead: Array.Sort(data.Select((e) => e.UninitRead ? -e.ReadCount : (Int32.MaxValue - e.ReadCount)).ToArray<int>(), data); break;
			}

			bool hideUnusedAddresses = chkHideUnusedAddresses.Checked;
			for(int i = 0; i < readCounts.Length; i++) {
				if(!hideUnusedAddresses || !data[i].Empty) {
					addresses.Add(data[i].Address);
					content.Add(data[i].Content);
				}
			}

			if(chkHighlightUninitRead.Checked) {
				ctrlScrollableTextbox.StyleProvider = new LineStyleProvider(new HashSet<int>(data.Where((e) => e.UninitRead).Select((e) => e.Address)));
			} else {
				ctrlScrollableTextbox.StyleProvider = null;
			}
			if(isPpu) {
				ctrlScrollableTextbox.Header = " " + "Read".PadRight(12) + "Write";
			} else {
				ctrlScrollableTextbox.Header = " " + "Read".PadRight(12) + "Write".PadRight(12) + "Execute";
			}
			ctrlScrollableTextbox.LineNumbers = addresses.ToArray();
			ctrlScrollableTextbox.TextLines = content.ToArray();
		}

		private class LineStyleProvider : ctrlTextbox.ILineStyleProvider
		{
			HashSet<int> _addresses = new HashSet<int>();

			public LineStyleProvider(HashSet<int> addresses)
			{
				_addresses = addresses;
			}

			public string GetLineComment(int lineIndex)
			{
				return null;
			}

			public LineProperties GetLineStyle(int cpuAddress, int lineIndex)
			{
				if(_addresses.Contains(cpuAddress)) {
					return new LineProperties() { TextBgColor = Color.LightCoral };
				}
				return null;
			}
		}

		private void cboMemoryType_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateMemoryType();
		}

		private void UpdateMemoryType()
		{
			_memoryType = cboMemoryType.GetEnumValue<DebugMemoryType>();
			RefreshData();
		}

		private void cboSort_SelectedIndexChanged(object sender, EventArgs e)
		{
			_sortType = (SortType)cboSort.SelectedIndex;
			RefreshData();
		}
		
		private void chkOption_CheckedChanged(object sender, EventArgs e)
		{
			RefreshData();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugResetMemoryAccessCounts();
			RefreshData();
		}

		public void GoToAddress()
		{
			ctrlScrollableTextbox.GoToAddress();
		}

		private enum SortType
		{
			Address = 0,
			Read = 1,
			Write = 2,
			Exec = 3,
			UninitRead = 4,
		}

		private class MemoryCountData
		{
			private bool _needRecalc = true;
			private int _readCount = 0;
			private int _writeCount = 0;
			private int _execCount = 0;
			private string _content = string.Empty;

			public int Address { get; set; }

			public int ReadCount
			{
				get { return _readCount; }
				set
				{
					if(this._readCount!=value) {
						this._readCount = value;
						this._needRecalc = true;
					};
				}
			}

			public int WriteCount
			{
				get { return _writeCount; }
				set
				{
					if(this._writeCount!=value) {
						this._writeCount = value;
						this._needRecalc = true;
					};
				}
			}

			public int ExecCount
			{
				get { return _execCount; }
				set
				{
					if(this._execCount!=value) {
						this._execCount = value;
						this._needRecalc = true;
					};
				}
			}

			public bool Empty { get { return ReadCount == 0 && WriteCount == 0 && ExecCount == 0; } }

			public string Content
			{
				get
				{
					if(this._needRecalc) {
						_content = " " + (_readCount == 0 ? "0" : _readCount.ToString()).PadRight(12) +
									(_writeCount == 0 ? "0" : _writeCount.ToString()).PadRight(12) +
									(IsPpu ? "" : (_execCount == 0 ? "0" : _execCount.ToString()));
						_needRecalc = false;
					}
					return _content;
				}
			}

			public bool UninitRead { get; set; }
			public bool IsPpu { get; set; }
		}

		private void mnuCopy_Click(object sender, EventArgs e)
		{
			ctrlScrollableTextbox.CopySelection(true, false, false);
		}

		private void mnuSelectAll_Click(object sender, EventArgs e)
		{
			ctrlScrollableTextbox.SelectAll();
		}
	}
}
