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
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class DataOrder : Form
    {
        public DataOrder()
        {
            InitializeComponent();
            BindData();
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            DataTable dt = new DataTable();
            string Connection = "server=bima-id.com;port=3306;username=bimaidco_atk;password=bobbycool123;database=bimaidco_atk;Allow Zero Datetime=true";
            MySqlConnection conn = new MySqlConnection(Connection);

            MySqlCommand read = new MySqlCommand("select * from dataorder where tanggal = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'", conn);
            conn.Open();
            MySqlDataReader reader = read.ExecuteReader();
            dt.Load(reader);

            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
        }

        private void DataOrder_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dateTimePicker1.MinDate = DateTime.Today;
            dateTimePicker1.MaxDate = DateTime.Today;

        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            SearchData(find.Text);
        }
        public void SearchData(string search)
        {
            string Connection = "server=bima-id.com;port=3306;username=bimaidco_atk;password=bobbycool123;database=bimaidco_atk;Allow Zero Datetime=true";
            MySqlConnection conn = new MySqlConnection(Connection);

            conn.Open();
            string query = "select * from dataorder where tanggal like '%" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "%' AND barang like '%" + find.Text + "%'";
                           
            MySqlDataAdapter adpt = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void kasirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main dow = new Main();
            dow.ShowDialog();
        }

        private void dataOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtbl = new DataTable();
            dtbl = dataGridView1.DataSource as DataTable;
            ExportToPDFLaporanOrder(dtbl, "Laporan Order Harian");

        }

        void ExportToPDFLaporanOrder(DataTable dtbl, string strHeader)
        {

            var saveFileDialoge = new SaveFileDialog();
            saveFileDialoge.FileName = "Laporan Order Harian";
            saveFileDialoge.DefaultExt = ".pdf";

            if (saveFileDialoge.ShowDialog() == DialogResult.OK)
            {
                Document doc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                FileStream stream = new FileStream(saveFileDialoge.FileName, FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);

                doc.Open();

                //Report Header

                BaseFont btnhead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fnthead = new iTextSharp.text.Font(btnhead, 16);
                Paragraph prgHeading = new Paragraph();
                prgHeading.Alignment = Element.ALIGN_CENTER;
                prgHeading.Add(new Chunk(strHeader.ToUpper(), fnthead));
                doc.Add(prgHeading);

                //add line break
                doc.Add(new Chunk("\n", fnthead));

                //ADD photo



                /*iTextSharp.text.Image PNG = iTextSharp.text.Image.GetInstance(bmp);
                PNG.Alignment = Element.ALIGN_LEFT;
                PNG.SetAbsolutePosition(doc.PageSize.Width - 20f - 500f, doc.PageSize.Height - 20f - 80f);
                PNG.ScalePercent(8f);
                doc.Add(PNG);*/

                //author
                Paragraph prgAuthor = new Paragraph();
                BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fntAuthor = new iTextSharp.text.Font(btnAuthor, 12);
                prgAuthor.Alignment = Element.ALIGN_RIGHT;
                prgAuthor.Add(new Chunk("A.n : Kepala Toko", fntAuthor));
                prgAuthor.Add(new Chunk("\nTanggal : " + DateTime.Now.ToShortDateString(), fntAuthor));
                prgAuthor.Add(new Chunk("\nKeterangan : Laporan Order Harian", fntAuthor));
                doc.Add(prgAuthor);


                //add line break
                doc.Add(new Chunk("\n", fnthead));

                // Add line Seperation
                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, iTextSharp.text.BaseColor.BLACK, Element.ALIGN_LEFT, 0)));
                doc.Add(p);

                //add line break
                doc.Add(new Chunk("\n", fnthead));


                //write table

                PdfPTable table = new PdfPTable(dtbl.Columns.Count);
                //table header
                BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fntColumnHeader = new iTextSharp.text.Font(btnColumnHeader, 10, 1, BaseColor.WHITE);
                for (int i = 0; i < dtbl.Columns.Count; i++)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BackgroundColor = BaseColor.GRAY;
                    cell.AddElement(new Chunk(dtbl.Columns[i].ColumnName.ToString(), fntColumnHeader));

                    table.AddCell(cell);
                }

                iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(btnColumnHeader, 8, 1);
                //The Table
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    for (int j = 0; j < dtbl.Columns.Count; j++)
                    {
                        table.AddCell(new PdfPCell(new Phrase(dtbl.Rows[i][j].ToString(), fontH1)));


                    }
                }

                //add line break
                doc.Add(new Chunk("\n", fnthead));

                //Add Description Before Table
                Paragraph Desc = new Paragraph();
                Desc.IndentationLeft = 60f;
                Desc.IndentationRight = 40f;
                BaseFont btnDesc = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fntDesc = new iTextSharp.text.Font(btnDesc, 12);
                Desc.Alignment = Element.ALIGN_LEFT;
                Desc.Add(new Chunk("Dibawah merupakan laporan order harian pada tanggal '" + DateTime.Now.ToShortDateString() + "' dan rincian tabel transaksi dibawah ini, untuk dapat mengontrol laporan order harian diharapkan agar management dan kepala toko untuk menandatangani serah terima laporan.", fntAuthor));
                doc.Add(Desc);

                //add line break
                doc.Add(new Chunk("\n", fnthead));

                //Add Table
                doc.Add(table);

                //add line break
                doc.Add(new Chunk("\n", fnthead));

                //Add Description after table
                Paragraph Desc2 = new Paragraph();
                Desc2.IndentationLeft = 60f;
                Desc2.IndentationRight = 40f;
                BaseFont btnDesc2 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fntDesc2 = new iTextSharp.text.Font(btnDesc2, 12);
                Desc2.Alignment = Element.ALIGN_LEFT;
                Desc2.Add(new Chunk("Demikian laporan order dicetak dan digunakan sebgaimana mestinya, harap untuk management dapat mengontrol order yang dikirimkan oleh kepala toko bidori", fntAuthor));
                Desc2.Add(new Chunk("Terima Kasih.", fntAuthor));
                doc.Add(Desc2);


                //add line break
                doc.Add(new Chunk("\n", fnthead));

                //Add Signature
                Paragraph sign = new Paragraph();
                sign.IndentationLeft = 60f;
                sign.IndentationRight = 40f;
                BaseFont btnsign = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fntsign = new iTextSharp.text.Font(btnsign, 12);
                sign.Alignment = Element.ALIGN_RIGHT;
                sign.Add(new Chunk("Bertanda Tangan Dibawah ini", fntAuthor));

                sign.Add(new Chunk("\n\n\n\n\nManagement, _____________", fntAuthor));

                doc.Add(sign);

                //Add Signature2
                Paragraph sign2 = new Paragraph();
                sign2.IndentationLeft = 60f;
                sign2.IndentationRight = 40f;
                BaseFont btnsign2 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font fntsign2 = new iTextSharp.text.Font(btnsign2, 12);
                sign2.Alignment = Element.ALIGN_LEFT;
                sign2.Add(new Chunk("Bertanda Tangan Dibawah ini", fntAuthor));

                sign2.Add(new Chunk("\n\n\n\n\nKepala Toko, _____________", fntAuthor));
                doc.Add(sign2);

                //add line break
                doc.Add(new Chunk("\n", fnthead));

                //close connection
                doc.Close();
                writer.Close();
                stream.Close();
            }

            else
            {
                MessageBox.Show("Terjadi Kesalahan Saat ingin Menyimpan File!");
            }




        }

    }
}
