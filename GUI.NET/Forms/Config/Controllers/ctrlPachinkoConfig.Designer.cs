namespace Mesen.GUI.Forms.Config
{
	partial class ctrlPachinkoConfig
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
			this.btn1 = new System.Windows.Forms.Button();
			this.btn2 = new System.Windows.Forms.Button();
			this.lblPress = new System.Windows.Forms.Label();
			this.lblRelease = new System.Windows.Forms.Label();
			this.btnClearKeys = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.btn1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.btn2, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPress, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblRelease, 2, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(185, 86);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// btn1
			// 
			this.btn1.Location = new System.Drawing.Point(26, 4);
			this.btn1.Name = "btn1";
			this.btn1.Size = new System.Drawing.Size(61, 59);
			this.btn1.TabIndex = 30;
			this.btn1.Text = "B";
			this.btn1.UseVisualStyleBackColor = true;
			// 
			// btn2
			// 
			this.btn2.Location = new System.Drawing.Point(93, 4);
			this.btn2.Name = "btn2";
			this.btn2.Size = new System.Drawing.Size(61, 59);
			this.btn2.TabIndex = 34;
			this.btn2.Text = "B";
			this.btn2.UseVisualStyleBackColor = true;
			// 
			// lblPress
			// 
			this.lblPress.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblPress.AutoSize = true;
			this.lblPress.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPress.Location = new System.Drawing.Point(32, 66);
			this.lblPress.Name = "lblPress";
			this.lblPress.Size = new System.Drawing.Size(49, 18);
			this.lblPress.TabIndex = 31;
			this.lblPress.Text = "Press";
			// 
			// lblRelease
			// 
			this.lblRelease.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblRelease.AutoSize = true;
			this.lblRelease.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRelease.Location = new System.Drawing.Point(93, 66);
			this.lblRelease.Name = "lblRelease";
			this.lblRelease.Size = new System.Drawing.Size(66, 18);
			this.lblRelease.TabIndex = 45;
			this.lblRelease.Text = "Release";
			// 
			// btnClearKeys
			// 
			this.btnClearKeys.AutoSize = true;
			this.btnClearKeys.Location = new System.Drawing.Point(3, 3);
			this.btnClearKeys.Name = "btnClearKeys";
			this.btnClearKeys.Size = new System.Drawing.Size(105, 23);
			this.btnClearKeys.TabIndex = 3;
			this.btnClearKeys.Text = "Clear Key Bindings";
			this.btnClearKeys.UseVisualStyleBackColor = true;
			// 
			// ctrlPachinkoConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlPachinkoConfig";
			this.Size = new System.Drawing.Size(185, 86);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btn1;
		private System.Windows.Forms.Button btn2;
		private System.Windows.Forms.Label lblPress;
		private System.Windows.Forms.Label lblRelease;
		private System.Windows.Forms.Button btnClearKeys;
	}
}