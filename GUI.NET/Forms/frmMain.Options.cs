using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms.Config;

namespace Mesen.GUI.Forms
{
	public partial class frmMain
	{
		private void SetScale(double scale)
		{
			_customSize = false;
			if(this.HideMenuStrip) {
				this.menuStrip.Visible = false;
			}
			InteropEmu.SetVideoScale(scale);
			UpdateScaleMenu(scale);
			UpdateViewerSize();
		}

		private void SetVideoFilter(VideoFilterType type)
		{
			if(!_fullscreenMode) {
				_customSize = false;
			}
			InteropEmu.SetVideoFilter(type);
			UpdateFilterMenu(type);
		}

		private void UpdateScaleMenu(double scale)
		{
			mnuScale1x.Checked = (scale == 1.0);
			mnuScale2x.Checked = (scale == 2.0);
			mnuScale3x.Checked = (scale == 3.0);
			mnuScale4x.Checked = (scale == 4.0);
			mnuScale5x.Checked = (scale == 5.0);
			mnuScale6x.Checked = (scale == 6.0);

			ConfigManager.Config.VideoInfo.VideoScale = scale;
			ConfigManager.ApplyChanges();
		}

		private void UpdateFilterMenu(VideoFilterType filterType)
		{
			mnuNoneFilter.Checked = (filterType == VideoFilterType.None);
			mnuNtscFilter.Checked = (filterType == VideoFilterType.NTSC);
			mnuNtscBisqwitFullFilter.Checked = (filterType == VideoFilterType.BisqwitNtsc);
			mnuNtscBisqwitHalfFilter.Checked = (filterType == VideoFilterType.BisqwitNtscHalfRes);
			mnuNtscBisqwitQuarterFilter.Checked = (filterType == VideoFilterType.BisqwitNtscQuarterRes);

			mnuXBRZ2xFilter.Checked = (filterType == VideoFilterType.xBRZ2x);
			mnuXBRZ3xFilter.Checked = (filterType == VideoFilterType.xBRZ3x);
			mnuXBRZ4xFilter.Checked = (filterType == VideoFilterType.xBRZ4x);
			mnuXBRZ5xFilter.Checked = (filterType == VideoFilterType.xBRZ5x);
			mnuXBRZ6xFilter.Checked = (filterType == VideoFilterType.xBRZ6x);
			mnuHQ2xFilter.Checked = (filterType == VideoFilterType.HQ2x);
			mnuHQ3xFilter.Checked = (filterType == VideoFilterType.HQ3x);
			mnuHQ4xFilter.Checked = (filterType == VideoFilterType.HQ4x);
			mnuScale2xFilter.Checked = (filterType == VideoFilterType.Scale2x);
			mnuScale3xFilter.Checked = (filterType == VideoFilterType.Scale3x);
			mnuScale4xFilter.Checked = (filterType == VideoFilterType.Scale4x);
			mnu2xSaiFilter.Checked = (filterType == VideoFilterType._2xSai);
			mnuSuper2xSaiFilter.Checked = (filterType == VideoFilterType.Super2xSai);
			mnuSuperEagleFilter.Checked = (filterType == VideoFilterType.SuperEagle);
			mnuPrescale2xFilter.Checked = (filterType == VideoFilterType.Prescale2x);
			mnuPrescale3xFilter.Checked = (filterType == VideoFilterType.Prescale3x);
			mnuPrescale4xFilter.Checked = (filterType == VideoFilterType.Prescale4x);
			mnuPrescale6xFilter.Checked = (filterType == VideoFilterType.Prescale6x);
			mnuPrescale8xFilter.Checked = (filterType == VideoFilterType.Prescale8x);
			mnuPrescale10xFilter.Checked = (filterType == VideoFilterType.Prescale10x);

			ConfigManager.Config.VideoInfo.VideoFilter = filterType;
			ConfigManager.ApplyChanges();
		}

