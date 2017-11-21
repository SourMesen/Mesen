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

		public UInt32 Microphone;

		public UInt32 LButton;
		public UInt32 RButton;

		public UInt32[] PowerPadButtons = new UInt32[12];
		public UInt32[] FamilyBasicKeyboardButtons = new UInt32[72];
		public UInt32[] PartyTapButtons = new UInt32[6];
		public UInt32[] PachinkoButtons = new UInt32[2];
		public UInt32[] ExcitingBoxingButtons = new UInt32[8];
		public UInt32[] JissenMahjong = new UInt32[21];
		public UInt32[] SuborKeyboardButtons = new UInt32[99];

		public KeyMappings()
		{
		}

		public KeyMappings Clone()
		{
			KeyMappings clone = (KeyMappings)this.MemberwiseClone();
			clone.PowerPadButtons = new UInt32[12];
			clone.FamilyBasicKeyboardButtons = new UInt32[72];
			clone.PartyTapButtons = new UInt32[6];
			clone.PachinkoButtons = new UInt32[2];
			clone.ExcitingBoxingButtons = new UInt32[8];
			clone.JissenMahjong = new UInt32[21];
			clone.SuborKeyboardButtons = new UInt32[99];
			return clone;
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
			mapping.Microphone = Microphone;
			mapping.LButton = LButton;
			mapping.RButton = RButton;
			mapping.PowerPadButtons = PowerPadButtons;
			//mapping.FamilyBasicKeyboardButtons = FamilyBasicKeyboardButtons;

			mapping.FamilyBasicKeyboardButtons = new UInt32[72] {
				InteropEmu.GetKeyCode("A"), InteropEmu.GetKeyCode("B"), InteropEmu.GetKeyCode("C"), InteropEmu.GetKeyCode("D"),
				InteropEmu.GetKeyCode("E"), InteropEmu.GetKeyCode("F"), InteropEmu.GetKeyCode("G"), InteropEmu.GetKeyCode("H"),
				InteropEmu.GetKeyCode("I"), InteropEmu.GetKeyCode("J"), InteropEmu.GetKeyCode("K"), InteropEmu.GetKeyCode("L"),
				InteropEmu.GetKeyCode("M"), InteropEmu.GetKeyCode("N"), InteropEmu.GetKeyCode("O"), InteropEmu.GetKeyCode("P"),
				InteropEmu.GetKeyCode("Q"), InteropEmu.GetKeyCode("R"), InteropEmu.GetKeyCode("S"), InteropEmu.GetKeyCode("T"),
				InteropEmu.GetKeyCode("U"), InteropEmu.GetKeyCode("V"), InteropEmu.GetKeyCode("W"), InteropEmu.GetKeyCode("X"),
				InteropEmu.GetKeyCode("Y"), InteropEmu.GetKeyCode("Z"), InteropEmu.GetKeyCode("0"), InteropEmu.GetKeyCode("1"),
				InteropEmu.GetKeyCode("2"), InteropEmu.GetKeyCode("3"), InteropEmu.GetKeyCode("4"), InteropEmu.GetKeyCode("5"),
				InteropEmu.GetKeyCode("6"), InteropEmu.GetKeyCode("7"), InteropEmu.GetKeyCode("8"), InteropEmu.GetKeyCode("9"),
				InteropEmu.GetKeyCode("Enter"), InteropEmu.GetKeyCode("Spacebar"), InteropEmu.GetKeyCode("Delete"), InteropEmu.GetKeyCode("Insert"),
				InteropEmu.GetKeyCode("Esc"), InteropEmu.GetKeyCode("Ctrl"), InteropEmu.GetKeyCode("RIGHT SHIFT"), InteropEmu.GetKeyCode("Shift"),
				InteropEmu.GetKeyCode("["), InteropEmu.GetKeyCode("]"),
				InteropEmu.GetKeyCode("Up Arrow"), InteropEmu.GetKeyCode("Down Arrow"), InteropEmu.GetKeyCode("Left Arrow"), InteropEmu.GetKeyCode("Right Arrow"),
				InteropEmu.GetKeyCode("."), InteropEmu.GetKeyCode(","), InteropEmu.GetKeyCode("'"), InteropEmu.GetKeyCode(";"),
				InteropEmu.GetKeyCode("="), InteropEmu.GetKeyCode("/"), InteropEmu.GetKeyCode("-"), InteropEmu.GetKeyCode("`"),
				InteropEmu.GetKeyCode("F1"), InteropEmu.GetKeyCode("F2"), InteropEmu.GetKeyCode("F3"), InteropEmu.GetKeyCode("F4"),
				InteropEmu.GetKeyCode("F5"), InteropEmu.GetKeyCode("F6"), InteropEmu.GetKeyCode("F7"), InteropEmu.GetKeyCode("F8"),
				InteropEmu.GetKeyCode("\\"), InteropEmu.GetKeyCode("Page Up"), InteropEmu.GetKeyCode("F9"), InteropEmu.GetKeyCode("Page Down"),
				InteropEmu.GetKeyCode("Home"), InteropEmu.GetKeyCode("End")
			};

			mapping.PartyTapButtons = new UInt32[6] {
				InteropEmu.GetKeyCode("1"),
				InteropEmu.GetKeyCode("2"),
				InteropEmu.GetKeyCode("3"),
				InteropEmu.GetKeyCode("4"),
				InteropEmu.GetKeyCode("5"),
				InteropEmu.GetKeyCode("6"),
			};

			mapping.PachinkoButtons = new UInt32[2] {
				InteropEmu.GetKeyCode("I"),
				InteropEmu.GetKeyCode("K"),
			};

			mapping.ExcitingBoxingButtons = new UInt32[8] {
				InteropEmu.GetKeyCode("Numpad 7"),
				InteropEmu.GetKeyCode("Numpad 6"),
				InteropEmu.GetKeyCode("Numpad 4"),
				InteropEmu.GetKeyCode("Numpad 9"),
				InteropEmu.GetKeyCode("Numpad 1"),
				InteropEmu.GetKeyCode("Numpad 5"),
				InteropEmu.GetKeyCode("Numpad 3"),
				InteropEmu.GetKeyCode("Numpad 8"),
			};

			mapping.JissenMahjongButtons = new UInt32[21] {
				InteropEmu.GetKeyCode("A"),
				InteropEmu.GetKeyCode("B"),
				InteropEmu.GetKeyCode("C"),
				InteropEmu.GetKeyCode("D"),
				InteropEmu.GetKeyCode("E"),
				InteropEmu.GetKeyCode("F"),
				InteropEmu.GetKeyCode("G"),
				InteropEmu.GetKeyCode("H"),
				InteropEmu.GetKeyCode("I"),
				InteropEmu.GetKeyCode("J"),
				InteropEmu.GetKeyCode("K"),
				InteropEmu.GetKeyCode("L"),
				InteropEmu.GetKeyCode("M"),
				InteropEmu.GetKeyCode("N"),
				InteropEmu.GetKeyCode("Spacebar"),
				InteropEmu.GetKeyCode("Enter"),
				InteropEmu.GetKeyCode("1"),
				InteropEmu.GetKeyCode("2"),
				InteropEmu.GetKeyCode("3"),
				InteropEmu.GetKeyCode("4"),
				InteropEmu.GetKeyCode("5"),
			};

			mapping.SuborKeyboardButtons = new UInt32[99] {
				InteropEmu.GetKeyCode("4"), InteropEmu.GetKeyCode("G"), InteropEmu.GetKeyCode("F"), InteropEmu.GetKeyCode("C"), InteropEmu.GetKeyCode("F2"), InteropEmu.GetKeyCode("E"), InteropEmu.GetKeyCode("5"), InteropEmu.GetKeyCode("V"),
				InteropEmu.GetKeyCode("2"), InteropEmu.GetKeyCode("D"), InteropEmu.GetKeyCode("S"), InteropEmu.GetKeyCode("End"), InteropEmu.GetKeyCode("F1"), InteropEmu.GetKeyCode("W"), InteropEmu.GetKeyCode("3"), InteropEmu.GetKeyCode("X"),
				InteropEmu.GetKeyCode("Insert"), InteropEmu.GetKeyCode("Backspace"), InteropEmu.GetKeyCode("Page Down"), InteropEmu.GetKeyCode("Right Arrow"), InteropEmu.GetKeyCode("F8"), InteropEmu.GetKeyCode("Page Up"), InteropEmu.GetKeyCode("Delete"), InteropEmu.GetKeyCode("Home"),
				InteropEmu.GetKeyCode("9"), InteropEmu.GetKeyCode("I"), InteropEmu.GetKeyCode("L"), InteropEmu.GetKeyCode(","), InteropEmu.GetKeyCode("F5"), InteropEmu.GetKeyCode("O"), InteropEmu.GetKeyCode("0"), InteropEmu.GetKeyCode("."),
				InteropEmu.GetKeyCode("]"), InteropEmu.GetKeyCode("Enter"), InteropEmu.GetKeyCode("Up Arrow"), InteropEmu.GetKeyCode("Left Arrow"), InteropEmu.GetKeyCode("F7"), InteropEmu.GetKeyCode("["), InteropEmu.GetKeyCode("\\"), InteropEmu.GetKeyCode("Down Arrow"),
				InteropEmu.GetKeyCode("Q"), InteropEmu.GetKeyCode("Caps Lock"), InteropEmu.GetKeyCode("Z"), InteropEmu.GetKeyCode("Tab"), InteropEmu.GetKeyCode("Esc"), InteropEmu.GetKeyCode("A"), InteropEmu.GetKeyCode("1"), InteropEmu.GetKeyCode("Ctrl"),
				InteropEmu.GetKeyCode("7"), InteropEmu.GetKeyCode("Y"), InteropEmu.GetKeyCode("K"), InteropEmu.GetKeyCode("M"), InteropEmu.GetKeyCode("F4"), InteropEmu.GetKeyCode("U"), InteropEmu.GetKeyCode("8"), InteropEmu.GetKeyCode("J"),
				InteropEmu.GetKeyCode("-"), InteropEmu.GetKeyCode(";"), InteropEmu.GetKeyCode("'"), InteropEmu.GetKeyCode("/"), InteropEmu.GetKeyCode("F6"), InteropEmu.GetKeyCode("P"), InteropEmu.GetKeyCode("="), InteropEmu.GetKeyCode("Shift"),
				InteropEmu.GetKeyCode("T"), InteropEmu.GetKeyCode("H"), InteropEmu.GetKeyCode("N"), InteropEmu.GetKeyCode("Spacebar"), InteropEmu.GetKeyCode("F3"), InteropEmu.GetKeyCode("R"), InteropEmu.GetKeyCode("6"), InteropEmu.GetKeyCode("B"),
				InteropEmu.GetKeyCode("Numpad Enter"), 0, 0, 0,
				InteropEmu.GetKeyCode("Left Menu"), InteropEmu.GetKeyCode("Numpad 4"), InteropEmu.GetKeyCode("Numpad 7"), InteropEmu.GetKeyCode("F11"), InteropEmu.GetKeyCode("F12"), InteropEmu.GetKeyCode("Numpad 1"), InteropEmu.GetKeyCode("Numpad 2"), InteropEmu.GetKeyCode("Numpad 8"),
				InteropEmu.GetKeyCode("Numpad -"), InteropEmu.GetKeyCode("Numpad +"), InteropEmu.GetKeyCode("Numpad *"), InteropEmu.GetKeyCode("Numpad 9"), InteropEmu.GetKeyCode("F10"), InteropEmu.GetKeyCode("Numpad 5"), InteropEmu.GetKeyCode("Numpad /"), InteropEmu.GetKeyCode("Num Lock"),
				InteropEmu.GetKeyCode("`"), InteropEmu.GetKeyCode("Numpad 6"), InteropEmu.GetKeyCode("Pause"), InteropEmu.GetKeyCode("F9"), InteropEmu.GetKeyCode("Numpad 3"), InteropEmu.GetKeyCode("Numpad ."), InteropEmu.GetKeyCode("Numpad 0")
			};

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

	public class ZapperInfo
	{
		[MinMax(0, 3)] public UInt32 DetectionRadius = 0;
	}

	public class InputInfo
	{
		public DefaultKeyMappingType DefaultMapping;

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
		public ZapperInfo Zapper = new ZapperInfo();

		public void InitializeDefaults()
		{
			KeyPresets presets = new KeyPresets();
			while(Controllers.Count < 4) {
				var controllerInfo = new ControllerInfo();
				controllerInfo.ControllerType = Controllers.Count <= 1 ? InteropEmu.ControllerType.StandardController : InteropEmu.ControllerType.None;

				if(Controllers.Count == 0) {
					if(DefaultMapping.HasFlag(DefaultKeyMappingType.Xbox)) {
						controllerInfo.Keys.Add(presets.XboxLayout1);
					}
					if(DefaultMapping.HasFlag(DefaultKeyMappingType.Ps4)) {
						controllerInfo.Keys.Add(presets.Ps4Layout1);
					}
					if(DefaultMapping.HasFlag(DefaultKeyMappingType.WasdKeys)) {
						controllerInfo.Keys.Add(presets.WasdLayout);
					}
					if(DefaultMapping.HasFlag(DefaultKeyMappingType.ArrowKeys)) {
						controllerInfo.Keys.Add(presets.ArrowLayout);
					}
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

			InteropEmu.SetZapperDetectionRadius(inputInfo.Zapper.DetectionRadius);
		}
	}
}
