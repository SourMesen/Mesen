namespace Mesen.GUI.Forms
{
	partial class frmHelp
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHelp));
			this.tabCommandLineOptions = new System.Windows.Forms.TabControl();
			this.tpgGeneralOptions = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtGeneralOptions = new System.Windows.Forms.TextBox();
			this.grpExample = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblExample = new System.Windows.Forms.Label();
			this.lblExplanation = new System.Windows.Forms.Label();
			this.tpgVideoOptions = new System.Windows.Forms.TabPage();
			this.txtVideoOptions = new System.Windows.Forms.TextBox();
			this.tpgAudioOptions = new System.Windows.Forms.TabPage();
			this.txtAudioOptions = new System.Windows.Forms.TextBox();
			this.tpgEmulationOptions = new System.Windows.Forms.TabPage();
			this.txtEmulationOptions = new System.Windows.Forms.TextBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.tabCommandLineOptions.SuspendLayout();
			this.tpgGeneralOptions.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.grpExample.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tpgVideoOptions.SuspendLayout();
			this.tpgAudioOptions.SuspendLayout();
			this.tpgEmulationOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabCommandLineOptions
			// 
			this.tabCommandLineOptions.Controls.Add(this.tpgGeneralOptions);
			this.tabCommandLineOptions.Controls.Add(this.tpgVideoOptions);
			this.tabCommandLineOptions.Controls.Add(this.tpgAudioOptions);
			this.tabCommandLineOptions.Controls.Add(this.tpgEmulationOptions);
			this.tabCommandLineOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabCommandLineOptions.Location = new System.Drawing.Point(0, 0);
			this.tabCommandLineOptions.Name = "tabCommandLineOptions";
			this.tabCommandLineOptions.SelectedIndex = 0;
			this.tabCommandLineOptions.Size = new System.Drawing.Size(582, 379);
			this.tabCommandLineOptions.TabIndex = 0;
			// 
			// tpgGeneralOptions
			// 
			this.tpgGeneralOptions.Controls.Add(this.tableLayoutPanel1);
			this.tpgGeneralOptions.Location = new System.Drawing.Point(4, 22);
			this.tpgGeneralOptions.Name = "tpgGeneralOptions";
			this.tpgGeneralOptions.Size = new System.Drawing.Size(574, 353);
			this.tpgGeneralOptions.TabIndex = 2;
			this.tpgGeneralOptions.Text = "General";
			this.tpgGeneralOptions.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.txtGeneralOptions, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.grpExample, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(574, 353);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// txtGeneralOptions
			// 
			this.txtGeneralOptions.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtGeneralOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtGeneralOptions.Location = new System.Drawing.Point(3, 140);
			this.txtGeneralOptions.Multiline = true;
			this.txtGeneralOptions.Name = "txtGeneralOptions";
			this.txtGeneralOptions.ReadOnly = true;
			this.txtGeneralOptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtGeneralOptions.Size = new System.Drawing.Size(568, 210);
			this.txtGeneralOptions.TabIndex = 3;
			this.txtGeneralOptions.Text = "/fullscreen - Start Mesen in fullscreen mode\r\n/DoNotSaveSettings - Prevent settin" +
    "gs from being saved to the disk (useful to prevent command line options from bec" +
    "oming the default settings)";
			// 
			// grpExample
			// 
			this.grpExample.Controls.Add(this.tableLayoutPanel2);
			this.grpExample.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpExample.Location = new System.Drawing.Point(3, 3);
			this.grpExample.Name = "grpExample";
			this.grpExample.Size = new System.Drawing.Size(568, 131);
			this.grpExample.TabIndex = 2;
			this.grpExample.TabStop = false;
			this.grpExample.Text = "Usage Example";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.lblExample, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblExplanation, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(562, 112);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// lblExample
			// 
			this.lblExample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblExample.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblExample.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblExample.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.lblExample.Location = new System.Drawing.Point(3, 0);
			this.lblExample.Name = "lblExample";
			this.lblExample.Size = new System.Drawing.Size(556, 38);
			this.lblExample.TabIndex = 0;
			this.lblExample.Text = "Mesen C:\\Games\\MyGame.nes /fullscreen /VideoFilter=NTSC /VideoScale=2 /OverscanTo" +
    "p=8 /OverscanBottom=8 /OverscanLeft=0 /OverscanRight=0 /DoNotSaveSettings";
			// 
			// lblExplanation
			// 
			this.lblExplanation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblExplanation.Location = new System.Drawing.Point(3, 28);
			this.lblExplanation.Name = "lblExplanation";
			this.lblExplanation.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.lblExplanation.Size = new System.Drawing.Size(551, 44);
			this.lblExplanation.TabIndex = 1;
			this.lblExplanation.Text = resources.GetString("lblExplanation.Text");
			// 
			// tpgVideoOptions
			// 
			this.tpgVideoOptions.Controls.Add(this.txtVideoOptions);
			this.tpgVideoOptions.Location = new System.Drawing.Point(4, 22);
			this.tpgVideoOptions.Name = "tpgVideoOptions";
			this.tpgVideoOptions.Padding = new System.Windows.Forms.Padding(3);
			this.tpgVideoOptions.Size = new System.Drawing.Size(574, 353);
			this.tpgVideoOptions.TabIndex = 0;
			this.tpgVideoOptions.Text = "Video Options";
			this.tpgVideoOptions.UseVisualStyleBackColor = true;
			// 
			// txtVideoOptions
			// 
			this.txtVideoOptions.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtVideoOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtVideoOptions.Location = new System.Drawing.Point(3, 3);
			this.txtVideoOptions.Multiline = true;
			this.txtVideoOptions.Name = "txtVideoOptions";
			this.txtVideoOptions.ReadOnly = true;
			this.txtVideoOptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtVideoOptions.Size = new System.Drawing.Size(568, 347);
			this.txtVideoOptions.TabIndex = 0;
			// 
			// tpgAudioOptions
			// 
			this.tpgAudioOptions.Controls.Add(this.txtAudioOptions);
			this.tpgAudioOptions.Location = new System.Drawing.Point(4, 22);
			this.tpgAudioOptions.Name = "tpgAudioOptions";
			this.tpgAudioOptions.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAudioOptions.Size = new System.Drawing.Size(574, 353);
			this.tpgAudioOptions.TabIndex = 1;
			this.tpgAudioOptions.Text = "Audio Options";
			this.tpgAudioOptions.UseVisualStyleBackColor = true;
			// 
			// txtAudioOptions
			// 
			this.txtAudioOptions.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtAudioOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtAudioOptions.Location = new System.Drawing.Point(3, 3);
			this.txtAudioOptions.Multiline = true;
			this.txtAudioOptions.Name = "txtAudioOptions";
			this.txtAudioOptions.ReadOnly = true;
			this.txtAudioOptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtAudioOptions.Size = new System.Drawing.Size(568, 347);
			this.txtAudioOptions.TabIndex = 1;
			// 
			// tpgEmulationOptions
			// 
			this.tpgEmulationOptions.Controls.Add(this.txtEmulationOptions);
			this.tpgEmulationOptions.Location = new System.Drawing.Point(4, 22);
			this.tpgEmulationOptions.Name = "tpgEmulationOptions";
			this.tpgEmulationOptions.Padding = new System.Windows.Forms.Padding(3);
			this.tpgEmulationOptions.Size = new System.Drawing.Size(574, 353);
			this.tpgEmulationOptions.TabIndex = 3;
			this.tpgEmulationOptions.Text = "Emulation Options";
			this.tpgEmulationOptions.UseVisualStyleBackColor = true;
			// 
			// txtEmulationOptions
			// 
			this.txtEmulationOptions.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txtEmulationOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtEmulationOptions.Location = new System.Drawing.Point(3, 3);
			this.txtEmulationOptions.Multiline = true;
			this.txtEmulationOptions.Name = "txtEmulationOptions";
			this.txtEmulationOptions.ReadOnly = true;
			this.txtEmulationOptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtEmulationOptions.Size = new System.Drawing.Size(568, 347);
			this.txtEmulationOptions.TabIndex = 2;
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(1000, 1000);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(0, 0);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// frmHelp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(582, 379);
			this.Controls.Add(this.tabCommandLineOptions);
			this.Controls.Add(this.btnClose);
			this.MinimumSize = new System.Drawing.Size(598, 417);
			this.Name = "frmHelp";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Command-line options";
			this.tabCommandLineOptions.ResumeLayout(false);
			this.tpgGeneralOptions.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.grpExample.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tpgVideoOptions.ResumeLayout(false);
			this.tpgVideoOptions.PerformLayout();
			this.tpgAudioOptions.ResumeLayout(false);
			this.tpgAudioOptions.PerformLayout();
			this.tpgEmulationOptions.ResumeLayout(false);
			this.tpgEmulationOptions.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabCommandLineOptions;
		private System.Windows.Forms.TabPage tpgVideoOptions;
		private System.Windows.Forms.TextBox txtVideoOptions;
		private System.Windows.Forms.TabPage tpgAudioOptions;
		private System.Windows.Forms.TextBox txtAudioOptions;
		private System.Windows.Forms.TabPage tpgGeneralOptions;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.GroupBox grpExample;
		private System.Windows.Forms.Label lblExample;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label lblExplanation;
		private System.Windows.Forms.TextBox txtGeneralOptions;
		private System.Windows.Forms.TabPage tpgEmulationOptions;
		private System.Windows.Forms.TextBox txtEmulationOptions;
		private System.Windows.Forms.Button btnClose;
	}
}