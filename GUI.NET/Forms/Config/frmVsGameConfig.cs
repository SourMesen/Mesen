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
	public partial class frmVsGameConfig : BaseConfigForm
	{
		public frmVsGameConfig(VsConfigInfo configInfo)
		{
			InitializeComponent();

			Entity = configInfo;

			if(string.IsNullOrWhiteSpace(configInfo.GameID)) {
				configInfo.GameID = VsGameConfig.GetGameID(InteropEmu.GetRomInfo().GetRomName());
			}

			AddBinding("PpuModel", cboPpuModel);

			foreach(KeyValuePair<string, VsGameConfig> kvp in VsGameConfig.GetGameConfigs()) {
				cboGame.Items.Add(kvp.Value.GameName);
				if(kvp.Key == configInfo.GameID) {
					cboGame.SelectedIndex = cboGame.Items.Count - 1;
				}
			}
		}

		private void cboGame_SelectedIndexChanged(object sender, EventArgs e)
		{
			VsGameConfig config = VsGameConfig.GetGameConfig(VsGameConfig.GetGameID(cboGame.SelectedItem.ToString()));
			UpdateDipSwitches(config, false);
		}

		private void UpdateDipSwitches(VsGameConfig config, bool updateDropdown)
		{
			grpDipSwitches.Controls.Clear();

			List<List<string>> dipSwitches;
			if(config != null) {
				dipSwitches = config.DipSwitches;
				if(updateDropdown) {
					cboGame.SelectedIndexChanged -= cboGame_SelectedIndexChanged;
					cboGame.SelectedItem = config.GameName;
					cboGame.SelectedIndexChanged += cboGame_SelectedIndexChanged;
				}
			} else {
				dipSwitches = new List<List<string>>();
				for(int i = 0; i < 8; i++) {
					dipSwitches.Add(new List<string>(new string[] { "Unknown", "Off", "On" }));
				}
			}

			int row = 0;
			var tlpDipSwitches = new TableLayoutPanel();
			tlpDipSwitches.Dock = DockStyle.Fill;
			tlpDipSwitches.ColumnStyles.Add(new ColumnStyle());
			tlpDipSwitches.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tlpDipSwitches.ColumnCount = 2;

			byte value = ((VsConfigInfo)Entity).DipSwitches;
			int currentBit = 0;
			foreach(List<string> setting in dipSwitches) {
				var optionLabel = new Label();
				optionLabel.AutoSize = true;
				optionLabel.Text = setting[0] + ":";
				optionLabel.TextAlign = ContentAlignment.MiddleLeft;
				optionLabel.Dock = DockStyle.Fill;

				var optionDropdown = new ComboBox();
				optionDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
				for(int i = 1, len = setting.Count; i < len; i++) {
					optionDropdown.Items.Add(new DipSwitchOption() { Index = i - 1, DisplayValue = setting[i] });
				}

				int bitCount = (int)Math.Round(Math.Log(optionDropdown.Items.Count) / Math.Log(2));

				int selectedIndex = (value >> currentBit) & ((1 << bitCount) - 1);
				optionDropdown.SelectedIndex = selectedIndex;
				currentBit += bitCount;
				

				tlpDipSwitches.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				tlpDipSwitches.Controls.Add(optionLabel, 0, row);
				tlpDipSwitches.Controls.Add(optionDropdown, 1, row);
				row++;
			}
			tlpDipSwitches.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tlpDipSwitches.RowCount = row + 1;
			grpDipSwitches.Controls.Add(tlpDipSwitches);
			tlpDipSwitches.PerformLayout();
		}

		private byte GetDipSwitchValue()
		{
			int value = 0;
			int currentBit = 0;
			if(grpDipSwitches.Controls.Count > 0) {
				foreach(Control control in grpDipSwitches.Controls[0].Controls) {
					if(control is ComboBox && control != cboPpuModel) {
						ComboBox dipSwitch = (ComboBox)control;
						int bitCount = (int)Math.Round(Math.Log(dipSwitch.Items.Count) / Math.Log(2));

						if(dipSwitch.SelectedItem != null) {
							value |= ((DipSwitchOption)dipSwitch.SelectedItem).Index << currentBit;
						}

						currentBit += bitCount;
					}
				}
			}

			return (byte)value;
		}

		protected override void UpdateConfig()
		{
			base.UpdateConfig();

			((VsConfigInfo)Entity).DipSwitches = (byte)GetDipSwitchValue();
			((VsConfigInfo)Entity).GameID = VsGameConfig.GetGameID(cboGame.SelectedItem.ToString());
		}
	}

	public class DipSwitchOption
	{
		public int Index;
		public string DisplayValue;

		public override string ToString()
		{
			return DisplayValue;
		}
	}
}
