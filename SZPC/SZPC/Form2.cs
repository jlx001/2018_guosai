using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SZPC
{
    public partial class Form2 : Form
    {
       
        public Form2()
        {
            InitializeComponent();           
        }
        int n = 0;
        int m = 0;
        int ki = 0;
        double d = 0;
        double sum0 = 0; //累计前视距
        double sum2 = 0; //累计后视距
        ArrayList cd = new ArrayList();   //点号
        ArrayList sj = new ArrayList();   //视距
        ArrayList ds = new ArrayList();   //尺读数
        ArrayList czs = new ArrayList();  //没测段对应测站数
        ArrayList A;
        ArrayList B;
        Label[] lab1;
        Label[] lab2;
        Label[] lab3;
        string[] str1;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int l1 = 0;
            int l2 = 0;
            int l3 = 0;
            for (int i = 0; i < n * 28; i++)    //查找窗体内所有label控件并删除
            {
                this.Controls.Remove(lab3[i]);
                l1++;
            
            }
            for (int i = 0; i < n + 3; i++)
            {
                this.Controls.Remove(lab1[i]);
                l2++;
            }
            for (int i = 0; i < 19; i++)
            {
                this.Controls.Remove(lab2[i]);
                l3++;
            }

             cd = new ArrayList();
             sj = new ArrayList();
             ds = new ArrayList();

             for (int i = 1; i <= czs.Count; i++)
             {
                 if (comboBox1.Text == "第" + i + "段")
                 {
                     for (int j = Convert.ToInt32(czs[i - 1]); j < Convert.ToInt32(czs[i]); j++)
                     {
                         string[] str2 = str1[j].Split(new char[] { ',' });   //按，分隔字符串
                         if (str1[j] != null & str1[j] != "")                        //判断本行非空
                         {
                             cd.Add(str2[0]);
                             cd.Add(str2[1]);
                             sj.Add(str2[2]);
                             sj.Add(str2[3]);
                             sj.Add(str2[4]);
                             sj.Add(str2[5]);
                             ds.Add(str2[6]);
                             ds.Add(str2[7]);
                             ds.Add(str2[8]);
                             ds.Add(str2[9]);
                             ki = 1;
                             n = Convert.ToInt32(czs[i]) - Convert.ToInt32(czs[i - 1]);
                         }
                     }
                 }
             }
                    if (comboBox1.Text == "所有测段"||comboBox1.Text=="请选择要显示的测段")
                    { 
                    for (int j =0; j < str1.Length; j++)
                        {
                       string[] str2 = str1[j].Split(new char[] { ',' });   //按，分隔字符串
                        if (str1[j] != null & str1[j] != "")                        //判断本行非空
                        {
                            cd.Add(str2[0]);
                            cd.Add(str2[1]);
                            sj.Add(str2[2]);
                            sj.Add(str2[3]);
                            sj.Add(str2[4]);
                            sj.Add(str2[5]);
                            ds.Add(str2[6]);
                            ds.Add(str2[7]);
                            ds.Add(str2[8]);
                            ds.Add(str2[9]);
                            ki = 1;
                            n = str1.Length;
                           
                        
                    }
                }                    
            }
             ShowButtonArray();
        }
        private void dqsj()
        { //打开文件流，读取文件
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;  //提取文件名
                str1 = File.ReadAllLines(fileName);
                czs.Add(0);
                for (int i = 0; i < str1.Length; i++)
                {
                    string[] str2 = str1[i].Split(new char[] { ',' });   //按，分隔字符串
                    if (str1[i] != null & str1[i] != "")                        //判断本行非空
                    {
                        cd.Add(str2[0]);
                        cd.Add(str2[1]);
                        sj.Add(str2[2]);
                        sj.Add(str2[3]);
                        sj.Add(str2[4]);
                        sj.Add(str2[5]);
                        ds.Add(str2[6]);
                        ds.Add(str2[7]);
                        ds.Add(str2[8]);
                        ds.Add(str2[9]);
                        ki = 1;
                        n++;                    //累加测站数
                        //判断测段数
                        string str3 = str2[0].ToString().Substring(0, 1); //提取每测站前后视点号
                        string str4 = str2[1].ToString().Substring(0, 1);
                        if (str3 != str4 && str3 == "T")              //若点号第一个字符不同且前视为转点，计一测段数
                        {
                            m++;
                            comboBox1.Items.Add("第"+m+"段");                          
                            czs.Add(n);
                        }
                    }

                }
  
            }
            else
            {
                return;
            }
        }
        private void ShowButtonArray()
        {

            if (ki == 1)
            {
                //创建表格
                lab1 = new Label[n + 3];      //四周单元格
                lab2 = new Label[19];        //表头
                lab3 = new Label[n * 28];
                for (int i = 0; i < lab2.Length; i++)      //设计表头
                {
                    lab2[i] = new Label();
                    lab2[0] = new Label();
                    lab2[0].Location = new System.Drawing.Point(0, 55);
                    lab2[0].Size = new System.Drawing.Size(40, 80);
                    lab2[0].Text = "测\n站\n编\n号";
                    lab2[1] = new Label();
                    lab2[1].Location = new System.Drawing.Point(40, 55);
                    lab2[1].Size = new System.Drawing.Size(25, 40);
                    lab2[1].Text = "后尺";
                    lab2[2] = new Label();
                    lab2[2].Location = new System.Drawing.Point(65, 55);
                    lab2[2].Size = new System.Drawing.Size(35, 20);
                    lab2[2].Text = "上丝";
                    lab2[3] = new Label();
                    lab2[3].Location = new System.Drawing.Point(65, 75);
                    lab2[3].Size = new System.Drawing.Size(35, 20);
                    lab2[3].Text = "下丝";
                    lab2[4] = new Label();
                    lab2[4].Location = new System.Drawing.Point(40, 95);
                    lab2[4].Size = new System.Drawing.Size(60, 20);
                    lab2[4].Text = "后视距";
                    lab2[5] = new Label();
                    lab2[5].Location = new System.Drawing.Point(40, 115);
                    lab2[5].Size = new System.Drawing.Size(60, 20);
                    lab2[5].Text = "视距差d";
                    lab2[6] = new Label();
                    lab2[6].Location = new System.Drawing.Point(100, 55);
                    lab2[6].Size = new System.Drawing.Size(25, 40);
                    lab2[6].Text = "前尺";
                    lab2[7] = new Label();
                    lab2[7].Location = new System.Drawing.Point(125, 55);
                    lab2[7].Size = new System.Drawing.Size(35, 20);
                    lab2[7].Text = "上丝";
                    lab2[8] = new Label();
                    lab2[8].Location = new System.Drawing.Point(125, 75);
                    lab2[8].Size = new System.Drawing.Size(35, 20);
                    lab2[8].Text = "下丝";
                    lab2[9] = new Label();
                    lab2[9].Location = new System.Drawing.Point(100, 95);
                    lab2[9].Size = new System.Drawing.Size(60, 20);
                    lab2[9].Text = "前视距";
                    lab2[10] = new Label();
                    lab2[10].Location = new System.Drawing.Point(100, 115);
                    lab2[10].Size = new System.Drawing.Size(60, 20);
                    lab2[10].Text = "Σd";
                    lab2[11] = new Label();
                    lab2[11].Location = new System.Drawing.Point(160, 55);
                    lab2[11].Size = new System.Drawing.Size(50, 80);
                    lab2[11].Text = "方向及\n尺号";
                    lab2[12] = new Label();
                    lab2[12].Location = new System.Drawing.Point(210, 55);
                    lab2[12].Size = new System.Drawing.Size(100, 40);
                    lab2[12].Text = "水准尺读数";
                    lab2[13] = new Label();
                    lab2[13].Location = new System.Drawing.Point(210, 95);
                    lab2[13].Size = new System.Drawing.Size(50, 40);
                    lab2[13].Text = "黑面";
                    lab2[14] = new Label();
                    lab2[14].Location = new System.Drawing.Point(260, 95);
                    lab2[14].Size = new System.Drawing.Size(50, 40);
                    lab2[14].Text = "红面";
                    lab2[15] = new Label();
                    lab2[15].Location = new System.Drawing.Point(310, 55);
                    lab2[15].Size = new System.Drawing.Size(60, 80);
                    lab2[15].Text = "K+黑-红";
                    lab2[16] = new Label();
                    lab2[16].Location = new System.Drawing.Point(370, 55);
                    lab2[16].Size = new System.Drawing.Size(50, 80);
                    lab2[16].Text = "高差\n中数";
                    lab2[17] = new Label();
                    lab2[17].Location = new System.Drawing.Point(420, 55);
                    lab2[17].Size = new System.Drawing.Size(50, 80);
                    lab2[17].Text = "备注";
                    lab2[18] = new Label();
                    lab2[18].Location = new System.Drawing.Point(0, 25);
                    lab2[18].Size = new System.Drawing.Size(470, 30);
                    lab2[18].Text = "水准测量记录簿";
                    lab2[18].TextAlign = ContentAlignment.MiddleCenter;
                    lab2[18].BorderStyle = BorderStyle.FixedSingle;
                    lab2[i].TextAlign = ContentAlignment.MiddleCenter; //设置对齐方式为居中
                    lab2[i].BorderStyle = BorderStyle.FixedSingle;     //显示边框
                    this.Controls.Add(lab2[i]);
                }

                for (int i = 0; i < lab1.Length; i++)
                {

                    lab1[i] = new Label();
                    if (i < n)          //测站列
                    {
                        lab1[i].Size = new System.Drawing.Size(40, 80);
                        lab1[i].Location = new System.Drawing.Point(0, 55 + ((i + 1) * 80));
                        lab1[i].TextAlign = ContentAlignment.MiddleCenter;
                        lab1[i].BorderStyle = BorderStyle.FixedSingle;
                        lab1[i].Text = (i + 1).ToString();
                        this.Controls.Add(lab1[i]);
                    }
                    else if (i == n)            //检核单元格
                    {
                        lab1[i].Size = new System.Drawing.Size(40, 80);
                        lab1[i].Location = new System.Drawing.Point(0, 55 + (i + 1) * 80);
                        lab1[i].Text = "  检  \n  核";
                        lab1[i].TextAlign = ContentAlignment.MiddleCenter;
                        lab1[i].BorderStyle = BorderStyle.FixedSingle;
                        this.Controls.Add(lab1[i]);
                    }
                    else if (i == n + 1)  //检核内单元格
                    {
                        lab1[i].Size = new System.Drawing.Size(430, 80);
                        lab1[i].Location = new System.Drawing.Point(40, 55 + (i * lab1[i].Height)); ;
                        lab1[i].TextAlign = ContentAlignment.TopLeft;
                        lab1[i].BorderStyle = BorderStyle.FixedSingle;
                        this.Controls.Add(lab1[i]);
                    }
                    else if (i == n + 2)  //备注内容单元格
                    {
                        lab1[i].Size = new System.Drawing.Size(50, 80 * n);
                        lab1[i].Location = new System.Drawing.Point(420, 135); ;
                        lab1[i].TextAlign = ContentAlignment.MiddleCenter;
                        lab1[i].BorderStyle = BorderStyle.FixedSingle;
                        this.Controls.Add(lab1[i]);
                    }

                }
                for (int i = 0; i < n * 8; i++)                           //视距 列
                {
                    lab3[i] = new Label();
                    lab3[i].Size = new System.Drawing.Size(60, 20);
                    lab3[i].Location = new System.Drawing.Point(40 + 60 * (i % 2), 135 + 20 * (i / 2));
                    lab3[i].TextAlign = ContentAlignment.MiddleCenter;
                    lab3[i].BorderStyle = BorderStyle.FixedSingle;
                    this.Controls.Add(lab3[i]);
                }

                for (int i = n * 8; i < n * 12; i++)              //方向及尺号列
                {
                    lab3[i] = new Label();
                    lab3[i].Size = new System.Drawing.Size(50, 20);
                    lab3[i].Location = new System.Drawing.Point(160, 135 + 20 * (i - n * 8));
                    lab3[i].TextAlign = ContentAlignment.MiddleCenter;
                    lab3[i].BorderStyle = BorderStyle.FixedSingle;

                    this.Controls.Add(lab3[i]);
                }

                for (int i = n * 12; i < n * 20; i++)              //水准尺读数列
                {
                    lab3[i] = new Label();
                    lab3[i].Size = new System.Drawing.Size(50, 20);
                    lab3[i].Location = new System.Drawing.Point(210 + 50 * ((i - n * 12) % 2), 135 + 20 * ((i - n * 12) / 2));
                    lab3[i].TextAlign = ContentAlignment.MiddleCenter;
                    lab3[i].BorderStyle = BorderStyle.FixedSingle;
                    this.Controls.Add(lab3[i]);
                }
                for (int i = n * 20; i < n * 24; i++)                 //K+黑-红 列
                {
                    lab3[i] = new Label();
                    lab3[i].Size = new System.Drawing.Size(60, 20);
                    lab3[i].Location = new System.Drawing.Point(310, 135 + 20 * (i - n * 20));
                    lab3[i].TextAlign = ContentAlignment.MiddleCenter;
                    lab3[i].BorderStyle = BorderStyle.FixedSingle;
                    this.Controls.Add(lab3[i]);
                }
                for (int i = n * 24; i < n * 28; i++)                  //高差中数列
                {
                    lab3[i] = new Label();
                    lab3[i].Size = new System.Drawing.Size(50, 20);
                    lab3[i].Location = new System.Drawing.Point(370, 135 + 20 * (i - n * 24));
                    lab3[i].TextAlign = ContentAlignment.MiddleCenter;
                    lab3[i].BorderStyle = BorderStyle.FixedSingle;
                    this.Controls.Add(lab3[i]);
                }

                //写入数据

                sum0 = 0;
                sum2 = 0;
                d = 0;
                for (int i = 1; i <= sj.Count / 4; i++)        //写入前后视距列
                {
                    string a1 = sj[4 * i - 4].ToString();      //读取数据
                    string a2 = sj[4 * i - 3].ToString();
                    string b1 = sj[4 * i - 2].ToString();
                    string b2 = sj[4 * i - 1].ToString();
                    double hsj = (Convert.ToDouble(a1) - Convert.ToDouble(a2)) / 10;  //计算视距差
                    double qsj = (Convert.ToDouble(b1) - Convert.ToDouble(b2)) / 10;
                    double sjc = Math.Round((hsj - qsj), 1);
                    d = d + sjc;                                 //计算累计视距差
                    sum2 = sum2 + hsj;
                    sum0 = sum0 + qsj;
                    int j = (i - 1) * 8 - 1;
                    lab3[j + 1].Text = a1.ToString();           //写入已知数据
                    lab3[j + 2].Text = b1.ToString();
                    lab3[j + 3].Text = a2.ToString();
                    lab3[j + 4].Text = b2.ToString();
                    lab3[j + 5].ForeColor = Color.Red;          //写入计算数据
                    lab3[j + 5].Text = hsj.ToString("f1");
                    lab3[j + 6].ForeColor = Color.Red;
                    lab3[j + 6].Text = qsj.ToString("f1");
                    lab3[j + 7].ForeColor = Color.Red;
                    lab3[j + 8].ForeColor = Color.Red;
                    if (sjc > 0)                             //判断数据正负,正数前加+号
                    {
                        lab3[j + 7].Text = "+" + sjc.ToString("f1");
                    }
                    else
                    {
                        lab3[j + 7].Text = sjc.ToString("f1");
                    }
                    if (d > 0)
                    {
                        lab3[j + 8].Text = "+" + d.ToString("f1");
                    }
                    else
                    {
                        lab3[j + 8].Text = d.ToString("f1");
                    }

                }
                A = new ArrayList();
                B = new ArrayList();
                for (int i = 1; i <= ds.Count / 4; i++)
                {
                    //提取数据
                    string a1 = ds[4 * i - 4].ToString();
                    string a2 = ds[4 * i - 3].ToString();
                    string b1 = ds[4 * i - 2].ToString();
                    string b2 = ds[4 * i - 1].ToString();

                    double ax = Convert.ToDouble(a1) - Convert.ToDouble(a2);  //计算黑面高差
                    double bx = Convert.ToDouble(b1) - Convert.ToDouble(b2);  //红面……
                    //计算K+黑-红
                    double x1 = Math.Round((Convert.ToDouble(a1) / 100 - Math.Floor(Convert.ToDouble(a1) / 100)) * 100, 0);
                    double x2 = Math.Round((Convert.ToDouble(b1) / 100 - Math.Floor(Convert.ToDouble(b1) / 100)) * 100, 0);
                    double y1 = Math.Round((Convert.ToDouble(a2) / 100 - Math.Floor(Convert.ToDouble(a2) / 100)) * 100, 0);
                    double y2 = Math.Round((Convert.ToDouble(b2) / 100 - Math.Floor(Convert.ToDouble(b2) / 100)) * 100, 0);
                    int X1 = Convert.ToInt32(Math.Round((((x2 + 13) / 100 - Math.Floor((x2 + 13) / 100)) * 100), 0));
                    int X2 = Convert.ToInt32(Math.Round((((y2 + 13) / 100 - Math.Floor((y2 + 13) / 100)) * 100), 0));
                    int cx = Convert.ToInt32((x1 - X1) - (y1 - X2));        // 黑红面高差较差
                    double zs = ax - Convert.ToDouble(cx) / 2;             //高差中数列数据
                    A.Add(x1 - X1);
                    A.Add(y1 - X2);
                    B.Add(ax);
                    B.Add(bx);
                    B.Add(cx);
                    B.Add(zs);
                    int k = (i - 1) * 8;
                    lab3[n * 12 + k].Text = ds[4 * i - 4].ToString();         //写入水准尺读数列
                    lab3[n * 12 + k + 1].Text = ds[4 * i - 2].ToString();
                    lab3[n * 12 + k + 2].Text = ds[4 * i - 3].ToString();
                    lab3[n * 12 + k + 3].Text = ds[4 * i - 1].ToString();
                    lab3[n * 12 + k + 4].ForeColor = Color.Red;
                    lab3[n * 12 + k + 4].Text = B[4 * i - 4].ToString();
                    lab3[n * 12 + k + 5].ForeColor = Color.Red;
                    lab3[n * 12 + k + 5].Text = B[4 * i - 3].ToString();


                    int j = (i - 1) * 4;
                    lab3[n * 20 + j + 2].ForeColor = Color.Red;
                    lab3[n * 20 + j + 2].Text = B[4 * i - 2].ToString();        //写入k+黑-红、高差中数列
                    lab3[n * 24 + j + 2].ForeColor = Color.Red;
                    lab3[n * 24 + j + 2].Text = (Convert.ToDouble(B[4 * i - 1]) / 1000).ToString("f4");
                    lab3[n * 20 + j].ForeColor = Color.Red;
                    lab3[n * 20 + j + 1].ForeColor = Color.Red;
                    if (x1 - X1 > 0)                                            //判断正负号，正数加正号
                    {
                        lab3[n * 20 + j].Text = "+" + A[2 * i - 2].ToString();
                    }
                    else
                    {
                        lab3[n * 20 + j].Text = A[2 * i - 2].ToString();
                    }
                    if (y1 - X2 > 0)
                    {
                        lab3[n * 20 + j + 1].Text = "+" + A[2 * i - 1].ToString();
                    }
                    else
                    {
                        lab3[n * 20 + j + 1].Text = A[2 * i - 1].ToString();
                    }

                }

                for (int i = 1; i <= cd.Count / 2; i++)        //写入方向及尺号列
                {
                    string str0 = cd[2 * i - 2].ToString();
                    string str1 = cd[2 * i - 1].ToString();

                    int j = (i - 1) * 4;
                    lab3[n * 8 + j].Text = "后 " + str0;
                    lab3[n * 8 + j + 1].Text = "前 " + str1;
                    lab3[n * 8 + j + 2].Text = "后-前";

                }
                for (int i = 0; i < lab1.Length; i++)  //写入检核内容
                {
                    if (i == n + 1)
                    {
                            lab1[i].Text = "Σ后视距-Σ前视距 = " + d.ToString("f1") + "\n";
                            lab1[i].Text += "总距离 = " + ((sum0 + sum2) / 1000).ToString("f2")+" km";
                        }
                    }
                }
          
            else if (ki == 0)
            {
                return; //MessageBox.Show("请选择数据文件");

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
                dqsj();
                ShowButtonArray();
            
              
                if (ki == 0)        //若数据未成功读取，关闭当前窗口返回初始窗口
                {
                    Form frm1 = new Form1();
                    frm1.Show();
                    this.Close();
                    MessageBox.Show("数据读取失败");
                }   
      
        }

        private void 测站检核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form3(n,sj,ds).Show();         //将检核需要变量传值到form3
        }

        private void 平差计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArrayList dh = new ArrayList();          
            ArrayList jl = new ArrayList();
            ArrayList gc = new ArrayList(); 
            this.Close();
            for (int i = 0; i < str1.Length; i++)
            {
                string[] str2 = str1[i].Split(new char[] { ',' });   //按，分隔字符串
                if (str1[i] != null & str1[i] != "")                        //判断本行非空
                {
                    string str3 = str2[0].ToString().Substring(0, 1); //提取每测站前后视点号
                    string str4 = str2[1].ToString().Substring(0, 1);
                    if (str3 != str4 && str3 != "T" )              //提取出水准点点号
                    {                
                        dh.Add(str2[0]);                      
                    }
                    else if (i == str1.Length - 1 && str3 != str4 && str4 != "T")
                    {
                        dh.Add(str2[1]);
                    }
                }
            }
            double sum1 = 0;
            gc.Add(0);
            for (int i = 1; i <= B.Count / 4; i++)          //提取高差中数
            {
                sum1 = sum1 + (Convert.ToDouble(B[4 * i - 1]) / 1000);
                for (int j = 0; j < czs.Count; j++)
                {
                    if (i == Convert.ToInt32(czs[j]))
                    {
                        gc.Add(sum1);
                    }
                }
            }
          
            jl.Add(0);
            sum2 =0;
            sum0 =0;
            for (int i = 1; i <= sj.Count / 4; i++)        //写入前后视距列
            {
                string a1 = sj[4 * i - 4].ToString();      //读取数据
                string a2 = sj[4 * i - 3].ToString();
                string b1 = sj[4 * i - 2].ToString();
                string b2 = sj[4 * i - 1].ToString();
                double hsj = (Convert.ToDouble(a1) - Convert.ToDouble(a2)) / 10;  //计算视距差
                double qsj = (Convert.ToDouble(b1) - Convert.ToDouble(b2)) / 10;
                sum2 = sum2 + hsj;
                sum0 = sum0 + qsj;
                
                for (int j = 0; j < czs.Count; j++)
                {
                    if (i ==Convert.ToInt32(czs[j]))
                    { 
                    jl.Add((sum2+sum0).ToString("f3"));
                    }
                }
            }
                new Form1(dh, gc, jl, czs).Show();
        }

    
       
    }
}
