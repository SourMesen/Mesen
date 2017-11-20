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
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.Config
{
	public partial class frmControllerConfig : BaseConfigForm
	{
		private List<BaseInputConfigControl> _controls = new List<BaseInputConfigControl>();
		private KeyPresets _presets = new KeyPresets();
		private ControllerInfo _controllerInfo;
		private int _portNumber;
		private ConsoleType _consoleType;
		private InteropEmu.ControllerType _controllerType;

		public frmControllerConfig(ControllerInfo controllerInfo, int portNumber, ConsoleType consoleType, InteropEmu.ControllerType controllerType)
		{
			InitializeComponent();

			if(!this.DesignMode) {
				Entity = controllerInfo;

				AddBinding("TurboSpeed", trkTurboSpeed);

				_controllerInfo = controllerInfo;
				_portNumber = portNumber;
				_consoleType = consoleType;
				_controllerType = controllerType;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			switch(_controllerType) {
				case InteropEmu.ControllerType.StandardController:
				case InteropEmu.ControllerType.SnesController:
					for(int i = 0; i < 4; i++) {
						_controls.Add(new ctrlStandardController(_controllerInfo.Keys[i], _portNumber, _consoleType, _controllerType));
					}
					btnSelectPreset.Image = BaseControl.DownArrow;
					break;

				case InteropEmu.ControllerType.PowerPad:
					for(int i = 0; i < 4; i++) {
						_controls.Add(new ctrlPowerPadConfig(_controllerInfo.Keys[i]));
					}
					tlpMain.Controls.Remove(tlpStandardController);
					break;
			}

			TabPage[] tabPages = new TabPage[4] { tpgSet1, tpgSet2, tpgSet3, tpgSet4 };
			TableLayoutPanel[] layoutPanels = new TableLayoutPanel[4] { tlpSet1, tlpSet2, tlpSet3, tlpSet4 };
			float factor = (float)(tabPages[0].Height - 10) / _controls[0].Height;
			for(int i = 0; i < 4; i++) {
				layoutPanels[i].Controls.Add(_controls[i]);
				_controls[i].Anchor = AnchorStyles.None;
				_controls[i].Scale(new SizeF(factor, factor));
			}

			ResourceHelper.ApplyResources(this, mnuStripPreset);

			this.Text += ": " + ResourceHelper.GetMessage("PlayerNumber", (_portNumber + 1).ToString());
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			//Do not save anything, the parent input form will handle the changes
			if(this.DialogResult == DialogResult.OK) {
				_controls[0].UpdateKeyMappings();
				_controls[1].UpdateKeyMappings();
				_controls[2].UpdateKeyMappings();
				_controls[3].UpdateKeyMappings();
				UpdateObject();
			}
		}

		private BaseInputConfigControl GetControllerControl()
		{
			if(tabMain.SelectedTab == tpgSet1) {
				return _controls[0];
			} else if(tabMain.SelectedTab == tpgSet2) {
				return _controls[1];
			} else if(tabMain.SelectedTab == tpgSet3) {
				return _controls[2];
			} else if(tabMain.SelectedTab == tpgSet4) {
				return _controls[3];
			}

			return _controls[0];
		}

		private void UpdateTabIcons()
		{
			tpgSet1.ImageIndex = (int)_controls[0].GetKeyType() - 1;
			tpgSet2.ImageIndex = (int)_controls[1].GetKeyType() - 1;
			tpgSet3.ImageIndex = (int)_controls[2].GetKeyType() - 1;
			tpgSet4.ImageIndex = (int)_controls[3].GetKeyType() - 1;
		}

		protected override void UpdateConfig()
		{
			_controls[0].UpdateKeyMappings();
			_controls[1].UpdateKeyMappings();
			_controls[2].UpdateKeyMappings();
			_controls[3].UpdateKeyMappings();
			base.UpdateConfig();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			ControllerInfo controllerInfo = (ControllerInfo)Entity;
			if(tabMain.SelectedTab == tpgSet1) {
				_controls[0].ClearKeys();
			} else if(tabMain.SelectedTab == tpgSet2) {
				_controls[1].ClearKeys();
			} else if(tabMain.SelectedTab == tpgSet3) {
				_controls[2].ClearKeys();
			} else if(tabMain.SelectedTab == tpgSet4) {
				_controls[3].ClearKeys();
			}
		}

		private void btnSelectPreset_Click(object sender, EventArgs e)
		{
			mnuStripPreset.Show(btnSelectPreset.PointToScreen(new Point(0, btnSelectPreset.Height-1)));
		}

		private void mnuWasdLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.WasdLayout);
		}

		private void mnuArrowLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.ArrowLayout);
		}

		private void mnuFceuxLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.FceuxLayout);
		}

		private void mnuNestopiaLayout_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.NestopiaLayout);
		}

		private void mnuXboxLayout1_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.XboxLayout1);
		}

		private void mnuXboxLayout2_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.XboxLayout1);
		}

		private void mnuPs4Layout1_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.Ps4Layout1);
		}

		private void mnuPs4Layout2_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.Ps4Layout2);
		}

		private void mnuSnes30Layout1_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.Snes30Layout1);
		}

		private void mnuSnes30Layout2_Click(object sender, EventArgs e)
		{
			GetControllerControl().Initialize(_presets.Snes30Layout2);
		}

		private void ctrlStandardController_OnChange(object sender, EventArgs e)
		{
			UpdateTabIcons();
		}
	}
}
