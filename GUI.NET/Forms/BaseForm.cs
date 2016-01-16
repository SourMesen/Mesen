using System;
using System.Collections.Generic;
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

		public BaseForm()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!DesignMode) {
				Icon = Properties.Resources.MesenIcon;
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
