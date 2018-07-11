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

			InteropEmu.InitializeHistoryViewer(this.Handle, ctrlRenderer.Handle);
			trkPosition.Maximum = (int)(InteropEmu.GetHistoryViewerTotalFrameCount() / 60);
			UpdatePositionLabel(0);
			InteropEmu.SetHistoryViewerPauseStatus(true);
			StartEmuThread();
			tmrUpdatePosition.Start();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			tmrUpdatePosition.Stop();
			InteropEmu.ReleaseHistoryViewer();
			base.OnClosing(e);
		}

		private void StartEmuThread()
		{
			if(_emuThread == null) {
				_emuThread = new Thread(() => {
					try {
						InteropEmu.RunHistoryViewer();
						_emuThread = null;
					} catch(Exception ex) {
						MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
						_emuThread = null;
					}
				});
				_emuThread.Start();
			}
		}

		private void btnPausePlay_Click(object sender, EventArgs e)
		{
			if(trkPosition.Value == trkPosition.Maximum) {
				InteropEmu.SetHistoryViewerPosition(0);
			}
			InteropEmu.SetHistoryViewerPauseStatus(!_paused);
		}

		private void trkPosition_ValueChanged(object sender, EventArgs e)
		{
			InteropEmu.SetHistoryViewerPosition((UInt32)trkPosition.Value);
		}

		private void tmrUpdatePosition_Tick(object sender, EventArgs e)
		{
			_paused = InteropEmu.GetHistoryViewerPauseStatus();
			if(_paused) {
				btnPausePlay.Image = Properties.Resources.Play;
			} else {
				btnPausePlay.Image = Properties.Resources.Pause;
			}

			UInt32 positionInSeconds = InteropEmu.GetHistoryViewerPosition() / 2;
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
	}
}
