using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	public class CodeInfo
	{
		public int[] LineNumbers { get; private set; }
		public string[] LineNumberNotes { get; private set; }
		public char[] LineMemoryType { get; private set; }
		public int[] AbsoluteLineNumbers { get; private set; }
		public string[] CodeNotes { get; private set; }
		public string[] CodeLines { get; private set; }
		public HashSet<int> UnexecutedAddresses { get; private set; }
		public HashSet<int> VerifiedDataAddresses { get; private set; }
		public HashSet<int> SpeculativeCodeAddreses { get; private set; }
		public Dictionary<int, string> CodeContent { get; private set; }
		public Dictionary<int, string> CodeByteCode { get; private set; }
		public string[] Addressing { get; private set; }
		public string[] Comments { get; private set; }
		public int[] LineIndentations { get; private set; }

		public CodeInfo(string code)
		{
			string[] token = code.Split('\x1');

			int lineCount = token.Length / 8;
			LineNumbers = new int[lineCount];
			LineNumberNotes = new string[lineCount];
			LineMemoryType = new char[lineCount];
			AbsoluteLineNumbers = new int[lineCount];
			CodeNotes = new string[lineCount];
			CodeLines = new string[lineCount];
			Addressing = new string[lineCount];
			Comments = new string[lineCount];
			LineIndentations = new int[lineCount];

			UnexecutedAddresses = new HashSet<int>();
			VerifiedDataAddresses = new HashSet<int>();
			SpeculativeCodeAddreses = new HashSet<int>();
			
			int tokenIndex = 0;
			int lineNumber = 0;

			while(tokenIndex < token.Length - 8) {
				int relativeAddress = ParseHexAddress(token[tokenIndex + 1]);

				//Flags:
				//1: Executed code
				//2: Speculative Code
				//4: Indented line
				switch(token[tokenIndex][0]) {
					case '2':
						SpeculativeCodeAddreses.Add(lineNumber);
						LineIndentations[lineNumber] = 0;
						break;

					case '4':
						UnexecutedAddresses.Add(lineNumber);
						LineIndentations[lineNumber] = 20;
						break;

					case '6':
						SpeculativeCodeAddreses.Add(lineNumber);
						LineIndentations[lineNumber] = 20;
						break;

					case '5':
						LineIndentations[lineNumber] = 20;
						break;

					case '8':
						VerifiedDataAddresses.Add(lineNumber);
						LineIndentations[lineNumber] = 0;
						break;

					case '9':
						VerifiedDataAddresses.Add(lineNumber);
						LineIndentations[lineNumber] = 20;
						break;

					default:
						LineIndentations[lineNumber] = 0;
						break;
				}

				LineNumbers[lineNumber] = relativeAddress;
				LineMemoryType[lineNumber] = token[tokenIndex + 2][0];
				LineNumberNotes[lineNumber] = token[tokenIndex + 3];
				AbsoluteLineNumbers[lineNumber] = this.ParseHexAddress(token[tokenIndex + 3]);
				CodeNotes[lineNumber] = token[tokenIndex + 4];
				CodeLines[lineNumber] = token[tokenIndex + 5];

				Addressing[lineNumber] = token[tokenIndex + 6];
				Comments[lineNumber] = token[tokenIndex + 7];


				lineNumber++;
				tokenIndex += 8;
			}
		}

		public void InitAssemblerValues()
		{
			if(CodeContent == null) {
				CodeContent = new Dictionary<int, string>(LineNumbers.Length);
				CodeByteCode = new Dictionary<int, string>(LineNumbers.Length);

				for(int i = 0; i < LineNumbers.Length; i++) {
					//Used by assembler
					int relativeAddress = LineNumbers[i];
					CodeByteCode[relativeAddress] = CodeNotes[i];
					CodeContent[relativeAddress] = CodeLines[i];
				}
			}
		}

		private int ParseHexAddress(string hexAddress)
		{
			if(string.IsNullOrWhiteSpace(hexAddress)) {
				return -1;
			} else {
				return HexConverter.FromHex(hexAddress);
			}
		}
	}
}
