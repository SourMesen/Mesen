using System;

namespace Mesen.GUI.Config
{
	public class KeyPresets
	{
		KeyMappings _wasdLayout;
		KeyMappings _arrowLayout;
		KeyMappings _nestopiaLayout;
		KeyMappings _fceuxLayout;
		KeyMappings _player2KeyboardLayout;
		KeyMappings[] _xboxLayouts = new KeyMappings[2];
		KeyMappings[] _ps4Layouts = new KeyMappings[2];
		KeyMappings[] _snes30Layouts = new KeyMappings[2];

		KeyMappings _familyBasic;
		public KeyMappings FamilyBasic { get { return _familyBasic.Clone(); } }

		KeyMappings _excitingBoxing;
		public KeyMappings ExcitingBoxing { get { return _excitingBoxing.Clone(); } }

		KeyMappings _partyTap;
		public KeyMappings PartyTap { get { return _partyTap.Clone(); } }

		KeyMappings _pachinko;
		public KeyMappings Pachinko { get { return _pachinko.Clone(); } }

		KeyMappings _powerPad;
		public KeyMappings PowerPad { get { return _powerPad.Clone(); } }
		
		KeyMappings _jissenMahjong;
		public KeyMappings JissenMahjong { get { return _jissenMahjong.Clone(); } }

		KeyMappings _suborKeyboard;
		public KeyMappings SuborKeyboard { get { return _suborKeyboard.Clone(); } }

		public KeyMappings WasdLayout { get { return _wasdLayout.Clone(); } }
		public KeyMappings ArrowLayout { get { return _arrowLayout.Clone(); } }
		public KeyMappings NestopiaLayout { get { return _nestopiaLayout.Clone(); } }
		public KeyMappings FceuxLayout { get { return _fceuxLayout.Clone(); } }
		public KeyMappings XboxLayout1 { get { return _xboxLayouts[0].Clone(); } }
		public KeyMappings XboxLayout2 { get { return _xboxLayouts[1].Clone(); } }
		public KeyMappings Ps4Layout1 { get { return _ps4Layouts[0].Clone(); } }
		public KeyMappings Ps4Layout2 { get { return _ps4Layouts[1].Clone(); } }
		public KeyMappings Snes30Layout1 { get { return _snes30Layouts[0].Clone(); } }
		public KeyMappings Snes30Layout2 { get { return _snes30Layouts[1].Clone(); } }
		public KeyMappings Player2KeyboardLayout { get { return _player2KeyboardLayout.Clone(); } }

