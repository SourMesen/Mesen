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
using FastColoredTextBoxNS;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;
using Mesen.GUI.Forms;
using Mesen.GUI.Properties;

namespace Mesen.GUI.Debugger
{
	public partial class frmScript : BaseForm
	{
		private InteropEmu.NotificationListener _notifListener;
		private int _scriptId = -1;
		private string _filePath = null;
		private DateTime _lastTimestamp = DateTime.MinValue;
		private AutocompleteMenu _popupMenu;
		private string _originalText = "";

		public frmScript()
		{
			InitializeComponent();

			_popupMenu = new AutocompleteMenu(txtScriptContent, this);
			_popupMenu.ImageList = new ImageList();
			_popupMenu.ImageList.Images.Add(Resources.Enum);
			_popupMenu.ImageList.Images.Add(Resources.Function);
			_popupMenu.SelectedColor = Color.LightBlue;
			_popupMenu.SearchPattern = @"[\w\.]";

			List<AutocompleteItem> items = new List<AutocompleteItem>();
			_availableFunctions.Sort((a, b) => {
				int type = a[0].CompareTo(b[0]);
				if(type == 0) {
					return a[1].CompareTo(b[1]);
				} else {
					return -type;
				}
			});

			foreach(List<string> item in _availableFunctions) {
				MethodAutocompleteItem autocompleteItem = new MethodAutocompleteItem(item[1]);
				autocompleteItem.ImageIndex = item[0] == "func" ? 1 : 0;
				autocompleteItem.ToolTipTitle = item[2];
				if(!string.IsNullOrWhiteSpace(item[3])) {
					autocompleteItem.ToolTipText = "Parameters" + Environment.NewLine + item[3] + Environment.NewLine + Environment.NewLine;
				}
				if(!string.IsNullOrWhiteSpace(item[4])) {
					autocompleteItem.ToolTipText += "Return Value" + Environment.NewLine + item[4] + Environment.NewLine + Environment.NewLine;
				}
				if(!string.IsNullOrWhiteSpace(item[5])) {
					autocompleteItem.ToolTipText += "Description" + Environment.NewLine + item[5] + Environment.NewLine + Environment.NewLine;
				}
				items.Add(autocompleteItem);
			}

			_popupMenu.Items.SetAutocompleteItems(items);

			UpdateRecentScripts();

			if(!ConfigManager.Config.DebugInfo.ScriptWindowSize.IsEmpty) {
				this.Size = ConfigManager.Config.DebugInfo.ScriptWindowSize;
			}
			mnuSaveBeforeRun.Checked = ConfigManager.Config.DebugInfo.SaveScriptBeforeRun;
			mnuAutoReload.Checked = ConfigManager.Config.DebugInfo.AutoReloadScript;
			if(ConfigManager.Config.DebugInfo.ScriptCodeWindowHeight >= ctrlSplit.Panel1MinSize) {
				if(ConfigManager.Config.DebugInfo.ScriptCodeWindowHeight == Int32.MaxValue) {
					ctrlSplit.CollapsePanel();
				} else {
					ctrlSplit.SplitterDistance = ConfigManager.Config.DebugInfo.ScriptCodeWindowHeight;
				}
			}
			txtScriptContent.Font = new Font(BaseControl.MonospaceFontFamily, 10);
			txtScriptContent.Zoom = ConfigManager.Config.DebugInfo.ScriptZoom;

			if(!this.DesignMode) {
				this._notifListener = new InteropEmu.NotificationListener();
				this._notifListener.OnNotification += this._notifListener_OnNotification;
			}
		}

