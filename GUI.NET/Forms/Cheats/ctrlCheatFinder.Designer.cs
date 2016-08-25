namespace Mesen.GUI.Forms.Cheats
{
	partial class ctrlCheatFinder
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
			this.components = new System.ComponentModel.Container();
			this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
			this.grpFilters = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblPreviousValue = new System.Windows.Forms.Label();
			this.cboPrevFilterType = new System.Windows.Forms.ComboBox();
			this.btnAddPrevFilter = new System.Windows.Forms.Button();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblCurrentValue = new System.Windows.Forms.Label();
			this.cboCurrentFilterType = new System.Windows.Forms.ComboBox();
			this.nudCurrentFilterValue = new System.Windows.Forms.NumericUpDown();
			this.btnAddCurrentFilter = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnUndo = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lstAddresses = new Mesen.GUI.Debugger.Controls.ctrlAddressList();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuCreateCheat = new System.Windows.Forms.ToolStripMenuItem();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnCreateCheat = new System.Windows.Forms.Button();
			this.lblAtAddress = new System.Windows.Forms.Label();
			this.lblAddress = new System.Windows.Forms.Label();
			this.chkPauseGameWhileWindowActive = new System.Windows.Forms.CheckBox();
			this.grpFilters.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCurrentFilterValue)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tmrRefresh
			// 
			this.tmrRefresh.Enabled = true;
			this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
			// 
			// grpFilters
			// 
			this.grpFilters.Controls.Add(this.tableLayoutPanel2);
			this.grpFilters.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpFilters.Location = new System.Drawing.Point(3, 3);
			this.grpFilters.Name = "grpFilters";
			this.grpFilters.Size = new System.Drawing.Size(399, 160);
			this.grpFilters.TabIndex = 4;
			this.grpFilters.TabStop = false;
			this.grpFilters.Text = "Filters";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel4, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.btnReset, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.btnUndo, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(393, 141);
			this.tableLayoutPanel2.TabIndex = 13;
			// 
			// flowLayoutPanel1
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.flowLayoutPanel1, 2);
			this.flowLayoutPanel1.Controls.Add(this.lblPreviousValue);
			this.flowLayoutPanel1.Controls.Add(this.cboPrevFilterType);
			this.flowLayoutPanel1.Controls.Add(this.btnAddPrevFilter);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 59);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(393, 31);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// lblPreviousValue
			// 
			this.lblPreviousValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPreviousValue.AutoSize = true;
			this.lblPreviousValue.Location = new System.Drawing.Point(3, 8);
			this.lblPreviousValue.Name = "lblPreviousValue";
			this.lblPreviousValue.Size = new System.Drawing.Size(99, 13);
			this.lblPreviousValue.TabIndex = 3;
			this.lblPreviousValue.Text = "Previous value was";
			// 
			// cboPrevFilterType
			// 
			this.cboPrevFilterType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cboPrevFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPrevFilterType.FormattingEnabled = true;
			this.cboPrevFilterType.Location = new System.Drawing.Point(108, 4);
			this.cboPrevFilterType.Name = "cboPrevFilterType";
			this.cboPrevFilterType.Size = new System.Drawing.Size(110, 21);
			this.cboPrevFilterType.TabIndex = 0;
			// 
			// btnAddPrevFilter
			// 
			this.btnAddPrevFilter.AutoSize = true;
			this.btnAddPrevFilter.Location = new System.Drawing.Point(224, 3);
			this.btnAddPrevFilter.Name = "btnAddPrevFilter";
			this.btnAddPrevFilter.Size = new System.Drawing.Size(75, 23);
			this.btnAddPrevFilter.TabIndex = 2;
			this.btnAddPrevFilter.Text = "Add Filter";
			this.btnAddPrevFilter.UseVisualStyleBackColor = true;
			this.btnAddPrevFilter.Click += new System.EventHandler(this.btnAddPrevFilter_Click);
			// 
			// flowLayoutPanel4
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.flowLayoutPanel4, 2);
			this.flowLayoutPanel4.Controls.Add(this.lblCurrentValue);
			this.flowLayoutPanel4.Controls.Add(this.cboCurrentFilterType);
			this.flowLayoutPanel4.Controls.Add(this.nudCurrentFilterValue);
			this.flowLayoutPanel4.Controls.Add(this.btnAddCurrentFilter);
			this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 29);
			this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(393, 30);
			this.flowLayoutPanel4.TabIndex = 0;
			// 
			// lblCurrentValue
			// 
			this.lblCurrentValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblCurrentValue.AutoSize = true;
			this.lblCurrentValue.Location = new System.Drawing.Point(3, 8);
			this.lblCurrentValue.Name = "lblCurrentValue";
			this.lblCurrentValue.Size = new System.Drawing.Size(80, 13);
			this.lblCurrentValue.TabIndex = 4;
			this.lblCurrentValue.Text = "Current value is";
			// 
			// cboCurrentFilterType
			// 
			this.cboCurrentFilterType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cboCurrentFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCurrentFilterType.FormattingEnabled = true;
			this.cboCurrentFilterType.Location = new System.Drawing.Point(89, 4);
			this.cboCurrentFilterType.Name = "cboCurrentFilterType";
			this.cboCurrentFilterType.Size = new System.Drawing.Size(110, 21);
			this.cboCurrentFilterType.TabIndex = 0;
			// 
			// nudCurrentFilterValue
			// 
			this.nudCurrentFilterValue.Location = new System.Drawing.Point(205, 5);
			this.nudCurrentFilterValue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.nudCurrentFilterValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudCurrentFilterValue.Name = "nudCurrentFilterValue";
			this.nudCurrentFilterValue.Size = new System.Drawing.Size(41, 20);
			this.nudCurrentFilterValue.TabIndex = 1;
			// 
			// btnAddCurrentFilter
			// 
			this.btnAddCurrentFilter.AutoSize = true;
			this.btnAddCurrentFilter.Location = new System.Drawing.Point(252, 3);
			this.btnAddCurrentFilter.Name = "btnAddCurrentFilter";
			this.btnAddCurrentFilter.Size = new System.Drawing.Size(75, 23);
			this.btnAddCurrentFilter.TabIndex = 2;
			this.btnAddCurrentFilter.Text = "Add Filter";
			this.btnAddCurrentFilter.UseVisualStyleBackColor = true;
			this.btnAddCurrentFilter.Click += new System.EventHandler(this.btnAddCurrentFilter_Click);
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(3, 3);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(75, 23);
			this.btnReset.TabIndex = 5;
			this.btnReset.Text = "Reset";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnUndo
			// 
			this.btnUndo.Location = new System.Drawing.Point(84, 3);
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Size = new System.Drawing.Size(75, 23);
			this.btnUndo.TabIndex = 11;
			this.btnUndo.Text = "Undo";
			this.btnUndo.UseVisualStyleBackColor = true;
			this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.grpFilters, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lstAddresses, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkPauseGameWhileWindowActive, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(565, 196);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// lstAddresses
			// 
			this.lstAddresses.ContextMenuStrip = this.contextMenuStrip;
			this.lstAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstAddresses.Location = new System.Drawing.Point(408, 3);
			this.lstAddresses.Name = "lstAddresses";
			this.lstAddresses.Size = new System.Drawing.Size(154, 160);
			this.lstAddresses.TabIndex = 3;
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCreateCheat});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(143, 26);
			this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
			// 
			// mnuCreateCheat
			// 
			this.mnuCreateCheat.Name = "mnuCreateCheat";
			this.mnuCreateCheat.Size = new System.Drawing.Size(142, 22);
			this.mnuCreateCheat.Text = "Create Cheat";
			this.mnuCreateCheat.Click += new System.EventHandler(this.btnCreateCheat_Click);
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.btnCreateCheat);
			this.flowLayoutPanel2.Controls.Add(this.lblAtAddress);
			this.flowLayoutPanel2.Controls.Add(this.lblAddress);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(405, 166);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(160, 30);
			this.flowLayoutPanel2.TabIndex = 5;
			// 
			// btnCreateCheat
			// 
			this.btnCreateCheat.Location = new System.Drawing.Point(3, 3);
			this.btnCreateCheat.Name = "btnCreateCheat";
			this.btnCreateCheat.Size = new System.Drawing.Size(85, 23);
			this.btnCreateCheat.TabIndex = 5;
			this.btnCreateCheat.Text = "Create Cheat";
			this.btnCreateCheat.UseVisualStyleBackColor = true;
			this.btnCreateCheat.Click += new System.EventHandler(this.btnCreateCheat_Click);
			// 
			// lblAtAddress
			// 
			this.lblAtAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAtAddress.AutoSize = true;
			this.lblAtAddress.Location = new System.Drawing.Point(94, 8);
			this.lblAtAddress.Name = "lblAtAddress";
			this.lblAtAddress.Size = new System.Drawing.Size(16, 13);
			this.lblAtAddress.TabIndex = 6;
			this.lblAtAddress.Text = "at";
			// 
			// lblAddress
			// 
			this.lblAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(116, 8);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(37, 13);
			this.lblAddress.TabIndex = 7;
			this.lblAddress.Text = "$0000";
			// 
			// chkPauseGameWhileWindowActive
			// 
			this.chkPauseGameWhileWindowActive.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkPauseGameWhileWindowActive.AutoSize = true;
			this.chkPauseGameWhileWindowActive.Location = new System.Drawing.Point(3, 172);
			this.chkPauseGameWhileWindowActive.Name = "chkPauseGameWhileWindowActive";
			this.chkPauseGameWhileWindowActive.Size = new System.Drawing.Size(278, 17);
			this.chkPauseGameWhileWindowActive.TabIndex = 6;
			this.chkPauseGameWhileWindowActive.Text = "Automatically pause game when this window is active";
			this.chkPauseGameWhileWindowActive.UseVisualStyleBackColor = true;
			this.chkPauseGameWhileWindowActive.CheckedChanged += new System.EventHandler(this.chkPauseGameWhileWindowActive_CheckedChanged);
			// 
			// ctrlCheatFinder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "ctrlCheatFinder";
			this.Size = new System.Drawing.Size(565, 196);
			this.grpFilters.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCurrentFilterValue)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.contextMenuStrip.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer tmrRefresh;
		private Debugger.Controls.ctrlAddressList lstAddresses;
		private System.Windows.Forms.GroupBox grpFilters;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnUndo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.ComboBox cboPrevFilterType;
		private System.Windows.Forms.Button btnAddPrevFilter;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.ComboBox cboCurrentFilterType;
		private System.Windows.Forms.NumericUpDown nudCurrentFilterValue;
		private System.Windows.Forms.Button btnAddCurrentFilter;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnCreateCheat;
		private System.Windows.Forms.Label lblPreviousValue;
		private System.Windows.Forms.Label lblCurrentValue;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label lblAtAddress;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem mnuCreateCheat;
		private System.Windows.Forms.CheckBox chkPauseGameWhileWindowActive;
	}
}
