using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mesen.GUI.Config
{
	public class KeyMappings
	{
		public UInt32 A;
		public UInt32 B;
		public UInt32 Select;
		public UInt32 Start;
		public UInt32 Up;
		public UInt32 Down;
		public UInt32 Left;
		public UInt32 Right;

		public UInt32 TurboA;
		public UInt32 TurboB;
		public UInt32 TurboStart;
		public UInt32 TurboSelect;

		public KeyMappings()
		{
		}

		public KeyMappings(int controllerIndex, int keySetIndex)
		{
			if(controllerIndex == 0) {
				if(keySetIndex == 0) {
					A = InteropEmu.GetKeyCode("A");
					B = InteropEmu.GetKeyCode("S");
					Select = InteropEmu.GetKeyCode("Q");
					Start = InteropEmu.GetKeyCode("W");
					Up = InteropEmu.GetKeyCode("Up Arrow");
					Down = InteropEmu.GetKeyCode("Down Arrow");
					Left = InteropEmu.GetKeyCode("Left Arrow");
					Right = InteropEmu.GetKeyCode("Right Arrow");

					TurboA = InteropEmu.GetKeyCode("Z");
					TurboB = InteropEmu.GetKeyCode("X");
				} else if(keySetIndex == 1) {
					//XInput Default (Xbox controllers)
					A = InteropEmu.GetKeyCode("Pad1 A");
					B = InteropEmu.GetKeyCode("Pad1 X");
					Select = InteropEmu.GetKeyCode("Pad1 Back");
					Start = InteropEmu.GetKeyCode("Pad1 Start");
					Up = InteropEmu.GetKeyCode("Pad1 Up");
					Down = InteropEmu.GetKeyCode("Pad1 Down");
					Left = InteropEmu.GetKeyCode("Pad1 Left");
					Right = InteropEmu.GetKeyCode("Pad1 Right");

					TurboA = InteropEmu.GetKeyCode("Pad1 B");
					TurboB = InteropEmu.GetKeyCode("Pad1 Y");
				} else if(keySetIndex == 2) {
					//DirectInput Default (Used PS4 controller as a default)
					A = InteropEmu.GetKeyCode("Joy1 But2");
					B = InteropEmu.GetKeyCode("Joy1 But1");
					Select = InteropEmu.GetKeyCode("Joy1 But9");
					Start = InteropEmu.GetKeyCode("Joy1 But10");
					Up = InteropEmu.GetKeyCode("Joy1 DPad Up");
					Down = InteropEmu.GetKeyCode("Joy1 DPad Down");
					Left = InteropEmu.GetKeyCode("Joy1 DPad Left");
					Right = InteropEmu.GetKeyCode("Joy1 DPad Right");

					TurboA = InteropEmu.GetKeyCode("Joy1 But3");
					TurboB = InteropEmu.GetKeyCode("Joy1 But4");
				}
			} else if(controllerIndex == 1) {
				if(keySetIndex == 0) {
					A = InteropEmu.GetKeyCode("G");
					B = InteropEmu.GetKeyCode("H");
					Select = InteropEmu.GetKeyCode("T");
					Start = InteropEmu.GetKeyCode("Y");
					Up = InteropEmu.GetKeyCode("I");
					Down = InteropEmu.GetKeyCode("K");
					Left = InteropEmu.GetKeyCode("J");
					Right = InteropEmu.GetKeyCode("L");

					TurboA = InteropEmu.GetKeyCode("B");
					TurboB = InteropEmu.GetKeyCode("N");
				} else if(keySetIndex == 1) {
					//XInput Default (Xbox controllers)
					A = InteropEmu.GetKeyCode("Pad2 A");
					B = InteropEmu.GetKeyCode("Pad2 X");
					Select = InteropEmu.GetKeyCode("Pad2 Back");
					Start = InteropEmu.GetKeyCode("Pad2 Start");
					Up = InteropEmu.GetKeyCode("Pad2 Up");
					Down = InteropEmu.GetKeyCode("Pad2 Down");
					Left = InteropEmu.GetKeyCode("Pad2 Left");
					Right = InteropEmu.GetKeyCode("Pad2 Right");

					TurboA = InteropEmu.GetKeyCode("Pad2 B");
					TurboB = InteropEmu.GetKeyCode("Pad2 Y");
				} else if(keySetIndex == 2) {
					//DirectInput Default (Used PS4 controller as a default)
					A = InteropEmu.GetKeyCode("Joy2 But2");
					B = InteropEmu.GetKeyCode("Joy2 But1");
					Select = InteropEmu.GetKeyCode("Joy2 But9");
					Start = InteropEmu.GetKeyCode("Joy2 But10");
					Up = InteropEmu.GetKeyCode("Joy2 DPad Up");
					Down = InteropEmu.GetKeyCode("Joy2 DPad Down");
					Left = InteropEmu.GetKeyCode("Joy2 DPad Left");
					Right = InteropEmu.GetKeyCode("Joy2 DPad Right");

					TurboA = InteropEmu.GetKeyCode("Joy2 But3");
					TurboB = InteropEmu.GetKeyCode("Joy2 But4");
				}
			}
		}

		public InteropEmu.KeyMapping ToInteropMapping()
		{
			InteropEmu.KeyMapping mapping = new InteropEmu.KeyMapping();

			mapping.A = A;
			mapping.B = B;
			mapping.Start = Start;
			mapping.Select = Select;
			mapping.Up = Up;
			mapping.Down = Down;
			mapping.Left = Left;
			mapping.Right = Right;
			mapping.TurboA = TurboA;
			mapping.TurboB = TurboB;
			mapping.TurboStart = TurboStart;
			mapping.TurboSelect = TurboSelect;

			return mapping;
		}
	}

	public class ControllerInfo
	{
		public InteropEmu.ControllerType ControllerType = InteropEmu.ControllerType.StandardController;
		[XmlElement(ElementName = "KeyMappings")]
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

		public bool DisplayInputPort1 = false;
		public bool DisplayInputPort2 = false;
		public bool DisplayInputPort3 = false;
		public bool DisplayInputPort4 = false;
		public InteropEmu.InputDisplayPosition DisplayInputPosition = InteropEmu.InputDisplayPosition.BottomRight;
		public bool DisplayInputHorizontally = true;

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

			byte displayPorts = (byte)((inputInfo.DisplayInputPort1 ? 1 : 0) + (inputInfo.DisplayInputPort2 ? 2 : 0) + (inputInfo.DisplayInputPort3 ? 4 : 0) + (inputInfo.DisplayInputPort4 ? 8 : 0));
			InteropEmu.SetInputDisplaySettings(displayPorts, inputInfo.DisplayInputPosition, inputInfo.DisplayInputHorizontally);
		}
	}
}
