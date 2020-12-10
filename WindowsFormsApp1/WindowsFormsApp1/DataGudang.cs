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
    public partial class DataGudang : Form
    {
        public DataGudang()
        {
            InitializeComponent();
            BindData();
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void DataGudang_Load(object sender, EventArgs e)
        {
          
        }

        private void BindData()
        {
            DataTable dt = new DataTable();
            string Connection = "Database Username and Password Here";
            MySqlConnection conn = new MySqlConnection(Connection);

            MySqlCommand read = new MySqlCommand("select id_barang,barang,barcode,penginput,tglmasuk,tgl_update_akhir,stock,harga_jual from gudang ORDER BY LENGTH(id_barang),id_barang", conn);
            conn.Open();
            MySqlDataReader reader = read.ExecuteReader();
            dt.Load(reader);

            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
        }

        public void SearchData(string search)
        {
            string Connection = "Database Username and Password Here";
            MySqlConnection conn = new MySqlConnection(Connection);

            conn.Open();
            string query = "select id_barang,barang,barcode,penginput,tglmasuk,stock,harga_jual from gudang WHERE barang like '%" + find.Text + "%' ORDER BY LENGTH(id_barang),id_barang";
            MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void find_TextChanged(object sender, EventArgs e)
        {
            SearchData(find.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void kasirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main mn = new Main();
            this.Hide();
            mn.ShowDialog();
        }

        private void dataOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataOrder dor = new DataOrder();
            this.Hide();
            dor.ShowDialog();
        }

        private void dataTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTransaksi dtr = new DataTransaksi();
            this.Hide();
            dtr.ShowDialog();
        }
    }
}
