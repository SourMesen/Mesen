using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mesen.GUI.Debugger.TblLoader;

namespace Be.Windows.Forms
{
	/// <summary>
	/// The default <see cref="IByteCharConverter"/> implementation.
	/// </summary>
	class TblByteCharConverter : IByteCharConverter
	{
		Dictionary<TblKey, string> _tblRules;

		public TblByteCharConverter(Dictionary<TblKey, string> tblRules)
		{
			this._tblRules = tblRules;
		}

		/// <summary>
		/// Returns the character to display for the byte passed across.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public virtual string ToChar(UInt64 value, out int keyLength)
		{
			int byteCount = 8;
			string result;
			while(!_tblRules.TryGetValue(new TblKey() { Key = value, Length = byteCount }, out result) && byteCount > 0) {
				value &= ~(((UInt64)0xFF) << (8 * (byteCount - 1)));
				byteCount--;
			}

			if(result != null) {
				keyLength = byteCount;
				return result;
			} else {
				keyLength = 1;
				return ".";
			}
		}

		/// <summary>
		/// Returns the byte to use for the character passed across.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public virtual byte ToByte(char c)
		{
			return (byte)c;
		}

		/// <summary>
		/// Returns a description of the byte char provider.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "TBL";
		}
	}
}
