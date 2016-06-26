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
using Mesen.GUI.Config;

namespace Mesen.GUI.Controls
{
	public partial class ctrlNsfPlayer : UserControl
	{
		private List<ComboboxItem> _trackList = new List<ComboboxItem>();
		private int _frameCount = 0;
		private bool _fastForwarding = false;
		private UInt32 _originalSpeed = 100;

		public ctrlNsfPlayer()
		{
			InitializeComponent();
		}

		public void ResetCount()
		{
			_frameCount = 0;
			this.BeginInvoke((MethodInvoker)(() => this.UpdateTimeDisplay(_frameCount)));
		}

		public void CountFrame()
		{
			_frameCount++;
			if(_frameCount % 30 == 0) {
				this.BeginInvoke((MethodInvoker)(() => this.UpdateTimeDisplay(_frameCount)));
			}
		}

		private void UpdateTimeDisplay(int frameCount)
		{
			if(!InteropEmu.IsNsf()) {
				_frameCount = 0;
				return;
			}

			NsfHeader header = InteropEmu.NsfGetHeader();
			int currentTrack = InteropEmu.NsfGetCurrentTrack();

			TimeSpan time = TimeSpan.FromSeconds((double)frameCount / ((header.Flags & 0x01) == 0x01 ? 50.006978 : 60.098812));
			string label = time.ToString(time.TotalHours < 1 ? @"mm\:ss" : @"hh\:mm\:ss");

			TimeSpan trackTime = GetTrackLength(header, currentTrack);
			if(trackTime.Ticks > 0) {
				label += " / " + trackTime.ToString(trackTime.TotalHours < 1 ? @"mm\:ss" : @"hh\:mm\:ss");
			}

			string[] trackNames = header.GetTrackNames();
			if(trackNames.Length > 1 && trackNames.Length > currentTrack) {
				label += Environment.NewLine + (string.IsNullOrWhiteSpace(trackNames[currentTrack]) ? ResourceHelper.GetMessage("NsfUnnamedTrack") : trackNames[currentTrack]);
			}

			lblTime.Text = label;
		}

		private TimeSpan GetTrackLength(NsfHeader header, int track)
		{
			int trackLength = header.TrackLength[track];
			if(header.TotalSongs > 1 && trackLength < 0 && ConfigManager.Config.PreferenceInfo.NsfMoveToNextTrackAfterTime) {
				trackLength = (ConfigManager.Config.PreferenceInfo.NsfMoveToNextTrackTime - 1) * 1000;
			}

			if(trackLength >= 0) {
				int trackFade = header.TrackFade[track];
				if(trackFade < 0) {
					//1 sec by default
					trackFade = 1000;
				}
				trackLength += trackFade;

				return TimeSpan.FromSeconds((double)trackLength / 1000);
			}

			return TimeSpan.FromSeconds(0);
		}

		private void UpdateTrackDisplay()
		{
			NsfHeader header = InteropEmu.NsfGetHeader();
			int currentTrack = InteropEmu.NsfGetCurrentTrack();

			string[] trackNames = header.GetTrackNames();

			if(header.TotalSongs != cboTrack.Items.Count) {
				_trackList = new List<ComboboxItem>();
				for(int i = 0; i < header.TotalSongs; i++) {
					string trackName = (i + 1).ToString();
					if(trackNames.Length > 1 && trackNames.Length > i) {
						trackName += " - " + (string.IsNullOrWhiteSpace(trackNames[i]) ? ResourceHelper.GetMessage("NsfUnnamedTrack") : trackNames[i]);
					}
					TimeSpan trackTime = GetTrackLength(header, i);
					if(trackTime.Ticks > 0) {
						trackName += " (" + trackTime.ToString(trackTime.TotalHours < 1 ? @"mm\:ss" : @"hh\:mm\:ss") + ")";
					}
					_trackList.Add(new ComboboxItem { Value = i +1, Description = trackName });
				}
				cboTrack.DataSource = _trackList;
				cboTrack.DisplayMember = "Value";
			}
			cboTrack.SelectedIndex = currentTrack;
			lblTrackTotal.Text = "/ " + header.TotalSongs.ToString();
		}

		public void UpdateText()
		{
			if(this.InvokeRequired) {
				this.BeginInvoke((MethodInvoker)(() => UpdateText()));
			} else {
				if(InteropEmu.IsNsf()) {
					UpdateTrackDisplay();

					toolTip.SetToolTip(btnNext, ResourceHelper.GetMessage("NsfNextTrack"));

					NsfHeader header = InteropEmu.NsfGetHeader();
					trkVolume.Value = (int)ConfigManager.Config.AudioInfo.MasterVolume;

					lblTitleValue.Text = header.GetSongName();
					lblArtistValue.Text = header.GetArtistName();
					lblCopyrightValue.Text = header.GetCopyrightHolder();

					lblVrc6.ForeColor = (header.SoundChips & 0x01) == 0x01 ? Color.White : Color.Gray;
					lblVrc7.ForeColor = (header.SoundChips & 0x02) == 0x02 ? Color.White : Color.Gray;
					lblFds.ForeColor = (header.SoundChips & 0x04) == 0x04 ? Color.White : Color.Gray;
					lblMmc5.ForeColor = (header.SoundChips & 0x08) == 0x08 ? Color.White : Color.Gray;
					lblNamco.ForeColor = (header.SoundChips & 0x10) == 0x10 ? Color.White : Color.Gray;
					lblSunsoft.ForeColor = (header.SoundChips & 0x20) == 0x20 ? Color.White : Color.Gray;

					if(InteropEmu.IsPaused()) {
						btnPause.Image = Properties.Resources.Play;
					} else {
						btnPause.Image = Properties.Resources.Pause;
					}
				}
			}
		}

