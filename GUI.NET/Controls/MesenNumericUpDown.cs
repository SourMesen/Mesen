using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Controls
{
	class MesenNumericUpDown : BaseControl
	{
		private NumericUpDown nud;

		public event EventHandler ValueChanged
		{
			add { nud.ValueChanged += value; }
			remove { nud.ValueChanged -= value; }
		}

		public new event EventHandler Validated
		{
			add { nud.Validated += value; }
			remove { nud.Validated -= value; }
		}

		public new event EventHandler Click
		{
			add { nud.Click += value; }
			remove { nud.Click -= value; }
		}

		public new event KeyEventHandler KeyDown
		{
			add { nud.KeyDown += value; }
			remove { nud.KeyDown -= value; }
		}

		public decimal Value
		{
			get { return nud.Value; }
			set
			{
				nud.Text = value.ToString();
				nud.Value = value;
			}
		}

		public new string Text
		{
			get { return nud.Text; }
			set { nud.Text = value; }
		}

		public decimal Maximum
		{
			get { return nud.Maximum; }
			set { nud.Maximum = value; }
		}

		public decimal Minimum
		{
			get { return nud.Minimum; }
			set { nud.Minimum = value; }
		}

		public decimal Increment
		{
			get { return nud.Increment; }
			set { nud.Increment = value; }
		}

		public int DecimalPlaces
		{
			get { return nud.DecimalPlaces; }
			set { nud.DecimalPlaces = value; }
		}

		public MesenNumericUpDown()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.nud = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.nud)).BeginInit();
			this.SuspendLayout();
			// 
			// nud
			// 
			this.nud.AutoSize = true;
			this.nud.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nud.Location = new System.Drawing.Point(0, 0);
			this.nud.Name = "nud";
			this.nud.Size = new System.Drawing.Size(48, 20);
			this.nud.TabIndex = 0;
			// 
			// MesenNumericUpDown
			// 
			this.Controls.Add(this.nud);
			this.MaximumSize = new Size(10000, 20);
			this.Name = "MesenNumericUpDown";
			this.Size = new System.Drawing.Size(48, 21);
			((System.ComponentModel.ISupportInitialize)(this.nud)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
