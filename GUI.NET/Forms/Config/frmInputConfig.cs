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

namespace Mesen.GUI.Forms.Config
{
	public partial class frmInputConfig : BaseConfigForm
	{
		public frmInputConfig()
		{
			InitializeComponent();

			InteropEmu.UpdateInputDevices();

			Entity = ConfigManager.Config.InputInfo;

			if(ConfigManager.Config.InputInfo.AutoConfigureInput) {
				for(int i = 0; i < 4; i++) {
					ConfigManager.Config.InputInfo.Controllers[i].ControllerType = InteropEmu.GetControllerType(i);
				}
				ConfigManager.Config.InputInfo.ExpansionPortDevice = InteropEmu.GetExpansionDevice();
				ConfigManager.Config.InputInfo.ConsoleType = InteropEmu.GetConsoleType();
				ConfigManager.Config.InputInfo.UseFourScore = InteropEmu.HasFourScore();
			}

			AddBinding("ExpansionPortDevice", cboExpansionPort);
			AddBinding("ConsoleType", cboConsoleType);
			AddBinding("UseFourScore", chkFourScore);
			AddBinding("AutoConfigureInput", chkAutoConfigureInput);

			AddBinding("DisplayInputPort1", chkDisplayPort1);
			AddBinding("DisplayInputPort2", chkDisplayPort2);
			AddBinding("DisplayInputPort3", chkDisplayPort3);
			AddBinding("DisplayInputPort4", chkDisplayPort4);
			AddBinding("DisplayInputPosition", cboDisplayInputPosition);
			AddBinding("DisplayInputHorizontally", chkDisplayInputHorizontally);
		}

		protected override void AfterUpdateUI()
		{
			base.AfterUpdateUI();

			this.UpdateInterface();
		}

		private void UpdateAvailableControllerTypes()
		{
			bool isNes = ((InputInfo)Entity).ConsoleType == ConsoleType.Nes;
			bool p3and4visible = (isNes && chkFourScore.Checked) || (!isNes && ((InputInfo)Entity).ExpansionPortDevice == InteropEmu.ExpansionPortDevice.FourPlayerAdapter);

			List<InteropEmu.ControllerType> controllerTypes = new List<InteropEmu.ControllerType>(new InteropEmu.ControllerType[] { InteropEmu.ControllerType.StandardController });
			SetAvailableControllerTypes(cboPlayer3, controllerTypes.ToArray());
			SetAvailableControllerTypes(cboPlayer4, controllerTypes.ToArray());

			if(isNes && !chkFourScore.Checked) {
				controllerTypes.Add(InteropEmu.ControllerType.Zapper);
				controllerTypes.Add(InteropEmu.ControllerType.ArkanoidController);
			}
			SetAvailableControllerTypes(cboPlayer1, controllerTypes.ToArray());
			SetAvailableControllerTypes(cboPlayer2, controllerTypes.ToArray());
		}

		private void SetAvailableControllerTypes(ComboBox comboBox, InteropEmu.ControllerType[] controllerTypes)
		{
			object currentSelection = comboBox.SelectedItem;
			comboBox.Items.Clear();
			comboBox.Items.Add(ResourceHelper.GetEnumText(InteropEmu.ControllerType.None));
			foreach(InteropEmu.ControllerType type in controllerTypes) {
				comboBox.Items.Add(ResourceHelper.GetEnumText(type));
			}

			comboBox.SelectedItem = currentSelection;
			if(comboBox.SelectedIndex < 0) {
				comboBox.SelectedIndex = 0;
			}
		}

		protected override void UpdateConfig()
		{
			InputInfo inputInfo = (InputInfo)Entity;

			inputInfo.Controllers[0].ControllerType = cboPlayer1.GetEnumValue<InteropEmu.ControllerType>();
			inputInfo.Controllers[1].ControllerType = cboPlayer2.GetEnumValue<InteropEmu.ControllerType>();
			inputInfo.Controllers[2].ControllerType = cboPlayer3.GetEnumValue<InteropEmu.ControllerType>();
			inputInfo.Controllers[3].ControllerType = cboPlayer4.GetEnumValue<InteropEmu.ControllerType>();

			InputInfo.ApplyConfig();
		}

