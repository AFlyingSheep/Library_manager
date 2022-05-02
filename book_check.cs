using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_manager
{
    public partial class book_check : Form
    {
        OracleConnection ocnn;
        int count;
        string[] booksname, booksISBM, booksinfo, booksauthor;
        int[] counts;
        public book_check(OracleConnection occn)
        {
            this.ocnn = occn;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string sql = "SELECT ISBM, BCOUNT, BNAME, BINFO, AUTHOR FROM BOOK WHERE ";
            string ISBM, info;

            if (textBox1.Text != "") sql += "ISBM = '" + textBox1.Text + "' AND ";
            if (textBox2.Text != "") sql += "BINFO LIKE '%" + textBox2.Text + "%' AND ";
            if (textBox3.Text != "") sql += "BNAME LIKE '%" + textBox3.Text + "%' AND ";
            if (textBox4.Text != "") sql += "AUTHOR LIKE '%" + textBox4.Text + "%' AND ";
            if (checkBox1.Checked) sql += "BCOUNT > 0";
            else sql += "BCOUNT >= 0";

            OracleDataReader odr = login.execute_sql(sql, ocnn);

            booksname = new string[100];
            booksISBM = new string[100];
            booksinfo = new string[100];
            booksauthor = new string[100];
            counts = new int[100];

            count = 0; 
            while (odr.Read())
            {
                counts[count] = int.Parse(odr.GetOracleValue(1).ToString());
                booksISBM[count] = odr.GetOracleValue(0).ToString();
                booksname[count] = odr.GetOracleString(2).ToString();
                booksinfo[count] = odr.GetOracleString(3).ToString();
                booksauthor[count] = odr.GetOracleValue(4).ToString();
                count++;
            }
            for (int i = 0; i < count; i++)
            {
                ListViewItem list = listView1.Items.Add(booksISBM[i]);
                list.SubItems.Add(booksname[i]);
                list.SubItems.Add(booksauthor[i]);
                list.SubItems.Add(booksinfo[i]);
                list.SubItems.Add(counts[i].ToString());

            }
        }
    }
}
