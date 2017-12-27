using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmMemoryViewerColors : BaseConfigForm
	{
		public frmMemoryViewerColors()
		{
			InitializeComponent();

			picRead.BackColor = ConfigManager.Config.DebugInfo.RamReadColor;
			picWrite.BackColor = ConfigManager.Config.DebugInfo.RamWriteColor;
			picExecute.BackColor = ConfigManager.Config.DebugInfo.RamExecColor;
			picLabelledByte.BackColor = ConfigManager.Config.DebugInfo.RamLabelledByteColor;
			picCodeByte.BackColor = ConfigManager.Config.DebugInfo.RamCodeByteColor;
			picDataByte.BackColor = ConfigManager.Config.DebugInfo.RamDataByteColor;
			picChrDrawnByte.BackColor = ConfigManager.Config.DebugInfo.RamChrDrawnByteColor;
			picChrReadByte.BackColor = ConfigManager.Config.DebugInfo.RamChrReadByteColor;
		}

		private void picColorPicker_Click(object sender, EventArgs e)
		{
			using(ColorDialog cd = new ColorDialog()) {
				cd.SolidColorOnly = true;
				cd.AllowFullOpen = true;
				cd.FullOpen = true;
				cd.Color = ((PictureBox)sender).BackColor;
				if(cd.ShowDialog() == DialogResult.OK) {
					((PictureBox)sender).BackColor = cd.Color;
				}
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			if(DialogResult == DialogResult.OK) {
				ConfigManager.Config.DebugInfo.RamReadColor = picRead.BackColor;
				ConfigManager.Config.DebugInfo.RamWriteColor = picWrite.BackColor;
				ConfigManager.Config.DebugInfo.RamExecColor = picExecute.BackColor;
				ConfigManager.Config.DebugInfo.RamLabelledByteColor = picLabelledByte.BackColor;
				ConfigManager.Config.DebugInfo.RamCodeByteColor = picCodeByte.BackColor;
				ConfigManager.Config.DebugInfo.RamDataByteColor = picDataByte.BackColor;
				ConfigManager.Config.DebugInfo.RamChrDrawnByteColor = picChrDrawnByte.BackColor;
				ConfigManager.Config.DebugInfo.RamChrReadByteColor = picChrReadByte.BackColor;
				ConfigManager.ApplyChanges();
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			picRead.BackColor = Color.Blue;
			picWrite.BackColor = Color.Red;
			picExecute.BackColor = Color.Green;
			picLabelledByte.BackColor = Color.LightPink;
			picCodeByte.BackColor = Color.DarkSeaGreen;
			picDataByte.BackColor = Color.LightSteelBlue;
			picChrDrawnByte.BackColor = Color.DarkSeaGreen;
			picChrReadByte.BackColor = Color.LightSteelBlue;
		}
	}
}
