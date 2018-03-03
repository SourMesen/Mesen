using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlAddressList : BaseScrollableTextboxUserControl
	{
		private int[] _addresses;

		public ctrlAddressList()
		{
			InitializeComponent();
			this.ctrlDataViewer.MarginWidth = 5;
		}

		protected override ctrlScrollableTextbox ScrollableTextbox
		{
			get { return this.ctrlDataViewer; }
		}
		
		public void SetData(byte[] values, int[] addresses)
		{
			this.ctrlDataViewer.Header = null;
			this.ctrlDataViewer.LineNumbers = addresses;
			this.ctrlDataViewer.LineIndentations = values.Select(val => 3).ToArray();
			this.ctrlDataViewer.TextLines = values.Select(val => val.ToString("X2")).ToArray();
			_addresses = addresses;
		}

		public int? CurrentAddress
		{
			get
			{
				if(_addresses?.Length > 0) {
					return _addresses[Math.Min(this.ctrlDataViewer.GetLineIndex(this.ctrlDataViewer.CurrentLine), _addresses.Length - 1)];
				} else {
					return null;
				}
			}
		}
	}
}
