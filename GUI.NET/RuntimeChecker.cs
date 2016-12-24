using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI
{
	class RuntimeChecker
	{
		public static bool TestDll()
		{
			try {
				return InteropEmu.TestDll();
			} catch {
			}
			
			bool dllExists;
			if(Program.IsMono) {
				dllExists = File.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "libMesenCore.dll"));
			} else {
				dllExists = File.Exists("MesenCore.dll");
			}

			if(!dllExists) {
				MesenMsgBox.Show("UnableToStartMissingFiles", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} else {
				MesenMsgBox.Show("UnableToStartMissingDependencies", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return false;
		}
	}
}
