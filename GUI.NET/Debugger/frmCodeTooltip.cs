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
using Mesen.GUI.Debugger.Controls;

namespace Mesen.GUI.Debugger
{
	public partial class frmCodeTooltip : TooltipForm
	{
		private ICodeViewer _codeViewer;
		private Dictionary<string, string> _values;
		private AddressTypeInfo _previewAddress;
		private CodeInfo _code;
		private Ld65DbgImporter _symbolProvider;

		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		public frmCodeTooltip(Form parent, Dictionary<string, string> values, AddressTypeInfo previewAddress = null, CodeInfo code = null, Ld65DbgImporter symbolProvider = null)
		{
			_parentForm = parent;
			_values = values;
			_previewAddress = previewAddress;
			_code = code;
			_symbolProvider = symbolProvider;
			InitializeComponent();
			this.TopLevel = false;
			this.Parent = _parentForm;
			_parentForm.Controls.Add(this);
		}

		protected override void OnLoad(EventArgs e)
		{
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

			if(_previewAddress != null && ConfigManager.Config.DebugInfo.ShowCodePreview) {
				tlpMain.RowStyles.Insert(1, new RowStyle());

				if(_code != null) {
					_codeViewer = new ctrlDebuggerCode();
					(_codeViewer as ctrlDebuggerCode).Code = _code;
				} else {
					_codeViewer = new ctrlSourceViewer();
					(_codeViewer as ctrlSourceViewer).HideFileDropdown = true;
				}

				_codeViewer.SymbolProvider = _symbolProvider;
				_codeViewer.CodeViewer.BaseFont = new Font(ConfigManager.Config.DebugInfo.FontFamily, ConfigManager.Config.DebugInfo.FontSize, ConfigManager.Config.DebugInfo.FontStyle);
				_codeViewer.CodeViewer.HideSelection = true;
				_codeViewer.CodeViewer.ShowScrollbars = false;
				_codeViewer.ScrollToAddress(_previewAddress, true);
				_codeViewer.SetConfig(ConfigManager.Config.DebugInfo.LeftView, true);

				Control control = _codeViewer as Control;
				control.Dock = DockStyle.Fill;
				tlpMain.SetRow(control, i);
				tlpMain.SetColumn(control, 0);
				tlpMain.SetColumnSpan(control, 2);
				tlpMain.Controls.Add(control);
			}
			tlpMain.ResumeLayout();
			this.Width = this.tlpMain.Width;
			if(this.Location.X + this.Width > _parentForm.ClientSize.Width) {
				int maxWidth = _parentForm.ClientSize.Width - this.Location.X - 10;
				this.tlpMain.MaximumSize = new Size(maxWidth, _parentForm.ClientSize.Height - 10);
				this.MaximumSize = new Size(maxWidth, _parentForm.ClientSize.Height - 10);
			}
			this.Height = this.tlpMain.Height; 
			this.BringToFront();

			base.OnLoad(e);
		}
	}
}
