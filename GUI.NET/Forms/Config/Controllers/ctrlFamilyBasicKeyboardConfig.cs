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
	public partial class ctrlFamilyBasicKeyboardConfig : BaseInputConfigControl
	{
		private List<Button> _keyIndexes;

		public ctrlFamilyBasicKeyboardConfig()
		{
			InitializeComponent();

			_keyIndexes = new List<Button>() {
				btnA, btnB, btnC, btnD, btnE, btnF, btnG, btnH, btnI, btnJ, btnK, btnL, btnM, btnN, btnO, btnP, btnQ, btnR, btnS, btnT, btnU, btnV, btnW, btnX, btnY, btnZ,
				btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9,
				btnReturn, btnSpace, btnDel, btnIns, btnEsc,
				btnCtr, btnRightShift, btnLeftShift,
				btnRightBracket, btnLeftBracket,
				btnUp, btnDown, btnLeft, btnRight,
				btnDot, btnComma, btnColon, btnSemiColon, btnUnderscore, btnSlash, btnMinus, btnCaret,
				btnF1, btnF2, btnF3, btnF4, btnF5, btnF6, btnF7, btnF8,
				btnYen, btnStop, btnAt, btnGrph, btnHome, btnKana
			};
		}

		public override void Initialize(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				InitButton(_keyIndexes[i], mappings.FamilyBasicKeyboardButtons[i]);
			}
		}
		
		public override void UpdateKeyMappings(KeyMappings mappings)
		{
			for(int i = 0; i < _keyIndexes.Count; i++) {
				mappings.FamilyBasicKeyboardButtons[i] = (UInt32)_keyIndexes[i].Tag;
			}
		}
	}
}
