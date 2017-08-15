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
using Mesen.GUI.Debugger.Controls;

namespace Mesen.GUI.Debugger
{
	public partial class frmMemoryViewer : BaseForm
	{
		private InteropEmu.NotificationListener _notifListener;
		private DebugMemoryType _memoryType = DebugMemoryType.CpuMemory;
		private int _previousIndex = -1;
		private DebugWorkspace _previousWorkspace;

		public frmMemoryViewer()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.mnuAutoRefresh.Checked = ConfigManager.Config.DebugInfo.RamAutoRefresh;
			this.mnuShowCharacters.Checked = ConfigManager.Config.DebugInfo.RamShowCharacters;
			this.ctrlHexViewer.SetFontSize((int)ConfigManager.Config.DebugInfo.RamFontSize);
			
			this.mnuHighlightExecution.Checked = ConfigManager.Config.DebugInfo.RamHighlightExecution;
			this.mnuHightlightReads.Checked = ConfigManager.Config.DebugInfo.RamHighlightReads;
			this.mnuHighlightWrites.Checked = ConfigManager.Config.DebugInfo.RamHighlightWrites;
			this.mnuHideUnusedBytes.Checked = ConfigManager.Config.DebugInfo.RamHideUnusedBytes;
			this.mnuHideReadBytes.Checked = ConfigManager.Config.DebugInfo.RamHideReadBytes;
			this.mnuHideWrittenBytes.Checked = ConfigManager.Config.DebugInfo.RamHideWrittenBytes;
			this.mnuHideExecutedBytes.Checked = ConfigManager.Config.DebugInfo.RamHideExecutedBytes;

			this.UpdateFadeOptions();

			this.InitTblMappings();

			this.ctrlHexViewer.StringViewVisible = mnuShowCharacters.Checked;

			UpdateImportButton();

			this.cboMemoryType.SelectedIndex = 0;

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			this.mnuShowCharacters.CheckedChanged += new EventHandler(this.mnuShowCharacters_CheckedChanged);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			DebugWorkspaceManager.SaveWorkspace();
		}

