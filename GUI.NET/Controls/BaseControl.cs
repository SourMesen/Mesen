using System.Windows.Forms;
using System.Drawing;
using Mesen.GUI;
using System;

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

		public static Bitmap DownArrow
		{
			get
			{
				if(!Program.IsMono && Environment.OSVersion.Version >= new Version(6, 2)) {
					return Properties.Resources.DownArrowWin10;
				} else {
					return Properties.Resources.DownArrow;
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

		public new SizeF AutoScaleDimensions
		{
			set 
			{ 
				if(!Program.IsMono) { 
					base.AutoScaleDimensions = value; 
				}
			}
		} 		
	}
}