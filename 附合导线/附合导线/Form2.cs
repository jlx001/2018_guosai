using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Collections;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace 附合导线
{
    public partial class Form2 : Form
    {
        int value1;
        int N,m,t,km=0;               //导线点数
        double sum0,sum1,fb,v,fx,fy,fs,vx,vy;         //观测角度和,距离和，闭合差,角度改正数,x坐标增量，y坐标增量,x坐标改正，y坐标改正。
        double sum2, sum3, sum4, sum5;             //
        RichTextBox[] bt;
        RichTextBox[] jd;
        RichTextBox[] zb;
        string[] str1;
        ComboBox com1 = new ComboBox();
        ArrayList dh;
        ArrayList gcz;
        ArrayList bc;
        ArrayList fwzb;
        List<double> sczb;
        System.Drawing.Point[] pic1;        //测点图上坐标
        RichTextBox qd = new RichTextBox();      //起点
        public Form2(int value1)                           
        {
            InitializeComponent();
            this.value1 = value1;
        }
        public int trf = 0;
        private void Form2_Load(object sender, EventArgs e)
        {

            if (value1 == 1)               //手动输入
            {
                try
                {
                    N = Convert.ToInt32(Interaction.InputBox("请输入导线点总数", "N=", ""));
                    m = 0;
                }
                catch
                {
                    this.Close();
                    trf = 1;
                }
                finally { }
            }
            else if (value1 == 2)                    //文件导入
            {
                OpenFileDialog txt_r = new OpenFileDialog();
                txt_r.InitialDirectory = "C:\\Users\\Administrator\\Desktop\\";
                txt_r.Filter = "文本文件(*.txt)|*.txt";
                txt_r.RestoreDirectory = true;
                txt_r.FilterIndex = 1;
                if (txt_r.ShowDialog() == DialogResult.OK)
                {
                    string file = txt_r.FileName;                             //得到选择的文件的完整路径
                    str1 = File.ReadAllLines(file);
                    N = str1.Length ;
                    m = 1;
                }
                else
                {
                    this.Close();
                    trf = 1;                 
                }

            }
            if (trf == 0)
            {
                
                BT();
                JD();
                ZB();
                if (value1 == 2)
                {
                    try
                    {
                        XR();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("请选择正确的文件\r\n"+x.ToString());
                    }
                    finally { }
                }
            }
            else if (trf == 1)
            {
                Form1 frm1 = new Form1();
                this.Hide();
                frm1.Show();
                return;
            }
           
        }
        private void BT()     //新建表头                   
        {
           
            com1.Location = new System.Drawing.Point(30, 50);
            com1.Size = new Size(100, 20);
            com1.Items.Add("左角");
            com1.Items.Add("右角");
            com1.Text = "选择前进方向";
            this.Controls.Add(com1);
            bt = new RichTextBox[19];
            for (int i = 0; i < bt.Length; i++)
            {
                bt[0] = new RichTextBox();
                bt[0].Location = new System.Drawing.Point(0, 0);
                bt[0].Size = new Size(30, 50);
                bt[0].SelectionAlignment = HorizontalAlignment.Center;
                bt[0].ReadOnly = true;
                bt[0].Text = "测\n点";
                bt[1] = new RichTextBox();
                bt[1].Location = new System.Drawing.Point(30, 0);
                bt[1].Size = new Size(100, 50);
                bt[1].SelectionAlignment = HorizontalAlignment.Center;
                bt[1].ReadOnly = true;
                bt[1].Text = "角度观测值\n°  ′  ″";
                bt[2] = new RichTextBox();
                bt[2].Location = new System.Drawing.Point(130, 0);
                bt[2].Size = new Size(100, 50);
                bt[2].SelectionAlignment = HorizontalAlignment.Center;
                bt[2].ReadOnly = true;
                bt[2].Text = "改正后角值\n°  ′  ″";
                bt[3] = new RichTextBox();
                bt[3].Location = new System.Drawing.Point(230, 0);
                bt[3].Size = new Size(100, 50);
                bt[3].SelectionAlignment = HorizontalAlignment.Center;
                bt[3].ReadOnly = true;
                bt[3].Text = "方 位 角\n°  ′  ″";
                bt[4] = new RichTextBox();
                bt[4].Location = new System.Drawing.Point(330, 0);
                bt[4].Size = new Size(80, 50);
                bt[4].SelectionAlignment = HorizontalAlignment.Center;
                bt[4].ReadOnly = true;
                bt[4].Text = "边 长";
                bt[5] = new RichTextBox();
                bt[5].Location = new System.Drawing.Point(410, 0);
                bt[5].Size = new Size(160, 25);
                bt[5].SelectionAlignment = HorizontalAlignment.Center;
                bt[5].ReadOnly = true;
                bt[5].Text = "坐 标 增 量";
                bt[6] = new RichTextBox();
                bt[6].Location = new System.Drawing.Point(570, 0);
                bt[6].Size = new Size(160, 25);
                bt[6].SelectionAlignment = HorizontalAlignment.Center;
                bt[6].ReadOnly = true;
                bt[6].Text = "改正后坐标增量";
                bt[7] = new RichTextBox();
                bt[7].Location = new System.Drawing.Point(730, 0);
                bt[7].Size = new Size(180, 25);
                bt[7].SelectionAlignment = HorizontalAlignment.Center;
                bt[7].ReadOnly = true;
                bt[7].Text = "坐   标";
                bt[8] = new RichTextBox();
                bt[8].Location = new System.Drawing.Point(410, 25);
                bt[8].Size = new Size(80, 25);
                bt[8].SelectionAlignment = HorizontalAlignment.Center;
                bt[8].ReadOnly = true;
                bt[8].Text = "△x";
                bt[9] = new RichTextBox();
                bt[9].Location = new System.Drawing.Point(490, 25);
                bt[9].Size = new Size(80, 25);
                bt[9].SelectionAlignment = HorizontalAlignment.Center;
                bt[9].ReadOnly = true;
                bt[9].Text = "△y";
                bt[10] = new RichTextBox();
                bt[10].Location = new System.Drawing.Point(570, 25);
                bt[10].Size = new Size(80, 25);
                bt[10].SelectionAlignment = HorizontalAlignment.Center;
                bt[10].ReadOnly = true;
                bt[10].Text = "△x";
                bt[11] = new RichTextBox();
                bt[11].Location = new System.Drawing.Point(650, 25);
                bt[11].Size = new Size(80, 25);
                bt[11].SelectionAlignment = HorizontalAlignment.Center;
                bt[11].ReadOnly = true;
                bt[11].Text = "△y";
                bt[12] = new RichTextBox();
                bt[12].Location = new System.Drawing.Point(730, 25);
                bt[12].Size = new Size(90, 25);
                bt[12].SelectionAlignment = HorizontalAlignment.Center;
                bt[12].ReadOnly = true;
                bt[12].Text = "x";
                bt[13] = new RichTextBox();
                bt[13].Location = new System.Drawing.Point(820, 25);
                bt[13].Size = new Size(90, 25);
                bt[13].SelectionAlignment = HorizontalAlignment.Center;
                bt[13].ReadOnly = true;
                bt[13].Text = "y";
                if (m == 0)
                {
                    bt[14] = new RichTextBox();
                    bt[14].Location = new System.Drawing.Point(0, 50);
                    bt[14].Size = new Size(30, 21);
                    bt[14].SelectionAlignment = HorizontalAlignment.Center;
                }//bt[14].Text = "A";              
                bt[16] = new RichTextBox();
                bt[16].Location = new System.Drawing.Point(130, 50);
                bt[16].Size = new Size(100, 21);
                bt[16].SelectionAlignment = HorizontalAlignment.Center;
                //bt[16].ReadOnly = true;
                bt[16].Text = "";
                bt[17] = new RichTextBox();
                bt[17].Location = new System.Drawing.Point(730, 50);
                bt[17].Size = new Size(90, 21);
                bt[17].SelectionAlignment = HorizontalAlignment.Center;
                // bt[17].ReadOnly = true;
                bt[17].Text = "";
                bt[18] = new RichTextBox();
                bt[18].Location = new System.Drawing.Point(820, 50);
                bt[18].Size = new Size(90, 21);
                bt[18].SelectionAlignment = HorizontalAlignment.Center;
                //bt[18].ReadOnly = true;
                bt[18].Text = "";

                this.Controls.Add(bt[i]);

            }
        }
        private void JD()     //两侧区域                   
        {
            jd = new RichTextBox[N * 5 + 1];
            for (int i = 0; i < N; i++)
            {
                jd[i] = new RichTextBox();
                if (i < N - 2)          //测点列第二行到倒数第三行
                {
                    jd[i].Size = new Size(30, 42);
                    jd[i].Location = new System.Drawing.Point(0, 71 + i * 42);
                    jd[i].SelectionAlignment = HorizontalAlignment.Center;
                }
                else if (i >= N - 2)    //测点列倒数后两行
                {
                    jd[i].Size = new Size(30, 21);
                    jd[i].Location = new System.Drawing.Point(0, 71 + (i - 1) * 42 + jd[i - 1].Height);
                    jd[i].SelectionAlignment = HorizontalAlignment.Center;
                   
                }
                this.Controls.Add(jd[i]);              
            }
            jd[N - 1].Text = "Σ";
            for (int i = N; i < N * 5 + 1; i++)
            {
                jd[i] = new RichTextBox();
                if (i < 2*N-2)             //角度观测值列
                {
                    jd[i].Size = new Size(100, 42);
                    jd[i].Location = new System.Drawing.Point(30 , 71 + (i - N) * 42);
                    jd[i].SelectionAlignment = HorizontalAlignment.Right;
                }
                else if (i >= 2 * N - 2 & i < 3 * N - 4)  //改正后角值列
                {
                    jd[i].Size = new Size(100, 42);
                    jd[i].Location = new System.Drawing.Point(130, 71 + (i - (N*2-2)) * 42);
                    jd[i].SelectionAlignment = HorizontalAlignment.Right;
                }
                else if (i >= 3 * N - 4 & i < 3 * N)  //角度列倒数后两行
                {
                    jd[i].Size = new Size(100, 21);
                    jd[i].Location = new System.Drawing.Point(30 + (i % 2) * 100, 71 + ((i - N) / 2 - 1) * 42 + jd[i - 2].Height);
                    jd[i].SelectionAlignment = HorizontalAlignment.Right;
                }
                else if (i >= N * 3 & i < N * 5 - 4)      //坐标列
                {
                    jd[i].Size = new Size(90, 42);
                    jd[i].Location = new System.Drawing.Point(730 + (i - 3 * N) / (N - 2) * 90, 71 + (i - 3 * N) % (N - 2) * 42);
                    jd[i].SelectionAlignment = HorizontalAlignment.Center;
                }
                else if (i >= N * 5 - 4 & i < N * 5)   //坐标列倒数后两行
                {
                    jd[i].Size = new Size(90, 21);
                    jd[i].Location = new System.Drawing.Point(730 + (i % 2) * 90, 71 + ((i - 3 * N) / 2 - 1) * 42 + jd[i - 2].Height);
                    jd[i].SelectionAlignment = HorizontalAlignment.Center;
                }
                else if (i == N * 5)                 //检核框
                {
                    jd[i].Size = new Size(910, 100);
                    jd[i].Location = new System.Drawing.Point(0, 113 + (N - 2) * 42);
                    jd[i].BorderStyle = BorderStyle.FixedSingle;
                }
                this.Controls.Add(jd[i]);
            }

        }
        private void ZB()     //中间区域                   
        {
            zb = new RichTextBox[N * 6];
            for (int i = 0; i < N; i++)   
            {
                zb[i] = new RichTextBox();
               /* if (i == 0)     //方位角列第一行
                {
                    zb[i].Size = new Size(100, 41);
                    zb[i].Location = new System.Drawing.Point(230, 50);
                    zb[i].SelectionAlignment = HorizontalAlignment.Right;
                }*/
                if ( i < N - 1)  //方位角列
                {
                    zb[i].Size = new Size(100, 42);
                    zb[i].Location = new System.Drawing.Point(230, 92 + (i - 1) * 42);
                    zb[i].SelectionAlignment = HorizontalAlignment.Right;
                }
                if (i == N - 1)         //方位角列最后一行
                {
                    zb[i].Size = new Size(100, 21);
                    zb[i].Location = new System.Drawing.Point(230, 92 + (i - 1) * 42);
                    zb[i].SelectionAlignment = HorizontalAlignment.Right;
                }
                this.Controls.Add(zb[i]);
            }
            for (int i = N; i < N * 6; i++)
            {
                zb[i] = new RichTextBox();
               /* if (i - N < 5)                    //边长至改正后y列第一行
                {
                    zb[i].Size = new Size(80, 42);
                    zb[i].Location = new System.Drawing.Point(330 + (i - N) * 80, 50);
                    zb[i].SelectionAlignment = HorizontalAlignment.Center;
                }*/
                if ( i < N * 6 - 5)  //第一行至倒数第二行
                {
                    zb[i].Size = new Size(80, 42);
                    zb[i].Location = new System.Drawing.Point(330 + ((i - N) / (N - 1)) * 80, 50 + ((i - N) % (N - 1)) * 42);
                    zb[i].SelectionAlignment = HorizontalAlignment.Right;
                }
                if (i >= N * 6 - 5)            //最后一行
                {                    
                    zb[i].Size = new Size(80, 21);
                    zb[i].Location = new System.Drawing.Point(330 + ((i - N) % 5) * 80, 92 + (N - 2) * 42);
                    zb[i].SelectionAlignment = HorizontalAlignment.Right;
                }
                
                this.Controls.Add(zb[i]);
            }
        }
        private void XR()     //写入已知数据               
        {
            dh = new ArrayList();
            gcz = new ArrayList();
            bc = new ArrayList();
            fwzb = new ArrayList();
            for (int i = 0; i < str1.Length; i++)           //读取数据
            {
                string[] str2 = str1[i].Split(new char[] { ',' });
                string[] str3=new string[2];
                if (i < 2)                                //读取已知数据
                {
                    str3 = str2[0].Split(new char[] { '-' });
                    for (int j = 0; j < 1; j++)
                    {
                        dh.Add(str3[j+i]);
                    }
                    fwzb.Add(str2[1]);
                    fwzb.Add(str2[2]);
                    fwzb.Add(str2[3]);
                }
                else if (i >= 2&i<str1.Length-1)         //读取观测数据
                {
                    dh.Add(str2[0]);
                    gcz.Add(str2[1]);
                    bc.Add(str2[2]);
                }
                else if (i == str1.Length - 1)
                {
                    dh.Add(str2[0]);
                    gcz.Add(str2[1]);
                }
            }

           
            qd.Location = new System.Drawing.Point(0, 50);
            qd.Size = new Size(30, 21);
            qd.SelectionAlignment = HorizontalAlignment.Center;
            qd.Text = dh[0].ToString();
            this.Controls.Add(qd);
            for (int i = 0; i < dh.Count; i++)    //写入数据
            {
                jd[N - 2].Text = dh[1].ToString();   //终点
                zb[0].Text = fwzb[0].ToString();     //A-B方位角 
                zb[N - 2].Text = fwzb[3].ToString(); //C-D方位角
                jd[3 * N].Text = fwzb[1].ToString(); //B点x坐标 
                jd[4 * N - 2].Text = fwzb[2].ToString();//B点y坐标
                jd[4 * N - 3].Text = fwzb[4].ToString();//C点x坐标
                jd[5 * N - 5].Text = fwzb[5].ToString();//C点y坐标
                if (i > 1)
                {
                    jd[i-2].Text = dh[i].ToString();
                }
            }
            for (int i = 0; i < gcz.Count; i++)
            {
                jd[N + i].Text =Convert.ToDouble(gcz[i]).ToString("f5");               
            }
            for (int i = 0; i < bc.Count; i++)
            {
                zb[N + 1 + i].Text = Convert.ToDouble(bc[i]).ToString("f3");
            }

        }
        private void JS()     //计算过程                   
        {
            JDZH l = new JDZH();
            //JDZH n = new JDZH();
            gcz = new ArrayList();

            if (com1.Text != "左角" & com1.Text != "右角"|com1.Text=="")
            {
                MessageBox.Show("请选择前进方向");
                return;
            }
            for (int i = N; i <2*N-2; i++)    //读取观测角度
            {
                double d1 =l.jzh(Convert.ToDouble(jd[i].Text));
                gcz.Add(d1);
            }
            for (int i = 0; i < gcz.Count; i++)  
            {
                sum0 = sum0 +Convert.ToDouble(gcz[i]);//计算观测角度和
                if (i + 3 == N)                     //显示观测角度和
                {
                    if (N % 2 == 0)
                    {
                        jd[3 * N - 2].Text = l.hzj(sum0).ToString("f5");
                    }
                    else if (N % 2 == 1)
                    {
                        jd[3 * N - 1].Text = l.hzj(sum0).ToString("f5");
                    }
                }
            }
            if (zb[0].Text == null || zb[N - 2].Text == null)
            {
                MessageBox.Show("请输入已知方位角");
            }
            else if (zb[0].Text != null && zb[N - 2].Text != null)
            {
                double b1 = l.jzh(Convert.ToDouble(zb[0].Text));
                double b2 = l.jzh(Convert.ToDouble(zb[N - 2].Text));
                if (com1.Text == "左角")
                {
                    fb = b1 + sum0 - (N - 2) *Math.PI -b2;                   
                    while (fb * Math.Sign(fb) > Math.PI)               //处理fb差若干2PI问题。
                    {
                        fb = fb - 2 * Math.PI * Math.Sign(fb);
                    }
                    v = fb / (N - 2)*(-1);
                }
                else if (com1.Text == "右角")
                {
                    fb = b1 + (N - 2) * Math.PI - sum0 - b2;
                    while (fb * Math.Sign(fb) > Math.PI)             //处理fb差若干2PI问题。
                    {
                        fb = fb - 2 * Math.PI * Math.Sign(fb);
                    }
                    v = fb / (N - 2);
                }
               
                for (int i = 0; i < gcz.Count; i++)
                {
                    jd[N + i].Text = (l.hzj(v)*10000).ToString("f1")+"\n" +l.hzj(Convert.ToDouble(gcz[i])).ToString("f5"); //带有改正数的观测值
                    if (N % 2 == 0)
                    {
                        jd[3 * N - 1].Text = l.hzj(sum0 + (N - 2) * v).ToString("f5");                                 //改正后观测角和
                    }
                    else if (N % 2 == 1)
                    {
                        jd[3 * N - 2].Text = l.hzj(sum0 + (N - 2) * v).ToString("f5");
                    }
                    //jd[3 * N - 2].Text = l.hzj(sum0).ToString("f4");                                         
                    jd[2 * N - 2 + i].Text =l.hzj(Convert.ToDouble(gcz[i]) + v).ToString("f5");              //改正后观测角
                    double x1 = Convert.ToDouble(zb[i].Text);
                    double x2 = Convert.ToDouble(jd[2 * N - 2 + i].Text);
                    double x = 0;
                    if (com1.Text == "左角")                                                        //计算坐标方位角
                    {                       
                       x = l.jzh(x1) + JDZH.PI + l.jzh(x2);                        
                    }
                    else if (com1.Text == "右角")
                    {                      
                       x = l.jzh(x1) + JDZH.PI - l.jzh(x2);
                    }
                    while (x < 0 | x * Math.Sign(x) > 2 * Math.PI)
                    {
                        x = x - 2 * Math.PI * Math.Sign(x);
                    }
                    if (i + 1 < N - 2)
                    {
                        zb[i + 1].Text = l.hzj(x).ToString("f5");          //显示方位角
                    }
                    zb[0].Font = new System.Drawing.Font(new System.Drawing.Font("宋体", 10), System.Drawing.FontStyle.Underline);
                    zb[N-2].Font = new System.Drawing.Font(new System.Drawing.Font("宋体", 10), System.Drawing.FontStyle.Underline);
                }                
            }
            //边长及坐标增量计算
            bc = new ArrayList();               
            for (int i = N + 1; i < 2 * N - 2; i++)
            {
                double x = Convert.ToDouble(zb[i].Text);     //读取数据
                bc.Add(x);
            }
            for (int i = 0; i < bc.Count; i++)        //计算累计边长
            {
                sum1 = sum1 + Convert.ToDouble(bc[i]);
                double x1 = Convert.ToDouble(zb[i + 1].Text);     //提取方位角
                double x2 = Convert.ToDouble(zb[i + N + 1].Text); //提取边长
                double a = x2 * Math.Cos(l.jzh(x1));              //x坐标增量
                double b = x2 * Math.Sin(l.jzh(x1));              //y坐标增量
                sum2 = sum2 + a;
                sum3 = sum3 + b;
                zb[2 * N + i].Text = a.ToString("f3");
                zb[3 * N - 1 + i].Text = b.ToString("f3");               
            }
                double X1 = Convert.ToDouble(jd[3 * N].Text);      //提取已知点坐标
                double Y1 = Convert.ToDouble(jd[4 * N - 2].Text);
                double X2 = Convert.ToDouble(jd[4 * N - 3].Text);
                double Y2 = Convert.ToDouble(jd[5 * N - 5].Text);
                fx = sum2 - (X2 - X1);                            //计算fx
                fy = sum3 - (Y2 - Y1);                             //计算fy
                fs = Math.Sqrt(Math.Pow(fx, 2) + Math.Pow(fy, 2));  //计算f
                    for (int i=0;i<bc.Count;i++)
                    {
                        vx =-1* fx * Convert.ToDouble(bc[i]) / sum1;    //x坐标改正数
                        vy =-1* fy * Convert.ToDouble(bc[i]) / sum1;    //y坐标改正数
                        zb[4 * N - 2 + i].Text = (Convert.ToDouble(zb[2 * N + i].Text) + vx).ToString("f3"); //改正后x增量
                        zb[5 * N - 3 + i].Text = (Convert.ToDouble(zb[3 * N - 1 + i].Text) + vy).ToString("f3");//改正后y增量
                        sum4 = sum4 + Convert.ToDouble(zb[4 * N - 2 + i].Text);      
                        sum5 = sum5 + Convert.ToDouble(zb[5 * N - 3 + i].Text);
                        if (i < bc.Count - 1)
                        {
                            jd[3 * N + 1 + i].Text = (Convert.ToDouble(zb[4 * N - 2 + i].Text) + Convert.ToDouble(jd[3 * N + i].Text)).ToString("f3");
                            jd[4 * N - 1 + i].Text = (Convert.ToDouble(zb[5 * N - 3 + i].Text) + Convert.ToDouble(jd[4 * N - 2 + i].Text)).ToString("f3");
                        }
                        if (vx > 0)      //填写改正数
                        {
                            zb[2 * N + i].Text = "+" + (vx * 1000).ToString("f0") + "\n" + zb[2 * N + i].Text;
                        }
                        else
                        {
                            zb[2 * N + i].Text = (vx * 1000).ToString("f0") + "\n" + zb[2 * N + i].Text;
                        }
                        if (vy > 0)
                        {
                            zb[3 * N - 1+i].Text = "+" + (vy * 1000).ToString("f0") + "\n" + zb[3 * N - 1+i].Text;
                        }
                        else
                        {
                            zb[3 * N - 1+i].Text =(vy * 1000).ToString("f0") + "\n" + zb[3 * N - 1+i].Text;
                        }
                        
                    }

                zb[N * 6 - 5].Text = sum1.ToString("f3");   //显示累计边长
                zb[N * 6 - 4].Text = sum2.ToString("f3");   //x总增量
                zb[N * 6 - 3].Text = sum3.ToString("f3");   //y总增量
                zb[N * 6 - 2].Text = sum4.ToString("f3");   //改正后x总增量
                zb[N * 6 - 1].Text = sum5.ToString("f3");   //改正后y总增量
                jd[N * 5].Text = "角度闭合差： fb=" +(l.hzj(fb)*10000).ToString("f1") + "″                                   fx=" + fx.ToString("f3") + "m  fy=" + fy.ToString("f3") + "m\n";
                jd[N * 5].Text += "闭合差允许值： fb限= ±40√n = ±" + (40 * Math.Sqrt(N-2)).ToString("f2") + " ″               f= " + fs.ToString("f2") + "m\n";
                jd[N * 5].Text += "                                                        K= f/∑d= 1/（∑d/f）= 1/" +(Math.Round((sum1 / fs/100),0)*100).ToString("f0")+" K限= 1/3000";

            
            HT();//显示图形
        }
        private void SC()     //输出数据                   
        {
            if (t == 1)     //输出到txt
            {
                //读取控件中数据
                string[] a = jd[5 * N].Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //选择路保存径
                SaveFileDialog saveFile1 = new SaveFileDialog();
                saveFile1.Filter = "文本文件(.txt)|*.txt";
                saveFile1.FilterIndex = 1;
                if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0) //判断文件名是否输入
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile1.FileName, false);              //创建写入流
                    for (int i = 0; i < a.Length; i++)
                    {
                        string[] str = a[i].Split("   ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < str.Length / 2; j++)          //输出检核单元格内容
                        {
                            sw.Write(str[2 * j] + str[2 * j + 1] + " ");
                        }
                        sw.Write("\r\n");
                    }
                    for (int i = 0; i < N - 2; i++)                               //输出各点坐标
                    {

                        sw.Write(jd[i].Text + "," + jd[3 * N + i].Text + "," + jd[4 * N - 2 + i].Text + "\r\n");
                    }
                    sw.Close();             //关闭流
                }
            }
            else if (t == 2)        //输出到excel
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.ApplicationClass();
                Workbook book = excel.Workbooks.Add(Missing.Value);                           // 添加一个工作簿
                Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveSheet;// 获取当前工作表
                sheet.Name = "sheetName1";                                                   // 修改工作表的名字
                Range range = null;                                                          // 创建一个空的单元格对象
                string A = "A" + (2 * N + 2).ToString();
                string K = "K" + (2 * N + 2).ToString();
                range = (Range)sheet.get_Range("A1", A);   //设置点号列
                range.Borders.LineStyle = 1;
                range.RowHeight = 18;
                range.ColumnWidth = 5;
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter; 

                range = (Range)sheet.get_Range("B1", K);   //设置第二列至最后一列格式
                range.Borders.LineStyle = 1;
                range.RowHeight = 18;
                range.ColumnWidth = 12;
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                range = (Range)sheet.get_Range("B3", K);
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                range = (Range)sheet.get_Range("A1", K);  //设置字体
                range.Font.Name = "黑体";
                range.Font.Size = 12;

                range = (Range)sheet.get_Range("A1", "A2");
                range.Merge(0);   //合并
                sheet.Cells[1, 1] = "测\n点";
                range = (Range)sheet.get_Range("B1", "B2");
                range.Merge(0);   //合并
                sheet.Cells[1, 2] = "角度观测值\n  °  ′  ″  ";
                range = (Range)sheet.get_Range("C1", "C2");
                range.Merge(0);   //合并
                sheet.Cells[1, 3] = "改正后角值\n  °  ′  ″  ";
                range = (Range)sheet.get_Range("D1", "D2");
                range.Merge(0);   //合并
                sheet.Cells[1, 4] = "方位角\n  °  ′  ″  ";
                range = (Range)sheet.get_Range("E1", "E2");
                range.Merge(0);   //合并
                sheet.Cells[1, 5] = "边长";
                range = (Range)sheet.get_Range("F1", "G1");
                range.Merge(0);   //合并
                sheet.Cells[1, 6] = "坐 标 增 量";
                range = (Range)sheet.get_Range("H1", "I1");
                range.Merge(0);   //合并
                sheet.Cells[1, 8] = "改 正 后 坐 标 增 量";
                range = (Range)sheet.get_Range("J1", "K1");
                range.Merge(0);   //合并
                range = (Range)sheet.get_Range(A, K);   //检核单元格
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                range.Merge(0);
                range.RowHeight = 80;
                sheet.Cells[1, 10] = "坐  标";
                sheet.Cells[2, 6] = "△x";
                sheet.Cells[2, 7] = "△y";
                sheet.Cells[2, 8] = "△x";
                sheet.Cells[2, 9] = "△y";
                sheet.Cells[2, 10] = "x";
                sheet.Cells[2, 11] = "y";
                Range range1 = null;
                Range range2 = null;
              //  Range range3 = null;
                for (int i = 0; i < N - 2; i++)       //设计测点、角度、坐标列
                {
                    string C = "A" + (2 * i + 4).ToString();
                    string D = "A" + (2 * i + 1 + 4).ToString();
                    range1 = (Range)sheet.get_Range(C, D);
                    range1.Merge(0);
                   
                    string E = "B" + (2 * i + 4).ToString();
                    string F = "B" + (2 * i + 1 + 4).ToString();
                    range1 = (Range)sheet.get_Range(E, F);
                    range1.Merge(0);
                    string G = "C" + (2 * i + 4).ToString();
                    string H = "C" + (2 * i + 1 + 4).ToString();
                    range1 = (Range)sheet.get_Range(G, H);
                    range1.Merge(0);
                    string I = "J" + (2 * i + 4).ToString();
                    string J = "J" + (2 * i + 1 + 4).ToString();
                    range1 = (Range)sheet.get_Range(I, J);
                    range1.Merge(0);
                    string M = "K" + (2 * i + 4).ToString();
                    string L = "K" + (2 * i + 1 + 4).ToString();
                    range1 = (Range)sheet.get_Range(M, L);
                    range1.Merge(0);
                }
                for (int i = 0; i < N-1; i++)             //中间区域单元格
                {
                    string D = "D" + (2 * i + 3).ToString();
                    string E = "D" + (2 * i + 4).ToString();
                    range2 = (Range)sheet.get_Range(D, E);
                    range2.Merge(0);
                   
                    string F = "E" + (2 * i + 3).ToString();
                    string G = "E" + (2 * i + 4).ToString();
                    range2 = (Range)sheet.get_Range(F, G);
                    range2.Merge(0);
                   
                    string H = "F" + (2 * i + 3).ToString();
                    string I = "F" + (2 * i + 4).ToString();
                    range2 = (Range)sheet.get_Range(H, I);
                    range2.Merge(0);
                    
                    string J = "G" + (2 * i + 3).ToString();
                    string L = "G" + (2 * i + 4).ToString();
                    range2 = (Range)sheet.get_Range(J, L);
                    range2.Merge(0);
                    
                    string M = "H" + (2 * i + 3).ToString();
                    string O = "H" + (2 * i + 4).ToString();
                    range2 = (Range)sheet.get_Range(M, O);
                    range2.Merge(0);
                    
                    string P = "I" + (2 * i + 3).ToString();
                    string Q = "I" + (2 * i + 4).ToString();
                    range2 = (Range)sheet.get_Range(P, Q);
                    range2.Merge(0);
                    
                }

                    for (int i = 0; i < N; i++)   //写入两侧数据
                    {
                        if (i == 0)                 //起点号
                        {
                            sheet.Cells[3, 1] = qd.Text;
                        }
                        if (i > 0)                 //其余点
                        {
                            sheet.Cells[2 * i - 1 + 3, 1] = jd[i - 1].Text;
                            if (i < N - 1)                       //观测角
                            {
                                sheet.Cells[2 * i - 1 + 3, 2] = jd[N + i - 1].Text;
                                sheet.Cells[2 * i - 1 + 3, 3] = jd[2 * N - 2 + i].Text;
                                sheet.Cells[2 * i - 1 + 3, 10] = jd[3 * N - 1 + i].Text;
                                sheet.Cells[2 * i - 1 + 3, 11] = jd[4 * N - 3 + i].Text;
                            }
                        }
                        sheet.Cells[2 * i + 3, 4] = zb[i].Text;               //写入方位角
                        sheet.Cells[2 * i + 3, 5] = zb[N + i].Text;           //写入边长
                        sheet.Cells[2 * i + 3, 6] = zb[2 * N - 1 + i].Text;   //△x
                        sheet.Cells[2 * i + 3, 7] = zb[3 * N - 2 + i].Text;   //△y
                        sheet.Cells[2 * i + 3, 8] = zb[4 * N - 3 + i].Text;       //△x
                        sheet.Cells[2 * i + 3, 9] = zb[5 * N - 4 + i].Text;       //△y



                        sheet.Cells[2 * N + 1, 1] = "∑";      //累计行    
                        if (N % 2 == 0)
                        {
                            sheet.Cells[2 * N + 1, 2] = jd[3 * N - 2].Text;
                            sheet.Cells[2 * N + 1, 3] = jd[3 * N - 1].Text;
                        }
                        else if (N % 2 == 1)
                        {
                            sheet.Cells[2 * N + 1, 2] = jd[3 * N - 1].Text;
                            sheet.Cells[2 * N + 1, 3] = jd[3 * N - 2].Text;
                        }
                       
                    }
                    sheet.Cells[2 * N + 1, 5] = zb[6 * N - 5].Text;
                    sheet.Cells[2 * N + 1, 6] = zb[6 * N - 4].Text;
                    sheet.Cells[2 * N + 1, 7] = zb[6 * N - 3].Text;
                    sheet.Cells[2 * N + 1, 8] = zb[6 * N - 2].Text;
                    sheet.Cells[2 * N + 1, 9] = zb[6 * N - 1].Text;
                    sheet.Cells[2 * N + 2, 1] = jd[N * 5].Text;
                SaveFileDialog saveFile2 = new SaveFileDialog();  //打开保存界面并输入文件名
                saveFile2.Filter = "Excel工作簿(.xlsx)|*.xlsx";  //保存格式 
                saveFile2.FilterIndex = 2;
                if (saveFile2.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile2.FileName.Length > 0)//判断是否输入文件名
                {
                    string P_str_path = saveFile2.FileName;      //读取文件名
                    excel.Visible = true;                        //显示Excel表格
                    excel.ActiveWorkbook.SaveAs(P_str_path);     //保存工作表
                }
            }
        }
        private void HT()     //绘图                       
        {
            sczb = new List<double>();
            JDZH l = new JDZH();
            HTCL ll = new HTCL();
            double ab = sum1 / (N - 3);
            double xA = Convert.ToDouble(jd[3 * N].Text) - ab * Math.Cos(l.jzh(Convert.ToDouble(zb[0].Text)));    //起点后视概略坐标
            double yA = Convert.ToDouble(jd[4 * N - 2].Text) - ab * Math.Sin(l.jzh(Convert.ToDouble(zb[0].Text)));
            double xd = Convert.ToDouble(jd[4 * N - 3].Text) + ab * Math.Cos(l.jzh(Convert.ToDouble(zb[N - 2].Text)));//终点前视概略坐标
            double yd = Convert.ToDouble(jd[5 * N - 5].Text) + ab * Math.Sin(l.jzh(Convert.ToDouble(zb[N - 2].Text)));
            sczb.Add(xA);
            sczb.Add(yA);
            for (int i = 3 * N; i < 4 * N - 2; i++)
            {
                sczb.Add(Convert.ToDouble(jd[i].Text));
                sczb.Add(Convert.ToDouble(jd[i + N - 2].Text));
            }
            sczb.Add(xd);
            sczb.Add(yd);
            double xmax = sczb[0];
            double xmin = sczb[0];
            double ymax = sczb[1];
            double ymin = sczb[1];

            for (int i = 2; i < sczb.Count / 2 + 1; i++)        //选出最大最小x，y值
            {
                if (sczb[2 * i - 2] < xmin) { xmin = sczb[2 * i - 2]; }
                if (sczb[2 * i - 2] > xmax) { xmax = sczb[2 * i - 2]; }
                if (sczb[2 * i - 1] < ymin) { ymin = sczb[2 * i - 1]; }
                if (sczb[2 * i - 1] > ymax) { ymax = sczb[2 * i - 1]; }
            }
            double xz = (xmax + xmin) / 2;
            double yz = (ymax + ymin) / 2;
            double m1 = pictureBox1.Width / (ymax - ymin) * 0.9;//计算水平比例尺
            double m2 = pictureBox1.Height / (xmax - xmin) * 0.9;//计算竖直比例尺
            double m3 = m1;
            if (m2 < m1)        //选择小比例尺
            {
                m3 = m2;
            }

            Graphics gr = pictureBox1.CreateGraphics();    //绘图过程
            Pen p = new Pen(Color.Red, 1);
            pic1 = new System.Drawing.Point[sczb.Count / 2];
            for (int i = 1; i < sczb.Count / 2 + 1; i++)
            {
                pic1[i - 1].X = (int)((sczb[2 * i - 1] - yz) * m3 + pictureBox1.Width / 2 + 0.0000000000001);   //计算测点图上坐标
                pic1[i - 1].Y = (int)(pictureBox1.Height / 2 - (sczb[2 * i - 2] - xz) * m3);
            }

            for (int i = 0; i < pic1.Length-1; i++)
            {
                gr.DrawLine(p, pic1[i], pic1[i + 1]);            //连线
                if (i > 1 & i < pic1.Length - 2)
                {
                    gr.DrawEllipse(p, pic1[i].X - 2, pic1[i].Y - 2, 4, 4);  //测点上画圆
                    gr.DrawString((i - 1).ToString(), new System.Drawing.Font("vredana", 10), new SolidBrush(Color.Blue), pic1[i].X, pic1[i].Y + 6);
                }

            }
            for(int i=0;i<pic1.Length;i++)
            {
                if (i == 0 | i == pic1.Length - 2)
                {
                    int u = pic1.Length - 2;
                    System.Drawing.Point[] pic2 = new System.Drawing.Point[2];     //双线

                    pic2[0] = pic1[i];
                    pic2[1] = pic1[i + 1];
                    pic2 = ll.sx(pic2);
                    gr.DrawLine(p, pic2[0], pic2[1]);
                   
                }
                if(i<2|i>=pic1.Length-2)
                {
                                                   // 三角点
                    int k=pic1.Length-2;
                    System.Drawing.Point[] pic3 = new System.Drawing.Point[3];
                    string jw = qd.Text;
                    pic3[0] = pic1[i];    //实际点坐标
                    pic3 = ll.sjx(pic3);  //三角形角点坐标
                    gr.DrawLines(p, pic3);
                    gr.DrawLine(p, pic3[0], pic3[2]);
                    if (i == 0)                   //点号
                    {
                        jw = qd.Text;
                    }
                    else if (i == 1)
                    {
                        jw = jd[0].Text;
                    }
                    else if (i == k)
                    {
                        jw = jd[N - 3].Text;
                    }
                    else if (i == k + 1)
                    {
                        jw = jd[N - 2].Text;
                    }
                    gr.DrawString(jw , new System.Drawing.Font("vredana", 10), new SolidBrush(Color.Blue), pic3[0].X, pic3[0].Y + 6);
                }

            }
        } 
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }
        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                JS();
                km = 1;
            }
            catch
           {
               MessageBox.Show("数据格式错误或未输入数据");
               return;
            }
            finally { }

        }
        private void 绘图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (km == 1)
                {
                   // HT();
                }
                else
                {
                    MessageBox.Show("请先计算");
                }
            }
            catch { }
            finally { }
        }
        private void 导出到txtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (km == 1)
                {
                    t = 1;
                    SC();
                }
                else 
                {
                    MessageBox.Show("请先计算");
                }
            }
            catch(Exception r)
            {
            MessageBox.Show(r.ToString());
            }
            finally { }
        }
        private void 导出到excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (km == 1)
                {
                    t = 2;
                    SC();
                }
                else {
                    MessageBox.Show("请先计算");
                }
            }
            catch { }
            finally
            {
                GC.Collect();
                System.Windows.Forms.Application.Exit();//关闭
            }
        }
    }
}
