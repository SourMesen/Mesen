using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;
using Mesen.GUI.Forms.Config;

namespace Mesen.GUI.Forms
{
	public class EntityBinder
	{
		private Dictionary<string, Control> _bindings = new Dictionary<string, Control>();
		private Dictionary<string, eNumberFormat> _fieldFormat = new Dictionary<string, eNumberFormat>();
		private Dictionary<string, FieldInfo> _fieldInfo = null;

		public object Entity { get; set; }

		protected virtual Type BindedType
		{
			get { return Entity.GetType(); }
		}

		public bool Updating { get; private set; }

		public void AddBinding(string fieldName, Control bindedField, eNumberFormat format = eNumberFormat.Default)
		{
			if(BindedType == null) {
				throw new Exception("Need to override BindedType to use bindings");
			}

			if(_fieldInfo == null) {
				_fieldInfo = new Dictionary<string, FieldInfo>();
				FieldInfo[] members = BindedType.GetFields();
				foreach(FieldInfo info in members) {
					_fieldInfo[info.Name] = info;
				}
			}

			if(_fieldInfo.ContainsKey(fieldName)) {
				Type fieldType = _fieldInfo[fieldName].FieldType;
				if(fieldType.IsSubclassOf(typeof(Enum)) && bindedField is ComboBox) {
					BaseConfigForm.InitializeComboBox(((ComboBox)bindedField), fieldType);
				}
				_bindings[fieldName] = bindedField;
				_fieldFormat[fieldName] = format;
			} else {
				throw new Exception("Invalid field name");
			}
		}

		public void UpdateUI()
		{
			this.Updating = true;

			foreach(KeyValuePair<string, Control> kvp in _bindings) {
				if(!_fieldInfo.ContainsKey(kvp.Key)) {
					throw new Exception("Invalid binding key");
				} else {
					FieldInfo field = _fieldInfo[kvp.Key];
					eNumberFormat format = _fieldFormat[kvp.Key];
					object value = field.GetValue(this.Entity);
					if(kvp.Value is TextBox) {
						if(value is IFormattable) {
							kvp.Value.Text = ((IFormattable)value).ToString(format == eNumberFormat.Decimal ? "" : "X", System.Globalization.CultureInfo.InvariantCulture);
						} else {
							kvp.Value.Text = (string)value;
						}
					} else if(kvp.Value is ctrlPathSelection) {
						kvp.Value.Text = (string)value;
					} else if(kvp.Value is CheckBox) {
						((CheckBox)kvp.Value).Checked = Convert.ToBoolean(value);
					} else if(kvp.Value is ctrlRiskyOption) {
						((ctrlRiskyOption)kvp.Value).Checked = Convert.ToBoolean(value);
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
						if(field.FieldType == typeof(Int32)) {
							((ctrlTrackbar)kvp.Value).Value = (int)value;
						} else {
							((ctrlTrackbar)kvp.Value).Value = (int)(uint)value;
						}
					} else if(kvp.Value is ctrlHorizontalTrackbar) {
						((ctrlHorizontalTrackbar)kvp.Value).Value = (int)value;
					} else if(kvp.Value is TrackBar) {
						if(field.FieldType == typeof(Int32)) {
							((TrackBar)kvp.Value).Value = (int)value;
						} else {
							((TrackBar)kvp.Value).Value = (int)(uint)value;
						}
					} else if(kvp.Value is NumericUpDown) {
						NumericUpDown nud = kvp.Value as NumericUpDown;
						decimal val;
						if(field.FieldType == typeof(UInt32)) {
							val = (UInt32)value;
						} else if(field.FieldType == typeof(Int32)) {
							val = (Int32)value;
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
							if(combo.SelectedIndex < 0 && combo.Items.Count > 0) {
								combo.SelectedIndex = 0;
							}
						}
					}
				}
			}

			this.Updating = false;
		}

