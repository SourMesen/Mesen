using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	public partial class frmSelectExportRange : BaseConfigForm
	{
		public UInt32 ExportStart { get; private set; }
		public UInt32 ExportEnd { get; private set; }

		public frmSelectExportRange(UInt32 segStart, UInt32 segEnd)
		{
			InitializeComponent();

			dtpStart.Value = new DateTime(2000, 1, 1, 0, 0, 0).AddSeconds((int)(Math.Ceiling((decimal)segStart / 2)));
			dtpEnd.Value = new DateTime(2000, 1, 1, 0, 0, 0).AddSeconds(segEnd / 2);

			dtpStart.MinDate = dtpStart.Value;
			dtpStart.MaxDate = dtpEnd.Value;
			dtpEnd.MinDate = dtpStart.Value;
			dtpEnd.MaxDate = dtpEnd.Value;
		}

		protected override bool ValidateInput()
		{
			return dtpStart.Value < dtpEnd.Value;
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			ExportStart = (UInt32)(dtpStart.Value.TimeOfDay.TotalSeconds * 2);
			ExportEnd = (UInt32)(dtpEnd.Value.TimeOfDay.TotalSeconds * 2);
		}
	}
}
