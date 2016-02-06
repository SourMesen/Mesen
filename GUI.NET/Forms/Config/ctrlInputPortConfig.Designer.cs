namespace Mesen.GUI.Forms.Config
{
	partial class ctrlInputPortConfig
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
			this.ctrlStandardController = new Mesen.GUI.Forms.Config.ctrlStandardController();
			this.cboControllerType = new System.Windows.Forms.ComboBox();
			this.lblControllerType = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblControllerType, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.ctrlStandardController, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cboControllerType, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(618, 353);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// ctrlStandardController
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.ctrlStandardController, 2);
			this.ctrlStandardController.Location = new System.Drawing.Point(3, 30);
			this.ctrlStandardController.Name = "ctrlStandardController";
			this.ctrlStandardController.Size = new System.Drawing.Size(611, 320);
			this.ctrlStandardController.TabIndex = 3;
			// 
			// cboControllerType
			// 
			this.cboControllerType.Dock = System.Windows.Forms.DockStyle.Top;
			this.cboControllerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboControllerType.FormattingEnabled = true;
			this.cboControllerType.Items.AddRange(new object[] {
            "None",
            "Standard NES Controller",
            "Zapper"});
			this.cboControllerType.Location = new System.Drawing.Point(90, 3);
			this.cboControllerType.Name = "cboControllerType";
			this.cboControllerType.Size = new System.Drawing.Size(525, 21);
			this.cboControllerType.TabIndex = 2;
			this.cboControllerType.SelectedIndexChanged += new System.EventHandler(this.cboControllerType_SelectedIndexChanged);
			// 
			// lblControllerType
			// 
			this.lblControllerType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblControllerType.AutoSize = true;
			this.lblControllerType.Location = new System.Drawing.Point(3, 7);
			this.lblControllerType.Name = "lblControllerType";
			this.lblControllerType.Size = new System.Drawing.Size(81, 13);
			this.lblControllerType.TabIndex = 1;
			this.lblControllerType.Text = "Controller Type:";
			// 
			// ctrlInputPortConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlInputPortConfig";
			this.Size = new System.Drawing.Size(618, 353);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private ctrlStandardController ctrlStandardController;
		private System.Windows.Forms.Label lblControllerType;
		private System.Windows.Forms.ComboBox cboControllerType;
	}
}
