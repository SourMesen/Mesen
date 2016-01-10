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

		private void lstWatch_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
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

		public void UpdateWatch(int currentSelection = -1)
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

					string newValue = "";
					EvalResultType resultType;
					Int32 result = InteropEmu.DebugEvaluateExpression(item.Text, out resultType);

					switch(resultType) {
						case EvalResultType.Numeric:
							if(mnuHexDisplay.Checked) {
								newValue = "$" + result.ToString("X");
							} else {
								newValue = result.ToString();
							}
							break;
						case EvalResultType.Boolean:
							newValue = result == 0 ? "false" : "true";
							break;

						case EvalResultType.Invalid:
							newValue = "<invalid expression>";
							break;
					}

					item.SubItems.Add(newValue);
					item.SubItems[1].ForeColor = newValue != previousValue ? Color.Red : Color.Black;
				}
			}
			AdjustColumnWidth();

			if(currentSelection >= 0 && lstWatch.Items.Count > currentSelection) {
				lstWatch.FocusedItem = lstWatch.Items[currentSelection];
				lstWatch.Items[currentSelection].Selected = true;
			}
		}

		public void AddWatch(UInt32 address)
		{
			ListViewItem item = lstWatch.Items.Insert(lstWatch.Items.Count - 1, "$" + address.ToString("x"));
			item.Tag = address;
			UpdateWatch();
		}

		private void lstWatch_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			this.BeginInvoke((MethodInvoker)(() => { this.UpdateWatch(e.Item); }));
		}

		private void mnuHexDisplay_Click(object sender, EventArgs e)
		{
			UpdateWatch();
		}

		private void lstWatch_SelectedIndexChanged(object sender, EventArgs e)
		{
			mnuRemoveWatch.Enabled = lstWatch.SelectedItems.Count >= 1;
		}

		private void lstWatch_Click(object sender, EventArgs e)
		{
			if(lstWatch.SelectedItems.Count == 1 && string.IsNullOrWhiteSpace(lstWatch.SelectedItems[0].Text)) {
				lstWatch.SelectedItems[0].BeginEdit();
			}
		}

		private void lstWatch_DoubleClick(object sender, EventArgs e)
		{
			if(lstWatch.SelectedItems.Count == 1) {
				lstWatch.SelectedItems[0].BeginEdit();
			}
		}
	}
}
