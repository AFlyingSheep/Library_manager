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
    public partial class readerMain : Form
    {
        string[] information;
        OracleConnection conn;
        public readerMain(string[] information, OracleConnection conn)
        {
            InitializeComponent();
            this.conn = conn;
            this.information = information;
            label.Text = label.Text + information[2];
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (isDefalut(information[0]))
            {
                MessageBox.Show("存在违约记录！请在我的借阅中查看并处理！");
                return;
            }
            BookBorrow book = new BookBorrow(conn, information[0]);
            book.Visible = true;
        }

        public bool isDefalut(string PID)
        {
            string sql = "SELECT * FROM BORROW WHERE PID = '" + PID + "' AND DEFALUT = 'YES'";
            OracleDataReader odr = login.execute_sql(sql, conn);
            if (odr.Read()) return true;
            return false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            book_check book = new book_check(conn);
            book.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            book_re book_ = new book_re(conn, information[0]);
            book_.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MyBorrow my = new MyBorrow(information[0], conn);
            my.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label_Click(object sender, EventArgs e)
        {

        }
    }
}
