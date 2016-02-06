namespace Mesen.GUI.Forms.Config
{
	partial class ctrlStandardController
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlStandardController));
			this.panel2 = new System.Windows.Forms.Panel();
			this.picControllerLayout = new System.Windows.Forms.PictureBox();
			this.btnA = new System.Windows.Forms.Button();
			this.btnDown = new System.Windows.Forms.Button();
			this.btnUp = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnB = new System.Windows.Forms.Button();
			this.btnTurboA = new System.Windows.Forms.Button();
			this.btnLeft = new System.Windows.Forms.Button();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnRight = new System.Windows.Forms.Button();
			this.btnTurboB = new System.Windows.Forms.Button();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picControllerLayout)).BeginInit();
			this.SuspendLayout();
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnTurboB);
			this.panel2.Controls.Add(this.btnRight);
			this.panel2.Controls.Add(this.btnSelect);
			this.panel2.Controls.Add(this.btnLeft);
			this.panel2.Controls.Add(this.btnTurboA);
			this.panel2.Controls.Add(this.btnB);
			this.panel2.Controls.Add(this.btnStart);
			this.panel2.Controls.Add(this.btnUp);
			this.panel2.Controls.Add(this.btnDown);
			this.panel2.Controls.Add(this.btnA);
			this.panel2.Controls.Add(this.picControllerLayout);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(585, 210);
			this.panel2.TabIndex = 3;
			// 
			// picControllerLayout
			// 
			this.picControllerLayout.Image = ((System.Drawing.Image)(resources.GetObject("picControllerLayout.Image")));
			this.picControllerLayout.Location = new System.Drawing.Point(0, 0);
			this.picControllerLayout.Name = "picControllerLayout";
			this.picControllerLayout.Size = new System.Drawing.Size(615, 211);
			this.picControllerLayout.TabIndex = 11;
			this.picControllerLayout.TabStop = false;
			// 
			// btnA
			// 
			this.btnA.Location = new System.Drawing.Point(499, 105);
			this.btnA.Name = "btnA";
			this.btnA.Size = new System.Drawing.Size(61, 59);
			this.btnA.TabIndex = 14;
			this.btnA.Text = "A";
			this.btnA.UseVisualStyleBackColor = true;
			this.btnA.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// btnDown
			// 
			this.btnDown.Location = new System.Drawing.Point(68, 141);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(64, 35);
			this.btnDown.TabIndex = 19;
			this.btnDown.Text = "Down";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// btnUp
			// 
			this.btnUp.Location = new System.Drawing.Point(68, 34);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(64, 35);
			this.btnUp.TabIndex = 18;
			this.btnUp.Text = "Up";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(316, 131);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(61, 37);
			this.btnStart.TabIndex = 13;
			this.btnStart.Text = "W";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// btnB
			// 
			this.btnB.Location = new System.Drawing.Point(417, 105);
			this.btnB.Name = "btnB";
			this.btnB.Size = new System.Drawing.Size(61, 59);
			this.btnB.TabIndex = 15;
			this.btnB.Text = "B";
			this.btnB.UseVisualStyleBackColor = true;
			this.btnB.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// btnTurboA
			// 
			this.btnTurboA.Location = new System.Drawing.Point(499, 17);
			this.btnTurboA.Name = "btnTurboA";
			this.btnTurboA.Size = new System.Drawing.Size(61, 59);
			this.btnTurboA.TabIndex = 20;
			this.btnTurboA.Text = "A";
			this.btnTurboA.UseVisualStyleBackColor = true;
			this.btnTurboA.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// btnLeft
			// 
			this.btnLeft.Location = new System.Drawing.Point(29, 90);
			this.btnLeft.Name = "btnLeft";
			this.btnLeft.Size = new System.Drawing.Size(64, 35);
			this.btnLeft.TabIndex = 17;
			this.btnLeft.Text = "Left";
			this.btnLeft.UseVisualStyleBackColor = true;
			this.btnLeft.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(234, 131);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(61, 37);
			this.btnSelect.TabIndex = 12;
			this.btnSelect.Text = "Q";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// btnRight
			// 
			this.btnRight.Location = new System.Drawing.Point(108, 90);
			this.btnRight.Name = "btnRight";
			this.btnRight.Size = new System.Drawing.Size(64, 35);
			this.btnRight.TabIndex = 16;
			this.btnRight.Text = "Right";
			this.btnRight.UseVisualStyleBackColor = true;
			this.btnRight.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// btnTurboB
			// 
			this.btnTurboB.Location = new System.Drawing.Point(417, 17);
			this.btnTurboB.Name = "btnTurboB";
			this.btnTurboB.Size = new System.Drawing.Size(61, 59);
			this.btnTurboB.TabIndex = 21;
			this.btnTurboB.Text = "B";
			this.btnTurboB.UseVisualStyleBackColor = true;
			this.btnTurboB.Click += new System.EventHandler(this.btnMapping_Click);
			// 
			// ctrlStandardController
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel2);
			this.Name = "ctrlStandardController";
			this.Size = new System.Drawing.Size(585, 210);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picControllerLayout)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnTurboB;
		private System.Windows.Forms.Button btnRight;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnLeft;
		private System.Windows.Forms.Button btnTurboA;
		private System.Windows.Forms.Button btnB;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.Button btnA;
		private System.Windows.Forms.PictureBox picControllerLayout;
	}
}
