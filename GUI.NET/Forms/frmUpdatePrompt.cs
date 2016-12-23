using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms
{
	public partial class frmUpdatePrompt : BaseForm
	{
		private string _fileHash;

		public frmUpdatePrompt(Version currentVersion, Version latestVersion, string changeLog, string fileHash)
		{
			InitializeComponent();

			this.txtChangelog.Font = new System.Drawing.Font(BaseControl.MonospaceFontFamily, 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

			_fileHash = fileHash;

			lblCurrentVersionString.Text = currentVersion.ToString();
			lblLatestVersionString.Text = latestVersion.ToString();
			txtChangelog.Text = changeLog.Replace("\n", Environment.NewLine);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			btnUpdate.Focus();
		}
		
		private void btnUpdate_Click(object sender, EventArgs e)
		{
			string destFilePath = System.Reflection.Assembly.GetEntryAssembly().Location;
			string srcFilePath = Path.Combine(ConfigManager.DownloadFolder, "Mesen." + lblLatestVersionString.Text + ".exe");
			string backupFilePath = Path.Combine(ConfigManager.BackupFolder, "Mesen." + lblCurrentVersionString.Text + ".exe");
			string updateHelper = Path.Combine(ConfigManager.HomeFolder, "MesenUpdater.exe");

			if(!File.Exists(updateHelper)) {
				MesenMsgBox.Show("UpdaterNotFound", MessageBoxButtons.OK, MessageBoxIcon.Error);
				DialogResult = DialogResult.Cancel;
			} else if(!string.IsNullOrWhiteSpace(srcFilePath)) {
				frmDownloadProgress frmDownload = new frmDownloadProgress("http://www.mesen.ca/Services/GetLatestVersion.php?a=download&p=win&v=" + InteropEmu.GetMesenVersion(), srcFilePath);
				if(frmDownload.ShowDialog() == DialogResult.OK) {
					FileInfo fileInfo = new FileInfo(srcFilePath);
					if(fileInfo.Length > 0 && GetSha1Hash(srcFilePath) == _fileHash) {
						if(Program.IsMono) {
							Process.Start("mono", string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\"", updateHelper, srcFilePath, destFilePath, backupFilePath));
						} else {
							Process.Start(updateHelper, string.Format("\"{0}\" \"{1}\" \"{2}\"", srcFilePath, destFilePath, backupFilePath));
						}
					} else {
						//Download failed, mismatching hashes
						MesenMsgBox.Show("UpdateDownloadFailed", MessageBoxButtons.OK, MessageBoxIcon.Error);
						DialogResult = DialogResult.Cancel;
					}
				}
			}
		}

		private string GetSha1Hash(string filename)
		{
			using(SHA1Managed sha1 = new SHA1Managed()) {
				byte[] hash = sha1.ComputeHash(File.ReadAllBytes(filename));

				var sb = new StringBuilder(hash.Length * 2);
				foreach(byte b in hash) {
					sb.Append(b.ToString("x2"));
				}
				return sb.ToString();
			}
		}
	}
}
