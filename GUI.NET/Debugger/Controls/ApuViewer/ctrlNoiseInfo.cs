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
	public partial class ctrlNoiseInfo : UserControl
	{
		public ctrlNoiseInfo()
		{
			InitializeComponent();
		}

		public void ProcessState(ref ApuNoiseState state)
		{
			chkEnabled.Checked = state.Enabled;
			txtPeriod.Text = state.Period.ToString();
			txtTimer.Text = state.Timer.ToString();
			txtFrequency.Text = ((int)(state.Frequency)).ToString();
			txtShiftRegister.Text = state.ShiftRegister.ToString("X4");
			txtOutputVolume.Text = state.OutputVolume.ToString();
			
			ctrlLengthCounterInfo.ProcessState(ref state.LengthCounter);
			ctrlEnvelopeInfo.ProcessState(ref state.Envelope);
		}
	}
}
