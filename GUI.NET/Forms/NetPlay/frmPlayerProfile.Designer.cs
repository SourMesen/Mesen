namespace Mesen.GUI.Forms.NetPlay
{
	partial class frmPlayerProfile
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPlayerProfile));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblName = new System.Windows.Forms.Label();
			this.lblAvatar = new System.Windows.Forms.Label();
			this.txtPlayerName = new System.Windows.Forms.TextBox();
			this.picAvatar = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblName, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblAvatar, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtPlayerName, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.picAvatar, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(302, 163);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// flowLayoutPanel1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
			this.flowLayoutPanel1.Controls.Add(this.btnCancel);
			this.flowLayoutPanel1.Controls.Add(this.btnOK);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 134);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(302, 29);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(224, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(143, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// lblName
			// 
			this.lblName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(3, 6);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(68, 13);
			this.lblName.TabIndex = 3;
			this.lblName.Text = "Player name:";
			// 
			// lblAvatar
			// 
			this.lblAvatar.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAvatar.AutoSize = true;
			this.lblAvatar.Location = new System.Drawing.Point(3, 55);
			this.lblAvatar.Name = "lblAvatar";
			this.lblAvatar.Size = new System.Drawing.Size(41, 13);
			this.lblAvatar.TabIndex = 4;
			this.lblAvatar.Text = "Avatar:";
			// 
			// txtPlayerName
			// 
			this.txtPlayerName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPlayerName.Location = new System.Drawing.Point(77, 3);
			this.txtPlayerName.Name = "txtPlayerName";
			this.txtPlayerName.Size = new System.Drawing.Size(222, 20);
			this.txtPlayerName.TabIndex = 5;
			this.txtPlayerName.Text = "DefaultPlayer";
			// 
			// picAvatar
			// 
			this.picAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picAvatar.Location = new System.Drawing.Point(77, 29);
			this.picAvatar.Name = "picAvatar";
			this.picAvatar.Size = new System.Drawing.Size(66, 66);
			this.picAvatar.TabIndex = 8;
			this.picAvatar.TabStop = false;
			this.picAvatar.Click += new System.EventHandler(this.picAvatar_Click);
			// 
			// frmPlayerProfile
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(302, 163);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmPlayerProfile";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Profile";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblAvatar;
		private System.Windows.Forms.TextBox txtPlayerName;
		private System.Windows.Forms.PictureBox picAvatar;
	}
}