using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Mesen.GUI.Controls;
using Mesen.GUI.Forms;
using Mesen.GUI.Config;
using System.Collections.ObjectModel;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlEventViewerListView : BaseControl
	{
		private List<DebugEventInfo> _debugEvents = new List<DebugEventInfo>();
		private ReadOnlyCollection<Breakpoint> _breakpoints = null;
		private eSortColumn _sortColumn = eSortColumn.Scanline;
		private bool _sortAscending = true;

		public ctrlEventViewerListView()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(!IsDesignMode) {
				mnuCopy.InitShortcut(this, nameof(DebuggerShortcutsConfig.Copy));
			}
		}

		public void GetData()
		{
			DebugEventInfo[] eventInfoArray;
			byte[] pictureData;
			_breakpoints = BreakpointManager.Breakpoints;
			InteropEmu.DebugGetDebugEvents(false, out pictureData, out eventInfoArray);

			this.BeginInvoke((Action)(() => {
				lstEvents.BeginUpdate();
				_debugEvents.Clear();
				_debugEvents.AddRange(eventInfoArray);
				SortData();
				lstEvents.VirtualListSize = _debugEvents.Count;
				lstEvents.EndUpdate();
			}));
		}

		private ListViewItem CreateListViewItem(int index)
		{
			DebugEventInfo eventInfo = _debugEvents[index];

			string details = "";
			if(eventInfo.PpuLatch >= 1) {
				details += "2nd Write";
			}

			bool isReadWrite = (
				eventInfo.Type == DebugEventType.MapperRegisterRead ||
				eventInfo.Type == DebugEventType.MapperRegisterWrite ||
				eventInfo.Type == DebugEventType.PpuRegisterRead ||
				eventInfo.Type == DebugEventType.PpuRegisterWrite
			);

			if(eventInfo.Type == DebugEventType.Breakpoint) {
				if(eventInfo.BreakpointId >= 0 && eventInfo.BreakpointId < _breakpoints.Count) {
					Breakpoint bp = _breakpoints[eventInfo.BreakpointId];
					details += " Type: " + bp.ToReadableType();
					details += " Addresses: " + bp.GetAddressString(true);
					if(bp.Condition.Length > 0) {
						details += " Condition: " + bp.Condition;
					}
				}
			}

			return new ListViewItem(new string[] {
				eventInfo.ProgramCounter.ToString("X4"),
				eventInfo.Scanline.ToString(),
				eventInfo.Cycle.ToString(),
				ResourceHelper.GetEnumText(eventInfo.Type),
				isReadWrite ? ("$" + eventInfo.Address.ToString("X4")) : "",
				isReadWrite ? ("$" + eventInfo.Value.ToString("X2")) : "",
				details
			});
		}

		private void lstEvents_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			e.Item = CreateListViewItem(e.ItemIndex);	
		}

		private void lstEvents_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			eSortColumn sortColumn = (eSortColumn)e.Column;
			if(sortColumn == _sortColumn) {
				_sortAscending = !_sortAscending;
			} else {
				_sortColumn = sortColumn;
				_sortAscending = true;
			}

			lstEvents.BeginUpdate();
			SortData();
			lstEvents.VirtualListSize = _debugEvents.Count;
			lstEvents.EndUpdate();
		}

		private void SortData()
		{
			_debugEvents.Sort((DebugEventInfo a, DebugEventInfo b) => {
				int result = 0;
				switch(_sortColumn) {
					case eSortColumn.PC: result = ((int)a.ProgramCounter - (int)b.ProgramCounter); break;
					case eSortColumn.Scanline: result = ((int)a.Scanline - (int)b.Scanline); break;
					case eSortColumn.Cycle: result = ((int)a.Cycle - (int)b.Cycle); break;
					case eSortColumn.Type: result = ((int)a.Type - (int)b.Type); break;
					case eSortColumn.Address: result = ((int)a.Address - (int)b.Address); break;
					case eSortColumn.Value: result = ((int)a.Value - (int)b.Value); break;
				}

				if(result == 0) {
					result = ((int)a.Cycle - (int)b.Cycle);
				}
				return _sortAscending ? result : -result;
			});
		}

		private void CopyList()
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < _debugEvents.Count; i++) {
				foreach(ListViewItem.ListViewSubItem subItem in CreateListViewItem(i).SubItems) {
					sb.Append(subItem.Text);
					sb.Append("\t");
				}
				sb.AppendLine();
			}
			Clipboard.SetText(sb.ToString());
		}
		
		private void mnuCopy_Click(object sender, EventArgs e)
		{
			CopyList();
		}
		
		private enum eSortColumn
		{
			PC,
			Scanline,
			Cycle,
			Type,
			Address,
			Value
		}
	}
}
