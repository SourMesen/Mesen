namespace Mesen.GUI.Debugger.Controls
{
	partial class ctrlFindOccurrences
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
			this.tlpSearchResults = new System.Windows.Forms.TableLayoutPanel();
			this.lstSearchResult = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.picCloseOccurrenceList = new System.Windows.Forms.PictureBox();
			this.lblSearchResult = new System.Windows.Forms.Label();
			this.tlpSearchResults.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picCloseOccurrenceList)).BeginInit();
			this.SuspendLayout();
			// 
			// tlpSearchResults
			// 
			this.tlpSearchResults.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tlpSearchResults.ColumnCount = 1;
			this.tlpSearchResults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpSearchResults.Controls.Add(this.lstSearchResult, 0, 1);
			this.tlpSearchResults.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tlpSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpSearchResults.Location = new System.Drawing.Point(0, 0);
			this.tlpSearchResults.Name = "tlpSearchResults";
			this.tlpSearchResults.RowCount = 2;
			this.tlpSearchResults.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpSearchResults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpSearchResults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpSearchResults.Size = new System.Drawing.Size(394, 136);
			this.tlpSearchResults.TabIndex = 13;
			// 
			// lstSearchResult
			// 
			this.lstSearchResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstSearchResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.lstSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstSearchResult.FullRowSelect = true;
			this.lstSearchResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lstSearchResult.Location = new System.Drawing.Point(1, 24);
			this.lstSearchResult.Margin = new System.Windows.Forms.Padding(0);
			this.lstSearchResult.Name = "lstSearchResult";
			this.lstSearchResult.Size = new System.Drawing.Size(392, 111);
			this.lstSearchResult.TabIndex = 9;
			this.lstSearchResult.UseCompatibleStateImageBehavior = false;
			this.lstSearchResult.View = System.Windows.Forms.View.Details;
			this.lstSearchResult.SizeChanged += new System.EventHandler(this.lstSearchResult_DoubleClick);
			this.lstSearchResult.DoubleClick += new System.EventHandler(this.lstSearchResult_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "";
			this.columnHeader2.Width = 160;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.picCloseOccurrenceList, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblSearchResult, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(392, 22);
			this.tableLayoutPanel2.TabIndex = 11;
			// 
			// picCloseOccurrenceList
			// 
			this.picCloseOccurrenceList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.picCloseOccurrenceList.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picCloseOccurrenceList.Image = global::Mesen.GUI.Properties.Resources.Close;
			this.picCloseOccurrenceList.Location = new System.Drawing.Point(373, 3);
			this.picCloseOccurrenceList.Name = "picCloseOccurrenceList";
			this.picCloseOccurrenceList.Size = new System.Drawing.Size(16, 16);
			this.picCloseOccurrenceList.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picCloseOccurrenceList.TabIndex = 10;
			this.picCloseOccurrenceList.TabStop = false;
			this.picCloseOccurrenceList.Click += new System.EventHandler(this.picCloseOccurrenceList_Click);
			// 
			// lblSearchResult
			// 
			this.lblSearchResult.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblSearchResult.AutoSize = true;
			this.lblSearchResult.Location = new System.Drawing.Point(3, 4);
			this.lblSearchResult.Name = "lblSearchResult";
			this.lblSearchResult.Size = new System.Drawing.Size(95, 13);
			this.lblSearchResult.TabIndex = 11;
			this.lblSearchResult.Text = "Search results for: ";
			// 
			// ctrlFindOccurrences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tlpSearchResults);
			this.Name = "ctrlFindOccurrences";
			this.Size = new System.Drawing.Size(394, 136);
			this.tlpSearchResults.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picCloseOccurrenceList)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlpSearchResults;
		private System.Windows.Forms.ListView lstSearchResult;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.PictureBox picCloseOccurrenceList;
		private System.Windows.Forms.Label lblSearchResult;
	}
}
