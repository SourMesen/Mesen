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
		private bool _formClosed;

		public frmMemoryViewer()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this._selectedTab = this.tabMain.SelectedTab;

			DebugInfo config = ConfigManager.Config.DebugInfo;

			this.mnuAutoRefresh.Checked = config.RamAutoRefresh;
			this.mnuHighDensityMode.Checked = config.RamHighDensityTextMode;
			this.mnuByteEditingMode.Checked = config.RamByteEditingMode;
			this.mnuEnablePerByteNavigation.Checked = config.RamEnablePerByteNavigation;
			UpdateRefreshSpeedMenu();

			this.mnuIgnoreRedundantWrites.Checked = config.RamIgnoreRedundantWrites;
			this.UpdateFlags();

			this.mnuShowCharacters.Checked = config.RamShowCharacters;
			this.mnuShowLabelInfoOnMouseOver.Checked = config.RamShowLabelInfo;

			this.ctrlHexViewer.TextZoom = config.RamTextZoom;
			this.ctrlHexViewer.BaseFont = new Font(config.RamFontFamily, config.RamFontSize, config.RamFontStyle);

			this.ctrlMemoryAccessCounters.BaseFont = new Font(config.RamFontFamily, config.RamFontSize, config.RamFontStyle);
			this.ctrlMemoryAccessCounters.TextZoom = config.RamTextZoom;

			this.mnuHighlightExecution.Checked = config.RamHighlightExecution;
			this.mnuHightlightReads.Checked = config.RamHighlightReads;
			this.mnuHighlightWrites.Checked = config.RamHighlightWrites;
			this.mnuHideUnusedBytes.Checked = config.RamHideUnusedBytes;
			this.mnuHideReadBytes.Checked = config.RamHideReadBytes;
			this.mnuHideWrittenBytes.Checked = config.RamHideWrittenBytes;
			this.mnuHideExecutedBytes.Checked = config.RamHideExecutedBytes;

			this.mnuHighlightLabelledBytes.Checked = config.RamHighlightLabelledBytes;
			this.mnuHighlightChrDrawnBytes.Checked = config.RamHighlightChrDrawnBytes;
			this.mnuHighlightChrReadBytes.Checked = config.RamHighlightChrReadBytes;
			this.mnuHighlightCodeBytes.Checked = config.RamHighlightCodeBytes;
			this.mnuHighlightDataBytes.Checked = config.RamHighlightDataBytes;
			this.mnuHighlightDmcDataBytes.Checked = config.RamHighlightDmcDataBytes;

			this.UpdateFadeOptions();

			this.InitTblMappings();

			this.ctrlHexViewer.StringViewVisible = mnuShowCharacters.Checked;
			this.ctrlHexViewer.MemoryViewer = this;

			UpdateImportButton();
			InitMemoryTypeDropdown(true);

			_notifListener = new InteropEmu.NotificationListener(ConfigManager.Config.DebugInfo.DebugConsoleId);
			_notifListener.OnNotification += _notifListener_OnNotification;

			this.mnuShowCharacters.CheckedChanged += this.mnuShowCharacters_CheckedChanged;
			this.mnuIgnoreRedundantWrites.CheckedChanged += mnuIgnoreRedundantWrites_CheckedChanged;

			if(!ConfigManager.Config.DebugInfo.MemoryViewerSize.IsEmpty) {
				this.StartPosition = FormStartPosition.Manual;
				this.Size = ConfigManager.Config.DebugInfo.MemoryViewerSize;
				this.Location = ConfigManager.Config.DebugInfo.MemoryViewerLocation;
			}

			this.InitShortcuts();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			UpdateConfig();
			ConfigManager.Config.DebugInfo.RamFontFamily = ctrlHexViewer.BaseFont.FontFamily.Name;
			ConfigManager.Config.DebugInfo.RamFontStyle = ctrlHexViewer.BaseFont.Style;
			ConfigManager.Config.DebugInfo.RamFontSize = ctrlHexViewer.BaseFont.Size;
			ConfigManager.Config.DebugInfo.MemoryViewerSize = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Size : this.Size;
			ConfigManager.Config.DebugInfo.MemoryViewerLocation = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Location : this.Location;
			ConfigManager.Config.DebugInfo.RamMemoryType = cboMemoryType.GetEnumValue<DebugMemoryType>();
			ConfigManager.ApplyChanges();
			DebugWorkspaceManager.SaveWorkspace();

			if(this._notifListener != null) {
				this._notifListener.Dispose();
				this._notifListener = null;
			}

			_formClosed = true;
		}

		private void InitShortcuts()
		{
			mnuIncreaseFontSize.InitShortcut(this, nameof(DebuggerShortcutsConfig.IncreaseFontSize));
			mnuDecreaseFontSize.InitShortcut(this, nameof(DebuggerShortcutsConfig.DecreaseFontSize));
			mnuResetFontSize.InitShortcut(this, nameof(DebuggerShortcutsConfig.ResetFontSize));

			mnuImport.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_Import));
			mnuExport.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_Export));

			mnuRefresh.InitShortcut(this, nameof(DebuggerShortcutsConfig.Refresh));

			mnuGoTo.InitShortcut(this, nameof(DebuggerShortcutsConfig.GoTo));
			mnuFind.InitShortcut(this, nameof(DebuggerShortcutsConfig.Find));
			mnuFindNext.InitShortcut(this, nameof(DebuggerShortcutsConfig.FindNext));
			mnuFindPrev.InitShortcut(this, nameof(DebuggerShortcutsConfig.FindPrev));
		}

		private void InitMemoryTypeDropdown(bool forStartup)
		{
			cboMemoryType.SelectedIndexChanged -= this.cboMemoryType_SelectedIndexChanged;

			DebugMemoryType originalValue = forStartup ? ConfigManager.Config.DebugInfo.RamMemoryType : cboMemoryType.GetEnumValue<DebugMemoryType>();

			cboMemoryType.BeginUpdate();
			cboMemoryType.Items.Clear();

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

			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.PaletteMemory));
			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.SpriteMemory));
			cboMemoryType.Items.Add(ResourceHelper.GetEnumText(DebugMemoryType.SecondarySpriteMemory));

			cboMemoryType.SelectedIndex = 0;
			cboMemoryType.SetEnumValue(originalValue);
			cboMemoryType.SelectedIndexChanged += this.cboMemoryType_SelectedIndexChanged;

			cboMemoryType.EndUpdate();
			UpdateMemoryType();
		}

		private void UpdateFlags()
		{
			if(mnuIgnoreRedundantWrites.Checked) {
				DebugWorkspaceManager.SetFlags(DebuggerFlags.IgnoreRedundantWrites);
			} else {
				DebugWorkspaceManager.ClearFlags(DebuggerFlags.IgnoreRedundantWrites);
			}
		}

		public void ShowAddress(int address, DebugMemoryType memoryType)
		{
			tabMain.SelectedTab = tpgMemoryViewer;
			cboMemoryType.SetEnumValue(memoryType);
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
					this.BeginInvoke((Action)(() => {
						if(_formClosed) {
							return;
						}
						this.InitMemoryTypeDropdown(false);
						ctrlMemoryAccessCounters.InitMemoryTypeDropdown();
					}));
					this.UpdateFlags();
					break;

				case InteropEmu.ConsoleNotificationType.PpuFrameDone:
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
			UpdateMemoryType();
		}

		private void UpdateMemoryType()
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
			if(_formClosed) {
				return;
			}

			if(DebugWorkspaceManager.GetWorkspace() != this._previousWorkspace) {
				this.InitTblMappings();
				_previousWorkspace = DebugWorkspaceManager.GetWorkspace();
			}

			if(this.tabMain.SelectedTab == this.tpgAccessCounters) {
				this.ctrlMemoryAccessCounters.RefreshData();
			} else if(this.tabMain.SelectedTab == this.tpgMemoryViewer) {
				this.UpdateByteColorProvider();
				this.ctrlHexViewer.RefreshData(_memoryType);
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
			this.ctrlHexViewer.TextZoom += 10;
			this.ctrlMemoryAccessCounters.TextZoom += 10;
			this.UpdateConfig();
		}

		private void mnuDecreaseFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.TextZoom -= 10;
			this.ctrlMemoryAccessCounters.TextZoom -= 10;
			this.UpdateConfig();
		}

		private void mnuResetFontSize_Click(object sender, EventArgs e)
		{
			this.ctrlHexViewer.TextZoom = 100;
			this.ctrlMemoryAccessCounters.TextZoom = 100;
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

			ConfigManager.Config.DebugInfo.RamTextZoom = this.ctrlHexViewer.TextZoom;
			ConfigManager.Config.DebugInfo.RamFontFamily = this.ctrlHexViewer.BaseFont.FontFamily.Name;
			ConfigManager.Config.DebugInfo.RamFontSize = this.ctrlHexViewer.BaseFont.Size;
			ConfigManager.Config.DebugInfo.RamFontStyle = this.ctrlHexViewer.BaseFont.Style;

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
					mnuImport.Enabled = true;
					break;

				default:
					mnuImport.Enabled = false;
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
		private void ctrlHexViewer_ByteMouseHover(int address, Point position)
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
					InteropEmu.DebugGetAbsoluteAddressAndType((UInt32)address, info);
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

					_tooltip = new frmCodeTooltip(this, values);
					_tooltip.FormClosed += (s, evt) => { _tooltip = null; };
					_tooltip.SetFormLocation(new Point(position.X, position.Y), ctrlHexViewer);
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

		private void mnuHighDensityMode_CheckedChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.RamHighDensityTextMode = mnuHighDensityMode.Checked;
			ConfigManager.ApplyChanges();
			ctrlHexViewer.HighDensityMode = ConfigManager.Config.DebugInfo.RamHighDensityTextMode;
		}

		private void mnuEnablePerByteNavigation_CheckedChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.RamEnablePerByteNavigation = mnuEnablePerByteNavigation.Checked;
			ConfigManager.ApplyChanges();
			ctrlHexViewer.EnablePerByteNavigation = ConfigManager.Config.DebugInfo.RamEnablePerByteNavigation;
		}

		private void mnuSelectFont_Click(object sender, EventArgs e)
		{
			ctrlHexViewer.BaseFont = FontDialogHelper.SelectFont(ctrlHexViewer.BaseFont);
			ctrlMemoryAccessCounters.BaseFont = ctrlHexViewer.BaseFont;
		}

		private void mnuByteEditingMode_CheckedChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.RamByteEditingMode = mnuByteEditingMode.Checked;
			ConfigManager.ApplyChanges();
			ctrlHexViewer.ByteEditingMode = ConfigManager.Config.DebugInfo.RamByteEditingMode;
		}
	}
}
