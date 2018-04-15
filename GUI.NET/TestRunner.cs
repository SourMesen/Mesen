using Mesen.GUI.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mesen.GUI
{
	internal class TestRunner
	{
		internal static int Run(string[] args)
		{
			ConfigManager.DoNotSaveSettings = true;
			string romPath;
			List<string> luaScriptsToLoad;
			CommandLineHelper.GetRomPathFromCommandLine(CommandLineHelper.PreprocessCommandLineArguments(args, false), out romPath, out luaScriptsToLoad);
			if(romPath == null) {
				//No rom specified
				return -1;
			}

			List<string> lcArgs = CommandLineHelper.PreprocessCommandLineArguments(args, true);

			int timeout = 100; //100 seconds
			string timeoutArg = lcArgs.Find(arg => arg.StartsWith("/timeout="));
			if(timeoutArg != null) {
				int timeoutValue;
				if(Int32.TryParse(timeoutArg.Substring(timeoutArg.IndexOf("=") + 1), out timeoutValue)) {
					timeout = timeoutValue;
				}
			}

			ConfigManager.ProcessSwitches(lcArgs);
			ConfigManager.Config.ApplyConfig();
			InteropEmu.SetFlag(EmulationFlags.ConsoleMode, true);

			InteropEmu.InitializeEmu(ConfigManager.HomeFolder, IntPtr.Zero, IntPtr.Zero, true, true, true);

			InteropEmu.LoadROM(romPath, string.Empty);

			foreach(string luaScript in luaScriptsToLoad) {
				try {
					string script = File.ReadAllText(luaScript);
					InteropEmu.DebugLoadScript(luaScript, script);
				} catch { }
			}

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			Task.Run(() => {
				InteropEmu.Run();
			});

			InteropEmu.SetFlag(EmulationFlags.ForceMaxSpeed, true);

			sw.Start();
			int result = -1;
			while(sw.ElapsedMilliseconds < timeout * 1000) {
				System.Threading.Thread.Sleep(100);

				if(!InteropEmu.IsRunning()) {
					result = InteropEmu.GetStopCode();
					break;
				}
			}

			InteropEmu.Stop();
			InteropEmu.Release();
			return result;
		}
	}
}