using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mesen.GUI.Config;

namespace Mesen.GUI.GoogleDriveIntegration
{
	public class CloudSyncHelper
	{
		private static object _lock = new object();
		private static GoogleDriveAccessor _accessor;

		public static bool Syncing
		{
			get
			{
				bool lockTaken = false;
				System.Threading.Monitor.TryEnter(_lock, ref lockTaken);
				if(lockTaken) {
					System.Threading.Monitor.Exit(_lock);
				}
				return !lockTaken;
			}
		}

		public static bool Sync()
		{
			if(!System.Threading.Monitor.TryEnter(_lock)) {
				//Already syncing, return when on-going sync is done
				lock(_lock) {
					return true;
				}
			} else {
				try {
					InteropEmu.DisplayMessage("GoogleDrive", "SynchronizationStarted");
					using(_accessor = new GoogleDriveAccessor()) {
						FileDownloadResult result = CloudSyncHelper.DownloadData();
						if(result == FileDownloadResult.Error) {
							InteropEmu.DisplayMessage("GoogleDrive", "SynchronizationFailed");
							return false;
						}

						CloudSyncHelper.UploadData();
						InteropEmu.DisplayMessage("GoogleDrive", "SynchronizationCompleted");
						ConfigManager.Config.PreferenceInfo.CloudLastSync = DateTime.Now;
						ConfigManager.ApplyChanges();

						return true;
					}
				} finally {
					System.Threading.Monitor.Exit(_lock);
				}
			}
		}

		public static bool EnableSync()
		{
			using(_accessor = new GoogleDriveAccessor()) {
				bool result = _accessor.AcquireToken();
				if(result) {
					ConfigManager.Config.PreferenceInfo.CloudSaveIntegration = true;
					ConfigManager.ApplyChanges();
				}
				return result;
			}
		}

		public static void DisableSync()
		{
			using(_accessor = new GoogleDriveAccessor()) {
				_accessor.RevokeToken();
			}

			ConfigManager.Config.PreferenceInfo.CloudSaveIntegration = false;
			ConfigManager.ApplyChanges();
		}

		private static bool UploadData()
		{
			using(MemoryStream stream = CloudSyncHelper.GetDataStream()) {
				var gdAccessor = new GoogleDriveAccessor();
				return _accessor.UploadFile(stream, "MesenData.zip");
			}
		}

		private static FileDownloadResult DownloadData()
		{
			using(MemoryStream stream = new MemoryStream()) {
				FileDownloadResult result = _accessor.DownloadFile(stream, "MesenData.zip");

				if(result == FileDownloadResult.OK) {
					try {
						stream.Position = 0;

						string homeFolder = ConfigManager.HomeFolder;

						//Make sure the folders exist
						string saveFolder = ConfigManager.SaveFolder;
						string saveStateFolder = ConfigManager.SaveStateFolder;

						using(ZipArchive archive = new ZipArchive(stream)) {
							foreach(ZipArchiveEntry entry in archive.Entries) {
								if(!string.IsNullOrWhiteSpace(entry.Name)) {
									string[] fileAndFolder = entry.FullName.Split('/');
									string fileName = Path.Combine(homeFolder, fileAndFolder[0], fileAndFolder[1]);
									if(!File.Exists(fileName) || File.GetLastWriteTime(fileName) < entry.LastWriteTime.ToLocalTime()) {
										//File on server is more recent, or doesn't exist on local computer, extract it
										try {
											entry.ExtractToFile(fileName, true);
										} catch {
											//Files might be opened by another program or not read-only, etc.
										}
									}
								}
							}
						}
					} catch {
						result = FileDownloadResult.FileCorrupted;
					}
				} else if(_accessor.Revoked) {
					ConfigManager.Config.PreferenceInfo.CloudSaveIntegration = false;
					ConfigManager.ApplyChanges();
				}

				return result;
			}
		}

		private static MemoryStream GetDataStream()
		{
			MemoryStream outputStream = new MemoryStream();
			using(ZipArchive archive = new ZipArchive(outputStream, ZipArchiveMode.Create, true)) {
				archive.CreateEntry("Saves/");
				foreach(string filename in System.IO.Directory.EnumerateFiles(ConfigManager.SaveFolder, "*.sav", System.IO.SearchOption.AllDirectories)) {
					archive.CreateEntryFromFile(filename, "Saves/" + System.IO.Path.GetFileName(filename));
				}

				archive.CreateEntry("SaveStates/");
				foreach(string filename in System.IO.Directory.EnumerateFiles(ConfigManager.SaveStateFolder, "*.mst", System.IO.SearchOption.AllDirectories)) {
					archive.CreateEntryFromFile(filename, "SaveStates/" + System.IO.Path.GetFileName(filename));
				}
			}
			return outputStream;
		}
	}
}
