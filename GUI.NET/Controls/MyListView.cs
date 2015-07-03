using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Controls
{
	//Code adapted from:
	//http://blogs.msdn.com/b/hippietim/archive/2006/03/27/562256.aspx
	class MyListView : ListView
	{
		private bool m_doubleClickDoesCheck = true;  //  maintain the default behavior
		private bool m_inDoubleClickCheckHack = false;

		//****************************************************************************************
		// This function helps us overcome the problem with the managed listview wrapper wanting
		// to turn double-clicks on checklist items into checkbox clicks.  We count on the fact
		// that the base handler for NM_DBLCLK will send a hit test request back at us right away.
		// So we set a special flag to return a bogus hit test result in that case.
		//****************************************************************************************
		private void OnWmReflectNotify(ref Message m)
		{
			if(!DoubleClickDoesCheck && CheckBoxes) {
				NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.NMHDR));

				if(nmhdr.code == NativeMethods.NM_DBLCLK) {
					m_inDoubleClickCheckHack = true;
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			switch(m.Msg) {
				//  This code is to hack around the fact that the managed listview
				//  wrapper translates double clicks into checks without giving the 
				//  host to participate.
				//  See OnWmReflectNotify() for more details.
				case NativeMethods.WM_REFLECT + NativeMethods.WM_NOTIFY:
					OnWmReflectNotify(ref m);
					break;

				//  This code checks to see if we have entered our hack check for
				//  double clicking items in check lists.  During the NM_DBLCLK
				//  processing, the managed handler will send a hit test message
				//  to see which item to check.  Returning -1 will convince that
				//  code not to proceed.
				case NativeMethods.LVM_HITTEST:
					if(m_inDoubleClickCheckHack) {
						m_inDoubleClickCheckHack = false;
						m.Result = (System.IntPtr)(-1);
						return;
					}
					break;
			}

			base.WndProc(ref m);
		}

		[Browsable(true),
		 Description("When CheckBoxes is true, this controls whether or not double clicking will toggle the check."),
		 Category("My Controls"),
		 DefaultValue(true)]
		public bool DoubleClickDoesCheck
		{
			get
			{
				return m_doubleClickDoesCheck;
			}

			set
			{
				m_doubleClickDoesCheck = value;
			}
		}
	}

	public class NativeMethods
	{
		public const int WM_USER = 0x0400;
		public const int WM_REFLECT = WM_USER + 0x1C00;
		public const int WM_NOTIFY = 0x004E;
		public const int LVM_HITTEST = (0x1000 + 18);
		public const int NM_DBLCLK = (-3);

		[StructLayout(LayoutKind.Sequential)]
		public struct NMHDR
		{
			public IntPtr hwndFrom;
			public UIntPtr idFrom;
			public int code;
		}
	}

}
