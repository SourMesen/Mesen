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

namespace Mesen.GUI.Debugger
{
	public partial class ctrlWatch : BaseControl
	{
		private int _currentSelection = -1;

		public ctrlWatch()
		{
			InitializeComponent();

			this.DoubleBuffered = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(!IsDesignMode) {
				this.mnuHexDisplay.Checked = ConfigManager.Config.DebugInfo.HexDisplay;
				WatchManager.WatchChanged += WatchManager_WatchChanged;
				mnuRemoveWatch.InitShortcut(this, nameof(DebuggerShortcutsConfig.WatchList_Delete));
			}
		}

		private void WatchManager_WatchChanged(object sender, EventArgs e)
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((Action)(() => this.UpdateWatch()));
			} else {
				this.UpdateWatch();
			}
		}

		public void UpdateWatch()
		{
			List<WatchValueInfo> watchContent = WatchManager.GetWatchContent(mnuHexDisplay.Checked);

			lstWatch.BeginUpdate();

			if(watchContent.Count != lstWatch.Items.Count - 1) {
				lstWatch.Items.Clear();

				List<ListViewItem> itemsToAdd = new List<ListViewItem>();
				foreach(WatchValueInfo watch in watchContent) {
					ListViewItem item = new ListViewItem(watch.Expression);
					item.UseItemStyleForSubItems = false;
					item.SubItems.Add(watch.Value).ForeColor = watch.HasChanged ? Color.Red : Color.Black;
					itemsToAdd.Add(item);
				}
				var lastItem = new ListViewItem("");
				lastItem.SubItems.Add("");
				itemsToAdd.Add(lastItem);
				lstWatch.Items.AddRange(itemsToAdd.ToArray());
			} else {
				for(int i = 0; i < watchContent.Count; i++) {
					ListViewItem item = lstWatch.Items[i];
					item.SubItems[0].Text = watchContent[i].Expression;
					item.SubItems[1].Text = watchContent[i].Value.ToString();
					item.SubItems[1].ForeColor = watchContent[i].HasChanged ? Color.Red : Color.Black;
				}
			}

			lstWatch.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
			if(colValue.Width < 100) {
				colValue.Width = 100;
			}

			lstWatch.EndUpdate();

			if(_currentSelection >= 0 && lstWatch.Items.Count > _currentSelection) {
				lstWatch.FocusedItem = lstWatch.Items[_currentSelection];
				lstWatch.Items[_currentSelection].Selected = true;
				_currentSelection = -1;
			}
		}
				
		private void lstWatch_AfterEdit(object sender, LabelEditEventArgs e)
		{
			_currentSelection = e.Item;
			WatchManager.UpdateWatch(e.Item, e.Label);
		}

		private void mnuHexDisplay_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.HexDisplay = this.mnuHexDisplay.Checked;
			ConfigManager.ApplyChanges();
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

		private void mnuRemoveWatch_Click(object sender, EventArgs e)
		{
			if(lstWatch.SelectedItems.Count >= 1 && !lstWatch.IsEditing) {
				var itemsToRemove = new List<int>();
				foreach(ListViewItem item in lstWatch.SelectedItems) {
					if(_currentSelection == -1) {
						_currentSelection = item.Index;
					}
					itemsToRemove.Add(item.Index);
				}
				WatchManager.RemoveWatch(itemsToRemove.ToArray());
			}
		}
	}
}
