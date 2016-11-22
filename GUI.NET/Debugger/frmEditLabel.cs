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
	public partial class frmEditLabel : BaseConfigForm
	{
		UInt32 _address;

		public frmEditLabel(UInt32 address, CodeLabel label)
		{
			InitializeComponent();

			_address = address;
			this.Text = "Edit Label: $" + address.ToString("X4");

			Entity = label;

			AddBinding("Label", txtLabel);
			AddBinding("Comment", txtComment);
		}

		protected override bool ValidateInput()
		{
			CodeLabel existingLabel = LabelManager.GetLabel(txtLabel.Text);
			return (existingLabel == null || existingLabel.Address == _address) 
				&& !txtComment.Text.Contains('\x1') && !txtComment.Text.Contains('\x2')
				&& !txtLabel.Text.Contains('\x1') && !txtLabel.Text.Contains('\x2');
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			txtLabel.Text = txtLabel.Text.Replace("$", "");
			base.OnFormClosed(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == (Keys.Control | Keys.Enter)) {
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
