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
			this.ctrlRenderer = new System.Windows.Forms.Panel();
			this.trkPosition = new System.Windows.Forms.TrackBar();
			this.btnPausePlay = new System.Windows.Forms.Button();
			this.lblPosition = new System.Windows.Forms.Label();
			this.tmrUpdatePosition = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkPosition)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.ctrlRenderer, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.trkPosition, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnPausePlay, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPosition, 2, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(520, 540);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// ctrlRenderer
			// 
			this.ctrlRenderer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tableLayoutPanel1.SetColumnSpan(this.ctrlRenderer, 3);
			this.ctrlRenderer.Location = new System.Drawing.Point(3, 3);
			this.ctrlRenderer.Name = "ctrlRenderer";
			this.ctrlRenderer.Size = new System.Drawing.Size(514, 482);
			this.ctrlRenderer.TabIndex = 0;
			// 
			// trkPosition
			// 
			this.trkPosition.Dock = System.Windows.Forms.DockStyle.Top;
			this.trkPosition.LargeChange = 10;
			this.trkPosition.Location = new System.Drawing.Point(56, 492);
			this.trkPosition.Name = "trkPosition";
			this.trkPosition.Size = new System.Drawing.Size(406, 45);
			this.trkPosition.TabIndex = 1;
			this.trkPosition.TickFrequency = 10;
			this.trkPosition.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.trkPosition.ValueChanged += new System.EventHandler(this.trkPosition_ValueChanged);
			// 
			// btnPausePlay
			// 
			this.btnPausePlay.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnPausePlay.Image = global::Mesen.GUI.Properties.Resources.Play;
			this.btnPausePlay.Location = new System.Drawing.Point(3, 496);
			this.btnPausePlay.Name = "btnPausePlay";
			this.btnPausePlay.Size = new System.Drawing.Size(47, 36);
			this.btnPausePlay.TabIndex = 2;
			this.btnPausePlay.Click += new System.EventHandler(this.btnPausePlay_Click);
			// 
			// lblPosition
			// 
			this.lblPosition.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblPosition.AutoSize = true;
			this.lblPosition.Location = new System.Drawing.Point(468, 508);
			this.lblPosition.MinimumSize = new System.Drawing.Size(49, 13);
			this.lblPosition.Name = "lblPosition";
			this.lblPosition.Size = new System.Drawing.Size(49, 13);
			this.lblPosition.TabIndex = 3;
			this.lblPosition.Text = "77:77:77";
			this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tmrUpdatePosition
			// 
			this.tmrUpdatePosition.Interval = 500;
			this.tmrUpdatePosition.Tick += new System.EventHandler(this.tmrUpdatePosition_Tick);
			// 
			// frmHistoryViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(520, 540);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmHistoryViewer";
			this.Text = "History Viewer";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkPosition)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel ctrlRenderer;
		private System.Windows.Forms.TrackBar trkPosition;
		private System.Windows.Forms.Button btnPausePlay;
		private System.Windows.Forms.Timer tmrUpdatePosition;
		private System.Windows.Forms.Label lblPosition;
	}
}