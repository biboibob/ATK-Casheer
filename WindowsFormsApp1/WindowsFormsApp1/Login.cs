using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
      
        public static MySqlConnection conn = new MySqlConnection("server=bima-id.com;port=3306;username=bimaidco_atk;password=bobbycool123;database=bimaidco_atk;Allow Zero Datetime=true");
        public Login()
        {
            InitializeComponent();
        }

        public static string passuname;

        private void Login_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "select username, password from user where username='" + textBox1.Text + "'and password='" + textBox2.Text + "'";
                MySqlCommand cm = new MySqlCommand(query, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cm);
              
                DataTable dt = new DataTable();
                Main op = new Main();
                da.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Login Sukses, Selamat datang di program ATK-Bidori");
                    passuname = textBox1.Text;                         
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;

                    this.Hide();
                    op.ShowDialog();
                    
                }
                
                else
                {
                    MessageBox.Show("Login Gagal, Silahkan check username atau password anda lagi");

                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       
    }
}
