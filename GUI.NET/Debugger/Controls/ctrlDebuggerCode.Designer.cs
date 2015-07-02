namespace Mesen.GUI.Debugger
{
	partial class ctrlDebuggerCode
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
			this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
			this.txtCode = new Mesen.GUI.Debugger.ctrlSyncTextBox();
			this.contextMenuCode = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuShowNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSetNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowOnlyDisassembledCode = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToLocation = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.txtCodeMargin = new Mesen.GUI.Debugger.ctrlSyncTextBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.mnuAddToWatch = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel11.SuspendLayout();
			this.contextMenuCode.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel11
			// 
			this.tableLayoutPanel11.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tableLayoutPanel11.ColumnCount = 2;
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 74F));
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel11.Controls.Add(this.txtCode, 1, 0);
			this.tableLayoutPanel11.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel11.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel11.Name = "tableLayoutPanel11";
			this.tableLayoutPanel11.RowCount = 1;
			this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel11.Size = new System.Drawing.Size(379, 218);
			this.tableLayoutPanel11.TabIndex = 3;
			// 
			// txtCode
			// 
			this.txtCode.BackColor = System.Drawing.Color.White;
			this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtCode.ContextMenuStrip = this.contextMenuCode;
			this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtCode.Font = new System.Drawing.Font("Consolas", 11F);
			this.txtCode.Location = new System.Drawing.Point(76, 1);
			this.txtCode.Margin = new System.Windows.Forms.Padding(0);
			this.txtCode.Name = "txtCode";
			this.txtCode.ReadOnly = true;
			this.txtCode.Size = new System.Drawing.Size(302, 216);
			this.txtCode.SyncedTextbox = null;
			this.txtCode.TabIndex = 0;
			this.txtCode.Text = "";
			this.txtCode.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtCode_MouseMove);
			this.txtCode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtCode_MouseUp);
			// 
			// contextMenuCode
			// 
			this.contextMenuCode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowNextStatement,
            this.mnuSetNextStatement,
            this.toolStripMenuItem1,
            this.mnuShowOnlyDisassembledCode,
            this.toolStripMenuItem2,
            this.mnuGoToLocation,
            this.mnuAddToWatch});
			this.contextMenuCode.Name = "contextMenuWatch";
			this.contextMenuCode.Size = new System.Drawing.Size(259, 148);
			this.contextMenuCode.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuCode_Opening);
			// 
			// mnuShowNextStatement
			// 
			this.mnuShowNextStatement.Name = "mnuShowNextStatement";
			this.mnuShowNextStatement.ShortcutKeyDisplayString = "Alt+*";
			this.mnuShowNextStatement.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Multiply)));
			this.mnuShowNextStatement.Size = new System.Drawing.Size(258, 22);
			this.mnuShowNextStatement.Text = "Show Next Statement";
			this.mnuShowNextStatement.Click += new System.EventHandler(this.mnuShowNextStatement_Click);
			// 
			// mnuSetNextStatement
			// 
			this.mnuSetNextStatement.Name = "mnuSetNextStatement";
			this.mnuSetNextStatement.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F10)));
			this.mnuSetNextStatement.Size = new System.Drawing.Size(258, 22);
			this.mnuSetNextStatement.Text = "Set Next Statement";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuShowOnlyDisassembledCode
			// 
			this.mnuShowOnlyDisassembledCode.Checked = true;
			this.mnuShowOnlyDisassembledCode.CheckOnClick = true;
			this.mnuShowOnlyDisassembledCode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mnuShowOnlyDisassembledCode.Name = "mnuShowOnlyDisassembledCode";
			this.mnuShowOnlyDisassembledCode.Size = new System.Drawing.Size(258, 22);
			this.mnuShowOnlyDisassembledCode.Text = "Show Only Disassembled Code";
			this.mnuShowOnlyDisassembledCode.Click += new System.EventHandler(this.mnuShowOnlyDisassembledCode_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(255, 6);
			// 
			// mnuGoToLocation
			// 
			this.mnuGoToLocation.Name = "mnuGoToLocation";
			this.mnuGoToLocation.Size = new System.Drawing.Size(258, 22);
			this.mnuGoToLocation.Text = "Go To Location";
			this.mnuGoToLocation.Click += new System.EventHandler(this.mnuGoToLocation_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.txtCodeMargin);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(1, 1);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(74, 216);
			this.panel1.TabIndex = 1;
			// 
			// txtCodeMargin
			// 
			this.txtCodeMargin.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtCodeMargin.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtCodeMargin.Dock = System.Windows.Forms.DockStyle.Left;
			this.txtCodeMargin.Font = new System.Drawing.Font("Consolas", 11F);
			this.txtCodeMargin.Location = new System.Drawing.Point(0, 0);
			this.txtCodeMargin.Margin = new System.Windows.Forms.Padding(0);
			this.txtCodeMargin.Name = "txtCodeMargin";
			this.txtCodeMargin.ReadOnly = true;
			this.txtCodeMargin.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.txtCodeMargin.Size = new System.Drawing.Size(90, 216);
			this.txtCodeMargin.SyncedTextbox = null;
			this.txtCodeMargin.TabIndex = 1;
			this.txtCodeMargin.Text = "";
			this.txtCodeMargin.Enter += new System.EventHandler(this.txtCodeMargin_Enter);
			// 
			// mnuAddToWatch
			// 
			this.mnuAddToWatch.Name = "mnuAddToWatch";
			this.mnuAddToWatch.Size = new System.Drawing.Size(258, 22);
			this.mnuAddToWatch.Text = "Add to Watch";
			this.mnuAddToWatch.Click += new System.EventHandler(this.mnuAddToWatch_Click);
			// 
			// ctrlDebuggerCode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel11);
			this.Name = "ctrlDebuggerCode";
			this.Size = new System.Drawing.Size(379, 218);
			this.tableLayoutPanel11.ResumeLayout(false);
			this.contextMenuCode.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
		private ctrlSyncTextBox txtCode;
		private System.Windows.Forms.Panel panel1;
		private ctrlSyncTextBox txtCodeMargin;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ContextMenuStrip contextMenuCode;
		private System.Windows.Forms.ToolStripMenuItem mnuShowNextStatement;
		private System.Windows.Forms.ToolStripMenuItem mnuSetNextStatement;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuShowOnlyDisassembledCode;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToLocation;
		private System.Windows.Forms.ToolStripMenuItem mnuAddToWatch;
	}
}
