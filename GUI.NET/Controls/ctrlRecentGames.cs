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
using Mesen.GUI.Forms;

namespace Mesen.GUI.Controls
{
	public partial class ctrlRecentGames : UserControl
	{
		public delegate void RecentGameLoadedHandler(RecentGameInfo gameInfo);
		public event RecentGameLoadedHandler OnRecentGameLoaded;

		public new event MouseEventHandler MouseMove
		{
			add { this.tlpPreviousState.MouseMove += value; }
			remove { this.tlpPreviousState.MouseMove -= value; }
		}

		public new event EventHandler DoubleClick
		{
			add { this.tlpPreviousState.DoubleClick += value; }
			remove { this.tlpPreviousState.DoubleClick -= value; }
		}

		private bool _initialized = false;
		private int _currentIndex = 0;
		private List<RecentGameInfo> _recentGames = new List<RecentGameInfo>();
		private PrivateFontCollection _fonts = new PrivateFontCollection();

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
			}
		}

		public new bool Visible
		{
			get { return base.Visible; }
			set
			{
				if(value && ((_initialized && _recentGames.Count == 0) || ConfigManager.Config.PreferenceInfo.DisableGameSelectionScreen)) {
					value = false;
				}

				if(value != base.Visible) {
					if(value && !_initialized) {
						//We just re-enabled the screen, initialize it
						Initialize();
					}

					base.Visible = value;
					tmrInput.Enabled = value;
				}
			}
		}
				
		public int GameCount
		{
			get { return _recentGames.Count; }
		}

		public void Initialize()
		{
			_initialized = true;
			tmrInput.Enabled = true;
			_recentGames = new List<RecentGameInfo>();
			_currentIndex = 0;

			foreach(string file in Directory.GetFiles(ConfigManager.RecentGamesFolder, "*.rgd")) {
				try {
					RecentGameInfo info = new RecentGameInfo();
					ZipArchive zip = new ZipArchive(new MemoryStream(File.ReadAllBytes(file)));

					using(StreamReader sr = new StreamReader(zip.GetEntry("RomInfo.txt").Open())) {
						info.RomName = sr.ReadLine();
						info.RomPath = sr.ReadLine();
					}

					info.Timestamp = new FileInfo(file).LastWriteTime;
					info.FileName = file;

					if(info.RomPath.Exists) {
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

				try {
					ZipArchive zip = new ZipArchive(new MemoryStream(File.ReadAllBytes(_recentGames[_currentIndex].FileName)));
					ZipArchiveEntry entry = zip.GetEntry("Screenshot.png");
					if(entry != null) {
						using(Stream stream = entry.Open()) {
							picPreviousState.Image = Image.FromStream(stream);
						}
					} else {
						picPreviousState.Image = null;
					}
				} catch {
					picPreviousState.Image = null;
				}
				UpdateSize();
			}
		}

		private void UpdateSize()
		{
			tlpPreviousState.Visible = false;
			Size maxSize = new Size(this.Size.Width - 120, this.Size.Height - 50);

			if(picPreviousState.Image != null) {
				double xRatio = (double)picPreviousState.Image.Width / maxSize.Width;
				double yRatio = (double)picPreviousState.Image.Height / maxSize.Height;
				double ratio = Math.Max(xRatio, yRatio);

				Size newSize = new Size((int)(picPreviousState.Image.Width / ratio), (int)(picPreviousState.Image.Height / ratio));
				picPreviousState.Size = newSize;
				pnlPreviousState.Size = new Size(newSize.Width+4, newSize.Height+4);
			}
			tlpPreviousState.Visible = true;
		}

		protected override void OnResize(EventArgs e)
		{
			if(Program.IsMono) {
				//Fix resize issues
				picNextGame.Dock = DockStyle.None;
				picPrevGame.Dock = DockStyle.None;
				picNextGame.Dock = DockStyle.Fill;
				picPrevGame.Dock = DockStyle.Fill;
			}

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
			LoadSelectedGame();
		}

		private void picNextGame_MouseDown(object sender, MouseEventArgs e)
		{
			GoToNextGame();
		}

		private void picPrevGame_MouseDown(object sender, MouseEventArgs e)
		{
			GoToPreviousGame();
		}

		private void GoToPreviousGame()
		{
			if(_currentIndex == 0) {
				_currentIndex = _recentGames.Count - 1;
			} else {
				_currentIndex--;
			}
			UpdateGameInfo();
		}

		private void GoToNextGame()
		{
			_currentIndex = (_currentIndex + 1) % _recentGames.Count;
			UpdateGameInfo();
		}

		private void LoadSelectedGame()
		{
			InteropEmu.LoadRecentGame(_recentGames[_currentIndex].FileName, ConfigManager.Config.PreferenceInfo.GameSelectionScreenResetGame);
			OnRecentGameLoaded?.Invoke(_recentGames[_currentIndex]);
		}

		private bool _waitForRelease = false;
		private void tmrInput_Tick(object sender, EventArgs e)
		{
			//Use player 1's controls to navigate the recent game selection screen
			if(!InteropEmu.IsRunning()) {
				uint keyCode = InteropEmu.GetPressedKey();

				if(keyCode > 0) {
					if(!_waitForRelease) {
						foreach(KeyMappings mapping in ConfigManager.Config.InputInfo.Controllers[0].Keys) {
							if(mapping.Left == keyCode) {
								_waitForRelease = true;
								GoToPreviousGame();
							} else if(mapping.Right == keyCode) {
								_waitForRelease = true;
								GoToNextGame();
							} else if(mapping.A == keyCode || mapping.B == keyCode) {
								_waitForRelease = true;
								LoadSelectedGame();
							}
						}
					}
				} else {
					_waitForRelease = false;
				}
			}
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

	public class RecentGameInfo
	{
		public string FileName { get; set; }
		public string RomName { get; set; }
		public ResourcePath RomPath { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