		public KeyPresets()
		{
			_wasdLayout = new KeyMappings() {
				A = InteropEmu.GetKeyCode("K"), B = InteropEmu.GetKeyCode("J"),
				TurboA = InteropEmu.GetKeyCode(","), TurboB = InteropEmu.GetKeyCode("M"),
				Select = InteropEmu.GetKeyCode("U"), Start = InteropEmu.GetKeyCode("I"),
				Up = InteropEmu.GetKeyCode("W"), Down = InteropEmu.GetKeyCode("S"),
				Left = InteropEmu.GetKeyCode("A"), Right = InteropEmu.GetKeyCode("D")
			};

			_arrowLayout= new KeyMappings() {
				A = InteropEmu.GetKeyCode("S"), B = InteropEmu.GetKeyCode("A"),
				TurboA = InteropEmu.GetKeyCode("X"), TurboB = InteropEmu.GetKeyCode("Z"),
				Select = InteropEmu.GetKeyCode("Q"), Start = InteropEmu.GetKeyCode("W"),
				Up = InteropEmu.GetKeyCode("Up Arrow"), Down = InteropEmu.GetKeyCode("Down Arrow"),
				Left = InteropEmu.GetKeyCode("Left Arrow"), Right = InteropEmu.GetKeyCode("Right Arrow")
			};

			_nestopiaLayout = new KeyMappings() {
				A = InteropEmu.GetKeyCode("."), B = InteropEmu.GetKeyCode(","),
				TurboA = InteropEmu.GetKeyCode("L"), TurboB = InteropEmu.GetKeyCode("K"),
				Select = InteropEmu.GetKeyCode("Shift"), Start = InteropEmu.GetKeyCode("Enter"),
				Up = InteropEmu.GetKeyCode("Up Arrow"), Down = InteropEmu.GetKeyCode("Down Arrow"),
				Left = InteropEmu.GetKeyCode("Left Arrow"), Right = InteropEmu.GetKeyCode("Right Arrow")
			};

			_fceuxLayout = new KeyMappings() {
				A = InteropEmu.GetKeyCode("F"), B = InteropEmu.GetKeyCode("D"),
				TurboA = 0, TurboB = 0,
				Select = InteropEmu.GetKeyCode("S"), Start = InteropEmu.GetKeyCode("Enter"),
				Up = InteropEmu.GetKeyCode("Up Arrow"), Down = InteropEmu.GetKeyCode("Down Arrow"),
				Left = InteropEmu.GetKeyCode("Left Arrow"), Right = InteropEmu.GetKeyCode("Right Arrow")
			};

			_player2KeyboardLayout = new KeyMappings() {
				A = InteropEmu.GetKeyCode(";"), B = InteropEmu.GetKeyCode("L"),
				TurboA = InteropEmu.GetKeyCode("/"), TurboB = InteropEmu.GetKeyCode("."),
				Select = InteropEmu.GetKeyCode("O"), Start = InteropEmu.GetKeyCode("P"),
				Up = InteropEmu.GetKeyCode("Y"), Down = InteropEmu.GetKeyCode("H"),
				Left = InteropEmu.GetKeyCode("G"), Right = InteropEmu.GetKeyCode("J")
			};

			if(Program.IsMono) {
				for(int i = 0; i < 2; i++) {
					string prefix = "Pad" + (i+1).ToString() + " ";
					_xboxLayouts[i] = new KeyMappings() {
						A = InteropEmu.GetKeyCode(prefix + "A"), B = InteropEmu.GetKeyCode(prefix + "X"),
						TurboA = InteropEmu.GetKeyCode(prefix + "B"), TurboB = InteropEmu.GetKeyCode(prefix + "Y"),
						Select = InteropEmu.GetKeyCode(prefix + "Select"), Start = InteropEmu.GetKeyCode(prefix + "Start"),
						Up = InteropEmu.GetKeyCode(prefix + "Up"), Down = InteropEmu.GetKeyCode(prefix + "Down"),
						Left = InteropEmu.GetKeyCode(prefix + "Left"), Right = InteropEmu.GetKeyCode(prefix + "Right")
					};

					_ps4Layouts[i] = new KeyMappings() {
						A = InteropEmu.GetKeyCode(prefix + "B"), B = InteropEmu.GetKeyCode(prefix + "A"),
						TurboA = InteropEmu.GetKeyCode(prefix + "C"), TurboB = InteropEmu.GetKeyCode(prefix + "X"),
						Select = InteropEmu.GetKeyCode(prefix + "L2"), Start = InteropEmu.GetKeyCode(prefix + "R2"),
						Up = InteropEmu.GetKeyCode(prefix + "Up"), Down = InteropEmu.GetKeyCode(prefix + "Down"),
						Left = InteropEmu.GetKeyCode(prefix + "Left"), Right = InteropEmu.GetKeyCode(prefix + "Right")
					};

					_snes30Layouts[i] = new KeyMappings() {
						A = InteropEmu.GetKeyCode(prefix + "Thumb"), B = InteropEmu.GetKeyCode(prefix + "Top2"),
						TurboA = InteropEmu.GetKeyCode(prefix + "Trigger"), TurboB = InteropEmu.GetKeyCode(prefix + "Top"),
						Select = InteropEmu.GetKeyCode(prefix + "Base5"), Start = InteropEmu.GetKeyCode(prefix + "Base6"),
						Up = InteropEmu.GetKeyCode(prefix + "Y-"), Down = InteropEmu.GetKeyCode(prefix + "Y+"),
						Left = InteropEmu.GetKeyCode(prefix + "X-"), Right = InteropEmu.GetKeyCode(prefix + "X+")
					};
				}
			} else {
				for(int i = 0; i < 2; i++) {
					string prefix = "Pad" + (i+1).ToString() + " ";
					_xboxLayouts[i] = new KeyMappings() {
						A = InteropEmu.GetKeyCode(prefix + "A"), B = InteropEmu.GetKeyCode(prefix + "X"),
						TurboA = InteropEmu.GetKeyCode(prefix + "B"), TurboB = InteropEmu.GetKeyCode(prefix + "Y"),
						Select = InteropEmu.GetKeyCode(prefix + "Back"), Start = InteropEmu.GetKeyCode(prefix + "Start"),
						Up = InteropEmu.GetKeyCode(prefix + "Up"), Down = InteropEmu.GetKeyCode(prefix + "Down"),
						Left = InteropEmu.GetKeyCode(prefix + "Left"), Right = InteropEmu.GetKeyCode(prefix + "Right")
					};

					prefix = "Joy" + (i+1).ToString() + " ";
					_ps4Layouts[i] = new KeyMappings() {
						A = InteropEmu.GetKeyCode(prefix + "But2"), B = InteropEmu.GetKeyCode(prefix + "But1"),
						TurboA = InteropEmu.GetKeyCode(prefix + "But3"), TurboB = InteropEmu.GetKeyCode(prefix + "But4"),
						Select = InteropEmu.GetKeyCode(prefix + "But9"), Start = InteropEmu.GetKeyCode(prefix + "But10"),
						Up = InteropEmu.GetKeyCode(prefix + "DPad Up"), Down = InteropEmu.GetKeyCode(prefix + "DPad Down"),
						Left = InteropEmu.GetKeyCode(prefix + "DPad Left"), Right = InteropEmu.GetKeyCode(prefix + "DPad Right")
					};

					_snes30Layouts[i] = new KeyMappings() {
						A = InteropEmu.GetKeyCode(prefix + "But2"), B = InteropEmu.GetKeyCode(prefix + "But5"),
						TurboA = InteropEmu.GetKeyCode(prefix + "But1"), TurboB = InteropEmu.GetKeyCode(prefix + "But4"),
						Select = InteropEmu.GetKeyCode(prefix + "But11"), Start = InteropEmu.GetKeyCode(prefix + "But12"),
						Up = InteropEmu.GetKeyCode(prefix + "Y+"), Down = InteropEmu.GetKeyCode(prefix + "Y-"),
						Left = InteropEmu.GetKeyCode(prefix + "X-"), Right = InteropEmu.GetKeyCode(prefix + "X+")
					};
				}
			}

			_familyBasic = new KeyMappings() {
				FamilyBasicKeyboardButtons = new UInt32[72] {
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
					InteropEmu.GetKeyCode("Esc"), InteropEmu.GetKeyCode("Ctrl"), InteropEmu.GetKeyCode("Menu"), InteropEmu.GetKeyCode("Shift"),
					InteropEmu.GetKeyCode("["), InteropEmu.GetKeyCode("]"),
					InteropEmu.GetKeyCode("Up Arrow"), InteropEmu.GetKeyCode("Down Arrow"), InteropEmu.GetKeyCode("Left Arrow"), InteropEmu.GetKeyCode("Right Arrow"),
					InteropEmu.GetKeyCode("."), InteropEmu.GetKeyCode(","), InteropEmu.GetKeyCode("'"), InteropEmu.GetKeyCode(";"),
					InteropEmu.GetKeyCode("="), InteropEmu.GetKeyCode("/"), InteropEmu.GetKeyCode("-"), InteropEmu.GetKeyCode("`"),
					InteropEmu.GetKeyCode("F1"), InteropEmu.GetKeyCode("F2"), InteropEmu.GetKeyCode("F3"), InteropEmu.GetKeyCode("F4"),
					InteropEmu.GetKeyCode("F5"), InteropEmu.GetKeyCode("F6"), InteropEmu.GetKeyCode("F7"), InteropEmu.GetKeyCode("F8"),
					InteropEmu.GetKeyCode("\\"), InteropEmu.GetKeyCode("Backspace"), InteropEmu.GetKeyCode("F9"), InteropEmu.GetKeyCode("Alt"),
					InteropEmu.GetKeyCode("Home"), InteropEmu.GetKeyCode("End")
				}
			};

			_powerPad = new KeyMappings() {
				PowerPadButtons = new UInt32[12] {
					InteropEmu.GetKeyCode("R"),
					InteropEmu.GetKeyCode("T"),
					InteropEmu.GetKeyCode("Y"),
					InteropEmu.GetKeyCode("U"),
					InteropEmu.GetKeyCode("F"),
					InteropEmu.GetKeyCode("G"),
					InteropEmu.GetKeyCode("H"),
					InteropEmu.GetKeyCode("J"),
					InteropEmu.GetKeyCode("V"),
					InteropEmu.GetKeyCode("B"),
					InteropEmu.GetKeyCode("N"),
					InteropEmu.GetKeyCode("M"),
				}
			};

			_partyTap = new KeyMappings() {
				PartyTapButtons = new UInt32[6] {
					InteropEmu.GetKeyCode("1"),
					InteropEmu.GetKeyCode("2"),
					InteropEmu.GetKeyCode("3"),
					InteropEmu.GetKeyCode("4"),
					InteropEmu.GetKeyCode("5"),
					InteropEmu.GetKeyCode("6"),
				}
			};

			_pachinko = new KeyMappings() {
				PachinkoButtons = new UInt32[2] {
					InteropEmu.GetKeyCode("R"),
					InteropEmu.GetKeyCode("F")
				}
			};

			_excitingBoxing = new KeyMappings() {
				ExcitingBoxingButtons = new UInt32[8] {
					InteropEmu.GetKeyCode("Numpad 7"),
					InteropEmu.GetKeyCode("Numpad 6"),
					InteropEmu.GetKeyCode("Numpad 4"),
					InteropEmu.GetKeyCode("Numpad 9"),
					InteropEmu.GetKeyCode("Numpad 1"),
					InteropEmu.GetKeyCode("Numpad 5"),
					InteropEmu.GetKeyCode("Numpad 3"),
					InteropEmu.GetKeyCode("Numpad 8"),
				}
			};

			_jissenMahjong = new KeyMappings() {
				JissenMahjongButtons = new UInt32[21] {
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
				}
			};

			_suborKeyboard = new KeyMappings() {
				SuborKeyboardButtons = new UInt32[99] {
					InteropEmu.GetKeyCode("A"), InteropEmu.GetKeyCode("B"), InteropEmu.GetKeyCode("C"), InteropEmu.GetKeyCode("D"),
					InteropEmu.GetKeyCode("E"), InteropEmu.GetKeyCode("F"), InteropEmu.GetKeyCode("G"), InteropEmu.GetKeyCode("H"),
					InteropEmu.GetKeyCode("I"), InteropEmu.GetKeyCode("J"), InteropEmu.GetKeyCode("K"), InteropEmu.GetKeyCode("L"),
					InteropEmu.GetKeyCode("M"), InteropEmu.GetKeyCode("N"), InteropEmu.GetKeyCode("O"), InteropEmu.GetKeyCode("P"),
					InteropEmu.GetKeyCode("Q"), InteropEmu.GetKeyCode("R"), InteropEmu.GetKeyCode("S"), InteropEmu.GetKeyCode("T"),
					InteropEmu.GetKeyCode("U"), InteropEmu.GetKeyCode("V"), InteropEmu.GetKeyCode("W"), InteropEmu.GetKeyCode("X"),
					InteropEmu.GetKeyCode("Y"), InteropEmu.GetKeyCode("Z"), InteropEmu.GetKeyCode("0"), InteropEmu.GetKeyCode("1"),
					InteropEmu.GetKeyCode("2"), InteropEmu.GetKeyCode("3"), InteropEmu.GetKeyCode("4"), InteropEmu.GetKeyCode("5"),
					InteropEmu.GetKeyCode("6"), InteropEmu.GetKeyCode("7"), InteropEmu.GetKeyCode("8"), InteropEmu.GetKeyCode("9"),
					InteropEmu.GetKeyCode("F1"), InteropEmu.GetKeyCode("F2"), InteropEmu.GetKeyCode("F3"), InteropEmu.GetKeyCode("F4"),
					InteropEmu.GetKeyCode("F5"), InteropEmu.GetKeyCode("F6"), InteropEmu.GetKeyCode("F7"), InteropEmu.GetKeyCode("F8"),
					InteropEmu.GetKeyCode("F9"), InteropEmu.GetKeyCode("F10"), InteropEmu.GetKeyCode("F11"), InteropEmu.GetKeyCode("F12"),

					InteropEmu.GetKeyCode("Numpad 0"), InteropEmu.GetKeyCode("Numpad 1"), InteropEmu.GetKeyCode("Numpad 2"), InteropEmu.GetKeyCode("Numpad 3"),
					InteropEmu.GetKeyCode("Numpad 4"), InteropEmu.GetKeyCode("Numpad 5"), InteropEmu.GetKeyCode("Numpad 6"), InteropEmu.GetKeyCode("Numpad 7"),
					InteropEmu.GetKeyCode("Numpad 8"), InteropEmu.GetKeyCode("Numpad 9"),

					InteropEmu.GetKeyCode("Numpad Enter"), InteropEmu.GetKeyCode("Numpad ."),
					InteropEmu.GetKeyCode("Numpad +"), InteropEmu.GetKeyCode("Numpad *"),
					InteropEmu.GetKeyCode("Numpad /"), InteropEmu.GetKeyCode("Numpad -"),

					InteropEmu.GetKeyCode("Num Lock"),

					InteropEmu.GetKeyCode(","), InteropEmu.GetKeyCode("."), InteropEmu.GetKeyCode(";"), InteropEmu.GetKeyCode("'"),
					InteropEmu.GetKeyCode("/"), InteropEmu.GetKeyCode("\\"),
					InteropEmu.GetKeyCode("="), InteropEmu.GetKeyCode("-"), InteropEmu.GetKeyCode("`"),

					InteropEmu.GetKeyCode("["), InteropEmu.GetKeyCode("]"),

					InteropEmu.GetKeyCode("Caps Lock"), InteropEmu.GetKeyCode("Pause"),

					InteropEmu.GetKeyCode("Ctrl"), InteropEmu.GetKeyCode("Shift"), InteropEmu.GetKeyCode("Alt"),

					InteropEmu.GetKeyCode("Spacebar"), InteropEmu.GetKeyCode("Backspace"), InteropEmu.GetKeyCode("Tab"), InteropEmu.GetKeyCode("Esc"), InteropEmu.GetKeyCode("Enter"),

					InteropEmu.GetKeyCode("End"), InteropEmu.GetKeyCode("Home"),
					InteropEmu.GetKeyCode("Insert"), InteropEmu.GetKeyCode("Delete"),

					InteropEmu.GetKeyCode("Page Up"), InteropEmu.GetKeyCode("Page Down"),

					InteropEmu.GetKeyCode("Up Arrow"), InteropEmu.GetKeyCode("Down Arrow"), InteropEmu.GetKeyCode("Left Arrow"), InteropEmu.GetKeyCode("Right Arrow"),

					0,0,0
				}
			};
		}
	}
}
