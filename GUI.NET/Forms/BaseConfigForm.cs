using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms
{
	public partial class BaseConfigForm : BaseForm
	{
		private Dictionary<string, Control> _bindings = new Dictionary<string, Control>();
		private Dictionary<string, FieldInfo> _fieldInfo = null;
		private object _entity;
		private Timer _validateTimer;
		
		public BaseConfigForm()
		{
			InitializeComponent();

			_validateTimer = new Timer();
			_validateTimer.Interval = 50;
			_validateTimer.Tick += OnValidateInput;
			_validateTimer.Start();

			this.ShowInTaskbar = false;
		}

		public new bool ShowInTaskbar
		{
			set
			{
				base.ShowInTaskbar = false;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			UpdateUI();
		}

		private void OnValidateInput(object sender, EventArgs e)
		{
			btnOK.Enabled = ValidateInput();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if(DialogResult == System.Windows.Forms.DialogResult.OK) {
				if(!ValidateInput()) {
					e.Cancel = true;
				} else {
					_validateTimer.Tick -= OnValidateInput;
					_validateTimer.Stop();
				}
			}
			base.OnFormClosing(e);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(this.DialogResult == System.Windows.Forms.DialogResult.OK) {
				UpdateObject();
				UpdateConfig();
				if(ApplyChangesOnOK) {
					ConfigManager.ApplyChanges();
				}
			} else {
				ConfigManager.RejectChanges();
			}
			base.OnFormClosed(e);
		}

		protected virtual bool ApplyChangesOnOK
		{
			get { return true; }
		}

		protected virtual void UpdateConfig()
		{
		}

		protected bool Updating
		{
			get;
			private set;
		}

		protected object Entity
		{
			get { return _entity; }
			set { _entity = value; }
		}

		protected virtual Type BindedType
		{
			get { return _entity.GetType(); }
		}

		protected virtual bool ValidateInput()
		{
			return true;
		}

		protected void AddBinding(string fieldName, RadioButton trueRadio, RadioButton falseRadio)
		{
			falseRadio.Checked = true;
			AddBinding(fieldName, trueRadio);
		}

		protected void AddBinding(string fieldName, Control bindedField)
		{
			if(BindedType == null) {
				throw new Exception("Need to override BindedType to use bindings");
			}

			if(_fieldInfo == null) {
				_fieldInfo = new Dictionary<string,FieldInfo>();
				FieldInfo[] members = BindedType.GetFields();
				foreach(FieldInfo info in members) {
					_fieldInfo[info.Name] = info;
				}
			}

			if(_fieldInfo.ContainsKey(fieldName)) {
				Type fieldType = _fieldInfo[fieldName].FieldType;
				if(fieldType.IsSubclassOf(typeof(Enum)) && bindedField is ComboBox) {
					ComboBox combo = ((ComboBox)bindedField);
					combo.DropDownStyle = ComboBoxStyle.DropDownList;
					combo.Items.Clear();
					foreach(Enum value in Enum.GetValues(fieldType)) {
						combo.Items.Add(ResourceHelper.GetEnumText(value));
					}
				}
				_bindings[fieldName] = bindedField;
			} else {
				throw new Exception("Invalid field name");
			}
		}

		protected void UpdateUI()
		{
			this.Updating = true;

			foreach(KeyValuePair<string, Control> kvp in _bindings) {
				if(!_fieldInfo.ContainsKey(kvp.Key)) {
					throw new Exception("Invalid binding key");
				} else {
					FieldInfo field = _fieldInfo[kvp.Key];
					object value = field.GetValue(this.Entity);
					if(kvp.Value is TextBox) {
						if(field.FieldType == typeof(UInt32)) {
							kvp.Value.Text = ((UInt32)value).ToString("X");
						} else if(field.FieldType == typeof(Byte)) {
							kvp.Value.Text = ((Byte)value).ToString("X");
						} else {
							kvp.Value.Text = (string)value;
						}
					} else if(kvp.Value is CheckBox) {
						((CheckBox)kvp.Value).Checked = (bool)value;
					} else if(kvp.Value is RadioButton) {
						((RadioButton)kvp.Value).Checked = (bool)value;
					} else if(kvp.Value is Panel) {
						RadioButton radio = kvp.Value.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Tag.Equals(value));
						if(radio != null) {
							radio.Checked = true;
						} else {
							throw new Exception("No radio button matching value found");
						}
					} else if(kvp.Value is ctrlTrackbar) {
						((ctrlTrackbar)kvp.Value).Value = (int)(uint)value;
					} else if(kvp.Value is TrackBar) {
						((TrackBar)kvp.Value).Value = (int)(uint)value;
					} else if(kvp.Value is NumericUpDown) {
						NumericUpDown nud = kvp.Value as NumericUpDown;
						decimal val;
						if(field.FieldType == typeof(UInt32)) {
							val = (uint)value;
						} else {
							val = (decimal)(double)value;
						}
						val = Math.Min(Math.Max(val, nud.Minimum), nud.Maximum);
						nud.Value = val;
					} else if(kvp.Value is ComboBox) {
						ComboBox combo = kvp.Value as ComboBox;
						if(value is Enum) {
							combo.SelectedItem = ResourceHelper.GetEnumText((Enum)value);
						} else if(field.FieldType == typeof(UInt32)) {
							for(int i = 0, len = combo.Items.Count; i < len; i++) {
								UInt32 numericValue;
								string item = Regex.Replace(combo.Items[i].ToString(), "[^0-9]", "");
								if(UInt32.TryParse(item, out numericValue)) {
									if(numericValue == (UInt32)value) {
										combo.SelectedIndex = i;
										break;
									}
								}
							}
						} else if(field.FieldType == typeof(string)) {
							combo.SelectedItem = value;
							if(combo.SelectedIndex < 0) {
								combo.SelectedIndex = 0;
							}
						}
					}
				}
			}

			this.Updating = false;

			this.AfterUpdateUI();
		}

		virtual protected void AfterUpdateUI()
		{
		}

		protected void UpdateObject()
		{
			foreach(KeyValuePair<string, Control> kvp in _bindings) {
				if(!_fieldInfo.ContainsKey(kvp.Key)) {
					throw new Exception("Invalid binding key");
				} else {
					try {
						FieldInfo field = _fieldInfo[kvp.Key];
						if(kvp.Value is TextBox) {
							object value = kvp.Value.Text;
							if(field.FieldType == typeof(UInt32)) {
								value = ((string)value).Trim().Replace("$", "").Replace("0x", "");
								value = (object)UInt32.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
							} else if(field.FieldType == typeof(Byte)) {
								value = (object)Byte.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
							}
							field.SetValue(Entity, value);
						} else if(kvp.Value is CheckBox) {
							field.SetValue(Entity, ((CheckBox)kvp.Value).Checked);
						} else if(kvp.Value is RadioButton) {
							field.SetValue(Entity, ((RadioButton)kvp.Value).Checked);
						} else if(kvp.Value is Panel) {
							field.SetValue(Entity, kvp.Value.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Tag);
						} else if(kvp.Value is ctrlTrackbar) {
							field.SetValue(Entity, (UInt32)((ctrlTrackbar)kvp.Value).Value);
						} else if(kvp.Value is TrackBar) {
							field.SetValue(Entity, (UInt32)((TrackBar)kvp.Value).Value);
						} else if(kvp.Value is NumericUpDown) {
							if(field.FieldType == typeof(UInt32)) {
								field.SetValue(Entity, (UInt32)((NumericUpDown)kvp.Value).Value);
							} else {
								field.SetValue(Entity, (double)((NumericUpDown)kvp.Value).Value);
							}
						} else if(kvp.Value is ComboBox) {
							if(field.FieldType.IsSubclassOf(typeof(Enum))) {
								object selectedItem = ((ComboBox)kvp.Value).SelectedItem;

								foreach(Enum value in Enum.GetValues(field.FieldType)) {
									if(ResourceHelper.GetEnumText(value) == selectedItem.ToString()) {
										field.SetValue(Entity, value);
										break;
									}
								}
							} else if(field.FieldType == typeof(UInt32)) {
								UInt32 numericValue;
								string item = Regex.Replace(((ComboBox)kvp.Value).SelectedItem.ToString(), "[^0-9]", "");
								if(UInt32.TryParse(item, out numericValue)) {
									field.SetValue(Entity, numericValue);
								}
							} else if(field.FieldType == typeof(string)) {
								field.SetValue(Entity, ((ComboBox)kvp.Value).SelectedItem);
							}
						}
					} catch {
						//Ignore exceptions caused by bad user input
					}
				}
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
