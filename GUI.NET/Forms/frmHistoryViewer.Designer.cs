namespace Mesen.GUI.Forms
{
	partial class frmHistoryViewer
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.trkPosition = new System.Windows.Forms.TrackBar();
			this.btnPausePlay = new System.Windows.Forms.Button();
			this.lblPosition = new System.Windows.Forms.Label();
			this.pnlRenderer = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.ctrlRenderer = new System.Windows.Forms.Panel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.lblVolume = new System.Windows.Forms.Label();
			this.trkVolume = new System.Windows.Forms.TrackBar();
			this.tmrUpdatePosition = new System.Windows.Forms.Timer(this.components);
			this.menuStrip2 = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImportMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExportMovie = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuResumeGameplay = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkPosition)).BeginInit();
			this.pnlRenderer.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkVolume)).BeginInit();
			this.menuStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.trkPosition, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnPausePlay, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPosition, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.pnlRenderer, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 3, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(532, 477);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// trkPosition
			// 
			this.trkPosition.Dock = System.Windows.Forms.DockStyle.Top;
			this.trkPosition.LargeChange = 10;
			this.trkPosition.Location = new System.Drawing.Point(56, 429);
			this.trkPosition.Name = "trkPosition";
			this.trkPosition.Size = new System.Drawing.Size(312, 45);
			this.trkPosition.TabIndex = 1;
			this.trkPosition.TickFrequency = 10;
			this.trkPosition.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.trkPosition.ValueChanged += new System.EventHandler(this.trkPosition_ValueChanged);
			// 
			// btnPausePlay
			// 
			this.btnPausePlay.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnPausePlay.Image = global::Mesen.GUI.Properties.Resources.Play;
			this.btnPausePlay.Location = new System.Drawing.Point(3, 433);
			this.btnPausePlay.Name = "btnPausePlay";
			this.btnPausePlay.Size = new System.Drawing.Size(47, 36);
			this.btnPausePlay.TabIndex = 2;
			this.btnPausePlay.Click += new System.EventHandler(this.btnPausePlay_Click);
			// 
			// lblPosition
			// 
			this.lblPosition.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblPosition.AutoSize = true;
			this.lblPosition.Location = new System.Drawing.Point(374, 445);
			this.lblPosition.MinimumSize = new System.Drawing.Size(49, 13);
			this.lblPosition.Name = "lblPosition";
			this.lblPosition.Size = new System.Drawing.Size(49, 13);
			this.lblPosition.TabIndex = 3;
			this.lblPosition.Text = "77:77:77";
			this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlRenderer
			// 
			this.pnlRenderer.BackColor = System.Drawing.Color.Black;
			this.pnlRenderer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tableLayoutPanel1.SetColumnSpan(this.pnlRenderer, 4);
			this.pnlRenderer.Controls.Add(this.tableLayoutPanel2);
			this.pnlRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRenderer.Location = new System.Drawing.Point(3, 3);
			this.pnlRenderer.Name = "pnlRenderer";
			this.pnlRenderer.Size = new System.Drawing.Size(526, 420);
			this.pnlRenderer.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.ctrlRenderer, 0, 0);
			this.tableLayoutPanel2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(524, 418);
			this.tableLayoutPanel2.TabIndex = 0;
			this.tableLayoutPanel2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ctrlRenderer_MouseClick);
			// 
			// ctrlRenderer
			// 
			this.ctrlRenderer.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ctrlRenderer.Location = new System.Drawing.Point(134, 89);
			this.ctrlRenderer.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlRenderer.Name = "ctrlRenderer";
			this.ctrlRenderer.Size = new System.Drawing.Size(256, 240);
			this.ctrlRenderer.TabIndex = 0;
			this.ctrlRenderer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ctrlRenderer_MouseClick);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.lblVolume, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.trkVolume, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(429, 429);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(100, 45);
			this.tableLayoutPanel3.TabIndex = 4;
			// 
			// lblVolume
			// 
			this.lblVolume.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblVolume.AutoSize = true;
			this.lblVolume.Location = new System.Drawing.Point(25, 31);
			this.lblVolume.MinimumSize = new System.Drawing.Size(49, 13);
			this.lblVolume.Name = "lblVolume";
			this.lblVolume.Size = new System.Drawing.Size(49, 13);
			this.lblVolume.TabIndex = 8;
			this.lblVolume.Text = "Volume";
			this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// trkVolume
			// 
			this.trkVolume.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trkVolume.Location = new System.Drawing.Point(0, 0);
			this.trkVolume.Margin = new System.Windows.Forms.Padding(0);
			this.trkVolume.Maximum = 100;
			this.trkVolume.Name = "trkVolume";
			this.trkVolume.Size = new System.Drawing.Size(100, 31);
			this.trkVolume.TabIndex = 7;
			this.trkVolume.TickFrequency = 10;
			this.trkVolume.ValueChanged += new System.EventHandler(this.trkVolume_ValueChanged);
			// 
			// tmrUpdatePosition
			// 
			this.tmrUpdatePosition.Interval = 150;
			this.tmrUpdatePosition.Tick += new System.EventHandler(this.tmrUpdatePosition_Tick);
			// 
			// menuStrip2
			// 
			this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
			this.menuStrip2.Location = new System.Drawing.Point(0, 0);
			this.menuStrip2.Name = "menuStrip2";
			this.menuStrip2.Size = new System.Drawing.Size(532, 24);
			this.menuStrip2.TabIndex = 1;
			this.menuStrip2.Text = "menuStrip2";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuImportMovie,
            this.mnuExportMovie,
            this.toolStripMenuItem1,
            this.mnuResumeGameplay,
            this.toolStripMenuItem2,
            this.mnuClose});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "File";
			this.mnuFile.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
			// 
			// mnuImportMovie
			// 
			this.mnuImportMovie.Image = global::Mesen.GUI.Properties.Resources.Import;
			this.mnuImportMovie.Name = "mnuImportMovie";
			this.mnuImportMovie.Size = new System.Drawing.Size(171, 22);
			this.mnuImportMovie.Text = "Import movie";
			// 
			// mnuExportMovie
			// 
			this.mnuExportMovie.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.mnuExportMovie.Name = "mnuExportMovie";
			this.mnuExportMovie.Size = new System.Drawing.Size(171, 22);
			this.mnuExportMovie.Text = "Export movie";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(168, 6);
			// 
			// mnuResumeGameplay
			// 
			this.mnuResumeGameplay.Image = global::Mesen.GUI.Properties.Resources.Play;
			this.mnuResumeGameplay.Name = "mnuResumeGameplay";
			this.mnuResumeGameplay.Size = new System.Drawing.Size(171, 22);
			this.mnuResumeGameplay.Text = "Resume gameplay";
			this.mnuResumeGameplay.Click += new System.EventHandler(this.mnuResumeGameplay_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(168, 6);
			// 
			// mnuClose
			// 
			this.mnuClose.Image = global::Mesen.GUI.Properties.Resources.Exit;
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(171, 22);
			this.mnuClose.Text = "Close";
			this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
			// 
			// frmHistoryViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(532, 501);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.menuStrip2);
			this.MinimumSize = new System.Drawing.Size(331, 384);
			this.Name = "frmHistoryViewer";
			this.Text = "History Viewer";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkPosition)).EndInit();
			this.pnlRenderer.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkVolume)).EndInit();
			this.menuStrip2.ResumeLayout(false);
			this.menuStrip2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel pnlRenderer;
		private System.Windows.Forms.TrackBar trkPosition;
		private System.Windows.Forms.Button btnPausePlay;
		private System.Windows.Forms.Timer tmrUpdatePosition;
		private System.Windows.Forms.Label lblPosition;
		private System.Windows.Forms.MenuStrip menuStrip2;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuImportMovie;
		private System.Windows.Forms.ToolStripMenuItem mnuExportMovie;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private System.Windows.Forms.ToolStripMenuItem mnuResumeGameplay;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Panel ctrlRenderer;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label lblVolume;
		private System.Windows.Forms.TrackBar trkVolume;
	}
}