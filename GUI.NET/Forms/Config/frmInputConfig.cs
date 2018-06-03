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
		private bool _hasCartridgeInput = false;

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

			//Sort expansion port dropdown alphabetically, but keep the "None" option at the top
			SortDropdown(cboExpansionPort, ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.None));

			UpdateCartridgeInputUi();
			UpdateConflictWarning();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			btnSetupP1.Click += btnSetup_Click;
			btnSetupP2.Click += btnSetup_Click;
			btnSetupP3.Click += btnSetup_Click;
			btnSetupP4.Click += btnSetup_Click;
			
			cboConsoleType.SelectedIndexChanged += cboNesType_SelectedIndexChanged;
			cboExpansionPort.SelectedIndexChanged += cboExpansionPort_SelectedIndexChanged;
			chkFourScore.CheckedChanged += chkFourScore_CheckedChanged;
			btnSetupExp.Click += btnSetup_Click;
			btnSetupCartridge.Click += btnSetupCartridge_Click;
		}

		private void UpdateCartridgeInputUi()
		{
			ConsoleFeatures features = InteropEmu.GetAvailableFeatures();
			bool hasCartridgeInput = features.HasFlag(ConsoleFeatures.BandaiMicrophone) || features.HasFlag(ConsoleFeatures.DatachBarcodeReader);
			_hasCartridgeInput = hasCartridgeInput;
			lblCartridge.Visible = hasCartridgeInput;
			cboCartridge.Visible = hasCartridgeInput;
			btnSetupCartridge.Visible = hasCartridgeInput;
			btnSetupCartridge.Enabled = features.HasFlag(ConsoleFeatures.BandaiMicrophone);
			if(hasCartridgeInput) {
				if(features.HasFlag(ConsoleFeatures.BandaiMicrophone)) {
					cboCartridge.Items.Add(ResourceHelper.GetMessage("BandaiMicrophone"));
					cboCartridge.SelectedIndex = 0;
				} else if(features.HasFlag(ConsoleFeatures.DatachBarcodeReader)) {
					cboCartridge.Items.Add(ResourceHelper.GetMessage("DatachBarcodeReader"));
					cboCartridge.SelectedIndex = 0;
				}
				this.Height += (int)(30 * _yFactor);
			}
		}

		private void SortDropdown(ComboBox dropdown, string optionAtTop)
		{
			dropdown.Sorted = true;
			dropdown.Sorted = false;
			int index = dropdown.FindStringExact(optionAtTop);

			if(index >= 0) {
				object topOption = dropdown.Items[index];
				dropdown.Items.RemoveAt(index);
				dropdown.Items.Insert(0, topOption);
			}
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
			cboPlayer1.SelectedIndexChanged -= cboPlayerController_SelectedIndexChanged;
			cboPlayer2.SelectedIndexChanged -= cboPlayerController_SelectedIndexChanged;
			cboPlayer3.SelectedIndexChanged -= cboPlayerController_SelectedIndexChanged;
			cboPlayer4.SelectedIndexChanged -= cboPlayerController_SelectedIndexChanged;

			bool isNes = ((InputInfo)Entity).ConsoleType == ConsoleType.Nes;
			bool p3and4visible = (isNes && chkFourScore.Checked) || (!isNes && ((InputInfo)Entity).ExpansionPortDevice == InteropEmu.ExpansionPortDevice.FourPlayerAdapter);
			bool isOriginalFamicom = !isNes && !ConfigManager.Config.EmulationInfo.UseNes101Hvc101Behavior;

			List<InteropEmu.ControllerType> controllerTypes = new List<InteropEmu.ControllerType>() { InteropEmu.ControllerType.None, InteropEmu.ControllerType.StandardController };
			controllerTypes.Add(InteropEmu.ControllerType.SnesMouse);
			controllerTypes.Add(InteropEmu.ControllerType.SuborMouse);
			if(!isNes) {
				controllerTypes.Add(InteropEmu.ControllerType.SnesController);
			}
			SetAvailableControllerTypes(cboPlayer3, controllerTypes.ToArray());
			SetAvailableControllerTypes(cboPlayer4, controllerTypes.ToArray());

			if(isOriginalFamicom) {
				//Original famicom only allows standard controllers in port 1/2
				controllerTypes = new List<InteropEmu.ControllerType>() { InteropEmu.ControllerType.StandardController };
			} else if(isNes && !chkFourScore.Checked) {
				//These use more than just bit 0, and won't work on the four score
				controllerTypes.Add(InteropEmu.ControllerType.ArkanoidController);
				controllerTypes.Add(InteropEmu.ControllerType.PowerPad);
				controllerTypes.Add(InteropEmu.ControllerType.Zapper);
				controllerTypes.Add(InteropEmu.ControllerType.SnesController);
			}

			SetAvailableControllerTypes(cboPlayer1, controllerTypes.ToArray());
			SetAvailableControllerTypes(cboPlayer2, controllerTypes.ToArray());

			cboPlayer1.SelectedIndexChanged += cboPlayerController_SelectedIndexChanged;
			cboPlayer2.SelectedIndexChanged += cboPlayerController_SelectedIndexChanged;
			cboPlayer3.SelectedIndexChanged += cboPlayerController_SelectedIndexChanged;
			cboPlayer4.SelectedIndexChanged += cboPlayerController_SelectedIndexChanged;
		}

		private void SetAvailableControllerTypes(ComboBox comboBox, InteropEmu.ControllerType[] controllerTypes)
		{
			comboBox.Items.Clear();
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

			SortDropdown(comboBox, ResourceHelper.GetEnumText(InteropEmu.ControllerType.None));

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
			bool isNes = ((InputInfo)Entity).ConsoleType == ConsoleType.Nes;
			cboExpansionPort.Visible = !isNes;
			lblExpansionPort.Visible = !isNes;
			btnSetupExp.Visible = !isNes;
			chkFourScore.Visible = isNes;

			UpdatePlayer3And4Visibility();
			UpdateAvailableControllerTypes();
			UpdateSetupButtons();
				
			if(ConfigManager.Config.PreferenceInfo.DisableGameDatabase) {
				//This option requires the game DB to be active
				chkAutoConfigureInput.Enabled = false;
				chkAutoConfigureInput.Checked = false;
			}

			UpdateConflictWarning();
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
			UpdateObject();
			if(((InputInfo)Entity).ConsoleType == ConsoleType.Famicom) {
				((InputInfo)Entity).Controllers[0].ControllerType = InteropEmu.ControllerType.StandardController;
				((InputInfo)Entity).Controllers[1].ControllerType = InteropEmu.ControllerType.StandardController;
			}
			UpdateInterface();
		}

		private void chkFourScore_CheckedChanged(object sender, EventArgs e)
		{
			UpdateObject();
			UpdateInterface();
		}

		private void cboExpansionPort_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateObject();
			UpdateInterface();
		}

		private void UpdateSetupButtons()
		{
			List<ComboBox> dropdowns = new List<ComboBox>() { cboPlayer1, cboPlayer2, cboPlayer3, cboPlayer4 };
			List<Button> buttons = new List<Button>() { btnSetupP1, btnSetupP2, btnSetupP3, btnSetupP4 };

			for(int i = 0; i < 4; i++) {
				object selectedItem = dropdowns[i].SelectedItem;
				bool enableButton = selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.StandardController)) ||
											selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.Zapper)) ||
											selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.SnesController)) ||
											selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.SnesMouse)) ||
											selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.SuborMouse)) ||
											selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.ArkanoidController)) ||
											selectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ControllerType.PowerPad));

				buttons[i].Enabled = enableButton;
			}

			btnSetupExp.Enabled = cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.Zapper)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.FamilyTrainerMat)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.ExcitingBoxing)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.JissenMahjong)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.FamilyBasicKeyboard)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.SuborKeyboard)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.Pachinko)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.ArkanoidController)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.HoriTrack)) ||
										cboExpansionPort.SelectedItem.Equals(ResourceHelper.GetEnumText(InteropEmu.ExpansionPortDevice.PartyTap));
		}

		private void cboPlayerController_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateObject();
			UpdateInterface();
		}

		private void btnSetup_Click(object sender, EventArgs e)
		{
			int index = 0;
			object selectedItem = null;
			string selectedText = "";
			if(sender == btnSetupP1) {
				selectedItem = cboPlayer1.GetEnumValue<InteropEmu.ControllerType>();
				selectedText = cboPlayer1.SelectedItem.ToString();
				index = 0;
			} else if(sender == btnSetupP2) {
				selectedItem = cboPlayer2.GetEnumValue<InteropEmu.ControllerType>();
				selectedText = cboPlayer2.SelectedItem.ToString();
				index = 1;
			} else if(sender == btnSetupP3) {
				selectedItem = cboPlayer3.GetEnumValue<InteropEmu.ControllerType>();
				selectedText = cboPlayer3.SelectedItem.ToString();
				index = 2;
			} else if(sender == btnSetupP4) {
				selectedItem = cboPlayer4.GetEnumValue<InteropEmu.ControllerType>();
				selectedText = cboPlayer4.SelectedItem.ToString();
				index = 3;
			} else if(sender == btnSetupExp) {
				selectedItem = cboExpansionPort.GetEnumValue<InteropEmu.ExpansionPortDevice>();
				selectedText = cboExpansionPort.SelectedItem.ToString();
				index = 0;
			}

			Form frm = null;
			InputInfo inputInfo = (InputInfo)Entity;
			if(selectedItem is InteropEmu.ControllerType) {
				InteropEmu.ControllerType type = (InteropEmu.ControllerType)selectedItem;
				switch(type) {
					case InteropEmu.ControllerType.StandardController:
					case InteropEmu.ControllerType.SnesController:
						frm = new frmControllerConfig(inputInfo.Controllers[index], index, cboConsoleType.GetEnumValue<ConsoleType>(), type);
						break;

					case InteropEmu.ControllerType.PowerPad:
						frm = new frmPowerPadConfig(inputInfo.Controllers[index], index);
						break;

					case InteropEmu.ControllerType.Zapper:
						frm = new frmZapperConfig(inputInfo.Zapper);
						break;

					case InteropEmu.ControllerType.SnesMouse:
						frm = new frmMouseConfig(inputInfo.SnesMouse);
						break;

					case InteropEmu.ControllerType.SuborMouse:
						frm = new frmMouseConfig(inputInfo.SuborMouse);
						break;

					case InteropEmu.ControllerType.ArkanoidController:
						frm = new frmMouseConfig(inputInfo.ArkanoidController);
						break;
				}
			} else if(selectedItem is InteropEmu.ExpansionPortDevice) {
				InteropEmu.ExpansionPortDevice device = (InteropEmu.ExpansionPortDevice)selectedItem;
				switch(device) {
					case InteropEmu.ExpansionPortDevice.FamilyTrainerMat:
						frm = new frmPowerPadConfig(inputInfo.Controllers[index], index);
						break;

					case InteropEmu.ExpansionPortDevice.PartyTap:
						frm = new frmPartytapConfig(inputInfo.Controllers[index]);
						break;

					case InteropEmu.ExpansionPortDevice.Pachinko:
						frm = new frmPachinkoConfig(inputInfo.Controllers[index]);
						break;

					case InteropEmu.ExpansionPortDevice.ExcitingBoxing:
						frm = new frmExcitingBoxingConfig(inputInfo.Controllers[index]);
						break;

					case InteropEmu.ExpansionPortDevice.JissenMahjong:
						frm = new frmJissenMahjongConfig(inputInfo.Controllers[index]);
						break;

					case InteropEmu.ExpansionPortDevice.FamilyBasicKeyboard:
						frm = new frmFamilyBasicKeyboardConfig(inputInfo.Controllers[index]);
						break;

					case InteropEmu.ExpansionPortDevice.SuborKeyboard:
						frm = new frmSuborKeyboardConfig(inputInfo.Controllers[index]);
						break;

					case InteropEmu.ExpansionPortDevice.Zapper:
					case InteropEmu.ExpansionPortDevice.BandaiHyperShot:
						frm = new frmZapperConfig(inputInfo.Zapper);
						break;

					case InteropEmu.ExpansionPortDevice.HoriTrack:
						frm = new frmMouseConfig(inputInfo.HoriTrack);
						break;

					case InteropEmu.ExpansionPortDevice.ArkanoidController:
						frm = new frmMouseConfig(inputInfo.ArkanoidController);
						break;
				}
			}				

			if(frm != null) {
				OpenSetupWindow(frm, (Button)sender, selectedText);
			}
		}

		private void OpenSetupWindow(Form frm, Button btn, string title)
		{
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

			frm.Text = title;
			frm.StartPosition = FormStartPosition.Manual;
			frm.Top = point.Y;
			frm.Left = point.X;
			if(frm.ShowDialog(this) == DialogResult.OK) {
				UpdateInterface();
			}
			frm.Dispose();
		}
		
		private void btnSetupCartridge_Click(object sender, EventArgs e)
		{
			Form frm = new frmBandaiMicrophone(((InputInfo)Entity).Controllers[0]);
			OpenSetupWindow(frm, (Button)sender, cboCartridge.SelectedItem.ToString());
		}

		private void UpdateConflictWarning()
		{
			bool needWarning = false;
			bool[] portConflicts = new bool[6];
			Dictionary<uint, List<int>> mappedKeys = new Dictionary<uint, List<int>>();
			Action<int, uint> countMapping = (int port, uint keyCode) => {
				if(keyCode > 0) {
					if(mappedKeys.ContainsKey(keyCode)) {
						needWarning = true;
						mappedKeys[keyCode].Add(port);
						foreach(int conflictingPorts in mappedKeys[keyCode]) {
							portConflicts[conflictingPorts] = true;
						}
					} else {
						mappedKeys.Add(keyCode, new List<int>() { port });
					}
				}
			};

			InputInfo inputInfo = (InputInfo)Entity;
			for(int i = 0; i < 4; i++) {
				ControllerInfo controllerInfo = inputInfo.Controllers[i];
				if(i < 2 || this.FourScoreAttached && ((i == 2 && btnSetupP3.Enabled) || (i == 3 && btnSetupP4.Enabled))) {
					foreach(KeyMappings mappings in controllerInfo.Keys) {
						switch(controllerInfo.ControllerType) {
							case InteropEmu.ControllerType.StandardController:
							case InteropEmu.ControllerType.SnesController:
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
								if(i == 1 && inputInfo.ConsoleType == ConsoleType.Famicom && controllerInfo.ControllerType == InteropEmu.ControllerType.StandardController) {
									countMapping(i, mappings.Microphone);
								}
								if(controllerInfo.ControllerType == InteropEmu.ControllerType.SnesController) {
									countMapping(i, mappings.LButton);
									countMapping(i, mappings.RButton);

								}
								break;
							case InteropEmu.ControllerType.PowerPad:
								foreach(UInt32 button in mappings.PowerPadButtons.Values) {
									countMapping(i, button);
								}
								break;
						}
					}
				}
			}

			if(inputInfo.ConsoleType == ConsoleType.Famicom) {
				foreach(KeyMappings mappings in inputInfo.Controllers[0].Keys) {
					switch(inputInfo.ExpansionPortDevice) {
						case InteropEmu.ExpansionPortDevice.ExcitingBoxing:
							foreach(UInt32 button in mappings.ExcitingBoxingButtons.Values) {
								countMapping(4, button);
							}
							break;
						case InteropEmu.ExpansionPortDevice.FamilyTrainerMat:
							foreach(UInt32 button in mappings.PowerPadButtons.Values) {
								countMapping(4, button);
							}
							break;
						case InteropEmu.ExpansionPortDevice.JissenMahjong:
							foreach(UInt32 button in mappings.JissenMahjongButtons.Values) {
								countMapping(4, button);
							}
							break;
						case InteropEmu.ExpansionPortDevice.Pachinko:
							foreach(UInt32 button in mappings.PachinkoButtons.Values) {
								countMapping(4, button);
							}
							break;
						case InteropEmu.ExpansionPortDevice.PartyTap:
							foreach(UInt32 button in mappings.PartyTapButtons.Values) {
								countMapping(4, button);
							}
							break;
					}
				}
			}

			if(_hasCartridgeInput && btnSetupCartridge.Enabled) {
				//Bandai microphone
				foreach(KeyMappings mappings in inputInfo.Controllers[0].Keys) {
					foreach(UInt32 button in mappings.BandaiMicrophoneButtons.Values) {
						countMapping(5, button);
					}
				}
			}

			if(pnlConflictWarning.Visible != needWarning) {
				pnlConflictWarning.Visible = needWarning;
				this.Height = (int)(((needWarning ? 360 : 310) + (_hasCartridgeInput ? 30 : 0)) * _yFactor);
			}
			if(portConflicts[0] == (btnSetupP1.Image == null)) {
				btnSetupP1.Image = portConflicts[0] ? Properties.Resources.Warning : null;
			}
			if(portConflicts[1] == (btnSetupP2.Image == null)) {
				btnSetupP2.Image = portConflicts[1] ? Properties.Resources.Warning : null;
			}
			if(portConflicts[2] == (btnSetupP3.Image == null)) {
				btnSetupP3.Image = portConflicts[2] ? Properties.Resources.Warning : null;
			}
			if(portConflicts[3] == (btnSetupP4.Image == null)) {
				btnSetupP4.Image = portConflicts[3] ? Properties.Resources.Warning : null;
			}
			if(portConflicts[4] == (btnSetupExp.Image == null)) {
				btnSetupExp.Image = portConflicts[4] ? Properties.Resources.Warning : null;
			}
			if(portConflicts[5] == (btnSetupCartridge.Image == null)) {
				btnSetupCartridge.Image = portConflicts[5] ? Properties.Resources.Warning : null;
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
