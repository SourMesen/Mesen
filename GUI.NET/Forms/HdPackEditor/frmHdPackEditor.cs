using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms.HdPackEditor
{
	public partial class frmHdPackEditor : BaseForm
	{
		private bool _isRecording = false;

		public frmHdPackEditor()
		{
			InitializeComponent();

			if(!InteropEmu.GetRomInfo().IsChrRam) {
				flpBankSize.Visible = false;
				lblBankSize.Visible = false;
			}

			txtSaveFolder.Text = Path.Combine(ConfigManager.HdPackFolder, InteropEmu.GetRomInfo().GetRomName());
			picBankPreview.Image = new Bitmap(256, 256);

			UpdateFilterDropdown();

			cboChrBankSize.SelectedIndex = 2;

			toolTip.SetToolTip(picScaleHelp, ResourceHelper.GetMessage("HdPackBuilderScaleHelp"));
			toolTip.SetToolTip(picBankSizeHelp, ResourceHelper.GetMessage("HdPackBuilderBankSizeHelp"));
			toolTip.SetToolTip(picFrequencyHelp, ResourceHelper.GetMessage("HdPackBuilderFrequencyHelp"));
			toolTip.SetToolTip(picGroupBlankHelp, ResourceHelper.GetMessage("HdPackBuilderGroupBlankHelp"));
			toolTip.SetToolTip(picLargeSpritesHelp, ResourceHelper.GetMessage("HdPackBuilderLargeSpritesHelp"));

			UpdateUI(false);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			if(_isRecording) {
				StopRecording();
			}
		}

		private void UpdateFilterDropdown()
		{
			int scaleFilter = -1;
			string hdDefFile = Path.Combine(txtSaveFolder.Text, "hires.txt");
			if(File.Exists(hdDefFile)) {
				string fileContent = File.ReadAllText(hdDefFile);
				Match match = Regex.Match(fileContent, "<scale>(\\d*)");
				if(match.Success) {
					int scale;
					if(Int32.TryParse(match.Groups[1].ToString(), out scale)) {
						scaleFilter = scale;
					}
				}
			}

			cboScale.Items.Clear();
			foreach(FilterInfo info in _filters) {
				if(scaleFilter == -1 || info.Scale == scaleFilter) {
					cboScale.Items.Add(info);
				}
			}
			cboScale.SelectedIndex = 0;
		}

		private void tmrRefresh_Tick(object sender, EventArgs e)
		{
			UInt32[] bankList = InteropEmu.HdBuilderGetChrBankList();
			if(bankList.Length > 0) {
				object selectedItem = cboBank.SelectedItem;
				if(bankList.Length != cboBank.Items.Count) {
					cboBank.Items.Clear();
					foreach(UInt32 bankNumber in bankList) {
						cboBank.Items.Add(bankNumber);
					}
				}
				cboBank.SelectedItem = selectedItem;
				if(cboBank.SelectedIndex < 0) {
					cboBank.SelectedIndex = 0;
				}

				int scale = (int)((FilterInfo)cboScale.SelectedItem).Scale;

				using(Graphics g = Graphics.FromImage(picBankPreview.Image)) {
					Byte[] rgbBuffer = InteropEmu.HdBuilderGetBankPreview((uint)cboBank.SelectedItem, scale, 0);
					GCHandle handle = GCHandle.Alloc(rgbBuffer, GCHandleType.Pinned);
					Bitmap source = new Bitmap(128*scale, 128*scale, 4*128*scale, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
					try {
						g.Clear(Color.Black);
						g.DrawImage(source, 0, 0, 256, 256);
					} finally {
						handle.Free();
					}
				}
				picBankPreview.Refresh();
			}
		}

		private void UpdateUI(bool isRecording)
		{
			btnStartRecording.Visible = !isRecording;
			btnStopRecording.Visible = isRecording;

			cboBank.Enabled = isRecording;

			chkLargeSprites.Enabled = !isRecording;
			chkSortByFrequency.Enabled = !isRecording;
			chkGroupBlankTiles.Enabled = !isRecording;

			cboChrBankSize.Enabled = !isRecording;
			cboScale.Enabled = !isRecording;
			btnSelectFolder.Enabled = !isRecording;

			_isRecording = isRecording;
		}

		private void btnStartRecording_Click(object sender, EventArgs e)
		{
			StartRecording();
		}

		private void btnStopRecording_Click(object sender, EventArgs e)
		{
			StopRecording();
		}

		private void StartRecording()
		{
			HdPackRecordFlags flags = HdPackRecordFlags.None;
			if(chkLargeSprites.Checked) {
				flags |= HdPackRecordFlags.UseLargeSprites;
			}
			if(chkSortByFrequency.Checked) {
				flags |= HdPackRecordFlags.SortByUsageFrequency;
			}
			if(chkGroupBlankTiles.Checked) {
				flags |= HdPackRecordFlags.GroupBlankTiles;
			}

			InteropEmu.HdBuilderStartRecording(txtSaveFolder.Text, ((FilterInfo)cboScale.SelectedItem).FilterType, ((FilterInfo)cboScale.SelectedItem).Scale, flags, (UInt32)Math.Pow(2, cboChrBankSize.SelectedIndex) * 0x400);
			tmrRefresh.Start();

			UpdateUI(true);
		}

		private void StopRecording()
		{
			tmrRefresh.Stop();
			InteropEmu.HdBuilderStopRecording();

			UpdateFilterDropdown();

			UpdateUI(false);

			btnOpenFolder.Visible = true;
		}

		private void btnOpenFolder_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(this.txtSaveFolder.Text);
		}

		private void btnSelectFolder_Click(object sender, EventArgs e)
		{
			using(FolderBrowserDialog fbd = new FolderBrowserDialog()) {
				fbd.SelectedPath = ConfigManager.HdPackFolder;
				if(fbd.ShowDialog() == DialogResult.OK) {
					txtSaveFolder.Text = Path.Combine(fbd.SelectedPath, InteropEmu.GetRomInfo().GetRomName());
					UpdateFilterDropdown();
				}
			}
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == Keys.Escape) {
				this.Close();
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private class FilterInfo
		{
			public string Name { get; set; }
			public ScaleFilterType FilterType { get; set; }
			public UInt32 Scale { get; set; }

			public override string ToString()
			{
				return Name;
			}
		}

		private FilterInfo[] _filters = {
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.None) + " (1x)", FilterType = ScaleFilterType.Prescale, Scale = 1 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Prescale2x), FilterType = ScaleFilterType.Prescale, Scale = 2 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Prescale3x), FilterType = ScaleFilterType.Prescale, Scale = 3 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Prescale4x), FilterType = ScaleFilterType.Prescale, Scale = 4 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Prescale6x), FilterType = ScaleFilterType.Prescale, Scale = 6 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Prescale8x), FilterType = ScaleFilterType.Prescale, Scale = 8 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Prescale10x), FilterType = ScaleFilterType.Prescale, Scale = 10 },

			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.HQ2x), FilterType = ScaleFilterType.HQX, Scale = 2 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.HQ3x), FilterType = ScaleFilterType.HQX, Scale = 3 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.HQ4x), FilterType = ScaleFilterType.HQX, Scale = 4 },

			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Scale2x), FilterType = ScaleFilterType.Scale2x, Scale = 2 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Scale3x), FilterType = ScaleFilterType.Scale2x, Scale = 3 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Scale4x), FilterType = ScaleFilterType.Scale2x, Scale = 4 },

			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.Super2xSai), FilterType = ScaleFilterType.Super2xSai, Scale = 2 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.SuperEagle), FilterType = ScaleFilterType.SuperEagle, Scale = 2 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType._2xSai), FilterType = ScaleFilterType._2xSai, Scale = 2 },

			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.xBRZ2x), FilterType = ScaleFilterType.xBRZ, Scale = 2 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.xBRZ3x), FilterType = ScaleFilterType.xBRZ, Scale = 3 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.xBRZ4x), FilterType = ScaleFilterType.xBRZ, Scale = 4 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.xBRZ5x), FilterType = ScaleFilterType.xBRZ, Scale = 5 },
			new FilterInfo() { Name = ResourceHelper.GetEnumText(VideoFilterType.xBRZ6x), FilterType = ScaleFilterType.xBRZ, Scale = 6 },
		};
	}
}