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
	public partial class frmGameConfig : BaseConfigForm
	{
		public frmGameConfig(GameSpecificInfo configInfo)
		{
			InitializeComponent();

			GameSpecificInfo existingConfig = GameSpecificInfo.GetGameSpecificInfo();
			if(existingConfig == null) {
				GameDipswitchDefinition dipswitchDefinition = GameDipswitchDefinition.GetDipswitchDefinition();
				configInfo.DipSwitches = dipswitchDefinition.DefaultDipSwitches;
			}

			Entity = configInfo;
			UpdateDipSwitches();
		}

		private void UpdateDipSwitches()
		{
			GameDipswitchDefinition dipswitchDefinition = GameDipswitchDefinition.GetDipswitchDefinition();

			grpDipSwitches.Controls.Clear();

			List<List<string>> dipSwitches;
			if(dipswitchDefinition != null) {
				dipSwitches = dipswitchDefinition.DipSwitches;
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

			UInt32 value = ((GameSpecificInfo)Entity).DipSwitches;
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

				int selectedIndex = (int)((value >> currentBit) & ((1 << bitCount) - 1));
				optionDropdown.SelectedIndex = selectedIndex;
				optionDropdown.Dock = DockStyle.Fill;
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

		private UInt32 GetDipSwitchValue()
		{
			int value = 0;
			int currentBit = 0;
			if(grpDipSwitches.Controls.Count > 0) {
				foreach(Control control in grpDipSwitches.Controls[0].Controls) {
					if(control is ComboBox) {
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

			((GameSpecificInfo)Entity).DipSwitches = GetDipSwitchValue();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(this.DialogResult == DialogResult.OK) {
				GameSpecificInfo.AddGameSpecificConfig((GameSpecificInfo)Entity);
				GameSpecificInfo.ApplyGameSpecificConfig();
			}
			base.OnFormClosed(e);
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			GameDipswitchDefinition defaultConfig = GameDipswitchDefinition.GetDipswitchDefinition();
			((GameSpecificInfo)Entity).DipSwitches = defaultConfig.DefaultDipSwitches;
			UpdateUI();
			UpdateDipSwitches();
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
