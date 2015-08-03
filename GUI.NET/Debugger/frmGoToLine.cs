using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmGoToLine : BaseConfigForm
	{
		public frmGoToLine(GoToAddress address)
		{
			InitializeComponent();

			Entity = address;
			AddBinding("Address", txtAddress);
		}
	}

	public class GoToAddress
	{
		public UInt32 Address;
	}
}