		private void UpdateInterface()
		{
			if(!this.Updating) {
				UpdateObject();
				bool isNes = ((InputInfo)Entity).ConsoleType == ConsoleType.Nes;
				cboExpansionPort.Visible = !isNes;
				lblExpansionPort.Visible = !isNes;
				chkFourScore.Visible = isNes;

				UpdatePlayer3And4Visibility();
				UpdateAvailableControllerTypes();

				cboPlayer1.SelectedItem = ResourceHelper.GetEnumText(ConfigManager.Config.InputInfo.Controllers[0].ControllerType);
				cboPlayer2.SelectedItem = ResourceHelper.GetEnumText(ConfigManager.Config.InputInfo.Controllers[1].ControllerType);
				cboPlayer3.SelectedItem = ResourceHelper.GetEnumText(ConfigManager.Config.InputInfo.Controllers[2].ControllerType);
				cboPlayer4.SelectedItem = ResourceHelper.GetEnumText(ConfigManager.Config.InputInfo.Controllers[3].ControllerType);
				
				if(ConfigManager.Config.PreferenceInfo.DisableGameDatabase) {
					//This option requires the game DB to be active
					chkAutoConfigureInput.Enabled = false;
					chkAutoConfigureInput.Checked = false;
				}
			}
		}

		private void UpdatePlayer3And4Visibility()
		{
			bool isNes = ((InputInfo)Entity).ConsoleType == ConsoleType.Nes;
			bool visible = (isNes && chkFourScore.Checked) || (!isNes && ((InputInfo)Entity).ExpansionPortDevice == InteropEmu.ExpansionPortDevice.FourPlayerAdapter);

			lblPlayer3.Visible = visible;
			lblPlayer4.Visible = visible;
			cboPlayer3.Visible = visible;
			cboPlayer4.Visible = visible;
			btnSetupP3.Visible = visible;
			btnSetupP4.Visible = visible;
		}

		private void cboNesType_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateInterface();
		}

		private void chkFourScore_CheckedChanged(object sender, EventArgs e)
		{
			UpdateInterface();
		}

		private void cboExpansionPort_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateInterface();
		}

		private void cboPlayerController_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool enableButton = (((ComboBox)sender).SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.StandardController)));
			if(sender == cboPlayer1) {
				btnSetupP1.Enabled = enableButton;
			} else if(sender == cboPlayer2) {
				btnSetupP2.Enabled = enableButton;
			} else if(sender == cboPlayer3) {
				btnSetupP3.Enabled = enableButton;
			} else if(sender == cboPlayer4) {
				btnSetupP4.Enabled = enableButton;
			}
		}

		private void btnSetup_Click(object sender, EventArgs e)
		{
			int index = 0;
			if(sender == btnSetupP1) {
				index = 0;
			} else if(sender == btnSetupP2) {
				index = 1;
			} else if(sender == btnSetupP3) {
				index = 2;
			} else if(sender == btnSetupP4) {
				index = 3;
			}
			var frm = new frmControllerConfig(ConfigManager.Config.InputInfo.Controllers[index]);

			Button btn = (Button)sender;
			Point point = btn.PointToScreen(new Point(0, btn.Height));
			Rectangle screen = Screen.FromControl(btn).Bounds;

			if(frm.Height + point.Y > screen.Bottom) {
				//Show on top instead
				point.Y -= btn.Height + frm.Height;
			}

			if(frm.Width + point.X > screen.Right) {
				//Show on left instead
				point.X -= frm.Width - btn.Width;
			}

			frm.StartPosition = FormStartPosition.Manual;
			frm.Top = point.Y;
			frm.Left = point.X;
			frm.ShowDialog(this);
		}
	}
}
