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
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Cheats
{
	public partial class ctrlCheatFinder : BaseControl
	{
		public event EventHandler OnAddCheat;
		public bool TabIsFocused
		{
			set
			{
				_tabIsFocused = value;
				if(chkPauseGameWhileWindowActive.Checked) {
					if(_tabIsFocused) {
						InteropEmu.Pause();
					} else {
						InteropEmu.Resume();
					}
				}
			}
		}

		private List<byte[]> _memorySnapshots;
		private List<FilterInfo> _filters;
		private bool _tabIsFocused = false;
		
		private enum CheatPrevFilterType
		{
			Smaller,
			Equal,
			NotEqual,
			Greater
		}

		private enum CheatCurrentFilterType
		{
			Smaller,
			Equal,
			NotEqual,
			Greater
		}

		private class FilterInfo
		{
			public CheatCurrentFilterType? CurrentType;
			public CheatPrevFilterType? PrevType;
			public int Operand;
		}

		public ctrlCheatFinder()
		{
			InitializeComponent();

			BaseConfigForm.InitializeComboBox(cboPrevFilterType, typeof(CheatPrevFilterType));
			BaseConfigForm.InitializeComboBox(cboCurrentFilterType, typeof(CheatCurrentFilterType));
			cboPrevFilterType.SelectedIndex = 0;
			cboCurrentFilterType.SelectedIndex = 0;

			btnUndo.Enabled = false;

			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime) {
				Reset();
				tmrRefresh.Start();
			}
		}
				
		private void Reset()
		{
			_filters = new List<FilterInfo>();
			_memorySnapshots = new List<byte[]>();
			TakeSnapshot();
		}

		private void TakeSnapshot()
		{
			if(!InteropEmu.IsRunning()) {
				return;
			}

			byte[] memory = GetSnapshot();
			_memorySnapshots.Add(memory);
			RefreshAddressList();

			UpdateUI();
		}

		private byte[] GetSnapshot()
		{
			bool release = !InteropEmu.DebugIsDebuggerRunning();
			byte[] memory = InteropEmu.DebugGetInternalRam();
			if(release) {
				InteropEmu.DebugRelease();
			}

			return memory;
		}

		private void tmrRefresh_Tick(object sender, EventArgs e)
		{
			if(_tabIsFocused) {
				RefreshAddressList();
			}
		}

		private void RefreshAddressList()
		{
			UpdateUI();
			if(!InteropEmu.IsRunning()) {
				return;
			}

			HashSet<int> matchingAddresses = new HashSet<int>();
			for(int i = 0; i < 0x800; i++) {
				matchingAddresses.Add(i);
			}

			if(_memorySnapshots.Count > 1) {
				for(int i = 0; i < _memorySnapshots.Count - 1; i++) {
					matchingAddresses = new HashSet<int>(matchingAddresses.Where(addr => {
						if(_filters[i].PrevType.HasValue) {
							switch(_filters[i].PrevType) {
								case CheatPrevFilterType.Smaller: return _memorySnapshots[i+1][addr] < _memorySnapshots[i][addr];
								case CheatPrevFilterType.Equal: return _memorySnapshots[i+1][addr] == _memorySnapshots[i][addr];
								case CheatPrevFilterType.NotEqual: return _memorySnapshots[i+1][addr] != _memorySnapshots[i][addr];
								case CheatPrevFilterType.Greater: return _memorySnapshots[i+1][addr] > _memorySnapshots[i][addr];
							}
						} else {
							switch(_filters[i].CurrentType) {
								case CheatCurrentFilterType.Smaller: return _memorySnapshots[i+1][addr] < _filters[i].Operand;
								case CheatCurrentFilterType.Equal: return _memorySnapshots[i+1][addr] == _filters[i].Operand;
								case CheatCurrentFilterType.NotEqual: return _memorySnapshots[i+1][addr] != _filters[i].Operand;
								case CheatCurrentFilterType.Greater: return _memorySnapshots[i+1][addr] > _filters[i].Operand;
							}
						}
						return false;
					}));
				}
			}

			byte[] memory = GetSnapshot();
			List<byte> values = new List<byte>(0x800);
			List<int> addresses = new List<int>(0x800);
			for(int i = 0; i < 0x800; i++) {
				if(matchingAddresses.Contains(i)) {
					addresses.Add(i);
					values.Add(memory[i]);
				}
			}
			lstAddresses.SetData(values.ToArray(), addresses.ToArray());
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			Reset();
		}

		private void btnUndo_Click(object sender, EventArgs e)
		{
			if(_filters.Count > 0) {
				_filters.RemoveAt(_filters.Count-1);
				_memorySnapshots.RemoveAt(_memorySnapshots.Count-1);
				UpdateUI();
			}
		}

		private void UpdateUI()
		{
			btnUndo.Enabled = _filters.Count > 0;
			chkPauseGameWhileWindowActive.Enabled = btnAddCurrentFilter.Enabled = btnAddPrevFilter.Enabled = btnReset.Enabled = cboCurrentFilterType.Enabled = cboPrevFilterType.Enabled = nudCurrentFilterValue.Enabled = InteropEmu.IsRunning();
			mnuCreateCheat.Enabled = btnCreateCheat.Enabled = lstAddresses.CurrentAddress.HasValue;

			if(lstAddresses.CurrentAddress.HasValue) {
				lblAddress.Visible = true;
				lblAtAddress.Visible = true;
				lblAddress.Text = "$" + lstAddresses.CurrentAddress?.ToString("X4");
			} else {
				lblAddress.Visible = false;
				lblAtAddress.Visible = false;
			}
		}

		private void btnAddPrevFilter_Click(object sender, EventArgs e)
		{
			_filters.Add(new FilterInfo { PrevType = (CheatPrevFilterType)cboPrevFilterType.SelectedIndex });
			TakeSnapshot();
		}

		private void btnAddCurrentFilter_Click(object sender, EventArgs e)
		{
			_filters.Add(new FilterInfo { CurrentType = (CheatCurrentFilterType)cboCurrentFilterType.SelectedIndex, Operand = (int)nudCurrentFilterValue.Value });
			TakeSnapshot();
		}

		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			UpdateUI();
		}

		private void btnCreateCheat_Click(object sender, EventArgs e)
		{
			RomInfo romInfo = InteropEmu.GetRomInfo();
			CheatInfo newCheat = new CheatInfo {
				GameCrc = romInfo.GetPrgCrcString(),
				GameName = romInfo.GetRomName(),
				Address = (uint)lstAddresses.CurrentAddress,
				CheatType = CheatType.Custom
			};

			frmCheat frm = new frmCheat(newCheat);
			if(frm.ShowDialog() == DialogResult.OK) {
				OnAddCheat?.Invoke(newCheat, new EventArgs());
			}
		}

		private void chkPauseGameWhileWindowActive_CheckedChanged(object sender, EventArgs e)
		{
			if(chkPauseGameWhileWindowActive.Checked) {
				InteropEmu.Pause();
			} else {
				InteropEmu.Resume();
			}
		}
	}
}
