using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Mesen.GUI.Config
{
	public class PreferenceInfo
	{
		public bool SingleInstance = true;
		public bool AutoLoadIpsPatches = true;
		public bool AssociateNesFiles = false;
		public bool AllowInvalidInput = false;
		public bool RemoveSpriteLimit = false;

		public bool UseAlternativeMmc3Irq = false;

		public PreferenceInfo()
		{
		}

		static public void ApplyConfig()
		{
			PreferenceInfo preferenceInfo = ConfigManager.Config.PreferenceInfo;

			if(preferenceInfo.AssociateNesFiles) {
				Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\Mesen\shell\open\command", null, Application.ExecutablePath + " \"%1\"");
				Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\.nes", null, "Mesen");
			} else {
				//Unregister Mesen if Mesen was registered for .nes files
				if(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Classes\.nes", null, "").Equals("Mesen")) {
					Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\.nes", null, "");
				}
			}

			InteropEmu.SetFlag(EmulationFlags.Mmc3IrqAltBehavior, preferenceInfo.UseAlternativeMmc3Irq);
			InteropEmu.SetFlag(EmulationFlags.AllowInvalidInput, preferenceInfo.AllowInvalidInput);
			InteropEmu.SetFlag(EmulationFlags.RemoveSpriteLimit, preferenceInfo.RemoveSpriteLimit);
		}
	}
}
