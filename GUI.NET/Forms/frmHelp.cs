using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Forms
{
	public partial class frmHelp : BaseForm
	{
		public frmHelp()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			lblExample.Font = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize - 2);
			txtAudioOptions.Font = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize - 4);
			txtEmulationOptions.Font = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize - 4);
			txtVideoOptions.Font = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize - 4);
			txtGeneralOptions.Font = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize - 4);

			lblExample.Text = ConvertSlashes(lblExample.Text);

			StringBuilder sb = new StringBuilder();
			DisplayOptions(typeof(VideoInfo), sb);
			txtVideoOptions.Text = ConvertSlashes(sb.ToString().Trim());

			sb.Clear();
			DisplayOptions(typeof(AudioInfo), sb);
			txtAudioOptions.Text = ConvertSlashes(sb.ToString().Trim());

			sb.Clear();
			DisplayOptions(typeof(EmulationInfo), sb);
			txtEmulationOptions.Text = ConvertSlashes(sb.ToString().Trim());

			sb.Clear();
			DisplayOptions(typeof(Configuration), sb);

			txtGeneralOptions.Text = ConvertSlashes(
				ResourceHelper.GetMessage("HelpFullscreen") + Environment.NewLine +
				ResourceHelper.GetMessage("HelpDoNotSaveSettings") + Environment.NewLine +
				ResourceHelper.GetMessage("HelpRecordMovie") + Environment.NewLine +
				ResourceHelper.GetMessage("HelpLoadLastSession") + Environment.NewLine +
				sb.ToString().Trim()
			);
		}

		private string ConvertSlashes(string options)
		{
			if(Program.IsMono) {
				return options.Replace("/", "--");
			}
			return options;
		}
		
		private void DisplayOptions(Type type, StringBuilder sb)
		{
			FieldInfo[] fields = type.GetFields();
			foreach(FieldInfo info in fields) {
				if(info.FieldType == typeof(int) || info.FieldType == typeof(uint) || info.FieldType == typeof(double)) {
					MinMaxAttribute minMaxAttribute = info.GetCustomAttribute(typeof(MinMaxAttribute)) as MinMaxAttribute;
					if(minMaxAttribute != null) {
						sb.AppendLine("/" + info.Name + " = [" + minMaxAttribute.Min.ToString() + ", " + minMaxAttribute.Max.ToString() + "]");
					} else {
						ValidValuesAttribute validValuesAttribute = info.GetCustomAttribute(typeof(ValidValuesAttribute)) as ValidValuesAttribute;
						if(validValuesAttribute != null) {
							sb.AppendLine("/" + info.Name + " = " + string.Join(" | ", validValuesAttribute.ValidValues));
						}
					}
				} else if(info.FieldType == typeof(bool)) {
					sb.AppendLine("/" + info.Name + " = true | false");
				} else if(info.FieldType.IsEnum) {
					sb.AppendLine("/" + info.Name + " = " + string.Join(" | ", Enum.GetNames(info.FieldType)));
				}
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
