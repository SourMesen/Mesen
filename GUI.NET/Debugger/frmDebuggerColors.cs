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
	public partial class frmDebuggerColors : BaseConfigForm
	{
		public frmDebuggerColors()
		{
			InitializeComponent();

			picVerifiedData.BackColor = ConfigManager.Config.DebugInfo.CodeVerifiedDataColor;
			picUnidentifiedData.BackColor = ConfigManager.Config.DebugInfo.CodeUnidentifiedDataColor;
			picUnexecutedCode.BackColor = ConfigManager.Config.DebugInfo.CodeUnexecutedCodeColor;

			picExecBreakpoint.BackColor = ConfigManager.Config.DebugInfo.CodeExecBreakpointColor;
			picWriteBreakpoint.BackColor = ConfigManager.Config.DebugInfo.CodeWriteBreakpointColor;
			picReadBreakpoint.BackColor = ConfigManager.Config.DebugInfo.CodeReadBreakpointColor;
			picActiveStatement.BackColor = ConfigManager.Config.DebugInfo.CodeActiveStatementColor;
			picEffectiveAddress.BackColor = ConfigManager.Config.DebugInfo.CodeEffectiveAddressColor;

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
				ConfigManager.Config.DebugInfo.CodeVerifiedDataColor = picVerifiedData.BackColor;
				ConfigManager.Config.DebugInfo.CodeUnidentifiedDataColor = picUnidentifiedData.BackColor;
				ConfigManager.Config.DebugInfo.CodeUnexecutedCodeColor = picUnexecutedCode.BackColor;

				ConfigManager.Config.DebugInfo.CodeExecBreakpointColor = picExecBreakpoint.BackColor;
				ConfigManager.Config.DebugInfo.CodeWriteBreakpointColor = picWriteBreakpoint.BackColor;
				ConfigManager.Config.DebugInfo.CodeReadBreakpointColor = picReadBreakpoint.BackColor;
				ConfigManager.Config.DebugInfo.CodeActiveStatementColor = picActiveStatement.BackColor;
				ConfigManager.Config.DebugInfo.CodeEffectiveAddressColor = picEffectiveAddress.BackColor;

				ConfigManager.Config.DebugInfo.AssemblerOpcodeColor = picOpcode.BackColor;
				ConfigManager.Config.DebugInfo.AssemblerLabelDefinitionColor = picLabelDefinition.BackColor;
				ConfigManager.Config.DebugInfo.AssemblerImmediateColor = picImmediate.BackColor;
				ConfigManager.Config.DebugInfo.AssemblerAddressColor = picAddress.BackColor;
				ConfigManager.Config.DebugInfo.AssemblerCommentColor = picComment.BackColor;

				ConfigManager.ApplyChanges();
			}
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			picVerifiedData.BackColor = Color.FromArgb(255, 252, 236);
			picUnidentifiedData.BackColor = Color.FromArgb(255, 242, 242);
			picUnexecutedCode.BackColor = Color.FromArgb(225, 244, 228);

			picExecBreakpoint.BackColor = Color.FromArgb(140, 40, 40);
			picWriteBreakpoint.BackColor = Color.FromArgb(40, 120, 80);
			picReadBreakpoint.BackColor = Color.FromArgb(40, 40, 200);
			picActiveStatement.BackColor = Color.Yellow;
			picEffectiveAddress.BackColor = Color.SteelBlue;


			picOpcode.BackColor = Color.FromArgb(22, 37, 37);
			picLabelDefinition.BackColor = Color.Blue;
			picImmediate.BackColor = Color.Chocolate;
			picAddress.BackColor = Color.DarkRed;
			picComment.BackColor = Color.Green;
		}
	}
}
