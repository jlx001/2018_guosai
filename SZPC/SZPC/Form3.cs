using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace SZPC
{
    public partial class Form3 : Form
    {
        int value1;             //提取form2中传来的值
        ArrayList value2;
        ArrayList value3;
        public Form3(int value1, ArrayList value2, ArrayList value3)
        {
            InitializeComponent();
            this.value1 = value1;   //测站数
            this.value2 = value2;   //视距读数
            this.value3 = value3;   //水准尺读数
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= value1; i++)
            {
                comboBox1.Items.Add(i.ToString());
                comboBox1.Text = "请选择要检核的测站";
                
            }
            comboBox2.Items.Add("三等");
            comboBox2.Items.Add("四等");
            comboBox2.Text = "请选择水准路线等级";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int km = 0;
            double SJC = 0;
            double D = 0;
            double CJC = 0;
            double gcjc = 0;
            //判断路线等级
            if (comboBox2.Text == "三等")
            {
                SJC = 3;
                D = 6;
                CJC = 2;
                gcjc = 3;
            }
            else if (comboBox2.Text == "四等")
            {
                SJC = 5;
                D = 10;
                CJC = 3;
                gcjc = 5;
            }
            else if (comboBox2.Text == "请选择水准路线等级")
            {
                MessageBox.Show("请选择路线等级");
                return;
            }
           if (comboBox1.Text == "请选择要检核的测站")
            {
                MessageBox.Show("请选择要检核的测站");
                return;
            }
            for (int i = 1; i <= value2.Count / 4; i++)
            {
                if (i.ToString() == comboBox1.Text)
                {
                    double d = 0;
                    string a1 = value2[4 * i - 4].ToString();      //读取数据
                    string a2 = value2[4 * i - 3].ToString();
                    string b1 = value2[4 * i - 2].ToString();
                    string b2 = value2[4 * i - 1].ToString();
                    double hsj = (Convert.ToDouble(a1) - Convert.ToDouble(a2)) / 10;  //计算视距差
                    double qsj = (Convert.ToDouble(b1) - Convert.ToDouble(b2)) / 10;
                    double sjc = Math.Round((hsj - qsj), 1);         //前后视距差
                    d = d + sjc;                                     //累计视距差
                    //显示数据
                    richTextBox1.Text = "第" + i + "站检核结果\n";
                    richTextBox1.Text += "后尺" + "              " + "前尺\n";
                    richTextBox1.Text += "上丝   " + a1 + "       " + "上丝   " + b1 + "\n";
                    richTextBox1.Text += "下丝   " + b2 + "       " + "下丝   " + b2 + "\n";
                    richTextBox1.Text += "后视距 " + hsj.ToString("f1") + "       " + "前视距 " + qsj.ToString("f1") + "\n";
                    richTextBox1.Text += "前后视距差 " + sjc.ToString("f1") + "    " + "累计视距差 " + d.ToString("f1") + "\n";
                    if (Math.Abs(sjc) >Math.Abs(SJC))
                    {
                        MessageBox.Show("视距差超限");
                    }
                    else if (Math.Abs( d) >Math.Abs( D))
                    {
                        MessageBox.Show("累计视距差超限");
                    }
                    else 
                    {
                        km = 1;
                    }
                }

            }
         
            for (int i = 1; i <= value3.Count / 4; i++)
            {
                if (i.ToString() == comboBox1.Text)
                {
                    //提取读数
                    string a1 = value3[4 * i - 4].ToString();
                    string a2 = value3[4 * i - 3].ToString();
                    string b1 = value3[4 * i - 2].ToString();
                    string b2 = value3[4 * i - 1].ToString();
                    double ax = Convert.ToDouble(a1) - Convert.ToDouble(a2);       //黑面所读高差
                    double bx = Convert.ToDouble(b1) - Convert.ToDouble(b2);       //红面高差
                    //计算K+黑-红
                    double x1 = Math.Round((Convert.ToDouble(a1) / 100 - Math.Floor(Convert.ToDouble(a1) / 100)) * 100, 0);
                    double x2 = Math.Round((Convert.ToDouble(b1) / 100 - Math.Floor(Convert.ToDouble(b1) / 100)) * 100, 0);
                    double y1 = Math.Round((Convert.ToDouble(a2) / 100 - Math.Floor(Convert.ToDouble(a2) / 100)) * 100, 0);
                    double y2 = Math.Round((Convert.ToDouble(b2) / 100 - Math.Floor(Convert.ToDouble(b2) / 100)) * 100, 0);
                    int X1 = Convert.ToInt32(Math.Round((((x2 + 13) / 100 - Math.Floor((x2 + 13) / 100)) * 100), 0));
                    int X2 = Convert.ToInt32(Math.Round((((y2 + 13) / 100 - Math.Floor((y2 + 13) / 100)) * 100), 0));
                    int cx = Convert.ToInt32((x1 - X1) - (y1 - X2));                           //黑红面所测高差较差
                    double zs = ax - Convert.ToDouble(cx) / 2;                                //高差中数                                               
                    richTextBox1.Text += "      黑面      红面      K+黑-红\n";
                    richTextBox1.Text += "后    " + a1 + "      " + b1 + "         " + (x1 - X1).ToString()+"\n";
                    richTextBox1.Text += "前    " + a2 + "      " + b2 + "         " + (y1 - X2).ToString()+"\n";
                    richTextBox1.Text += "后-前 " + ax + "      " + bx + "         " + cx+"\n";
                    richTextBox1.Text += "高差中数   " + zs/1000;
                    if (Math.Abs( x1 - X1) >Math.Abs( CJC))
                    {
                        MessageBox.Show("黑面读数较差超限");
                    }
                    else if (Math.Abs( y1 - X2) >Math.Abs( CJC))
                    {
                        MessageBox.Show("红面读数较差超限");
                    }
                    else if (Math.Abs( cx) >Math.Abs( gcjc))
                    {
                        MessageBox.Show("高差较差超限");
                    }
                    else if (km == 1)
                    {
                        MessageBox.Show("本站数据合格");
                    }
                }
            }
        }
    }
}