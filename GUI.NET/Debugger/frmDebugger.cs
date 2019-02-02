﻿using System;
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
using System.Collections.ObjectModel;

namespace Mesen.GUI.Debugger
{
	public partial class frmDebugger : BaseForm
	{
		private bool _debuggerInitialized = false;
		private bool _firstBreak = true;
		private bool _wasPaused = false;
		private bool _executionIsStopped = false; //Flag used to break on the first instruction after power cycle/reset if execution was stopped before the reset
		private int _previousCycle = 0;

		private InteropEmu.NotificationListener _notifListener;
		private ICodeViewer _lastCodeWindow;
		private Size _minimumSize;
		private int _topPanelMinSize;

		public ICodeViewer LastCodeWindow
		{
			get { return _lastCodeWindow; }
			set
			{
				_lastCodeWindow = value;
				UpdateCodeMenu();
			}
		}

		public frmDebugger()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			this.InitShortcuts();
			this.InitToolbar();

			base.OnLoad(e);

			_minimumSize = this.MinimumSize;
			_topPanelMinSize = splitContainer.Panel1MinSize;

			_wasPaused = InteropEmu.IsPaused();
			bool debuggerAlreadyRunning = InteropEmu.DebugIsDebuggerRunning();

			ctrlConsoleStatus.OnStateChanged += ctrlConsoleStatus_OnStateChanged;
			LabelManager.OnLabelUpdated += LabelManager_OnLabelUpdated;
			BreakpointManager.BreakpointsChanged += BreakpointManager_BreakpointsChanged;
			ctrlProfiler.OnFunctionSelected += ctrlProfiler_OnFunctionSelected;

			Font font = new Font(ConfigManager.Config.DebugInfo.FontFamily, ConfigManager.Config.DebugInfo.FontSize, ConfigManager.Config.DebugInfo.FontStyle);
			ctrlDebuggerCode.CodeViewer.BaseFont = font;
			ctrlDebuggerCodeSplit.CodeViewer.BaseFont = font;
			ctrlSourceViewer.CodeViewer.BaseFont = font;
			ctrlSourceViewerSplit.CodeViewer.BaseFont = font;

			this.UpdateWorkspace();
			this.AutoLoadCdlFiles();
			DebugWorkspaceManager.AutoLoadDbgFiles(true);

			if(!Program.IsMono) {
				this.mnuSplitView.Checked = ConfigManager.Config.DebugInfo.SplitView;
			} else {
				Task.Run(() => {
					//Wait 2 seconds before we turn split view on (otherwise Mono tends to cause GDI-related crashes)
					System.Threading.Thread.Sleep(500);
					this.BeginInvoke((Action)(() => {
						this.mnuSplitView.Checked = ConfigManager.Config.DebugInfo.SplitView;
						if(this.mnuSplitView.Checked) {
							this.UpdateDebugger(false, false);
						}
					}));
				});
			}
			this.mnuAutoCreateJumpLabels.Checked = ConfigManager.Config.DebugInfo.AutoCreateJumpLabels;
			this.mnuCopyAddresses.Checked = ConfigManager.Config.DebugInfo.CopyAddresses;
			this.mnuCopyByteCode.Checked = ConfigManager.Config.DebugInfo.CopyByteCode;
			this.mnuCopyComments.Checked = ConfigManager.Config.DebugInfo.CopyComments;
			this.mnuPpuPartialDraw.Checked = ConfigManager.Config.DebugInfo.PpuPartialDraw;
			this.mnuPpuShowPreviousFrame.Checked = ConfigManager.Config.DebugInfo.PpuShowPreviousFrame;
			this.mnuHidePauseIcon.Checked = ConfigManager.Config.DebugInfo.HidePauseIcon;
			this.mnuShowEffectiveAddresses.Checked = ConfigManager.Config.DebugInfo.ShowEffectiveAddresses;
			this.mnuShowCodePreview.Checked = ConfigManager.Config.DebugInfo.ShowCodePreview;
			this.mnuShowOpCodeTooltips.Checked = ConfigManager.Config.DebugInfo.ShowOpCodeTooltips;
			this.mnuOnlyShowTooltipOnShift.Checked = ConfigManager.Config.DebugInfo.OnlyShowTooltipsOnShift;
			this.mnuShowToolbar.Checked = ConfigManager.Config.DebugInfo.ShowToolbar;
			this.mnuShowCpuMemoryMapping.Checked = ConfigManager.Config.DebugInfo.ShowCpuMemoryMapping;
			this.mnuShowPpuMemoryMapping.Checked = ConfigManager.Config.DebugInfo.ShowPpuMemoryMapping;
			this.mnuAutoLoadDbgFiles.Checked = ConfigManager.Config.DebugInfo.AutoLoadDbgFiles;
			this.mnuAutoLoadCdlFiles.Checked = ConfigManager.Config.DebugInfo.AutoLoadCdlFiles;
			this.mnuEnableSubInstructionBreakpoints.Checked = !ConfigManager.Config.DebugInfo.BreakOnFirstCycle;
			this.mnuBreakOnReset.Checked = ConfigManager.Config.DebugInfo.BreakOnReset;
			this.mnuBreakOnInit.Checked = ConfigManager.Config.DebugInfo.BreakOnInit;
			this.mnuBreakOnPlay.Checked = ConfigManager.Config.DebugInfo.BreakOnPlay;
			this.mnuBreakOnOpen.Checked = ConfigManager.Config.DebugInfo.BreakOnOpen;
			this.mnuBreakOnUnofficialOpcodes.Checked = ConfigManager.Config.DebugInfo.BreakOnUnofficialOpcodes;
			this.mnuBreakOnBrk.Checked = ConfigManager.Config.DebugInfo.BreakOnBrk;
			this.mnuBreakOnUninitMemoryRead.Checked = ConfigManager.Config.DebugInfo.BreakOnUninitMemoryRead;
			this.mnuBreakOnDecayedOamRead.Checked = ConfigManager.Config.DebugInfo.BreakOnDecayedOamRead;
			this.mnuBreakOnCrash.Checked = ConfigManager.Config.DebugInfo.BreakOnCrash;
			this.mnuBreakOnDebuggerFocus.Checked = ConfigManager.Config.DebugInfo.BreakOnDebuggerFocus;
			this.mnuBringToFrontOnBreak.Checked = ConfigManager.Config.DebugInfo.BringToFrontOnBreak;
			this.mnuBringToFrontOnPause.Checked = ConfigManager.Config.DebugInfo.BringToFrontOnPause;
			this.mnuDisplayOpCodesInLowerCase.Checked = ConfigManager.Config.DebugInfo.DisplayOpCodesInLowerCase;

			this.mnuDisassembleVerifiedData.Checked = ConfigManager.Config.DebugInfo.DisassembleVerifiedData;
			this.mnuDisassembleUnidentifiedData.Checked = ConfigManager.Config.DebugInfo.DisassembleUnidentifiedData;

