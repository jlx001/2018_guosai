using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 附合导线
{
    public partial class Form1 : Form
    {
        public static int srf =1;
        public Form1()
        {
            InitializeComponent();
            srf = 1;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(srf);
            this.Hide();
            frm.Show();
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            srf = 1;
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            srf = 2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
