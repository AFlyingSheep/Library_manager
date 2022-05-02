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
    public partial class user : Form
    {
        OracleConnection occn;
        private ContextMenuStrip strip = new ContextMenuStrip();//1
        int totalcount;
        userInfo[] userinfo;
        public user(OracleConnection occn)
        {
            this.occn = occn;
            InitializeComponent();
            ToolStripMenuItem add = new ToolStripMenuItem();
            add.Text = "修改";
            add.Click += new EventHandler(add_Click);

            ToolStripMenuItem delete = new ToolStripMenuItem();
            delete.Text = "删除";
            delete.Click += new EventHandler(delete_Click);

            strip.Items.Add(add);
            strip.Items.Add(delete);

            strip.MouseClick += new MouseEventHandler(listView1_MouseClick);
            flushUser();
            flushlistbook(occn);

        }
        private void add_Click(object sender, EventArgs e)
        {
            userchanges change = new userchanges(occn, userinfo[findUserIndex(userinfo, listView1.SelectedItems[0].Text)], this);
            change.Visible = true;
            flushUser();
            flushlistbook(occn);
        }

        private void delete_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM PACCOUNT WHERE PID = '" + userinfo[findUserIndex(userinfo, listView1.SelectedItems[0].Text)].PID + "'";
            login.execute_sql(sql, occn);
            MessageBox.Show("删除成功");
            flushUser();
            flushlistbook(occn);
        }

        

        public void flushUser()
        {
            totalcount = 0;
            userinfo = new userInfo[200];
            string sql = "SELECT PID, PPASSWORD, PNAME, PSEX, PTEL, PPERMISSION FROM PACCOUNT ORDER BY PPERMISSION, PID";

            OracleDataReader odr = login.execute_sql(sql, occn);
            while (odr.Read())
            {
                userinfo[totalcount] = new userInfo(odr.GetOracleValue(0).ToString(),
                    odr.GetOracleValue(1).ToString(),
                    odr.GetOracleValue(2).ToString(),
                    odr.GetOracleValue(3).ToString(),
                    odr.GetOracleValue(4).ToString(),
                    odr.GetOracleValue(5).ToString()
                    );
                totalcount++;
            }
        }
        public void flushlistbook(OracleConnection occn)
        {
            listView1.Items.Clear();
            for (int i = 0; i < totalcount; i++)
            {
                ListViewItem listViewItem = listView1.Items.Add(userinfo[i].PID);
                listViewItem.SubItems.Add(userinfo[i].password);
                listViewItem.SubItems.Add(userinfo[i].name);
                listViewItem.SubItems.Add(userinfo[i].sex);
                listViewItem.SubItems.Add(userinfo[i].tel);
                listViewItem.SubItems.Add(userinfo[i].permission);
            }
        }
        public int findUserIndex(userInfo[] userinfo, string PID)
        {
            for (int i = 0; i < userinfo.Length; i++)
            {
                if (userinfo[i].PID == PID) return i;
            }
            return -1;
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                strip.Show(listView1, ((MouseEventArgs)e).Location);//鼠标右键按下弹出菜单
            }
        }
    }

    public class userInfo
    {
        public string PID;
        public string password;
        public string name;
        public string sex;
        public string tel;
        public string permission;
        public userInfo(string PID, string password, string name, string sex, string tel, string permission)
        { 
            this.PID = PID;
            this.permission = permission;
            this.password = password;
            this.sex = sex;
            this.tel = tel;
            this.name = name;
        }
    }
}
