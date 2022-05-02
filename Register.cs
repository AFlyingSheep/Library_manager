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
    public partial class Register : Form
    {
        OracleConnection conn;
        public Register(OracleConnection conn)
        {
            InitializeComponent();
            this.conn = conn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long PID = long.Parse(textBox1.Text);
            string password = textBox2.Text;
            long TEL = long.Parse(textBox3.Text);
            string name = textBox4.Text;
            bool isman = comboBox1.SelectedIndex == 0 ? true : false;

            string pid_check = "SELECT PID FROM PACCOUNT WHERE PID = '" + PID.ToString() + "'";
            OracleDataReader pid = login.execute_sql(pid_check, conn);
            if (pid.Read())
            {
                MessageBox.Show("PID已存在！请更改。");
                return;
            }

            if (password == "")
            {
                MessageBox.Show("密码为空！");
                return;
            }

            if (PID.ToString() == "")
            {
                MessageBox.Show("PID为空！");
                return;
            }

            if (TEL.ToString() == "")
            {
                MessageBox.Show("电话号为空！");
                return;
            }

            if (name == "")
            {
                MessageBox.Show("姓名为空！");
                return;
            }
            string sex = isman? "男":"女";
            string pid_insert = "INSERT INTO Paccount(PID, PPASSWORD, PNAME, PSEX, PTEL, PPERMISSION) " +
                "VALUES ('" + PID.ToString() + "','" + password + "','" + 
                name + "','"+ sex + "','" + TEL.ToString() + "','reader')";

            login.execute_sql(pid_insert, conn);
            MessageBox.Show("注册成功！");
            this.Close();
        }
    }
}
