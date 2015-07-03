using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesen.GUI.Config
{
	public class ServerInfo
	{
		public string Name = "Default";
		public UInt16 Port = 8888;
		public string Password = null;
		public int MaxPlayers = 4;
		public bool AllowSpectators = true;
		public bool PublicServer = false;
	}
}
