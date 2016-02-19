using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	public class BaseForm : Form
	{
		protected ToolTip toolTip;
		private System.ComponentModel.IContainer components;
		private bool _iconSet = false;


		public BaseForm()
		{
			InitializeComponent();
		}

		public void Show(object sender, IWin32Window owner = null)
		{
			if(sender is ToolStripMenuItem) {
				ToolStripItem menuItem = (ToolStripMenuItem)sender;
				if(menuItem.Image == null) {
					menuItem = menuItem.OwnerItem;
				}
				this.Icon = menuItem.Image;
			}

			base.Show(owner);
		}

		public DialogResult ShowDialog(object sender, IWin32Window owner = null)
		{
			if(sender is ToolStripMenuItem) {
				ToolStripItem menuItem = (ToolStripMenuItem)sender;
				if(menuItem.Image == null) {
					menuItem = menuItem.OwnerItem;
				}
				this.Icon = menuItem.Image;
			}
			return base.ShowDialog(owner);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!DesignMode) {
				if(!_iconSet) {
					base.Icon = Properties.Resources.MesenIcon;
				}
			}

			int tabIndex = 0;
			InitializeTabIndexes(this, ref tabIndex);
			ResourceHelper.ApplyResources(this);
		}

		private void InitializeTabIndexes(TableLayoutPanel tlp, ref int tabIndex)
		{
			tlp.TabIndex = tabIndex;
			tabIndex++;

			for(int i = 0; i < tlp.RowCount; i++) {
				for(int j = 0; j < tlp.ColumnCount; j++) {
					Control ctrl = tlp.GetControlFromPosition(j, i);
					if(ctrl != null) {
						if(ctrl is TableLayoutPanel) {
							InitializeTabIndexes(((TableLayoutPanel)ctrl), ref tabIndex);
						} else {
							InitializeTabIndexes(ctrl, ref tabIndex);
						}
					}
				}
			}
		}

		private void InitializeTabIndexes(Control container, ref int tabIndex)
		{
			container.TabIndex = tabIndex;
			tabIndex++;

			foreach(Control ctrl in container.Controls) {
				if(ctrl is TableLayoutPanel) {
					InitializeTabIndexes(((TableLayoutPanel)ctrl), ref tabIndex);
				} else {
					InitializeTabIndexes(ctrl, ref tabIndex);
				}
			}
		}

		public new Image Icon
		{
			set
			{
				if(value != null) {
					Bitmap b = new Bitmap(value);
					Icon i = System.Drawing.Icon.FromHandle(b.GetHicon());
					base.Icon = i;
					i.Dispose();

					_iconSet = true;
				}
			}				
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 0;
			this.toolTip.AutoPopDelay = 32700;
			this.toolTip.InitialDelay = 10;
			this.toolTip.ReshowDelay = 10;
			// 
			// BaseForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Name = "BaseForm";
			this.ResumeLayout(false);

		}
	}
}
