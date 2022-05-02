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
    public partial class book_re : Form
    {
        OracleConnection ocnn;
        string PID;
        string ISBM;
        public book_re(OracleConnection ocnn, string PID)
        {
            this.ocnn = ocnn;
            InitializeComponent();
            this.PID = PID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT SDATE, FDATE FROM BORROW WHERE ISBM = '" + textBox1.Text + "' AND PID = '" + PID + "' AND ISRETURN = 'NO'";
            OracleDataReader odr = login.execute_sql(sql, ocnn);
            if (!odr.Read())
            {
                MessageBox.Show("您没有此图书的在借记录！请检查ISBM！");
                return;
            }

            ISBM = textBox1.Text;

            textBox2.AppendText("记录查询成功！" + Environment.NewLine);
            textBox2.AppendText("图书ISBM：" + textBox1.Text + Environment.NewLine);
            textBox2.AppendText("起始节约时间：" + odr.GetOracleValue(0).ToString() + Environment.NewLine);
            textBox2.AppendText("截止借阅时间：" + odr.GetOracleValue(1).ToString() + Environment.NewLine);
            textBox2.AppendText("当前时间：" + DateTime.Now);

            DateTime dt = DateTime.Parse(odr.GetOracleValue(1).ToString());
            DateTime now = DateTime.Now;
            if (now.Subtract(dt).TotalDays > 0)
            {
                MessageBox.Show("本次借阅逾期！请在归还后前往我的借阅中消除记录！");
            }


            textBox2.AppendText("请将书籍放入扫描机，并点击确认归还按钮！");

            button1.Enabled = false;
            textBox1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("归还成功！请将图书放在右侧回收车中。");
            string sql = "UPDATE BORROW SET ISRETURN = 'YES' WHERE PID = '" + PID + "' AND ISBM = '" + ISBM + "'";
            login.execute_sql(sql, ocnn);
            sql = "UPDATE BOOK SET BCOUNT = BCOUNT + 1 WHERE ISBM = '" + ISBM + "'";
            login.execute_sql(sql, ocnn);
            this.Close();
        }

        private void book_re_Load(object sender, EventArgs e)
        {

        }
    }
}
