using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Portable_Autofill
{
	public partial class Form1 : Form
	{
		public String getMonth;
		public String getEOD;

		public String returnGetMonth()
		{
			return textBox1.Text;//getMonth;
		}
		public String returnGetEOD()
		{
			return textBox2.Text;
		}
		public Form1()
		{
			InitializeComponent();
		}
		//Autofill
		private void ButtonAutoFill_Click(object sender, EventArgs e)
		{
			ExcelInteract fill = new ExcelInteract();
			fill.autoFill(textBox1.Text, textBox2.Text);

		}
		//Monthly loc
		private void button3_Click(object sender, EventArgs e)
		{
			DialogResult result = openFileDialog2.ShowDialog();

			if (result == DialogResult.OK) // Test result.
			{
				string file = openFileDialog2.FileName;
				textBox1.Text = file;
				getMonth = file;
				Console.WriteLine(file);
			}
			//Console.WriteLine(result);
		}
		//EOD loc
		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult result = openFileDialog2.ShowDialog();
			if (result == DialogResult.OK) // Test result.
			{
				string file = openFileDialog2.FileName;
				textBox2.Text = file;
				getEOD = file;
				Console.WriteLine(file);
			}
			//Console.WriteLine(result);
		}

		private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
		{

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
