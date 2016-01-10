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
		private int _editItemIndex = -1;
		private string _originalText;

		public bool IsEditing
		{
			get { return _editItemIndex >= 0; }
		}

		protected override void OnItemCheck(ItemCheckEventArgs e)
		{
			if(this._preventCheck) {
				e.NewValue = e.CurrentValue;
				this._preventCheck = false;
			} else
				base.OnItemCheck(e);
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

		protected override void OnBeforeLabelEdit(LabelEditEventArgs e)
		{
			_editItemIndex = e.Item;		
			base.OnBeforeLabelEdit(e);
		}

		protected override void OnAfterLabelEdit(LabelEditEventArgs e)
		{
			base.OnAfterLabelEdit(e);
			_editItemIndex = -1;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if(!this.IsEditing && e.KeyData == Keys.Delete) {
				if(this.SelectedItems.Count >= 1) {
					var itemsToRemove = new List<ListViewItem>();
					foreach(ListViewItem item in this.SelectedItems) {
						itemsToRemove.Add(item);
					}
					foreach(ListViewItem item in itemsToRemove) {
						this.Items.Remove(item);
					}
				}
			}
			this._preventCheck = false;
			base.OnKeyDown(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(this.SelectedItems.Count > _editItemIndex && _editItemIndex >= 0) {
				if(keyData == Keys.Escape) {
					this.SelectedItems[_editItemIndex].Text = _originalText;
				}
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if(this.LabelEdit && _editItemIndex < 0 && this.SelectedItems.Count > 0) {
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
}
