using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Mesen.GUI.Debugger
{
	class ctrlSyncTextBox : RichTextBox
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		private const int WM_MOUSEWHEEL = 0x20A;
		private const int WM_VSCROLL = 0x115;
		private const int WM_USER = 0x0400;
		private const int EM_SETEVENTMASK = (WM_USER + 69);
		private const int WM_SETREDRAW = 0x0b;
		private IntPtr OldEventMask;

		private Timer _syncTimer;
		private RichTextBox _syncedTextbox;

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public RichTextBox SyncedTextbox
		{
			get
			{
				return _syncedTextbox;
			}
			set
			{
				if(_syncedTextbox == null) {
					_syncedTextbox = value;
					if(!DesignMode && value != null && _syncTimer == null) {
						CreateTimer();
					}
				}
			}
		}

		public void BeginUpdate()
		{
			SendMessage(Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
			OldEventMask = (IntPtr)SendMessage(Handle, EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero);
		}

		public void EndUpdate()
		{
			SendMessage(Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
			SendMessage(Handle, EM_SETEVENTMASK, IntPtr.Zero, OldEventMask);
		}

		private void CreateTimer()
		{
			_syncTimer = new Timer();
			_syncTimer.Interval = 50;
			_syncTimer.Enabled = true;
			_syncTimer.Tick += (object sender, EventArgs e) => {
				int line1 = SyncedTextbox.GetLineFromCharIndex(SyncedTextbox.GetCharIndexFromPosition(new Point(0, 0)));
				int line2 = GetLineFromCharIndex(GetCharIndexFromPosition(new Point(0, 0)));
				if(line1 != line2) {
					int selectionStart = SyncedTextbox.GetFirstCharIndexFromLine(line2);
					if(selectionStart >= 0) {
						SyncedTextbox.SelectionStart = selectionStart;
						SyncedTextbox.ScrollToCaret();
					}
				}
			};
			_syncTimer.Start();
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg == WM_VSCROLL && SyncedTextbox != null && SyncedTextbox.IsHandleCreated) {
				SendMessage(SyncedTextbox.Handle, m.Msg, m.WParam, m.LParam);
			}
			if(m.Msg == WM_MOUSEWHEEL) {
				//Disable smooth scrolling by replacing the WM_MOUSEWHEEL event by regular VM_VSCROLL events
				int scrollLines = SystemInformation.MouseWheelScrollLines;
				for(int i = 0; i < scrollLines; i++) {
					if((int)m.WParam > 0) {
						SendMessage(Handle, WM_VSCROLL, (IntPtr)0, IntPtr.Zero);
					} else {
						SendMessage(Handle, WM_VSCROLL, (IntPtr)1, IntPtr.Zero);
					}
				}
				return;
			} else {
				base.WndProc(ref m);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				if(_syncTimer != null) {
					_syncTimer.Stop();
					_syncTimer = null;
				}
			}
			base.Dispose(disposing);
		}
	}
}
