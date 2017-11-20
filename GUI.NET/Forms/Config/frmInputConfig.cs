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

			tlpControllers.Enabled = !InteropEmu.MoviePlaying() && !InteropEmu.MovieRecording();

			InteropEmu.UpdateInputDevices();

			Entity = ConfigManager.Config.InputInfo;

			for(int i = 0; i < 4; i++) {
				ConfigManager.Config.InputInfo.Controllers[i].ControllerType = InteropEmu.GetControllerType(i);
			}
			ConfigManager.Config.InputInfo.ExpansionPortDevice = InteropEmu.GetExpansionDevice();
			ConfigManager.Config.InputInfo.ConsoleType = InteropEmu.GetConsoleType();
			ConfigManager.Config.InputInfo.UseFourScore = InteropEmu.HasFourScore();

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

			UpdateConflictWarning();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			if(DialogResult == DialogResult.OK) {
				InputInfo.ApplyConfig();
			}
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

			List<InteropEmu.ControllerType> controllerTypes = new List<InteropEmu.ControllerType>() { InteropEmu.ControllerType.StandardController };
			if(!isNes) {
				controllerTypes.Add(InteropEmu.ControllerType.SnesMouse);
			}
			SetAvailableControllerTypes(cboPlayer3, controllerTypes.ToArray(), false);
			SetAvailableControllerTypes(cboPlayer4, controllerTypes.ToArray(), false);

			if(isNes) {
				if(!chkFourScore.Checked) {
					controllerTypes = new List<InteropEmu.ControllerType>() { InteropEmu.ControllerType.StandardController };
					controllerTypes.Add(InteropEmu.ControllerType.ArkanoidController);
					controllerTypes.Add(InteropEmu.ControllerType.PowerPad);
					controllerTypes.Add(InteropEmu.ControllerType.Zapper);
					controllerTypes.Add(InteropEmu.ControllerType.SnesController);
				}
				controllerTypes.Add(InteropEmu.ControllerType.SnesMouse);
				controllerTypes.Add(InteropEmu.ControllerType.SuborMouse);
			} else {
				controllerTypes = new List<InteropEmu.ControllerType>() { InteropEmu.ControllerType.StandardController };
			}

			bool isOriginalFamicom = !isNes && !ConfigManager.Config.EmulationInfo.UseNes101Hvc101Behavior;

			SetAvailableControllerTypes(cboPlayer1, controllerTypes.ToArray(), isOriginalFamicom);
			SetAvailableControllerTypes(cboPlayer2, controllerTypes.ToArray(), isOriginalFamicom);
		}

		private void SetAvailableControllerTypes(ComboBox comboBox, InteropEmu.ControllerType[] controllerTypes, bool forceController)
		{
			comboBox.Items.Clear();
			if(!forceController) {
				comboBox.Items.Add(ResourceHelper.GetEnumText(InteropEmu.ControllerType.None));
			}
			foreach(InteropEmu.ControllerType type in controllerTypes) {
				comboBox.Items.Add(ResourceHelper.GetEnumText(type));
			}

			InputInfo inputInfo = (InputInfo)Entity;
			string currentSelection = null;
			if(comboBox == cboPlayer1) {
				currentSelection = ResourceHelper.GetEnumText(inputInfo.Controllers[0].ControllerType);
			} else if(comboBox == cboPlayer2) {
				currentSelection = ResourceHelper.GetEnumText(inputInfo.Controllers[1].ControllerType);
			} else if(comboBox == cboPlayer3) {
				currentSelection = ResourceHelper.GetEnumText(inputInfo.Controllers[2].ControllerType);
			} else if(comboBox == cboPlayer4) {
				currentSelection = ResourceHelper.GetEnumText(inputInfo.Controllers[3].ControllerType);
			}

			if(currentSelection != null && comboBox.Items.Contains(currentSelection)) {
				comboBox.SelectedItem = currentSelection;
			} else {
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
		}

		private void UpdateInterface()
		{
			if(!this.Updating) {
				bool isNes = ((InputInfo)Entity).ConsoleType == ConsoleType.Nes;
				cboExpansionPort.Visible = !isNes;
				lblExpansionPort.Visible = !isNes;
				btnSetupExp.Visible = !isNes;
				chkFourScore.Visible = isNes;

				UpdatePlayer3And4Visibility();
				UpdateAvailableControllerTypes();
				
				if(ConfigManager.Config.PreferenceInfo.DisableGameDatabase) {
					//This option requires the game DB to be active
					chkAutoConfigureInput.Enabled = false;
					chkAutoConfigureInput.Checked = false;
				}

				UpdateConflictWarning();
			}
		}

		private bool FourScoreAttached
		{
			get
			{
				bool isNes = ((InputInfo)Entity).ConsoleType == ConsoleType.Nes;
				return (isNes && chkFourScore.Checked) || (!isNes && ((InputInfo)Entity).ExpansionPortDevice == InteropEmu.ExpansionPortDevice.FourPlayerAdapter);
			}
		}

		private void UpdatePlayer3And4Visibility()
		{
			bool visible = this.FourScoreAttached;

			if(lblPlayer3.Visible != visible) {
				lblPlayer3.Visible = visible;
				lblPlayer4.Visible = visible;
				cboPlayer3.Visible = visible;
				cboPlayer4.Visible = visible;
				btnSetupP3.Visible = visible;
				btnSetupP4.Visible = visible;
			}
		}

		private void cboNesType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(!this.Updating) {
				UpdateObject();
				UpdateInterface();
			}
		}

		private void chkFourScore_CheckedChanged(object sender, EventArgs e)
		{
			if(!this.Updating) {
				UpdateObject();
				UpdateInterface();
			}
		}

		private void cboExpansionPort_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(!this.Updating) {
				UpdateObject();
				UpdateInterface();
			}

			btnSetupExp.Enabled = cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.Zapper)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.FamilyTrainerMat));
		}

		private void cboPlayerController_SelectedIndexChanged(object sender, EventArgs e)
		{
			object selectedItem = ((ComboBox)sender).SelectedItem;

			bool enableButton = selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.StandardController)) ||
										selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.Zapper)) ||
										selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.SnesController)) ||
										selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.PowerPad));

			if(sender == cboPlayer1) {
				btnSetupP1.Enabled = enableButton;
			} else if(sender == cboPlayer2) {
				btnSetupP2.Enabled = enableButton;
			} else if(sender == cboPlayer3) {
				btnSetupP3.Enabled = enableButton;
			} else if(sender == cboPlayer4) {
				btnSetupP4.Enabled = enableButton;
			}
			UpdateConflictWarning();
		}

		private void btnSetup_Click(object sender, EventArgs e)
		{
			int index = 0;
			object selectedItem = null;
			if(sender == btnSetupP1) {
				selectedItem = cboPlayer1.GetEnumValue<InteropEmu.ControllerType>();
				index = 0;
			} else if(sender == btnSetupP2) {
				selectedItem = cboPlayer2.GetEnumValue<InteropEmu.ControllerType>();
				index = 1;
			} else if(sender == btnSetupP3) {
				selectedItem = cboPlayer3.GetEnumValue<InteropEmu.ControllerType>();
				index = 2;
			} else if(sender == btnSetupP4) {
				selectedItem = cboPlayer4.GetEnumValue<InteropEmu.ControllerType>();
				index = 3;
			} else if(sender == btnSetupExp) {
				selectedItem = cboExpansionPort.GetEnumValue<InteropEmu.ExpansionPortDevice>();
				index = 0;
			}

			Form frm = null;
			InputInfo inputInfo = (InputInfo)Entity;
			if(selectedItem is InteropEmu.ControllerType) {
				InteropEmu.ControllerType type = (InteropEmu.ControllerType)selectedItem;
				switch(type) {
					case InteropEmu.ControllerType.StandardController:
					case InteropEmu.ControllerType.SnesController:
					case InteropEmu.ControllerType.PowerPad:
						frm = new frmControllerConfig(inputInfo.Controllers[index], index, cboConsoleType.GetEnumValue<ConsoleType>(), type);
						break;

					case InteropEmu.ControllerType.Zapper:
						frm = new frmZapperConfig(inputInfo.Zapper);
						break;
				}
			} else if(selectedItem is InteropEmu.ExpansionPortDevice) {
				InteropEmu.ExpansionPortDevice device = (InteropEmu.ExpansionPortDevice)selectedItem;
				switch(device) {
					case InteropEmu.ExpansionPortDevice.FamilyTrainerMat:
						frm = new frmControllerConfig(inputInfo.Controllers[index], index, cboConsoleType.GetEnumValue<ConsoleType>(), InteropEmu.ControllerType.PowerPad);
						break;

					case InteropEmu.ExpansionPortDevice.Zapper:
						frm = new frmZapperConfig(inputInfo.Zapper);
						break;
				}
			}				

			if(frm != null) {
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
				if(frm.ShowDialog(this) == DialogResult.OK) {
					UpdateConflictWarning();
				}
				frm.Dispose();
			}
		}

		private void UpdateConflictWarning()
		{
			bool needWarning = false;
			bool[] portConflicts = new bool[4];
			Dictionary<uint, int> mappedKeys = new Dictionary<uint, int>();
			Action<int, uint> countMapping = (int port, uint keyCode) => {
				if(keyCode > 0) {
					if(mappedKeys.ContainsKey(keyCode)) {
						needWarning = true;
						portConflicts[port] = true;
						portConflicts[mappedKeys[keyCode]] = true;
					} else {
						mappedKeys.Add(keyCode, port);
					}
				}
			};

			for(int i = 0; i < 4; i++) {
				if(i < 2 || this.FourScoreAttached && ((i == 2 && btnSetupP3.Enabled) || (i == 3 && btnSetupP4.Enabled))) {
					foreach(KeyMappings mappings in ((InputInfo)Entity).Controllers[i].Keys) {
						countMapping(i, mappings.A);
						countMapping(i, mappings.B);
						countMapping(i, mappings.Select);
						countMapping(i, mappings.Start);
						countMapping(i, mappings.TurboA);
						countMapping(i, mappings.TurboB);
						countMapping(i, mappings.TurboSelect);
						countMapping(i, mappings.TurboStart);
						countMapping(i, mappings.Up);
						countMapping(i, mappings.Down);
						countMapping(i, mappings.Left);
						countMapping(i, mappings.Right);
					}
				}
			}

			if(pnlConflictWarning.Visible != needWarning) {
				pnlConflictWarning.Visible = needWarning;
				this.Height = (int)((needWarning ? 360 : 310) * _yFactor);
			}
			if(portConflicts[0] == (btnSetupP1.Image == null)) {
				btnSetupP1.Image = portConflicts[0] ? Properties.Resources.Warning : null;
			}
			if(portConflicts[0] == (btnSetupP2.Image == null)) {
				btnSetupP2.Image = portConflicts[1] ? Properties.Resources.Warning : null;
			}
			if(portConflicts[2] == (btnSetupP3.Image == null)) {
				btnSetupP3.Image = portConflicts[2] ? Properties.Resources.Warning : null;
			}
			if(portConflicts[3] == (btnSetupP4.Image == null)) {
				btnSetupP4.Image = portConflicts[3] ? Properties.Resources.Warning : null;
			}
		}

		float _yFactor = 1;
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			_yFactor = factor.Height;
			base.ScaleControl(factor, specified);
		}
	}
}
