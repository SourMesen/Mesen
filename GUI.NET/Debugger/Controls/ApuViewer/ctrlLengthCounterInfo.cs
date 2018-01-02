using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger.Controls.ApuViewer
{
	public partial class ctrlLengthCounterInfo : UserControl
	{
		public ctrlLengthCounterInfo()
		{
			InitializeComponent();
		}
		
		public void ProcessState(ref ApuLengthCounterState state)
		{
			chkHalt.Checked = state.Halt;
			txtCounter.Text = state.Counter.ToString();
			txtReloadValue.Text = state.ReloadValue.ToString();
		}
	}
}
