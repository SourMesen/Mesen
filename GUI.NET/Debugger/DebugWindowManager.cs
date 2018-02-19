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
		private static HashSet<Form> _openedWindows = new HashSet<Form>();

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
					case DebugWindow.ApuViewer: frm = new frmApuViewer(); break;
					case DebugWindow.EventViewer: frm = new frmEventViewer(); break;
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

		public static void OpenMemoryViewer(int address)
		{
			frmMemoryViewer frm = GetMemoryViewer();
			if(frm == null) {
				frm = new frmMemoryViewer();
				frm.FormClosed += Debugger_FormClosed;
				_openedWindows.Add(frm);
			}
			frm.Show();
			frm.ShowAddress(address);
		}

		public static void OpenScriptWindow(bool forceBlank)
		{
			frmScript frm = new frmScript(forceBlank);
			_openedWindows.Add(frm);
			frm.FormClosed += Debugger_FormClosed;
			frm.Show();
		}

		public static frmDebugger GetDebugger()
		{
			return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger)) as frmDebugger;
		}

		public static frmMemoryViewer GetMemoryViewer()
		{
			return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmMemoryViewer)) as frmMemoryViewer;
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
			switch(window) {
				case DebugWindow.TraceLogger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmTraceLogger));
				case DebugWindow.Assembler: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmAssembler));
				case DebugWindow.Debugger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger));
				case DebugWindow.ApuViewer: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmApuViewer));
				case DebugWindow.EventViewer: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmEventViewer));
			}

			return null;
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
		ApuViewer,
		EventViewer
	}
}
