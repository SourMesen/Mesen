using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlProfiler : BaseControl
	{
		public static event EventHandler OnFunctionSelected;
		private Int64[] _exclusiveTime;
		private Int64[] _inclusiveTime;
		private Int64[] _callCount;

		private int _sortColumn = 5;
		private bool _sortOrder = true;

		public ctrlProfiler()
		{
			InitializeComponent();

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				lstFunctions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			}
		}

		public void RefreshData()
		{
			_exclusiveTime = InteropEmu.DebugGetProfilerData(ProfilerDataType.FunctionExclusive);
			_inclusiveTime = InteropEmu.DebugGetProfilerData(ProfilerDataType.FunctionInclusive);
			_callCount = InteropEmu.DebugGetProfilerData(ProfilerDataType.FunctionCallCount);

			RefreshList();
		}

		private int GetMaxAddrHexSize()
		{
			int size = _exclusiveTime.Length - 2;
			int bitCount = 0;
			int hexCount = 1;
			while(size > 0) {
				size /= 2;
				if(bitCount == 4) {
					hexCount++;
				}
			}

			return hexCount;
		}

		private void RefreshList()
		{
			Int64 exclusiveTotal = _exclusiveTime.Sum();

			int hexCount = GetMaxAddrHexSize();

			lstFunctions.BeginUpdate();
			lstFunctions.ListViewItemSorter = null;
			lstFunctions.Items.Clear();			
			for(UInt32 i = 0; i < _exclusiveTime.Length; i++) {
				if(_exclusiveTime[i] > 0) {
					string functionName;

					if(i == _exclusiveTime.Length - 2) {
						functionName = "[Reset]";
					} else if(i == _exclusiveTime.Length - 1) {
						functionName = "[In-Memory Function]";
					} else {
						CodeLabel label = LabelManager.GetLabel((UInt32)i, AddressType.PrgRom);
						functionName = "$" + i.ToString("X" + hexCount.ToString());
						if(label != null) {
							functionName = label.Label + " (" + functionName + ")";
						}
					}

					ListViewItem item = lstFunctions.Items.Add(functionName);
					item.Tag = i;

					item.SubItems.Add(_callCount[i].ToString());
					item.SubItems[1].Tag = _callCount[i];

					item.SubItems.Add(_inclusiveTime[i].ToString());
					item.SubItems[2].Tag = _inclusiveTime[i];

					double ratio = ((double)_inclusiveTime[i] / exclusiveTotal)*100;
					item.SubItems.Add(ratio.ToString("0.00"));
					item.SubItems[3].Tag = (Int64)(ratio*100);

					item.SubItems.Add(_exclusiveTime[i].ToString());
					item.SubItems[4].Tag = _exclusiveTime[i];

					ratio = ((double)_exclusiveTime[i] / exclusiveTotal)*100;
					item.SubItems.Add(ratio.ToString("0.00"));
					item.SubItems[5].Tag = (Int64)(ratio*100);
				}
			}
			lstFunctions.ListViewItemSorter = new ListComparer(_sortColumn, _sortOrder);
			lstFunctions.EndUpdate();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugResetProfiler();
			RefreshData();
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			this.RefreshData();
		}

		private void lstFunctions_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if(_sortColumn == e.Column) {
				_sortOrder = !_sortOrder;
			} else {
				_sortColumn = e.Column;
				_sortOrder = e.Column == 0 ? false : true;
			}

			RefreshList();
		}
		
		private void lstFunctions_DoubleClick(object sender, EventArgs e)
		{
			if(lstFunctions.SelectedItems.Count > 0) {
				OnFunctionSelected?.Invoke(lstFunctions.SelectedItems[0].Tag, EventArgs.Empty);
			}
		}

		private class ListComparer : IComparer
		{
			private int _columnIndex;
			private bool _sortOrder;

			public ListComparer(int columnIndex, bool sortOrder)
			{
				_columnIndex = columnIndex;
				_sortOrder = sortOrder;
			}

			public int Compare(object x, object y)
			{
				if(_columnIndex == 0) {
					if(_sortOrder) {
						return String.Compare(((ListViewItem)y).SubItems[0].Text, ((ListViewItem)x).SubItems[0].Text);
					} else {
						return String.Compare(((ListViewItem)x).SubItems[0].Text, ((ListViewItem)y).SubItems[0].Text);
					}
				} else {
					if(_sortOrder) {
						return (Int64)((ListViewItem)y).SubItems[_columnIndex].Tag > (Int64)((ListViewItem)x).SubItems[_columnIndex].Tag ? 1 : -1;
					} else {
						return (Int64)((ListViewItem)y).SubItems[_columnIndex].Tag > (Int64)((ListViewItem)x).SubItems[_columnIndex].Tag ? -1 : 1;
					}
				}
			}
		}
	}
}
