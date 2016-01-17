using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI
{
	static class Program
	{
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if(!RuntimeChecker.TestDll()) {
				return;
			}

			Guid guid = new Guid("{A46606B7-2D1C-4CC5-A52F-43BCAF094AED}");
			using(SingleInstance singleInstance = new SingleInstance(guid)) {
				if(singleInstance.FirstInstance || !Config.ConfigManager.Config.PreferenceInfo.SingleInstance) {
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);

					Mesen.GUI.Forms.frmMain frmMain = new Mesen.GUI.Forms.frmMain(args);

					if(Config.ConfigManager.Config.PreferenceInfo.SingleInstance) {
						singleInstance.ListenForArgumentsFromSuccessiveInstances();
						singleInstance.ArgumentsReceived += (object sender, ArgumentsReceivedEventArgs e) => {
							frmMain.BeginInvoke((MethodInvoker)(() => {
								frmMain.ProcessCommandLineArguments(e.Args);
							}));
						};
					}

					Application.Run(frmMain);
				} else {
					if(singleInstance.PassArgumentsToFirstInstance(args)) {
						Process current = Process.GetCurrentProcess();
						foreach(Process process in Process.GetProcessesByName(current.ProcessName)) {
							if(process.Id != current.Id) {
								Program.SetForegroundWindow(process.MainWindowHandle);
								break;
							}
						}
					} else {
						Application.EnableVisualStyles();
						Application.SetCompatibleTextRenderingDefault(false);
						Application.Run(new Mesen.GUI.Forms.frmMain(args));
					}
				}
			}
		}
	}
}
