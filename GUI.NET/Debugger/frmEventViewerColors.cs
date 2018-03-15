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
		public static frmEventViewerColors Instance { get; private set; }
		public frmEventViewerColors()
		{
			InitializeComponent();

			Instance = this;

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

			Instance = null;
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			picWrite2000.BackColor = ColorTranslator.FromHtml("#FF5E5E");
			picWrite2001.BackColor = ColorTranslator.FromHtml("#8E33FF");
			picWrite2003.BackColor = ColorTranslator.FromHtml("#FF84E0");
			picWrite2004.BackColor = ColorTranslator.FromHtml("#FAFF39");
			picWrite2005.BackColor = ColorTranslator.FromHtml("#2EFF28");
			picWrite2006.BackColor = ColorTranslator.FromHtml("#3D2DFF");
			picWrite2007.BackColor = ColorTranslator.FromHtml("#FF060D");

			picRead2002.BackColor = ColorTranslator.FromHtml("#FF8224");
			picRead2004.BackColor = ColorTranslator.FromHtml("#24A672");
			picRead2007.BackColor = ColorTranslator.FromHtml("#6AF0FF");

			picMapperRead.BackColor = ColorTranslator.FromHtml("#C92929");
			picMapperWrite.BackColor = ColorTranslator.FromHtml("#007597");

			picNmi.BackColor = ColorTranslator.FromHtml("#ABADAC");
			picIrq.BackColor = ColorTranslator.FromHtml("#F9FEAC");
			picSpriteZeroHit.BackColor = ColorTranslator.FromHtml("#9F93C6");
			picBreakpoint.BackColor = ColorTranslator.FromHtml("#1898E4");
		}
	}
}
