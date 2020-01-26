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
using System.IO.Compression;
using Mesen.GUI.Config;
using static Mesen.GUI.Controls.ctrlRecentGames;

namespace Mesen.GUI.Controls
{
	public partial class ctrlRecentGame : UserControl
	{
		public event RecentGameLoadedHandler OnRecentGameLoaded;
		private RecentGameInfo _recentGame;

		public ctrlRecentGame()
		{
			InitializeComponent();
		}

		public RecentGameInfo RecentGame
		{
			get { return _recentGame; }
			set
			{
				if(value == null) {
					_recentGame = null;
					picPreviousState.Visible = false;
					lblGameName.Visible = false;
					lblSaveDate.Visible = false;
					return;
				}

				if(_recentGame == value) {
					return;
				}

				_recentGame = value;

				lblGameName.Text = Path.GetFileNameWithoutExtension(_recentGame.FileName);
				lblSaveDate.Text = new FileInfo(_recentGame.FileName).LastWriteTime.ToString();

				lblGameName.Visible = true;
				lblSaveDate.Visible = true;
				picPreviousState.Visible = true;

				Task.Run(() => {
					Image img = null;
					try {
						ZipArchive zip = new ZipArchive(new MemoryStream(File.ReadAllBytes(value.FileName)));
						ZipArchiveEntry entry = zip.GetEntry("Screenshot.png");
						if(entry != null) {
							using(Stream stream = entry.Open()) {
								img = Image.FromStream(stream);
							}
						}
						using(StreamReader sr = new StreamReader(zip.GetEntry("RomInfo.txt").Open())) {
							string romName = sr.ReadLine();
							value.RomPath = sr.ReadLine();
						}
					} catch { }

					this.BeginInvoke((Action)(() => {
						picPreviousState.Image = img;
					}));
				});
			}
		}

		public bool Highlight
		{
			set { picPreviousState.Highlight = value; }
		}

		private void picPreviousState_Click(object sender, EventArgs e)
		{
			InteropEmu.LoadRecentGame(_recentGame.FileName, ConfigManager.Config.PreferenceInfo.GameSelectionScreenResetGame);
			OnRecentGameLoaded?.Invoke(_recentGame);
		}
	}
}