			this.mnuShowVerifiedData.Checked = ConfigManager.Config.DebugInfo.ShowVerifiedData;
			this.mnuShowUnidentifiedData.Checked = ConfigManager.Config.DebugInfo.ShowUnidentifiedData;

			this.mnuShowBreakNotifications.Checked = ConfigManager.Config.DebugInfo.ShowBreakNotifications;
			this.mnuShowInstructionProgression.Checked = ConfigManager.Config.DebugInfo.ShowInstructionProgression;
			this.mnuShowSelectionLength.Checked = ConfigManager.Config.DebugInfo.ShowSelectionLength;
			this.mnuAlwaysScrollToCenter.Checked = ConfigManager.Config.DebugInfo.AlwaysScrollToCenter;
			this.mnuRefreshWhileRunning.Checked = ConfigManager.Config.DebugInfo.RefreshWhileRunning;
			this.mnuShowMemoryValues.Checked = ConfigManager.Config.DebugInfo.ShowMemoryValuesInCodeWindow;
			ctrlDebuggerCode.ShowMemoryValues = mnuShowMemoryValues.Checked;
			ctrlDebuggerCodeSplit.ShowMemoryValues = mnuShowMemoryValues.Checked;

			if(ConfigManager.Config.DebugInfo.WindowWidth > -1) {
				this.StartPosition = FormStartPosition.Manual;
				this.Width = ConfigManager.Config.DebugInfo.WindowWidth;
				this.Height = ConfigManager.Config.DebugInfo.WindowHeight;
				this.Location = ConfigManager.Config.DebugInfo.WindowLocation;
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

			this.mnuUseVerticalLayout.Checked = ConfigManager.Config.DebugInfo.VerticalLayout;

			LastCodeWindow = ctrlDebuggerCode;

			this.toolTip.SetToolTip(this.picWatchHelp, ctrlWatch.GetTooltipText());

			_notifListener = new InteropEmu.NotificationListener(ConfigManager.Config.DebugInfo.DebugConsoleId);
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
			InteropEmu.Resume();

			UpdateDebuggerFlags();
			UpdateCdlRatios();
			UpdateFileOptions();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			ctrlDebuggerCode.Visible = true;
			ctrlDebuggerCodeSplit.Visible = true;
			ctrlSourceViewer.Visible = true;
			ctrlSourceViewerSplit.Visible = true;

			ctrlDebuggerCode.CodeViewerActions.OnSetNextStatement += ctrlDebuggerCode_OnSetNextStatement;
			ctrlDebuggerCode.CodeViewerActions.OnShowInSplitView += ctrlDebuggerCode_OnShowInSplitView;
			ctrlDebuggerCode.CodeViewerActions.OnGoToDestination += ctrlDebuggerCode_OnGoToDestination;
			ctrlDebuggerCode.CodeViewerActions.OnSwitchView += ctrlDebuggerCode_OnSwitchView;

			ctrlDebuggerCodeSplit.CodeViewerActions.OnSetNextStatement += ctrlDebuggerCode_OnSetNextStatement;
			ctrlDebuggerCodeSplit.CodeViewerActions.OnShowInSplitView += ctrlDebuggerCode_OnShowInSplitView;
			ctrlDebuggerCodeSplit.CodeViewerActions.OnGoToDestination += ctrlDebuggerCode_OnGoToDestination;
			ctrlDebuggerCodeSplit.CodeViewerActions.OnSwitchView += ctrlDebuggerCode_OnSwitchView;

			ctrlSourceViewer.CodeViewerActions.OnSetNextStatement += ctrlDebuggerCode_OnSetNextStatement;
			ctrlSourceViewer.CodeViewerActions.OnShowInSplitView += ctrlDebuggerCode_OnShowInSplitView;
			ctrlSourceViewer.CodeViewerActions.OnGoToDestination += ctrlDebuggerCode_OnGoToDestination;
			ctrlSourceViewer.CodeViewerActions.OnSwitchView += ctrlDebuggerCode_OnSwitchView;

			ctrlSourceViewerSplit.CodeViewerActions.OnSetNextStatement += ctrlDebuggerCode_OnSetNextStatement;
			ctrlSourceViewerSplit.CodeViewerActions.OnShowInSplitView += ctrlDebuggerCode_OnShowInSplitView;
			ctrlSourceViewerSplit.CodeViewerActions.OnGoToDestination += ctrlDebuggerCode_OnGoToDestination;
			ctrlSourceViewerSplit.CodeViewerActions.OnSwitchView += ctrlDebuggerCode_OnSwitchView;

			ctrlDebuggerCode.SetConfig(ConfigManager.Config.DebugInfo.LeftView);
			ctrlSourceViewer.SetConfig(ConfigManager.Config.DebugInfo.LeftView);
			ctrlDebuggerCodeSplit.SetConfig(ConfigManager.Config.DebugInfo.RightView);
			ctrlSourceViewerSplit.SetConfig(ConfigManager.Config.DebugInfo.RightView);

			ctrlSourceViewer.Visible = false;
			ctrlSourceViewerSplit.Visible = false;
		}

