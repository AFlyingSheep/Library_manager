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

    public partial class userchanges : Form
    {
        userInfo userinfo;
        OracleConnection ocnn;
        user u;
        public userchanges(OracleConnection ocnn, userInfo userinfo, user u)
        {
            this.u = u;
            this.ocnn = ocnn;
            this.userinfo = userinfo;
            InitializeComponent();
            this.textBox1.Text = userinfo.PID;
            textBox2.Text = userinfo.password;
            textBox3.Text = userinfo.name;
            textBox4.Text = userinfo.sex;
            textBox5.Text = userinfo.tel;
            textBox6.Text = userinfo.permission;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "UPDATE PACCOUNT SET PPASSWORD = '" + textBox2.Text + "', PNAME = '" + textBox3.Text +
                "', PSEX = '" + textBox4.Text + "', PTEL = '" 
                + textBox5.Text + "', PPERMISSION = '" + textBox6.Text + "' WHERE PID = '" + textBox1.Text + "'";
            login.execute_sql(sql, ocnn);
            MessageBox.Show("修改成功！");
            u.flushUser();
            u.flushlistbook(ocnn);
        }
    }
}
