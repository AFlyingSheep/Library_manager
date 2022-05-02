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
    public partial class book_change : Form
    {
        Bookinfo[] bookinfo;
        int totalcount;
        OracleConnection occn;
        private ContextMenuStrip strip = new ContextMenuStrip();//1
        public book_change(OracleConnection occn)
        {
            InitializeComponent();
            this.occn = occn;
            flushbook(occn);
            flushlistbook(occn);
            ToolStripMenuItem add = new ToolStripMenuItem();
            add.Text = "修改";
            add.Click += new EventHandler(add_Click);

            ToolStripMenuItem delete = new ToolStripMenuItem();
            delete.Text = "删除";
            delete.Click += new EventHandler(delete_Click);

            strip.Items.Add(add);
            strip.Items.Add(delete);

            strip.MouseClick += new MouseEventHandler(listView1_MouseClick);
        }

        public void flushbook(OracleConnection occn)
        {
            totalcount = 0;
            bookinfo = new Bookinfo[200];
            string ISBM = textBox1.Text;
            ISBM = ISBM.Replace(" ", "");
            string info = textBox2.Text;
            string author = textBox3.Text;
            string bookname = textBox4.Text;
            string sql;
            if (ISBM == "")
                sql = "SELECT ISBM, BCOUNT, BNAME, BINFO, AUTHOR FROM BOOK WHERE ISBM LIKE '%" + ISBM
            + "%' AND AUTHOR LIKE '%" + author + "%' AND BNAME LIKE '%" + bookname + "%' AND BINFO LIKE '%" + info + "%' ORDER BY ISBM";
            else
                sql = "SELECT ISBM, BCOUNT, BNAME, BINFO, AUTHOR FROM BOOK WHERE ISBM = '" + ISBM + "' ORDER BY ISBM";
            OracleDataReader odr = login.execute_sql(sql, occn);
            while (odr.Read())
            {
                bookinfo[totalcount] = new Bookinfo(odr.GetOracleValue(0).ToString(),
                    int.Parse(odr.GetOracleValue(1).ToString()),
                    odr.GetOracleValue(2).ToString(),
                    odr.GetOracleValue(3).ToString(),
                    odr.GetOracleValue(4).ToString()
                    );
                totalcount++;
            }
        }
        public void flushlistbook(OracleConnection occn)
        {
            listView1.Items.Clear();
            for (int i = 0; i < totalcount; i++)
            {
                ListViewItem listViewItem = listView1.Items.Add(bookinfo[i].ISBM);
                listViewItem.SubItems.Add(bookinfo[i].bookname);
                listViewItem.SubItems.Add(bookinfo[i].bookauthor);
                listViewItem.SubItems.Add(bookinfo[i].bookinfo);
                listViewItem.SubItems.Add(bookinfo[i].count.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flushbook(occn);
            flushlistbook(occn);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                strip.Show(listView1, e.Location);//鼠标右键按下弹出菜单
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            changebookitem change = new changebookitem(bookinfo[findIndex(bookinfo, listView1.SelectedItems[0].Text)], occn, this) ;
            change.Visible = true;
            flushbook(occn);
            flushlistbook(occn);
        }

        private void delete_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM BOOK WHERE ISBM = '" + bookinfo[findIndex(bookinfo, listView1.SelectedItems[0].Text)].ISBM + "'";
            login.execute_sql(sql, occn);
            MessageBox.Show("删除成功！");
            flushbook(occn);
            flushlistbook(occn);
        }

        public int findIndex(Bookinfo[] bookinfos, string ISBM)
        {
           for (int i = 0; i < bookinfo.Length; i++)
            {
                if (bookinfo[i].ISBM == ISBM) return i;
            }
            return -1;
        }





    }

    public class Bookinfo
    {
        public string ISBM;
        public int count;
        public string bookname;
        public string bookinfo;
        public string bookauthor;
        public Bookinfo(string ISBM, int count, string bookname, string bookinfo, string bookauthor)
        {
            this.ISBM = ISBM;
            this.count = count;
            this.bookauthor = bookauthor;
            this.bookinfo = bookinfo;
            this.bookname = bookname;
        }
    }
}
