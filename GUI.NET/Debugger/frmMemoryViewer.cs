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
		private DebugWorkspace _previousWorkspace;
		private bool _updating = false;
		private DateTime _lastUpdate = DateTime.MinValue;
		private TabPage _selectedTab;

		public frmMemoryViewer()
		{
			InitializeComponent();

			this._selectedTab = this.tabMain.SelectedTab;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.mnuAutoRefresh.Checked = ConfigManager.Config.DebugInfo.RamAutoRefresh;
			UpdateRefreshSpeedMenu();

			this.mnuIgnoreRedundantWrites.Checked = ConfigManager.Config.DebugInfo.RamIgnoreRedundantWrites;
			this.UpdateFlags();

			this.mnuShowCharacters.Checked = ConfigManager.Config.DebugInfo.RamShowCharacters;
			this.mnuShowLabelInfoOnMouseOver.Checked = ConfigManager.Config.DebugInfo.RamShowLabelInfo;

			this.ctrlHexViewer.SetFontSize((int)ConfigManager.Config.DebugInfo.RamFontSize);
			
			this.mnuHighlightExecution.Checked = ConfigManager.Config.DebugInfo.RamHighlightExecution;
			this.mnuHightlightReads.Checked = ConfigManager.Config.DebugInfo.RamHighlightReads;
			this.mnuHighlightWrites.Checked = ConfigManager.Config.DebugInfo.RamHighlightWrites;
			this.mnuHideUnusedBytes.Checked = ConfigManager.Config.DebugInfo.RamHideUnusedBytes;
			this.mnuHideReadBytes.Checked = ConfigManager.Config.DebugInfo.RamHideReadBytes;
			this.mnuHideWrittenBytes.Checked = ConfigManager.Config.DebugInfo.RamHideWrittenBytes;
			this.mnuHideExecutedBytes.Checked = ConfigManager.Config.DebugInfo.RamHideExecutedBytes;

			this.mnuHighlightLabelledBytes.Checked = ConfigManager.Config.DebugInfo.RamHighlightLabelledBytes;
			this.mnuHighlightChrDrawnBytes.Checked = ConfigManager.Config.DebugInfo.RamHighlightChrDrawnBytes;
			this.mnuHighlightChrReadBytes.Checked = ConfigManager.Config.DebugInfo.RamHighlightChrReadBytes;
			this.mnuHighlightCodeBytes.Checked = ConfigManager.Config.DebugInfo.RamHighlightCodeBytes;
			this.mnuHighlightDataBytes.Checked = ConfigManager.Config.DebugInfo.RamHighlightDataBytes;
			this.mnuHighlightDmcDataBytes.Checked = ConfigManager.Config.DebugInfo.RamHighlightDmcDataBytes;

			this.UpdateFadeOptions();

			this.InitTblMappings();

			this.ctrlHexViewer.StringViewVisible = mnuShowCharacters.Checked;

			UpdateImportButton();
			InitMemoryTypeDropdown();

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			this.mnuShowCharacters.CheckedChanged += this.mnuShowCharacters_CheckedChanged;
			this.mnuIgnoreRedundantWrites.CheckedChanged += mnuIgnoreRedundantWrites_CheckedChanged;

			if(!ConfigManager.Config.DebugInfo.MemoryViewerSize.IsEmpty) {
				this.Size = ConfigManager.Config.DebugInfo.MemoryViewerSize;
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			UpdateConfig();
			ConfigManager.Config.DebugInfo.MemoryViewerSize = this.WindowState == FormWindowState.Maximized ? this.RestoreBounds.Size : this.Size;
			ConfigManager.ApplyChanges();
			DebugWorkspaceManager.SaveWorkspace();
		}

		private void InitMemoryTypeDropdown()
		{
			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.CpuMemory));
			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.PpuMemory));
			cboMemoryType.Items.Add("-");

			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.PrgRom) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.PrgRom));
			}
			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.WorkRam) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.WorkRam));
			}
			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.SaveRam) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.SaveRam));
			}

			if(cboMemoryType.Items.Count > 3) {
				cboMemoryType.Items.Add("-");
			}

			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRom) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.ChrRom));
			}

			if(InteropEmu.DebugGetMemorySize(DebugMemoryType.ChrRam) > 0) {
				cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.ChrRam));
			}

			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.SpriteMemory));
			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.SecondarySpriteMemory));

			this.cboMemoryType.SelectedIndex = 0;
		}

		private void UpdateFlags()
		{
			if(mnuIgnoreRedundantWrites.Checked) {
				DebugWorkspaceManager.SetFlags(DebuggerFlags.IgnoreRedundantWrites);
			} else {
				DebugWorkspaceManager.ClearFlags(DebuggerFlags.IgnoreRedundantWrites);
			}
		}

		public void ShowAddress(int address)
		{
			tabMain.SelectedTab = tpgMemoryViewer;
			cboMemoryType.SelectedIndex = 0; //Select CPU Memory
			ctrlHexViewer.GoToAddress(address);
		}

		private void InitTblMappings()
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

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.CodeBreak:
					this.BeginInvoke((MethodInvoker)(() => this.RefreshData()));
					break;
				
				case InteropEmu.ConsoleNotificationType.GameReset:
				case InteropEmu.ConsoleNotificationType.GameLoaded:
					this.UpdateFlags();
					break;

				case InteropEmu.ConsoleNotificationType.PpuViewerDisplayFrame:
					int refreshDelay = 90;
					switch(ConfigManager.Config.DebugInfo.RamAutoRefreshSpeed) {
						case RefreshSpeed.Low: refreshDelay= 90; break;
						case RefreshSpeed.Normal: refreshDelay = 32; break;
						case RefreshSpeed.High: refreshDelay = 16; break;
					}

					if(_selectedTab == tpgProfiler) {
						refreshDelay *= 10;
					}

					DateTime now = DateTime.Now;
					if(!_updating && ConfigManager.Config.DebugInfo.RamAutoRefresh && (now - _lastUpdate).Milliseconds >= refreshDelay) {
						_lastUpdate = now;
						_updating = true;
						this.BeginInvoke((Action)(() => {
							this.RefreshData();
							_updating = false;
						}));
					}
					break;
			}
		}

		private void cboMemoryType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._memoryType = this.cboMemoryType.GetEnumValue<DebugMemoryType>();
			this.UpdateImportButton();
			this.RefreshData();
		}

		private void UpdateByteColorProvider()
		{
			switch(this._memoryType) {
				case DebugMemoryType.CpuMemory:
				case DebugMemoryType.PrgRom:
				case DebugMemoryType.WorkRam:
				case DebugMemoryType.SaveRam:
				case DebugMemoryType.InternalRam:
					this.ctrlHexViewer.ByteColorProvider = new ByteColorProvider(
						this._memoryType,
						mnuHighlightExecution.Checked,
						mnuHighlightWrites.Checked,
						mnuHightlightReads.Checked,
						ConfigManager.Config.DebugInfo.RamFadeSpeed,
						mnuHideUnusedBytes.Checked,
						mnuHideReadBytes.Checked,
						mnuHideWrittenBytes.Checked,
						mnuHideExecutedBytes.Checked,
						mnuHighlightDmcDataBytes.Checked,
						mnuHighlightDataBytes.Checked,
						mnuHighlightCodeBytes.Checked,
						mnuHighlightLabelledBytes.Checked
					);
					break;

				case DebugMemoryType.PpuMemory:
				case DebugMemoryType.ChrRom:
					this.ctrlHexViewer.ByteColorProvider = new ChrByteColorProvider(
						this._memoryType,
						mnuHighlightChrDrawnBytes.Checked,
						mnuHighlightChrReadBytes.Checked
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
				this.ctrlHexViewer.SetData(InteropEmu.DebugGetMemoryState(this._memoryType));
			} else if(this.tabMain.SelectedTab == this.tpgProfiler) {
				this.ctrlProfiler.RefreshData();
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

			ConfigManager.Config.DebugInfo.RamIgnoreRedundantWrites = this.mnuIgnoreRedundantWrites.Checked;

			ConfigManager.Config.DebugInfo.RamShowCharacters = this.mnuShowCharacters.Checked;
			ConfigManager.Config.DebugInfo.RamShowLabelInfo = this.mnuShowLabelInfoOnMouseOver.Checked;
			ConfigManager.Config.DebugInfo.RamFontSize = this.ctrlHexViewer.HexFont.Size;

			ConfigManager.Config.DebugInfo.RamHighlightExecution = this.mnuHighlightExecution.Checked;
			ConfigManager.Config.DebugInfo.RamHighlightReads = this.mnuHightlightReads.Checked;
			ConfigManager.Config.DebugInfo.RamHighlightWrites = this.mnuHighlightWrites.Checked;
			ConfigManager.Config.DebugInfo.RamHideUnusedBytes = this.mnuHideUnusedBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHideReadBytes = this.mnuHideReadBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHideWrittenBytes = this.mnuHideWrittenBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHideExecutedBytes = this.mnuHideExecutedBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHideExecutedBytes = this.mnuHideExecutedBytes.Checked;

			ConfigManager.Config.DebugInfo.RamHighlightLabelledBytes = this.mnuHighlightLabelledBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHighlightChrDrawnBytes = this.mnuHighlightChrDrawnBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHighlightChrReadBytes = this.mnuHighlightChrReadBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHighlightCodeBytes = this.mnuHighlightCodeBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHighlightDataBytes = this.mnuHighlightDataBytes.Checked;
			ConfigManager.Config.DebugInfo.RamHighlightDmcDataBytes = this.mnuHighlightDmcDataBytes.Checked;

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

		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			_selectedTab = this.tabMain.SelectedTab;
			this.RefreshData();
		}

		private void ctrlHexViewer_RequiredWidthChanged(object sender, EventArgs e)
		{
			this.Size = new Size(this.ctrlHexViewer.RequiredWidth + 20, this.Height);
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

		private void mnuIgnoreRedundantWrites_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateFlags();
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

				Breakpoint bp = BreakpointManager.GetMatchingBreakpoint(startAddress, endAddress, this._memoryType);
				if(bp == null) {
					bp = new Breakpoint() { Address = startAddress, MemoryType = this._memoryType, StartAddress = startAddress, EndAddress = endAddress, AddressType = addressType, BreakOnWrite = true, BreakOnRead = true };
					if(bp.IsCpuBreakpoint) {
						bp.BreakOnExec = true;
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

			var mnuMarkSelectionAs = new ToolStripMenuItem();
			var mnuMarkAsCode = new ToolStripMenuItem();
			mnuMarkAsCode.Text = "Verified Code";
			mnuMarkAsCode.Click += (s, e) => {
				int startAddress = (int)hexBox.SelectionStart;
				int endAddress = (int)(hexBox.SelectionStart + (hexBox.SelectionLength == 0 ? 0 : (hexBox.SelectionLength - 1)));
				this.MarkSelectionAs(startAddress, endAddress, CdlPrgFlags.Code);
			};
			var mnuMarkAsData = new ToolStripMenuItem();
			mnuMarkAsData.Text = "Verified Data";
			mnuMarkAsData.Click += (s, e) => {
				int startAddress = (int)hexBox.SelectionStart;
				int endAddress = (int)(hexBox.SelectionStart + (hexBox.SelectionLength == 0 ? 0 : (hexBox.SelectionLength - 1)));
				this.MarkSelectionAs(startAddress, endAddress, CdlPrgFlags.Data);
			};
			var mnuMarkAsUnidentifiedData = new ToolStripMenuItem();
			mnuMarkAsUnidentifiedData.Text = "Unidentified Code/Data";
			mnuMarkAsUnidentifiedData.Click += (s, e) => {
				int startAddress = (int)hexBox.SelectionStart;
				int endAddress = (int)(hexBox.SelectionStart + (hexBox.SelectionLength == 0 ? 0 : (hexBox.SelectionLength - 1)));
				this.MarkSelectionAs(startAddress, endAddress, CdlPrgFlags.None);
			};

			mnuMarkSelectionAs.DropDownItems.Add(mnuMarkAsCode);
			mnuMarkSelectionAs.DropDownItems.Add(mnuMarkAsData);
			mnuMarkSelectionAs.DropDownItems.Add(mnuMarkAsUnidentifiedData);

			var mnuFreeze = new ToolStripMenuItem();
			mnuFreeze.Click += (s, e) => {
				UInt32 startAddress = (UInt32)hexBox.SelectionStart;
				UInt32 endAddress = (UInt32)(hexBox.SelectionStart + (hexBox.SelectionLength == 0 ? 0 : (hexBox.SelectionLength - 1)));

				for(UInt32 i = startAddress; i <= endAddress; i++) {
					InteropEmu.DebugSetFreezeState((UInt16)i, true);
				}
			};

			var mnuUnfreeze = new ToolStripMenuItem();
			mnuUnfreeze.Click += (s, e) => {
				UInt32 startAddress = (UInt32)hexBox.SelectionStart;
				UInt32 endAddress = (UInt32)(hexBox.SelectionStart + (hexBox.SelectionLength == 0 ? 0 : (hexBox.SelectionLength - 1)));

				for(UInt32 i = startAddress; i <= endAddress; i++) {
					InteropEmu.DebugSetFreezeState((UInt16)i, false);
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
					mnuFreeze.Enabled = !freezeState.All((frozen) => frozen);
					mnuUnfreeze.Enabled = freezeState.Any((frozen) => frozen);
					mnuFreeze.Text = $"Freeze ({addressRange})";
					mnuUnfreeze.Text = $"Unfreeze ({addressRange})";
				} else {
					mnuFreeze.Text = $"Freeze";
					mnuUnfreeze.Text = $"Unfreeze";
					mnuFreeze.Enabled = false;
					mnuUnfreeze.Enabled = false;
				}

				if(this._memoryType == DebugMemoryType.CpuMemory) {
					int absStart = InteropEmu.DebugGetAbsoluteAddress(startAddress);
					int absEnd = InteropEmu.DebugGetAbsoluteAddress(endAddress);

					if(absStart >= 0 && absEnd >= 0 && absStart <= absEnd) {
						mnuMarkSelectionAs.Text = "Mark selection as... (" + addressRange + ")";
						mnuMarkSelectionAs.Enabled = true;
					} else {
						mnuMarkSelectionAs.Text = "Mark selection as...";
						mnuMarkSelectionAs.Enabled = false;
					}
				} else if(this._memoryType == DebugMemoryType.PrgRom) {
					mnuMarkSelectionAs.Text = "Mark selection as... (" + addressRange + ")";
					mnuMarkSelectionAs.Enabled = true;
				} else {
					mnuMarkSelectionAs.Text = "Mark selection as...";
					mnuMarkSelectionAs.Enabled = false;
				}

				bool disableEditLabel = false;
				if(this._memoryType == DebugMemoryType.CpuMemory) {
					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType(startAddress, ref info);
					disableEditLabel = info.Address == -1;
				}

				mnuEditLabel.Enabled = !disableEditLabel && (this._memoryType == DebugMemoryType.CpuMemory || this.GetAddressType().HasValue);
				mnuEditBreakpoint.Enabled = DebugWindowManager.GetDebugger() != null && (
					this._memoryType == DebugMemoryType.CpuMemory ||
					this._memoryType == DebugMemoryType.PpuMemory ||
					this._memoryType == DebugMemoryType.PrgRom ||
					this._memoryType == DebugMemoryType.WorkRam ||
					this._memoryType == DebugMemoryType.SaveRam ||
					this._memoryType == DebugMemoryType.ChrRam ||
					this._memoryType == DebugMemoryType.ChrRom ||
					this._memoryType == DebugMemoryType.PaletteMemory
				);

				mnuAddWatch.Enabled = this._memoryType == DebugMemoryType.CpuMemory;
			};

			hexBox.ContextMenuStrip.Items.Insert(0, new ToolStripSeparator());
			hexBox.ContextMenuStrip.Items.Insert(0, mnuFreeze);
			hexBox.ContextMenuStrip.Items.Insert(0, mnuUnfreeze);
			hexBox.ContextMenuStrip.Items.Insert(0, new ToolStripSeparator());
			hexBox.ContextMenuStrip.Items.Insert(0, mnuEditLabel);
			hexBox.ContextMenuStrip.Items.Insert(0, mnuEditBreakpoint);
			hexBox.ContextMenuStrip.Items.Insert(0, mnuAddWatch);
			hexBox.ContextMenuStrip.Items.Insert(0, new ToolStripSeparator());
			hexBox.ContextMenuStrip.Items.Insert(0, mnuMarkSelectionAs);
		}

		private void MarkSelectionAs(int start, int end, CdlPrgFlags type)
		{
			if(_memoryType == DebugMemoryType.CpuMemory) {
				start = InteropEmu.DebugGetAbsoluteAddress((UInt32)start);
				end = InteropEmu.DebugGetAbsoluteAddress((UInt32)end);
			}

			if(start >= 0 && end >= 0 && start <= end) {
				InteropEmu.DebugMarkPrgBytesAs((UInt32)start, (UInt32)end, type);
				frmDebugger debugger = DebugWindowManager.GetDebugger();
				if(debugger != null) {
					debugger.UpdateDebugger(false, false);
				}
			}
		}

		private void mnuColorProviderOptions_Click(object sender, EventArgs e)
		{
			this.UpdateConfig();
			this.UpdateByteColorProvider();
		}

		private void mnuConfigureColors_Click(object sender, EventArgs e)
		{
			using(frmMemoryViewerColors frm = new frmMemoryViewerColors()) {
				if(frm.ShowDialog(this, this) == DialogResult.OK) {
					this.RefreshData();
				}
			}
		}

		private frmCodeTooltip _tooltip = null;
		private CodeLabel _lastLabelTooltip = null;
		private int _lastTooltipAddress = -1;
		private void ctrlHexViewer_ByteMouseHover(int address)
		{
			if(address < 0 || !mnuShowLabelInfoOnMouseOver.Checked) {
				if(_tooltip != null) {
					_tooltip.Close();
					_lastLabelTooltip = null;
				}
				return;
			}

			if(_lastTooltipAddress == address) {
				return;
			}

			_lastTooltipAddress = address;

			CodeLabel label = null;
			switch(_memoryType) {
				case DebugMemoryType.CpuMemory:
					AddressTypeInfo info = new AddressTypeInfo();
					InteropEmu.DebugGetAbsoluteAddressAndType((UInt32)address, ref info);
					if(info.Address >= 0) {
						label = LabelManager.GetLabel((UInt32)info.Address, info.Type);
					} 
					if(label == null) {
						label = LabelManager.GetLabel((UInt32)address, AddressType.Register);
					}
					break;

				case DebugMemoryType.InternalRam:
					label = LabelManager.GetLabel((UInt32)address, AddressType.InternalRam);
					break;

				case DebugMemoryType.WorkRam:
					label = LabelManager.GetLabel((UInt32)address, AddressType.WorkRam);
					break;

				case DebugMemoryType.SaveRam:
					label = LabelManager.GetLabel((UInt32)address, AddressType.SaveRam);
					break;

				case DebugMemoryType.PrgRom:
					label = LabelManager.GetLabel((UInt32)address, AddressType.PrgRom);
					break;
			}

			if(label != null) {
				if(_lastLabelTooltip != label) {
					if(_tooltip != null) {
						_tooltip.Close();
					}

					Dictionary<string, string> values = new Dictionary<string, string>();
					if(!string.IsNullOrWhiteSpace(label.Label)) {
						values["Label"] = label.Label;
					}
					values["Address"] = "$" + label.Address.ToString("X4");
					values["Address Type"] = label.AddressType.ToString();
					if(!string.IsNullOrWhiteSpace(label.Comment)) {
						values["Comment"] = label.Comment;
					}
					_tooltip = new frmCodeTooltip(values);
					_tooltip.Left = Cursor.Position.X + 10;
					_tooltip.Top = Cursor.Position.Y + 10;
					_tooltip.Show(this);

					_lastLabelTooltip = label;
				}
			} else {
				if(_tooltip != null) {
					_tooltip.Close();
					_lastLabelTooltip = null;
				}
			}
		}

		private void mnuAutoRefreshSpeed_Click(object sender, EventArgs e)
		{
			if(sender == mnuAutoRefreshLow) {
				ConfigManager.Config.DebugInfo.RamAutoRefreshSpeed = RefreshSpeed.Low;
			} else if(sender == mnuAutoRefreshNormal) {
				ConfigManager.Config.DebugInfo.RamAutoRefreshSpeed = RefreshSpeed.Normal;
			} else if(sender == mnuAutoRefreshHigh) {
				ConfigManager.Config.DebugInfo.RamAutoRefreshSpeed = RefreshSpeed.High;
			}
			ConfigManager.ApplyChanges();

			UpdateRefreshSpeedMenu();
		}

		private void UpdateRefreshSpeedMenu()
		{
			mnuAutoRefreshLow.Checked = ConfigManager.Config.DebugInfo.RamAutoRefreshSpeed == RefreshSpeed.Low;
			mnuAutoRefreshNormal.Checked = ConfigManager.Config.DebugInfo.RamAutoRefreshSpeed == RefreshSpeed.Normal;
			mnuAutoRefreshHigh.Checked = ConfigManager.Config.DebugInfo.RamAutoRefreshSpeed == RefreshSpeed.High;
		}
	}
}
