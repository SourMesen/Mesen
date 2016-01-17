using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI
{
	static class Program
	{
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.ExceptionObject.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		[HandleProcessCorruptedStateExceptions]
		private static void Main(string[] args)
		{
			try {
				Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
				Application.ThreadException += Application_ThreadException;
				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

				Directory.CreateDirectory(ConfigManager.HomeFolder);
				Directory.SetCurrentDirectory(ConfigManager.HomeFolder);
				ResourceManager.ExtractResources();

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
			} catch(Exception e) {
				MessageBox.Show(e.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
