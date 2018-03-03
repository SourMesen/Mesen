using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Debugger.Controls;
using Mesen.GUI.Forms;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger
{
	public partial class frmDebugger : BaseForm
	{
		private bool _debuggerInitialized = false;
		private bool _firstBreak = true;
		private bool _wasPaused = false;
		private int _previousCycle = 0;

		private InteropEmu.NotificationListener _notifListener;
		private ctrlDebuggerCode _lastCodeWindow;
		private Size _minimumSize;

		public frmDebugger()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);

			_minimumSize = this.MinimumSize;
			 
			if(Program.IsMono) {
				//This doesn't work in Mono (menu is blank) - hide it for now
				mnuCode.Visible = false;
			}

			_wasPaused = InteropEmu.IsPaused();
			bool debuggerAlreadyRunning = InteropEmu.DebugIsDebuggerRunning();

			ctrlConsoleStatus.OnStateChanged += ctrlConsoleStatus_OnStateChanged;
			LabelManager.OnLabelUpdated += LabelManager_OnLabelUpdated;
			BreakpointManager.BreakpointsChanged += BreakpointManager_BreakpointsChanged;
			ctrlProfiler.OnFunctionSelected += ctrlProfiler_OnFunctionSelected;

			Font font = new Font(ConfigManager.Config.DebugInfo.FontFamily, ConfigManager.Config.DebugInfo.FontSize, ConfigManager.Config.DebugInfo.FontStyle);
			ctrlDebuggerCode.BaseFont = font;
			ctrlDebuggerCodeSplit.BaseFont = font;

			this.InitToolbar();

			this.UpdateWorkspace();
			this.AutoLoadCdlFiles();
			this.AutoLoadDbgFiles(true);

			this.mnuSplitView.Checked = ConfigManager.Config.DebugInfo.SplitView;
			this.mnuPpuPartialDraw.Checked = ConfigManager.Config.DebugInfo.PpuPartialDraw;
			this.mnuPpuShowPreviousFrame.Checked = ConfigManager.Config.DebugInfo.PpuShowPreviousFrame;
			this.mnuHidePauseIcon.Checked = ConfigManager.Config.DebugInfo.HidePauseIcon;
			this.mnuShowEffectiveAddresses.Checked = ConfigManager.Config.DebugInfo.ShowEffectiveAddresses;
			this.mnuShowCodePreview.Checked = ConfigManager.Config.DebugInfo.ShowCodePreview;
			this.mnuShowToolbar.Checked = ConfigManager.Config.DebugInfo.ShowToolbar;
			this.mnuShowCpuMemoryMapping.Checked = ConfigManager.Config.DebugInfo.ShowCpuMemoryMapping;
			this.mnuShowPpuMemoryMapping.Checked = ConfigManager.Config.DebugInfo.ShowPpuMemoryMapping;
			this.mnuAutoLoadDbgFiles.Checked = ConfigManager.Config.DebugInfo.AutoLoadDbgFiles;
			this.mnuAutoLoadCdlFiles.Checked = ConfigManager.Config.DebugInfo.AutoLoadCdlFiles;
			this.mnuBreakOnReset.Checked = ConfigManager.Config.DebugInfo.BreakOnReset;
			this.mnuBreakOnOpen.Checked = ConfigManager.Config.DebugInfo.BreakOnOpen;
			this.mnuBreakOnUnofficialOpcodes.Checked = ConfigManager.Config.DebugInfo.BreakOnUnofficialOpcodes;
			this.mnuBreakOnBrk.Checked = ConfigManager.Config.DebugInfo.BreakOnBrk;
			this.mnuBreakOnCrash.Checked = ConfigManager.Config.DebugInfo.BreakOnCrash;
			this.mnuBreakOnDebuggerFocus.Checked = ConfigManager.Config.DebugInfo.BreakOnDebuggerFocus;
			this.mnuBringToFrontOnBreak.Checked = ConfigManager.Config.DebugInfo.BringToFrontOnBreak;
			this.mnuBringToFrontOnPause.Checked = ConfigManager.Config.DebugInfo.BringToFrontOnPause;
			this.mnuDisplayOpCodesInLowerCase.Checked = ConfigManager.Config.DebugInfo.DisplayOpCodesInLowerCase;

			this.mnuDisassembleVerifiedData.Checked = ConfigManager.Config.DebugInfo.DisassembleVerifiedData;
			this.mnuDisassembleUnidentifiedData.Checked = ConfigManager.Config.DebugInfo.DisassembleUnidentifiedData;

			this.mnuShowVerifiedData.Checked = ConfigManager.Config.DebugInfo.ShowVerifiedData;
			this.mnuShowUnidentifiedData.Checked = ConfigManager.Config.DebugInfo.ShowUnidentifiedData;

			this.mnuRefreshWatchWhileRunning.Checked = ConfigManager.Config.DebugInfo.RefreshWatchWhileRunning;
			this.mnuShowMemoryValues.Checked = ConfigManager.Config.DebugInfo.ShowMemoryValuesInCodeWindow;
			ctrlDebuggerCode.ShowMemoryValues = mnuShowMemoryValues.Checked;
			ctrlDebuggerCodeSplit.ShowMemoryValues = mnuShowMemoryValues.Checked;

			if(ConfigManager.Config.DebugInfo.WindowWidth > -1) {
				this.Width = ConfigManager.Config.DebugInfo.WindowWidth;
				this.Height = ConfigManager.Config.DebugInfo.WindowHeight;
			}

			tsToolbar.Visible = mnuShowToolbar.Checked;
			ctrlCpuMemoryMapping.Visible = mnuShowCpuMemoryMapping.Checked;
			ctrlPpuMemoryMapping.Visible = mnuShowPpuMemoryMapping.Checked;

			if(ConfigManager.Config.DebugInfo.LeftPanelWidth > 0) {
				this.ctrlSplitContainerTop.SplitterDistance = ConfigManager.Config.DebugInfo.LeftPanelWidth;
			}
			if(ConfigManager.Config.DebugInfo.TopPanelHeight > 0) {
				this.splitContainer.SplitterDistance = ConfigManager.Config.DebugInfo.TopPanelHeight;
			}

			if(!ConfigManager.Config.DebugInfo.ShowRightPanel) {
				ctrlSplitContainerTop.CollapsePanel();
			} else {
				mnuShowFunctionLabelLists.Checked = true;
			}

			if(!ConfigManager.Config.DebugInfo.ShowBottomPanel) {
				splitContainer.CollapsePanel();
			} else {
				mnuShowBottomPanel.Checked = true;
			}

			_lastCodeWindow = ctrlDebuggerCode;

			this.ctrlDebuggerCode.SetConfig(ConfigManager.Config.DebugInfo.LeftView);
			this.ctrlDebuggerCodeSplit.SetConfig(ConfigManager.Config.DebugInfo.RightView);

			this.toolTip.SetToolTip(this.picWatchHelp, frmBreakpoint.GetConditionTooltip(true));

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			InteropEmu.DebugInitialize();

			_debuggerInitialized = true;

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			_previousCycle = state.CPU.CycleCount;

			//Pause a few frames later to give the debugger a chance to disassemble some code
			_firstBreak = true;
			if(!debuggerAlreadyRunning) {
				InteropEmu.SetFlag(EmulationFlags.ForceMaxSpeed, true);
				InteropEmu.DebugStep((uint)(_wasPaused ? 1 : 5000));
			} else {
				//Break once to show code and then resume execution
				InteropEmu.DebugStep(1);
			}
			InteropEmu.SetFlag(EmulationFlags.Paused, false);

			UpdateDebuggerFlags();
			UpdateCdlRatios();
			UpdateFileOptions();

			tmrCdlRatios.Start();
		}

		private void InitToolbar()
		{
			AddItemsToToolbar(
				mnuSaveRom, mnuRevertChanges, null,
				mnuImportLabels, mnuExportLabels, null,
				mnuContinue, mnuBreak, null,
				mnuStepInto, mnuStepOver, mnuStepOut, mnuStepBack, null,
				mnuRunPpuCycle, mnuRunScanline, mnuRunOneFrame, null,
				mnuToggleBreakpoint, mnuDisableEnableBreakpoint, null,
				mnuFind, mnuFindPrev, mnuFindNext, null,
				mnuApuViewer, mnuAssembler, mnuMemoryViewer, mnuEventViewer, mnuPpuViewer, mnuScriptWindow, mnuTraceLogger, null,
				mnuEditHeader, null
			);
			AddItemToToolbar(mnuShowVerifiedData, "Show Verified Data");
			AddItemToToolbar(mnuShowUnidentifiedData, "Show Unidentified Code/Data");
			AddItemsToToolbar(null, mnuBreakIn);
		}

		private void AddItemToToolbar(ToolStripMenuItem item, string caption = null)
		{
			if(item == null) {
				tsToolbar.Items.Add("-");
			} else {
				ToolStripButton newItem = new ToolStripButton(item.Image);
				if(item.Image == null) {
					newItem.Text = item.Text;
				}
				newItem.ToolTipText = (caption ?? item.Text) + (item.ShortcutKeys != Keys.None ? $" ({new KeysConverter().ConvertToString(item.ShortcutKeys)})" : "");
				newItem.Click += (s, e) => item.PerformClick();
				newItem.Checked = item.Checked;
				newItem.Enabled = item.Enabled;
				item.EnabledChanged += (s, e) => newItem.Enabled = item.Enabled;
				item.CheckedChanged += (s, e) => newItem.Checked = item.Checked;
				item.VisibleChanged += (s, e) => newItem.Visible = item.Visible;
				tsToolbar.Items.Add(newItem);
			}
		}

		private void AddItemsToToolbar(params ToolStripMenuItem[] items)
		{
			foreach(ToolStripMenuItem item in items) {
				AddItemToToolbar(item);
			}
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if(ConfigManager.Config.DebugInfo.BreakOnDebuggerFocus && !InteropEmu.DebugIsExecutionStopped()) {
				InteropEmu.DebugStep(1);
			}
		}

		private void ctrlProfiler_OnFunctionSelected(object sender, EventArgs e)
		{
			int relativeAddress = InteropEmu.DebugGetRelativeAddress((UInt32)sender, AddressType.PrgRom);
			if(relativeAddress >= 0) {
				BringToFront();
				_lastCodeWindow.ScrollToLineNumber(relativeAddress);
			}
		}

		private void mnuFile_DropDownOpening(object sender, EventArgs e)
		{
			UpdateFileOptions();
		}

		private void UpdateFileOptions()
		{
			bool hasChanges = InteropEmu.DebugHasPrgChrChanges();
			RomInfo romInfo = InteropEmu.GetRomInfo();
			mnuSaveRom.Enabled = romInfo.Format == RomFormat.iNes && hasChanges && !romInfo.RomFile.Compressed;
			mnuSaveAsIps.Enabled = romInfo.Format == RomFormat.iNes && hasChanges;
			mnuRevertChanges.Enabled = hasChanges;
			mnuSaveRomAs.Enabled = romInfo.Format == RomFormat.iNes;
			mnuEditHeader.Enabled = romInfo.Format == RomFormat.iNes;

			mnuCdlStripUnusedData.Enabled = romInfo.Format == RomFormat.iNes;
			mnuCdlStripUsedData.Enabled = romInfo.Format == RomFormat.iNes;
		}

		private void AutoLoadCdlFiles()
		{
			if(ConfigManager.Config.DebugInfo.AutoLoadCdlFiles) {
				//This loads CDL files that are next to the rom - useful when developing with a compiler that can produce a CDL file
				RomInfo info = InteropEmu.GetRomInfo();
				string cdlPath = Path.Combine(info.RomFile.Folder, info.GetRomName() + ".cdl");
				if(File.Exists(cdlPath)) {
					if(InteropEmu.DebugLoadCdlFile(cdlPath)) {
						UpdateDebugger(false, false);
					}
				}
			}
		}

		private void AutoLoadDbgFiles(bool silent)
		{
			if(ConfigManager.Config.DebugInfo.AutoLoadDbgFiles) {
				RomInfo info = InteropEmu.GetRomInfo();
				string dbgPath = Path.Combine(info.RomFile.Folder, info.GetRomName() + ".dbg");
				if(File.Exists(dbgPath)) {
					Ld65DbgImporter dbgImporter = new Ld65DbgImporter();
					dbgImporter.Import(dbgPath, silent);
				} else {
					string mlbPath = Path.Combine(info.RomFile.Folder, info.GetRomName() + ".mlb");
					if(File.Exists(mlbPath)) {
						MesenLabelFile.Import(mlbPath, silent);
					}
				}
			}
		}

		private void UpdateWorkspace()
		{
			DebugWorkspaceManager.SaveWorkspace();
			DebugWorkspace workspace = DebugWorkspaceManager.GetWorkspace();

			ctrlLabelList.UpdateLabelList();
			ctrlFunctionList.UpdateFunctionList(true);
			ctrlBreakpoints.RefreshList();
		}

		private void UpdateCdlRatios()
		{
			CdlRatios ratios = new CdlRatios();
			InteropEmu.DebugGetCdlRatios(ref ratios);

			lblPrgAnalysisResult.Text = string.Format("{0:0.00}% (Code: {1:0.00}%, Data: {2:0.00}%, Unknown: {3:0.00}%)", ratios.PrgRatio * 100, ratios.CodeRatio * 100, ratios.DataRatio * 100, (1 - ratios.PrgRatio) * 100);
			if(ratios.ChrRatio >= 0) {
				lblChrAnalysisResult.Text = string.Format("{0:0.00}% (Drawn: {1:0.00}%, Read: {2:0.00}%, Unknown: {3:0.00}%)", ratios.ChrRatio * 100, ratios.ChrDrawnRatio * 100, ratios.ChrReadRatio * 100, (1 - ratios.ChrRatio) * 100);
			} else {
				lblChrAnalysisResult.Text = "N/A (CHR RAM)";
			}
		}

		private void SetFlag(DebuggerFlags flag, bool enabled)
		{
			if(enabled) {
				DebugWorkspaceManager.SetFlags(flag);
			} else {
				DebugWorkspaceManager.ClearFlags(flag);
			}
		}

		private void UpdateDebuggerFlags()
		{
			SetFlag(DebuggerFlags.PpuPartialDraw, mnuPpuPartialDraw.Checked);
			SetFlag(DebuggerFlags.PpuShowPreviousFrame, mnuPpuShowPreviousFrame.Checked);
			SetFlag(DebuggerFlags.ShowEffectiveAddresses, mnuShowEffectiveAddresses.Checked);
			SetFlag(DebuggerFlags.DisplayOpCodesInLowerCase, mnuDisplayOpCodesInLowerCase.Checked);
			SetFlag(DebuggerFlags.DisassembleVerifiedData, mnuDisassembleVerifiedData.Checked);
			SetFlag(DebuggerFlags.DisassembleUnidentifiedData, mnuDisassembleUnidentifiedData.Checked);
			SetFlag(DebuggerFlags.ShowVerifiedData, mnuShowVerifiedData.Checked);
			SetFlag(DebuggerFlags.ShowUnidentifiedData, mnuShowUnidentifiedData.Checked);
			SetFlag(DebuggerFlags.BreakOnUnofficialOpCode, mnuBreakOnUnofficialOpcodes.Checked);
			SetFlag(DebuggerFlags.BreakOnBrk, mnuBreakOnBrk.Checked);
			SetFlag(DebuggerFlags.HidePauseIcon, mnuHidePauseIcon.Checked);
			InteropEmu.SetFlag(EmulationFlags.DebuggerWindowEnabled, true);
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.PpuFrameDone:
					if(ConfigManager.Config.DebugInfo.RefreshWatchWhileRunning) {
						this.BeginInvoke((MethodInvoker)(() => ctrlWatch.UpdateWatch()));
					}
					break;

				case InteropEmu.ConsoleNotificationType.CodeBreak:
					this.BeginInvoke((MethodInvoker)(() => {
						BreakSource source = (BreakSource)e.Parameter.ToInt32();
						bool bringToFront = (
							source == BreakSource.Break && ConfigManager.Config.DebugInfo.BringToFrontOnBreak ||
							source == BreakSource.Pause && ConfigManager.Config.DebugInfo.BringToFrontOnPause
						);
						UpdateDebugger(true, bringToFront);
						mnuContinue.Enabled = true;
						mnuBreak.Enabled = false;
					}));
					BreakpointManager.SetBreakpoints();
					break;

				case InteropEmu.ConsoleNotificationType.GameReset:
				case InteropEmu.ConsoleNotificationType.GameLoaded:
					this.BeginInvoke((MethodInvoker)(() => {
						this.UpdateWorkspace();
						this.AutoLoadCdlFiles();
						this.AutoLoadDbgFiles(true);
						UpdateDebugger(true, false);
						BreakpointManager.SetBreakpoints();

						if(!ConfigManager.Config.DebugInfo.BreakOnReset) {
							ClearActiveStatement();
						}
					}));

					if(ConfigManager.Config.DebugInfo.BreakOnReset) {
						InteropEmu.DebugStep(1);
					}
					break;
			}
		}

		private bool UpdateSplitView()
		{
			if(mnuSplitView.Checked) {
				tlpTop.ColumnStyles[1].SizeType = SizeType.Percent;
				tlpTop.ColumnStyles[0].Width = 50f;
				tlpTop.ColumnStyles[1].Width = 50f;
				this.MinimumSize = new Size(_minimumSize.Width + 250, _minimumSize.Height);
			} else {
				tlpTop.ColumnStyles[1].SizeType = SizeType.Absolute;
				tlpTop.ColumnStyles[1].Width = 0f;
				this.MinimumSize = _minimumSize;
			}
			ctrlDebuggerCodeSplit.Visible = mnuSplitView.Checked;
			return mnuSplitView.Checked;
		}

		private void UpdateVectorAddresses()
		{
			int nmiHandler = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFA) | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFB) << 8);
			int resetHandler = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFC) | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFD) << 8);
			int irqHandler = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFE) | (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFF) << 8);

			mnuGoToNmiHandler.Text = "NMI Handler ($" + nmiHandler.ToString("X4") + ")";
			mnuGoToResetHandler.Text = "Reset Handler ($" + resetHandler.ToString("X4") + ")";
			mnuGoToIrqHandler.Text = "IRQ Handler ($" + irqHandler.ToString("X4") + ")";
		}

		public void UpdateDebugger(bool updateActiveAddress = true, bool bringToFront = true)
		{
			if(!_debuggerInitialized) {
				return;
			}

			ctrlBreakpoints.RefreshListAddresses();
			ctrlLabelList.UpdateLabelListAddresses();
			ctrlFunctionList.UpdateFunctionList(false);
			UpdateDebuggerFlags();
			UpdateVectorAddresses();

			string newCode = InteropEmu.DebugGetCode(_firstBreak);
			if(newCode != null) {
				ctrlDebuggerCode.Code = newCode;
			}

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			lblCyclesElapsedCount.Text = (state.CPU.CycleCount - _previousCycle).ToString();
			_previousCycle = state.CPU.CycleCount;

			if(UpdateSplitView()) {
				if(newCode != null || ctrlDebuggerCodeSplit.Code == null) {
					ctrlDebuggerCodeSplit.Code = ctrlDebuggerCode.Code;
				}
				ctrlDebuggerCodeSplit.UpdateCode(true);
			} else {
				_lastCodeWindow = ctrlDebuggerCode;
			}

			if(updateActiveAddress) {
				_lastCodeWindow.SelectActiveAddress(state.CPU.DebugPC);
			}

			ctrlDebuggerCode.SetActiveAddress(state.CPU.DebugPC);
			ctrlDebuggerCode.UpdateLineColors();

			if(UpdateSplitView()) {
				ctrlDebuggerCodeSplit.SetActiveAddress(state.CPU.DebugPC);
				ctrlDebuggerCodeSplit.UpdateLineColors();
			}

			ctrlConsoleStatus.UpdateStatus(ref state);
			ctrlWatch.UpdateWatch();
			ctrlCallstack.UpdateCallstack();

			ctrlCpuMemoryMapping.UpdateCpuRegions(state.Cartridge);
			ctrlPpuMemoryMapping.UpdatePpuRegions(state.Cartridge);

			if(bringToFront) {
				this.BringToFront();
			}

			if(_firstBreak) {
				InteropEmu.SetFlag(EmulationFlags.ForceMaxSpeed, false);
				if(!_wasPaused && !ConfigManager.Config.DebugInfo.BreakOnOpen) {
					ResumeExecution();
				}
				_firstBreak = false;
			}
		}

		private void ClearActiveStatement()
		{
			ctrlDebuggerCode.ClearActiveAddress();
			ctrlDebuggerCode.UpdateLineColors();
			ctrlDebuggerCodeSplit.ClearActiveAddress();
			ctrlDebuggerCodeSplit.UpdateLineColors();
		}

		public void TogglePause()
		{
			if(mnuBreak.Enabled) {
				ctrlConsoleStatus.ApplyChanges();
				InteropEmu.DebugBreakOnScanline(241);
			} else {
				ResumeExecution();
			}
		}

		private void ToggleBreakpoint(bool toggleEnabled)
		{
			_lastCodeWindow.ToggleBreakpoint(toggleEnabled);
		}
		
		private void ResumeExecution()
		{
			mnuContinue.Enabled = false;
			mnuBreak.Enabled = true;

			ctrlConsoleStatus.ApplyChanges();
			ClearActiveStatement();
			UpdateDebuggerFlags();
			InteropEmu.DebugRun();
		}

		private void mnuContinue_Click(object sender, EventArgs e)
		{
			ResumeExecution();
		}

		private void mnuToggleBreakpoint_Click(object sender, EventArgs e)
		{
			ToggleBreakpoint(false);
		}

		private void mnuDisableEnableBreakpoint_Click(object sender, EventArgs e)
		{
			ToggleBreakpoint(true);
		}

		private void mnuBreak_Click(object sender, EventArgs e)
		{
			ctrlConsoleStatus.ApplyChanges();
			InteropEmu.DebugStep(1);
		}

		private void mnuStepInto_Click(object sender, EventArgs e)
		{
			ctrlConsoleStatus.ApplyChanges();
			InteropEmu.DebugStep(1);
		}

		private void mnuStepOut_Click(object sender, EventArgs e)
		{
			ctrlConsoleStatus.ApplyChanges();
			InteropEmu.DebugStepOut();
		}
		
		private void mnuStepOver_Click(object sender, EventArgs e)
		{
			ctrlConsoleStatus.ApplyChanges();
			InteropEmu.DebugStepOver();
		}

		private void mnuStepBack_Click(object sender, EventArgs e)
		{
			ctrlConsoleStatus.ApplyChanges();
			InteropEmu.DebugStepBack();
		}

		private void mnuRunPpuCycle_Click(object sender, EventArgs e)
		{
			ctrlConsoleStatus.ApplyChanges();
			InteropEmu.DebugPpuStep(1);
		}

		private void mnuRunScanline_Click(object sender, EventArgs e)
		{
			ctrlConsoleStatus.ApplyChanges();
			InteropEmu.DebugPpuStep(341);
		}

		private void mnuRunOneFrame_Click(object sender, EventArgs e)
		{
			ctrlConsoleStatus.ApplyChanges();
			InteropEmu.DebugPpuStep(89341);
		}
		
		private void ctrlDebuggerCode_OnScrollToAddress(ctrlDebuggerCode sender, AddressEventArgs args)
		{
			UInt16 addr = (UInt16)args.Address;
			if(sender == ctrlDebuggerCode) {
				if(!ConfigManager.Config.DebugInfo.SplitView) {
					mnuSplitView.Checked = true;
					ConfigManager.Config.DebugInfo.SplitView = true;
					ConfigManager.ApplyChanges();
					UpdateDebugger(false);
				}
				ctrlDebuggerCodeSplit.ScrollToLineNumber(addr);
			} else {
				ctrlDebuggerCode.ScrollToLineNumber(addr);
			}
		}

		private void ctrlDebuggerCode_OnSetNextStatement(ctrlDebuggerCode sender, AddressEventArgs args)
		{
			UInt16 addr = (UInt16)args.Address;
			InteropEmu.DebugSetNextStatement(addr);
			this.UpdateDebugger();
		}

		private void mnuFind_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.OpenSearchBox();
		}
		
		private void mnuFindNext_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FindNext();
		}

		private void mnuFindPrev_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FindPrevious();
		}

		private void mnuSplitView_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.SplitView = this.mnuSplitView.Checked;
			ConfigManager.ApplyChanges();

			UpdateDebugger(false);
		}

		private void mnuMemoryViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.MemoryViewer);
		}

		private void mnuEventViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.EventViewer);
		}

		private void BreakpointManager_BreakpointsChanged(object sender, EventArgs e)
		{
			ctrlDebuggerCodeSplit.UpdateLineColors();
			ctrlDebuggerCode.UpdateLineColors();
		}

		private void ctrlDebuggerCode_Enter(object sender, EventArgs e)
		{
			_lastCodeWindow = ctrlDebuggerCode;
		}

		private void ctrlDebuggerCodeSplit_Enter(object sender, EventArgs e)
		{
			_lastCodeWindow = ctrlDebuggerCodeSplit;
		}

		private void mnuGoToAddress_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.GoToAddress();
		}

		private void mnuGoToIrqHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFF) << 8) | InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFE);
			_lastCodeWindow.ScrollToLineNumber(address);
		}

		private void mnuGoToNmiHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFB) << 8) | InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFA);
			_lastCodeWindow.ScrollToLineNumber(address);
		}

		private void mnuGoToResetHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFD) << 8) | InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFC);
			_lastCodeWindow.ScrollToLineNumber(address);
		}
		
		private void mnuGoToProgramCount_Click(object sender, EventArgs e)
		{
			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);
			_lastCodeWindow.ScrollToActiveAddress();
		}

		private void mnuIncreaseFontSize_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.TextZoom += 10;
		}

		private void mnuDecreaseFontSize_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.TextZoom -= 10;
		}

		private void mnuResetFontSize_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.TextZoom = 100;
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			tmrCdlRatios.Stop();

			LabelManager.OnLabelUpdated -= LabelManager_OnLabelUpdated;
			BreakpointManager.BreakpointsChanged -= BreakpointManager_BreakpointsChanged;
			ctrlConsoleStatus.OnStateChanged -= ctrlConsoleStatus_OnStateChanged;
			ctrlProfiler.OnFunctionSelected -= ctrlProfiler_OnFunctionSelected;

			if(_notifListener != null) {
				_notifListener.Dispose();
				_notifListener = null;
			}

			InteropEmu.DebugSetFlags(0);
			InteropEmu.SetFlag(EmulationFlags.DebuggerWindowEnabled, false);
			BreakpointManager.SetBreakpoints();
			if(_wasPaused) {
				InteropEmu.SetFlag(EmulationFlags.Paused, true);
			}
			InteropEmu.DebugRun();

			ConfigManager.Config.DebugInfo.FontFamily = ctrlDebuggerCode.BaseFont.FontFamily.Name;
			ConfigManager.Config.DebugInfo.FontStyle = ctrlDebuggerCode.BaseFont.Style;
			ConfigManager.Config.DebugInfo.FontSize = ctrlDebuggerCode.BaseFont.Size;
			ConfigManager.Config.DebugInfo.WindowWidth = this.WindowState == FormWindowState.Maximized ? this.RestoreBounds.Width : this.Width;
			ConfigManager.Config.DebugInfo.WindowHeight = this.WindowState == FormWindowState.Maximized ? this.RestoreBounds.Height : this.Height;
			ConfigManager.Config.DebugInfo.TopPanelHeight = this.splitContainer.GetSplitterDistance();
			ConfigManager.Config.DebugInfo.LeftPanelWidth = this.ctrlSplitContainerTop.GetSplitterDistance();
			ConfigManager.ApplyChanges();

			DebugWorkspaceManager.SaveWorkspace();
		}

		private void mnuNametableViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.PpuViewer);
		}
		
		private void mnuApuViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.ApuViewer);
		}

		private void ctrlCallstack_FunctionSelected(object sender, EventArgs e)
		{
			_lastCodeWindow.ScrollToLineNumber((int)sender);
		}

		private void ctrlConsoleStatus_OnStateChanged(object sender, EventArgs e)
		{
			UpdateDebugger(true, false);
		}

		private void tmrCdlRatios_Tick(object sender, EventArgs e)
		{
			this.UpdateCdlRatios();
		}

		private void mnuLoadCdlFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter("CDL files (*.cdl)|*.cdl");
			if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				if(!InteropEmu.DebugLoadCdlFile(ofd.FileName)) {
					MessageBox.Show("Could not load CDL file.  The file selected file is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void mnuSaveAsCdlFile_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.SetFilter("CDL files (*.cdl)|*.cdl");
			sfd.AddExtension = true;
			if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				if(!InteropEmu.DebugSaveCdlFile(sfd.FileName)) {
					MessageBox.Show("Error while trying to save CDL file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void mnuResetCdlLog_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugResetCdlLog();
			UpdateDebugger(false);
		}

		private void ctrlBreakpoints_BreakpointNavigation(object sender, EventArgs e)
		{
			Breakpoint bp = (Breakpoint)sender;
			if(bp.IsCpuBreakpoint) {
				int relAddress = bp.GetRelativeAddress();
				if(relAddress >= 0) {
					_lastCodeWindow.ScrollToLineNumber(relAddress);
				}
			}
		}

		private void mnuTraceLogger_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.TraceLogger);
		}

		private void mnuHidePauseIcon_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.HidePauseIcon = mnuHidePauseIcon.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuPpuPartialDraw_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.PpuPartialDraw = mnuPpuPartialDraw.Checked;
			ConfigManager.ApplyChanges();
			mnuPpuShowPreviousFrame.Enabled = mnuPpuPartialDraw.Checked;
		}
		
		private void mnuShowPreviousFrame_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.PpuShowPreviousFrame = mnuPpuShowPreviousFrame.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuShowEffectiveAddresses_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowEffectiveAddresses = mnuShowEffectiveAddresses.Checked;
			ConfigManager.ApplyChanges();
			UpdateDebugger(false);
		}

		private void mnuShowToolbar_Click(object sender, EventArgs e)
		{
			tsToolbar.Visible = mnuShowToolbar.Checked;
			ConfigManager.Config.DebugInfo.ShowToolbar = mnuShowToolbar.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuShowCpuMemoryMapping_Click(object sender, EventArgs e)
		{
			ctrlCpuMemoryMapping.Visible = mnuShowCpuMemoryMapping.Checked;
			ConfigManager.Config.DebugInfo.ShowCpuMemoryMapping = mnuShowCpuMemoryMapping.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuShowPpuMemoryMapping_Click(object sender, EventArgs e)
		{
			ctrlPpuMemoryMapping.Visible = mnuShowPpuMemoryMapping.Checked;
			ConfigManager.Config.DebugInfo.ShowPpuMemoryMapping = mnuShowPpuMemoryMapping.Checked;
			ConfigManager.ApplyChanges();
		}
		
		private void mnuShowCodePreview_CheckedChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowCodePreview = mnuShowCodePreview.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuShowOpCodeTooltips_CheckedChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowOpCodeTooltips = mnuShowOpCodeTooltips.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuBreakOnReset_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnReset = mnuBreakOnReset.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuBreakOnOpen_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnOpen = mnuBreakOnOpen.Checked;
			ConfigManager.ApplyChanges();
		}
		
		private void mnuBreakOnUnofficialOpcodes_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnUnofficialOpcodes = mnuBreakOnUnofficialOpcodes.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuBreakOnBrk_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnBrk = mnuBreakOnBrk.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuBreakOnCrash_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnCrash = mnuBreakOnCrash.Checked;
			ConfigManager.ApplyChanges();
			DebugInfo.ApplyConfig();
		}

		private void mnuBreakOnDebuggerFocus_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnDebuggerFocus = mnuBreakOnDebuggerFocus.Checked;
			ConfigManager.ApplyChanges();
		}
		
		private void mnuBringToFrontOnPause_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BringToFrontOnPause = mnuBringToFrontOnPause.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuBringToFrontOnBreak_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BringToFrontOnBreak = mnuBringToFrontOnBreak.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuRefreshWatchWhileRunning_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.RefreshWatchWhileRunning = mnuRefreshWatchWhileRunning.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuShowMemoryValues_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowMemoryValuesInCodeWindow = mnuShowMemoryValues.Checked;
			ConfigManager.ApplyChanges();

			ctrlDebuggerCode.ShowMemoryValues = mnuShowMemoryValues.Checked;
			ctrlDebuggerCodeSplit.ShowMemoryValues = mnuShowMemoryValues.Checked;
		}

		private void mnuDisableDefaultLabels_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.DisableDefaultLabels = mnuDisableDefaultLabels.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuDisplayOpCodesInLowerCase_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.DisplayOpCodesInLowerCase = mnuDisplayOpCodesInLowerCase.Checked;
			ConfigManager.ApplyChanges();
			this.UpdateDebuggerFlags();
			this.UpdateDebugger(false);
		}

		private void ctrlFunctionList_OnFunctionSelected(object relativeAddress, EventArgs e)
		{
			_lastCodeWindow.ScrollToLineNumber((Int32)relativeAddress);
		}

		private void LabelManager_OnLabelUpdated(object sender, EventArgs e)
		{
			DebugWorkspaceManager.SaveWorkspace();
			ctrlLabelList.UpdateLabelList();
			ctrlFunctionList.UpdateFunctionList(true);
			UpdateDebugger(false, false);
		}

		private void ctrlLabelList_OnLabelSelected(object relativeAddress, EventArgs e)
		{
			_lastCodeWindow.ScrollToLineNumber((Int32)relativeAddress);
		}

		private void mnuResetWorkspace_Click(object sender, EventArgs e)
		{
			if(MessageBox.Show("This operation will empty the watch window, remove all breakpoints, and reset labels to their default state." + Environment.NewLine + "Are you sure?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
				DebugWorkspaceManager.ResetWorkspace();
				LabelManager.ResetLabels();
				UpdateWorkspace();
				UpdateDebugger(false);
			}
		}

		private void mnuImportLabels_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter("All supported files (*.dbg, *.mlb)|*.dbg;*.mlb");
			if(ofd.ShowDialog() == DialogResult.OK) {
				string ext = Path.GetExtension(ofd.FileName).ToLower();
				if(ext == ".mlb") {
					MesenLabelFile.Import(ofd.FileName);
				} else {
					Ld65DbgImporter dbgImporter = new Ld65DbgImporter();
					dbgImporter.Import(ofd.FileName);
				}					
			}
		}

		private void mnuExportLabels_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.SetFilter("All supported files (*.mlb)|*.mlb");
			if(sfd.ShowDialog() == DialogResult.OK) {
				MesenLabelFile.Export(sfd.FileName);
			}
		}

		private void ctrlLabelList_OnFindOccurrence(object sender, EventArgs e)
		{
			CodeLabel label = sender as CodeLabel;
			_lastCodeWindow.FindAllOccurrences(label.Label, true, true);
		}

		private void ctrlFunctionList_OnFindOccurrence(object sender, EventArgs e)
		{
			_lastCodeWindow.FindAllOccurrences(sender as string, true, true);
		}

		private void mnuBreakIn_Click(object sender, EventArgs e)
		{
			using(frmBreakIn frm = new frmBreakIn()) {
				frm.ShowDialog();
			}
		}

		private void mnuFindAllOccurrences_Click(object sender, EventArgs e)
		{
			frmFindOccurrences frm = new Debugger.frmFindOccurrences();
			if(frm.ShowDialog() == DialogResult.OK) {
				_lastCodeWindow.FindAllOccurrences(frm.SearchString, frm.MatchWholeWord, frm.MatchCase);
			}
		}

		private void mnuAutoLoadDbgFiles_Click(object sender, EventArgs e)
		{
			if(_debuggerInitialized) {
				ConfigManager.Config.DebugInfo.AutoLoadDbgFiles = mnuAutoLoadDbgFiles.Checked;
				ConfigManager.ApplyChanges();
				AutoLoadDbgFiles(false);
			}
		}

		private void mnuAutoLoadCdlFiles_Click(object sender, EventArgs e)
		{
			if(_debuggerInitialized) {
				ConfigManager.Config.DebugInfo.AutoLoadCdlFiles = mnuAutoLoadCdlFiles.Checked;
				ConfigManager.ApplyChanges();
				AutoLoadCdlFiles();
			}
		}

		private void ctrlConsoleStatus_OnGotoLocation(object sender, EventArgs e)
		{
			_lastCodeWindow.ScrollToLineNumber((int)sender);
		}

		private void UpdateDisassembleFlags()
		{
			ConfigManager.ApplyChanges();
			UpdateDebuggerFlags();
			UpdateDebugger(false);
		}

		private void mnuDisassembleVerifiedData_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.DisassembleVerifiedData = mnuDisassembleVerifiedData.Checked;
			UpdateDisassembleFlags();
		}

		private void mnuDisassembleUnidentifiedData_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.DisassembleUnidentifiedData = mnuDisassembleUnidentifiedData.Checked;
			UpdateDisassembleFlags();
		}

		private void mnuShowVerifiedData_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowVerifiedData = mnuShowVerifiedData.Checked;
			UpdateDisassembleFlags();
		}

		private void mnuShowUnidentifiedData_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowUnidentifiedData = mnuShowUnidentifiedData.Checked;
			UpdateDisassembleFlags();
		}

		private void ctrlSplitContainerTop_PanelCollapsed(object sender, EventArgs e)
		{
			mnuShowFunctionLabelLists.Checked = false;
			ConfigManager.Config.DebugInfo.ShowRightPanel = mnuShowFunctionLabelLists.Checked;
			ConfigManager.ApplyChanges();
		}

		private void ctrlSplitContainerTop_PanelExpanded(object sender, EventArgs e)
		{
			mnuShowFunctionLabelLists.Checked = true;
			ConfigManager.Config.DebugInfo.ShowRightPanel = mnuShowFunctionLabelLists.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuShowFunctionLabelLists_Click(object sender, EventArgs e)
		{
			if(mnuShowFunctionLabelLists.Checked) {
				this.ctrlSplitContainerTop.ExpandPanel();
			} else {
				this.ctrlSplitContainerTop.CollapsePanel();
			}
		}

		private void splitContainer_PanelCollapsed(object sender, EventArgs e)
		{
			mnuShowBottomPanel.Checked = false;
			ConfigManager.Config.DebugInfo.ShowBottomPanel = mnuShowBottomPanel.Checked;
			ConfigManager.ApplyChanges();
		}

		private void splitContainer_PanelExpanded(object sender, EventArgs e)
		{
			mnuShowBottomPanel.Checked = true;
			ConfigManager.Config.DebugInfo.ShowBottomPanel = mnuShowBottomPanel.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuShowBottomPanel_Click(object sender, EventArgs e)
		{
			if(mnuShowBottomPanel.Checked) {
				splitContainer.ExpandPanel();
			} else {
				splitContainer.CollapsePanel();
			}
		}
		
		private void mnuSaveRom_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugSaveRomToDisk(InteropEmu.GetRomInfo().RomFile.Path);
		}

		private void mnuSaveRomAs_Click(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter("NES roms (*.nes)|*.nes");
				sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + "_Modified.nes";
				sfd.InitialDirectory = ConfigManager.DebuggerFolder;
				if(sfd.ShowDialog() == DialogResult.OK) {
					InteropEmu.DebugSaveRomToDisk(sfd.FileName);
				}
			}
		}
				
		private void mnuSaveAsIps_Click(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter("IPS patch files (*.ips)|*.ips");
				sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".ips";
				sfd.InitialDirectory = ConfigManager.DebuggerFolder;
				if(sfd.ShowDialog() == DialogResult.OK) {
					InteropEmu.DebugSaveRomToDisk(sfd.FileName, true);
				}
			}
		}
		
		private void SaveRomWithCdlStripping(CdlStripFlag cdlStripFlag)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter("NES roms (*.nes)|*.nes");
				sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + (cdlStripFlag == CdlStripFlag.StripUnused ? "_StripUnusedData.nes" : "_StripUsedData.nes");
				sfd.InitialDirectory = ConfigManager.DebuggerFolder;
				if(sfd.ShowDialog() == DialogResult.OK) {
					InteropEmu.DebugSaveRomToDisk(sfd.FileName, false, null, cdlStripFlag);
				}
			}
		}

		private void mnuRevertChanges_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugRevertPrgChrChanges();
			UpdateDebugger(false);
		}

		private void mnuEditHeader_Click(object sender, EventArgs e)
		{
			using(frmEditHeader frm = new frmEditHeader()) {
				frm.ShowDialog(sender, this);
			}
		}

		private void mnuScriptWindow_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.ScriptWindow);
		}

		private void mnuAssembler_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.Assembler);
		}

		private void ctrlDebuggerCode_OnEditCode(AssemblerEventArgs args)
		{
			DebugWindowManager.OpenAssembler(args.Code, args.StartAddress, args.BlockLength);
		}

		private void mnuCode_DropDownOpening(object sender, EventArgs e)
		{
			this._lastCodeWindow.UpdateContextMenuItemVisibility(false);
			mnuCode.DropDownItems.AddRange(this._lastCodeWindow.ContextMenuItems.ToArray());
		}

		private void mnuCode_DropDownClosed(object sender, EventArgs e)
		{
			List<ToolStripItem> items = new List<ToolStripItem>();
			foreach(ToolStripItem item in mnuCode.DropDownItems) {
				items.Add(item);
			}
			this._lastCodeWindow.ContextMenuItems = items;
		}

		private void mnuCdlStripUsedData_Click(object sender, EventArgs e)
		{
			SaveRomWithCdlStripping(CdlStripFlag.StripUsed);
		}

		private void mnuCdlStripUnusedData_Click(object sender, EventArgs e)
		{
			SaveRomWithCdlStripping(CdlStripFlag.StripUnused);
		}

		private void mnuConfigureColors_Click(object sender, EventArgs e)
		{
			using(frmDebuggerColors frm = new frmDebuggerColors()) {
				if(frm.ShowDialog(this, this) == DialogResult.OK) {
					this.UpdateDebugger(false, true);
				}
			}
		}

		private void mnuSelectFont_Click(object sender, EventArgs e)
		{
			ctrlDebuggerCode.BaseFont = FontDialogHelper.SelectFont(ctrlDebuggerCode.Font);
			ctrlDebuggerCodeSplit.BaseFont = ctrlDebuggerCode.BaseFont;
		}
	}
}
