using System;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms
{
	public partial class BaseConfigForm : BaseForm
	{
		private EntityBinder _binder;
		private object _entity;
		private Timer _validateTimer;
		
		public BaseConfigForm()
		{
			InitializeComponent();

			_validateTimer = new Timer();
			_validateTimer.Interval = 50;
			_validateTimer.Tick += OnValidateInput;
			_validateTimer.Start();

			_binder = new EntityBinder();

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

		protected void UpdateUI()
		{
			_binder.UpdateUI();
			this.AfterUpdateUI();
		}

		protected void UpdateObject()
		{
			_binder.UpdateObject();
			UpdateConfig();
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
			get { return _binder.Updating; }
		}

		protected object Entity
		{
			get { return _entity; }
			set
			{
				_binder.Entity = value;
				_entity = value;
			}
		}

		protected virtual bool ValidateInput()
		{
			return true;
		}

		protected void AddBinding(string fieldName, RadioButton trueRadio, RadioButton falseRadio)
		{
			falseRadio.Checked = true;
			_binder.AddBinding(fieldName, trueRadio);
		}

		protected void AddBinding(string fieldName, Control bindedField)
		{
			_binder.AddBinding(fieldName, bindedField);
		}

		public static void InitializeComboBox(ComboBox combo, Type enumType)
		{
			combo.DropDownStyle = ComboBoxStyle.DropDownList;
			combo.Items.Clear();
			foreach(Enum value in Enum.GetValues(enumType)) {
				combo.Items.Add(ResourceHelper.GetEnumText(value));
			}
		}

		virtual protected void AfterUpdateUI()
		{
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

	public static class ComboBoxExtensions
	{
		public static Enum GetEnumValue(this ComboBox cbo, Type enumType)
		{
			foreach(Enum value in Enum.GetValues(enumType)) {
				if(ResourceHelper.GetEnumText(value) == cbo.SelectedItem.ToString()) {
					return value;
				}
			}

			return null;
		}

		public static T GetEnumValue<T>(this ComboBox cbo)
		{
			foreach(Enum value in Enum.GetValues(typeof(T))) {
				if(ResourceHelper.GetEnumText(value) == cbo.SelectedItem.ToString()) {
					return (T)(object)value;
				}
			}

			return default(T);
		}

		public static void SetEnumValue<T>(this ComboBox cbo, T value)
		{
			for(int i = 0; i < cbo.Items.Count; i++) {
				if(ResourceHelper.GetEnumText((Enum)(object)value) == cbo.Items[i].ToString()) {
					cbo.SelectedIndex = i;
					break;
				}
			}
		}
	}
}