		private void _notifListener_OnNotification(InteropEmu.NotificationEventArgs e)
		{
			if(e.NotificationType == InteropEmu.ConsoleNotificationType.GameStopped) {
				this._scriptId = -1;
				lblScriptActive.Visible = false;
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if(_originalText != txtScriptContent.Text) {
				DialogResult result = MessageBox.Show("You have unsaved changes for this script - would you like to save them?", "Script Window", MessageBoxButtons.YesNoCancel);
				if((result == DialogResult.Yes && !SaveScript()) || result == DialogResult.Cancel) {
					e.Cancel = true;
					return;
				}
			}

			if(_scriptId >= 0) {
				InteropEmu.DebugRemoveScript(_scriptId);
			}
			ConfigManager.Config.DebugInfo.ScriptWindowSize = this.WindowState == FormWindowState.Maximized ? this.RestoreBounds.Size : this.Size;
			ConfigManager.Config.DebugInfo.SaveScriptBeforeRun = mnuSaveBeforeRun.Checked;
			ConfigManager.Config.DebugInfo.AutoReloadScript = mnuAutoReload.Checked;
			ConfigManager.Config.DebugInfo.ScriptCodeWindowHeight = ctrlSplit.Panel2.Height <= 2 ? Int32.MaxValue : ctrlSplit.SplitterDistance;
			ConfigManager.Config.DebugInfo.ScriptZoom = txtScriptContent.Zoom;
			ConfigManager.ApplyChanges();

			base.OnClosing(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == Keys.Escape) {
				StopScript();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void LoadScript()
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter("LUA scripts (*.lua)|*.lua");
				if(ofd.ShowDialog() == DialogResult.OK) {
					LoadScriptFile(ofd.FileName);
				}
			}
		}

		private void LoadScriptFile(string filepath)
		{
			if(File.Exists(filepath)) {
				string content = File.ReadAllText(filepath);
				SetFilePath(filepath);
				txtScriptContent.Text = content;
				txtScriptContent.ClearUndo();
				ConfigManager.Config.DebugInfo.AddRecentScript(filepath);
				UpdateRecentScripts();
				RunScript();

				_originalText = txtScriptContent.Text;
				_lastTimestamp = File.GetLastWriteTime(filepath);
			}
		}

		private void UpdateRecentScripts()
		{
			mnuRecentScripts.DropDownItems.Clear();
			foreach(string recentScript in ConfigManager.Config.DebugInfo.RecentScripts) {
				ToolStripMenuItem tsmi = new ToolStripMenuItem();
				tsmi.Text = Path.GetFileName(recentScript);
				tsmi.Click += (object sender, EventArgs args) => {
					LoadScriptFile(recentScript);
				};
				mnuRecentScripts.DropDownItems.Add(tsmi);
			}

			mnuRecentScripts.Enabled = mnuRecentScripts.DropDownItems.Count > 0;
		}

		private void RunScript()
		{
			if(_filePath != null && mnuSaveBeforeRun.Checked && txtScriptContent.UndoEnabled) {
				txtScriptContent.SaveToFile(_filePath, Encoding.UTF8);
			}

			_scriptId = InteropEmu.DebugLoadScript(txtScriptContent.Text, _scriptId);
			if(_scriptId < 0) {
				MessageBox.Show("Error while loading script.");
			} else {
				lblScriptActive.Visible = true;
			}
		}

		private void StopScript()
		{
			_scriptId = InteropEmu.DebugLoadScript(string.Empty, _scriptId);
			if(_scriptId < 0) {
				MessageBox.Show("Error while stopping script.");
			} else {
				lblScriptActive.Visible = false;
			}
		}

		private bool SaveScript()
		{
			if(_filePath != null && txtScriptContent.UndoEnabled) {
				txtScriptContent.SaveToFile(_filePath, Encoding.UTF8);
				_originalText = txtScriptContent.Text;
				return true;
			} else {
				return SaveAs("NewScript.lua");
			}
		}

		private bool SaveAs(string newName)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.FileName = newName;
				sfd.SetFilter("LUA scripts (*.lua)|*.lua");
				if(sfd.ShowDialog() == DialogResult.OK) {
					SetFilePath(sfd.FileName);
					txtScriptContent.SaveToFile(_filePath, Encoding.UTF8);
					ConfigManager.Config.DebugInfo.AddRecentScript(sfd.FileName);
					UpdateRecentScripts();
					_originalText = txtScriptContent.Text;
					return true;
				}
			}
			return false;
		}

		private void SetFilePath(string filePath)
		{
			_filePath = filePath;
			this.Text = $"{Path.GetFileName(_filePath)} - Script Window";
		}

