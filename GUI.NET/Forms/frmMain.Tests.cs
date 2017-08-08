using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms
{
	public partial class frmMain
	{
		private void mnuTestRun_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterTest"));
				ofd.InitialDirectory = ConfigManager.TestFolder;
				ofd.Multiselect = true;
				if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					List<string> passedTests = new List<string>();
					List<string> failedTests = new List<string>();
					List<int> failedFrameCount = new List<int>();

					this.menuStrip.Enabled = false;

					Task.Run(() => {
						foreach(string filename in ofd.FileNames) {
							int result = InteropEmu.RunRecordedTest(filename);

							if(result == 0) {
								passedTests.Add(Path.GetFileNameWithoutExtension(filename));
							} else {
								failedTests.Add(Path.GetFileNameWithoutExtension(filename));
								failedFrameCount.Add(result);
							}
						}

						this.BeginInvoke((MethodInvoker)(() => {
							if(failedTests.Count == 0) {
								MessageBox.Show("All tests passed.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
							} else {
								StringBuilder message = new StringBuilder();
								if(passedTests.Count > 0) {
									message.AppendLine("Passed tests:");
									foreach(string test in passedTests) {
										message.AppendLine("  -" + test);
									}
									message.AppendLine("");
								}
								message.AppendLine("Failed tests:");
								for(int i = 0, len = failedTests.Count; i < len; i++) {
									message.AppendLine("  -" + failedTests[i] + " (" + failedFrameCount[i] + ")");
								}
								MessageBox.Show(message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}

							this.menuStrip.Enabled = true;
						}));
					});
				}
			}
		}

		private void mnuTestRecordStart_Click(object sender, EventArgs e)
		{
			RecordTest(true);
		}

		private void mnuTestRecordNow_Click(object sender, EventArgs e)
		{
			RecordTest(false);
		}

		private void RecordTest(bool resetEmu)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter(ResourceHelper.GetMessage("FilterTest"));
				sfd.InitialDirectory = ConfigManager.TestFolder;
				sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".mtp";
				if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					InteropEmu.RomTestRecord(sfd.FileName, resetEmu);
				}
			}
		}

		private void mnuTestRecordMovie_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterMovie"));
				ofd.InitialDirectory = ConfigManager.MovieFolder;
				if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					SaveFileDialog sfd = new SaveFileDialog();
					sfd.SetFilter(ResourceHelper.GetMessage("FilterTest"));
					sfd.InitialDirectory = ConfigManager.TestFolder;
					sfd.FileName = Path.GetFileNameWithoutExtension(ofd.FileName) + ".mtp";
					if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
						InteropEmu.RomTestRecordFromMovie(sfd.FileName, ofd.FileName);
					}
				}
			}
		}

		private void mnuTestRecordTest_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterTest"));
				ofd.InitialDirectory = ConfigManager.TestFolder;

				if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					SaveFileDialog sfd = new SaveFileDialog();
					sfd.SetFilter(ResourceHelper.GetMessage("FilterTest"));
					sfd.InitialDirectory = ConfigManager.TestFolder;
					sfd.FileName = Path.GetFileNameWithoutExtension(ofd.FileName) + ".mtp";
					if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
						InteropEmu.RomTestRecordFromTest(sfd.FileName, ofd.FileName);
					}
				}
			}
		}
		
		private void mnuRunAutomaticTest_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter("*.nes|*.nes");
				if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					string filename = ofd.FileName;

					Task.Run(() => {
						int result = InteropEmu.RunAutomaticTest(filename);
					});
				}
			}
		}

		private void mnuRunAllTests_Click(object sender, EventArgs e)
		{
			string workingDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.FileName = "TestHelper.exe";
			startInfo.WorkingDirectory = workingDirectory;
			Process.Start(startInfo);
		}

		private void mnuRunAllGameTests_Click(object sender, EventArgs e)
		{
			string workingDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.FileName = "TestHelper.exe";
			startInfo.Arguments = "\"" + Path.Combine(ConfigManager.HomeFolder, "TestGames") + "\"";
			startInfo.WorkingDirectory = workingDirectory;
			Process.Start(startInfo);
		}

		private void mnuTestStopRecording_Click(object sender, EventArgs e)
		{
			InteropEmu.RomTestStop();
		}
	}
}
