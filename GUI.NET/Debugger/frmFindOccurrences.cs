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
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmFindOccurrences : BaseConfigForm
	{
		public string SearchString { get { return txtSearchString.Text; } }
		public bool MatchWholeWord { get { return chkMatchWholeWord.Checked; } }
		public bool MatchCase { get { return chkMatchCase.Checked; } }

		public frmFindOccurrences()
		{
			InitializeComponent();

			txtSearchString.Text = ConfigManager.Config.DebugInfo.FindOccurrencesLastSearch;
			chkMatchWholeWord.Checked = ConfigManager.Config.DebugInfo.FindOccurrencesMatchWholeWord;
			chkMatchCase.Checked = ConfigManager.Config.DebugInfo.FindOccurrencesMatchCase;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			txtSearchString.Focus();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			if(DialogResult == DialogResult.OK) {
				ConfigManager.Config.DebugInfo.FindOccurrencesLastSearch = txtSearchString.Text;
				ConfigManager.Config.DebugInfo.FindOccurrencesMatchWholeWord = chkMatchWholeWord.Checked;
				ConfigManager.Config.DebugInfo.FindOccurrencesMatchCase = chkMatchCase.Checked;
				ConfigManager.ApplyChanges();
			}
		}
	}
}
