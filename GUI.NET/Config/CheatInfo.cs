using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public enum CheatType
	{
		GameGenie = 0,
		ProActionRocky = 1,
		Custom = 2
	}

	public class CheatInfo
	{
		public bool Enabled;
		public string CheatName;
		public string GameName;
		public string GameHash;
		public CheatType CheatType;
		public string Code;
		public UInt32 Address;
		public Byte Value;
		public Byte CompareValue;
		public bool IsRelativeAddress;

		public override string ToString()
		{
			if(CheatType == CheatType.Custom) {
				return !IsRelativeAddress ? "!" : "" + string.Format("{0:X4}:{1:X2}:{2:X2}", Address, Value, CompareValue);
			} else {
				return Code;
			}
		}
	}
}
