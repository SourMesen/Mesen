using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms
{
	public partial class BaseConfigForm : BaseForm
	{
		public BaseConfigForm()
		{
			InitializeComponent();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(this.DialogResult == System.Windows.Forms.DialogResult.OK) {
				UpdateConfig();
				if(ApplyChangesOnOK) {
					ConfigManager.ApplyChanges();
				}
			} else {
				ConfigManager.RejectChanges();
			}
			base.OnFormClosed(e);
		}

		protected virtual bool ApplyChangesOnOK
		{
			get { return true; }
		}

		protected virtual void UpdateConfig()
		{
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
