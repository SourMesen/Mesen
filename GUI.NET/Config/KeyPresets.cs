namespace Mesen.GUI.Config
{
	public class KeyPresets
	{
		KeyMappings _wasdLayout;
		KeyMappings _arrowLayout;
		KeyMappings _nestopiaLayout;
		KeyMappings _fceuxLayout;
		KeyMappings[] _xboxLayouts = new KeyMappings[2];
		KeyMappings[] _ps4Layouts = new KeyMappings[2];
		KeyMappings[] _snes30Layouts = new KeyMappings[2];

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
				TurboA = InteropEmu.GetKeyCode("Z"), TurboB = InteropEmu.GetKeyCode("X"),
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
	}
}
