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
    public partial class superMain : Form
    {
        OracleConnection ocnn;
        string[] information;

        public superMain(string[] information, OracleConnection ocnn)
        {
            InitializeComponent();
            this.ocnn = ocnn;
            this.information = information;
            label.Text = label.Text + information[2];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            user u = new user(ocnn);
            u.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Adduser adduser = new Adduser(ocnn);
            adduser.Visible = true;
        }
    }
}
