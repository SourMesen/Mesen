﻿using System;
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
using System.Text.RegularExpressions;
using System.Globalization;

namespace Mesen.GUI.Debugger
{
	public partial class ctrlWatch : BaseControl
	{
		private static Regex _watchAddressOrLabel = new Regex(@"^(\[|{)(\s*((\$[0-9A-Fa-f]+)|(\d+)|([@_a-zA-Z0-9]+)))\s*[,]{0,1}\d*\s*(\]|})$", RegexOptions.Compiled);

		private int _currentSelection = -1;
		private int _previousMaxLength = -1;
		private int _selectedAddress = -1;
		private CodeLabel _selectedLabel = null;

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
				mnuEditInMemoryViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.CodeWindow_EditInMemoryViewer));
				mnuViewInDisassembly.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_ViewInDisassembly));
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			UpdateActions();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void contextMenuWatch_Opening(object sender, CancelEventArgs e)
		{
			UpdateActions();
		}

		private void WatchManager_WatchChanged(object sender, EventArgs e)
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((Action)(() => this.UpdateWatch()));
			} else {
				this.UpdateWatch();
			}
		}

		public void UpdateWatch(bool autoResizeColumns = true)
		{
			List<WatchValueInfo> watchContent = WatchManager.GetWatchContent(mnuHexDisplay.Checked);

			bool updating = false;
			if(watchContent.Count != lstWatch.Items.Count - 1) {
				lstWatch.BeginUpdate();
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
				updating = true;
			} else {
				for(int i = 0; i < watchContent.Count; i++) {
					ListViewItem item = lstWatch.Items[i];
					bool needUpdate = (
						item.SubItems[0].Text != watchContent[i].Expression ||
						item.SubItems[1].Text != watchContent[i].Value ||
						item.SubItems[1].ForeColor != (watchContent[i].HasChanged ? Color.Red : Color.Black)
					);
					if(needUpdate) {
						updating = true;
						item.SubItems[0].Text = watchContent[i].Expression;
						item.SubItems[1].Text = watchContent[i].Value;
						item.SubItems[1].ForeColor = watchContent[i].HasChanged ? Color.Red : Color.Black;
					}
				}
			}

			if(updating) {
				if(watchContent.Count > 0) {
					int maxLength = watchContent.Select(info => info.Value.Length).Max();
					if(_previousMaxLength != maxLength) {
						if(autoResizeColumns) {
							lstWatch.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
						}
						if(colValue.Width < 100) {
							colValue.Width = 100;
						}
						_previousMaxLength = maxLength;
					}
				}
				lstWatch.EndUpdate();
			}

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
			UpdateActions();
		}

		private void UpdateActions()
		{
			mnuEditInMemoryViewer.Enabled = false;
			mnuViewInDisassembly.Enabled = false;

			if(lstWatch.SelectedItems.Count == 1) {
				Match match = _watchAddressOrLabel.Match(lstWatch.SelectedItems[0].Text);
				if(match.Success) {
					string address = match.Groups[3].Value;

					if(address[0] >= '0' && address[0] <= '9' || address[0] == '$') {
						//CPU Address
						_selectedAddress = Int32.Parse(address[0] == '$' ? address.Substring(1) : address, address[0] == '$' ? NumberStyles.AllowHexSpecifier : NumberStyles.None);
						_selectedLabel = null;
						mnuEditInMemoryViewer.Enabled = true;
						mnuViewInDisassembly.Enabled = true;
					} else {
						//Label
						_selectedAddress = -1;
						_selectedLabel = LabelManager.GetLabel(address);
						if(_selectedLabel != null) {
							mnuEditInMemoryViewer.Enabled = true;
							mnuViewInDisassembly.Enabled = true;
						}
					}
				}
			}
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

		private void mnuViewInDisassembly_Click(object sender, EventArgs e)
		{
			if(lstWatch.SelectedItems.Count != 1) {
				return;
			}

			if(_selectedAddress >= 0) {
				DebugWindowManager.GetDebugger().ScrollToAddress(_selectedAddress);
			} else if(_selectedLabel != null) {
				int relAddress = _selectedLabel.GetRelativeAddress();
				if(relAddress >= 0) {
					DebugWindowManager.GetDebugger().ScrollToAddress(relAddress);
				}
			}
		}

		private void mnuEditInMemoryViewer_Click(object sender, EventArgs e)
		{
			if(lstWatch.SelectedItems.Count != 1) {
				return;
			}

			if(_selectedAddress >= 0) {
				DebugWindowManager.OpenMemoryViewer(_selectedAddress, DebugMemoryType.CpuMemory);
			} else if(_selectedLabel != null) {
				DebugWindowManager.OpenMemoryViewer((int)_selectedLabel.Address, _selectedLabel.AddressType.ToMemoryType());
			}
		}
	}
}
