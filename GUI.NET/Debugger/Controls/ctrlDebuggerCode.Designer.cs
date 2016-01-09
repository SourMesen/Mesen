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
			this.contextMenuCode = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuShowNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSetNextStatement = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowOnlyDisassembledCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowLineNotes = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowCodeNotes = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToLocation = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddToWatch = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.ctrlCodeViewer = new Mesen.GUI.Debugger.ctrlScrollableTextbox();
			this.contextMenuCode.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuCode
			// 
			this.contextMenuCode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowNextStatement,
            this.mnuSetNextStatement,
            this.toolStripMenuItem1,
            this.mnuShowOnlyDisassembledCode,
            this.mnuShowLineNotes,
            this.mnuShowCodeNotes,
            this.toolStripMenuItem2,
            this.mnuGoToLocation,
            this.mnuAddToWatch});
			this.contextMenuCode.Name = "contextMenuWatch";
			this.contextMenuCode.Size = new System.Drawing.Size(259, 170);
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
			this.mnuSetNextStatement.Click += new System.EventHandler(this.mnuSetNextStatement_Click);
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
			// mnuShowLineNotes
			// 
			this.mnuShowLineNotes.CheckOnClick = true;
			this.mnuShowLineNotes.Name = "mnuShowLineNotes";
			this.mnuShowLineNotes.Size = new System.Drawing.Size(258, 22);
			this.mnuShowLineNotes.Text = "Show PRG Addresses";
			this.mnuShowLineNotes.Click += new System.EventHandler(this.mnuShowLineNotes_Click);
			// 
			// mnuShowCodeNotes
			// 
			this.mnuShowCodeNotes.CheckOnClick = true;
			this.mnuShowCodeNotes.Name = "mnuShowCodeNotes";
			this.mnuShowCodeNotes.Size = new System.Drawing.Size(258, 22);
			this.mnuShowCodeNotes.Text = "Show Byte Code";
			this.mnuShowCodeNotes.Click += new System.EventHandler(this.mnuShowCodeNotes_Click);
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
			// mnuAddToWatch
			// 
			this.mnuAddToWatch.Name = "mnuAddToWatch";
			this.mnuAddToWatch.Size = new System.Drawing.Size(258, 22);
			this.mnuAddToWatch.Text = "Add to Watch";
			this.mnuAddToWatch.Click += new System.EventHandler(this.mnuAddToWatch_Click);
			// 
			// ctrlCodeViewer
			// 
			this.ctrlCodeViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ctrlCodeViewer.ContextMenuStrip = this.contextMenuCode;
			this.ctrlCodeViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCodeViewer.FontSize = 13F;
			this.ctrlCodeViewer.Location = new System.Drawing.Point(0, 0);
			this.ctrlCodeViewer.Name = "ctrlCodeViewer";
			this.ctrlCodeViewer.ShowContentNotes = false;
			this.ctrlCodeViewer.ShowLineNumberNotes = false;
			this.ctrlCodeViewer.Size = new System.Drawing.Size(379, 218);
			this.ctrlCodeViewer.TabIndex = 1;
			this.ctrlCodeViewer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseUp);
			this.ctrlCodeViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrlCodeViewer_MouseMove);
			// 
			// ctrlDebuggerCode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ctrlCodeViewer);
			this.Name = "ctrlDebuggerCode";
			this.Size = new System.Drawing.Size(379, 218);
			this.contextMenuCode.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ContextMenuStrip contextMenuCode;
		private System.Windows.Forms.ToolStripMenuItem mnuShowNextStatement;
		private System.Windows.Forms.ToolStripMenuItem mnuSetNextStatement;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuShowOnlyDisassembledCode;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToLocation;
		private System.Windows.Forms.ToolStripMenuItem mnuAddToWatch;
		private Mesen.GUI.Debugger.ctrlScrollableTextbox ctrlCodeViewer;
		private System.Windows.Forms.ToolStripMenuItem mnuShowLineNotes;
		private System.Windows.Forms.ToolStripMenuItem mnuShowCodeNotes;
	}
}
