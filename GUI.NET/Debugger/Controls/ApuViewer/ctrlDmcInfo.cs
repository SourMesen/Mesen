using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlDmcInfo : UserControl
	{
		public ctrlDmcInfo()
		{
			InitializeComponent();
		}

		public void ProcessState(ref ApuDmcState state)
		{
			txtPeriod.Text = state.Period.ToString();
			txtTimer.Text = state.Timer.ToString();
			txtOutputVolume.Text = state.OutputVolume.ToString();
			txtSampleRate.Text = ((int)state.SampleRate).ToString();

			txtSampleAddress.Text = state.SampleAddr.ToString("X4");
			txtSampleLength.Text = state.SampleLength.ToString();
			txtBytesRemaining.Text = state.BytesRemaining.ToString();

			chkEnabled.Checked = state.BytesRemaining > 0;
			chkLoop.Checked = state.Loop;
			chkIrqEnabled.Checked = state.IrqEnabled;
		}
	}
}
