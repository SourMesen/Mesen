namespace Mesen.GUI.Controls
{
	partial class ctrlNsfPlayer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.tlpNsfInfo = new System.Windows.Forms.TableLayoutPanel();
			this.lblCopyrightValue = new System.Windows.Forms.Label();
			this.lblArtistValue = new System.Windows.Forms.Label();
			this.lblTitleValue = new System.Windows.Forms.Label();
			this.lblArtist = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.lblMmc5 = new System.Windows.Forms.Label();
			this.lblFds = new System.Windows.Forms.Label();
			this.lblNamco = new System.Windows.Forms.Label();
			this.lblSunsoft = new System.Windows.Forms.Label();
			this.lblVrc6 = new System.Windows.Forms.Label();
			this.lblVrc7 = new System.Windows.Forms.Label();
			this.lblSoundChips = new System.Windows.Forms.Label();
			this.trkVolume = new System.Windows.Forms.TrackBar();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.cboTrack = new System.Windows.Forms.ComboBox();
			this.lblTrackTotal = new System.Windows.Forms.Label();
			this.lblTime = new System.Windows.Forms.Label();
			this.pnlBackground = new System.Windows.Forms.Panel();
			this.picBackground = new System.Windows.Forms.PictureBox();
			this.tmrFastForward = new System.Windows.Forms.Timer(this.components);
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.lblRecording = new System.Windows.Forms.Label();
			this.lblRecordingDot = new System.Windows.Forms.Label();
			this.lblFastForward = new System.Windows.Forms.Label();
			this.lblFastForwardIcon = new System.Windows.Forms.Label();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.lblRewinding = new System.Windows.Forms.Label();
			this.lblRewindIcon = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.lblSlowMotion = new System.Windows.Forms.Label();
			this.lblSlowMotionIcon = new System.Windows.Forms.Label();
			this.tlpRepeatShuffle = new System.Windows.Forms.TableLayoutPanel();
			this.picRepeat = new System.Windows.Forms.PictureBox();
			this.picShuffle = new System.Windows.Forms.PictureBox();
			this.tlpMain.SuspendLayout();
			this.tlpNsfInfo.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkVolume)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.pnlBackground.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
			this.tableLayoutPanel4.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.tlpRepeatShuffle.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picRepeat)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picShuffle)).BeginInit();
			this.SuspendLayout();
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 5;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpMain.Controls.Add(this.btnPrevious, 1, 2);
			this.tlpMain.Controls.Add(this.btnPause, 2, 2);
			this.tlpMain.Controls.Add(this.btnNext, 3, 2);
			this.tlpMain.Controls.Add(this.tlpNsfInfo, 0, 1);
			this.tlpMain.Controls.Add(this.trkVolume, 4, 2);
			this.tlpMain.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.tlpMain.Controls.Add(this.lblTime, 0, 3);
			this.tlpMain.Controls.Add(this.pnlBackground, 0, 0);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 4;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpMain.Size = new System.Drawing.Size(371, 281);
			this.tlpMain.TabIndex = 0;
			// 
			// btnPrevious
			// 
			this.btnPrevious.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnPrevious.BackColor = System.Drawing.SystemColors.Control;
			this.btnPrevious.Image = global::Mesen.GUI.Properties.Resources.PrevTrack;
			this.btnPrevious.Location = new System.Drawing.Point(126, 220);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(33, 25);
			this.btnPrevious.TabIndex = 1;
			this.btnPrevious.UseVisualStyleBackColor = true;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// btnPause
			// 
			this.btnPause.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnPause.BackColor = System.Drawing.SystemColors.Control;
			this.btnPause.Image = global::Mesen.GUI.Properties.Resources.Pause;
			this.btnPause.Location = new System.Drawing.Point(165, 216);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(40, 33);
			this.btnPause.TabIndex = 0;
			this.btnPause.UseVisualStyleBackColor = true;
			this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
			// 
			// btnNext
			// 
			this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnNext.BackColor = System.Drawing.SystemColors.Control;
			this.btnNext.Image = global::Mesen.GUI.Properties.Resources.NextTrack;
			this.btnNext.Location = new System.Drawing.Point(211, 220);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(33, 25);
			this.btnNext.TabIndex = 2;
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			this.btnNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNext_MouseDown);
			this.btnNext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnNext_MouseUp);
			// 
			// tlpNsfInfo
			// 
			this.tlpNsfInfo.AutoSize = true;
			this.tlpNsfInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpNsfInfo.ColumnCount = 4;
			this.tlpMain.SetColumnSpan(this.tlpNsfInfo, 5);
			this.tlpNsfInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpNsfInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpNsfInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
			this.tlpNsfInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpNsfInfo.Controls.Add(this.lblCopyrightValue, 2, 2);
			this.tlpNsfInfo.Controls.Add(this.lblArtistValue, 2, 1);
			this.tlpNsfInfo.Controls.Add(this.lblTitleValue, 2, 0);
			this.tlpNsfInfo.Controls.Add(this.lblArtist, 1, 1);
			this.tlpNsfInfo.Controls.Add(this.lblTitle, 1, 0);
			this.tlpNsfInfo.Controls.Add(this.lblCopyright, 1, 2);
			this.tlpNsfInfo.Controls.Add(this.tableLayoutPanel3, 2, 3);
			this.tlpNsfInfo.Controls.Add(this.lblSoundChips, 1, 3);
			this.tlpNsfInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tlpNsfInfo.Location = new System.Drawing.Point(0, 121);
			this.tlpNsfInfo.Margin = new System.Windows.Forms.Padding(0);
			this.tlpNsfInfo.Name = "tlpNsfInfo";
			this.tlpNsfInfo.RowCount = 4;
			this.tlpNsfInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNsfInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNsfInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNsfInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpNsfInfo.Size = new System.Drawing.Size(371, 86);
			this.tlpNsfInfo.TabIndex = 3;
			// 
			// lblCopyrightValue
			// 
			this.lblCopyrightValue.AutoSize = true;
			this.lblCopyrightValue.ForeColor = System.Drawing.Color.White;
			this.lblCopyrightValue.Location = new System.Drawing.Point(101, 42);
			this.lblCopyrightValue.Name = "lblCopyrightValue";
			this.lblCopyrightValue.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.lblCopyrightValue.Size = new System.Drawing.Size(63, 21);
			this.lblCopyrightValue.TabIndex = 5;
			this.lblCopyrightValue.Text = "[[Copyright]]";
			// 
			// lblArtistValue
			// 
			this.lblArtistValue.AutoSize = true;
			this.lblArtistValue.ForeColor = System.Drawing.Color.White;
			this.lblArtistValue.Location = new System.Drawing.Point(101, 21);
			this.lblArtistValue.Name = "lblArtistValue";
			this.lblArtistValue.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.lblArtistValue.Size = new System.Drawing.Size(42, 21);
			this.lblArtistValue.TabIndex = 4;
			this.lblArtistValue.Text = "[[Artist]]";
			// 
			// lblTitleValue
			// 
			this.lblTitleValue.AutoSize = true;
			this.lblTitleValue.ForeColor = System.Drawing.Color.White;
			this.lblTitleValue.Location = new System.Drawing.Point(101, 0);
			this.lblTitleValue.Name = "lblTitleValue";
			this.lblTitleValue.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.lblTitleValue.Size = new System.Drawing.Size(39, 21);
			this.lblTitleValue.TabIndex = 3;
			this.lblTitleValue.Text = "[[Title]]";
			// 
			// lblArtist
			// 
			this.lblArtist.AutoSize = true;
			this.lblArtist.ForeColor = System.Drawing.Color.White;
			this.lblArtist.Location = new System.Drawing.Point(25, 21);
			this.lblArtist.Name = "lblArtist";
			this.lblArtist.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.lblArtist.Size = new System.Drawing.Size(33, 21);
			this.lblArtist.TabIndex = 2;
			this.lblArtist.Text = "Artist:";
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(25, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.lblTitle.Size = new System.Drawing.Size(30, 21);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "Title:";
			// 
			// lblCopyright
			// 
			this.lblCopyright.AutoSize = true;
			this.lblCopyright.ForeColor = System.Drawing.Color.White;
			this.lblCopyright.Location = new System.Drawing.Point(25, 42);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.lblCopyright.Size = new System.Drawing.Size(54, 21);
			this.lblCopyright.TabIndex = 1;
			this.lblCopyright.Text = "Copyright:";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 6;
			this.tlpNsfInfo.SetColumnSpan(this.tableLayoutPanel3, 2);
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.lblMmc5, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.lblFds, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.lblNamco, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.lblSunsoft, 3, 0);
			this.tableLayoutPanel3.Controls.Add(this.lblVrc6, 4, 0);
			this.tableLayoutPanel3.Controls.Add(this.lblVrc7, 5, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(101, 66);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(267, 17);
			this.tableLayoutPanel3.TabIndex = 6;
			// 
			// lblMmc5
			// 
			this.lblMmc5.AutoSize = true;
			this.lblMmc5.BackColor = System.Drawing.Color.Transparent;
			this.lblMmc5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMmc5.ForeColor = System.Drawing.Color.White;
			this.lblMmc5.Location = new System.Drawing.Point(28, 0);
			this.lblMmc5.Margin = new System.Windows.Forms.Padding(0);
			this.lblMmc5.Name = "lblMmc5";
			this.lblMmc5.Size = new System.Drawing.Size(38, 13);
			this.lblMmc5.TabIndex = 2;
			this.lblMmc5.Text = "MMC5";
			// 
			// lblFds
			// 
			this.lblFds.AutoSize = true;
			this.lblFds.BackColor = System.Drawing.Color.Transparent;
			this.lblFds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFds.ForeColor = System.Drawing.Color.White;
			this.lblFds.Location = new System.Drawing.Point(0, 0);
			this.lblFds.Margin = new System.Windows.Forms.Padding(0);
			this.lblFds.Name = "lblFds";
			this.lblFds.Size = new System.Drawing.Size(28, 13);
			this.lblFds.TabIndex = 0;
			this.lblFds.Text = "FDS";
			// 
			// lblNamco
			// 
			this.lblNamco.AutoSize = true;
			this.lblNamco.BackColor = System.Drawing.Color.Transparent;
			this.lblNamco.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNamco.ForeColor = System.Drawing.Color.White;
			this.lblNamco.Location = new System.Drawing.Point(66, 0);
			this.lblNamco.Margin = new System.Windows.Forms.Padding(0);
			this.lblNamco.Name = "lblNamco";
			this.lblNamco.Size = new System.Drawing.Size(41, 13);
			this.lblNamco.TabIndex = 1;
			this.lblNamco.Text = "Namco";
			// 
			// lblSunsoft
			// 
			this.lblSunsoft.AutoSize = true;
			this.lblSunsoft.BackColor = System.Drawing.Color.Transparent;
			this.lblSunsoft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSunsoft.ForeColor = System.Drawing.Color.White;
			this.lblSunsoft.Location = new System.Drawing.Point(107, 0);
			this.lblSunsoft.Margin = new System.Windows.Forms.Padding(0);
			this.lblSunsoft.Name = "lblSunsoft";
			this.lblSunsoft.Size = new System.Drawing.Size(43, 13);
			this.lblSunsoft.TabIndex = 5;
			this.lblSunsoft.Text = "Sunsoft";
			// 
			// lblVrc6
			// 
			this.lblVrc6.AutoSize = true;
			this.lblVrc6.BackColor = System.Drawing.Color.Transparent;
			this.lblVrc6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVrc6.ForeColor = System.Drawing.Color.White;
			this.lblVrc6.Location = new System.Drawing.Point(150, 0);
			this.lblVrc6.Margin = new System.Windows.Forms.Padding(0);
			this.lblVrc6.Name = "lblVrc6";
			this.lblVrc6.Size = new System.Drawing.Size(35, 13);
			this.lblVrc6.TabIndex = 4;
			this.lblVrc6.Text = "VRC6";
			// 
			// lblVrc7
			// 
			this.lblVrc7.AutoSize = true;
			this.lblVrc7.BackColor = System.Drawing.Color.Transparent;
			this.lblVrc7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVrc7.ForeColor = System.Drawing.Color.White;
			this.lblVrc7.Location = new System.Drawing.Point(185, 0);
			this.lblVrc7.Margin = new System.Windows.Forms.Padding(0);
			this.lblVrc7.Name = "lblVrc7";
			this.lblVrc7.Size = new System.Drawing.Size(35, 13);
			this.lblVrc7.TabIndex = 3;
			this.lblVrc7.Text = "VRC7";
			// 
			// lblSoundChips
			// 
			this.lblSoundChips.AutoSize = true;
			this.lblSoundChips.ForeColor = System.Drawing.Color.White;
			this.lblSoundChips.Location = new System.Drawing.Point(25, 63);
			this.lblSoundChips.Name = "lblSoundChips";
			this.lblSoundChips.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.lblSoundChips.Size = new System.Drawing.Size(70, 21);
			this.lblSoundChips.TabIndex = 7;
			this.lblSoundChips.Text = "Sound Chips:";
			// 
			// trkVolume
			// 
			this.trkVolume.Location = new System.Drawing.Point(257, 210);
			this.trkVolume.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			this.trkVolume.Maximum = 100;
			this.trkVolume.Name = "trkVolume";
			this.trkVolume.Size = new System.Drawing.Size(106, 45);
			this.trkVolume.TabIndex = 6;
			this.trkVolume.TickFrequency = 10;
			this.trkVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.trkVolume.ValueChanged += new System.EventHandler(this.trkVolume_ValueChanged);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.cboTrack);
			this.flowLayoutPanel1.Controls.Add(this.lblTrackTotal);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(28, 219);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(80, 27);
			this.flowLayoutPanel1.TabIndex = 9;
			// 
			// cboTrack
			// 
			this.cboTrack.BackColor = System.Drawing.Color.Black;
			this.cboTrack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTrack.DropDownWidth = 200;
			this.cboTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cboTrack.ForeColor = System.Drawing.Color.White;
			this.cboTrack.FormattingEnabled = true;
			this.cboTrack.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
			this.cboTrack.Location = new System.Drawing.Point(3, 3);
			this.cboTrack.Name = "cboTrack";
			this.cboTrack.Size = new System.Drawing.Size(47, 21);
			this.cboTrack.TabIndex = 8;
			this.cboTrack.DropDown += new System.EventHandler(this.cboTrack_DropDown);
			this.cboTrack.SelectedIndexChanged += new System.EventHandler(this.cboTrack_SelectedIndexChanged);
			this.cboTrack.DropDownClosed += new System.EventHandler(this.cboTrack_DropDownClosed);
			// 
			// lblTrackTotal
			// 
			this.lblTrackTotal.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblTrackTotal.AutoSize = true;
			this.lblTrackTotal.ForeColor = System.Drawing.Color.White;
			this.lblTrackTotal.Location = new System.Drawing.Point(53, 7);
			this.lblTrackTotal.Margin = new System.Windows.Forms.Padding(0);
			this.lblTrackTotal.Name = "lblTrackTotal";
			this.lblTrackTotal.Size = new System.Drawing.Size(27, 13);
			this.lblTrackTotal.TabIndex = 4;
			this.lblTrackTotal.Text = "/ 24";
			// 
			// lblTime
			// 
			this.lblTime.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lblTime.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.lblTime, 5);
			this.lblTime.ForeColor = System.Drawing.Color.White;
			this.lblTime.Location = new System.Drawing.Point(168, 258);
			this.lblTime.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(34, 13);
			this.lblTime.TabIndex = 10;
			this.lblTime.Text = "00:00";
			this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// pnlBackground
			// 
			this.tlpMain.SetColumnSpan(this.pnlBackground, 5);
			this.pnlBackground.Controls.Add(this.picBackground);
			this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBackground.Location = new System.Drawing.Point(3, 3);
			this.pnlBackground.Name = "pnlBackground";
			this.pnlBackground.Size = new System.Drawing.Size(365, 115);
			this.pnlBackground.TabIndex = 11;
			// 
			// picBackground
			// 
			this.picBackground.BackgroundImage = global::Mesen.GUI.Properties.Resources.NsfBackground;
			this.picBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.picBackground.Location = new System.Drawing.Point(114, -3);
			this.picBackground.Margin = new System.Windows.Forms.Padding(0);
			this.picBackground.MaximumSize = new System.Drawing.Size(500, 90);
			this.picBackground.Name = "picBackground";
			this.picBackground.Size = new System.Drawing.Size(150, 90);
			this.picBackground.TabIndex = 5;
			this.picBackground.TabStop = false;
			// 
			// tmrFastForward
			// 
			this.tmrFastForward.Interval = 500;
			this.tmrFastForward.Tick += new System.EventHandler(this.tmrFastForward_Tick);
			// 
			// lblRecording
			// 
			this.lblRecording.AutoSize = true;
			this.lblRecording.ForeColor = System.Drawing.Color.Red;
			this.lblRecording.Location = new System.Drawing.Point(15, 4);
			this.lblRecording.Margin = new System.Windows.Forms.Padding(0);
			this.lblRecording.Name = "lblRecording";
			this.lblRecording.Size = new System.Drawing.Size(29, 13);
			this.lblRecording.TabIndex = 9;
			this.lblRecording.Text = "REC";
			// 
			// lblRecordingDot
			// 
			this.lblRecordingDot.AutoSize = true;
			this.lblRecordingDot.BackColor = System.Drawing.Color.Transparent;
			this.lblRecordingDot.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRecordingDot.ForeColor = System.Drawing.Color.Red;
			this.lblRecordingDot.Location = new System.Drawing.Point(1, 0);
			this.lblRecordingDot.Margin = new System.Windows.Forms.Padding(0);
			this.lblRecordingDot.Name = "lblRecordingDot";
			this.lblRecordingDot.Size = new System.Drawing.Size(17, 18);
			this.lblRecordingDot.TabIndex = 10;
			this.lblRecordingDot.Text = "●";
			// 
			// lblFastForward
			// 
			this.lblFastForward.AutoSize = true;
			this.lblFastForward.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblFastForward.Location = new System.Drawing.Point(14, 4);
			this.lblFastForward.Margin = new System.Windows.Forms.Padding(0);
			this.lblFastForward.Name = "lblFastForward";
			this.lblFastForward.Size = new System.Drawing.Size(68, 13);
			this.lblFastForward.TabIndex = 11;
			this.lblFastForward.Text = "Fast Forward";
			// 
			// lblFastForwardIcon
			// 
			this.lblFastForwardIcon.AutoSize = true;
			this.lblFastForwardIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFastForwardIcon.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblFastForwardIcon.Location = new System.Drawing.Point(0, 0);
			this.lblFastForwardIcon.Margin = new System.Windows.Forms.Padding(0);
			this.lblFastForwardIcon.Name = "lblFastForwardIcon";
			this.lblFastForwardIcon.Size = new System.Drawing.Size(17, 18);
			this.lblFastForwardIcon.TabIndex = 12;
			this.lblFastForwardIcon.Text = "»";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.panel4, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.panel2, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.panel3, 0, 3);
			this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 5);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 5;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(121, 85);
			this.tableLayoutPanel4.TabIndex = 13;
			// 
			// panel4
			// 
			this.panel4.AutoSize = true;
			this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel4.Controls.Add(this.lblRewinding);
			this.panel4.Controls.Add(this.lblRewindIcon);
			this.panel4.Location = new System.Drawing.Point(0, 36);
			this.panel4.Margin = new System.Windows.Forms.Padding(0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(71, 18);
			this.panel4.TabIndex = 3;
			// 
			// lblRewinding
			// 
			this.lblRewinding.AutoSize = true;
			this.lblRewinding.ForeColor = System.Drawing.Color.DarkOrange;
			this.lblRewinding.Location = new System.Drawing.Point(14, 4);
			this.lblRewinding.Margin = new System.Windows.Forms.Padding(0);
			this.lblRewinding.Name = "lblRewinding";
			this.lblRewinding.Size = new System.Drawing.Size(57, 13);
			this.lblRewinding.TabIndex = 11;
			this.lblRewinding.Text = "Rewinding";
			// 
			// lblRewindIcon
			// 
			this.lblRewindIcon.AutoSize = true;
			this.lblRewindIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRewindIcon.ForeColor = System.Drawing.Color.DarkOrange;
			this.lblRewindIcon.Location = new System.Drawing.Point(0, 0);
			this.lblRewindIcon.Margin = new System.Windows.Forms.Padding(0);
			this.lblRewindIcon.Name = "lblRewindIcon";
			this.lblRewindIcon.Size = new System.Drawing.Size(17, 18);
			this.lblRewindIcon.TabIndex = 12;
			this.lblRewindIcon.Text = "«";
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel2.Controls.Add(this.lblRecordingDot);
			this.panel2.Controls.Add(this.lblRecording);
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(44, 18);
			this.panel2.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.lblFastForward);
			this.panel1.Controls.Add(this.lblFastForwardIcon);
			this.panel1.Location = new System.Drawing.Point(0, 18);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(82, 18);
			this.panel1.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.AutoSize = true;
			this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel3.Controls.Add(this.lblSlowMotion);
			this.panel3.Controls.Add(this.lblSlowMotionIcon);
			this.panel3.Location = new System.Drawing.Point(0, 54);
			this.panel3.Margin = new System.Windows.Forms.Padding(0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(79, 18);
			this.panel3.TabIndex = 2;
			// 
			// lblSlowMotion
			// 
			this.lblSlowMotion.AutoSize = true;
			this.lblSlowMotion.ForeColor = System.Drawing.Color.DarkOrange;
			this.lblSlowMotion.Location = new System.Drawing.Point(14, 4);
			this.lblSlowMotion.Margin = new System.Windows.Forms.Padding(0);
			this.lblSlowMotion.Name = "lblSlowMotion";
			this.lblSlowMotion.Size = new System.Drawing.Size(65, 13);
			this.lblSlowMotion.TabIndex = 11;
			this.lblSlowMotion.Text = "Slow Motion";
			// 
			// lblSlowMotionIcon
			// 
			this.lblSlowMotionIcon.AutoSize = true;
			this.lblSlowMotionIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSlowMotionIcon.ForeColor = System.Drawing.Color.DarkOrange;
			this.lblSlowMotionIcon.Location = new System.Drawing.Point(0, 0);
			this.lblSlowMotionIcon.Margin = new System.Windows.Forms.Padding(0);
			this.lblSlowMotionIcon.Name = "lblSlowMotionIcon";
			this.lblSlowMotionIcon.Size = new System.Drawing.Size(17, 18);
			this.lblSlowMotionIcon.TabIndex = 12;
			this.lblSlowMotionIcon.Text = "«";
			// 
			// tlpRepeatShuffle
			// 
			this.tlpRepeatShuffle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tlpRepeatShuffle.ColumnCount = 1;
			this.tlpRepeatShuffle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpRepeatShuffle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpRepeatShuffle.Controls.Add(this.picRepeat, 0, 1);
			this.tlpRepeatShuffle.Controls.Add(this.picShuffle, 0, 0);
			this.tlpRepeatShuffle.Location = new System.Drawing.Point(336, 8);
			this.tlpRepeatShuffle.Margin = new System.Windows.Forms.Padding(0);
			this.tlpRepeatShuffle.Name = "tlpRepeatShuffle";
			this.tlpRepeatShuffle.RowCount = 3;
			this.tlpRepeatShuffle.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpRepeatShuffle.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpRepeatShuffle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpRepeatShuffle.Size = new System.Drawing.Size(30, 60);
			this.tlpRepeatShuffle.TabIndex = 14;
			// 
			// picRepeat
			// 
			this.picRepeat.BackgroundImage = global::Mesen.GUI.Properties.Resources.Repeat;
			this.picRepeat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.picRepeat.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picRepeat.Location = new System.Drawing.Point(3, 33);
			this.picRepeat.Name = "picRepeat";
			this.picRepeat.Size = new System.Drawing.Size(24, 24);
			this.picRepeat.TabIndex = 4;
			this.picRepeat.TabStop = false;
			this.picRepeat.Click += new System.EventHandler(this.picRepeat_Click);
			// 
			// picShuffle
			// 
			this.picShuffle.BackgroundImage = global::Mesen.GUI.Properties.Resources.Shuffle;
			this.picShuffle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.picShuffle.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picShuffle.Location = new System.Drawing.Point(3, 3);
			this.picShuffle.Name = "picShuffle";
			this.picShuffle.Size = new System.Drawing.Size(24, 24);
			this.picShuffle.TabIndex = 3;
			this.picShuffle.TabStop = false;
			this.picShuffle.Click += new System.EventHandler(this.picShuffle_Click);
			// 
			// ctrlNsfPlayer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.Controls.Add(this.tlpRepeatShuffle);
			this.Controls.Add(this.tableLayoutPanel4);
			this.Controls.Add(this.tlpMain);
			this.Name = "ctrlNsfPlayer";
			this.Size = new System.Drawing.Size(371, 281);
			this.VisibleChanged += new System.EventHandler(this.ctrlNsfPlayer_VisibleChanged);
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.tlpNsfInfo.ResumeLayout(false);
			this.tlpNsfInfo.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkVolume)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.pnlBackground.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.tlpRepeatShuffle.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picRepeat)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picShuffle)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.TableLayoutPanel tlpNsfInfo;
		private System.Windows.Forms.Label lblCopyrightValue;
		private System.Windows.Forms.Label lblArtistValue;
		private System.Windows.Forms.Label lblTitleValue;
		private System.Windows.Forms.Label lblArtist;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Label lblCopyright;
		private System.Windows.Forms.Label lblTrackTotal;
		private System.Windows.Forms.PictureBox picBackground;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label lblSunsoft;
		private System.Windows.Forms.Label lblVrc6;
		private System.Windows.Forms.Label lblVrc7;
		private System.Windows.Forms.Label lblMmc5;
		private System.Windows.Forms.Label lblNamco;
		private System.Windows.Forms.Label lblFds;
		private System.Windows.Forms.Label lblSoundChips;
		private System.Windows.Forms.TrackBar trkVolume;
		private System.Windows.Forms.ComboBox cboTrack;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label lblTime;
		private System.Windows.Forms.Timer tmrFastForward;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label lblRecording;
		private System.Windows.Forms.Label lblRecordingDot;
		private System.Windows.Forms.Label lblFastForward;
		private System.Windows.Forms.Label lblFastForwardIcon;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label lblSlowMotion;
		private System.Windows.Forms.Label lblSlowMotionIcon;
		private System.Windows.Forms.TableLayoutPanel tlpRepeatShuffle;
		private System.Windows.Forms.PictureBox picShuffle;
		private System.Windows.Forms.PictureBox picRepeat;
		private System.Windows.Forms.Panel pnlBackground;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label lblRewinding;
		private System.Windows.Forms.Label lblRewindIcon;
	}
}
