using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;

namespace Mesen.GUI.GoogleDriveIntegration
{
	public class GoogleDriveAccessor : IDisposable
	{
		private const string _contentType = @"application/zip";
		private readonly string[] _scopes = new[] { DriveService.Scope.DriveAppdata };
		private File _driveFile = null;
		private UserCredential _credentials = null;
		private DriveService _service = null;
		private bool _connected = false;
		private bool _revoked = false;

		public bool Revoked { get { return _revoked; } }

		public bool UploadFile(System.IO.MemoryStream fileStream, string filename)
		{
			fileStream.Position = 0;
			try {
				this.Connect().GetAwaiter().GetResult();
				if(_connected) {
					this.UploadFileAsync(fileStream, filename).GetAwaiter().GetResult();
				}
			} catch(TokenResponseException ex) {
				_revoked = true;
				_connected = false;
				_credentials = null;
				_service = null;
			} catch {
				_connected = false;
				_credentials = null;
				_service = null;
			}

			return _connected;
		}

		public bool DownloadFile(System.IO.MemoryStream fileStream, string filename)
		{
			try {
				this.Connect().GetAwaiter().GetResult();

				if(_connected) {
					this.DownloadFileAsync(fileStream, filename).GetAwaiter().GetResult();
				}
			} catch(TokenResponseException ex) {
				_revoked = true;
				_connected = false;
				_credentials = null;
				_service = null;
			} catch {
				_connected = false;
				_credentials = null;
				_service = null;
			}
			return _connected;
		}

		public bool AcquireToken()
		{
			this.Connect().GetAwaiter().GetResult();

			return _connected;
		}

		public void RevokeToken()
		{
			Task.Run(async () => {
				try {
					_credentials = await this.GetCredentials().ConfigureAwait(false);
					await _credentials.RevokeTokenAsync(CancellationToken.None).ConfigureAwait(false);
				} catch {
				}
				_service = null;
				_credentials = null;
			}).Wait();
		}

		private async Task Connect()
		{
			if(_service == null) {
				try {
					_credentials = await this.GetCredentials().ConfigureAwait(false);
					_service = new DriveService(new BaseClientService.Initializer() {
						HttpClientInitializer = _credentials,
						ApplicationName = "Mesen",
					});
					_connected = true;
				} catch(AggregateException ex) {
					foreach(Exception innerException in ex.InnerExceptions) {
						if(innerException is TokenResponseException) {
							_connected = false;
							_revoked = true;
						}
					}
					_connected = false;
					_credentials = null;
					_service = null;
				} catch {
					_connected = false;
					_credentials = null;
					_service = null;
				}
			}
		}
		
		private async Task<UserCredential> GetCredentials()
		{
			var clientSecrets = new ClientSecrets { ClientId = "478233037635-nf90q052c32suhm0l8r9fkkk34k7hkl5.apps.googleusercontent.com", ClientSecret = "zGatV81vs5kKuhHq3fZuw4lc" };
			GoogleWebAuthorizationBroker.Folder = System.IO.Path.Combine(Config.ConfigManager.HomeFolder, "GoogleDrive");

			return await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, _scopes, Environment.UserName, CancellationToken.None, codeReceiver: new MesenCodeReceiver()).ConfigureAwait(false);
		}

		private File GetFileMatchingName(string filename)
		{
			var listService = _service.Files.List();
			listService.Spaces = "appDataFolder";

			foreach(File file in listService.Execute().Files) {
				if(file.Name == filename) {
					return file;
				}
			}

			return null;
		}

		private Task<IUploadProgress> UploadFileAsync(System.IO.MemoryStream fileStream, string filename)
		{
			File driveFile = this.GetFileMatchingName(filename);

			Task<IUploadProgress> task;
			if(driveFile == null) {
				//File does not exist, create it
				var insert = _service.Files.Create(new File { Name = "MesenData.zip", Parents = new List<string>() { "appDataFolder" } }, fileStream, _contentType);
				insert.ChunkSize = FilesResource.CreateMediaUpload.MinimumChunkSize * 2;
				insert.ResponseReceived += Upload_ResponseReceived;

				task = insert.UploadAsync();
			} else {
				//File exists, update it
				var update = _service.Files.Update(null, driveFile.Id, fileStream, _contentType);
				update.ResponseReceived += Upload_ResponseReceived;
				task = update.UploadAsync();
			}

			task.ContinueWith(t => {
				// NotOnRanToCompletion - this code will be called if the upload fails
				Console.WriteLine("Upload Failed. " + t.Exception);
			}, TaskContinuationOptions.NotOnRanToCompletion);

			task.ContinueWith(t => {
				fileStream.Dispose();
			});

			return task;
		}

		private async Task DownloadFileAsync(System.IO.MemoryStream outStream, string filename)
		{
			File driveFile = this.GetFileMatchingName(filename);

			if(driveFile != null) {
				var request = _service.Files.Get(driveFile.Id);
				var progress = await request.DownloadAsync(outStream).ConfigureAwait(false);
				if(progress.Status == DownloadStatus.Completed) {
					_driveFile = driveFile;
				} else {
					_driveFile = null;
				}
			}
		}

		private async Task DeleteFile(File file)
		{
			await _service.Files.Delete(file.Id).ExecuteAsync().ConfigureAwait(false);
		}

		void Upload_ResponseReceived(File file)
		{
			_driveFile = file;
		}

		void IDisposable.Dispose()
		{
			if(_service != null) {
				_service.Dispose();
			}
		}
	}
}
