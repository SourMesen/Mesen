using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	public partial class ctrlSuborKeyboardConfig : BaseInputConfigControl
	{
		private List<Button> _keyIndexes;

		public ctrlSuborKeyboardConfig()
		{
			InitializeComponent();

			_keyIndexes = new List<Button>() {
				btnA, btnB, btnC, btnD, btnE, btnF, btnG, btnH, btnI, btnJ, btnK, btnL, btnM, btnN, btnO, btnP, btnQ, btnR, btnS, btnT, btnU, btnV, btnW, btnX, btnY, btnZ,
				btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
				btnF1, btnF2, btnF3, btnF4, btnF5, btnF6, btnF7, btnF8, btnF9, btnF10, btnF11, btnF12,
				btnNumpad0, btnNumpad1, btnNumpad2, btnNumpad3, btnNumpad4, btnNumpad5, btnNumpad6, btnNumpad7, btnNumpad8, btnNumpad9,
				btnNumpadEnter, btnNumpadDot, btnNumpadPlus, btnNumpadMultiply, btnNumpadDivide, btnNumpadMinus, btnNumLock,
				btnComma, btnDot, btnSemiColon, btnApostrophe,
				btnSlash, btnBackslash,
				btnEqual, btnMinus, btnGrave,
				btnLeftBracket, btnRightBracket,
				btnCapsLock, btnPause,
				btnCtrl, btnShift, btnAlt,
				btnSpace, btnBackspace, btnTab, btnEsc, btnEnter,
				btnEnd, btnHome,
				btnIns, btnDelete,
				btnPageUp, btnPageDown,
				btnUp, btnDown, btnLeft, btnRight,
				//btnUnknown1, btnUnknown2, btnUnknown3
			};
		}

		public override void Initialize(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				InitButton(_keyIndexes[i], mappings.SuborKeyboardButtons[i]);
			}
		}
		
		public override void UpdateKeyMappings(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				mappings.SuborKeyboardButtons[i] = (UInt32)_keyIndexes[i].Tag;
			}
		}
	}
}
