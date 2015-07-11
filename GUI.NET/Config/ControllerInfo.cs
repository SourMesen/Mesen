using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public enum ControllerType
	{
		None = 0,
		StandardController = 1,
	}

	public class KeyMappings
	{
		public string A = "A";
		public string B = "B";
		public string Select = "W";
		public string Start = "Q";
		public string Up = "Up Arrow";
		public string Down = "Down Arrow";
		public string Left = "Left Arrow";
		public string Right = "Right Arrow";

		public string TurboA = "Z";
		public string TurboB = "X";
		public string TurboStart = "";
		public string TurboSelect = "";
		public UInt32 TurboSpeed;

		public InteropEmu.KeyMapping ToInteropMapping()
		{
			InteropEmu.KeyMapping mapping = new InteropEmu.KeyMapping();

			mapping.A = InteropEmu.GetKeyCode(A);
			mapping.B = InteropEmu.GetKeyCode(B);
			mapping.Start = InteropEmu.GetKeyCode(Start);
			mapping.Select = InteropEmu.GetKeyCode(Select);
			mapping.Up = InteropEmu.GetKeyCode(Up);
			mapping.Down = InteropEmu.GetKeyCode(Down);
			mapping.Left = InteropEmu.GetKeyCode(Left);
			mapping.Right = InteropEmu.GetKeyCode(Right);
			mapping.TurboA = InteropEmu.GetKeyCode(TurboA);
			mapping.TurboB = InteropEmu.GetKeyCode(TurboB);
			mapping.TurboStart = InteropEmu.GetKeyCode(TurboStart);
			mapping.TurboSelect = InteropEmu.GetKeyCode(TurboSelect);
			mapping.TurboSpeed = TurboSpeed;

			return mapping;
		}
	}

	public class ControllerInfo
	{
		public ControllerType ControllerType = ControllerType.StandardController;
		public KeyMappings Keys = new KeyMappings();

		public static void ApplyConfig()
		{
			for(int i = 0; i < 4; i++) {
				InteropEmu.ClearKeyMappings(i);

				if(ConfigManager.Config.Controllers[i].ControllerType != ControllerType.None) {
					InteropEmu.AddKeyMappings(i, ConfigManager.Config.Controllers[i].Keys.ToInteropMapping());
				}
			}
		}
	}
}
