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
    public partial class Money : Form
    {
        BookBorrowOne book;
        OracleConnection ocnn;
        MyBorrow book_;

        public Money(BookBorrowOne book, OracleConnection ocnn, MyBorrow book_)
        {
            InitializeComponent();
            this.book = book;
            this.book_ = book_;
            this.ocnn = ocnn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("缴纳成功！");
            book.DEFALUT = "YES";

            string sql = "UPDATE BORROW SET DEFALUT = 'NO' WHERE PID = '" + book.PID + "' AND  ISBM = '" + book.ISBM + "'";
            login.execute_sql(sql, ocnn);
            
            book_.flush();
            book_.flushList();
            this.Close();
            
        }
    }
}
