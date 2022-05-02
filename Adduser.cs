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
    public partial class Adduser : Form
    {
        OracleConnection ocnn;
        public Adduser(OracleConnection ocnn)
        {
            InitializeComponent();
            this.ocnn = ocnn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string PID = textBox1.Text, password = textBox2.Text, name = textBox3.Text,
                sex = textBox4.Text, tel = textBox5.Text, permission = textBox6.Text;
            PID.Replace(" ", "");
            if (PID == "")
            {
                MessageBox.Show("PID不可为空！");
                return;
            }
            if (password == "")
            {
                MessageBox.Show("密码不可为空！");
                return;
            }
            if (name == "")
            {
                MessageBox.Show("姓名不可为空！");
                return;
            }
            if (sex == "")
            {
                MessageBox.Show("性别不可为空！");
                return;
            }
            if (tel == "")
            {
                MessageBox.Show("电话号不可为空！");
                return;
            }
            if (permission == "")
            {
                MessageBox.Show("权限不可为空！");
                return;
            }
            string sql = "SELECT * FROM PACCOUNT WHERE PID = '" + PID + "'";
            OracleDataReader odr = login.execute_sql(sql, ocnn);
            if (odr.Read())
            {
                MessageBox.Show("PID已经存在！请修改。");
                return;
            }

            sql = "INSERT INTO PACCOUNT(PID, PPASSWORD, PNAME, PSEX, PTEL, PPERMISSION) VALUES ('"
                + PID + "', '" + password + "', '" + name + "' ,'" + sex + "' , '" + tel + "', '"
                + permission + "')";
            login.execute_sql(sql, ocnn);
            MessageBox.Show("插入成功！");
            this.Close();
        }
    }
}