		private void mnuOpen_Click(object sender, EventArgs e)
		{
			LoadScript();
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			RunScript();
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void mnuRun_Click(object sender, EventArgs e)
		{
			RunScript();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			SaveScript();
		}

		private void mnuSaveAs_Click(object sender, EventArgs e)
		{
			SaveAs(Path.GetFileName(this._filePath));
		}

		private void btnOpen_Click(object sender, EventArgs e)
		{
			LoadScript();
		}

		private void mnuStop_Click(object sender, EventArgs e)
		{
			StopScript();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			StopScript();
		}

		private void tmrLog_Tick(object sender, EventArgs e)
		{
			if(_scriptId >= 0) {
				string newLog = InteropEmu.DebugGetScriptLog(_scriptId);
				if(txtLog.Text != newLog) {
					txtLog.SuspendLayout();
					txtLog.Text = newLog;
					txtLog.SelectionStart = newLog.Length;
					txtLog.ScrollToCaret();
					txtLog.ResumeLayout();
				}
			}

			mnuSave.Enabled = btnSave.Enabled = txtScriptContent.Text != _originalText;

			if(_filePath != null && File.Exists(_filePath) && mnuAutoReload.Checked) {
				if(_lastTimestamp < File.GetLastWriteTime(_filePath)) {
					LoadScriptFile(_filePath);
				}
			}
		}

		private void mnuView_DropDownOpening(object sender, EventArgs e)
		{
			mnuShowLogWindow.Checked = ctrlSplit.Panel2.Height > 5;
		}

		private void mnuShowLogWindow_Click(object sender, EventArgs e)
		{
			if(mnuShowLogWindow.Checked) {
				ctrlSplit.ExpandPanel();
			} else {
				ctrlSplit.CollapsePanel();
			}
		}

		private void mnuCopy_Click(object sender, EventArgs e)
		{
			txtScriptContent.Copy();
		}

		private void mnuCut_Click(object sender, EventArgs e)
		{
			txtScriptContent.Cut();
		}

		private void mnuPaste_Click(object sender, EventArgs e)
		{
			txtScriptContent.Paste();
		}

		private void mnuIncreaseFontSize_Click(object sender, EventArgs e)
		{
			txtScriptContent.ChangeFontSize(2);
		}

		private void mnuDecreaseFontSize_Click(object sender, EventArgs e)
		{
			txtScriptContent.ChangeFontSize(-2);
		}

		private void mnuResetFontSize_Click(object sender, EventArgs e)
		{
			txtScriptContent.RestoreFontSize();
		}

		private void mnuNewScript_Click(object sender, EventArgs e)
		{
			DebugWindowManager.OpenDebugWindow(DebugWindow.ScriptWindow);
		}

		static readonly List<List<string>> _availableFunctions = new List<List<string>>() {
			new List<string> {"enum", "emu", "", "", "", "", "" },
			new List<string> {"func","emu.addEventCallback","emu.addEventCallback(function, type)","function - A LUA function.\ntype - *Enum* See eventCallbackType.","Returns an integer value that can be used to remove the callback by calling removeEventCallback.","Registers a callback function to be called whenever the specified event occurs.",},
			new List<string> {"func","emu.removeEventCallback","emu.removeEventCallback(reference, type)","reference - The value returned by the call to[addEventCallback] (#addEventCallback).\ntype - *Enum* See eventCallbackType.","","Removes a previously registered callback function.",},
			new List<string> {"func","emu.addMemoryCallback","emu.addMemoryCallback(function, type, startAddress, endAddress)","function - A LUA function.\ntype - *Enum* See memCallbackType\nstartAddress - *Integer* Start of the CPU memory address range to register the callback on.\nendAddress - *Integer* End of the CPU memory address range to register the callback on.","Returns an integer value that can be used to remove the callback by callingremoveMemoryCallback.","Registers a callback function to be called whenever the specified event occurs."},
			new List<string> {"func","emu.removeMemoryCallback","emu.removeMemoryCallback(reference, type, startAddress, endAddress)","reference - The value returned by the call to[addMemoryCallback] (#addMemoryCallback).\ntype - *Enum* See memCallbackType.\nstartAddress - *Integer* Start of the CPU memory address range to unregister the callback from.\nendAddress - *Integer* End of the CPU memory address range to unregister the callback from.","","Removes a previously registered callback function."},
			new List<string> {"func","emu.read","emu.read(address, type)","address - *Integer* The address/offset to read from.\ntype - *Enum* The type of memory to read from. See memType.","An 8-bit (read) or 16-bit (readWord) value.","Reads a value from the specified memory type.\nThe read / readWord variants may cause side-effects that can alter the emulation's behavior.\nThe debugRead/debugReadWord variants have no side-effects."},
			new List<string> {"func","emu.readWord","emu.readWord(address, type)","address - *Integer* The address/offset to read from.\ntype - *Enum* The type of memory to read from. See memType.","An 8-bit (read) or 16-bit (readWord) value.","Reads a value from the specified memory type.\nThe read / readWord variants may cause side-effects that can alter the emulation's behavior.\nThe debugRead/debugReadWord variants have no side-effects."},
			new List<string> {"func","emu.debugRead","emu.debugRead(address, type)","address - *Integer* The address/offset to read from.\ntype - *Enum* The type of memory to read from. See memType.","An 8-bit (read) or 16-bit (readWord) value.","Reads a value from the specified memory type.\nThe read / readWord variants may cause side-effects that can alter the emulation's behavior.\nThe debugRead/debugReadWord variants have no side-effects."},
			new List<string> {"func","emu.debugReadWord","emu.debugReadWord(address, type)","address - *Integer* The address/offset to read from.\ntype - *Enum* The type of memory to read from. See memType.","An 8-bit (read) or 16-bit (readWord) value.","Reads a value from the specified memory type.\nThe read / readWord variants may cause side-effects that can alter the emulation's behavior.\nThe debugRead/debugReadWord variants have no side-effects."},
			new List<string> {"func","emu.write","emu.write(address, value, type)","address - *Integer* The address/offset to write to.\nvalue - *Integer* The value to write.\ntype - *Enum* The type of memory to write to. See memType.","","Writes an 8-bit or 16-bit value to the specified memory type.\nNormally read-only types such as PRG-ROM or CHR-ROM can be written to when using [memType.prgRom]\n(#memType) or memType.chrRom.\nChanges will remain in effect until a power cycle occurs.\nTo revert changes done to ROM, see revertPrgChrChanges.\nThe write / writeWord variants may cause side-effects that can alter the emulation's behavior.\nThe debugWrite/debugWriteWord variants have no side-effects."},
			new List<string> {"func","emu.writeWord","emu.writeWord(address, value, type)","address - *Integer* The address/offset to write to.\nvalue - *Integer* The value to write.\ntype - *Enum* The type of memory to write to. See memType.","","Writes an 8-bit or 16-bit value to the specified memory type.\nNormally read-only types such as PRG-ROM or CHR-ROM can be written to when using [memType.prgRom]\n(#memType) or memType.chrRom.\nChanges will remain in effect until a power cycle occurs.\nTo revert changes done to ROM, see revertPrgChrChanges.\nThe write / writeWord variants may cause side-effects that can alter the emulation's behavior.\nThe debugWrite/debugWriteWord variants have no side-effects."},
			new List<string> {"func","emu.debugWrite","emu.debugWrite(address, value, type)","address - *Integer* The address/offset to write to.\nvalue - *Integer* The value to write.\ntype - *Enum* The type of memory to write to. See memType.","","Writes an 8-bit or 16-bit value to the specified memory type.\nNormally read-only types such as PRG-ROM or CHR-ROM can be written to when using [memType.prgRom]\n(#memType) or memType.chrRom.\nChanges will remain in effect until a power cycle occurs.\nTo revert changes done to ROM, see revertPrgChrChanges.\nThe write / writeWord variants may cause side-effects that can alter the emulation's behavior.\nThe debugWrite/debugWriteWord variants have no side-effects."},
			new List<string> {"func","emu.debugWriteWord","emu.debugWriteWord(address, value, type)","address - *Integer* The address/offset to write to.\nvalue - *Integer* The value to write.\ntype - *Enum* The type of memory to write to. See memType.","","Writes an 8-bit or 16-bit value to the specified memory type.\nNormally read-only types such as PRG-ROM or CHR-ROM can be written to when using [memType.prgRom]\n(#memType) or memType.chrRom.\nChanges will remain in effect until a power cycle occurs.\nTo revert changes done to ROM, see revertPrgChrChanges.\nThe write / writeWord variants may cause side-effects that can alter the emulation's behavior.\nThe debugWrite/debugWriteWord variants have no side-effects."},
			new List<string> {"func","emu.revertPrgChrChanges","emu.revertPrgChrChanges()","","","Reverts all modifications done to PRG-ROM and CHR-ROM via write/writeWord calls."},
			new List<string> {"func","emu.drawPixel","emu.drawPixel(x, y, color, duration)","x - *Integer* X position\ny - *Integer* Y position\ncolor - *Integer* Color\nduration - *Integer* Number of frames to display(Default: 1 frame)","","Draws a pixel at the specified (x, y) coordinates using the specified color for a specific number of frames."},
			new List<string> {"func","emu.drawLine","emu.drawLine(x, y, x2, y2, color, duration)","x - *Integer* X position(start of line)\ny - *Integer* Y position(start of line)\nx2 - *Integer* X position(end of line)\ny2 - *Integer* Y position(end of line)\ncolor - *Integer* Color\nduration - *Integer* Number of frames to display(Default: 1 frame)","","Draws a line between(x, y) to(x2, y2) using the specified color for a specific number of frames."},
			new List<string> {"func","emu.drawRectangle","emu.drawRectangle(x, y, width, height, color, fill, duration)","x - *Integer* X position\ny - *Integer* Y position\nwidth - *Integer* The rectangle's width\nheight - *Integer* The rectangle's height\ncolor - *Integer* Color\nfill - * Boolean* Whether or not to draw an outline, or a filled rectangle.\nduration - *Integer* Number of frames to display(Default: 1 frame)","","Draws a rectangle between(x, y) to(x+width, y+height) using the specified color for a specific number of frames.\nIf *fill* is false, only the rectangle's outline will be drawn."},
			new List<string> {"func","emu.drawString","emu.drawString(x, y, text, textColor, backgroundColor, duration)","x - *Integer* X position\ny - *Integer* Y position\ntext- *String* The text to display\ntextColor - *Integer* Color to use for the text\nbackgroundColor - *Integer* Color to use for the text's background color\nduration - *Integer* Number of frames to display(Default: 1 frame)","","Draws text at(x, y) using the specified text and colors for a specific number of frames."},
			new List<string> {"func","emu.clearScreen","emu.clearScreen()","","","Removes all drawings from the screen."},
			new List<string> {"func","emu.getPixel","emu.getPixel(x, y)","x - *Integer* X position\ny - *Integer* Y position","*Integer* ARGB color","Returns the color(in ARGB format) of the PPU's output for the specified location."},
			new List<string> {"func","emu.displayMessage","emu.displayMessage(category, text)","category - *String* The category is the portion shown between brackets[]\ntext - *String* Text to show on the screen","","Displays a message on the main window in the format '[category] text'"},
			new List<string> {"func","emu.log","emu.log(text)","text - *String* Text to log","","Logs the given string in the script's log window - useful for debugging scripts."},
			new List<string> {"func","emu.getState","emu.getState()","","* Table* Current emulation state","Return a table containing information about the state of the CPU, PPU, APU and cartridge."},
			new List<string> {"func","emu.setState","emu.setState(state)","state - *Table* A table containing the state of the emulation to apply.","","Updates the CPU and PPU's state.\nThe* state* parameter must be a table in the same format as the one returned by getState()\nNote: the state of the APU or cartridge cannot be modified by using setState()." },
			new List<string> {"func","emu.breakExecution","emu.breakExecution()","","","Breaks the execution of the game and displays the debugger window."},
			new List<string> {"func","emu.execute","emu.execute(count, type)","count - *Integer* The number of cycles or instructions to run before breaking\ntype - *Enum* See executeCountType","","Runs the emulator for the specified number of cycles/instructions and then breaks the execution."},
			new List<string> {"func","emu.reset","emu.reset()","","","Resets the current game."},
			new List<string> {"func","emu.resume","emu.resume()","","","Resumes execution after breaking."},
			new List<string> {"func","emu.rewind","emu.rewind(seconds)","seconds - *Integer* The number of seconds to rewind","","Instantly rewinds the emulation by the number of seconds specified.\n Note: this can only be called from within a 'StartFrame' event callback."},
			new List<string> {"func","emu.saveSavestate","emu.saveSavestate()","","*String* A string containing a binary blob representing the emulation's current state.","Creates a savestate and returns it as a binary string. (The savestate is not saved on disk)\n Note: this can only be called from within a 'StartFrame' event callback."},
			new List<string> {"func","emu.loadSavestate","emu.loadSavestate(savestate)","savestate - *String* A binary blob representing a savestate, as returned by saveSavestate()","","Loads the specified savestate.\nNote: this can only be called from within a 'StartFrame' event callback."},
			new List<string> {"func","emu.getInput","emu.getInput(port)","port - *Integer* The port number to read (0 to 3)","*Table* A table containing the status of all 8 buttons.","Returns a table containing the status of all 8 buttons: { a, b, select, start, up, down, left, right }"},
			new List<string> {"func","emu.setInput","emu.setInput(port, input)","port - *Integer* The port number to apply the input to (0 to 3)\ninput - *Table* A table containing the state of all 8 buttons (as returned by getInput)","","Buttons enabled via setInput are permanently enabled until they are disabled by a subsequent call to setInput.\nThis means you will need to call setInput with an empty table (e.g: emu.setInput(0, { }) to restore the emulator's default behavior."},
			new List<string> {"func","emu.getMouseState","emu.getMouseState()","","*Table* The mouse's state","Returns a table containing the position and the state of all 3 buttons: { x, y, left, middle, right }"},
			new List<string> {"func","emu.addCheat","emu.addCheat(cheatCode)","cheatCode - *String* A game genie format cheat code.","","Activates a game genie cheat code (6 or 8 characters).\nNote: cheat codes added via this function are not permanent and not visible in the UI."},
			new List<string> {"func","emu.clearCheats","emu.clearCheats()","","","Removes all active cheat codes (has no impact on cheat codes saved within the UI)"},
			new List<string> {"func","emu.takeScreenshot","emu.takeScreenshot()","","*String* A binary string containing a PNG image.","Takes a screenshot and returns a PNG file as a string.\nThe screenshot is not saved to the disk."},
			new List<string> {"enum","emu.eventType","emu.eventType.[value]","","","Values:\npower = 0,\nreset = 1,\nnmi = 2,\nirq = 3,\nstartFrame = 4,\nendFrame = 5,\ncodeBreak = 6\n\nUsed by addEventCallback / removeEventCallback calls."},
			new List<string> {"enum","emu.executeCountType","emu.executeCountType.[value]","","","Values:\ncpuCycles = 0,\nppuCycles = 1,\ncpuInstructions = 2\n\nUsed by execute calls." },
			new List<string> {"enum","emu.memCallbackType","emu.memCallbackType.[value]","","","Values:\ncpuRead = 0,\ncpuWrite = 1,\ncpuExec = 2,\nppuRead = 3,\nppuWrite = 4\n\nUsed by addMemoryCallback / removeMemoryCallback calls."},
			new List<string> {"enum","emu.memType","emu.memType.[value]", "","","Values:\ncpu = 0,\nppu = 1,\npalette = 2,\noam = 3,\nsecondaryOam = 4,\nprgRom = 5,\nchrRom = 6,\nchrRam = 7,\nworkRam = 8,\nsaveRam = 9\n\nUsed by read / write calls."},
		};
	}

