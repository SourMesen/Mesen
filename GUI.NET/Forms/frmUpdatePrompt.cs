using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms
{
	public partial class frmUpdatePrompt : BaseForm
	{
		public frmUpdatePrompt(Version currentVersion, Version latestVersion, string changeLog)
		{
			InitializeComponent();

			lblCurrentVersionString.Text = currentVersion.ToString();
			lblLatestVersionString.Text = latestVersion.ToString();
			txtChangelog.Text = changeLog.Replace("\n", Environment.NewLine);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			btnUpdate.Focus();
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			if(DialogResult == DialogResult.OK) {
				string destFilePath = Process.GetCurrentProcess().MainModule.FileName;
				string srcFilePath = Path.Combine(ConfigManager.DownloadFolder, "Mesen." + lblLatestVersionString.Text + ".exe");
				string backupFilePath = Path.Combine(ConfigManager.BackupFolder, "Mesen." + lblCurrentVersionString.Text + ".exe");
				string updateHelper = Path.Combine(ConfigManager.HomeFolder, "MesenUpdater.exe");

				if(!string.IsNullOrWhiteSpace(srcFilePath)) {
					frmDownloadProgress frmDownload = new frmDownloadProgress("http://www.mesen.ca/Services/GetLatestVersion.php?a=download&&p=win", srcFilePath);
					if(frmDownload.ShowDialog() == DialogResult.OK) {
						Process.Start(updateHelper, string.Format("\"{0}\" \"{1}\" \"{2}\"", srcFilePath, destFilePath, backupFilePath));
						Application.Exit();
					}
				}
			}
		}
	}
}
