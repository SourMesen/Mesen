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
	public partial class frmCodePreviewTooltip : TooltipForm
	{
		private ICodeViewer _codeViewer;

		private int _lineIndex;
		private CodeInfo _code;
		private Ld65DbgImporter _symbolProvider;
		private Ld65DbgImporter.FileInfo _selectedFile;

		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		public frmCodePreviewTooltip(Form parent, int lineIndex, CodeInfo code = null, Ld65DbgImporter symbolProvider = null, Ld65DbgImporter.FileInfo selectedFile = null)
		{
			_parentForm = parent;
			_code = code;
			_symbolProvider = symbolProvider;
			_lineIndex = lineIndex;
			_selectedFile = selectedFile;
			InitializeComponent();

			this.TopLevel = false;
			this.Parent = _parentForm;
			_parentForm.Controls.Add(this);
		}

		protected override void OnLoad(EventArgs e)
		{
			panel.SuspendLayout();

			if(_code != null) {
				_codeViewer = new ctrlDebuggerCode();
				_codeViewer.SymbolProvider = _symbolProvider;
				(_codeViewer as ctrlDebuggerCode).Code = _code;
			} else {
				_codeViewer = new ctrlSourceViewer();

				//Must set symbol provider before setting CurrentFile
				_codeViewer.SymbolProvider = _symbolProvider;

				(_codeViewer as ctrlSourceViewer).HideFileDropdown = true;
				(_codeViewer as ctrlSourceViewer).CurrentFile = _selectedFile;
			}

			_codeViewer.CodeViewer.BaseFont = new Font(ConfigManager.Config.DebugInfo.FontFamily, ConfigManager.Config.DebugInfo.FontSize, ConfigManager.Config.DebugInfo.FontStyle);
			_codeViewer.CodeViewer.HideSelection = true;
			_codeViewer.CodeViewer.ShowScrollbars = false;
			_codeViewer.CodeViewer.ScrollToLineIndex(_lineIndex, eHistoryType.Always, true);
			_codeViewer.SetConfig(ConfigManager.Config.DebugInfo.LeftView, true);

			Control control = _codeViewer as Control;
			control.Dock = DockStyle.Fill;
			panel.Controls.Add(control);

			panel.ResumeLayout();
			this.BringToFront();
			base.OnLoad(e);

			panel.BackColor = ThemeHelper.IsDark ? ThemeHelper.Theme.FormBgColor : SystemColors.Info;
		}

		public void ScrollToLineIndex(int lineIndex)
		{
			_codeViewer?.CodeViewer.ScrollToLineIndex(0);
			_codeViewer?.CodeViewer.ScrollToLineIndex(lineIndex);
		}
	}
}
