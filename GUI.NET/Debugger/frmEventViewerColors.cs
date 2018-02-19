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
	public partial class frmEventViewerColors : BaseConfigForm
	{
		public frmEventViewerColors()
		{
			InitializeComponent();

			picMapperWrite.BackColor = ConfigManager.Config.DebugInfo.EventViewerMapperRegisterWriteColor;
			picMapperRead.BackColor = ConfigManager.Config.DebugInfo.EventViewerMapperRegisterReadColor;

			picNmi.BackColor = ConfigManager.Config.DebugInfo.EventViewerNmiColor;
			picIrq.BackColor = ConfigManager.Config.DebugInfo.EventViewerIrqColor;
			picSpriteZeroHit.BackColor = ConfigManager.Config.DebugInfo.EventViewerSpriteZeroHitColor;
			picBreakpoint.BackColor = ConfigManager.Config.DebugInfo.EventViewerBreakpointColor;

			picWrite2000.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[0];
			picWrite2001.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[1];
			picWrite2003.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[3];
			picWrite2004.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[4];
			picWrite2005.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[5];
			picWrite2006.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[6];
			picWrite2007.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[7];

			picRead2002.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterReadColors[2];
			picRead2004.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterReadColors[4];
			picRead2007.BackColor = ConfigManager.Config.DebugInfo.EventViewerPpuRegisterReadColors[7];
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
				ConfigManager.Config.DebugInfo.EventViewerMapperRegisterWriteColor = picMapperWrite.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerMapperRegisterReadColor = picMapperRead.BackColor;

				ConfigManager.Config.DebugInfo.EventViewerNmiColor = picNmi.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerIrqColor = picIrq.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerSpriteZeroHitColor = picSpriteZeroHit.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerBreakpointColor = picBreakpoint.BackColor;

				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[0] = picWrite2000.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[1] = picWrite2001.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[3] = picWrite2003.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[4] = picWrite2004.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[5] = picWrite2005.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[6] = picWrite2006.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterWriteColors[7] = picWrite2007.BackColor;

				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterReadColors[2] = picRead2002.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterReadColors[4] = picRead2004.BackColor;
				ConfigManager.Config.DebugInfo.EventViewerPpuRegisterReadColors[7] = picRead2007.BackColor;

				ConfigManager.ApplyChanges();
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			picWrite2000.BackColor = Color.FromArgb(93, 179, 255);
			picWrite2001.BackColor = Color.FromArgb(200, 144, 255);
			picWrite2003.BackColor = Color.FromArgb(255, 131, 192);
			picWrite2004.BackColor = Color.FromArgb(239, 154, 73);
			picWrite2005.BackColor = Color.FromArgb(133, 188, 47);
			picWrite2006.BackColor = Color.FromArgb(85, 199, 83);
			picWrite2007.BackColor = Color.FromArgb(60, 201, 140);

			picRead2002.BackColor = Color.FromArgb(143, 161, 255);
			picRead2004.BackColor = Color.FromArgb(247, 133, 250);
			picRead2007.BackColor = Color.FromArgb(255, 139, 127);

			picMapperRead.BackColor = Color.FromArgb(189, 172, 44);
			picMapperWrite.BackColor = Color.FromArgb(62, 194, 205);

			picNmi.BackColor = Color.FromArgb(0, 255, 50);
			picIrq.BackColor = Color.FromArgb(249, 254, 172);
			picSpriteZeroHit.BackColor = Color.FromArgb(255, 0, 100);
			picBreakpoint.BackColor = Color.FromArgb(200, 50, 200);
		}
	}
}
