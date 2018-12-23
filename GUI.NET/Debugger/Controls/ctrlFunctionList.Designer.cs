﻿namespace Mesen.GUI.Debugger.Controls
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
			this.components = new System.ComponentModel.Container();
			this.lstFunctions = new Mesen.GUI.Controls.DoubleBufferedListView();
			this.colFunctionLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFunctionAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colMemoryAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuEditLabel = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAddBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFindOccurrences = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstFunctions
			// 
			this.lstFunctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFunctionLabel,
            this.colFunctionAddress,
            this.colMemoryAddress});
			this.lstFunctions.ContextMenuStrip = this.contextMenuStrip;
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
			this.lstFunctions.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lstFunctions_RetrieveVirtualItem);
			this.lstFunctions.SearchForVirtualItem += new System.Windows.Forms.SearchForVirtualItemEventHandler(this.lstFunctions_SearchForVirtualItem);
			this.lstFunctions.SelectedIndexChanged += new System.EventHandler(this.lstFunctions_SelectedIndexChanged);
			this.lstFunctions.DoubleClick += new System.EventHandler(this.lstFunctions_DoubleClick);
			// 
			// colFunctionLabel
			// 
			this.colFunctionLabel.Text = "Function Name";
			this.colFunctionLabel.Width = 136;
			// 
			// colFunctionAddress
			// 
			this.colFunctionAddress.Text = "Address";
			this.colFunctionAddress.Width = 62;
			// 
			// colMemoryAddress
			// 
			this.colMemoryAddress.Text = "ROM Addr";
			this.colMemoryAddress.Width = 71;
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditLabel,
            this.toolStripMenuItem1,
            this.mnuAddBreakpoint,
            this.toolStripMenuItem2,
            this.mnuFindOccurrences});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(167, 82);
			// 
			// mnuEditLabel
			// 
			this.mnuEditLabel.Image = global::Mesen.GUI.Properties.Resources.EditLabel;
			this.mnuEditLabel.Name = "mnuEditLabel";
			this.mnuEditLabel.Size = new System.Drawing.Size(166, 22);
			this.mnuEditLabel.Text = "Edit Label";
			this.mnuEditLabel.Click += new System.EventHandler(this.mnuEditLabel_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(163, 6);
			// 
			// mnuAddBreakpoint
			// 
			this.mnuAddBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Breakpoint;
			this.mnuAddBreakpoint.Name = "mnuAddBreakpoint";
			this.mnuAddBreakpoint.Size = new System.Drawing.Size(166, 22);
			this.mnuAddBreakpoint.Text = "Add breakpoint";
			this.mnuAddBreakpoint.Click += new System.EventHandler(this.mnuAddBreakpoint_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(163, 6);
			// 
			// mnuFindOccurrences
			// 
			this.mnuFindOccurrences.Image = global::Mesen.GUI.Properties.Resources.Find;
			this.mnuFindOccurrences.Name = "mnuFindOccurrences";
			this.mnuFindOccurrences.Size = new System.Drawing.Size(166, 22);
			this.mnuFindOccurrences.Text = "Find Occurrences";
			this.mnuFindOccurrences.Click += new System.EventHandler(this.mnuFindOccurrences_Click);
			// 
			// ctrlFunctionList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lstFunctions);
			this.Name = "ctrlFunctionList";
			this.Size = new System.Drawing.Size(275, 112);
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Mesen.GUI.Controls.DoubleBufferedListView lstFunctions;
		private System.Windows.Forms.ColumnHeader colFunctionLabel;
		private System.Windows.Forms.ColumnHeader colFunctionAddress;
		private System.Windows.Forms.ColumnHeader colMemoryAddress;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem mnuEditLabel;
		private System.Windows.Forms.ToolStripMenuItem mnuFindOccurrences;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuAddBreakpoint;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
	}
}
