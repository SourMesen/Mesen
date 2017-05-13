using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Mesen.GUI.Config;
using System.Drawing.Text;
using System.IO.Compression;

namespace Mesen.GUI.Controls
{
	public partial class ctrlRecentGames : UserControl
	{
		private int _currentIndex = 0;
		private List<RecentGameInfo> _recentGames = new List<RecentGameInfo>();
		private PrivateFontCollection _fonts = new PrivateFontCollection();

		private class RecentGameInfo
		{
			public string FileName { get; set; }
			public string RomName { get; set; }
			public string RomPath { get; set; }
			public DateTime Timestamp { get; set; }
			public Image Screenshot { get; set; }
		}

		public ctrlRecentGames()
		{
			InitializeComponent();

			DoubleBuffered = true;

			bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			if(!designMode) {
				_fonts.AddFontFile(Path.Combine(ConfigManager.HomeFolder, "Resources", "PixelFont.ttf"));
				lblGameName.Font = new Font(_fonts.Families[0], 10);
				lblSaveDate.Font = new Font(_fonts.Families[0], 10);

				picPrevGame.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
				Initialize();
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			if(_recentGames.Count == 0) {
				this.Visible = false;
			}
			base.OnVisibleChanged(e);
		}

		public void Initialize()
		{
			_recentGames = new List<RecentGameInfo>();
			_currentIndex = 0;

			foreach(string file in Directory.GetFiles(ConfigManager.RecentGamesFolder, "*.rgd")) {
				try {
					RecentGameInfo info = new RecentGameInfo();
					ZipArchive zip = new ZipArchive(new MemoryStream(File.ReadAllBytes(file)));

					Stream stream = zip.GetEntry("Screenshot.png").Open();
					info.Screenshot = Image.FromStream(stream);

					using(StreamReader sr = new StreamReader(zip.GetEntry("RomInfo.txt").Open())) {
						info.RomName = sr.ReadLine();
						info.RomPath = sr.ReadLine();
					}

					info.Timestamp = new FileInfo(file).LastWriteTime;
					info.FileName = file;

					if(File.Exists(info.RomPath)) {
						_recentGames.Add(info);
					}
				} catch { }
			}

			_recentGames = _recentGames.OrderBy((info) => info.Timestamp).Reverse().ToList();

			if(_recentGames.Count > 5) {
				_recentGames.RemoveRange(5, _recentGames.Count - 5);
			}

			picPrevGame.Visible = _recentGames.Count > 1;
			picNextGame.Visible = _recentGames.Count > 1;

			if(_recentGames.Count == 0) {
				this.Visible = false;
			} else {
				UpdateGameInfo();
			}
		}

		public void UpdateGameInfo()
		{
			if(_currentIndex < _recentGames.Count) {
				lblGameName.Text = Path.GetFileNameWithoutExtension(_recentGames[_currentIndex].RomName);
				lblSaveDate.Text = _recentGames[_currentIndex].Timestamp.ToString();
				picPreviousState.Image = _recentGames[_currentIndex].Screenshot;
				UpdateSize();
			}
		}

		private void UpdateSize()
		{
			tlpPreviousState.Visible = false;
			Size maxSize = new Size(this.Size.Width - 120, this.Size.Height - 50);

			double xRatio = (double)picPreviousState.Image.Width / maxSize.Width;
			double yRatio = (double)picPreviousState.Image.Height / maxSize.Height;
			double ratio = Math.Max(xRatio, yRatio);

			Size newSize = new Size((int)(picPreviousState.Image.Width / ratio), (int)(picPreviousState.Image.Height / ratio));
			picPreviousState.Size = newSize;
			pnlPreviousState.Size = new Size(newSize.Width+4, newSize.Height+4);
			tlpPreviousState.Visible = true;
		}

		protected override void OnResize(EventArgs e)
		{
			if(picPreviousState.Image != null) {
				UpdateSize();
			}
			base.OnResize(e);
		}

		private void picPreviousState_MouseEnter(object sender, EventArgs e)
		{
			pnlPreviousState.BackColor = Color.LightBlue;
		}

		private void picPreviousState_MouseLeave(object sender, EventArgs e)
		{
			pnlPreviousState.BackColor = Color.Gray;
		}

		private void picPreviousState_Click(object sender, EventArgs e)
		{
			InteropEmu.LoadRecentGame(_recentGames[_currentIndex].FileName);
		}

		private void picNextGame_MouseDown(object sender, MouseEventArgs e)
		{
			_currentIndex = (_currentIndex + 1) % _recentGames.Count;
			UpdateGameInfo();
		}

		private void picPrevGame_MouseDown(object sender, MouseEventArgs e)
		{
			if(_currentIndex == 0) {
				_currentIndex = _recentGames.Count - 1;
			} else {
				_currentIndex--;
			}
			UpdateGameInfo();
		}
	}

	public class DBTableLayoutPanel : TableLayoutPanel
	{
		public DBTableLayoutPanel()
		{
			DoubleBuffered = true;
		}
	}

	public class GamePreviewBox : PictureBox
	{
		public System.Drawing.Drawing2D.InterpolationMode InterpolationMode { get; set; }

		public GamePreviewBox()
		{
			DoubleBuffered = true;
			InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.InterpolationMode = InterpolationMode;
			base.OnPaint(pe);
		}
	}
}