		private void btnPause_Click(object sender, EventArgs e)
		{
			if(InteropEmu.IsPaused()) {
				InteropEmu.Resume();
				btnPause.Image = Properties.Resources.Pause;
			} else {
				InteropEmu.Pause();
				btnPause.Image = Properties.Resources.Play;
			}
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			if(!_fastForwarding) {
				int soundCount = InteropEmu.NsfGetHeader().TotalSongs;
				int currentTrack = InteropEmu.NsfGetCurrentTrack();
				currentTrack = (currentTrack + 1) % soundCount;
				InteropEmu.NsfSelectTrack((byte)currentTrack);
				_frameCount = 0;
			}
		}

		private void btnPrevious_Click(object sender, EventArgs e)
		{
			int soundCount = InteropEmu.NsfGetHeader().TotalSongs;
			int currentTrack = InteropEmu.NsfGetCurrentTrack();
			if(_frameCount < 120) {
				//Reload current track if it has been playing for more than 2 seconds
				currentTrack--;
				if(currentTrack < 0) {
					currentTrack = soundCount - 1;
				}
			}
			InteropEmu.NsfSelectTrack((byte)currentTrack);
			_frameCount = 0;
		}

		private void trkVolume_ValueChanged(object sender, EventArgs e)
		{
			ConfigManager.Config.AudioInfo.MasterVolume = (uint)trkVolume.Value;
			ConfigManager.ApplyChanges();
			AudioInfo.ApplyConfig();
		}

		private void cboTrack_SelectedIndexChanged(object sender, EventArgs e)
		{
			int currentTrack = InteropEmu.NsfGetCurrentTrack();
			if(currentTrack != cboTrack.SelectedIndex) {
				InteropEmu.NsfSelectTrack((byte)cboTrack.SelectedIndex);
				_frameCount = 0;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == Keys.Left) {
				btnPrevious_Click(null, null);
				return true;
			} else if(keyData == Keys.Right) {
				btnNext_Click(null, null);
				return true;
			} else if(keyData == Keys.Up) {
				trkVolume.Value = Math.Min(trkVolume.Value+5, 100);
				return true;
			} else if(keyData == Keys.Down) {
				trkVolume.Value = Math.Max(trkVolume.Value-5, 0);
				return true;
			} else if(keyData == Keys.Space) {
				btnPause_Click(null, null);
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void ctrlNsfPlayer_VisibleChanged(object sender, EventArgs e)
		{
			btnPause.Focus();
		}

		private void btnNext_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				tmrFastForward.Start();
				_originalSpeed = ConfigManager.Config.EmulationInfo.EmulationSpeed;
			}
		}

		private void tmrFastForward_Tick(object sender, EventArgs e)
		{
			if(Control.MouseButtons.HasFlag(MouseButtons.Left)) {
				if(!_fastForwarding) {
					tmrFastForward.Interval = 50;
					_fastForwarding = true;
					ConfigManager.Config.EmulationInfo.EmulationSpeed = 0;
					ConfigManager.ApplyChanges();
					EmulationInfo.ApplyConfig();
				}
			} else {
				tmrFastForward.Interval = 500;
				tmrFastForward.Stop();
				ConfigManager.Config.EmulationInfo.EmulationSpeed = _originalSpeed;
				ConfigManager.ApplyChanges();
				EmulationInfo.ApplyConfig();
				_fastForwarding = false;
			}
		}

		private void cboTrack_DropDown(object sender, EventArgs e)
		{
			cboTrack.DisplayMember = "Description";
			int scrollBarWidth = (cboTrack.Items.Count>cboTrack.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;

			int width = 100;
			using(Graphics g = cboTrack.CreateGraphics()) {
				foreach(ComboboxItem item in ((ComboBox)sender).Items) {
					width = Math.Max(width, (int)g.MeasureString(item.Description, cboTrack.Font).Width + scrollBarWidth);
				}
			}
			cboTrack.DropDownWidth = Math.Min(width, 300);
		}

		private void cboTrack_DropDownClosed(object sender, EventArgs e)
		{
			int index = cboTrack.SelectedIndex;
			cboTrack.DisplayMember = "Value";
			cboTrack.SelectedIndex = index;
			btnPause.Focus();
		}
	}

	public class ComboboxItem
	{
		public int Value { get; set; }
		public string Description { get; set; }
	}
}
