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

		public KeyMappings Clone()
		{
			return (KeyMappings)this.MemberwiseClone();
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

		[XmlElement(ElementName = "InputDevice")]
		public List<ControllerInfo> Controllers = new List<ControllerInfo>();

		public void InitializeDefaults()
		{
			KeyPresets presets = new KeyPresets();
			while(Controllers.Count < 4) {
				var controllerInfo = new ControllerInfo();
				controllerInfo.ControllerType = Controllers.Count <= 1 ? InteropEmu.ControllerType.StandardController : InteropEmu.ControllerType.None;

				if(Controllers.Count <= 1) {
					controllerInfo.Keys.Add(Controllers.Count == 0 ? presets.ArrowLayout : presets.Player2KeyboardLayout);
					controllerInfo.Keys.Add(Controllers.Count == 0 ? presets.XboxLayout1 : presets.XboxLayout2);
					controllerInfo.Keys.Add(Controllers.Count == 0 ? presets.Ps4Layout1 : presets.Ps4Layout2);
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
