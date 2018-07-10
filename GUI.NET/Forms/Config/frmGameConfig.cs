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
		private List<List<string>> _dipSwitches;
		private List<ComboBox> _dropdowns;

		public frmGameConfig(GameSpecificInfo configInfo)
		{
			InitializeComponent();

			GameSpecificInfo existingConfig = GameSpecificInfo.GetGameSpecificInfo();
			GameDipswitchDefinition dipswitchDefinition = GameDipswitchDefinition.GetDipswitchDefinition();
			if(existingConfig == null && dipswitchDefinition != null) {
				configInfo.DipSwitches = dipswitchDefinition.DefaultDipSwitches;
			}

			if(dipswitchDefinition != null) {
				_dipSwitches = dipswitchDefinition.DipSwitches;
			} else {
				_dipSwitches = new List<List<string>>();
				for(int i = 0; i < (InteropEmu.IsVsDualSystem() ? 16 : 8); i++) {
					_dipSwitches.Add(new List<string>(new string[] { "Dipswitch #" + i.ToString(), "Off", "On" }));
				}
			}

			if(_dipSwitches.Count > 8) {
				this.Width *= 2;
			}

			Entity = configInfo;
			InitializeDipSwitches();
		}

		private void InitializeDipSwitches()
		{
			_dropdowns = new List<ComboBox>();
			grpDipSwitches.Controls.Clear();

			int row = 0;
			int baseColumn = 0;
			var tlpDipSwitches = new TableLayoutPanel();
			tlpDipSwitches.Dock = DockStyle.Fill;

			if(_dipSwitches.Count > 8) {
				tlpDipSwitches.ColumnStyles.Add(new ColumnStyle());
				tlpDipSwitches.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
				tlpDipSwitches.ColumnStyles.Add(new ColumnStyle());
				tlpDipSwitches.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
				tlpDipSwitches.ColumnCount = 4;
			} else {
				tlpDipSwitches.ColumnStyles.Add(new ColumnStyle());
				tlpDipSwitches.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
				tlpDipSwitches.ColumnCount = 2;
			}

			UInt32 value = ((GameSpecificInfo)Entity).DipSwitches;
			int currentBit = 0;
			foreach(List<string> setting in _dipSwitches) {
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

				if(Program.IsMono) {
					tlpDipSwitches.RowStyles.Add(new RowStyle(SizeType.Absolute, 27));
				} else {
					tlpDipSwitches.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				}
				tlpDipSwitches.Controls.Add(optionLabel, baseColumn, row);
				tlpDipSwitches.Controls.Add(optionDropdown, baseColumn + 1, row);
				row++;
				if(row == 8) {
					baseColumn += 2;
					row = 0;
				}

				_dropdowns.Add(optionDropdown);
			}
			tlpDipSwitches.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tlpDipSwitches.RowCount = _dipSwitches.Count + 1;

			grpDipSwitches.Controls.Add(tlpDipSwitches);
			tlpDipSwitches.PerformLayout();
		}

		private void UpdateDipSwitches()
		{
			UInt32 value = ((GameSpecificInfo)Entity).DipSwitches;
			int currentBit = 0;
			for(int i = 0; i < _dipSwitches.Count; i++) {
				int bitCount = (int)Math.Round(Math.Log(_dropdowns[i].Items.Count) / Math.Log(2));

				int selectedIndex = (int)((value >> currentBit) & ((1 << bitCount) - 1));
				_dropdowns[i].SelectedIndex = selectedIndex;
			}
		}

		private UInt32 GetDipSwitchValue()
		{
			int value = 0;
			int currentBit = 0;
			foreach(ComboBox dipSwitch in _dropdowns) {
				int bitCount = (int)Math.Round(Math.Log(dipSwitch.Items.Count) / Math.Log(2));

				if(dipSwitch.SelectedItem != null) {
					value |= ((DipSwitchOption)dipSwitch.SelectedItem).Index << currentBit;
				}

				currentBit += bitCount;
			}

			return (UInt32)value;
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
			((GameSpecificInfo)Entity).DipSwitches = defaultConfig?.DefaultDipSwitches ?? 0;
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
