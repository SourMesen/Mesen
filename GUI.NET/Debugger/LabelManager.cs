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
		public AddressType AddressType;
		public string Label;
		public string Comment;
	}

	public class LabelManager
	{
		private static Dictionary<string, CodeLabel> _labels = new Dictionary<string, CodeLabel>();
		private static Dictionary<string, CodeLabel> _reverseLookup = new Dictionary<string, CodeLabel>();

		public static event EventHandler OnLabelUpdated;

		public static void ResetLabels()
		{
			_labels.Clear();
			_reverseLookup.Clear();
		}

		public static CodeLabel GetLabel(UInt32 address, AddressType type)
		{
			return _labels.ContainsKey(GetKey(address, type)) ? _labels[GetKey(address, type)] : null;
		}

		public static CodeLabel GetLabel(string label)
		{
			return _reverseLookup.ContainsKey(label) ? _reverseLookup[label] : null;
		}

		public static Dictionary<string, CodeLabel> GetLabels()
		{
			return _labels;
		}

		private static string GetKey(UInt32 address, AddressType addressType)
		{
			return address.ToString() + addressType.ToString();
		}

		public static bool SetLabel(UInt32 address, AddressType type, string label, string comment)
		{
			if(_labels.ContainsKey(GetKey(address, type))) {
				_reverseLookup.Remove(_labels[GetKey(address, type)].Label);
			}

			_labels[GetKey(address, type)] = new CodeLabel() { Address = address, AddressType = type, Label = label, Comment = comment };
			if(label.Length > 0) {
				_reverseLookup[label] = _labels[GetKey(address, type)];
			}

			InteropEmu.DebugSetLabel(address, type, label, comment);
			OnLabelUpdated?.Invoke(null, null);

			return true;
		}

		public static void DeleteLabel(UInt32 address, AddressType type, bool raiseEvent)
		{
			if(_labels.ContainsKey(GetKey(address, type))) {
				_reverseLookup.Remove(_labels[GetKey(address, type)].Label);
			}
			if(_labels.Remove(GetKey(address, type))) {
				InteropEmu.DebugSetLabel(address, type, string.Empty, string.Empty);
				if(raiseEvent) {
					OnLabelUpdated?.Invoke(null, null);
				}
			}
		}
	}
}
