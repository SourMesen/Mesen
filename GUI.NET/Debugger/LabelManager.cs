using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger
{
	public class CodeLabel
	{
		public UInt32 Address;
		public string Label;
		public string Comment;
	}

	public class LabelManager
	{
		private static Dictionary<UInt32, CodeLabel> _labels = new Dictionary<uint, CodeLabel>();
		private static Dictionary<string, CodeLabel> _reverseLookup = new Dictionary<string, CodeLabel>();

		public static event EventHandler OnLabelUpdated;

		public static CodeLabel GetLabel(UInt32 address)
		{
			return _labels.ContainsKey(address) ? _labels[address] : null;
		}

		public static CodeLabel GetLabel(string label)
		{
			return _reverseLookup.ContainsKey(label) ? _reverseLookup[label] : null;
		}

		public static Dictionary<UInt32, CodeLabel> GetLabels()
		{
			return _labels;
		}

		public static bool SetLabel(UInt32 address, string label, string comment)
		{
			if(_reverseLookup.ContainsKey(label) && _reverseLookup[label].Address != address) {
				//Label already exists
				return false;
			}

			if(_labels.ContainsKey(address)) {
				_reverseLookup.Remove(_labels[address].Label);
			}

			_labels[address] = new CodeLabel() { Address = address, Label = label, Comment = comment };
			if(label.Length > 0) {
				_reverseLookup[label] = _labels[address];
			}

			InteropEmu.DebugSetLabel(address, label, comment);
			OnLabelUpdated?.Invoke(null, null);

			return true;
		}

		public static void DeleteLabel(UInt32 address)
		{
			if(_labels.ContainsKey(address)) {
				_reverseLookup.Remove(_labels[address].Label);
			}
			if(_labels.Remove(address)) {
				InteropEmu.DebugSetLabel(address, string.Empty, string.Empty);
				OnLabelUpdated?.Invoke(null, null);
			}
		}
	}
}
