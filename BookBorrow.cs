using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OracleClient;

namespace Library_manager
{
    public partial class BookBorrow : Form
    {
        OracleConnection occn;
        string[] booksname;
        string[] booksISBM;
        string[] booksinfo;
        string PID;
        int count;

        public BookBorrow(OracleConnection occn, string PID)
        {
            InitializeComponent();
            this.PID = PID;
            this.occn = occn;
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string sql;
            if (comboBox1.SelectedIndex == 0)
                sql = "SELECT ISBM, BNAME, BCOUNT, BINFO FROM BOOK WHERE ISBM = "
                    + "'" + textBox1.Text.ToString() + "'";
            else
                sql = "SELECT ISBM, BNAME, BCOUNT, BINFO FROM BOOK WHERE BNAME LIKE '%"
                    + textBox1.Text.ToString() + "%'";
            
            OracleDataReader odr = login.execute_sql(sql, occn);

            booksname = new string[100];
            booksISBM = new string[100];
            booksinfo = new string[100];
            count = 0;
            while (odr.Read())
            {
                if (int.Parse(odr.GetOracleValue(2).ToString()) > 0)
                {
                    booksISBM[count] = odr.GetOracleValue(0).ToString();
                    booksname[count] = odr.GetOracleString(1).ToString();
                    booksinfo[count] = odr.GetOracleString(3).ToString();
                    count++;
                }
            }
            for (int i = 0; i < count; i++)
            {
                ListViewItem list = listView1.Items.Add(booksISBM[i]);
                list.SubItems.Add(booksname[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            int index = listView1.Items.IndexOf(listView1.FocusedItem);

            Borrowcheck borrowcheck = new Borrowcheck(booksISBM[index], booksname[index], booksinfo[index], this,occn, PID);
            borrowcheck.Visible = true;

        }
    }
}
