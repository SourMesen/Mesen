namespace Mesen.GUI.Forms.Config
{
	partial class ctrlEmulatorShortcuts
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
			this.gridShortcuts = new System.Windows.Forms.DataGridView();
			this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colBinding1 = new System.Windows.Forms.DataGridViewButtonColumn();
			this.colBinding2 = new System.Windows.Forms.DataGridViewButtonColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridShortcuts)).BeginInit();
			this.SuspendLayout();
			// 
			// gridShortcuts
			// 
			this.gridShortcuts.AllowUserToAddRows = false;
			this.gridShortcuts.AllowUserToDeleteRows = false;
			this.gridShortcuts.AllowUserToResizeColumns = false;
			this.gridShortcuts.AllowUserToResizeRows = false;
			this.gridShortcuts.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
			this.gridShortcuts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridShortcuts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAction,
            this.colBinding1,
            this.colBinding2});
			this.gridShortcuts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridShortcuts.Location = new System.Drawing.Point(0, 0);
			this.gridShortcuts.MultiSelect = false;
			this.gridShortcuts.Name = "gridShortcuts";
			this.gridShortcuts.RowHeadersVisible = false;
			this.gridShortcuts.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.gridShortcuts.Size = new System.Drawing.Size(448, 248);
			this.gridShortcuts.TabIndex = 2;
			this.gridShortcuts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridShortcuts_CellContentClick);
			// 
			// colAction
			// 
			this.colAction.HeaderText = "Action";
			this.colAction.Name = "colAction";
			this.colAction.ReadOnly = true;
			this.colAction.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colAction.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.colAction.Width = 250;
			// 
			// colBinding1
			// 
			this.colBinding1.HeaderText = "Binding #1";
			this.colBinding1.Name = "colBinding1";
			this.colBinding1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colBinding1.Width = 110;
			// 
			// colBinding2
			// 
			this.colBinding2.HeaderText = "Binding #2";
			this.colBinding2.Name = "colBinding2";
			this.colBinding2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colBinding2.Width = 110;
			// 
			// ctrlEmulatorShortcuts
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridShortcuts);
			this.Name = "ctrlEmulatorShortcuts";
			this.Size = new System.Drawing.Size(448, 248);
			((System.ComponentModel.ISupportInitialize)(this.gridShortcuts)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.DataGridView gridShortcuts;
		private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
		private System.Windows.Forms.DataGridViewButtonColumn colBinding1;
		private System.Windows.Forms.DataGridViewButtonColumn colBinding2;
	}
}
