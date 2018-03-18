using Mesen.GUI.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Debugger.Controls
{
	public delegate void SetNextStatementEventHandler(AddressEventArgs args);
	public delegate void ShowInSplitViewEventHandler(ICodeViewer sender, AddressEventArgs args);
	public delegate void SwitchToSourceEventHandler(ICodeViewer sender);

	public interface ICodeViewer
	{
		void ScrollToLineNumber(int lineNumber, bool scrollToTop = false);
		void ScrollToAddress(AddressTypeInfo addressInfo, bool scrollToTop = false);
		void SetConfig(DebugViewInfo config);
		void EditSubroutine();
		void EditSelectedCode();

		CodeViewerActions CodeViewerActions { get; }
		ctrlScrollableTextbox CodeViewer { get; }
		Ld65DbgImporter SymbolProvider { get; set; }

		UInt32? ActiveAddress { get; }

		void FindAllOccurrences(string text, bool matchWholeWord, bool matchCase);
		void SelectActiveAddress(UInt32 activeAddress);
		void ClearActiveAddress();
		AddressTypeInfo GetAddressInfo(int lineIndex);
	}

	public class AddressEventArgs : EventArgs
	{
		public UInt32 Address { get; set; }
	}
}
