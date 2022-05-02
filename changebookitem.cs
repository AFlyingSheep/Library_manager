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
    
    public partial class changebookitem : Form
    {
        Bookinfo bookinfo;
        OracleConnection occn;
        book_change book_;
        public changebookitem(Bookinfo bookinfo, OracleConnection occn, book_change book)
        {
            this.bookinfo = bookinfo;
            InitializeComponent();
            textBox1.Text = bookinfo.ISBM;
            textBox2.Text = bookinfo.bookname;
            textBox3.Text = bookinfo.bookauthor;
            textBox4.Text = bookinfo.bookinfo;
            textBox5.Text = bookinfo.count.ToString();
            this.occn = occn;
            this.book_ = book;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "UPDATE BOOK SET BCOUNT = '" + textBox5.Text + "', BNAME = '" + textBox2.Text
                + "', BINFO = '" + textBox4.Text + "', AUTHOR = '" + textBox3.Text + "' WHERE ISBM = '" + textBox1.Text + "'";
            login.execute_sql(sql, occn);
            MessageBox.Show("修改成功！");
            book_.flushbook(occn);
            book_.flushlistbook(occn);
        }


    }
}
