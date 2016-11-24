namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlFunctionList
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
			this.lstFunctions = new System.Windows.Forms.ListView();
			this.colFunctionLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFunctionAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colMemoryAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// lstFunctions
			// 
			this.lstFunctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFunctionLabel,
            this.colFunctionAddress,
            this.colMemoryAddress});
			this.lstFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstFunctions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstFunctions.FullRowSelect = true;
			this.lstFunctions.GridLines = true;
			this.lstFunctions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstFunctions.HideSelection = false;
			this.lstFunctions.Location = new System.Drawing.Point(0, 0);
			this.lstFunctions.MultiSelect = false;
			this.lstFunctions.Name = "lstFunctions";
			this.lstFunctions.Size = new System.Drawing.Size(275, 112);
			this.lstFunctions.TabIndex = 2;
			this.lstFunctions.UseCompatibleStateImageBehavior = false;
			this.lstFunctions.View = System.Windows.Forms.View.Details;
			this.lstFunctions.DoubleClick += new System.EventHandler(this.lstFunctions_DoubleClick);
			// 
			// colFunctionLabel
			// 
			this.colFunctionLabel.Text = "Label";
			this.colFunctionLabel.Width = 136;
			// 
			// colFunctionAddress
			// 
			this.colFunctionAddress.Text = "Address";
			this.colFunctionAddress.Width = 62;
			// 
			// colMemoryAddress
			// 
			this.colMemoryAddress.Text = "ROM Addr.";
			this.colMemoryAddress.Width = 71;
			// 
			// ctrlFunctionList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lstFunctions);
			this.Name = "ctrlFunctionList";
			this.Size = new System.Drawing.Size(275, 112);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lstFunctions;
		private System.Windows.Forms.ColumnHeader colFunctionLabel;
		private System.Windows.Forms.ColumnHeader colFunctionAddress;
		private System.Windows.Forms.ColumnHeader colMemoryAddress;
	}
}
