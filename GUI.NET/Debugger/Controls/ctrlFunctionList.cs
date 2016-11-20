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

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlFunctionList : UserControl
	{
		public event EventHandler OnFunctionSelected;
		public ctrlFunctionList()
		{
			InitializeComponent();
		}

		private class FunctionComparer : IComparer
		{
			int IComparer.Compare(object x, object y)
			{
				ListViewItem a = x as ListViewItem;
				ListViewItem b = y as ListViewItem;

				string aText = string.IsNullOrWhiteSpace(a.Text) ? "ZZZZZZZZZZZZZZZZZZZZZZZ" : a.Text;
				string bText = string.IsNullOrWhiteSpace(b.Text) ? "ZZZZZZZZZZZZZZZZZZZZZZZ" : b.Text;
				Int32 aRelative = (Int32)a.Tag == -1 ? Int32.MaxValue : (Int32)a.Tag;
				Int32 bRelative = (Int32)b.Tag == -1 ? Int32.MaxValue : (Int32)b.Tag;
				Int32 aAbsolute = (Int32)a.SubItems[1].Tag;
				Int32 bAbsolute = (Int32)b.SubItems[1].Tag;

				if(a.Text == b.Text) {
					if(a.Tag == b.Tag) {
						return aAbsolute > bAbsolute ? 1 : -1;
					} else {
						return aRelative > bRelative ? 1 : -1;
					}
				} else {
					return string.Compare(aText, bText);
				}
			}
		}

		public void UpdateFunctionList()
		{
			Int32[] entryPoints = InteropEmu.DebugGetFunctionEntryPoints();

			lstFunctions.BeginUpdate();
			lstFunctions.ListViewItemSorter = null;
			lstFunctions.Items.Clear();
			for(int i = 0; entryPoints[i] >= 0; i++) {
				ListViewItem item = lstFunctions.Items.Add("");

				Int32 relativeAddress = InteropEmu.DebugGetRelativeAddress((UInt32)entryPoints[i]);
				if(relativeAddress >= 0) {
					item.SubItems.Add("$" + relativeAddress.ToString("X4"));
				} else {
					item.SubItems.Add("[n/a]");
					item.ForeColor = Color.Gray;
					item.Font = new Font(item.Font, FontStyle.Italic);
				}
				item.SubItems.Add("$" + entryPoints[i].ToString("X4"));
				item.SubItems[1].Tag = entryPoints[i];
				
				item.Tag = relativeAddress;
			}
			lstFunctions.ListViewItemSorter = new FunctionComparer();
			lstFunctions.Sort();
			lstFunctions.EndUpdate();
		}

		private void lstFunctions_DoubleClick(object sender, EventArgs e)
		{
			if(lstFunctions.SelectedItems.Count > 0) {
				Int32 relativeAddress = (Int32)lstFunctions.SelectedItems[0].Tag;

				if(relativeAddress >= 0) {
					OnFunctionSelected?.Invoke(relativeAddress, e);
				}
			}
		}
	}
}
