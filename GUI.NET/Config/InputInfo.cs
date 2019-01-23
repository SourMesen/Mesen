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

		[XmlElement("PowerPad")] public XmlIntArray PowerPadButtons = new UInt32[12];
		[XmlElement("FamilyBasic")] public XmlIntArray FamilyBasicKeyboardButtons = new UInt32[72];
		[XmlElement("PartyTap")] public XmlIntArray PartyTapButtons = new UInt32[6];
		[XmlElement("Pachinko")] public XmlIntArray PachinkoButtons = new UInt32[2];
		[XmlElement("ExcitingBoxing")] public XmlIntArray ExcitingBoxingButtons = new UInt32[8];
		[XmlElement("JissenMahjong")] public XmlIntArray JissenMahjongButtons = new UInt32[21];
		[XmlElement("SuborKeyboard")] public XmlIntArray SuborKeyboardButtons = new UInt32[99];
		[XmlElement("BandaiMicrophone")] public XmlIntArray BandaiMicrophoneButtons = new UInt32[3];

		public KeyMappings()
		{
		}

		public KeyMappings Clone()
		{
			KeyMappings clone = (KeyMappings)this.MemberwiseClone();
			clone.PowerPadButtons = this.PowerPadButtons.Clone();
			clone.FamilyBasicKeyboardButtons = this.FamilyBasicKeyboardButtons.Clone();
			clone.PartyTapButtons = this.PartyTapButtons.Clone();
			clone.PachinkoButtons = this.PachinkoButtons.Clone();
			clone.ExcitingBoxingButtons = this.ExcitingBoxingButtons.Clone();
			clone.JissenMahjongButtons = this.JissenMahjongButtons.Clone();
			clone.SuborKeyboardButtons = this.SuborKeyboardButtons.Clone();			
			clone.BandaiMicrophoneButtons = this.BandaiMicrophoneButtons.Clone();			
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
			mapping.BandaiMicrophoneButtons = BandaiMicrophoneButtons;

			return mapping;
		}
	}

	public class ControllerInfo
	{
		public InteropEmu.ControllerType ControllerType = InteropEmu.ControllerType.StandardController;
		public List<KeyMappings> Keys = new List<KeyMappings>();
		public UInt32 TurboSpeed = 2;
		public bool PowerpadUseSideA = false;

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
			mappingSet.PowerpadUseSideA = PowerpadUseSideA;
			return mappingSet;
		}
	}

	public class ZapperInfo
	{
		[MinMax(0, 3)] public UInt32 DetectionRadius = 0;
	}

	public class MouseInfo
	{
		[MinMax(0, 3)] public UInt32 Sensitivity = 1;
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

		[MinMax(0, 4)] public UInt32 ControllerDeadzoneSize = 2;
		public bool HideMousePointerForZapper = true;

		[XmlElement(ElementName = "InputDevice")]
		public List<ControllerInfo> Controllers = new List<ControllerInfo>();
		public ZapperInfo Zapper = new ZapperInfo();
		public MouseInfo ArkanoidController = new MouseInfo();
		public MouseInfo HoriTrack = new MouseInfo();
		public MouseInfo SnesMouse = new MouseInfo();
		public MouseInfo SuborMouse = new MouseInfo();

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
					controllerInfo.Keys[0].PachinkoButtons = presets.Pachinko.PachinkoButtons;
					controllerInfo.Keys[0].PartyTapButtons = presets.PartyTap.PartyTapButtons;
					controllerInfo.Keys[0].PowerPadButtons = presets.PowerPad.PowerPadButtons;
					controllerInfo.Keys[0].SuborKeyboardButtons = presets.SuborKeyboard.SuborKeyboardButtons;
					controllerInfo.Keys[0].BandaiMicrophoneButtons = presets.BandaiMicrophone.BandaiMicrophoneButtons;
				} else if(Controllers.Count == 1) {
					if(controllerInfo.Keys.Count == 0) {
						controllerInfo.Keys.Add(new KeyMappings());
					}
					controllerInfo.Keys[0].PowerPadButtons = presets.PowerPad.PowerPadButtons;
				}

				while(controllerInfo.Keys.Count < 4) {
					controllerInfo.Keys.Add(new KeyMappings());
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
			InteropEmu.SetControllerDeadzoneSize(inputInfo.ControllerDeadzoneSize);

			InteropEmu.SetMouseSensitivity(InteropEmu.MouseDevice.ArkanoidController, (inputInfo.ArkanoidController.Sensitivity + 1) / 2.0);
			InteropEmu.SetMouseSensitivity(InteropEmu.MouseDevice.HoriTrack, (inputInfo.HoriTrack.Sensitivity + 1) / 2.0);
			InteropEmu.SetMouseSensitivity(InteropEmu.MouseDevice.SnesMouse, (inputInfo.SnesMouse.Sensitivity + 1) / 2.0);
			InteropEmu.SetMouseSensitivity(InteropEmu.MouseDevice.SuborMouse, (inputInfo.SuborMouse.Sensitivity + 1) / 2.0);
		}
	}

	public class XmlIntArray
	{
		private List<UInt32> _values = new List<UInt32>();

		public XmlIntArray() { }

		public XmlIntArray(List<UInt32> values)
		{
			_values = new List<UInt32>(values);
			KeyCount = _values.Count;
		}

		public XmlIntArray(UInt32[] values)
		{
			_values = new List<UInt32>(values);
			KeyCount = _values.Count;
		}

		[XmlIgnore]
		public List<UInt32> Values
		{
			get { return new List<UInt32>(_values); }
		}

		public XmlIntArray Clone()
		{
			return new XmlIntArray(this.Values);
		}

		public static implicit operator List<UInt32>(XmlIntArray x)
		{
			return new List<UInt32>(x.Values);
		}

		public static implicit operator UInt32[](XmlIntArray x)
		{
			return x.Values.ToArray();
		}

		public static implicit operator XmlIntArray(List<UInt32> values)
		{
			return new XmlIntArray(values);
		}

		public static implicit operator XmlIntArray(UInt32[] values)
		{
			return new XmlIntArray(values);
		}

		[XmlElement(Order = 0)]
		public int KeyCount;

		[XmlElement("Values", Order = 1)]
		public string IntValues
		{
			get { return _values.Any(v => v > 0) ? string.Join(",", _values) : ""; }
			set
			{
				try {
					if(!string.IsNullOrWhiteSpace(value)) {
						List<UInt32> values = new List<UInt32>();
						foreach(string val in value.Split(',')) {
							values.Add(UInt32.Parse(val));
						}
						while(values.Count < KeyCount) {
							values.Add(0);
						}
						_values = values;
					} else {
						_values = new List<UInt32>(new UInt32[KeyCount]);
					}
				} catch {
					_values = new List<UInt32>(new UInt32[KeyCount]);
				}
			}
		}

		public UInt32 this[int index]
		{
			get { return _values[index]; }
			set { _values[index] = value; }
		}
	}
}
