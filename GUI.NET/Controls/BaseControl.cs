using System.Windows.Forms;
using System.Drawing;
using Mesen.GUI;

namespace Mesen.GUI.Controls
{
	public class BaseControl : UserControl
	{
		public static float DefaultFontSize = Program.IsMono ? 10 : 12;
				
		public static string MonospaceFontFamily
		{
			get
			{
				if(Program.IsMono) {
					return "DroidSansMono";
				} else {
					return "Consolas";
				}
			}
		}

		public new AutoScaleMode AutoScaleMode
		{
			set {
				if(Program.IsMono) { 
					base.AutoScaleMode = AutoScaleMode.None;
				} else {
					base.AutoScaleMode = value;
				}
			}
		}
	}
}