using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;

namespace Mesen.GUI.GoogleDriveIntegration
{
	public class MesenCodeReceiver : LocalServerCodeReceiver, ICodeReceiver
	{
		private const string baseResponse =
		@"<html>
			<head>
				<title>Mesen - Authentication Successful</title>
				<style>
					html, body {
						font-family: Calibri;
						height:100%;
						background-color: #EEE;
					}
					img {
						margin: 10px;
						vertical-align: middle;
					}
					div {
						background-color: #FFF;
						position: relative;
						top: 50px;

						margin:auto;
						width:600px;
						height:154px;
						border:1px solid #999;
						text-align:center;
					}
					span {
						vertical-align: middle;
					}
				</style>
			</head>";

		private const string successResponse = baseResponse + 
			@"<body>
				<div>
					<img src='http://www.mesen.ca/Images/MesenIcon.png'/><span>Mesen - Authentication Successful</span><br/>
					Mesen will now save battery files and save states in your Google Drive account.<br/>
					<br/>
					Please close this window.
				</div>
			</body>
		</html>";

		private const string failureResponse = baseResponse +
			@"<body>
				<div>
					<img src='http://www.mesen.ca/Images/MesenIcon.png'/><span>Mesen - Authentication <span style='color:red'>Failed</span></span><br/>
					Mesen was unable to integrate with your Google Drive account - please close this window and try again.<br/>
				</div>
			</body>
		</html>";

		async Task<AuthorizationCodeResponseUrl> ICodeReceiver.ReceiveCodeAsync(AuthorizationCodeRequestUrl url, CancellationToken taskCancellationToken)
		{
			var authorizationUrl = url.Build().ToString();
			using(var listener = new HttpListener()) {
				listener.Prefixes.Add(RedirectUri);
				try {
					listener.Start();

					Process.Start(authorizationUrl);

					// Wait to get the authorization code response.
					var context = await listener.GetContextAsync().ConfigureAwait(false);
					NameValueCollection coll = context.Request.QueryString;

					// Write a "close" response.
					Thread.Sleep(200);

					// Create a new response URL with a dictionary that contains all the response query parameters.
					var codeResponse = new AuthorizationCodeResponseUrl(coll.AllKeys.ToDictionary(k => k, k => coll[k]));
					using(var writer = new System.IO.StreamWriter(context.Response.OutputStream)) {
						writer.WriteLine(string.IsNullOrWhiteSpace(codeResponse.Error) ? successResponse : failureResponse);
						writer.Flush();
					}
					context.Response.OutputStream.Close();
					return codeResponse;
				} finally {
					listener.Close();
				}
			}
		}
	}
}
