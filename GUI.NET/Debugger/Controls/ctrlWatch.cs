using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class ctrlWatch : UserControl
	{
		public ctrlWatch()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			AdjustColumnWidth();
		}

		private void lstWatch_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			if(e.ColumnIndex == 2) {
				e.Cancel = true;
			}
			AdjustColumnWidth();
		}

		void lstWatch_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			AdjustColumnWidth();
		}

		private void AdjustColumnWidth()
		{
			lstWatch.ColumnWidthChanging -= lstWatch_ColumnWidthChanging;
			lstWatch.ColumnWidthChanged -= lstWatch_ColumnWidthChanged;

			//Force watch values to take the full width of the list
			int totalWidth = lstWatch.Columns[0].Width + lstWatch.Columns[1].Width;
			lstWatch.Columns[2].Width = lstWatch.ClientSize.Width - totalWidth;

			lstWatch.ColumnWidthChanging += lstWatch_ColumnWidthChanging;
			lstWatch.ColumnWidthChanged += lstWatch_ColumnWidthChanged;
		}

		public void UpdateWatch()
		{
			lstWatch.SelectedIndices.Clear();

			//Remove empty items
			for(int i = lstWatch.Items.Count - 1; i >= 0; i--) {
				if(string.IsNullOrWhiteSpace(lstWatch.Items[i].Text)) {
					lstWatch.Items.RemoveAt(i);
				}
			}
			lstWatch.Items.Add("");

			ListViewItem lastItem = lstWatch.Items[lstWatch.Items.Count - 1];
			foreach(ListViewItem item in lstWatch.Items) {
				item.UseItemStyleForSubItems = false;
				if(item != lastItem) {
					string previousValue = null;
					if(item.SubItems.Count > 1) {
						previousValue = item.SubItems[1].Text;
						item.SubItems.RemoveAt(1);
					}
					item.SubItems[0].Text = item.SubItems[0].Text.ToUpper();

					string newValue;
					Byte memoryValue = InteropEmu.DebugGetMemoryValue((UInt32)item.Tag);
					if(mnuHexDisplay.Checked) {
						newValue = "$" + memoryValue.ToString("X");
					} else {
						newValue = memoryValue.ToString();
					}
					item.SubItems.Add(newValue);
					item.SubItems[1].ForeColor = newValue != previousValue ? Color.Red : Color.Black;
				}
			}
			AdjustColumnWidth();
		}

		public void AddWatch(UInt32 address)
		{
			ListViewItem item = lstWatch.Items.Insert(lstWatch.Items.Count - 1, "$" + address.ToString("x"));
			item.Tag = address;
			UpdateWatch();
		}

		private void lstWatch_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			e.CancelEdit = true;

			if(e.Label != null) {
				ListViewItem item = lstWatch.Items[e.Item];
				item.Text = e.Label;
				if(!string.IsNullOrWhiteSpace(item.Text)) {
					if(!item.Text.StartsWith("$")) {
						item.Text = "$" + item.Text;
					}

					UInt32 address;
					if(UInt32.TryParse(item.Text.Substring(1), System.Globalization.NumberStyles.AllowHexSpecifier, null, out address)) {
						lstWatch.Items[e.Item].Tag = address;
					} else {
						item.Text = string.Empty;
					}
				}
				UpdateWatch();
			}
		}

		private void mnuRemoveWatch_Click(object sender, EventArgs e)
		{
			if(lstWatch.SelectedItems.Count >= 1) {
				var itemsToRemove = new List<ListViewItem>();
				foreach(ListViewItem item in lstWatch.SelectedItems) {
					itemsToRemove.Add(item);
				}
				foreach(ListViewItem item in itemsToRemove) {
					lstWatch.Items.Remove(item);
				}
				UpdateWatch();
			}
		}

		private void mnuHexDisplay_Click(object sender, EventArgs e)
		{
			UpdateWatch();
		}

		private void lstWatch_SelectedIndexChanged(object sender, EventArgs e)
		{
			mnuRemoveWatch.Enabled = lstWatch.SelectedItems.Count >= 1;
		}
	}
}
