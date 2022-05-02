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
    public partial class MyBorrow : Form
    {
        string PID;
        BookBorrowOne[] bookBorrowOnes;
        OracleConnection ocnn;
        int count;
        bool onlyNotReturn;
        bool onlyDefalut;
        bool onlyCanRenew;

        public MyBorrow(string PID, OracleConnection ocnn)
        {
            InitializeComponent();
            this.PID = PID;
            onlyCanRenew = checkBox3.Checked;
            onlyNotReturn = checkBox1.Checked;
            onlyDefalut = checkBox2.Checked;
            this.ocnn = ocnn;
            flush();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            flush();
            flushList();
        }
        
        public void flush ()
        {
            string sql = "SELECT PID, ISBM, SDATE, FDATE, ISRENEW, DEFALUT, ISRETURN FROM BORROW WHERE " +
                "PID = '" + PID + "' ORDER BY SDATE DESC";
            bookBorrowOnes = new BookBorrowOne[100];
            count = 0;
            OracleDataReader odr = login.execute_sql(sql, ocnn);
            while (odr.Read())
            {
                bookBorrowOnes[count] = new BookBorrowOne(odr.GetOracleValue(0).ToString(), odr.GetOracleValue(1).ToString(),
                     DateTime.Parse(odr.GetOracleValue(2).ToString()), DateTime.Parse(odr.GetOracleValue(3).ToString()),
                     odr.GetOracleValue(4).ToString(), odr.GetOracleValue(5).ToString(), odr.GetOracleValue(6).ToString()
                     );
                count++;
            }
            flushList();
        }

        public void flushList()
        {
            listView1.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                if (onlyCanRenew && bookBorrowOnes[i].ISRENEW == "NO") continue;
                if (onlyDefalut && bookBorrowOnes[i].DEFALUT == "NO") continue;
                if (onlyNotReturn && bookBorrowOnes[i].ISRETURN == "YES") continue;

                ListViewItem item = listView1.Items.Add(bookBorrowOnes[i].PID);
                item.SubItems.Add(bookBorrowOnes[i].ISBM);
                item.SubItems.Add(bookBorrowOnes[i].SDATE.ToShortDateString());
                item.SubItems.Add(bookBorrowOnes[i].FDATE.ToShortDateString());
                item.SubItems.Add(bookBorrowOnes[i].ISRENEW);
                item.SubItems.Add(bookBorrowOnes[i].DEFALUT);
                item.SubItems.Add(bookBorrowOnes[i].ISRETURN);

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            onlyNotReturn = checkBox1.Checked;
            flushList();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            onlyDefalut = checkBox2.Checked;
            flushList();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            onlyCanRenew = checkBox3.Checked;
            flushList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count <= 0) return;
            int index = findIndex(bookBorrowOnes, listView1.SelectedItems[0].Text,
                listView1.SelectedItems[0].SubItems[1].Text, listView1.SelectedItems[0].SubItems[2].Text); 

            if (bookBorrowOnes[index].ISRENEW == "NO")
            {
                MessageBox.Show("本图书已经续借过，不可再次续借！");
                return;
            }

            DialogResult mesSelection = MessageBox.Show("确定续借？","提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (mesSelection != DialogResult.OK) return;
            bookBorrowOnes[index].FDATE = bookBorrowOnes[index].FDATE.AddMonths(2);
            string sql = "UPDATE BORROW SET FDATE = to_date('" + bookBorrowOnes[index].FDATE.ToShortDateString()+"', 'YYYY-MM-DD') WHERE PID = '" + bookBorrowOnes[index].PID + "' AND " +
                "ISBM = '" + bookBorrowOnes[index].ISBM + "'";
            login.execute_sql(sql, ocnn);
            MessageBox.Show("执行成功！");
            sql = "UPDATE BORROW SET ISRENEW = 'NO' WHERE PID = '" + bookBorrowOnes[index].PID + "' AND " +
               "ISBM = '" + bookBorrowOnes[index].ISBM + "'";
            login.execute_sql(sql, ocnn);
            flush();
            flushList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count <= 0) return;
            int index = listView1.SelectedItems[0].Index;

            if (bookBorrowOnes[index].DEFALUT == "NO")
            {
                MessageBox.Show("本条记录未违约！");
                return;
            }

            Money money = new Money(bookBorrowOnes[index], ocnn, this);
            money.Visible = true;
        }

        public int findIndex(BookBorrowOne[] bookBorrowOnes, string PID, string ISBM, string dt)
        {
            for (int i = 0; i < bookBorrowOnes.Length; i++)
            {
                if (bookBorrowOnes[i].PID == PID && bookBorrowOnes[i].ISBM == ISBM &&
                    bookBorrowOnes[i].SDATE.ToShortDateString() == dt)
                {
                    return i;
                }
            }
            return -1;
        }
    }
    public class BookBorrowOne
    {
        public string PID;
        public string ISBM;
        public DateTime SDATE;
        public DateTime FDATE;
        public string ISRENEW;
        public string DEFALUT;
        public string ISRETURN;

        public BookBorrowOne(string PID, string ISBM, DateTime SDATE, DateTime FDATE, string ISRENEW, string DEFALUT, string ISRETURN)
        {
            this.ISBM = ISBM;
            this.PID = PID;
            this.SDATE = SDATE;
            this.FDATE = FDATE;
            this.ISRENEW = ISRENEW;
            this.DEFALUT = DEFALUT;
            this.ISRENEW = ISRENEW;
            this.ISRETURN = ISRETURN;
        }
    }
}
