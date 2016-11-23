using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmEditLabel : BaseConfigForm
	{
		private UInt32 _originalAddress;
		private AddressType _originalMemoryType;

		public frmEditLabel(CodeLabel label)
		{
			InitializeComponent();

			_originalAddress = label.Address;
			_originalMemoryType = label.AddressType;

			Entity = label;

			AddBinding("AddressType", cboRegion);
			AddBinding("Address", txtAddress);
			AddBinding("Label", txtLabel);
			AddBinding("Comment", txtComment);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			txtLabel.Focus();
		}

		protected override bool ValidateInput()
		{
			CodeLabel existingLabel = LabelManager.GetLabel(txtLabel.Text);

			return (existingLabel == null || (existingLabel.Address == _originalAddress && existingLabel.AddressType == _originalMemoryType)) 
				&& !txtComment.Text.Contains('\x1') && !txtComment.Text.Contains('\x2')
				&& (txtLabel.Text.Length == 0 || Regex.IsMatch(txtLabel.Text, "^[_a-zA-Z]+[_a-zA-Z0-9]*"));
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