		private void mnuVideoConfig_Click(object sender, EventArgs e)
		{
			Size originalSize = this.Size;
			InteropEmu.ScreenSize originalScreenSize = InteropEmu.GetScreenSize(false);
			Configuration configBackup = ConfigManager.Config.Clone();
			bool cancelled = false;
			using(frmVideoConfig frm = new frmVideoConfig()) {
				cancelled = frm.ShowDialog(sender, this) == DialogResult.Cancel;
			}
			if(cancelled) {
				ConfigManager.RevertToBackup(configBackup);
				ConfigManager.Config.ApplyConfig();
			}
			UpdateVideoSettings();
			InteropEmu.ScreenSize screenSize = InteropEmu.GetScreenSize(false);
			if((cancelled || (screenSize.Height == originalScreenSize.Height && screenSize.Width == originalScreenSize.Width)) && this.WindowState == FormWindowState.Normal) {
				this.Size = originalSize;
			}
			if(_fullscreenMode && ConfigManager.Config.VideoInfo.UseExclusiveFullscreen && _frmFullscreenRenderer == null) {
				StopFullscreenWindowMode();
				if(!this._isNsfPlayerMode) {
					SetFullscreenState(true);
				}
			}
		}

		private void mnuInput_Click(object sender, EventArgs e)
		{
			using(frmInputConfig frm = new frmInputConfig()) {
				frm.ShowDialog(sender, this);
			}
		}

		private void mnuAudioConfig_Click(object sender, EventArgs e)
		{
			using(frmAudioConfig frm = new frmAudioConfig()) {
				frm.ShowDialog(sender, this);
			}
			this.ctrlNsfPlayer.UpdateVolume();
		}

		private void mnuPreferences_Click(object sender, EventArgs e)
		{
			using(frmPreferences frm = new frmPreferences()) {
				VsDualOutputOption originalVsDualOutput = ConfigManager.Config.PreferenceInfo.VsDualVideoOutput;

				if(frm.ShowDialog(sender, this) == DialogResult.OK) {
					if(frm.NeedRestart) {
						//Data folder has changed, end process
						ConfigManager.DoNotSaveSettings = true;
						this.Close();
						return;
					}

					VsDualOutputOption newVsDualOutput = ConfigManager.Config.PreferenceInfo.VsDualVideoOutput;
					if(originalVsDualOutput != newVsDualOutput) {
						if(newVsDualOutput == VsDualOutputOption.Both || originalVsDualOutput == VsDualOutputOption.Both) {
							UpdateViewerSize(true);
						} else {
							UpdateDualSystemViewer();
						}
					}
					ResourceHelper.LoadResources(ConfigManager.Config.PreferenceInfo.DisplayLanguage);
					ResourceHelper.UpdateEmuLanguage();
					ResourceHelper.ApplyResources(this);
					UpdateMenus();
					InitializeNsfMode();
					ctrlRecentGames.Visible = _emuThread == null;
					ctrlRecentGames.UpdateGameInfo();
					TopMost = ConfigManager.Config.PreferenceInfo.AlwaysOnTop;
					FormBorderStyle = ConfigManager.Config.PreferenceInfo.DisableMouseResize ? FormBorderStyle.Fixed3D : FormBorderStyle.Sizable;
				} else {
					UpdateVideoSettings();
					UpdateMenus();
					UpdateViewerSize();
				}
			}
			ResizeRecentGames(sender, e);
		}

		private void mnuEmulationConfig_Click(object sender, EventArgs e)
		{
			using(frmEmulationConfig frm = new frmEmulationConfig()) {
				frm.ShowDialog(sender, this);
			}
		}

		private void mnuRegion_Click(object sender, EventArgs e)
		{
			if(sender == mnuRegionAuto) {
				ConfigManager.Config.Region = NesModel.Auto;
			} else if(sender == mnuRegionNtsc) {
				ConfigManager.Config.Region = NesModel.NTSC;
			} else if(sender == mnuRegionPal) {
				ConfigManager.Config.Region = NesModel.PAL;
			} else if(sender == mnuRegionDendy) {
				ConfigManager.Config.Region = NesModel.Dendy;
			}
			InteropEmu.SetNesModel(ConfigManager.Config.Region);
		}
		
		private void mnuShowFPS_Click(object sender, EventArgs e)
		{
			ToggleFps();
		}

		private void UpdateEmulationFlags()
		{
			ConfigManager.Config.VideoInfo.ShowFPS = mnuShowFPS.Checked;
			ConfigManager.ApplyChanges();

			VideoInfo.ApplyConfig();
		}

