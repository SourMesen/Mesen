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
		public UInt32[] JissenMahjongButtons = new UInt32[21];
		public UInt32[] SuborKeyboardButtons = new UInt32[99];

		public KeyMappings()
		{
		}

		public KeyMappings Clone()
		{
			KeyMappings clone = (KeyMappings)this.MemberwiseClone();
			clone.PowerPadButtons = (UInt32[])this.PowerPadButtons.Clone();
			clone.FamilyBasicKeyboardButtons = (UInt32[])this.FamilyBasicKeyboardButtons.Clone();
			clone.PartyTapButtons = (UInt32[])this.PartyTapButtons.Clone();
			clone.PachinkoButtons = (UInt32[])this.PachinkoButtons.Clone();
			clone.ExcitingBoxingButtons = (UInt32[])this.ExcitingBoxingButtons.Clone();
			clone.JissenMahjongButtons = (UInt32[])this.JissenMahjongButtons.Clone();
			clone.SuborKeyboardButtons = (UInt32[])this.SuborKeyboardButtons.Clone();			
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
			mapping.FamilyBasicKeyboardButtons = FamilyBasicKeyboardButtons;
			mapping.PartyTapButtons= PartyTapButtons;
			mapping.PachinkoButtons = PachinkoButtons;
			mapping.ExcitingBoxingButtons= ExcitingBoxingButtons;
			mapping.JissenMahjongButtons = JissenMahjongButtons;
			mapping.SuborKeyboardButtons = SuborKeyboardButtons;

			mapping.PachinkoButtons = new UInt32[2] {
				InteropEmu.GetKeyCode("I"),
				InteropEmu.GetKeyCode("K"),
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
					if(controllerInfo.Keys.Count == 0) {
						controllerInfo.Keys.Add(new KeyMappings());
					}

					controllerInfo.Keys[0].ExcitingBoxingButtons = presets.ExcitingBoxing.ExcitingBoxingButtons;
					controllerInfo.Keys[0].FamilyBasicKeyboardButtons = presets.FamilyBasic.FamilyBasicKeyboardButtons;
					controllerInfo.Keys[0].JissenMahjongButtons = presets.JissenMahjong.JissenMahjongButtons;
					//controllerInfo.Keys[0].PachinkoButtons = presets.Pachinko.PachinkoButtons;
					controllerInfo.Keys[0].PartyTapButtons = presets.PartyTap.PartyTapButtons;
					controllerInfo.Keys[0].PowerPadButtons = presets.PowerPad.PowerPadButtons;
					controllerInfo.Keys[0].SuborKeyboardButtons = presets.SuborKeyboard.SuborKeyboardButtons;

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
