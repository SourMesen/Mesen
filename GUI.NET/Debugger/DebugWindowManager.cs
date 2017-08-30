using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public class DebugWindowManager
	{
		private static List<Form> _openedWindows = new List<Form>();

		public static void OpenDebugWindow(DebugWindow window)
		{
			Form existingWindow = GetExistingSingleInstanceWindow(window);
			if(existingWindow != null) {
				existingWindow.BringToFront();
				existingWindow.Focus();
			} else {
				Form frm = null;
				switch(window) {
					case DebugWindow.PpuViewer: frm = new frmPpuViewer(); break;
					case DebugWindow.TraceLogger: frm = new frmTraceLogger(); break;
					case DebugWindow.MemoryViewer: frm = new frmMemoryViewer(); break;
					case DebugWindow.Assembler: frm = new frmAssembler(); break;
					case DebugWindow.Debugger: frm = new frmDebugger(); break;
					case DebugWindow.ScriptWindow: frm = new frmScript(); break;
				}
				_openedWindows.Add(frm);
				frm.FormClosed += Debugger_FormClosed;
				frm.Show();
			}
		}

		public static void OpenAssembler(string code = "", UInt16 startAddress = 0, UInt16 blockLength = 0)
		{
			frmAssembler frm = new frmAssembler(code, startAddress, blockLength);
			_openedWindows.Add(frm);
			frm.FormClosed += Debugger_FormClosed;
			frm.Show();
		}

		public static frmDebugger GetDebugger()
		{
			int index = _openedWindows.FindIndex((form) => form.GetType() == typeof(frmDebugger));
			if(index >= 0) {
				return (frmDebugger)_openedWindows[index];
			}
			return null;
		}

		public static void CloseAll()
		{
			List<Form> openedWindows = new List<Form>(_openedWindows);
			foreach(Form frm in openedWindows) {
				frm.Close();
			}
		}

		private static Form GetExistingSingleInstanceWindow(DebugWindow window)
		{
			//Only one of each of these windows can be opened at once, check if one is already opened
			int index = -1;
			switch(window) {
				case DebugWindow.TraceLogger:
					index = _openedWindows.FindIndex((form) => form.GetType() == typeof(frmTraceLogger));
					break;
				case DebugWindow.Assembler:
					index = _openedWindows.FindIndex((form) => form.GetType() == typeof(frmAssembler));
					break;
				case DebugWindow.Debugger:
					index = _openedWindows.FindIndex((form) => form.GetType() == typeof(frmDebugger));
					break;
			}

			if(index >= 0) {
				return _openedWindows[index];
			} else {
				return null;
			}
		}

		private static void Debugger_FormClosed(object sender, FormClosedEventArgs e)
		{
			_openedWindows.Remove((Form)sender);
			if(_openedWindows.Count == 0) {
				//All windows have been closed, disable debugger
				DebugWorkspaceManager.Clear();
				InteropEmu.DebugRelease();
			}
		}
	}

	public enum DebugWindow
	{
		PpuViewer,
		MemoryViewer,
		TraceLogger,
		Assembler,
		Debugger,
		ScriptWindow,
	}
}
