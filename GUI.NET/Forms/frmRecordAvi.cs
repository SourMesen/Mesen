using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms
{
	public partial class frmRecordAvi : BaseConfigForm
	{
		public frmRecordAvi()
		{
			InitializeComponent();
		}

		public string Filename { get; internal set; }
		public bool UseCompression { get; internal set; }

		protected override bool ValidateInput()
		{
			return !string.IsNullOrWhiteSpace(txtFilename.Text);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			this.Filename = txtFilename.Text;
			this.UseCompression = chkUseCompression.Checked;
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.SetFilter(ResourceHelper.GetMessage("FilterAvi"));
			sfd.InitialDirectory = ConfigManager.AviFolder;
			sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".avi";
			if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				txtFilename.Text = sfd.FileName;
			}
		}
	}
}
