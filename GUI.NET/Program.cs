using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI
{
	static class Program
	{
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, e.Exception.ToString());
		}
		
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, e.ExceptionObject.ToString());
		}

		[DebuggerNonUserCode]
		private static Assembly LoadAssemblies(object sender, ResolveEventArgs e)
		{
			//Allow assemblies to be loaded from subfolders in the home folder (used for Google Drive API dlls)
			string assemblyFile = e.Name.Contains(',') ? e.Name.Substring(0, e.Name.IndexOf(',')) : e.Name;
			assemblyFile += ".dll";

			string absoluteFolder = new FileInfo((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath).Directory.FullName;
			string targetPath = Path.Combine(ConfigManager.HomeFolder, "GoogleDrive", assemblyFile);

			try {
				if(File.Exists(targetPath)) {
					return Assembly.LoadFile(targetPath);
				}
			} catch(Exception) {
				return null;
			}
			return null;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		[HandleProcessCorruptedStateExceptions]
		private static void Main(string[] args)
		{
			try {
				AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblies;
				Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
				Application.ThreadException += Application_ThreadException;
				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

				Directory.CreateDirectory(ConfigManager.HomeFolder);
				Directory.SetCurrentDirectory(ConfigManager.HomeFolder);
				ResourceHelper.LoadResources(ConfigManager.Config.PreferenceInfo.DisplayLanguage);
				try {
					ResourceManager.ExtractResources();
				} catch {
					MesenMsgBox.Show("Net45NotFound", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if(!RuntimeChecker.TestDll()) {
					return;
				}

				ResourceHelper.UpdateEmuLanguage();

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
				MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, e.ToString());
			}
		}
	}
}
