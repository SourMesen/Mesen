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
				try {
					ResourceHelper.LoadResources(ConfigManager.Config.PreferenceInfo.DisplayLanguage);
					ResourceManager.ExtractResources();
				} catch(FileNotFoundException) {
					string message = "The Microsoft .NET Framework 4.5 could not be found. Please download and install the latest version of the .NET Framework from Microsoft's website and try again.";
					switch(ResourceHelper.GetCurrentLanguage()) {
						case Language.French: message = "Le .NET Framework 4.5 de Microsoft n'a pas été trouvé. Veuillez télécharger la plus récente version du .NET Framework à partir du site de Microsoft et essayer à nouveau."; break;
						case Language.Japanese: message = "Microsoft .NET Framework 4.5はインストールされていないため、Mesenは起動できません。Microsoft .NET Frameworkの最新版をMicrosoftのサイトからダウンロードして、インストールしてください。"; break;
						case Language.Russian: message = "Microsoft .NET Framework 4.5 не найден. Пожалуйста загрузите и установите последнюю версию .NET Framework с сайта Microsoft и попробуйте снова."; break;
						case Language.Spanish: message = "El Microsoft .NET Framework 4.5 no se ha encontrado. Por favor, descargue la versión más reciente de .NET Framework desde el sitio de Microsoft y vuelva a intentarlo."; break;
					}
					MessageBox.Show(message, "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				} catch(Exception e) {
					string message = "An unexpected error has occurred.\n\nError details:\n{0}";
					switch(ResourceHelper.GetCurrentLanguage()) {
						case Language.French: message = "Une erreur inattendue s'est produite.\n\nDétails de l'erreur :\n{0}"; break;
						case Language.Japanese: message = "予期しないエラーが発生しました。\n\nエラーの詳細:\n{0}"; break;
						case Language.Russian: message = "Неизвестная ошибка.&#xA;&#xA;Подробно:&#xA;{0}"; break;
						case Language.Spanish: message = "Se ha producido un error inesperado.&#xA;&#xA;Detalles del error:&#xA;{0}"; break;
					}
					MessageBox.Show(string.Format(message, e.ToString()), "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
									frmMain.ProcessCommandLineArguments(e.Args, false);
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
