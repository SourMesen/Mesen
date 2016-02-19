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
				MesenMsgBox.Show("UnableToStartMissingFiles", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} else {
				if(MesenMsgBox.Show("UnableToStartMissingDependencies", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
					if(!RuntimeChecker.DownloadRuntime()) {
						MesenMsgBox.Show("CouldNotInstallRuntime", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				link = "http://download.microsoft.com/download/C/E/5/CE514EAE-78A8-4381-86E8-29108D78DBD4/VC_redist.x86.exe";
			} else {
				//x64
				link = "http://download.microsoft.com/download/C/E/5/CE514EAE-78A8-4381-86E8-29108D78DBD4/VC_redist.x64.exe";
			}

			string tempFilename = Path.GetTempPath() + "Mesen_VsRuntime2015.exe";

			try {
				frmDownloadProgress frm = new frmDownloadProgress(link, tempFilename);

				if(frm.ShowDialog() == DialogResult.OK) {
					Process installer = Process.Start(tempFilename, "/passive /norestart");
					installer.WaitForExit();
					if(installer.ExitCode != 0) {
						MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, installer.ExitCode.ToString());
						return false;
					} else {
						//Runtime should now be installed, try to launch Mesen again
						return true;
					}
				}
			} catch(Exception e) {
				MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, e.ToString());
			} finally {
				try {
					File.Delete(tempFilename);
				} catch { }
			}

			return false;
		}
	}
}
