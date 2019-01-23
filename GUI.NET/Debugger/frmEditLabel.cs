﻿using System;
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
		private CodeLabel _originalLabel;
		private const int _maxInternalRamAddress = 0x1FFF;
		private const int _maxRegisterAddress = 0xFFFF;
		private int _maxPrgRomAddress = 0;
		private int _maxWorkRamAddress = 0;
		private int _maxSaveRamAddress = 0;

		public frmEditLabel(CodeLabel label, CodeLabel originalLabel = null)
		{
			InitializeComponent();

			_originalLabel = originalLabel;

			Entity = label;

			_maxPrgRomAddress = Math.Max(0, InteropEmu.DebugGetMemorySize(DebugMemoryType.PrgRom) - 1);
			_maxWorkRamAddress = Math.Max(0, InteropEmu.DebugGetMemorySize(DebugMemoryType.WorkRam) - 1);
			_maxSaveRamAddress = Math.Max(0, InteropEmu.DebugGetMemorySize(DebugMemoryType.SaveRam) - 1);

			AddBinding("AddressType", cboRegion);
			AddBinding("Address", txtAddress);
			AddBinding("Label", txtLabel);
			AddBinding("Comment", txtComment);
			AddBinding("Length", nudLength);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			UpdateByteLabel();
			txtLabel.Focus();
		}

		private int GetMaxAddress(AddressType type)
		{
			switch(type) {
				case AddressType.InternalRam: return _maxInternalRamAddress;
				case AddressType.PrgRom: return _maxPrgRomAddress;
				case AddressType.WorkRam: return _maxWorkRamAddress;
				case AddressType.SaveRam: return _maxSaveRamAddress;
				case AddressType.Register: return _maxRegisterAddress;
			}
			return 0;
		}

		protected override bool ValidateInput()
		{
			UpdateObject();

			UInt32 address = ((CodeLabel)Entity).Address;
			UInt32 length = ((CodeLabel)Entity).Length;
			AddressType type = ((CodeLabel)Entity).AddressType;
			CodeLabel sameLabel = LabelManager.GetLabel(txtLabel.Text);
			int maxAddress = GetMaxAddress(type);

			if(maxAddress <= 0) {
				lblRange.Text = "(unavailable)";
			} else {
				lblRange.Text = "($0000 - $" + maxAddress.ToString("X4") + ")";
			}

			for(UInt32 i = 0; i < length; i++) {
				CodeLabel sameAddress = LabelManager.GetLabel(address + i, type);
				if(sameAddress != null) {
					if(_originalLabel == null) {
						//A label already exists and we're not editing an existing label, so we can't add it
						return false;
					} else {
						if(sameAddress.Label != _originalLabel.Label && !sameAddress.Label.StartsWith(_originalLabel.Label + "+")) {
							//A label already exists, we're trying to edit an existing label, but the existing label
							//and the label we're editing aren't the same label.  Can't override an existing label with a different one.
							return false;
						}
					}
				}
			}

			return
				length >= 1 && length <= 65536 &&
				address + (length - 1) <= maxAddress &&
				(sameLabel == null || sameLabel == _originalLabel) 
				&& (_originalLabel != null || txtLabel.Text.Length > 0 || txtComment.Text.Length > 0)
				&& !txtComment.Text.Contains('\x1')
				&& (txtLabel.Text.Length == 0 || LabelManager.LabelRegex.IsMatch(txtLabel.Text));
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

		private void nudLength_ValueChanged(object sender, EventArgs e)
		{
			UpdateByteLabel();
		}

		private void UpdateByteLabel()
		{
			if(nudLength.Value > 1) {
				lblBytes.Text = "bytes";
			} else {
				lblBytes.Text = "byte";
			}
		}
	}
}