		public void UpdateObject()
		{
			foreach(KeyValuePair<string, Control> kvp in _bindings) {
				if(!_fieldInfo.ContainsKey(kvp.Key)) {
					throw new Exception("Invalid binding key");
				} else {
					try {
						FieldInfo field = _fieldInfo[kvp.Key];
						eNumberFormat format = _fieldFormat[kvp.Key];
						if(kvp.Value is TextBox) {
							object value = kvp.Value.Text;
							NumberStyles numberStyle = format == eNumberFormat.Decimal ? NumberStyles.Integer : NumberStyles.HexNumber;
							value = ((string)value).Trim().Replace("$", "").Replace("0x", "");
							if(field.FieldType != typeof(string) && string.IsNullOrWhiteSpace((string)value)) {
								value = "0";
							}
							if(field.FieldType == typeof(UInt32)) {
								value = (object)UInt32.Parse((string)value, numberStyle);
							} else if(field.FieldType == typeof(Int32)) {
								value = (object)Int32.Parse((string)value, numberStyle);
							} else if(field.FieldType == typeof(Byte)) {
								value = (object)Byte.Parse((string)value, numberStyle);
							} else if(field.FieldType == typeof(UInt16)) {
								value = (object)UInt16.Parse((string)value, numberStyle);
							}
							field.SetValue(Entity, value);
						} else if(kvp.Value is ctrlPathSelection) {
							field.SetValue(Entity, ((ctrlPathSelection)kvp.Value).Text);
						} else if(kvp.Value is CheckBox) {
							if(field.FieldType == typeof(bool)) {
								field.SetValue(Entity, ((CheckBox)kvp.Value).Checked);
							} else if(field.FieldType == typeof(byte)) {
								field.SetValue(Entity, ((CheckBox)kvp.Value).Checked ? (byte)1 : (byte)0);
							}
						} else if(kvp.Value is ctrlRiskyOption) {
							if(field.FieldType == typeof(bool)) {
								field.SetValue(Entity, ((ctrlRiskyOption)kvp.Value).Checked);
							} else if(field.FieldType == typeof(byte)) {
								field.SetValue(Entity, ((ctrlRiskyOption)kvp.Value).Checked ? (byte)1 : (byte)0);
							}
						} else if(kvp.Value is RadioButton) {
							field.SetValue(Entity, ((RadioButton)kvp.Value).Checked);
						} else if(kvp.Value is Panel) {
							field.SetValue(Entity, kvp.Value.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Tag);
						} else if(kvp.Value is ctrlTrackbar) {
							if(field.FieldType == typeof(Int32)) {
								field.SetValue(Entity, (Int32)((ctrlTrackbar)kvp.Value).Value);
							} else {
								field.SetValue(Entity, (UInt32)((ctrlTrackbar)kvp.Value).Value);
							}
						} else if(kvp.Value is ctrlHorizontalTrackbar) {
							field.SetValue(Entity, (Int32)((ctrlHorizontalTrackbar)kvp.Value).Value);
						} else if(kvp.Value is TrackBar) {
							if(field.FieldType == typeof(Int32)) {
								field.SetValue(Entity, ((TrackBar)kvp.Value).Value);
							} else {
								field.SetValue(Entity, (UInt32)((TrackBar)kvp.Value).Value);
							}
						} else if(kvp.Value is NumericUpDown) {
							if(field.FieldType == typeof(UInt32)) {
								field.SetValue(Entity, (UInt32)((NumericUpDown)kvp.Value).Value);
							} else if(field.FieldType == typeof(Int32)) {
								field.SetValue(Entity, (Int32)((NumericUpDown)kvp.Value).Value);
							} else {
								field.SetValue(Entity, (double)((NumericUpDown)kvp.Value).Value);
							}
						} else if(kvp.Value is ComboBox) {
							if(field.FieldType.IsSubclassOf(typeof(Enum))) {
								Enum enumValue = ((ComboBox)kvp.Value).GetEnumValue(field.FieldType);
								if(enumValue != null) {
									field.SetValue(Entity, enumValue);
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
	}

	public enum eNumberFormat
	{
		Default,
		Hex,
		Decimal,
	}
}
