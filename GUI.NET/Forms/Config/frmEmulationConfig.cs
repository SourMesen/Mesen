﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.GoogleDriveIntegration;

namespace Mesen.GUI.Forms.Config
{
	public partial class frmEmulationConfig : BaseConfigForm
	{
		public frmEmulationConfig()
		{
			InitializeComponent();

			tpgOverclocking.Enabled = !InteropEmu.MoviePlaying() && !InteropEmu.MovieRecording();
			tpgAdvanced.Enabled = !InteropEmu.MoviePlaying() && !InteropEmu.MovieRecording();

			ConfigManager.Config.EmulationInfo.EmulationSpeed = InteropEmu.GetEmulationSpeed();
			Entity = ConfigManager.Config.EmulationInfo;

			AddBinding("EmulationSpeed", nudEmulationSpeed);
			AddBinding("TurboSpeed", nudTurboSpeed);
			AddBinding("RewindSpeed", nudRewindSpeed);

			AddBinding("UseAlternativeMmc3Irq", chkUseAlternativeMmc3Irq);
			AddBinding("AllowInvalidInput", chkAllowInvalidInput);
			AddBinding("DisablePpu2004Reads", chkDisablePpu2004Reads);
			AddBinding("DisablePaletteRead", chkDisablePaletteRead);
			AddBinding("DisableOamAddrBug", chkDisableOamAddrBug);
			AddBinding("DisablePpuReset", chkDisablePpuReset);
			AddBinding("EnableOamDecay", chkEnableOamDecay);
			AddBinding("UseNes101Hvc101Behavior", chkUseNes101Hvc101Behavior);
			AddBinding("EnableMapperRandomPowerOnState", chkMapperRandomPowerOnState);

			AddBinding("OverclockRate", nudOverclockRate);
			AddBinding("OverclockAdjustApu", chkOverclockAdjustApu);

			AddBinding("PpuExtraScanlinesBeforeNmi", nudExtraScanlinesBeforeNmi);
			AddBinding("PpuExtraScanlinesAfterNmi", nudExtraScanlinesAfterNmi);

			AddBinding("ShowLagCounter", chkShowLagCounter);
			
			AddBinding("RamPowerOnState", cboRamPowerOnState);		
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			EmulationInfo.ApplyConfig();
		}

		private void tmrUpdateClockRate_Tick(object sender, EventArgs e)
		{
			decimal clockRateMultiplierNtsc = (nudOverclockRate.Value * (1 + (nudExtraScanlinesAfterNmi.Value + nudExtraScanlinesBeforeNmi.Value) / 262));
			decimal clockRateMultiplierPal = (nudOverclockRate.Value * (1 + (nudExtraScanlinesAfterNmi.Value + nudExtraScanlinesBeforeNmi.Value) / 312));
			decimal clockRateMultiplierDendy = (nudOverclockRate.Value * (1 + (nudExtraScanlinesAfterNmi.Value + nudExtraScanlinesBeforeNmi.Value) / 312));
			lblEffectiveClockRateValue.Text = (1789773 * clockRateMultiplierNtsc / 100000000).ToString("#.####") + " mhz (" + ((int)clockRateMultiplierNtsc).ToString() + "%)";
			lblEffectiveClockRateValuePal.Text = (1662607 * clockRateMultiplierPal / 100000000).ToString("#.####") + " mhz (" + ((int)clockRateMultiplierPal).ToString() + "%)";
			lblEffectiveClockRateValueDendy.Text = (1773448 * clockRateMultiplierDendy / 100000000).ToString("#.####") + " mhz (" + ((int)clockRateMultiplierDendy).ToString() + "%)";
		}

		private void btnResetLagCounter_Click(object sender, EventArgs e)
		{
			InteropEmu.ResetLagCounter();
		}
	}
}
