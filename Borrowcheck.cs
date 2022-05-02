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
    public partial class Borrowcheck : Form
    {
        OracleConnection occn;
        string ISBM, name, info, PID;
        BookBorrow book;
        

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT BCOUNT FROM BOOK WHERE ISBM = '" + ISBM + "'";
            OracleDataReader odr = login.execute_sql(sql, occn);
            odr.Read();
            long count = long.Parse(odr.GetOracleValue(0).ToString());
            count--;
            sql = "UPDATE BOOK SET BCOUNT = '" + count + "' WHERE ISBM = '" + ISBM + "'";
            login.execute_sql(sql, occn);
            string now = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
            string or = DateTime.Now.AddMonths(3).Year + "-" + 
                DateTime.Now.AddMonths(3).Month + "-" + DateTime.Now.AddMonths(3).Day;
            sql = "INSERT INTO BORROW(PID, ISBM, SDATE, FDATE, ISRENEW, DEFALUT, ISRETURN) VALUES ('" +
                PID + "','" + ISBM +
                "', to_date('"+now+"', 'YYYY-MM-DD'), to_date('"+or+"', 'YYYY-MM-DD'), 'YES', 'NO', 'NO')";
            login.execute_sql(sql, occn);
            MessageBox.Show("借阅成功！请在" + or + "前归还！");
            book.Close();
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DateTime dt;
        public Borrowcheck(string ISBM, string name, string info, BookBorrow book, OracleConnection occn, string PID)
        {
            this.PID = PID;
            this.occn = occn;
            this.book = book;
            InitializeComponent();
            this.ISBM = ISBM;
            this.name = name;
            this.info = info;

            textBox1.Text = name;
            textBox2.Text = ISBM;
            textBox3.Text = info;

            dt = DateTime.Now;
            textBox4.Text = dt.AddMonths(3).ToLongDateString();
        }
    }
}
