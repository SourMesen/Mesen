using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Controls
{
	class MyListView : ListView
	{
		private bool _preventCheck = false;

		public MyListView()
		{
			this.DoubleBuffered = true;
		}

		protected override void OnItemCheck(ItemCheckEventArgs e)
		{
			if(this._preventCheck || Control.ModifierKeys.HasFlag(Keys.Control) || Control.ModifierKeys.HasFlag(Keys.Shift)) {
				e.NewValue = e.CurrentValue;
				this._preventCheck = false;
			} else {
				base.OnItemCheck(e);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left && e.Clicks > 1) {
				this._preventCheck = true;
			} else {
				this._preventCheck = false;
			}
			base.OnMouseDown(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			this._preventCheck = false;
			base.OnKeyDown(e);
		}
	}

	class WatchList : MyListView
	{
		private int _editItemIndex = -1;
		private string _originalText = null;
		private bool _pressedEsc = false;

		public event LabelEditEventHandler AfterEdit;

		public WatchList()
		{
			this.DoubleBuffered = true;
		}

		public bool IsEditing
		{
			get { return _editItemIndex >= 0; }
		}

		protected override void OnBeforeLabelEdit(LabelEditEventArgs e)
		{
			if(_originalText == null) {
				_originalText = this.Items[e.Item].Text;
			}
			_editItemIndex = e.Item;
			base.OnBeforeLabelEdit(e);
		}

		protected override void OnAfterLabelEdit(LabelEditEventArgs e)
		{
			base.OnAfterLabelEdit(e);
			if(e.Label != null) {
				string text = e.Label;
				var item = this.Items[e.Item];
				AfterEdit?.Invoke(this, new LabelEditEventArgs(item.Index, text));
			} else if(_pressedEsc) {
				string text = _originalText;
				var originalItem = this.Items[e.Item];
				var newItem = new ListViewItem(_originalText);
				newItem.SubItems.Add(originalItem.SubItems[1].Text);
				this.Items.RemoveAt(e.Item);
				this.Items.Insert(e.Item, newItem);
				this.FocusedItem = newItem;
				foreach(ListViewItem item in this.Items) {
					item.Selected = false;
				}
				newItem.Selected = true;
				_pressedEsc = false;
			}
			_originalText = null;
			_editItemIndex = -1;
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(this.IsEditing && keyData == Keys.Escape) {
				_pressedEsc = true;
			}
			if(!this.IsEditing || keyData != Keys.Delete) {
				return base.ProcessCmdKey(ref msg, keyData);
			}
			return false;
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if(this.LabelEdit && !this.IsEditing && this.SelectedItems.Count > 0) {
				if(e.KeyChar >= 32 && e.KeyChar <= 127) {
					_originalText = this.SelectedItems[0].Text;
					this.SelectedItems[0].Text = e.KeyChar.ToString();
					this.SelectedItems[0].BeginEdit();
					SendKeys.Send("{RIGHT}");
				}
			}

			base.OnKeyPress(e);
		}
	}

	public class DoubleBufferedListView : ListView
	{
		public DoubleBufferedListView()
		{
			this.DoubleBuffered = true;
		}
	}
}
