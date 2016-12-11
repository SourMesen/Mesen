using System.Windows.Forms;
using System.Drawing;
using Mesen.GUI;

namespace Mesen.GUI.Controls
{
	public class BaseControl : UserControl
	{
		public static string MonospaceFontFamily
		{
			get
			{
				if(Program.IsMono) {
					return "Ubuntu Mono";
				} else {
					return "Consolas";
				}
			}
		}

		public new SizeF AutoScaleDimensions
		{
			set { 
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