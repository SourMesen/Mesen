using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	public partial class frmSelectRom : BaseForm
	{
		private List<string> _romFiles;
		private int SelectedIndex { get; set; }

		public frmSelectRom(List<string> romFiles)
		{
			InitializeComponent();

			_romFiles = romFiles;
			lblRomCount.Text = ResourceHelper.GetMessage("RomsFound", _romFiles.Count.ToString());

			lstRoms.Items.AddRange(romFiles.ToArray());
			lstRoms.Sorted = true;
			this.DialogResult = DialogResult.Cancel;
		}

		public static bool SelectRom(string filename, out int archiveFileIndex)
		{
			archiveFileIndex = -1;

			List<string> archiveRomList = InteropEmu.GetArchiveRomList(filename);
			if(archiveRomList.Count > 1) {
				frmSelectRom frm = new frmSelectRom(archiveRomList);
				if(frm.ShowDialog(null, Application.OpenForms[0]) == DialogResult.OK) {
					archiveFileIndex = frm.SelectedIndex;
				} else {
					return false;
				}
			}

			return true;
		}

		void SetSelectedIndex()
		{
			this.SelectedIndex = _romFiles.IndexOf(lstRoms.SelectedItem.ToString());
			this.DialogResult = DialogResult.OK;
		}

		private void lstRoms_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnOK.Enabled = lstRoms.SelectedItems.Count > 0;
		}

		private void lstRoms_DoubleClick(object sender, EventArgs e)
		{
			SetSelectedIndex();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SetSelectedIndex();
		}
	}
}
