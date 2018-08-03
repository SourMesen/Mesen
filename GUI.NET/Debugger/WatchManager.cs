using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	class WatchManager
	{
		public static event EventHandler WatchChanged;
		private static List<string> _watchEntries = new List<string>();
		private static List<WatchValueInfo> _previousValues = new List<WatchValueInfo>();
		private static Regex _arrayWatchRegex = new Regex(@"\[((\$[0-9A-Fa-f]+)|(\d+)|([@_a-zA-Z0-9]+))\s*,\s*(\d+)\]", RegexOptions.Compiled);

		public static List<string> WatchEntries
		{
			get { return _watchEntries; }
			set
			{
				_watchEntries = new List<string>(value);
				WatchChanged?.Invoke(null, EventArgs.Empty);
			}
		}

		public static List<WatchValueInfo> GetWatchContent(bool useHex)
		{
			var list = new List<WatchValueInfo>();
			for(int i = 0; i < _watchEntries.Count; i++) {
				string expression = _watchEntries[i];
				string newValue = "";
				EvalResultType resultType;

				bool forceHasChanged = false;
				Match match = _arrayWatchRegex.Match(expression);
				if(match.Success) {
					//Watch expression matches the array display syntax (e.g: [$300,10] = display 10 bytes starting from $300)
					newValue = ProcessArrayDisplaySyntax(useHex, ref forceHasChanged, match);
				} else {
					Int32 result = InteropEmu.DebugEvaluateExpression(expression, out resultType, true);
					switch(resultType) {
						case EvalResultType.Numeric: newValue = useHex ? ("$" + result.ToString("X2")) : result.ToString(); break;
						case EvalResultType.Boolean: newValue = result == 0 ? "false" : "true";	break;
						case EvalResultType.Invalid: newValue = "<invalid expression>"; forceHasChanged = true; break;
						case EvalResultType.DivideBy0: newValue = "<division by zero>"; forceHasChanged = true; break;
					}
				}

				list.Add(new WatchValueInfo() { Expression = expression, Value = newValue, HasChanged = forceHasChanged || (i < _previousValues.Count ? (_previousValues[i].Value != newValue) : false) });
			}

			_previousValues = list;
			return list;
		}

		private static string ProcessArrayDisplaySyntax(bool useHex, ref bool forceHasChanged, Match match)
		{
			string newValue;
			int address;
			if(match.Groups[2].Value.Length > 0) {
				address = int.Parse(match.Groups[2].Value.Substring(1), System.Globalization.NumberStyles.HexNumber);
			} else if(match.Groups[3].Value.Length > 0) {
				address = int.Parse(match.Groups[3].Value);
			} else {
				CodeLabel label = LabelManager.GetLabel(match.Groups[4].Value);
				if(label == null) {
					forceHasChanged = true;
					return "<invalid label>";
				}
				address = label.GetRelativeAddress();
			}
			int elemCount = int.Parse(match.Groups[5].Value);

			if(address >= 0) {
				List<string> values = new List<string>(elemCount);
				for(int j = address, end = address + elemCount; j < end; j++) {
					int memValue = InteropEmu.DebugGetMemoryValue(DebugMemoryType.CpuMemory, (uint)j);
					values.Add(useHex ? memValue.ToString("X2") : memValue.ToString());
				}
				newValue = string.Join(" ", values);
			} else {
				newValue = "<label out of scope>";
				forceHasChanged = true;
			}

			return newValue;
		}

		public static void AddWatch(params string[] expressions)
		{
			foreach(string expression in expressions) {
				_watchEntries.Add(expression);
			}
			WatchChanged?.Invoke(null, EventArgs.Empty);
		}

		public static void UpdateWatch(int index, string expression)
		{
			if(string.IsNullOrWhiteSpace(expression)) {
				RemoveWatch(index);
			} else {
				if(index >= _watchEntries.Count) {
					_watchEntries.Add(expression);
				} else {
					_watchEntries[index] = expression;
				}
				WatchChanged?.Invoke(null, EventArgs.Empty);
			}
		}

		public static void RemoveWatch(params int[] indexes)
		{
			HashSet<int> set = new HashSet<int>(indexes);
			_watchEntries = _watchEntries.Where((el, index) => !set.Contains(index)).ToList();
			_previousValues = _previousValues.Where((el, index) => !set.Contains(index)).ToList();
			WatchChanged?.Invoke(null, EventArgs.Empty);
		}
	}

	public class WatchValueInfo
	{
		public string Expression { get; set; }
		public string Value { get; set; }
		public bool HasChanged { get; set; }
	}
}
