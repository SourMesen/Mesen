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
		private List<Form> _childForms = new List<Form>();
		private bool _debuggerInitialized = false;
		private bool _firstBreak = true;

		private InteropEmu.NotificationListener _notifListener;
		private ctrlDebuggerCode _lastCodeWindow;
		private frmTraceLogger _traceLogger;
		private DebugWorkspace _workspace;

		public frmDebugger()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
 			base.OnLoad(e);
			 
			if(Program.IsMono) {
				//This doesn't work in Mono (menu is blank) - hide it for now
				mnuCode.Visible = false;
			}

			ctrlConsoleStatus.OnStateChanged += ctrlConsoleStatus_OnStateChanged;
			LabelManager.OnLabelUpdated += LabelManager_OnLabelUpdated;
			BreakpointManager.BreakpointsChanged += BreakpointManager_BreakpointsChanged;
			ctrlProfiler.OnFunctionSelected += ctrlProfiler_OnFunctionSelected;

			this.UpdateWorkspace();
			this.AutoLoadDbgFile(true);

			this.mnuSplitView.Checked = ConfigManager.Config.DebugInfo.SplitView;
			this.mnuPpuPartialDraw.Checked = ConfigManager.Config.DebugInfo.PpuPartialDraw;
			this.mnuShowEffectiveAddresses.Checked = ConfigManager.Config.DebugInfo.ShowEffectiveAddresses;
			this.mnuShowCpuMemoryMapping.Checked = ConfigManager.Config.DebugInfo.ShowCpuMemoryMapping;
			this.mnuShowPpuMemoryMapping.Checked = ConfigManager.Config.DebugInfo.ShowPpuMemoryMapping;
			this.mnuShowOnlyDisassembledCode.Checked = ConfigManager.Config.DebugInfo.ShowOnlyDisassembledCode;
			this.mnuHighlightUnexecutedCode.Checked = ConfigManager.Config.DebugInfo.HighlightUnexecutedCode;
			this.mnuAutoLoadDbgFiles.Checked = ConfigManager.Config.DebugInfo.AutoLoadDbgFiles;
			this.mnuBreakOnReset.Checked = ConfigManager.Config.DebugInfo.BreakOnReset;
			this.mnuBreakOnOpen.Checked = ConfigManager.Config.DebugInfo.BreakOnOpen;
			this.mnuBreakOnUnofficialOpcodes.Checked = ConfigManager.Config.DebugInfo.BreakOnUnofficialOpcodes;
			this.mnuBreakOnBrk.Checked = ConfigManager.Config.DebugInfo.BreakOnBrk;
			this.mnuDisplayOpCodesInLowerCase.Checked = ConfigManager.Config.DebugInfo.DisplayOpCodesInLowerCase;
			this.mnuDisassembleVerifiedCodeOnly.Checked = ConfigManager.Config.DebugInfo.DisassemblyType == DisassemblyType.VerifiedCode;
			this.mnuDisassembleEverything.Checked = ConfigManager.Config.DebugInfo.DisassemblyType == DisassemblyType.Everything;
			this.mnuDisassembleEverythingButData.Checked = ConfigManager.Config.DebugInfo.DisassemblyType == DisassemblyType.EverythingButData;
			this.mnuRefreshWatchWhileRunning.Checked = ConfigManager.Config.DebugInfo.RefreshWatchWhileRunning;

			if(ConfigManager.Config.DebugInfo.WindowWidth > -1) {
				this.Width = ConfigManager.Config.DebugInfo.WindowWidth;
				this.Height = ConfigManager.Config.DebugInfo.WindowHeight;
			}

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

			this.toolTip.SetToolTip(this.picWatchHelp,
				"Most expressions/operators are accepted (C++ syntax)." + Environment.NewLine +
				"Note: Use the $ prefix to denote hexadecimal values." + Environment.NewLine + 
				"Note 2: Labels assigned to the code can be used (their value will match the label's address in CPU memory)." + Environment.NewLine + Environment.NewLine +
				"A/X/Y/PS/SP: Value of registers" + Environment.NewLine +
				"PC: Program Counter" + Environment.NewLine +
				"OpPC: Address of the current instruction's first byte" + Environment.NewLine +
				"Irq/Nmi: True if the Irq/Nmi flags are set" + Environment.NewLine +
				"Cycle/Scanline: Current cycle (0-340)/scanline(-1 to 260) of the PPU" + Environment.NewLine +
				"Frame: PPU frame number (since power on/reset)" + Environment.NewLine +
				"Value: Current value being read/written from/to memory" + Environment.NewLine +
				"[<address>]: (Byte) Memory value at <address> (CPU)" + Environment.NewLine +
				"{<address>}: (Word) Memory value at <address> (CPU)" + Environment.NewLine + Environment.NewLine +

				"Examples:" + Environment.NewLine +
				"a == 10 || x == $23" + Environment.NewLine +
				"scanline == 10 && (cycle >= 55 && cycle <= 100)" + Environment.NewLine +
				"x == [$150] || y == [10]" + Environment.NewLine +
				"[[$15] + y]   -> Reads the value at address $15, adds Y to it and reads the value at the resulting address." + Environment.NewLine +
				"{$FFFA}  -> Returns the NMI handler's address."
			);

			_notifListener = new InteropEmu.NotificationListener();
			_notifListener.OnNotification += _notifListener_OnNotification;

			InteropEmu.DebugInitialize();

			_debuggerInitialized = true;

			//Pause a few frames later to give the debugger a chance to disassemble some code
			_firstBreak = true;
			InteropEmu.DebugStep(30000);

			UpdateCdlRatios();
			tmrCdlRatios.Start();

			mnuSaveRom.Enabled = InteropEmu.GetRomInfo().Format == RomFormat.iNes;
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if(ConfigManager.Config.DebugInfo.BreakOnDebuggerFocus) {
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

		private void AutoLoadDbgFile(bool silent)
		{
			if(ConfigManager.Config.DebugInfo.AutoLoadDbgFiles) {
				string dbgPath = Path.Combine(ConfigManager.Config.RecentFiles[0].RomFile.Folder, Path.GetFileNameWithoutExtension(ConfigManager.Config.RecentFiles[0].RomFile.FileName) + ".dbg");
				if(File.Exists(dbgPath)) {
					Ld65DbgImporter dbgImporter = new Ld65DbgImporter();
					dbgImporter.Import(dbgPath, silent);
				}
			}
		}

		private void SaveWorkspace()
		{
			if(_workspace != null) {
				_workspace.WatchValues = WatchManager.WatchEntries;
				_workspace.Labels = LabelManager.GetLabels();
				_workspace.Breakpoints = BreakpointManager.Breakpoints;
				_workspace.Save();
			}
		}

		private void UpdateWorkspace()
		{
			SaveWorkspace();
			
			_workspace = DebugWorkspace.GetWorkspace();

			LabelManager.OnLabelUpdated -= LabelManager_OnLabelUpdated;
			if(_workspace.Labels.Count == 0) {
				LabelManager.ResetLabels();
				if(!ConfigManager.Config.DebugInfo.DisableDefaultLabels) {
					LabelManager.SetDefaultLabels(InteropEmu.FdsGetSideCount() > 0);
				}
			} else {
				LabelManager.ResetLabels();
				LabelManager.SetLabels(_workspace.Labels);
			}
			LabelManager.OnLabelUpdated += LabelManager_OnLabelUpdated;

			ctrlLabelList.UpdateLabelList();
			ctrlFunctionList.UpdateFunctionList(true);

			WatchManager.WatchEntries = _workspace.WatchValues;

			BreakpointManager.Breakpoints.Clear();
			BreakpointManager.Breakpoints.AddRange(_workspace.Breakpoints);
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

		private void UpdateDebuggerFlags()
		{
			DebuggerFlags flags = mnuPpuPartialDraw.Checked ? DebuggerFlags.PpuPartialDraw : DebuggerFlags.None;
			if(mnuShowEffectiveAddresses.Checked) {
				flags |= DebuggerFlags.ShowEffectiveAddresses;
			}
			if(mnuShowOnlyDisassembledCode.Checked) {
				flags |= DebuggerFlags.ShowOnlyDisassembledCode;
			}
			if(mnuDisplayOpCodesInLowerCase.Checked) {
				flags |= DebuggerFlags.DisplayOpCodesInLowerCase;
			}
			if(mnuDisassembleEverything.Checked) {
				flags |= DebuggerFlags.DisassembleEverything;
			} else if(mnuDisassembleEverythingButData.Checked) {
				flags |= DebuggerFlags.DisassembleEverythingButData;
			}
			if(mnuBreakOnUnofficialOpcodes.Checked) {
				flags |= DebuggerFlags.BreakOnUnofficialOpCode;
			}
			if(mnuBreakOnBrk.Checked) {
				flags |= DebuggerFlags.BreakOnBrk;
			}
			InteropEmu.DebugSetFlags(flags);
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
					this.BeginInvoke((MethodInvoker)(() => UpdateDebugger()));
					BreakpointManager.SetBreakpoints();
					break;

				case InteropEmu.ConsoleNotificationType.GameReset:
				case InteropEmu.ConsoleNotificationType.GameLoaded:
					this.BeginInvoke((MethodInvoker)(() => {
						mnuSaveRom.Enabled = InteropEmu.GetRomInfo().Format == RomFormat.iNes;

						this.UpdateWorkspace();
						this.AutoLoadDbgFile(true);
						UpdateDebugger();
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
				this.MinimumSize = new Size(1250, 725);
			} else {
				tlpTop.ColumnStyles[1].SizeType = SizeType.Absolute;
				tlpTop.ColumnStyles[1].Width = 0f;
				this.MinimumSize = new Size(1000, 725);
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

		private void UpdateDebugger(bool updateActiveAddress = true)
		{
			if(!_debuggerInitialized) {
				return;
			}

			ctrlLabelList.UpdateLabelListAddresses();
			ctrlFunctionList.UpdateFunctionList(false);
			UpdateDebuggerFlags();
			UpdateVectorAddresses();

			string newCode = InteropEmu.DebugGetCode();
			if(newCode != null) {
				ctrlDebuggerCode.Code = newCode;
			}

			DebugState state = new DebugState();
			InteropEmu.DebugGetState(ref state);

			if(UpdateSplitView()) {
				if(newCode != null || ctrlDebuggerCodeSplit.Code == null) {
					ctrlDebuggerCodeSplit.Code = ctrlDebuggerCode.Code;
				}
				ctrlDebuggerCodeSplit.UpdateCode(true);
			} else {
				_lastCodeWindow = ctrlDebuggerCode;
			}

			ctrlDebuggerCode.SetActiveAddress(state.CPU.DebugPC);
			ctrlDebuggerCodeSplit.SetActiveAddress(state.CPU.DebugPC);

			if(updateActiveAddress) {
				_lastCodeWindow.SelectActiveAddress(state.CPU.DebugPC);
			}

			UpdateLineColors();

			ctrlConsoleStatus.UpdateStatus(ref state);
			ctrlWatch.UpdateWatch();
			ctrlCallstack.UpdateCallstack();

			ctrlCpuMemoryMapping.UpdateCpuRegions(state.Cartridge);
			ctrlPpuMemoryMapping.UpdatePpuRegions(state.Cartridge);

			this.BringToFront();

			if(_firstBreak && !ConfigManager.Config.DebugInfo.BreakOnOpen) {
				ResumeExecution();
			}

			_firstBreak = false;
		}
		private void ClearActiveStatement()
		{
			ctrlDebuggerCode.ClearActiveAddress();
			ctrlDebuggerCodeSplit.ClearActiveAddress();
			UpdateLineColors();
		}

		private void ToggleBreakpoint(bool toggleEnabled)
		{
			BreakpointManager.ToggleBreakpoint(_lastCodeWindow.GetCurrentLine(), toggleEnabled);
		}
		
		private void UpdateLineColors()
		{
			ctrlDebuggerCodeSplit.UpdateLineColors();
			ctrlDebuggerCode.UpdateLineColors();
		}

		private void OpenChildForm(Form frm)
		{
			this._childForms.Add(frm);
			frm.FormClosed += (obj, args) => {
				this._childForms.Remove((Form)obj);
			};
			frm.StartPosition = FormStartPosition.Manual;
			frm.Left = this.Left + this.Width / 2 - frm.Width / 2;
			frm.Top = this.Top + this.Height / 2 - frm.Height / 2;
			frm.Show();
		}

		private void ResumeExecution()
		{
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

		private void ctrlDebuggerCode_OnSetNextStatement(AddressEventArgs args)
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
			OpenChildForm(new frmMemoryViewer(() => { return _workspace; }));
		}

		private void BreakpointManager_BreakpointsChanged(object sender, EventArgs e)
		{
			UpdateLineColors();
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

		private void mnuIncreaseFontSize_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FontSize++;
		}

		private void mnuDecreaseFontSize_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FontSize--;
		}

		private void mnuResetFontSize_Click(object sender, EventArgs e)
		{
			_lastCodeWindow.FontSize = BaseControl.DefaultFontSize;
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			tmrCdlRatios.Stop();
			foreach(Form frm in this._childForms.ToArray()) {
				frm.Close();
			}

			LabelManager.OnLabelUpdated -= LabelManager_OnLabelUpdated;
			BreakpointManager.BreakpointsChanged -= BreakpointManager_BreakpointsChanged;
			ctrlConsoleStatus.OnStateChanged -= ctrlConsoleStatus_OnStateChanged;
			ctrlProfiler.OnFunctionSelected -= ctrlProfiler_OnFunctionSelected;

			if(_notifListener != null) {
				_notifListener.Dispose();
				_notifListener = null;
			}

			InteropEmu.DebugRelease();

			ConfigManager.Config.DebugInfo.WindowWidth = this.Width;
			ConfigManager.Config.DebugInfo.WindowHeight = this.Height;
			ConfigManager.Config.DebugInfo.TopPanelHeight = this.splitContainer.GetSplitterDistance();
			ConfigManager.Config.DebugInfo.LeftPanelWidth = this.ctrlSplitContainerTop.GetSplitterDistance();
			ConfigManager.ApplyChanges();

			SaveWorkspace();

			base.OnFormClosed(e);
		}

		private void mnuNametableViewer_Click(object sender, EventArgs e)
		{
			OpenChildForm(new frmPpuViewer());
		}

		private void ctrlCallstack_FunctionSelected(object sender, EventArgs e)
		{
			_lastCodeWindow.ScrollToLineNumber((int)sender);
		}

		private void ctrlConsoleStatus_OnStateChanged(object sender, EventArgs e)
		{
			UpdateDebugger(true);
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
		}

		private void ctrlBreakpoints_BreakpointNavigation(object sender, EventArgs e)
		{
			_lastCodeWindow.ScrollToLineNumber((int)((Breakpoint)sender).Address);
		}

		private void mnuTraceLogger_Click(object sender, EventArgs e)
		{
			if(_traceLogger == null) {
				_traceLogger = new frmTraceLogger();
				_traceLogger.FormClosed += (s, evt) => {
					_traceLogger = null;
				};
				OpenChildForm(_traceLogger);
			} else {
				_traceLogger.Focus();
			}
		}

		private void mnuPpuPartialDraw_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.PpuPartialDraw = mnuPpuPartialDraw.Checked;
			ConfigManager.ApplyChanges();
		}
		
		private void mnuShowEffectiveAddresses_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowEffectiveAddresses = mnuShowEffectiveAddresses.Checked;
			ConfigManager.ApplyChanges();
			UpdateDebugger(false);
		}

		private void mnuShowOnlyDisassembledCode_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.ShowOnlyDisassembledCode = mnuShowOnlyDisassembledCode.Checked;
			ConfigManager.ApplyChanges();
			UpdateDebugger(false);
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

		private void mnuHighlightUnexecutedCode_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.HighlightUnexecutedCode = mnuHighlightUnexecutedCode.Checked;
			ConfigManager.ApplyChanges();
			ctrlDebuggerCode.UpdateLineColors();
			ctrlDebuggerCodeSplit.UpdateLineColors();
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
		
		private void mnuBreakOnDebuggerFocus_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.BreakOnDebuggerFocus = mnuBreakOnDebuggerFocus.Checked;
			ConfigManager.ApplyChanges();
		}

		private void mnuRefreshWatchWhileRunning_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.DebugInfo.RefreshWatchWhileRunning = mnuRefreshWatchWhileRunning.Checked;
			ConfigManager.ApplyChanges();
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
			SaveWorkspace();
			ctrlLabelList.UpdateLabelList();
			ctrlFunctionList.UpdateFunctionList(true);
			UpdateDebugger(false);
		}

		private void ctrlLabelList_OnLabelSelected(object relativeAddress, EventArgs e)
		{
			_lastCodeWindow.ScrollToLineNumber((Int32)relativeAddress);
		}

		private void mnuResetWorkspace_Click(object sender, EventArgs e)
		{
			if(MessageBox.Show("This operation will empty the watch window, remove all breakpoints, and reset labels to their default state." + Environment.NewLine + "Are you sure?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
				_workspace.Breakpoints = new List<Breakpoint>();
				_workspace.Labels = new List<CodeLabel>();
				_workspace.WatchValues = new List<string>();
				_workspace.Save();
				_workspace = null;
				LabelManager.ResetLabels();
				UpdateWorkspace();
				UpdateDebugger(false);
			}
		}

		private void mnuImportLabels_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.SetFilter("All supported files (*.dbg)|*.dbg");
			if(ofd.ShowDialog() == DialogResult.OK) {
				Ld65DbgImporter dbgImporter = new Ld65DbgImporter();
				dbgImporter.Import(ofd.FileName);
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

				AutoLoadDbgFile(false);
			}
		}

		private void ctrlConsoleStatus_OnGotoLocation(object sender, EventArgs e)
		{
			_lastCodeWindow.ScrollToLineNumber((int)sender);
		}

		private void SetDisassemblyType(DisassemblyType type, ToolStripMenuItem item)
		{
			mnuDisassembleVerifiedCodeOnly.Checked = mnuDisassembleEverything.Checked = mnuDisassembleEverythingButData.Checked = false;
			item.Checked = true;

			ConfigManager.Config.DebugInfo.DisassemblyType = type;
			ConfigManager.ApplyChanges();

			UpdateDebuggerFlags();
			UpdateDebugger();
		}

		private void mnuDisassembleVerifiedCodeOnly_Click(object sender, EventArgs e)
		{
			SetDisassemblyType(DisassemblyType.VerifiedCode, sender as ToolStripMenuItem);
		}

		private void mnuDisassembleEverything_Click(object sender, EventArgs e)
		{
			SetDisassemblyType(DisassemblyType.Everything, sender as ToolStripMenuItem);
		}

		private void mnuDisassembleEverythingButData_Click(object sender, EventArgs e)
		{
			SetDisassemblyType(DisassemblyType.EverythingButData, sender as ToolStripMenuItem);
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
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.Filter = "NES roms (*.nes)|*.nes";
				sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + "_Modified.nes";
				sfd.InitialDirectory = ConfigManager.DebuggerFolder;
				if(sfd.ShowDialog() == DialogResult.OK) {
					InteropEmu.DebugSaveRomToDisk(sfd.FileName);
				}
			}
		}

		private void OpenAssembler(string code, UInt16 startAddress, UInt16 blockLength = 0)
		{
			frmAssembler assembler = new frmAssembler(code, startAddress, blockLength);
			assembler.FormClosed += (s, e) => {
				if(assembler.DialogResult == DialogResult.OK) {
					this.UpdateDebugger(false);
				}
			};
			OpenChildForm(assembler);
		}

		private void mnuAssembler_Click(object sender, EventArgs e)
		{
			this.OpenAssembler(string.Empty, 0x8000);
		}

		private void ctrlDebuggerCode_OnEditCode(AssemblerEventArgs args)
		{
			this.OpenAssembler(args.Code, args.StartAddress, args.BlockLength);
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
	}
}
