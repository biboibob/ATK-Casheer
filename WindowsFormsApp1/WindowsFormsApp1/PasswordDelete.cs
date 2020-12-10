using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class PasswordDelete : Form
    {
        public PasswordDelete()
        {
            InitializeComponent();
        }

        private void PasswordDelete_Load(object sender, EventArgs e)
        {
         
            radioButton1.Checked = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.UseSystemPasswordChar = true;
            

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.UseSystemPasswordChar = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "titis" || textBox1.Text == "TITIS")
            {
                // The password is ok.
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // The password is invalid.
                textBox1.Clear();
                MessageBox.Show("Inivalid password.");
                textBox1.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
    }
}
