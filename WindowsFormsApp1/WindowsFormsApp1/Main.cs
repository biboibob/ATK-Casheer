using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagingToolkit.Barcode;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Drawing.Printing;
using System.Runtime.InteropServices;



namespace WindowsFormsApp1
{
    public partial class Main : Form
    {
        
        public static MySqlConnection conn = new MySqlConnection("server=bima-id.com;port=3306;username=bimaidco_atk;password=bobbycool123;database=bimaidco_atk;Allow Zero Datetime=true");
        public Main()
        {
            InitializeComponent();

        }

        BarcodeEncoder Generator;
        BarcodeDecoder Scanner;


        private void Main_Load(object sender, EventArgs e)
        {


            label15.Text = Login.passuname;
            dateTimePicker1.MaxDate = dateTimePicker1.MinDate = DateTime.Now;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.ActiveControl = barcode;
            read();
            


            dataGridView1.ColumnCount = 7;
          
            dataGridView1.Columns[0].Name = "barang";
            dataGridView1.Columns[1].Name = "barcode";
            dataGridView1.Columns[2].Name = "kode_order";
            dataGridView1.Columns[3].Name = "tanggal";
            dataGridView1.Columns[4].Name = "jumlah";
            dataGridView1.Columns[5].Name = "harga";
            dataGridView1.Columns[6].Name = "Diskon";

            //MEMBERIKAN KODE ORDER

            try
            {

                string query = "select max(kode_order) from dataorder";
                conn.Open();
                MySqlCommand comm = new MySqlCommand(query, conn);
                MySqlDataReader reader = comm.ExecuteReader();


                while (reader.Read())
                {
                    int value = int.Parse(reader[0].ToString()) + 1;
                    no.Text = value.ToString();

                }
                conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void read()
        {
            akutot.ReadOnly = true;
            barang.ReadOnly = true;
            harga.ReadOnly = true;
        }

      

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

  

        private void barcode_TextChanged(object sender, EventArgs e)
        {
            try
            {

                string query = "select * from gudang where barcode = '" + barcode.Text + "'";
                conn.Open();
                MySqlCommand comm = new MySqlCommand(query, conn);
                MySqlDataReader reader = comm.ExecuteReader();


                while (reader.Read())
                {
                    string rbarang = reader.GetString("barang");
                    string rharga = reader.GetInt32("harga_jual").ToString();
                    string rDiskon = reader.GetInt32("Diskon").ToString();
                    barang.Text = rbarang;
                    harga.Text  = rharga;
                    Diskon.Text = rDiskon;
                  
                }
                conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           
        }

       

        private void button1_Click(object sender, EventArgs e)
        {

            
            if (barcode.Text == "")
            {
                MessageBox.Show("Barcode belum diinput!");
            }

            else if (jumlah.Value == 0)
            {
                MessageBox.Show("Jumlah Tidak Boleh 0!");
            }

            else
            {
                var hasilperkalian = int.Parse(harga.Text) * jumlah.Value;
                string barcodee = barcode.Text;
                string barangg = barang.Text;
                string hargaa = hasilperkalian.ToString();
                string jumlahh = jumlah.Value.ToString();
                string tanggall = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string kodee = no.Text;
                string diskonn = Diskon.Text;

                string[] rows = { barangg, barcodee, kodee,tanggall,jumlahh, hargaa, diskonn };
                dataGridView1.Rows.Add(rows);


            }
                                  
                int sum = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
                }
                akutot.Text = sum.ToString();

            barcode.Text = String.Empty;
            barang.Text = String.Empty;
            harga.Text = String.Empty;
            Diskon.Text = String.Empty;
            jumlah.Value = 0;
            barcode.Focus();
                            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < dataGridView1.Rows.Count; ++j)
            {
                for (int x = 0; x < dataGridView1.Rows.Count; ++x)
                {

                    if (dataGridView1.Rows[j].Cells[x].Value == null)
                    {
                        MessageBox.Show("Belum ada Data Order");
                    }
                    else
                    {
                        try
                        {
                            /*int rowIndex = dataGridView1.CurrentRow.Index;
                            dataGridView1.Rows.RemoveAt(rowIndex);*/


                            PasswordDelete dlt = new PasswordDelete();
                            if (dlt.ShowDialog() == DialogResult.OK)
                            {

                                foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                                {
                                    dataGridView1.Rows.RemoveAt(item.Index);
                                }

                                int dec = 0;
                                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                                {
                                    dec -= Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
                                }

                                var dectot = Math.Abs(dec);
                                akutot.Text = dectot.ToString();
                            }


                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
                      
           
        }

        // PRINT RECEIPT //


     

        void PrintPage(object sender, PrintPageEventArgs e)
        {

          

            //Pembuatan Struk dari sini

            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;

            Graphics graphics = e.Graphics;
            Font font10 = new Font("Courier New", 3);
            Font font12 = new Font("Courier New", 5);
            Font font14 = new Font("Courier New", 7);
            Font font15 = new Font("StrikeOut", 7);


            float leading = 4;
            float lineheight10 = font10.GetHeight() + leading;
            float lineheight12 = font12.GetHeight() + leading;
            float lineheight14 = font14.GetHeight() + leading;
            float lineheight15 = font15.GetHeight() + leading;

            float startX = 0;
            float startY = leading;
            float Offset = 0;

            StringFormat formatLeft = new StringFormat(StringFormatFlags.NoClip);
            StringFormat formatCenter = new StringFormat(formatLeft);
            StringFormat formatRight = new StringFormat(formatLeft);

            formatCenter.Alignment = StringAlignment.Center;
            formatRight.Alignment = StringAlignment.Far;
            formatLeft.Alignment = StringAlignment.Near;

            SizeF layoutSize = new SizeF(200 - Offset * 2, lineheight14); //ATUR LEBAR DARI LAYOUT HASIL PRINT
          
            RectangleF layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize); //pembatas setiap row
      
            StringFormat strFormat = new StringFormat(); //pengaturan word spacing setiap item
            

            Brush brush = Brushes.Black;

            graphics.DrawString("ATK - Bidori", font14, brush, layout, formatCenter);
            Offset = Offset + lineheight14;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("Kode Transaksi :" + no.Text, font12, brush, layout, formatLeft);
            Offset = Offset + lineheight12;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("Tanggal :" + date, font12, brush, layout, formatLeft);
            Offset = Offset + lineheight12;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("Kasir :" + label15.Text, font12, brush, layout, formatLeft);
            Offset = Offset + lineheight12;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("".PadRight(70, '_'), font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10;

            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                               

                layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
                graphics.DrawString(dataGridView1.Rows[i].Cells["barang"].Value.ToString().PadLeft(5), font14, brush, layout, formatLeft);
                graphics.DrawString(dataGridView1.Rows[i].Cells["jumlah"].Value.ToString().PadLeft(20), font14, brush, layout, formatLeft);
                graphics.DrawString(dataGridView1.Rows[i].Cells["harga"].Value.ToString().PadLeft(30), font14, brush, layout, formatLeft);

              
                if (Convert.ToInt32(dataGridView1.Rows[i].Cells["Diskon"].Value) > 0)
                {
                    
                    Offset = Offset + lineheight15;
                    layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
                    var calculate = (Convert.ToInt32(dataGridView1.Rows[i].Cells["harga"].Value) / (100 - Convert.ToInt32(dataGridView1.Rows[i].Cells["Diskon"].Value)));
                    var ResultCalculate = (calculate * 100);

                    graphics.DrawString("Diskon : ".PadLeft(25), font14, brush, layout);
                    graphics.DrawString(ResultCalculate.ToString().PadLeft(72), font15, brush, layout);
                   
                }

                Offset = Offset + lineheight12;
                
            }

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("".PadRight(70, '_'), font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("Total Pembayaran " + akutot.Text.PadLeft(12), font14, brush, layout, formatLeft);
            Offset = Offset + lineheight12;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("".PadRight(70, '_'), font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("Uang " + uang.Text.PadLeft(24), font14, brush, layout, formatLeft);
            Offset = Offset + lineheight12;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("Kembali " + kembali.Text.PadLeft(22), font14, brush, layout, formatLeft);
            Offset = Offset + lineheight12;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("".PadRight(70, '_'), font10, brush, layout, formatLeft);
            Offset = Offset + lineheight10;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("Jalan Pulogebang Permai Blok FD No 4", font12, brush, layout, formatCenter);
            Offset = Offset + lineheight10;

            layout = new RectangleF(new PointF(startX, startY + Offset), layoutSize);
            graphics.DrawString("No Telp. (021)46821454", font12, brush, layout, formatCenter);
            Offset = Offset + lineheight10;
        }

      
        
        private void button3_Click(object sender, EventArgs e)
        {
           

            if (int.Parse(uang.Text) < int.Parse(akutot.Text))
            {
                MessageBox.Show("Uang Anda Kurang!");
            }

            else if (uang.Text == "" || int.Parse(uang.Text) == 0)
            {
                MessageBox.Show("Anda Belum Memasukan Nominal!");
            }

            else if (int.Parse(akutot.Text) == 0)
            {
                MessageBox.Show("Total Bayar Anda Masih 0!");
            }


            else if (akutot.Text == "")
            {
                MessageBox.Show("Anda belum Menginputkan uang atau anda belum mengorder barang");
            }

            
            else
            {
                var kembalian = int.Parse(uang.Text) - int.Parse(akutot.Text);
                kembali.Text = kembalian.ToString();

                //baut struk sehabis melakukan transaksi 

                PrintDocument prtDoc = new PrintDocument(); //membuat dokumen
                //prtDialog.Document = prtDoc; //deklarasi bahwa print dialog document harus print document
               

                prtDoc.PrinterSettings.PrinterName = "POS-58";


                prtDoc.PrintPage += new PrintPageEventHandler(PrintPage); //menentukan seberapa lebar dan tinggi page untuk setiap struk
                   
                prtDoc.Print();
                

                //////////////////////////////////////////////////

                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        try
                        {
                            string query = "UPDATE  gudang SET stock = stock - @jumlahkurang WHERE barang = @barangg";
                            conn.Open();
                            MySqlCommand min = new MySqlCommand(query, conn);
                            min.Parameters.AddWithValue("jumlahkurang", dataGridView1.Rows[i].Cells[4].Value ?? DBNull.Value);
                            min.Parameters.AddWithValue("barangg", dataGridView1.Rows[i].Cells[0].Value ?? DBNull.Value);
                            min.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        //string query = "insert into dataorder (barcode,barang,jumlah,harga) values ('"+ dataGridView1.Rows[i].Cells[0]+"','"+ dataGridView1.Rows[i].Cells[1]+"','"+ dataGridView1.Rows[i].Cells[2]+ "','" + dataGridView1.Rows[i].Cells[3] + "')";
                        try
                        {
                            string query = "insert into dataorder (karyawan,barang,barcode,kode_order,tanggal,jumlah) values ('" + label15.Text + "',@barangs,@barcodes,@kode_orders,@tanggals,@jumlahs)";
                            conn.Open();
                            MySqlCommand cm = new MySqlCommand(query, conn);

                            cm.Parameters.AddWithValue("barcodes", dataGridView1.Rows[i].Cells[1].Value ?? DBNull.Value);
                            cm.Parameters.AddWithValue("barangs", dataGridView1.Rows[i].Cells[0].Value ?? DBNull.Value);
                            cm.Parameters.AddWithValue("jumlahs", dataGridView1.Rows[i].Cells[4].Value ?? DBNull.Value);
                            cm.Parameters.AddWithValue("kode_orders", dataGridView1.Rows[i].Cells[2].Value ?? DBNull.Value);
                            cm.Parameters.AddWithValue("tanggals", dataGridView1.Rows[i].Cells[3].Value ?? DBNull.Value);


                            cm.ExecuteNonQuery();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }

                MessageBox.Show("Data Transaksi Telah Terinput!");

                try
                    {
                        string query = "insert into datatransaksi (karyawan,kode_order,tanggal,uang,kembali,total) values ('" + label15.Text + "','" + no.Text + "','" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" + uang.Text + "','" + kembali.Text + "','" + akutot.Text + "')";
                        conn.Open();
                        MySqlCommand cm = new MySqlCommand(query, conn);
                        cm.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        akutot.Text = string.Empty;
                        uang.Text = string.Empty;
                        kembali.Text = string.Empty;
                    }
                                             

            }



                try
                {

                    string query = "select max(kode_order) from dataorder";
                    conn.Open();
                    MySqlCommand comm = new MySqlCommand(query, conn);
                    MySqlDataReader reader = comm.ExecuteReader();


                    while (reader.Read())
                    {
                        int value = int.Parse(reader[0].ToString()) + 1;
                        no.Text = value.ToString();

                    }
                    conn.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                dataGridView1.Rows.Clear();
            }
        

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            DataOrder dow = new DataOrder();
            dow.ShowDialog();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Login fm = new Login();
            this.Hide();
            fm.ShowDialog();
            label15.Text = string.Empty;
        }

        private void dataTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            DataTransaksi dtt = new DataTransaksi();
            dtt.ShowDialog();
        }

        private void dataGudangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGudang dg = new DataGudang();
            this.Hide();
            dg.ShowDialog();
        }

        private void akutot_TextChanged(object sender, EventArgs e)
        {

        }

        private void Diskon_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

   


}
