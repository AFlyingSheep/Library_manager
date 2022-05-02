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
    public partial class defalutmanager : Form
    {
        OracleConnection conn;
        public defalutmanager(OracleConnection conn)
        {
            InitializeComponent();
            this.conn = conn;
            update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string PID = listView1.SelectedItems[0].SubItems[0].Text;
            string ISBM = listView1.SelectedItems[0].SubItems[2].Text;

            string sql = "UPDATE BORROW SET DEFALUT = 'NO' WHERE PID = '" + PID + "' AND ISBM = '" + ISBM + "'" ;
            login.execute_sql(sql, conn);
            MessageBox.Show("更改成功！");
            update();

        }
        private void update()
        {
            listView1.Items.Clear();
            string sql = "SELECT BORROW.PID, ISBM, PTEL, PNAME, FDATE FROM BORROW, PACCOUNT WHERE " +
                "BORROW.PID = PACCOUNT.PID AND DEFALUT = 'YES'";
            OracleDataReader odr = login.execute_sql(sql, conn);
            while (odr.Read())
            {
                ListViewItem list = listView1.Items.Add(odr.GetOracleValue(0).ToString());
                list.SubItems.Add(odr.GetOracleValue(3).ToString());
                list.SubItems.Add(odr.GetOracleValue(1).ToString());
                list.SubItems.Add(odr.GetOracleValue(4).ToString());
                list.SubItems.Add(odr.GetOracleValue(2).ToString());
            }
        }
    }
}