	public class MethodAutocompleteItem : AutocompleteItem
	{
		string firstPart;
		string lastPart;

		public MethodAutocompleteItem(string text) : base(text)
		{
			var i = text.LastIndexOf('.');
			if(i < 0) {
				firstPart = text;
			} else {
				firstPart = text.Substring(0, i);
				lastPart = text.Substring(i + 1);
			}
		}

		public override CompareResult Compare(string fragmentText)
		{
			int i = fragmentText.LastIndexOf('.');

			if(i < 0) {
				if(firstPart.StartsWith(fragmentText) && string.IsNullOrEmpty(lastPart)) {
					return CompareResult.VisibleAndSelected;
				}
				//if (firstPart.ToLower().Contains(fragmentText.ToLower()))
				//  return CompareResult.Visible;
			} else {
				var fragmentFirstPart = fragmentText.Substring(0, i);
				var fragmentLastPart = fragmentText.Substring(i + 1);

				if(firstPart != fragmentFirstPart) {
					return CompareResult.Hidden;
				}

				if(lastPart != null && lastPart.StartsWith(fragmentLastPart)) {
					return CompareResult.VisibleAndSelected;
				}

				if(lastPart != null && lastPart.ToLower().Contains(fragmentLastPart.ToLower())) {
					return CompareResult.Visible;
				}

			}

			return CompareResult.Hidden;
		}

		public override string GetTextForReplace()
		{
			if(lastPart == null) {
				return firstPart;
			}

			return firstPart + "." + lastPart;
		}

		public override string ToString()
		{
			if(lastPart == null) {
				return firstPart;
			}

			return lastPart;
		}
	}
}