		void InitTblMappings()
		{
			DebugWorkspace workspace = DebugWorkspaceManager.GetWorkspace();
			if(workspace.TblMappings != null && workspace.TblMappings.Count > 0) {
				var tblDict = TblLoader.ToDictionary(workspace.TblMappings.ToArray());
				if(tblDict != null) {
					this.ctrlHexViewer.ByteCharConverter = new TblByteCharConverter(tblDict);
				}
			} else {
				this.ctrlHexViewer.ByteCharConverter = null;
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

		private void UpdateByteColorProvider()
		{
			switch((DebugMemoryType)this.cboMemoryType.SelectedIndex) {
				case DebugMemoryType.CpuMemory:
				case DebugMemoryType.PrgRom:
				case DebugMemoryType.WorkRam:
				case DebugMemoryType.SaveRam:
				case DebugMemoryType.InternalRam:
					this.ctrlHexViewer.ByteColorProvider = new ByteColorProvider(
						(DebugMemoryType)this.cboMemoryType.SelectedIndex,
						mnuHighlightExecution.Checked,
						mnuHighlightWrites.Checked,
						mnuHightlightReads.Checked,
						ConfigManager.Config.DebugInfo.RamFadeSpeed,
						mnuHideUnusedBytes.Checked,
						mnuHideReadBytes.Checked,
						mnuHideWrittenBytes.Checked,
						mnuHideExecutedBytes.Checked
					);
					break;

				default:
					this.ctrlHexViewer.ByteColorProvider = null;
					break;
			}
		}

		private void mnuRefresh_Click(object sender, EventArgs e)
		{
			this.RefreshData();
		}

		private void RefreshData()
		{
			if(DebugWorkspaceManager.GetWorkspace() != this._previousWorkspace) {
				this.InitTblMappings();
				_previousWorkspace = DebugWorkspaceManager.GetWorkspace();
			}

			if(this.tabMain.SelectedTab == this.tpgAccessCounters) {
				this.ctrlMemoryAccessCounters.RefreshData();
			} else if(this.tabMain.SelectedTab == this.tpgMemoryViewer) {
				this.UpdateByteColorProvider();
				this.ctrlHexViewer.SetData(InteropEmu.DebugGetMemoryState((DebugMemoryType)this.cboMemoryType.SelectedIndex));
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

			ConfigManager.Config.DebugInfo.RamHighlightExecution = this.mnuHighlightExecution.Checked;
			ConfigManager.Config.DebugInfo.RamHighlightReads = this.mnuHightlightReads.Checked;
			ConfigManager.Config.DebugInfo.RamHighlightWrites = this.mnuHighlightWrites.Checked;
			ConfigManager.Config.DebugInfo.RamHideUnusedBytes = this.mnuHideUnusedBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHideReadBytes = this.mnuHideReadBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHideWrittenBytes = this.mnuHideWrittenBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHideExecutedBytes = this.mnuHideExecutedBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHideExecutedBytes = this.mnuHideExecutedBytes.Checked;

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

		private void ctrlHexViewer_ByteChanged(int byteIndex, byte newValue, byte oldValue)
		{
			InteropEmu.DebugSetMemoryValue(_memoryType, (UInt32)byteIndex, newValue);
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
					DebugWorkspaceManager.GetWorkspace().TblMappings = new List<string>(fileContents);
					this.ctrlHexViewer.ByteCharConverter = new TblByteCharConverter(tblDict);
					this.mnuShowCharacters.Checked = true;
				}
			}
		}

		private void mnuResetTblMappings_Click(object sender, EventArgs e)
		{
			DebugWorkspaceManager.GetWorkspace().TblMappings = null;
			this.ctrlHexViewer.ByteCharConverter = null;
		}

		private void mnuShowCharacters_CheckedChanged(object sender, EventArgs e)
		{
			this.ctrlHexViewer.StringViewVisible = mnuShowCharacters.Checked;
			this.UpdateConfig();
		}

		private void UpdateFadeOptions()
		{
			int fadeSpeed = ConfigManager.Config.DebugInfo.RamFadeSpeed;
			mnuFadeSlow.Checked = fadeSpeed == 600;
			mnuFadeNormal.Checked = fadeSpeed == 300;
			mnuFadeFast.Checked = fadeSpeed == 120;
			mnuFadeNever.Checked = fadeSpeed == 0;
			mnuCustomFadeSpeed.Checked = !mnuFadeSlow.Checked && !mnuFadeNormal.Checked && !mnuFadeFast.Checked && !mnuFadeSlow.Checked;
		}

		private void mnuFadeSpeed_Click(object sender, EventArgs e)
		{
			if(sender == mnuFadeSlow) {
				ConfigManager.Config.DebugInfo.RamFadeSpeed = 600;
			} else if(sender == mnuFadeNormal) {
				ConfigManager.Config.DebugInfo.RamFadeSpeed = 300;
			} else if(sender == mnuFadeFast) {
				ConfigManager.Config.DebugInfo.RamFadeSpeed = 120;
			} else if(sender == mnuFadeNever) {
				ConfigManager.Config.DebugInfo.RamFadeSpeed = 0;
			}
			ConfigManager.ApplyChanges();
			UpdateFadeOptions();
		}

		private void mnuCustomFadeSpeed_Click(object sender, EventArgs e)
		{
			using(frmFadeSpeed frm = new frmFadeSpeed(ConfigManager.Config.DebugInfo.RamFadeSpeed)) {
				if(frm.ShowDialog() == DialogResult.OK) {
					ConfigManager.Config.DebugInfo.RamFadeSpeed = frm.FadeSpeed;
					ConfigManager.ApplyChanges();
					UpdateFadeOptions();
				}
			}
		}

		private AddressType? GetAddressType()
		{
			switch(_memoryType) {
				case DebugMemoryType.InternalRam: return AddressType.InternalRam;
				case DebugMemoryType.WorkRam: return AddressType.WorkRam;
				case DebugMemoryType.SaveRam: return AddressType.SaveRam;
				case DebugMemoryType.PrgRom: return AddressType.PrgRom;
			}

			return null;
		}

		private void ctrlHexViewer_InitializeContextMenu(object sender, EventArgs evt)
		{
			HexBox hexBox = (HexBox)sender;

			var mnuEditLabel = new ToolStripMenuItem();
			mnuEditLabel.Click += (s, e) => {
				UInt32 address = (UInt32)hexBox.SelectionStart;
				if(this._memoryType == DebugMemoryType.CpuMemory) {
					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType(address, ref info);
					ctrlLabelList.EditLabel((UInt32)info.Address, info.Type);
				} else {
					ctrlLabelList.EditLabel(address, GetAddressType().Value);
				}
			};

			var mnuEditBreakpoint = new ToolStripMenuItem();
			mnuEditBreakpoint.Click += (s, e) => {
				UInt32 startAddress = (UInt32)hexBox.SelectionStart;
				UInt32 endAddress = (UInt32)(hexBox.SelectionStart + (hexBox.SelectionLength == 0 ? 0 : (hexBox.SelectionLength - 1)));
				BreakpointAddressType addressType = startAddress == endAddress ? BreakpointAddressType.SingleAddress : BreakpointAddressType.AddressRange;

				Breakpoint bp = BreakpointManager.GetMatchingBreakpoint(startAddress, endAddress, this._memoryType == DebugMemoryType.PpuMemory);
				if(bp == null) {
					bp = new Breakpoint() { Address = startAddress, StartAddress = startAddress, EndAddress = endAddress, AddressType = addressType, IsAbsoluteAddress = false };
					if(this._memoryType == DebugMemoryType.CpuMemory) {
						bp.BreakOnWrite = bp.BreakOnRead = true;
					} else {
						bp.BreakOnWriteVram = bp.BreakOnReadVram = true;
					}
				}
				BreakpointManager.EditBreakpoint(bp);
			};

			var mnuAddWatch = new ToolStripMenuItem();
			mnuAddWatch.Click += (s, e) => {
				UInt32 startAddress = (UInt32)hexBox.SelectionStart;
				UInt32 endAddress = (UInt32)(hexBox.SelectionStart + (hexBox.SelectionLength == 0 ? 0 : (hexBox.SelectionLength - 1)));
				string[] toAdd = Enumerable.Range((int)startAddress, (int)(endAddress - startAddress + 1)).Select((num) => $"[${num.ToString("X4")}]").ToArray();
				WatchManager.AddWatch(toAdd);
			};

			var mnuFreeze = new ToolStripMenuItem();
			mnuFreeze.Click += (s, e) => {
				UInt32 startAddress = (UInt32)hexBox.SelectionStart;
				UInt32 endAddress = (UInt32)(hexBox.SelectionStart + (hexBox.SelectionLength == 0 ? 0 : (hexBox.SelectionLength - 1)));

				for(UInt32 i = startAddress; i <= endAddress; i++) {
					InteropEmu.DebugSetFreezeState((UInt16)i, (bool)mnuFreeze.Tag);
				}
			};

			hexBox.ContextMenuStrip.Opening += (s, e) => {
				UInt32 startAddress = (UInt32)hexBox.SelectionStart;
				UInt32 endAddress = (UInt32)(hexBox.SelectionStart + (hexBox.SelectionLength == 0 ? 0 : (hexBox.SelectionLength - 1)));

				string address = "$" + startAddress.ToString("X4");
				string addressRange;
				if(startAddress != endAddress) {
					addressRange = "$" + startAddress.ToString("X4") + "-$" + endAddress.ToString("X4");
				} else {
					addressRange = address;
				}

				mnuEditLabel.Text = $"Edit Label ({address})";
				mnuEditBreakpoint.Text = $"Edit Breakpoint ({addressRange})";
				mnuAddWatch.Text = $"Add to Watch ({addressRange})";

				if(this._memoryType == DebugMemoryType.CpuMemory) {
					bool[] freezeState = InteropEmu.DebugGetFreezeState((UInt16)startAddress, (UInt16)(endAddress - startAddress + 1));
					if(freezeState.All((frozen) => frozen)) {
						mnuFreeze.Text = $"Unfreeze ({addressRange})";
						mnuFreeze.Tag = false;
					} else {
						mnuFreeze.Text = $"Freeze ({addressRange})";
						mnuFreeze.Tag = true;
					}
				} else {
					mnuFreeze.Text = $"Freeze";
					mnuFreeze.Tag = false;
				}

				bool disableEditLabel = false;
				if(this._memoryType == DebugMemoryType.CpuMemory) {
					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType(startAddress, ref info);
					disableEditLabel = info.Address == -1;
				}

				mnuEditLabel.Enabled = !disableEditLabel && (this._memoryType == DebugMemoryType.CpuMemory || this.GetAddressType().HasValue);
				mnuEditBreakpoint.Enabled = (this._memoryType == DebugMemoryType.CpuMemory || this._memoryType == DebugMemoryType.PpuMemory) && DebugWindowManager.GetDebugger() != null;
				mnuAddWatch.Enabled = this._memoryType == DebugMemoryType.CpuMemory;
				mnuFreeze.Enabled = this._memoryType == DebugMemoryType.CpuMemory;
			};

			hexBox.ContextMenuStrip.Items.Insert(0, new ToolStripSeparator());
			hexBox.ContextMenuStrip.Items.Insert(0, mnuFreeze);
			hexBox.ContextMenuStrip.Items.Insert(0, mnuEditLabel);
			hexBox.ContextMenuStrip.Items.Insert(0, mnuEditBreakpoint);
			hexBox.ContextMenuStrip.Items.Insert(0, mnuAddWatch);
		}

		private void mnuColorProviderOptions_Click(object sender, EventArgs e)
		{
			this.UpdateConfig();
			this.UpdateByteColorProvider();
		}
	}
}
