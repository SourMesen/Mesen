﻿using System;
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
using System.Drawing.Drawing2D;
using System.IO.Compression;
using Mesen.GUI.Forms;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Controls
{
	public partial class ctrlRecentGames : BaseControl
	{
		private int _elementsPerRow = 0;
		private int _elementsPerPage = 0;

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
		private List<ctrlRecentGame> _controls = new List<ctrlRecentGame>();

		public ctrlRecentGames()
		{
			InitializeComponent();
			if(IsDesignMode) {
				return;
			}

			DoubleBuffered = true;
			picPrevGame.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
			ThemeHelper.ExcludeFromTheme(this);
		}

		private void InitGrid()
		{
			int elementsPerRow = 1;
			if(ClientSize.Width > 850 && ClientSize.Height > 850) {
				elementsPerRow = 3;
			} else if(ClientSize.Width > 450 && ClientSize.Height > 450) {
				elementsPerRow = 2;
			}

			if(_recentGames.Count <= 1) {
				elementsPerRow = 1;
			} else if(_recentGames.Count <= 4) {
				elementsPerRow = Math.Min(2, elementsPerRow);
			}

			if(_elementsPerRow == elementsPerRow) {
				return;
			}

			_elementsPerRow = elementsPerRow;
			_elementsPerPage = elementsPerRow * elementsPerRow;

			_controls = new List<ctrlRecentGame>();
			tlpGrid.SuspendLayout();
			tlpGrid.ColumnCount = _elementsPerRow;
			tlpGrid.RowCount = _elementsPerRow;
			tlpGrid.ColumnStyles.Clear();
			tlpGrid.RowStyles.Clear();
			tlpGrid.Controls.Clear();
			for(int j = 0; j < _elementsPerRow; j++) {
				tlpGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / _elementsPerRow));
				tlpGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / _elementsPerRow));
			}

			for(int j = 0; j < _elementsPerRow; j++) {
				for(int i = 0; i < _elementsPerRow; i++) {
					ctrlRecentGame ctrl = new ctrlRecentGame();
					ctrl.OnRecentGameLoaded += RecentGameLoaded;
					ctrl.Dock = DockStyle.Fill;
					ctrl.Margin = new Padding(2);
					tlpGrid.Controls.Add(ctrl, i, j);
					_controls.Add(ctrl);
				}
			}
			tlpGrid.ResumeLayout();
			UpdateGameInfo();
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
					InitGrid();
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

			if(_recentGames.Count > 36) {
				_recentGames.RemoveRange(36, _recentGames.Count - 36);
			}

			InitGrid();

			if(_recentGames.Count == 0) {
				this.Visible = false;
				tmrInput.Enabled = false;
			} else {
				UpdateGameInfo();
				tmrInput.Enabled = true;
			}

			picPrevGame.Visible = true;
			picNextGame.Visible = true;
		}

		public void UpdateGameInfo()
		{
			if(!_initialized) {
				return;
			}

			int count = _recentGames.Count;
			int pageStart = _currentIndex / _elementsPerPage * _elementsPerPage;

			for(int i = 0; i < _elementsPerPage; i++) {
				_controls[i].RecentGame = count > pageStart + i ? _recentGames[pageStart + i] : null;
				_controls[i].Highlight = (_currentIndex % _elementsPerPage) == i;
			}
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

			if(this._elementsPerRow > 0) {
				InitGrid();
			}
			base.OnResize(e);
		}

		private void picNextGame_MouseDown(object sender, MouseEventArgs e)
		{
			GoToNextPage();
		}

		private void picPrevGame_MouseDown(object sender, MouseEventArgs e)
		{
			GoToPreviousPage();
		}

		private void GoToPreviousPage()
		{
			if(_currentIndex < _elementsPerPage) {
				_currentIndex = _recentGames.Count - 1;
			} else {
				_currentIndex -= _elementsPerPage;
			}
			UpdateGameInfo();
		}

		private bool IsOnLastPage { get { return (_currentIndex / _elementsPerPage) == ((_recentGames.Count - 1) / _elementsPerPage); } }

		private void GoToNextPage()
		{
			if(_currentIndex + _elementsPerPage < _recentGames.Count) {
				_currentIndex += _elementsPerPage;
			} else {
				_currentIndex = IsOnLastPage ? 0 : (_recentGames.Count - 1);
			}
			UpdateGameInfo();
		}

		private void LoadSelectedGame()
		{
			InteropEmu.LoadRecentGame(_recentGames[_currentIndex].FileName, ConfigManager.Config.PreferenceInfo.GameSelectionScreenResetGame);
			OnRecentGameLoaded?.Invoke(_recentGames[_currentIndex]);
		}

		private void RecentGameLoaded(RecentGameInfo gameInfo)
		{
			OnRecentGameLoaded?.Invoke(gameInfo);
		}

		private bool _waitForRelease = false;
		private void tmrInput_Tick(object sender, EventArgs e)
		{
			//Use player 1's controls to navigate the recent game selection screen
			if(Application.OpenForms.Count > 0 && Application.OpenForms[0].ContainsFocus && !InteropEmu.IsRunning()) {
				List<uint> keyCodes = InteropEmu.GetPressedKeys();
				uint keyCode = keyCodes.Count > 0 ? keyCodes[0] : 0;
				if(keyCode > 0) {
					if(!_waitForRelease) {
						foreach(KeyMappings mapping in ConfigManager.Config.InputInfo.Controllers[0].Keys) {
							if(mapping.Left == keyCode) {
								_waitForRelease = true;
								if(_currentIndex == 0) {
									_currentIndex = _recentGames.Count - 1;
								} else {
									_currentIndex--;
								}
								UpdateGameInfo();
							} else if(mapping.Right == keyCode) {
								_waitForRelease = true;
								_currentIndex = (_currentIndex + 1) % _recentGames.Count;
								UpdateGameInfo();
							} else if(mapping.Down == keyCode) {
								_waitForRelease = true;
								if(_currentIndex + _elementsPerRow < _recentGames.Count) {
									_currentIndex += _elementsPerRow;
								} else {
									_currentIndex = IsOnLastPage ? 0 : (_recentGames.Count - 1);
								}
								UpdateGameInfo();
							} else if(mapping.Up == keyCode) {
								_waitForRelease = true;
								if(_currentIndex < _elementsPerRow) {
									_currentIndex = _recentGames.Count - 1;
								} else {
									_currentIndex -= _elementsPerRow;
								}
								UpdateGameInfo();
							} else if(mapping.A == keyCode || mapping.B == keyCode || mapping.Select == keyCode || mapping.Start == keyCode) {
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
		public InterpolationMode InterpolationMode { get; set; }
		private bool _hovered = false;
		private bool _highlight = false;

		public GamePreviewBox()
		{
			DoubleBuffered = true;
			InterpolationMode = InterpolationMode.Default;
		}

		public bool Highlight
		{
			get { return _highlight; }
			set
			{
				_highlight = value;
				this.Invalidate();
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			_hovered = true;
			this.Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			_hovered = false;
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			base.OnPaint(pe);

			using(Pen pen = new Pen(_hovered ? Color.DeepSkyBlue : (_highlight ? Color.DodgerBlue : Color.DimGray), 2)) {
				pe.Graphics.DrawRectangle(pen, 1, 1, this.Width - 2, this.Height - 2);
			}
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