		private void InitShortcuts()
		{
			mnuIncreaseFontSize.InitShortcut(this, nameof(DebuggerShortcutsConfig.IncreaseFontSize));
			mnuDecreaseFontSize.InitShortcut(this, nameof(DebuggerShortcutsConfig.DecreaseFontSize));
			mnuResetFontSize.InitShortcut(this, nameof(DebuggerShortcutsConfig.ResetFontSize));

			mnuSaveRom.InitShortcut(this, nameof(DebuggerShortcutsConfig.SaveRom));
			mnuSaveRomAs.InitShortcut(this, nameof(DebuggerShortcutsConfig.SaveRomAs));
			mnuSaveAsIps.InitShortcut(this, nameof(DebuggerShortcutsConfig.SaveEditAsIps));
			mnuRevertChanges.InitShortcut(this, nameof(DebuggerShortcutsConfig.RevertPrgChrChanges));

			mnuReset.InitShortcut(this, nameof(DebuggerShortcutsConfig.Reset));
			mnuPowerCycle.InitShortcut(this, nameof(DebuggerShortcutsConfig.PowerCycle));

			mnuContinue.InitShortcut(this, nameof(DebuggerShortcutsConfig.Continue));
			mnuBreak.InitShortcut(this, nameof(DebuggerShortcutsConfig.Break));
			mnuBreakIn.InitShortcut(this, nameof(DebuggerShortcutsConfig.BreakIn));
			mnuBreakOn.InitShortcut(this, nameof(DebuggerShortcutsConfig.BreakOn));

			mnuStepBack.InitShortcut(this, nameof(DebuggerShortcutsConfig.StepBack));
			mnuStepOut.InitShortcut(this, nameof(DebuggerShortcutsConfig.StepOut));
			mnuStepInto.InitShortcut(this, nameof(DebuggerShortcutsConfig.StepInto));
			mnuStepOver.InitShortcut(this, nameof(DebuggerShortcutsConfig.StepOver));

			mnuRunCpuCycle.InitShortcut(this, nameof(DebuggerShortcutsConfig.RunCpuCycle));
			mnuRunPpuCycle.InitShortcut(this, nameof(DebuggerShortcutsConfig.RunPpuCycle));
			mnuRunScanline.InitShortcut(this, nameof(DebuggerShortcutsConfig.RunPpuScanline));
			mnuRunOneFrame.InitShortcut(this, nameof(DebuggerShortcutsConfig.RunPpuFrame));

			mnuGoToAll.InitShortcut(this, nameof(DebuggerShortcutsConfig.GoToAll));
			mnuGoToAddress.InitShortcut(this, nameof(DebuggerShortcutsConfig.GoTo));
			mnuFind.InitShortcut(this, nameof(DebuggerShortcutsConfig.Find));
			mnuFindNext.InitShortcut(this, nameof(DebuggerShortcutsConfig.FindNext));
			mnuFindPrev.InitShortcut(this, nameof(DebuggerShortcutsConfig.FindPrev));
			mnuFindAllOccurrences.InitShortcut(this, nameof(DebuggerShortcutsConfig.FindOccurrences));

			mnuShowVerifiedData.InitShortcut(this, nameof(DebuggerShortcutsConfig.ToggleVerifiedData));
			mnuShowUnidentifiedData.InitShortcut(this, nameof(DebuggerShortcutsConfig.ToggleUnidentifiedCodeData));

			mnuApuViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenApuViewer));
			mnuAssembler.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenAssembler));
			mnuEventViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenEventViewer));
			mnuMemoryViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenMemoryTools));
			mnuPpuViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenPpuViewer));
			mnuScriptWindow.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenScriptWindow));
			mnuTraceLogger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenTraceLogger));
			mnuTextHooker.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenTextHooker));
			mnuProfiler.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenProfiler));
			mnuWatchWindow.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenWatchWindow));

			mnuOpenNametableViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenNametableViewer));
			mnuOpenChrViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenChrViewer));
			mnuOpenSpriteViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenSpriteViewer));
			mnuOpenPaletteViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenPaletteViewer));
		}

		private void InitToolbar()
		{
			tsToolbar.AddItemsToToolbar(
				mnuSaveRom, mnuRevertChanges, null,
				mnuImportLabels, mnuExportLabels, null,
				mnuContinue, mnuBreak, null,
				mnuStepInto, mnuStepOver, mnuStepOut, mnuStepBack, null,
				mnuRunCpuCycle, null,
				mnuRunPpuCycle, mnuRunScanline, mnuRunOneFrame, null,
				mnuToggleBreakpoint, mnuDisableEnableBreakpoint, null,
				mnuFind, mnuFindPrev, mnuFindNext, null,
				mnuApuViewer, mnuAssembler, mnuEventViewer, mnuMemoryViewer, mnuPpuViewer, mnuScriptWindow, mnuTextHooker, mnuTraceLogger, null,
				mnuEditHeader, null,
				mnuSplitView, mnuUseVerticalLayout, null
			);
			tsToolbar.AddItemToToolbar(mnuShowVerifiedData, "Show Verified Data");
			tsToolbar.AddItemToToolbar(mnuShowUnidentifiedData, "Show Unidentified Code/Data");
			tsToolbar.AddItemsToToolbar(null, mnuBreakIn, null, mnuBreakOn);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(!((UserControl)LastCodeWindow).ContainsFocus) {
				//Allow Ctrl+C, etc to work normally while editing watch, or other fields
				CleanupMenu(mnuCode.DropDownItems);
				mnuCode.DropDownItems.Clear();
			} else {
				UpdateCodeMenu();
			}

			if(keyData == ConfigManager.Config.DebugInfo.Shortcuts.ToggleBreakContinue) {
				if(mnuBreak.Enabled) {
					ctrlConsoleStatus.ApplyChanges();
					InteropEmu.DebugStep(1);
				} else {
					ResumeExecution();
				}
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if(ConfigManager.Config.DebugInfo.BreakOnDebuggerFocus && !InteropEmu.DebugIsExecutionStopped()) {
				InteropEmu.DebugStep(1, BreakSource.BreakOnFocus);
			}
		}

		private void ctrlProfiler_OnFunctionSelected(object sender, EventArgs e)
		{
			int relativeAddress = InteropEmu.DebugGetRelativeAddress((UInt32)sender, AddressType.PrgRom);
			if(relativeAddress >= 0) {
				BringToFront();
				LastCodeWindow.ScrollToLineNumber(relativeAddress);
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

		private void UpdateWorkspace()
		{
			DebugWorkspaceManager.SaveWorkspace();
			DebugWorkspaceManager.GetWorkspace();

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
			DebugInfo config = ConfigManager.Config.DebugInfo;
			SetFlag(DebuggerFlags.PpuPartialDraw, config.PpuPartialDraw);
			SetFlag(DebuggerFlags.PpuShowPreviousFrame, config.PpuShowPreviousFrame);
			SetFlag(DebuggerFlags.ShowEffectiveAddresses, config.ShowEffectiveAddresses);
			SetFlag(DebuggerFlags.DisplayOpCodesInLowerCase, config.DisplayOpCodesInLowerCase);
			SetFlag(DebuggerFlags.DisassembleVerifiedData, config.DisassembleVerifiedData);
			SetFlag(DebuggerFlags.DisassembleUnidentifiedData, config.DisassembleUnidentifiedData);
			SetFlag(DebuggerFlags.ShowVerifiedData, config.ShowVerifiedData);
			SetFlag(DebuggerFlags.ShowUnidentifiedData, config.ShowUnidentifiedData);
			SetFlag(DebuggerFlags.BreakOnUnofficialOpCode, config.BreakOnUnofficialOpcodes);
			SetFlag(DebuggerFlags.BreakOnBrk, config.BreakOnBrk);
			SetFlag(DebuggerFlags.BreakOnUninitMemoryRead, config.BreakOnUninitMemoryRead);
			SetFlag(DebuggerFlags.BreakOnDecayedOamRead, config.BreakOnDecayedOamRead);
			SetFlag(DebuggerFlags.BreakOnInit, config.BreakOnInit);
			SetFlag(DebuggerFlags.BreakOnPlay, config.BreakOnPlay);
			SetFlag(DebuggerFlags.BreakOnFirstCycle, config.BreakOnFirstCycle);
			SetFlag(DebuggerFlags.HidePauseIcon, config.HidePauseIcon);
			InteropEmu.SetFlag(EmulationFlags.DebuggerWindowEnabled, true);
		}

		private string GetBreakNotification(Int64 param)
		{
			BreakSource source = (BreakSource)(byte)param;

			string message = null;
			if(ConfigManager.Config.DebugInfo.ShowBreakNotifications) {
				message = ResourceHelper.GetEnumText(source);
				if(source == BreakSource.Breakpoint) {
					int breakpointId = (int)(param >> 40);
					byte bpValue = (byte)((param >> 32) & 0xFF);
					BreakpointType bpType = (BreakpointType)(byte)((param >> 8) & 0x0F);
					UInt16 bpAddress = (UInt16)(param >> 16);

					ReadOnlyCollection<Breakpoint> breakpoints = BreakpointManager.Breakpoints;
					if(breakpointId >= 0 && breakpointId < breakpoints.Count) {
						Breakpoint bp = breakpoints[breakpointId];
						if(bpType != BreakpointType.Global) {
							message += ": " + ResourceHelper.GetEnumText(bpType) + " ($" + bpAddress.ToString("X4") + ":$" + bpValue.ToString("X2") + ")";
						}
						if(!string.IsNullOrWhiteSpace(bp.Condition)) {
							string cond = bp.Condition.Trim();
							if(cond.Length > 27) {
								message += Environment.NewLine + cond.Substring(0, 24) + "...";
							} else {
								message += Environment.NewLine + cond;
							}
						}
					}
				} else if(source == BreakSource.BreakOnUninitMemoryRead) {
					UInt16 address = (UInt16)(param >> 16);
					byte value = (byte)((param >> 32) & 0xFF);
					message += " ($" + address.ToString("X4") + ":$" + value.ToString("X2") + ")";
				} else if(source == BreakSource.CpuStep || source == BreakSource.PpuStep) {
					//Don't display anything when breaking due to stepping
					message = null;
				}
			}

			return message;
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case InteropEmu.ConsoleNotificationType.PpuFrameDone:
					if(ConfigManager.Config.DebugInfo.RefreshWhileRunning) {
						DebugState state = new DebugState();
						InteropEmu.DebugGetState(ref state);

						this.BeginInvoke((MethodInvoker)(() => {
							if(state.PPU.FrameCount % 30 == 0) {
								//Update UI every 30 frames, since this is a relatively slow operation
								ctrlCpuMemoryMapping.UpdateCpuRegions(state.Cartridge);
								ctrlPpuMemoryMapping.UpdatePpuRegions(state.Cartridge);
								ctrlConsoleStatus.UpdateStatus(ref state);
								UpdateCdlRatios();
							}
							ctrlWatch.UpdateWatch(false);
						}));
					}
					break;

				case InteropEmu.ConsoleNotificationType.CodeBreak:
					this._executionIsStopped = true;

					this.BeginInvoke((MethodInvoker)(() => {
						BreakSource source = (BreakSource)(byte)e.Parameter.ToInt64();
						
						bool bringToFront = (
							source < BreakSource.Pause && ConfigManager.Config.DebugInfo.BringToFrontOnBreak ||
							source == BreakSource.Pause && ConfigManager.Config.DebugInfo.BringToFrontOnPause
						);
						UpdateDebugger(true, bringToFront);

						_lastCodeWindow.SetMessage(new TextboxMessageInfo() { Message = GetBreakNotification(e.Parameter.ToInt64()) });

						mnuContinue.Enabled = true;
						mnuBreak.Enabled = false;
					}));
					BreakpointManager.SetBreakpoints();
					break;

				case InteropEmu.ConsoleNotificationType.GameReset:
				case InteropEmu.ConsoleNotificationType.GameLoaded:
					UpdateDebuggerFlags();
					BreakpointManager.SetBreakpoints();

					bool breakOnReset = ConfigManager.Config.DebugInfo.BreakOnReset && !InteropEmu.IsNsf();
					this.BeginInvoke((MethodInvoker)(() => {
						this.UpdateWorkspace();
						LabelManager.RefreshLabels();
						this.AutoLoadCdlFiles();
						DebugWorkspaceManager.AutoLoadDbgFiles(true);
						UpdateDebugger(true, false);

						if(!breakOnReset) {
							ClearActiveStatement();
						}
					}));

					if(breakOnReset || _executionIsStopped) {
						InteropEmu.DebugStep(1, BreakSource.BreakOnReset);
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
				this.UpdateMinimumSize();
			} else {
				tlpTop.ColumnStyles[1].SizeType = SizeType.Absolute;
				tlpTop.ColumnStyles[1].Width = 0f;
				this.UpdateMinimumSize();
			}
			ctrlDebuggerCodeSplit.Visible = mnuSplitView.Checked;
			return mnuSplitView.Checked;
		}

		private void UpdateMinimumSize()
		{
			int minWidth, minHeight;
			if(mnuSplitView.Checked) {
				minWidth = _minimumSize.Width + 250;
			} else {
				minWidth = _minimumSize.Width;
			}

			if(mnuUseVerticalLayout.Checked) {
				minHeight = _minimumSize.Height + 150;
				splitContainer.Panel1MinSize = _topPanelMinSize + 100;
			} else {
				minHeight = _minimumSize.Height;
				splitContainer.Panel1MinSize = _topPanelMinSize;
			}

			this.MinimumSize = new Size(minWidth, minHeight);
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

			if(ConfigManager.Config.DebugInfo.AutoCreateJumpLabels) {
				LabelManager.CreateAutomaticJumpLabels();
			}

			ctrlBreakpoints.RefreshListAddresses();
			ctrlLabelList.UpdateLabelListAddresses();
			ctrlFunctionList.UpdateFunctionList(false);
			UpdateDebuggerFlags();
			UpdateVectorAddresses();

			string newCode = InteropEmu.DebugGetCode(_firstBreak);
			if(newCode != null) {
				ctrlDebuggerCode.Code = new CodeInfo(newCode);
			}

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			lblCyclesElapsedCount.Text = (state.CPU.CycleCount - _previousCycle).ToString();
			_previousCycle = state.CPU.CycleCount;

			if(UpdateSplitView()) {
				if(ctrlDebuggerCodeSplit.Code != ctrlDebuggerCode.Code) {
					ctrlDebuggerCodeSplit.Code = ctrlDebuggerCode.Code;
				}
			} else {
				LastCodeWindow = ctrlSourceViewer.Visible ? (ICodeViewer)ctrlSourceViewer : (ICodeViewer)ctrlDebuggerCode;
			}

			if(updateActiveAddress) {
				LastCodeWindow.SelectActiveAddress(state.CPU.DebugPC);
			}

			ctrlDebuggerCode.SetActiveAddress(state.CPU.DebugPC);
			ctrlSourceViewer.SetActiveAddress(state.CPU.DebugPC);
			ctrlDebuggerCode.UpdateLineColors();

			if(UpdateSplitView()) {
				ctrlDebuggerCodeSplit.SetActiveAddress(state.CPU.DebugPC);
				ctrlSourceViewerSplit.SetActiveAddress(state.CPU.DebugPC);
				ctrlDebuggerCodeSplit.UpdateLineColors();
			}

			ctrlConsoleStatus.UpdateStatus(ref state);
			ctrlWatch.UpdateWatch();
			ctrlCallstack.UpdateCallstack();
			UpdateCdlRatios();

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

				//Switch to source viewer after reopening debugger if we were in source viewer when we closed it
				if(ctrlSourceViewer.CurrentFile != null && ConfigManager.Config.DebugInfo.LeftView.UsingSourceView) {
					ctrlDebuggerCode_OnSwitchView(this.ctrlDebuggerCode);
				}
				if(ctrlSourceViewerSplit.CurrentFile != null && ConfigManager.Config.DebugInfo.RightView.UsingSourceView) {
					ctrlDebuggerCode_OnSwitchView(this.ctrlDebuggerCodeSplit);
				}
			}
		}

		private void ClearActiveStatement()
		{
			ctrlDebuggerCode.ClearActiveAddress();
			ctrlDebuggerCode.UpdateLineColors();
			ctrlDebuggerCodeSplit.ClearActiveAddress();
			ctrlDebuggerCodeSplit.UpdateLineColors();

			ctrlSourceViewer.ClearActiveAddress();
			ctrlSourceViewerSplit.ClearActiveAddress();

			ctrlDebuggerCode.SetMessage(null);
			ctrlDebuggerCodeSplit.SetMessage(null);
			ctrlSourceViewer.SetMessage(null);
			ctrlSourceViewerSplit.SetMessage(null);
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
			LastCodeWindow.CodeViewerActions.ToggleBreakpoint(toggleEnabled);
		}
		
		private void ResumeExecution()
		{
			_executionIsStopped = false;
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

		private void mnuRunCpuCycle_Click(object sender, EventArgs e)
		{
			ctrlConsoleStatus.ApplyChanges();
			InteropEmu.DebugStepCycles(1);
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
			NesModel model = InteropEmu.GetNesModel();
			int extraScanlines = (int)(ConfigManager.Config.EmulationInfo.PpuExtraScanlinesAfterNmi + ConfigManager.Config.EmulationInfo.PpuExtraScanlinesBeforeNmi);
			int cycleCount = ((model == NesModel.NTSC ? 262 : 312) + extraScanlines) * 341;
			InteropEmu.DebugPpuStep((UInt32)cycleCount);
		}

		private void ctrlDebuggerCode_OnGoToDestination(ICodeViewer sender, GoToDestination dest)
		{
			GoToDestination(sender, dest);
		}

		private void ctrlDebuggerCode_OnShowInSplitView(ICodeViewer sender, GoToDestination dest)
		{
			if(!ConfigManager.Config.DebugInfo.SplitView) {
				mnuSplitView.Checked = true;
				ConfigManager.Config.DebugInfo.SplitView = true;
				ConfigManager.ApplyChanges();
				UpdateDebugger(false);
			}

			if(sender == ctrlDebuggerCode || sender == ctrlSourceViewer) {
				if(ctrlSourceViewerSplit.Visible) {
					GoToDestination(ctrlSourceViewerSplit, dest);
				} else {
					GoToDestination(ctrlDebuggerCodeSplit, dest);
				}
			} else {
				if(ctrlSourceViewer.Visible) {
					GoToDestination(ctrlSourceViewer, dest);
				} else {
					GoToDestination(ctrlDebuggerCode, dest);
				}
			}
		}

		private void ctrlDebuggerCode_OnSetNextStatement(AddressEventArgs args)
		{
			UInt16 addr = (UInt16)args.Address;
			InteropEmu.DebugSetNextStatement(addr);
			this.UpdateDebugger();
		}

		private void ctrlDebuggerCode_OnSwitchView(ICodeViewer sender)
		{
			if(ctrlDebuggerCode == sender) {
				if(ctrlSourceViewer.CurrentFile != null) {
					ctrlDebuggerCode.Visible = false;
					ctrlSourceViewer.Visible = true;
					if(ctrlDebuggerCode.CodeViewer.CurrentLine >= 0) {
						ctrlSourceViewer.ScrollToLineNumber(ctrlDebuggerCode.CodeViewer.CurrentLine);
					}
					ctrlSourceViewer.Focus();
					ctrlSourceViewer.SetConfig(ConfigManager.Config.DebugInfo.LeftView);
				} else {
					MessageBox.Show("Source files could not be found/loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} else if(ctrlSourceViewer == sender) {
				ctrlSourceViewer.Visible = false;
				ctrlDebuggerCode.Visible = true;
				if(ctrlDebuggerCode.CodeViewer.CurrentLine >= 0) {
					ctrlDebuggerCode.ScrollToLineNumber(ctrlDebuggerCode.CodeViewer.CurrentLine);
				}
				ctrlDebuggerCode.Focus();
				ctrlDebuggerCode.SetConfig(ConfigManager.Config.DebugInfo.LeftView);
			} else if(ctrlSourceViewerSplit == sender) {
				if(ctrlSourceViewerSplit.CurrentFile != null) {
					ctrlSourceViewerSplit.Visible = false;
					ctrlDebuggerCodeSplit.Visible = true;
					if(ctrlSourceViewerSplit.CodeViewer.CurrentLine >= 0) {
						ctrlDebuggerCodeSplit.ScrollToLineNumber(ctrlSourceViewerSplit.CodeViewer.CurrentLine);
					}
					ctrlDebuggerCodeSplit.Focus();
					ctrlDebuggerCodeSplit.SetConfig(ConfigManager.Config.DebugInfo.RightView);
				} else {
					MessageBox.Show("Source files could not be found/loaded", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} else {
				ctrlDebuggerCodeSplit.Visible = false;
				ctrlSourceViewerSplit.Visible = true;
				if(ctrlDebuggerCodeSplit.CodeViewer.CurrentLine >= 0) {
					ctrlSourceViewerSplit.ScrollToLineNumber(ctrlDebuggerCodeSplit.CodeViewer.CurrentLine);
				}
				ctrlSourceViewerSplit.Focus();
				ctrlSourceViewerSplit.SetConfig(ConfigManager.Config.DebugInfo.RightView);
			}
		}

		private void mnuFind_Click(object sender, EventArgs e)
		{
			LastCodeWindow.CodeViewer.OpenSearchBox();
		}
		
		private void mnuFindNext_Click(object sender, EventArgs e)
		{
			LastCodeWindow.CodeViewer.FindNext();
		}

		private void mnuFindPrev_Click(object sender, EventArgs e)
		{
			LastCodeWindow.CodeViewer.FindPrevious();
		}

		private void mnuSplitView_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.SplitView = this.mnuSplitView.Checked;
			ConfigManager.ApplyChanges();

			ctrlDebuggerCodeSplit.Code = ctrlDebuggerCode.Code;
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

			ctrlSourceViewer.RefreshViewer();
			ctrlSourceViewerSplit.RefreshViewer();
		}

		public void ScrollToAddress(int address)
		{
			LastCodeWindow.ScrollToLineNumber(address);
			if(WindowState == FormWindowState.Minimized) {
				//Unminimize window if it was minimized
				WindowState = FormWindowState.Normal;
			}
			BringToFront();
		}

		private void ctrlDebuggerCode_Enter(object sender, EventArgs e)
		{
			LastCodeWindow = (ICodeViewer)sender;
		}

		private void mnuGoToAddress_Click(object sender, EventArgs e)
		{
			LastCodeWindow.CodeViewer.GoToAddress();
		}

		private void mnuGoToIrqHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFF) << 8) | InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFE);
			LastCodeWindow.ScrollToLineNumber(address);
		}

		private void mnuGoToNmiHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFB) << 8) | InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFA);
			LastCodeWindow.ScrollToLineNumber(address);
		}

		private void mnuGoToResetHandler_Click(object sender, EventArgs e)
		{
			int address = (InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFD) << 8) | InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, 0xFFFC);
			LastCodeWindow.ScrollToLineNumber(address);
		}
		
		private void mnuGoToProgramCount_Click(object sender, EventArgs e)
		{
			LastCodeWindow.CodeViewerActions.ScrollToActiveAddress();
		}

		private void mnuIncreaseFontSize_Click(object sender, EventArgs e)
		{
			LastCodeWindow.CodeViewer.TextZoom += 10;
		}

		private void mnuDecreaseFontSize_Click(object sender, EventArgs e)
		{
			LastCodeWindow.CodeViewer.TextZoom -= 10;
		}

		private void mnuResetFontSize_Click(object sender, EventArgs e)
		{
			LastCodeWindow.CodeViewer.TextZoom = 100;
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
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
				InteropEmu.Pause();
			}
			InteropEmu.DebugRun();

			ConfigManager.Config.DebugInfo.FontFamily = ctrlDebuggerCode.CodeViewer.BaseFont.FontFamily.Name;
			ConfigManager.Config.DebugInfo.FontStyle = ctrlDebuggerCode.CodeViewer.BaseFont.Style;
			ConfigManager.Config.DebugInfo.FontSize = ctrlDebuggerCode.CodeViewer.BaseFont.Size;
			ConfigManager.Config.DebugInfo.WindowLocation = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Location : this.Location;
			ConfigManager.Config.DebugInfo.WindowWidth = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Width : this.Width;
			ConfigManager.Config.DebugInfo.WindowHeight = this.WindowState != FormWindowState.Normal ? this.RestoreBounds.Height : this.Height;
			ConfigManager.Config.DebugInfo.TopPanelHeight = this.splitContainer.GetSplitterDistance();
			ConfigManager.Config.DebugInfo.LeftPanelWidth = this.ctrlSplitContainerTop.GetSplitterDistance();

			ConfigManager.Config.DebugInfo.LeftView.UsingSourceView = this.ctrlSourceViewer.Visible;
			ConfigManager.Config.DebugInfo.RightView.UsingSourceView = this.ctrlSourceViewerSplit.Visible;

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
			LastCodeWindow.ScrollToLineNumber((int)sender);
		}

		private void ctrlConsoleStatus_OnStateChanged(object sender, EventArgs e)
		{
			UpdateDebugger(true, false);
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
					LastCodeWindow.ScrollToLineNumber(relAddress);
				}
			}
		}

		private void mnuTextHooker_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.TextHooker);
		}

		private void mnuTraceLogger_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.TraceLogger);
		}
		
		private void mnuAutoCreateJumpLabels_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.AutoCreateJumpLabels = mnuAutoCreateJumpLabels.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuCopyAddresses_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.CopyAddresses = mnuCopyAddresses.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuCopyByteCode_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.CopyByteCode = mnuCopyByteCode.Checked;
			ConfigManager.ApplyChanges();
		}
		
		private void mnuCopyComments_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.CopyComments = mnuCopyComments.Checked;
			ConfigManager.ApplyChanges();
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

		private void mnuTooltipShowOnShift_CheckedChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.OnlyShowTooltipsOnShift = mnuOnlyShowTooltipOnShift.Checked;
			ConfigManager.ApplyChanges();
		}
		
		private void mnuBreakOnFirstCycle_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnFirstCycle = !mnuEnableSubInstructionBreakpoints.Checked;
			ConfigManager.ApplyChanges();
			UpdateDebuggerFlags();
		}

		private void mnuBreakOnReset_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnReset = mnuBreakOnReset.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuBreakOnInit_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnInit = mnuBreakOnInit.Checked;
			ConfigManager.ApplyChanges();
			UpdateDebuggerFlags();
		}

		private void mnuBreakOnPlay_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnPlay = mnuBreakOnPlay.Checked;
			ConfigManager.ApplyChanges();
			UpdateDebuggerFlags();
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
			UpdateDebuggerFlags();
		}

		private void mnuBreakOnBrk_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnBrk = mnuBreakOnBrk.Checked;
			ConfigManager.ApplyChanges();
			UpdateDebuggerFlags();
		}

		private void mnuBreakOnUninitMemoryRead_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnUninitMemoryRead = mnuBreakOnUninitMemoryRead.Checked;
			ConfigManager.ApplyChanges();
			UpdateDebuggerFlags();
		}

		private void mnuBreakOnDecayedOamRead_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnDecayedOamRead = mnuBreakOnDecayedOamRead.Checked;
			ConfigManager.ApplyChanges();
			UpdateDebuggerFlags();
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
		
		private void mnuShowBreakNotifications_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowBreakNotifications = mnuShowBreakNotifications.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuShowInstructionProgression_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowInstructionProgression = mnuShowInstructionProgression.Checked;
			ConfigManager.ApplyChanges();
			
			UpdateDebugger(false, false);
		}

		private void mnuShowSelectionLength_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowSelectionLength = mnuShowSelectionLength.Checked;
			ConfigManager.ApplyChanges();

			UpdateDebugger(false, false);
		}

		private void mnuAlwaysScrollToCenter_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.AlwaysScrollToCenter = mnuAlwaysScrollToCenter.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuRefreshWhileRunning_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.RefreshWhileRunning = mnuRefreshWhileRunning.Checked;
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

		private void ctrlFunctionList_OnFunctionSelected(GoToDestination dest)
		{
			GoToDestination(LastCodeWindow, dest);
		}

		private void ctrlLabelList_OnLabelSelected(GoToDestination dest)
		{
			GoToDestination(LastCodeWindow, dest);
		}

		private void LabelManager_OnLabelUpdated(object sender, EventArgs e)
		{
			DebugWorkspaceManager.SaveWorkspace();
			ctrlLabelList.UpdateLabelList();
			ctrlFunctionList.UpdateFunctionList(true);
			UpdateDebugger(false, false);
		}

		private void mnuResetWorkspace_Click(object sender, EventArgs e)
		{
			if(MessageBox.Show("This operation will empty the watch window, remove all breakpoints, and reset labels to their default state." + Environment.NewLine + "Are you sure?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
				DebugWorkspaceManager.ResetWorkspace();
				UpdateWorkspace();
				UpdateDebugger(false);
			}
		}

		private void mnuResetLabels_Click(object sender, EventArgs e)
		{
			if(MessageBox.Show("This operation will reset labels to their default state." + Environment.NewLine + "Are you sure?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
				DebugWorkspaceManager.ResetLabels();
				UpdateWorkspace();
				UpdateDebugger(false);
			}
		}

		private void ImportLabelFile(string path)
		{
			string ext = Path.GetExtension(path).ToLower();
			if(ext == ".mlb") {
				DebugWorkspaceManager.ImportMlbFile(path);
			} else if(ext == ".fns") {
				DebugWorkspaceManager.ImportNesasmFnsFile(path);
			} else {
				DebugWorkspaceManager.ImportDbgFile(path, false);
			}
		}

		private void mnuImportLabels_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter("All supported files (*.dbg, *.mlb, *.fns)|*.dbg;*.mlb;*.fns");
			if(ofd.ShowDialog() == DialogResult.OK) {
				ImportLabelFile(ofd.FileName);
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
			LastCodeWindow.FindAllOccurrences(label.Label, true, true);
		}

		private void ctrlFunctionList_OnFindOccurrence(object sender, EventArgs e)
		{
			LastCodeWindow.FindAllOccurrences(sender as string, true, true);
		}

		private void mnuBreakIn_Click(object sender, EventArgs e)
		{
			using(frmBreakIn frm = new frmBreakIn()) {
				frm.ShowDialog();
			}
		}
		
		private void mnuBreakOn_Click(object sender, EventArgs e)
		{
			using(frmBreakOn frm = new frmBreakOn()) {
				frm.ShowDialog();
			}
		}

		private void mnuFindAllOccurrences_Click(object sender, EventArgs e)
		{
			frmFindOccurrences frm = new Debugger.frmFindOccurrences();
			if(frm.ShowDialog() == DialogResult.OK) {
				LastCodeWindow.FindAllOccurrences(frm.SearchString, frm.MatchWholeWord, frm.MatchCase);
			}
		}

		private void mnuAutoLoadDbgFiles_Click(object sender, EventArgs e)
		{
			if(_debuggerInitialized) {
				ConfigManager.Config.DebugInfo.AutoLoadDbgFiles = mnuAutoLoadDbgFiles.Checked;
				ConfigManager.ApplyChanges();
				DebugWorkspaceManager.AutoLoadDbgFiles(false);
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
			LastCodeWindow.ScrollToLineNumber((int)sender);
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

		private void mnuProfiler_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.Profiler);
		}

		private void mnuAssembler_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.Assembler);
		}

		private void ctrlDebuggerCode_OnEditCode(AssemblerEventArgs args)
		{
			DebugWindowManager.OpenAssembler(args.Code, args.StartAddress, args.BlockLength);
		}

		private void CopyMenu(ToolStripItemCollection source, ToolStripItemCollection dest)
		{
			List<ToolStripItem> items = new List<ToolStripItem>();
			foreach(ToolStripItem item in source) {
				if(item is ToolStripSeparator) {
					ToolStripSeparator sep = new ToolStripSeparator();
					sep.Name = item.Name;
					sep.Visible = true;
					items.Add(sep);
				} else if(item is ToolStripMenuItem) {
					ToolStripMenuItem copy = new ToolStripMenuItem(item.Text, item.Image);
					copy.Name = item.Name;
					copy.Enabled = item.Enabled;
					copy.CheckOnClick = ((ToolStripMenuItem)item).CheckOnClick;
					copy.Checked = ((ToolStripMenuItem)item).Checked;
					copy.Visible = true;
					copy.Click += (s, e) => {
						item.PerformClick();
					};

					if(((ToolStripMenuItem)item).DropDownItems.Count > 0) {
						CopyMenu(((ToolStripMenuItem)item).DropDownItems, copy.DropDownItems);
					}

					if(item.Tag is ShortcutInfo) {
						copy.InitShortcut(this, ((ShortcutInfo)item.Tag).ShortcutKey);
					}

					items.Add(copy);
				} else {
					//Not used
				}
			}
			dest.AddRange(items.ToArray());
		}

		private void CleanupMenu(ToolStripItemCollection items)
		{
			foreach(ToolStripItem item in items) {
				if(item.Tag is ShortcutInfo && ((ShortcutInfo)item.Tag).KeyHandler != null) {
					DebuggerShortcutsConfig.ClearProcessCmdKeyHandler((ToolStripMenuItem)item, this);
				}

				if(item is ToolStripMenuItem && ((ToolStripMenuItem)item).HasDropDownItems) {
					CleanupMenu(((ToolStripMenuItem)item).DropDownItems);
				}
			}
		}

		private void UpdateCodeMenu()
		{
			CleanupMenu(mnuCode.DropDownItems);
			mnuCode.DropDownItems.Clear();
			CopyMenu(this.LastCodeWindow.CodeViewerActions.contextMenu.Items, mnuCode.DropDownItems);
			this.LastCodeWindow.CodeViewerActions.UpdateContextMenuItemVisibility(mnuCode.DropDownItems);
		}
		
		private void mnuCode_DropDownOpening(object sender, EventArgs e)
		{
			UpdateCodeMenu();
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
			Font newFont = FontDialogHelper.SelectFont(ctrlDebuggerCode.CodeViewer.BaseFont);

			ConfigManager.Config.DebugInfo.FontFamily = newFont.FontFamily.Name;
			ConfigManager.Config.DebugInfo.FontStyle = newFont.Style;
			ConfigManager.Config.DebugInfo.FontSize = newFont.Size;
			ConfigManager.ApplyChanges();

			ctrlDebuggerCode.CodeViewer.BaseFont = newFont;
			ctrlDebuggerCodeSplit.CodeViewer.BaseFont = newFont;
			ctrlSourceViewer.CodeViewer.BaseFont = newFont;
			ctrlSourceViewerSplit.CodeViewer.BaseFont = newFont;
		}

		private void mnuPreferences_Click(object sender, EventArgs e)
		{
			using(frmDbgPreferences frm = new frmDbgPreferences()) {
				frm.ShowDialog(sender, this);
			}
		}

		private void mnuImportSettings_Click(object sender, EventArgs e)
		{
			using(frmImportSettings frm = new frmImportSettings()) {
				frm.ShowDialog(sender, this);
			}
		}

		private void mnuReset_Click(object sender, EventArgs e)
		{
			InteropEmu.Reset();
		}

		private void mnuPowerCycle_Click(object sender, EventArgs e)
		{
			InteropEmu.PowerCycle();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if(lblPrgAnalysis != null) {
				if(this.Width - this._minimumSize.Width < 100) {
					if(lblPrgAnalysis.Text != "PRG:") {
						lblPrgAnalysis.Text = "PRG:";
						lblChrAnalysis.Text = "CHR:";
					}
				} else if(lblPrgAnalysis.Text == "PRG:") {
					lblPrgAnalysis.Text = "PRG analysis:";
					lblChrAnalysis.Text = "CHR analysis:";
				}
			}
		}

		private void mnuUseVerticalLayout_CheckedChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.VerticalLayout = this.mnuUseVerticalLayout.Checked;
			ConfigManager.ApplyChanges();

			if(mnuUseVerticalLayout.Checked) {
				this.ctrlSplitContainerTop.HidePanel2 = true;
				this.ctrlSplitContainerTop.CollapsePanel();
				this.tlpVerticalLayout.Controls.Add(this.grpLabels, 0, 0);
				this.tlpVerticalLayout.Controls.Add(this.grpFunctions, 1, 0);
			} else {
				this.tlpFunctionLabelLists.Controls.Add(this.grpLabels, 0, 1);
				this.tlpFunctionLabelLists.Controls.Add(this.grpFunctions, 0, 0);
				this.ctrlSplitContainerTop.HidePanel2 = false;
				this.ctrlSplitContainerTop.ExpandPanel();
			}

			mnuShowFunctionLabelLists.Enabled = !mnuUseVerticalLayout.Checked;
			this.UpdateMinimumSize();
		}

		private void mnuBreakOptions_DropDownOpening(object sender, EventArgs e)
		{
			this.mnuBreakOnDecayedOamRead.Enabled = ConfigManager.Config.EmulationInfo.EnableOamDecay;

			bool isNsf = InteropEmu.IsNsf();
			mnuBreakOnInit.Visible = isNsf;
			mnuBreakOnPlay.Visible = isNsf;
			sepBreakNsfOptions.Visible = isNsf;

			mnuBreakOnReset.Enabled = !isNsf;
		}

		private void frmDebugger_DragDrop(object sender, DragEventArgs e)
		{
			try {
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				if(files != null && File.Exists(files[0])) {
					ImportLabelFile(files[0]);
				}
			} catch(Exception ex) {
				MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
			}
		}

		private void frmDebugger_DragEnter(object sender, DragEventArgs e)
		{
			try {
				if(e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) {
					string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
					if(files != null && files.Length > 0) {
						string ext = Path.GetExtension(files[0]).ToLower();
						if(ext == ".dbg" || ext == ".mlb" || ext == ".fns") {
							e.Effect = DragDropEffects.Copy;
						}
					}
				}
			} catch(Exception ex) {
				MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
			}
		}

		private ICodeViewer GetAlternateView(ICodeViewer viewer)
		{
			if(viewer == ctrlDebuggerCode) {
				return ctrlSourceViewer;
			} else if(viewer == ctrlSourceViewer) {
				return ctrlDebuggerCode;
			} else if(viewer == ctrlDebuggerCodeSplit) {
				return ctrlSourceViewerSplit;
			} else if(viewer == ctrlSourceViewerSplit) {
				return ctrlDebuggerCodeSplit;
			}
			return null;
		}

		private void GoToDestination(ICodeViewer target, GoToDestination dest)
		{
			if(target is ctrlSourceViewer && !string.IsNullOrWhiteSpace(dest.File)) {
				((ctrlSourceViewer)target).ScrollToFileLine(dest.File, dest.Line);
			} else if(dest.CpuAddress >= 0) {
				target.ScrollToLineNumber(dest.CpuAddress);
			} else if(!string.IsNullOrWhiteSpace(dest.File)) {
				if(!(target is ctrlSourceViewer)) {
					ctrlDebuggerCode_OnSwitchView(target);
					target = GetAlternateView(target);
				}
				if(target is ctrlSourceViewer) {
					((ctrlSourceViewer)target).ScrollToFileLine(dest.File, dest.Line);
				}
			} else {
				AddressTypeInfo addressInfo = dest.AddressInfo;
				if(addressInfo == null && dest.Label != null) {
					addressInfo = new AddressTypeInfo() { Address = (int)dest.Label.Address, Type = dest.Label.AddressType };
				}

				if(InteropEmu.DebugGetRelativeAddress((uint)addressInfo.Address, addressInfo.Type) < 0) {
					//Try to display the label in the source view if possible (when code is out of scope)
					if(ctrlSourceViewer.CurrentFile != null && !(target is ctrlSourceViewer)) {
						ctrlDebuggerCode_OnSwitchView(target);
						target = GetAlternateView(target);
					}
				}
				target.ScrollToAddress(addressInfo);
			}

			if(WindowState == FormWindowState.Minimized) {
				//Unminimize window if it was minimized
				WindowState = FormWindowState.Normal;
			}
			BringToFront();
		}

		public void GoToDestination(GoToDestination dest)
		{
			GoToDestination(LastCodeWindow, dest);
		}

		private void mnuGoToAll_Click(object sender, EventArgs e)
		{
			using(frmGoToAll frm = new frmGoToAll(false, true)) {
				if(frm.ShowDialog() == DialogResult.OK) {
					GoToDestination(_lastCodeWindow, frm.Destination);

					if(Program.IsMono) {
						//Delay by 150ms before giving focus when running on Mono
						//Otherwise this doesn't work (presumably because Mono sets the debugger form to disabled while the popup is opened)
						Task.Run(() => {
							System.Threading.Thread.Sleep(150);
							this.BeginInvoke((Action)(() => {
								_lastCodeWindow.CodeViewer.Focus();
							}));
						});
					} else {
						_lastCodeWindow.CodeViewer.Focus();
					}
				}
			}
		}

		private void mnuConfigureExternalEditor_Click(object sender, EventArgs e)
		{
			using(frmExternalEditorConfig frm = new frmExternalEditorConfig()) {
				frm.ShowDialog(mnuConfigureExternalEditor, this);
			}
		}

		private void mnuOpenChrViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenPpuViewer(PpuViewerMode.ChrViewer);
		}

		private void mnuOpenNametableViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenPpuViewer(PpuViewerMode.NametableViewer);
		}

		private void mnuOpenSpriteViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenPpuViewer(PpuViewerMode.SpriteViewer);
		}

		private void mnuOpenPaletteViewer_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenPpuViewer(PpuViewerMode.PaletteViewer);
		}

		private void mnuWatchWindow_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.WatchWindow);
		}
	}
}
