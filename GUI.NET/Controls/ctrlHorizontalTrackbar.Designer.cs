namespace Mesen.GUI.Controls
{
	partial class ctrlHorizontalTrackbar
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.trackBar = new System.Windows.Forms.TrackBar();
			this.txtValue = new System.Windows.Forms.TextBox();
			this.lblText = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.trackBar, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtValue, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblText, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(206, 55);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// trackBar
			// 
			this.trackBar.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.trackBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trackBar.Location = new System.Drawing.Point(0, 20);
			this.trackBar.Margin = new System.Windows.Forms.Padding(0);
			this.trackBar.Maximum = 100;
			this.trackBar.Minimum = -100;
			this.trackBar.Name = "trackBar";
			this.trackBar.Size = new System.Drawing.Size(172, 35);
			this.trackBar.TabIndex = 13;
			this.trackBar.TickFrequency = 10;
			this.trackBar.Value = 50;
			this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
			// 
			// txtValue
			// 
			this.txtValue.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtValue.BackColor = System.Drawing.Color.White;
			this.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtValue.Location = new System.Drawing.Point(175, 23);
			this.txtValue.Multiline = true;
			this.txtValue.Name = "txtValue";
			this.txtValue.ReadOnly = true;
			this.txtValue.Size = new System.Drawing.Size(28, 17);
			this.txtValue.TabIndex = 17;
			this.txtValue.TabStop = false;
			this.txtValue.Text = "100";
			this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblText
			// 
			this.lblText.AutoSize = true;
			this.lblText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblText.Location = new System.Drawing.Point(3, 0);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(166, 20);
			this.lblText.TabIndex = 18;
			this.lblText.Text = "Text";
			this.lblText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ctrlHorizontalTrackbar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.MaximumSize = new System.Drawing.Size(400, 55);
			this.MinimumSize = new System.Drawing.Size(206, 55);
			this.Name = "ctrlHorizontalTrackbar";
			this.Size = new System.Drawing.Size(206, 55);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TrackBar trackBar;
		private System.Windows.Forms.TextBox txtValue;
		private System.Windows.Forms.Label lblText;
	}
}
