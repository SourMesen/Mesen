using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmBreakpoint : BaseConfigForm
	{
		public frmBreakpoint(Breakpoint breakpoint)
		{
			InitializeComponent();

			Entity = breakpoint;

			radTypeExecute.Tag = BreakpointType.Execute;
			radTypeRead.Tag = BreakpointType.Read;
			radTypeWrite.Tag = BreakpointType.Write;

			AddBinding("Enabled", chkEnabled);
			AddBinding("Address", txtAddress);
			AddBinding("Type", radTypeExecute.Parent);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(DialogResult == System.Windows.Forms.DialogResult.OK) {
				((Breakpoint)Entity).Remove();
			}
			base.OnFormClosed(e);
			if(DialogResult == System.Windows.Forms.DialogResult.OK) {
				((Breakpoint)Entity).Add();
			}
		}
	}
}
