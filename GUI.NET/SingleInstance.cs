using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Collections.Generic;

namespace Mesen.GUI
{
	public class ArgumentsReceivedEventArgs : EventArgs
	{
		public String[] Args { get; set; }
	}

	public class SingleInstance : IDisposable
	{
		/// <summary>
		/// Taken from flawlesscode.com (website no longer available)
		/// </summary>
		private Mutex _mutex = null;
		private Boolean _firstInstance = false;
		private Guid _identifier = Guid.Empty;

		/// <summary>
		/// Enforces single instance for an application.
		/// </summary>
		/// <param name="identifier">An identifier unique to this application.</param>
		public SingleInstance(Guid identifier)
		{
			this._identifier = identifier;
			this._mutex = new Mutex(true, identifier.ToString(), out _firstInstance);
		}

		/// <summary>
		/// Indicates whether this is the first instance of this application.
		/// </summary>
		public Boolean FirstInstance { get { return _firstInstance; } }

		/// <summary>
		/// Passes the given arguments to the first running instance of the application.
		/// </summary>
		/// <param name="arguments">The arguments to pass.</param>
		/// <returns>Return true if the operation succeded, false otherwise.</returns>
		public Boolean PassArgumentsToFirstInstance(String[] arguments)
		{
			try {
				using(NamedPipeClientStream client = new NamedPipeClientStream(_identifier.ToString())) {
					using(StreamWriter writer = new StreamWriter(client)) {
						client.Connect(200);

						foreach(String argument in arguments) {
							writer.WriteLine(argument);
						}
					}
				}
				return true;
			} catch { }

			return false;
		}

		/// <summary>
		/// Listens for arguments being passed from successive instances of the applicaiton.
		/// </summary>
		public void ListenForArgumentsFromSuccessiveInstances()
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(ListenForArguments));
		}

		/// <summary>
		/// Listens for arguments on a named pipe.
		/// </summary>
		/// <param name="state">State object required by WaitCallback delegate.</param>
		private void ListenForArguments(Object state)
		{
			try {
				using(NamedPipeServerStream server = new NamedPipeServerStream(_identifier.ToString())) {
					using(StreamReader reader = new StreamReader(server)) {
						server.WaitForConnection();

						List<String> arguments = new List<String>();
						while(server.IsConnected) {
							arguments.Add(reader.ReadLine());
						}

						ThreadPool.QueueUserWorkItem(new WaitCallback(CallOnArgumentsReceived), arguments.ToArray());
					}
				}
			} catch(IOException) { 
				//Pipe was broken
			} finally {
				ListenForArguments(null);
			}
		}

		/// <summary>
		/// Calls the OnArgumentsReceived method casting the state Object to String[].
		/// </summary>
		/// <param name="state">The arguments to pass.</param>
		private void CallOnArgumentsReceived(Object state)
		{
			OnArgumentsReceived((String[])state);
		}

		/// <summary>
		/// Event raised when arguments are received from successive instances.
		/// </summary>
		public event EventHandler<ArgumentsReceivedEventArgs> ArgumentsReceived;

		/// <summary>
		/// Fires the ArgumentsReceived event.
		/// </summary>
		/// <param name="arguments">The arguments to pass with the ArgumentsReceivedEventArgs.</param>
		private void OnArgumentsReceived(String[] arguments)
		{
			if(ArgumentsReceived != null)
				ArgumentsReceived(this, new ArgumentsReceivedEventArgs() { Args = arguments });
		}

		#region IDisposable
		private Boolean _disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if(!_disposed) {
				if(_mutex != null && _firstInstance) {
					_mutex.ReleaseMutex();
					_mutex = null;
				}
				_disposed = true;
			}
		}

		~SingleInstance()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}