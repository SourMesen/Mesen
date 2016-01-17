using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI
{
	class RuntimeChecker
	{
		public static bool TestDll()
		{
			try {
				return InteropEmu.TestDll();
			} catch { }

			if(!File.Exists("WinMesen.dll")) {
				MessageBox.Show("Mesen was unable to start due to missing files." + Environment.NewLine + Environment.NewLine + "Error: WinMesen.dll is missing.", "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} else {
				if(MessageBox.Show("Mesen was unable to start due to missing dependencies."  + Environment.NewLine + Environment.NewLine + "Mesen requires the Visual Studio 2013 runtime.  Would you like to download the runtime from Microsoft's website and install it?", "Mesen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
					if(!RuntimeChecker.DownloadRuntime()) {
						MessageBox.Show("The Visual Studio Runtime could not be installed properly.", "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Error);
					} else {
						Process.Start(Process.GetCurrentProcess().MainModule.FileName);
					}
				}
			}
			return false;
		}

		private static bool DownloadRuntime()
		{
			string link = string.Empty;
			if(IntPtr.Size == 4) {
				//x86
				link = "https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x86.exe";
			} else {
				//x64
				link = "https://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x64.exe";
			}

			string tempFilename = Path.GetTempPath() + "Mesen_VsRuntime2013.exe";

			try {
				frmDownloadProgress frm = new frmDownloadProgress(link, tempFilename);

				if(frm.ShowDialog() == DialogResult.OK) {
					Process installer = Process.Start(tempFilename, "/passive /norestart");
					installer.WaitForExit();
					if(installer.ExitCode != 0) {
						MessageBox.Show("Unexpected error: " + installer.ExitCode, "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					} else {
						//Runtime should now be installed, try to launch Mesen again
						return true;
					}
				}
			} catch(Exception e) {
				MessageBox.Show("Unexpected error: " + Environment.NewLine + e.Message, "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				try {
					File.Delete(tempFilename);
				} catch { }
			}

			return false;
		}
	}
}
