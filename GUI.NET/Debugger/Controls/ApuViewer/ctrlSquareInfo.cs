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
	public partial class ctrlSquareInfo : UserControl
	{
		public ctrlSquareInfo()
		{
			InitializeComponent();
		}

		public void ProcessState(ref ApuSquareState state)
		{
			chkEnabled.Checked = state.Enabled;
			txtPeriod.Text = state.Period.ToString();
			txtTimer.Text = state.Timer.ToString();
			txtFrequency.Text = ((int)(state.Frequency)).ToString();
			txtDuty.Text = state.Duty.ToString();
			txtDutyPosition.Text = state.DutyPosition.ToString();
			txtOutputVolume.Text = state.OutputVolume.ToString();

			chkSweepEnabled.Checked = state.SweepEnabled;
			chkSweepNegate.Checked = state.SweepNegate;
			txtSweepPeriod.Text = state.SweepPeriod.ToString();
			txtSweepShift.Text = state.SweepShift.ToString();

			ctrlLengthCounterInfo.ProcessState(ref state.LengthCounter);
			ctrlEnvelopeInfo.ProcessState(ref state.Envelope);
		}
	}
}
