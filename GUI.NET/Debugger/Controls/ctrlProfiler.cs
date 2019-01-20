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
		private object _resetLock = new object();

		private int _sortColumn = 5;
		private bool _sortOrder = true;

		public ctrlProfiler()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				lstFunctions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
				int newWidth = Math.Max(colFunction.Width * 2, 250);
				colExclusiveTimePercent.Width -= (newWidth - colFunction.Width) + 30;
				colFunction.Width = newWidth;
			}
		}

		public void RefreshData()
		{
			lock(_resetLock) {
				_exclusiveTime = InteropEmu.DebugGetProfilerData(ProfilerDataType.FunctionExclusive);
				_inclusiveTime = InteropEmu.DebugGetProfilerData(ProfilerDataType.FunctionInclusive);
				_callCount = InteropEmu.DebugGetProfilerData(ProfilerDataType.FunctionCallCount);
			}
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

			int? topItemIndex = lstFunctions.TopItem?.Index;
			int selectedIndex = lstFunctions.SelectedIndices.Count > 0 ? lstFunctions.SelectedIndices[0] : -1;

			int itemNumber = 0;
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

					ListViewItem item;
					if(itemNumber >= lstFunctions.Items.Count) {
						item = lstFunctions.Items.Add("");
						item.SubItems.Add("");
						item.SubItems.Add("");
						item.SubItems.Add("");
						item.SubItems.Add("");
						item.SubItems.Add("");
					} else {
						item = lstFunctions.Items[itemNumber];
					}

					item.Text = functionName;

					item.Tag = i;
					item.Selected = false;
					item.Focused = false;

					item.SubItems[1].Text = _callCount[i].ToString();
					item.SubItems[1].Tag = _callCount[i];

					item.SubItems[2].Text = _inclusiveTime[i].ToString();
					item.SubItems[2].Tag = _inclusiveTime[i];

					double ratio = ((double)_inclusiveTime[i] / exclusiveTotal) *100;
					item.SubItems[3].Text = ratio.ToString("0.00");
					item.SubItems[3].Tag = (Int64)(ratio*100);

					item.SubItems[4].Text = _exclusiveTime[i].ToString();
					item.SubItems[4].Tag = _exclusiveTime[i];

					ratio = ((double)_exclusiveTime[i] / exclusiveTotal)*100;
					item.SubItems[5].Text = ratio.ToString("0.00");
					item.SubItems[5].Tag = (Int64)(ratio*100);

					itemNumber++;
				}
			}

			lstFunctions.ListViewItemSorter = new ListComparer(_sortColumn, _sortOrder);
			lstFunctions.EndUpdate();

			if(topItemIndex.HasValue) {
				lstFunctions.TopItem = lstFunctions.Items[topItemIndex.Value];
			}

			if(selectedIndex >= 0) {
				lstFunctions.Items[selectedIndex].Selected = true;
				lstFunctions.Items[selectedIndex].Focused = true;
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			lock(_resetLock) {
				InteropEmu.DebugResetProfiler();
			}
			lstFunctions.Items.Clear();
			RefreshData();
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
					Int64 columnValueY = (Int64)((ListViewItem)y).SubItems[_columnIndex].Tag;
					Int64 columnValueX = (Int64)((ListViewItem)x).SubItems[_columnIndex].Tag;
					if(_sortOrder) {
						return (int)(columnValueY - columnValueX);
					} else {
						return (int)(columnValueX - columnValueY);
					}
				}
			}
		}
	}
}
