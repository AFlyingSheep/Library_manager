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
    public partial class addBook : Form
    {
        OracleConnection conn;
        public addBook(OracleConnection conn)
        {
            this.conn = conn;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("图书ISBM不能为空！");
                return;
            }
            if (login.execute_sql("SELECT * FROM BOOK WHERE ISBM = '" + textBox1.Text + "'", conn).Read())
            {
                DialogResult dialogResult = MessageBox.Show("有重复ISBM，是否将其数量增加图书数量？", "提示", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    login.execute_sql("UPDATE BOOK SET COUNT = COUNT + " + textBox5.Text 
                        + "WHERE ISBM = '" + textBox1.Text + "'", conn);
                    MessageBox.Show("添加成功！");
                    return;
                }
                else
                {
                    return;
                }
            }
            string ISBM = textBox1.Text;
            string title = textBox2.Text;
            string author = textBox3.Text;
            string info = textBox4.Text;
            string count = textBox5.Text;
            if (textBox2.Text == "")
            {
                MessageBox.Show("图书名称不能为空！");
                return;
            }
            if (title == "")
            {
                MessageBox.Show("图书作者不能为空！");
                return;
            }
            if (info == "")
            {
                MessageBox.Show("图书简介不能为空！");
                return;
            }
            if (count == "")
            {
                MessageBox.Show("图书数量不能为空！");
                return;
            }
            login.execute_sql("INSERT INTO BOOK (ISBM, BCOUNT, ISABLE, BNAME, BINFO, AUTHOR) VALUES" +
                " ('" + ISBM + "','" + count + "','y','" + title + "','" + info + "','" + author + "')", conn);
            MessageBox.Show("添加成功！");
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
    }
}
