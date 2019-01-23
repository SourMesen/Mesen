using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlFindOccurrences : UserControl
	{
		public event EventHandler OnSearchResultsClosed;

		public ctrlFindOccurrences()
		{
			InitializeComponent();
			this.lstSearchResult.Font = new System.Drawing.Font(BaseControl.MonospaceFontFamily, 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		}

		public ICodeViewer Viewer { get; set; }

		public void FindAllOccurrences(string text, List<FindAllOccurrenceResult> results)
		{
			this.lstSearchResult.BeginUpdate();
			this.lstSearchResult.Items.Clear();
			foreach(FindAllOccurrenceResult searchResult in results) {
				var item = this.lstSearchResult.Items.Add(searchResult.Location);
				item.Tag = searchResult.Destination;
				item.SubItems.Add(searchResult.MatchedLine);
				item.SubItems.Add("");
			}
			this.lstSearchResult.EndUpdate();

			this.lblSearchResult.Text = $"Search results for: {text} ({this.lstSearchResult.Items.Count} results)";
			this.lstSearchResult.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			this.lstSearchResult.Columns[0].Width += 20;
			this.lstSearchResult.Columns[1].Width = Math.Max(this.lstSearchResult.Columns[1].Width, this.lstSearchResult.Width - this.lstSearchResult.Columns[0].Width - 24);
		}

		private void lstSearchResult_SizeChanged(object sender, EventArgs e)
		{
			this.lstSearchResult.SizeChanged -= lstSearchResult_SizeChanged;
			this.lstSearchResult.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			this.lstSearchResult.Columns[1].Width = Math.Max(this.lstSearchResult.Columns[1].Width, this.lstSearchResult.Width - this.lstSearchResult.Columns[0].Width - 24);
			this.lstSearchResult.SizeChanged += lstSearchResult_SizeChanged;
		}

		private void picCloseOccurrenceList_Click(object sender, EventArgs e)
		{
			OnSearchResultsClosed?.Invoke(null, EventArgs.Empty);
		}

		private void lstSearchResult_DoubleClick(object sender, EventArgs e)
		{
			if(lstSearchResult.SelectedItems.Count > 0) {
				GoToDestination dest = lstSearchResult.SelectedItems[0].Tag as GoToDestination;
				Viewer.CodeViewerActions.GoToDestination(dest);
			}
		}
	}

	public class FindAllOccurrenceResult
	{
		public string Location;
		public string MatchedLine;
		public GoToDestination Destination;
	}
}
