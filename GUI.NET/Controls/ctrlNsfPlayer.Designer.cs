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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
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
			this.picBackground = new System.Windows.Forms.PictureBox();
			this.trkVolume = new System.Windows.Forms.TrackBar();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.cboTrack = new System.Windows.Forms.ComboBox();
			this.lblTrackTotal = new System.Windows.Forms.Label();
			this.lblTime = new System.Windows.Forms.Label();
			this.tmrFastForward = new System.Windows.Forms.Timer(this.components);
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkVolume)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.btnPrevious, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnPause, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnNext, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.picBackground, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkVolume, 4, 2);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblTime, 0, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(371, 281);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnPrevious
			// 
			this.btnPrevious.Anchor = System.Windows.Forms.AnchorStyles.Left;
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
			this.btnNext.Image = global::Mesen.GUI.Properties.Resources.NextTrack;
			this.btnNext.Location = new System.Drawing.Point(211, 220);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(33, 25);
			this.btnNext.TabIndex = 2;
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			this.btnNext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNext_MouseDown);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 5);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.lblCopyrightValue, 2, 2);
			this.tableLayoutPanel2.Controls.Add(this.lblArtistValue, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblTitleValue, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblArtist, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblTitle, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblCopyright, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 2, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblSoundChips, 1, 3);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 121);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(371, 86);
			this.tableLayoutPanel2.TabIndex = 3;
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
			this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel3, 2);
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
			this.tableLayoutPanel3.Location = new System.Drawing.Point(101, 66);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(197, 17);
			this.tableLayoutPanel3.TabIndex = 6;
			// 
			// lblMmc5
			// 
			this.lblMmc5.AutoSize = true;
			this.lblMmc5.BackColor = System.Drawing.Color.Transparent;
			this.lblMmc5.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMmc5.ForeColor = System.Drawing.Color.White;
			this.lblMmc5.Location = new System.Drawing.Point(25, 0);
			this.lblMmc5.Margin = new System.Windows.Forms.Padding(0);
			this.lblMmc5.Name = "lblMmc5";
			this.lblMmc5.Size = new System.Drawing.Size(35, 15);
			this.lblMmc5.TabIndex = 2;
			this.lblMmc5.Text = "MMC5";
			// 
			// lblFds
			// 
			this.lblFds.AutoSize = true;
			this.lblFds.BackColor = System.Drawing.Color.Transparent;
			this.lblFds.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFds.ForeColor = System.Drawing.Color.White;
			this.lblFds.Location = new System.Drawing.Point(0, 0);
			this.lblFds.Margin = new System.Windows.Forms.Padding(0);
			this.lblFds.Name = "lblFds";
			this.lblFds.Size = new System.Drawing.Size(25, 15);
			this.lblFds.TabIndex = 0;
			this.lblFds.Text = "FDS";
			// 
			// lblNamco
			// 
			this.lblNamco.AutoSize = true;
			this.lblNamco.BackColor = System.Drawing.Color.Transparent;
			this.lblNamco.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNamco.ForeColor = System.Drawing.Color.White;
			this.lblNamco.Location = new System.Drawing.Point(60, 0);
			this.lblNamco.Margin = new System.Windows.Forms.Padding(0);
			this.lblNamco.Name = "lblNamco";
			this.lblNamco.Size = new System.Drawing.Size(37, 15);
			this.lblNamco.TabIndex = 1;
			this.lblNamco.Text = "Namco";
			// 
			// lblSunsoft
			// 
			this.lblSunsoft.AutoSize = true;
			this.lblSunsoft.BackColor = System.Drawing.Color.Transparent;
			this.lblSunsoft.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSunsoft.ForeColor = System.Drawing.Color.White;
			this.lblSunsoft.Location = new System.Drawing.Point(97, 0);
			this.lblSunsoft.Margin = new System.Windows.Forms.Padding(0);
			this.lblSunsoft.Name = "lblSunsoft";
			this.lblSunsoft.Size = new System.Drawing.Size(37, 15);
			this.lblSunsoft.TabIndex = 5;
			this.lblSunsoft.Text = "Sunsoft";
			// 
			// lblVrc6
			// 
			this.lblVrc6.AutoSize = true;
			this.lblVrc6.BackColor = System.Drawing.Color.Transparent;
			this.lblVrc6.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVrc6.ForeColor = System.Drawing.Color.White;
			this.lblVrc6.Location = new System.Drawing.Point(134, 0);
			this.lblVrc6.Margin = new System.Windows.Forms.Padding(0);
			this.lblVrc6.Name = "lblVrc6";
			this.lblVrc6.Size = new System.Drawing.Size(32, 15);
			this.lblVrc6.TabIndex = 4;
			this.lblVrc6.Text = "VRC6";
			// 
			// lblVrc7
			// 
			this.lblVrc7.AutoSize = true;
			this.lblVrc7.BackColor = System.Drawing.Color.Transparent;
			this.lblVrc7.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVrc7.ForeColor = System.Drawing.Color.White;
			this.lblVrc7.Location = new System.Drawing.Point(166, 0);
			this.lblVrc7.Margin = new System.Windows.Forms.Padding(0);
			this.lblVrc7.Name = "lblVrc7";
			this.lblVrc7.Size = new System.Drawing.Size(32, 15);
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
			// picBackground
			// 
			this.picBackground.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.picBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.tableLayoutPanel1.SetColumnSpan(this.picBackground, 5);
			this.picBackground.Image = global::Mesen.GUI.Properties.Resources.NsfBackground;
			this.picBackground.Location = new System.Drawing.Point(66, 13);
			this.picBackground.Margin = new System.Windows.Forms.Padding(10);
			this.picBackground.MaximumSize = new System.Drawing.Size(334, 380);
			this.picBackground.Name = "picBackground";
			this.picBackground.Size = new System.Drawing.Size(238, 95);
			this.picBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picBackground.TabIndex = 5;
			this.picBackground.TabStop = false;
			// 
			// trkVolume
			// 
			this.trkVolume.Location = new System.Drawing.Point(257, 210);
			this.trkVolume.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			this.trkVolume.Maximum = 100;
			this.trkVolume.Name = "trkVolume";
			this.trkVolume.Size = new System.Drawing.Size(104, 45);
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
			this.cboTrack.SelectionChangeCommitted += new System.EventHandler(this.cboTrack_SelectionChangeCommitted);
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
			this.tableLayoutPanel1.SetColumnSpan(this.lblTime, 5);
			this.lblTime.ForeColor = System.Drawing.Color.White;
			this.lblTime.Location = new System.Drawing.Point(168, 258);
			this.lblTime.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(34, 13);
			this.lblTime.TabIndex = 10;
			this.lblTime.Text = "00:00";
			this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tmrFastForward
			// 
			this.tmrFastForward.Interval = 500;
			this.tmrFastForward.Tick += new System.EventHandler(this.tmrFastForward_Tick);
			// 
			// ctrlNsfPlayer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlNsfPlayer";
			this.Size = new System.Drawing.Size(371, 281);
			this.VisibleChanged += new System.EventHandler(this.ctrlNsfPlayer_VisibleChanged);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkVolume)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
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
	}
}
