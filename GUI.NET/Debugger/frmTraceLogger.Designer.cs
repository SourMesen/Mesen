namespace Mesen.GUI.Debugger
{
	partial class frmTraceLogger
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnStartLogging = new System.Windows.Forms.Button();
			this.btnStopLogging = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.btnStopLogging, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnStartLogging, 0, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(57, 46);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(278, 144);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnStartLogging
			// 
			this.btnStartLogging.Location = new System.Drawing.Point(3, 3);
			this.btnStartLogging.Name = "btnStartLogging";
			this.btnStartLogging.Size = new System.Drawing.Size(95, 23);
			this.btnStartLogging.TabIndex = 0;
			this.btnStartLogging.Text = "Start Logging";
			this.btnStartLogging.UseVisualStyleBackColor = true;
			this.btnStartLogging.Click += new System.EventHandler(this.btnStartLogging_Click);
			// 
			// btnStopLogging
			// 
			this.btnStopLogging.Location = new System.Drawing.Point(142, 3);
			this.btnStopLogging.Name = "btnStopLogging";
			this.btnStopLogging.Size = new System.Drawing.Size(95, 23);
			this.btnStopLogging.TabIndex = 1;
			this.btnStopLogging.Text = "Stop Logging";
			this.btnStopLogging.UseVisualStyleBackColor = true;
			this.btnStopLogging.Click += new System.EventHandler(this.btnStopLogging_Click);
			// 
			// frmTraceLogger
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(546, 233);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "frmTraceLogger";
			this.Text = "Trace Logger";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnStopLogging;
		private System.Windows.Forms.Button btnStartLogging;

	}
}