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
using System.Xml.Serialization;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI
{
	static class Program
	{
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		public static bool IsMono { get; private set; }
		public static string OriginalFolder { get; private set; }

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

			if(ResourceManager.GoogleDlls.Contains(assemblyFile)) {
				ResourceManager.ExtractGoogleDriveResources();
			}

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
				Task.Run(() => {
					//Cache deserializers in another thread
					new XmlSerializer(typeof(Configuration));
					new XmlSerializer(typeof(DebugWorkspace));
				});

				if(Type.GetType("Mono.Runtime") != null) {
					Program.IsMono = true;
				}

				Program.OriginalFolder = Directory.GetCurrentDirectory();
        				
				Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
				Application.ThreadException += Application_ThreadException;
				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				if(ConfigManager.GetConfigFile() == null) {
					//Show config wizard
					ResourceHelper.LoadResources(Language.SystemDefault);
					Application.Run(new frmConfigWizard());

					if(ConfigManager.GetConfigFile() == null) {
						Application.Exit();
						return;
					}
				}

				AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblies;

				Directory.CreateDirectory(ConfigManager.HomeFolder);
				Directory.SetCurrentDirectory(ConfigManager.HomeFolder);
				try {
					if(!ResourceManager.ExtractResources()) { 
						return;
					}
				} catch(FileNotFoundException e) {
					string message = "The Microsoft .NET Framework 4.5 could not be found. Please download and install the latest version of the .NET Framework from Microsoft's website and try again.";
					switch(ResourceHelper.GetCurrentLanguage()) {
						case Language.French: message = "Le .NET Framework 4.5 de Microsoft n'a pas été trouvé. Veuillez télécharger la plus récente version du .NET Framework à partir du site de Microsoft et essayer à nouveau."; break;
						case Language.Japanese: message = "Microsoft .NET Framework 4.5はインストールされていないため、Mesenは起動できません。Microsoft .NET Frameworkの最新版をMicrosoftのサイトからダウンロードして、インストールしてください。"; break;
						case Language.Russian: message = "Microsoft .NET Framework 4.5 не найден. Пожалуйста загрузите и установите последнюю версию .NET Framework с сайта Microsoft и попробуйте снова."; break;
						case Language.Spanish: message = "Microsoft .NET Framework 4.5 no se ha encontrado. Por favor, descargue la versión más reciente de .NET Framework desde el sitio de Microsoft y vuelva a intentarlo."; break;
						case Language.Ukrainian: message = "Microsoft .NET Framework 4.5 не знайдений. Будь ласка завантажте і встановіть останню версію .NET Framework з сайту Microsoft і спробуйте знову."; break;
						case Language.Portuguese: message = "Microsoft .NET Framework 4.5 não foi encontrado. Por favor, baixe a versão mais recente de .NET Framework do site da Microsoft e tente novamente."; break;
					}
					MessageBox.Show(message + Environment.NewLine + Environment.NewLine + e.ToString(), "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				} catch(Exception e) {
					string message = "An unexpected error has occurred.\n\nError details:\n{0}";
					switch(ResourceHelper.GetCurrentLanguage()) {
						case Language.French: message = "Une erreur inattendue s'est produite.\n\nDétails de l'erreur :\n{0}"; break;
						case Language.Japanese: message = "予期しないエラーが発生しました。\n\nエラーの詳細:\n{0}"; break;
						case Language.Russian: message = "Неизвестная ошибка.&#xA;&#xA;Подробно:&#xA;{0}"; break;
						case Language.Spanish: message = "Se ha producido un error inesperado.&#xA;&#xA;Detalles del error:&#xA;{0}"; break;
						case Language.Ukrainian: message = "Невідома помилка.&#xA;&#xA;Детально:&#xA;{0}"; break;
						case Language.Portuguese: message = "Houve um erro inesperado.&#xA;&#xA;Detalhes do erro:&#xA;{0}"; break;
					}
					MessageBox.Show(string.Format(message, e.ToString()), "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				if(!RuntimeChecker.TestDll()) {
					return;
				}

				if(CommandLineHelper.PreprocessCommandLineArguments(args, true).Contains("/testrunner")) {
					Environment.ExitCode = TestRunner.Run(args);
					return;
				}

				using(SingleInstance singleInstance = new SingleInstance()) {
					if(singleInstance.FirstInstance || !ConfigManager.Config.PreferenceInfo.SingleInstance) {
						frmMain frmMain = new frmMain(args);

						singleInstance.ListenForArgumentsFromSuccessiveInstances();
						singleInstance.ArgumentsReceived += (object sender, ArgumentsReceivedEventArgs e) => {
							if(frmMain.IsHandleCreated) {
								frmMain.BeginInvoke((MethodInvoker)(() => {
									frmMain.ProcessCommandLineArguments(CommandLineHelper.PreprocessCommandLineArguments(e.Args, true), false);
									frmMain.LoadGameFromCommandLine(CommandLineHelper.PreprocessCommandLineArguments(e.Args, false));
								}));
							}
						};

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
							Application.Run(new frmMain(args));
						}
					}
				}
			} catch(Exception e) {
				MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, e.ToString());
			}
		}
	}
}
