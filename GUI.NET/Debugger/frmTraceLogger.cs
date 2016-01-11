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
	public partial class frmTraceLogger : BaseForm
	{
		public frmTraceLogger()
		{
			InitializeComponent();
		}

		private void btnStartLogging_Click(object sender, EventArgs e)
		{
			TraceLoggingOptions options;
			InteropEmu.DebugStartTraceLogger(options);
		}

		private void btnStopLogging_Click(object sender, EventArgs e)
		{
			InteropEmu.DebugStopTraceLogger();
		}
	}
}
