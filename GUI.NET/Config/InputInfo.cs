using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public class KeyMappings
	{
		public string A;
		public string B;
		public string Select;
		public string Start;
		public string Up;
		public string Down;
		public string Left;
		public string Right;

		public string TurboA;
		public string TurboB;
		public string TurboStart;
		public string TurboSelect;

		public KeyMappings()
		{
		}

		public KeyMappings(int controllerIndex, int keySetIndex)
		{
			if(controllerIndex == 0) {
				if(keySetIndex == 0) {
					A = "A";
					B = "S";
					Select = "W";
					Start = "Q";
					Up = "Up Arrow";
					Down = "Down Arrow";
					Left = "Left Arrow";
					Right = "Right Arrow";

					TurboA = "Z";
					TurboB = "X";
				} else if(keySetIndex == 1) {
					A = "Pad1 A";
					B = "Pad1 X";
					Select = "Pad1 Back";
					Start = "Pad1 Start";
					Up = "Pad1 Up";
					Down = "Pad1 Down";
					Left = "Pad1 Left";
					Right = "Pad1 Right";

					TurboA = "Pad1 B";
					TurboB = "Pad1 Y";
				}
			}
		}

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

			return mapping;
		}
	}

	public class ControllerInfo
	{
		public InteropEmu.ControllerType ControllerType = InteropEmu.ControllerType.StandardController;
		public List<KeyMappings> Keys = new List<KeyMappings>();
		public UInt32 TurboSpeed = 2;

		public InteropEmu.KeyMappingSet GetKeyMappingSet()
		{
			while(Keys.Count < 4) {
				Keys.Add(new KeyMappings());
			}

			InteropEmu.KeyMappingSet mappingSet = new InteropEmu.KeyMappingSet();
			mappingSet.Mapping1 = Keys[0].ToInteropMapping();
			mappingSet.Mapping2 = Keys[1].ToInteropMapping();
			mappingSet.Mapping3 = Keys[2].ToInteropMapping();
			mappingSet.Mapping4 = Keys[3].ToInteropMapping();
			mappingSet.TurboSpeed = TurboSpeed;
			return mappingSet;
		}
	}

	public class InputInfo
	{
		public ConsoleType ConsoleType = ConsoleType.Nes;
		public InteropEmu.ExpansionPortDevice ExpansionPortDevice = InteropEmu.ExpansionPortDevice.None;
		public bool UseFourScore = false;

		[NonSerialized]
		public InteropEmu.ControllerType ControllerType1;
		public InteropEmu.ControllerType ControllerType2;
		public InteropEmu.ControllerType ControllerType3;
		public InteropEmu.ControllerType ControllerType4;

		public List<ControllerInfo> Controllers = new List<ControllerInfo>();

		public void InitializeDefaults()
		{
			while(Controllers.Count < 4) {
				var controllerInfo = new ControllerInfo();
				controllerInfo.ControllerType = Controllers.Count == 0 ? InteropEmu.ControllerType.StandardController : InteropEmu.ControllerType.None;

				if(Controllers.Count == 0) {
					controllerInfo.Keys.Add(new KeyMappings(0, 0));
					controllerInfo.Keys.Add(new KeyMappings(0, 1));
				}
				Controllers.Add(controllerInfo);
			}
		}

		public static void ApplyConfig()
		{
			InputInfo inputInfo = ConfigManager.Config.InputInfo;

			InteropEmu.ExpansionPortDevice expansionDevice;
			if(inputInfo.ConsoleType == ConsoleType.Nes) {
				expansionDevice = InteropEmu.ExpansionPortDevice.None;
			} else {
				expansionDevice = inputInfo.ExpansionPortDevice;
			}

			InteropEmu.SetConsoleType(inputInfo.ConsoleType);
			InteropEmu.SetExpansionDevice(inputInfo.ExpansionPortDevice);
			bool hasFourScore = (inputInfo.ConsoleType == ConsoleType.Nes && inputInfo.UseFourScore) || (inputInfo.ConsoleType == ConsoleType.Famicom && expansionDevice == InteropEmu.ExpansionPortDevice.FourPlayerAdapter);
			InteropEmu.SetFlag(EmulationFlags.HasFourScore, hasFourScore);
			for(int i = 0; i < 4; i++) {
				InteropEmu.SetControllerType(i, i < 2 || hasFourScore ? inputInfo.Controllers[i].ControllerType : InteropEmu.ControllerType.None);
				InteropEmu.SetControllerKeys(i, inputInfo.Controllers[i].GetKeyMappingSet());
			}
		}
	}
}
