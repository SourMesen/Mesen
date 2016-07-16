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
					Select = "Q";
					Start = "W";
					Up = "Up Arrow";
					Down = "Down Arrow";
					Left = "Left Arrow";
					Right = "Right Arrow";

					TurboA = "Z";
					TurboB = "X";
				} else if(keySetIndex == 1) {
					//XInput Default (Xbox controllers)
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
				} else if(keySetIndex == 2) {
					//DirectInput Default (Used PS4 controller as a default)
					A = "Joy1 But2";
					B = "Joy1 But1";
					Select = "Joy1 But9";
					Start = "Joy1 But10";
					Up = "Joy1 DPad Up";
					Down = "Joy1 DPad Down";
					Left = "Joy1 DPad Left";
					Right = "Joy1 DPad Right";

					TurboA = "Joy1 But3";
					TurboB = "Joy1 But4";
				}
			} else if(controllerIndex == 1) {
				if(keySetIndex == 0) {
					A = "G";
					B = "H";
					Select = "T";
					Start = "Y";
					Up = "I";
					Down = "K";
					Left = "J";
					Right = "L";

					TurboA = "B";
					TurboB = "N";
				} else if(keySetIndex == 1) {
					//XInput Default (Xbox controllers)
					A = "Pad2 A";
					B = "Pad2 X";
					Select = "Pad2 Back";
					Start = "Pad2 Start";
					Up = "Pad2 Up";
					Down = "Pad2 Down";
					Left = "Pad2 Left";
					Right = "Pad2 Right";

					TurboA = "Pad2 B";
					TurboB = "Pad2 Y";
				} else if(keySetIndex == 2) {
					//DirectInput Default (Used PS4 controller as a default)
					A = "Joy2 But2";
					B = "Joy2 But1";
					Select = "Joy2 But9";
					Start = "Joy2 But10";
					Up = "Joy2 DPad Up";
					Down = "Joy2 DPad Down";
					Left = "Joy2 DPad Left";
					Right = "Joy2 DPad Right";

					TurboA = "Joy2 But3";
					TurboB = "Joy2 But4";
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
		public bool AutoConfigureInput = true;

		public List<ControllerInfo> Controllers = new List<ControllerInfo>();

		public void InitializeDefaults()
		{
			while(Controllers.Count < 4) {
				var controllerInfo = new ControllerInfo();
				controllerInfo.ControllerType = Controllers.Count <= 1 ? InteropEmu.ControllerType.StandardController : InteropEmu.ControllerType.None;

				if(Controllers.Count <= 1) {
					controllerInfo.Keys.Add(new KeyMappings(Controllers.Count, 0));
					controllerInfo.Keys.Add(new KeyMappings(Controllers.Count, 1));
					controllerInfo.Keys.Add(new KeyMappings(Controllers.Count, 2));
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

			InteropEmu.SetFlag(EmulationFlags.AutoConfigureInput, inputInfo.AutoConfigureInput);
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
