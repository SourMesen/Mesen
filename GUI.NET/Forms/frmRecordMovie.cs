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
	public partial class frmRecordMovie : BaseConfigForm
	{
		public frmRecordMovie()
		{
			InitializeComponent();

			Entity = ConfigManager.Config.MovieRecordInfo;
			AddBinding("Author", txtAuthor);
			AddBinding("Description", txtDescription);
			AddBinding("RecordFrom", cboRecordFrom);
		}

		protected override bool ValidateInput()
		{
			return !string.IsNullOrWhiteSpace(txtFilename.Text);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			if(this.DialogResult == DialogResult.OK) {
				RecordMovieOptions options = new RecordMovieOptions(
					this.txtFilename.Text,
					this.txtAuthor.Text,
					this.txtDescription.Text,
					this.cboRecordFrom.GetEnumValue<RecordMovieFrom>()
				);
				InteropEmu.MovieRecord(ref options);
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.SetFilter(ResourceHelper.GetMessage("FilterMovie"));
			sfd.InitialDirectory = ConfigManager.MovieFolder;
			sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".mmo";
			if(sfd.ShowDialog() == DialogResult.OK) {
				txtFilename.Text = sfd.FileName;
			}
		}
	}
}
