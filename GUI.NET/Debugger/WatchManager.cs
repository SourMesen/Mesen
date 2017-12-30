using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	class WatchManager
	{
		public static event EventHandler WatchChanged;
		private static List<string> _watchEntries = new List<string>();
		private static List<WatchValueInfo> _previousValues = new List<WatchValueInfo>();

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
				Int32 result = InteropEmu.DebugEvaluateExpression(expression, out resultType);
				bool forceHasChanged = false;
				switch(resultType) {
					case EvalResultType.Numeric: newValue = useHex ? ("$" + result.ToString("X2")) : result.ToString(); break;
					case EvalResultType.Boolean: newValue = result == 0 ? "false" : "true";	break;
					case EvalResultType.Invalid: newValue = "<invalid expression>"; forceHasChanged = true; break;
					case EvalResultType.DivideBy0: newValue = "<division by zero>"; forceHasChanged = true; break;
				}

				list.Add(new WatchValueInfo() { Expression = expression, Value = newValue, HasChanged = forceHasChanged || (i < _previousValues.Count ? (_previousValues[i].Value != newValue) : false) });
			}

			_previousValues = list;
			return list;
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
