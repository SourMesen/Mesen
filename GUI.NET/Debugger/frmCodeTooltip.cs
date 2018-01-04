using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;
using Mesen.GUI.Config;

namespace Mesen.GUI.Debugger
{
	public partial class frmCodeTooltip : Form
	{
		private Dictionary<string, string> _values;
		private int _previewAddress;
		private string _code;

		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		public frmCodeTooltip(Dictionary<string, string> values, int previewAddress = -1, string code = null)
		{
			_values = values;
			_previewAddress = previewAddress;
			_code = code;
			InitializeComponent();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			tlpMain.SuspendLayout();
			int i = 0;
			foreach(KeyValuePair<string, string> kvp in _values) {
				tlpMain.RowStyles.Insert(1, new RowStyle());
				Label lbl = new Label();
				lbl.Margin = new Padding(2, 3, 2, 2);
				lbl.Text = kvp.Key + ":";
				lbl.Font = new Font(lbl.Font, FontStyle.Bold);
				lbl.AutoSize = true;
				tlpMain.SetRow(lbl, i);
				tlpMain.SetColumn(lbl, 0);
				tlpMain.Controls.Add(lbl);

				lbl = new Label();
				lbl.Font = new Font(BaseControl.MonospaceFontFamily, 10);
				lbl.Margin = new Padding(2);
				lbl.AutoSize = true;
				lbl.Text = kvp.Value;
				tlpMain.SetRow(lbl, i);
				tlpMain.SetColumn(lbl, 1);
				tlpMain.Controls.Add(lbl);

				i++;
			}

			if(_previewAddress >= 0) {
				tlpMain.RowStyles.Insert(1, new RowStyle());

				ctrlDebuggerCode codeWindow = new ctrlDebuggerCode();
				codeWindow.SetConfig(ConfigManager.Config.DebugInfo.LeftView);
				codeWindow.Code = _code;
				codeWindow.Dock = DockStyle.Fill;
				codeWindow.ShowScrollbars = false;
				codeWindow.ScrollToLineNumber(_previewAddress, true);

				tlpMain.SetRow(codeWindow, i);
				tlpMain.SetColumn(codeWindow, 0);
				tlpMain.SetColumnSpan(codeWindow, 2);
				tlpMain.Controls.Add(codeWindow);
			}
			tlpMain.ResumeLayout();
			this.Width = this.tlpMain.Width;
			this.Height = this.tlpMain.Height; 
		}
	}
}
