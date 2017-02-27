using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;
using Mesen.GUI.Controls;
using Be.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmMemoryViewer : BaseForm
	{
		private InteropEmu.NotificationListener _notifListener;
		private DebugMemoryType _memoryType = DebugMemoryType.CpuMemory;
		private Func<DebugWorkspace> _getWorkspace;

		public frmMemoryViewer(Func<DebugWorkspace> getWorkspace)
		{
			InitializeComponent();

			this._getWorkspace = getWorkspace;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.mnuAutoRefresh.Checked = ConfigManager.Config.DebugInfo.RamAutoRefresh;
			this.mnuShowCharacters.Checked = ConfigManager.Config.DebugInfo.RamShowCharacters;
			this.ctrlHexViewer.SetFontSize((int)ConfigManager.Config.DebugInfo.RamFontSize);

			if(this._getWorkspace().TblMappings != null && this._getWorkspace().TblMappings.Count > 0) {
				this.InitTblMappings();
			}
			this.ctrlHexViewer.StringViewVisible = mnuShowCharacters.Checked;

			UpdateImportButton();

			this.cboMemoryType.SelectedIndex = 0;

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			this.mnuShowCharacters.CheckedChanged += new EventHandler(this.mnuShowCharacters_CheckedChanged);
		}

		void InitTblMappings()
		{
			var tblDict = TblLoader.ToDictionary(this._getWorkspace().TblMappings.ToArray());
			if(tblDict != null) {
				this.ctrlHexViewer.ByteCharConverter = new TblByteCharConverter(tblDict);
			}
		}

		void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.CodeBreak) {
				this.BeginInvoke((MethodInvoker)(() => this.RefreshData()));
			}
		}
		
		private void cboMemoryType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._memoryType = (DebugMemoryType)this.cboMemoryType.SelectedIndex;
			this.UpdateImportButton();
			this.RefreshData();
		}

		private void mnuRefresh_Click(object sender, EventArgs e)
		{
			this.RefreshData();
		}

		int _previousIndex = -1;
		DebugWorkspace _previousWorkspace;
		private void RefreshData()
		{
			if(this._getWorkspace() != this._previousWorkspace) {
				this.InitTblMappings();
			}
			_previousWorkspace = this._getWorkspace();

			if(this.tabMain.SelectedTab == this.tpgAccessCounters) {
				this.ctrlMemoryAccessCounters.RefreshData();
			} else if(this.tabMain.SelectedTab == this.tpgMemoryViewer) {
				this.ctrlHexViewer.SetData(InteropEmu.DebugGetMemoryState((DebugMemoryType)this.cboMemoryType.SelectedIndex), this.cboMemoryType.SelectedIndex != _previousIndex);
				this._previousIndex = this.cboMemoryType.SelectedIndex;
			}
		}

		private void mnuFind_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.OpenSearchBox();
		}

		private void mnuFindNext_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.FindNext();
		}

		private void mnuFindPrev_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.FindPrevious();
		}

		private void mnuGoTo_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.GoToAddress();
		}

		private void mnuIncreaseFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.IncreaseFontSize();
			this.UpdateConfig();
		}

		private void mnuDecreaseFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.DecreaseFontSize();
			this.UpdateConfig();
		}

		private void mnuResetFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.ResetFontSize();
			this.UpdateConfig();
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void UpdateConfig()
		{
			ConfigManager.Config.DebugInfo.RamAutoRefresh = this.mnuAutoRefresh.Checked;
			ConfigManager.Config.DebugInfo.RamShowCharacters = this.mnuShowCharacters.Checked;
			ConfigManager.Config.DebugInfo.RamFontSize = this.ctrlHexViewer.HexFont.Size;
			ConfigManager.ApplyChanges();
		}

		private void mnuAutoRefresh_Click(object sender, EventArgs e)
		{
			this.UpdateConfig();
		}

		private void UpdateImportButton()
		{
			switch(_memoryType) {
				case DebugMemoryType.ChrRam:
				case DebugMemoryType.InternalRam:
				case DebugMemoryType.PaletteMemory:
				case DebugMemoryType.SecondarySpriteMemory:
				case DebugMemoryType.SpriteMemory:
				case DebugMemoryType.WorkRam:
				case DebugMemoryType.SaveRam:
					btnImport.Enabled = mnuImport.Enabled = true;
					break;

				default:
					btnImport.Enabled = mnuImport.Enabled = false;
					break;
			}
		}

		private void mnuImport_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter("Memory dump files (*.dmp)|*.dmp|All files (*.*)|*.*");
			ofd.InitialDirectory = ConfigManager.DebuggerFolder;
			if(ofd.ShowDialog() == DialogResult.OK) {
				InteropEmu.DebugSetMemoryState(_memoryType, File.ReadAllBytes(ofd.FileName));
				RefreshData();
			}
		}

		private void mnuExport_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.SetFilter("Memory dump files (*.dmp)|*.dmp|All files (*.*)|*.*");
			sfd.InitialDirectory = ConfigManager.DebuggerFolder;
			sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + " - " + cboMemoryType.SelectedItem.ToString() + ".dmp";
			if(sfd.ShowDialog() == DialogResult.OK) {
				File.WriteAllBytes(sfd.FileName, this.ctrlHexViewer.GetData());
			}
		}

		private void tmrRefresh_Tick(object sender, EventArgs e)
		{
			if(this.mnuAutoRefresh.Checked) {
				this.RefreshData();
			}
		}

		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.tmrRefresh.Interval = this.tabMain.SelectedTab == this.tpgMemoryViewer ? 100 : 500;

			if(this.tabMain.SelectedTab == this.tpgProfiler) {
				this.ctrlProfiler.RefreshData();
			}
		}

		private void ctrlHexViewer_RequiredWidthChanged(object sender, EventArgs e)
		{
			this.Size = new Size(this.ctrlHexViewer.RequiredWidth + this.Width - this.ctrlHexViewer.Width + 30, this.Height);
		}

		private void mnuLoadTblFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter("TBL files (*.tbl)|*.tbl");
			if(ofd.ShowDialog() == DialogResult.OK) {
				string[] fileContents = File.ReadAllLines(ofd.FileName);
				var tblDict = TblLoader.ToDictionary(fileContents);
				if(tblDict == null) {
					MessageBox.Show("Could not load TBL file.  The file selected file appears to be invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else {
					this._getWorkspace().TblMappings = new List<string>(fileContents);
					this.ctrlHexViewer.ByteCharConverter = new TblByteCharConverter(tblDict);
					this.mnuShowCharacters.Checked = true;
				}
			}
		}

		private void mnuResetTblMappings_Click(object sender, EventArgs e)
		{
			this._getWorkspace().TblMappings = null;
			this.ctrlHexViewer.ByteCharConverter = null;
		}

		private void mnuShowCharacters_CheckedChanged(object sender, EventArgs e)
		{
			this.ctrlHexViewer.StringViewVisible = mnuShowCharacters.Checked;
			this.UpdateConfig();
		}
	}
}
