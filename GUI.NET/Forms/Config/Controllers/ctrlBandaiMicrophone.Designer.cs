namespace Mesen.GUI.Forms.Config
{
	partial class ctrlBandaiMicrophone
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
			this.btn3 = new System.Windows.Forms.Button();
			this.lblA = new System.Windows.Forms.Label();
			this.lblB = new System.Windows.Forms.Label();
			this.lblMicrophone = new System.Windows.Forms.Label();
			this.btnClearKeys = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
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
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.btn1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.btn2, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.btn3, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblA, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblB, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblMicrophone, 3, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(292, 86);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// btn1
			// 
			this.btn1.Location = new System.Drawing.Point(32, 4);
			this.btn1.Name = "btn1";
			this.btn1.Size = new System.Drawing.Size(61, 59);
			this.btn1.TabIndex = 30;
			this.btn1.Text = "B";
			this.btn1.UseVisualStyleBackColor = true;
			// 
			// btn2
			// 
			this.btn2.Location = new System.Drawing.Point(99, 4);
			this.btn2.Name = "btn2";
			this.btn2.Size = new System.Drawing.Size(61, 59);
			this.btn2.TabIndex = 34;
			this.btn2.Text = "B";
			this.btn2.UseVisualStyleBackColor = true;
			// 
			// btn3
			// 
			this.btn3.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btn3.Location = new System.Drawing.Point(182, 4);
			this.btn3.Name = "btn3";
			this.btn3.Size = new System.Drawing.Size(61, 59);
			this.btn3.TabIndex = 32;
			this.btn3.Text = "B";
			this.btn3.UseVisualStyleBackColor = true;
			// 
			// lblA
			// 
			this.lblA.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblA.AutoSize = true;
			this.lblA.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblA.Location = new System.Drawing.Point(54, 66);
			this.lblA.Name = "lblA";
			this.lblA.Size = new System.Drawing.Size(17, 18);
			this.lblA.TabIndex = 31;
			this.lblA.Text = "A";
			// 
			// lblB
			// 
			this.lblB.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblB.AutoSize = true;
			this.lblB.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblB.Location = new System.Drawing.Point(120, 66);
			this.lblB.Name = "lblB";
			this.lblB.Size = new System.Drawing.Size(19, 18);
			this.lblB.TabIndex = 45;
			this.lblB.Text = "B";
			// 
			// lblMicrophone
			// 
			this.lblMicrophone.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblMicrophone.AutoSize = true;
			this.lblMicrophone.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMicrophone.Location = new System.Drawing.Point(166, 66);
			this.lblMicrophone.Name = "lblMicrophone";
			this.lblMicrophone.Size = new System.Drawing.Size(93, 18);
			this.lblMicrophone.TabIndex = 44;
			this.lblMicrophone.Text = "Microphone";
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
			// ctrlBandaiMicrophone
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlBandaiMicrophone";
			this.Size = new System.Drawing.Size(292, 86);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btn1;
		private System.Windows.Forms.Button btn2;
		private System.Windows.Forms.Button btn3;
		private System.Windows.Forms.Label lblA;
		private System.Windows.Forms.Label lblB;
		private System.Windows.Forms.Label lblMicrophone;
		private System.Windows.Forms.Button btnClearKeys;
	}
}