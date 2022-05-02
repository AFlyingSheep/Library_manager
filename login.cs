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
    
    public partial class login : Form
    {
        OracleConnection conn;
        public login()
        {
            InitializeComponent();
            string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))" +
                "(CONNECT_DATA=(SERVICE_NAME=mydatabase)));Persist Security Info=True;User ID=c##zhaoyang;Password=1;";
            conn = new OracleConnection(connString);//实例化
            conn.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM paccount WHERE PID = '" + yonghuming.Text.ToString()+"'";
            OracleCommand lo_cmd = new OracleCommand(sql, conn);
            OracleDataReader odr = lo_cmd.ExecuteReader();
            string[] information = new string[6];
            while (odr.Read())
            {
                for (int i = 0; i < 6; i++)
                {
                    information[i] = odr.GetOracleValue(i).ToString();
                }
                
            }
            // 账号 密码 姓名 性别 电话号 权限
            if (information[1] == null)
            {
                MessageBox.Show("用户名不存在！");
                return;
            }
            if (information[1] != mima.Text.ToString())
            {
                MessageBox.Show("密码错误！");
                return;
            }
            if (information[5] == "reader")
            {
                MessageBox.Show("登陆成功！欢迎你，读者！");
                readerMain reader = new readerMain(information, conn);
                reader.Visible = true;
                this.Close();
            }
               
            else if (information[5] == "admin")
            {
                MessageBox.Show("登陆成功！欢迎你，管理员！");
                admiMain admin = new admiMain(information, conn);
                admin.Visible = true;
                this.Close();
            }
               
            else
            {
                MessageBox.Show("登陆成功！欢迎你，超级管理员！");
                superMain super = new superMain(information, conn);
                super.Visible = true;
                this.Close();
            }
                
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register register = new Register(conn);
            register.Visible = true;

        }

        public static OracleDataReader execute_sql(string sql, OracleConnection oracleConnection)
        {
            OracleCommand lo_cmd = new OracleCommand(sql, oracleConnection);
            OracleDataReader odr = lo_cmd.ExecuteReader();
            return odr;
        }
    }
}
