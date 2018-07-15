using Mesen.GUI.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	public partial class frmHistoryViewer : BaseForm
	{
		private Thread _emuThread;
		private bool _paused = true;

		public frmHistoryViewer()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(ConfigManager.Config.HistoryViewerInfo.WindowSize.HasValue) {
				this.Size = ConfigManager.Config.HistoryViewerInfo.WindowSize.Value;
			}
			if(ConfigManager.Config.HistoryViewerInfo.WindowLocation.HasValue) {
				this.StartPosition = FormStartPosition.Manual;
				this.Location = ConfigManager.Config.HistoryViewerInfo.WindowLocation.Value;
			}

			trkVolume.Value = ConfigManager.Config.HistoryViewerInfo.Volume;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			ConfigManager.Config.HistoryViewerInfo.WindowLocation = this.WindowState == FormWindowState.Normal ? this.Location : this.RestoreBounds.Location;
			ConfigManager.Config.HistoryViewerInfo.WindowSize = this.WindowState == FormWindowState.Normal ? this.Size : this.RestoreBounds.Size;
			ConfigManager.Config.HistoryViewerInfo.Volume = trkVolume.Value;
			ConfigManager.ApplyChanges();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			InteropEmu.HistoryViewerInitialize(this.Handle, ctrlRenderer.Handle);
			trkPosition.Maximum = (int)(InteropEmu.HistoryViewerGetHistoryLength() / 60);
			UpdatePositionLabel(0);
			StartEmuThread();
			InteropEmu.Resume(InteropEmu.ConsoleId.HistoryViewer);
			tmrUpdatePosition.Start();

			btnPausePlay.Focus();

			UpdateScale();
			this.Resize += (s, evt) => {
				UpdateScale();
			};
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			tmrUpdatePosition.Stop();
			InteropEmu.HistoryViewerRelease();
			base.OnClosing(e);
		}

		private void StartEmuThread()
		{
			if(_emuThread == null) {
				_emuThread = new Thread(() => {
					try {
						InteropEmu.HistoryViewerRun();
						_emuThread = null;
					} catch(Exception ex) {
						MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
						_emuThread = null;
					}
				});
				_emuThread.Start();
			}
		}

		private void TogglePause()
		{
			if(trkPosition.Value == trkPosition.Maximum) {
				InteropEmu.HistoryViewerSetPosition(0);
			}
			if(_paused) {
				InteropEmu.Resume(InteropEmu.ConsoleId.HistoryViewer);
			} else {
				InteropEmu.Pause(InteropEmu.ConsoleId.HistoryViewer);
			}
		}

		private void trkPosition_ValueChanged(object sender, EventArgs e)
		{
			InteropEmu.HistoryViewerSetPosition((UInt32)trkPosition.Value * 2);
		}

		private void UpdateScale()
		{
			Size dimensions = pnlRenderer.ClientSize;
			InteropEmu.ScreenSize size = InteropEmu.GetScreenSize(true, InteropEmu.ConsoleId.HistoryViewer);

			double verticalScale = (double)dimensions.Height / size.Height;
			double horizontalScale = (double)dimensions.Width / size.Width;
			double scale = Math.Min(verticalScale, horizontalScale);
			InteropEmu.SetVideoScale(scale, InteropEmu.ConsoleId.HistoryViewer);
		}

		private void tmrUpdatePosition_Tick(object sender, EventArgs e)
		{
			InteropEmu.ScreenSize size = InteropEmu.GetScreenSize(false, InteropEmu.ConsoleId.HistoryViewer);
			if(size.Width != ctrlRenderer.ClientSize.Width || size.Height != ctrlRenderer.ClientSize.Height) {
				//MessageBox.Show(size.Height.ToString() + " vs " + ctrlRenderer.ClientSize.Height.ToString());
				ctrlRenderer.ClientSize = new Size(size.Width, size.Height);
			}

			_paused = InteropEmu.IsPaused(InteropEmu.ConsoleId.HistoryViewer);
			if(_paused) {
				btnPausePlay.Image = Properties.Resources.Play;
			} else {
				btnPausePlay.Image = Properties.Resources.Pause;
			}

			UInt32 positionInSeconds = InteropEmu.HistoryViewerGetPosition() / 2;
			UpdatePositionLabel(positionInSeconds);

			if(positionInSeconds <= trkPosition.Maximum) {
				trkPosition.ValueChanged -= trkPosition_ValueChanged;
				trkPosition.Value = (int)positionInSeconds;
				trkPosition.ValueChanged += trkPosition_ValueChanged;
			}
		}

		private void UpdatePositionLabel(uint positionInSeconds)
		{
			TimeSpan currentPosition = new TimeSpan(0, 0, (int)positionInSeconds);
			TimeSpan totalLength = new TimeSpan(0, 0, trkPosition.Maximum);
			lblPosition.Text = (
				currentPosition.Minutes.ToString("00") + ":" + currentPosition.Seconds.ToString("00")
				+ " / " +
				totalLength.Minutes.ToString("00") + ":" + totalLength.Seconds.ToString("00")
			);
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void mnuResumeGameplay_Click(object sender, EventArgs e)
		{
			InteropEmu.HistoryViewerResumeGameplay(InteropEmu.HistoryViewerGetPosition());
		}
		
		private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			mnuExportMovie.DropDownItems.Clear();

			List<UInt32> segments = new List<UInt32>(InteropEmu.HistoryViewerGetSegments());
			UInt32 segmentStart = 0;
			segments.Add(InteropEmu.HistoryViewerGetHistoryLength() / 30);

			for(int i = 0; i < segments.Count; i++) {
				if(segments[i] - segmentStart > 4) {
					//Only list segments that are at least 2 seconds long
					UInt32 segStart = segmentStart;
					UInt32 segEnd = segments[i];
					TimeSpan start = new TimeSpan(0, 0, (int)(segmentStart) / 2);
					TimeSpan end = new TimeSpan(0, 0, (int)(segEnd / 2));

					ToolStripMenuItem item = new ToolStripMenuItem("Segment #" + (mnuExportMovie.DropDownItems.Count + 1).ToString() + ", " + start.ToString() + " - " + end.ToString());
					item.Click += (s, evt) => {
						SaveFileDialog sfd = new SaveFileDialog();
						sfd.SetFilter(ResourceHelper.GetMessage("FilterMovie"));
						sfd.InitialDirectory = ConfigManager.MovieFolder;
						sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".mmo";
						if(sfd.ShowDialog() == DialogResult.OK) {
							if(!InteropEmu.HistoryViewerSaveMovie(sfd.FileName, segStart, segEnd)) {
								MessageBox.Show("An error occurred while trying to save the movie file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					};
					mnuExportMovie.DropDownItems.Add(item);
				}
				segmentStart = segments[i] + 1;
			}

			mnuImportMovie.Visible = false;
			mnuExportMovie.Enabled = mnuExportMovie.HasDropDownItems;
		}

		private void btnPausePlay_Click(object sender, EventArgs e)
		{
			TogglePause();
		}

		private void ctrlRenderer_MouseClick(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				TogglePause();
			}
		}

		private void trkVolume_ValueChanged(object sender, EventArgs e)
		{
			InteropEmu.SetMasterVolume(trkVolume.Value / 10d, 0, InteropEmu.ConsoleId.HistoryViewer);
		}
	}
}
