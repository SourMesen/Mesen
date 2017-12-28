using Mesen.GUI.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms.Config
{
	public class BaseInputConfigForm : BaseConfigForm
	{
		private TabControl _tabMain;
		private KeyPresets _presets;

		protected KeyPresets Presets
		{
			get
			{
				if(_presets == null) {
					_presets = new KeyPresets();
				}
				return _presets;
			}
		}

		public BaseInputConfigForm()
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			ResourceHelper.ApplyResources(this, typeof(BaseInputConfigForm).Name);

			base.OnLoad(e);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			//Do not save anything, the parent input form will handle the changes
			if(this.DialogResult == DialogResult.OK) {
				UpdateKeyMappings();
				UpdateObject();
			}
			base.OnFormClosed(e);			
		}

		protected override bool ValidateInput()
		{
			UpdateTabIcons();
			return base.ValidateInput();
		}

		protected override void UpdateConfig()
		{
			UpdateKeyMappings();
			base.UpdateConfig();
		}

		protected void SetMainTab(TabControl tabMain)
		{
			_tabMain = tabMain;
		}

		protected BaseInputConfigControl GetControllerControl()
		{
			return GetControllerControl(_tabMain.SelectedTab);
		}

		private BaseInputConfigControl GetControllerControl(TabPage tab)
		{
			return (BaseInputConfigControl)tab.Controls[0];
		}

		private BaseInputConfigControl GetControllerControl(int index)
		{
			return GetControllerControl(_tabMain.Controls[index] as TabPage);
		}

		private void UpdateTabIcon(TabPage tabPage)
		{
			int newIndex = (int)GetControllerControl(tabPage).GetKeyType() - 1;
			if(tabPage.ImageIndex != newIndex) {
				tabPage.ImageIndex = newIndex;
			}
		}

		protected void UpdateTabIcons()
		{
			UpdateTabIcon(_tabMain.Controls[0] as TabPage);
			UpdateTabIcon(_tabMain.Controls[1] as TabPage);
			UpdateTabIcon(_tabMain.Controls[2] as TabPage);
			UpdateTabIcon(_tabMain.Controls[3] as TabPage);
		}

		protected void ClearCurrentTab()
		{
			GetControllerControl().ClearKeys();
		}

		protected void UpdateKeyMappings()
		{
			GetControllerControl(0).UpdateKeyMappings(((ControllerInfo)Entity).Keys[0]);
			GetControllerControl(1).UpdateKeyMappings(((ControllerInfo)Entity).Keys[1]);
			GetControllerControl(2).UpdateKeyMappings(((ControllerInfo)Entity).Keys[2]);
			GetControllerControl(3).UpdateKeyMappings(((ControllerInfo)Entity).Keys[3]);
		}
	}
}
