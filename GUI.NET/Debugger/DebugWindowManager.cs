using Mesen.GUI.Forms;
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

		public static bool ScriptWindowOpened { get { return _openedWindows.Any(wnd => wnd is frmScript); } }

		public static void OpenDebugWindow(DebugWindow window)
		{
			Form existingWindow = GetExistingSingleInstanceWindow(window);
			if(existingWindow != null) {
				existingWindow.BringToFront();
				if(existingWindow.WindowState == FormWindowState.Minimized) {
					//Unminimize window if it was minimized
					existingWindow.WindowState = FormWindowState.Normal;
				}
				existingWindow.Focus();
			} else {
				BaseForm frm = null;
				switch(window) {
					case DebugWindow.PpuViewer: frm = new frmPpuViewer(); frm.Icon = Properties.Resources.Video; break;
					case DebugWindow.TraceLogger: frm = new frmTraceLogger(); frm.Icon = Properties.Resources.LogWindow; break;
					case DebugWindow.MemoryViewer: frm = new frmMemoryViewer(); frm.Icon = Properties.Resources.CheatCode; break;
					case DebugWindow.Assembler: frm = new frmAssembler(); frm.Icon = Properties.Resources.Chip; break;
					case DebugWindow.Debugger: frm = new frmDebugger(); frm.Icon = Properties.Resources.Bug; break;
					case DebugWindow.ScriptWindow: frm = new frmScript(); frm.Icon = Properties.Resources.Script; break;
					case DebugWindow.ApuViewer: frm = new frmApuViewer(); frm.Icon = Properties.Resources.Audio; break;
					case DebugWindow.EventViewer: frm = new frmEventViewer(); frm.Icon = Properties.Resources.NesEventViewer; break;
					case DebugWindow.TextHooker: frm = new frmTextHooker(); frm.Icon = Properties.Resources.Font; break;
				}
				_openedWindows.Add(frm);
				frm.FormClosed += Debugger_FormClosed;
				frm.Show();
			}
		}

		public static void OpenAssembler(string code = "", UInt16 startAddress = 0, UInt16 blockLength = 0)
		{
			frmAssembler frm = new frmAssembler(code, startAddress, blockLength);
			frm.Icon = Properties.Resources.Chip;
			_openedWindows.Add(frm);
			frm.FormClosed += Debugger_FormClosed;
			frm.Show();
		}

		private static frmMemoryViewer OpenMemoryViewer()
		{
			frmMemoryViewer frm = GetMemoryViewer();
			if(frm == null) {
				frm = new frmMemoryViewer();
				frm.Icon = Properties.Resources.CheatCode;
				frm.FormClosed += Debugger_FormClosed;
				_openedWindows.Add(frm);
			} else {
				if(frm.WindowState == FormWindowState.Minimized) {
					//Unminimize window if it was minimized
					frm.WindowState = FormWindowState.Normal;
				}
				frm.BringToFront();
			}
			frm.Show();
			return frm;
		}

		public static void OpenMemoryViewer(int address, DebugMemoryType memoryType)
		{
			frmMemoryViewer frm = OpenMemoryViewer();
			frm.ShowAddress(address, memoryType);
		}

		public static void OpenMemoryViewer(GoToDestination dest)
		{
			frmMemoryViewer frm = OpenMemoryViewer();
			frm.GoToDestination(dest);
		}

		public static frmScript OpenScriptWindow(bool forceBlank)
		{
			frmScript frm = new frmScript(forceBlank);
			frm.Icon = Properties.Resources.Script;
			_openedWindows.Add(frm);
			frm.FormClosed += Debugger_FormClosed;
			frm.Show();
			return frm;
		}

		public static frmDebugger GetDebugger()
		{
			return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger)) as frmDebugger;
		}

		public static frmMemoryViewer GetMemoryViewer()
		{
			return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmMemoryViewer)) as frmMemoryViewer;
		}

		public static bool HasOpenedWindow
		{
			get
			{
				return _openedWindows.Count > 0;
			}
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
				case DebugWindow.TextHooker: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmTextHooker));
			}

			return null;
		}

		public static void CleanupDebugger()
		{
			if(_openedWindows.Count == 0) {
				//All windows have been closed, disable debugger
				DebugWorkspaceManager.Clear();

				Task.Run(() => {
					//Run this in another thread to avoid deadlocks when this is called within a notification handler
					InteropEmu.DebugRelease();
				});
			}
		}

		private static void Debugger_FormClosed(object sender, FormClosedEventArgs e)
		{
			_openedWindows.Remove((Form)sender);
			CleanupDebugger();
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
		EventViewer,
		TextHooker,
	}
}
