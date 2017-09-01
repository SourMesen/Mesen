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
	public partial class frmAssemblerColors : BaseConfigForm
	{
		public frmAssemblerColors()
		{
			InitializeComponent();

			picOpcode.BackColor = ConfigManager.Config.DebugInfo.AssemblerOpcodeColor;
			picLabelDefinition.BackColor = ConfigManager.Config.DebugInfo.AssemblerLabelDefinitionColor;
			picImmediate.BackColor = ConfigManager.Config.DebugInfo.AssemblerImmediateColor;
			picAddress.BackColor = ConfigManager.Config.DebugInfo.AssemblerAddressColor;
			picComment.BackColor = ConfigManager.Config.DebugInfo.AssemblerCommentColor;
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
				ConfigManager.Config.DebugInfo.AssemblerOpcodeColor = picOpcode.BackColor;
				ConfigManager.Config.DebugInfo.AssemblerLabelDefinitionColor = picLabelDefinition.BackColor;
				ConfigManager.Config.DebugInfo.AssemblerImmediateColor = picImmediate.BackColor;
				ConfigManager.Config.DebugInfo.AssemblerAddressColor = picAddress.BackColor;
				ConfigManager.Config.DebugInfo.AssemblerCommentColor = picComment.BackColor;
				ConfigManager.ApplyChanges();
			}
		}
	}
}
