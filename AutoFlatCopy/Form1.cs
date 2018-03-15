using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFlatCopy
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		public void SetSettings(string srcFolder, string dstFolder)
		{
			this.textBox1.Text = srcFolder;
			this.textBox2.Text = dstFolder;
		}

		private void button1_Click(object sender, EventArgs e)
		{	
			FlatCopyApplicationContext.sourceFolder = this.textBox1.Text;
			FlatCopyApplicationContext.destFolder = this.textBox2.Text;
			FlatCopyApplicationContext.filter = this.textBoxFilter.Text;
			FlatCopyApplicationContext.SaveSettings();
			this.Close();
		}

		//copy all now
		private void button2_Click(object sender, EventArgs e)
		{
			if (DialogResult.OK == MessageBox.Show(
					this, 
					"This will DELETE ALL files in the Target Folder, and copy the files from the source folder into the target folder (flattened)", "Delete all files in Target Folder?", 
					MessageBoxButtons.OKCancel, 
					MessageBoxIcon.Warning))
				{
				FlatCopyApplicationContext.sourceFolder = this.textBox1.Text;
				FlatCopyApplicationContext.destFolder = this.textBox2.Text;
				FlatCopyApplicationContext.filter = this.textBoxFilter.Text;

				FlatCopyApplicationContext.ResyncAllNow();
			}
		}
	}
}
