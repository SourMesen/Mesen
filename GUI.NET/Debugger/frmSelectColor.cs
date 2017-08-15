using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmSelectColor : BaseForm
	{
		public int ColorIndex { get; private set; }

		public frmSelectColor()
		{
			InitializeComponent();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == Keys.Escape) {
				this.Close();
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			ctrlPaletteDisplay.ShowColorIndexes = true;
			ctrlPaletteDisplay.PaletteData = InteropEmu.GetRgbPalette();
		}

		private void ctrlPaletteDisplay_ColorClick(int colorIndex)
		{
			this.ColorIndex = colorIndex;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