		private void UpdateVideoSettings()
		{
			mnuShowFPS.Checked = ConfigManager.Config.VideoInfo.ShowFPS;
			mnuBilinearInterpolation.Checked = ConfigManager.Config.VideoInfo.UseBilinearInterpolation;
			UpdateScaleMenu(ConfigManager.Config.VideoInfo.VideoScale);
			UpdateFilterMenu(ConfigManager.Config.VideoInfo.VideoFilter);

			_customSize = false;
			UpdateViewerSize();
		}

		private void InitializeEmulationSpeedMenu()
		{
			mnuEmuSpeedNormal.Tag = 100;
			mnuEmuSpeedTriple.Tag = 300;
			mnuEmuSpeedDouble.Tag = 200;
			mnuEmuSpeedHalf.Tag = 50;
			mnuEmuSpeedQuarter.Tag = 25;
			mnuEmuSpeedMaximumSpeed.Tag = 0;
		}

		private void UpdateEmulationSpeedMenu()
		{
			ConfigManager.Config.EmulationInfo.EmulationSpeed = InteropEmu.GetEmulationSpeed();
			foreach(ToolStripMenuItem item in new ToolStripMenuItem[] { mnuEmuSpeedDouble, mnuEmuSpeedHalf, mnuEmuSpeedNormal, mnuEmuSpeedQuarter, mnuEmuSpeedTriple, mnuEmuSpeedMaximumSpeed }) {
				item.Checked = ((int)item.Tag == ConfigManager.Config.EmulationInfo.EmulationSpeed);
			}
		}

		private void SetEmulationSpeed(uint emulationSpeed)
		{
			ConfigManager.Config.EmulationInfo.EmulationSpeed = emulationSpeed;
			ConfigManager.ApplyChanges();
			EmulationInfo.ApplyConfig();
		}

		private void mnuEmulationSpeed_DropDownOpening(object sender, EventArgs e)
		{
			UpdateEmulationSpeedMenu();
		}

		private void mnuEmulationSpeedOption_Click(object sender, EventArgs e)
		{
			SetEmulationSpeed((uint)(int)((ToolStripItem)sender).Tag);
		}

		private void mnuBilinearInterpolation_Click(object sender, EventArgs e)
		{
			ConfigManager.Config.VideoInfo.UseBilinearInterpolation = mnuBilinearInterpolation.Checked;
			ConfigManager.ApplyChanges();
			VideoInfo.ApplyConfig();
		}

		private void mnuNoneFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.None);
		}

		private void mnuNtscFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.NTSC);
		}

		private void mnuXBRZ2xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.xBRZ2x);
		}

		private void mnuXBRZ3xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.xBRZ3x);
		}

		private void mnuXBRZ4xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.xBRZ4x);
		}

		private void mnuXBRZ5xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.xBRZ5x);
		}

		private void mnuXBRZ6xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.xBRZ6x);
		}

		private void mnuHQ2xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.HQ2x);
		}

		private void mnuHQ3xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.HQ3x);
		}

		private void mnuHQ4xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.HQ4x);
		}

		private void mnuScale2xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Scale2x);
		}

		private void mnuScale3xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Scale3x);
		}

		private void mnuScale4xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Scale4x);
		}

		private void mnu2xSaiFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType._2xSai);
		}

		private void mnuSuper2xSaiFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Super2xSai);
		}

		private void mnuSuperEagleFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.SuperEagle);
		}

		private void mnuPrescale2xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Prescale2x);
		}

		private void mnuPrescale3xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Prescale3x);
		}

		private void mnuPrescale4xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Prescale4x);
		}

		private void mnuPrescale6xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Prescale6x);
		}

		private void mnuPrescale8xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Prescale8x);
		}

		private void mnuPrescale10xFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.Prescale10x);
		}

		private void mnuNtscBisqwitFullFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.BisqwitNtsc);
		}

		private void mnuNtscBisqwitHalfFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.BisqwitNtscHalfRes);
		}

		private void mnuNtscBisqwitQuarterFilter_Click(object sender, EventArgs e)
		{
			SetVideoFilter(VideoFilterType.BisqwitNtscQuarterRes);
		}
	}
}
