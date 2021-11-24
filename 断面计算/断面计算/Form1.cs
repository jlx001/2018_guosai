using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace 断面计算
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class T
        {
            public string 点名 { get; set; }
            public double 里程 { get; set; }
            public double X坐标 { get; set; }
            public double Y坐标 { get; set; }
            public double 高程 { get; set; }
        }
        List<T> dw = new List<T>();         //所有点位信息
        List<T> ds = new List<T>();         //散点信息
        ArrayList ck = new ArrayList();     //已知高程--h0
        List<T> dj = new List<T>();         //
        List<T> yzd = new List<T>();          //已知点信息
        List<T> bz = new List<T>();
        List<double> fj = new List<double>();  //已知边方位
        List<double> gc = new List<double>();  //内插高程点高程
        List<double> S = new List<double>();   //纵断面梯形面积
        List<T> zs = new List<T>();             //内插高程点全部信息
        List<double> DD = new List<double>();    //线路距离
        List<T> HD = new List<T>();               //所有横断面
        List<T> hd = new List<T>();              //单独横断面
        List<T> zbz = new List<T>();              //坐标轴
        List<T> kd = new List<T>();             //刻度
        List<double> fw = new List<double>();   //坐标轴数量和轴大小范围
        List<T> zbbz = new List<T>();
        List<List<T>> ht = new List<List<T>>();             // 纵横断面平面图
        List<List<double>> ht1 = new List<List<double>>();  //纵横断面平面
        List<List<T>> ht2 = new List<List<T>>();      //纵横断面截面图
        List<T> ht3 = new List<T>();               //图名
        List<List<T>> ht4 = new List<List<T>>();  //dxd坐标轴
        double SH;
        int ks = 0;
        int k = 1;
        zbz zb = new zbz();
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)                              
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "文本文件(*.txt)|*.txt";
                if (file.ShowDialog() == DialogResult.OK & file.FileName != "")
                {
                    ks = 1;
                    string[] str0 = File.ReadAllLines(file.FileName);
                    for (int i = 0; i < str0.Length; i++)
                    {
                        string[] str2 = str0[i].Split(new char[] { ',' });
                        if (i < 2 & str0[i] != "")
                        {
                            for (int j = 0; j < str2.Length; j++)
                            {
                                ck.Add(str2[j]);
                            }
                        }
                        else if (i >= 2 & str0[i] != "")
                        {
                            dw.Add(new T { 点名 = str2[0], X坐标 = Convert.ToDouble(str2[1]), Y坐标 = Convert.ToDouble(str2[2]), 高程 = Convert.ToDouble(str2[3]) });
                            if (str2[0].Substring(0, 1) == "K")
                            {
                                yzd.Add(new T { 点名 = str2[0], X坐标 = Convert.ToDouble(str2[1]), Y坐标 = Convert.ToDouble(str2[2]), 高程 = Convert.ToDouble(str2[3]) });
                                bz.Add(new T { 点名 = str2[0], X坐标 = Convert.ToDouble(str2[1]), Y坐标 = Convert.ToDouble(str2[2]), 高程 = Convert.ToDouble(str2[3]) });
                            }
                            if (str2[0].Substring(0, 1) != "K")
                            {
                                ds.Add(new T { 点名 = str2[0], X坐标 = Convert.ToDouble(str2[1]), Y坐标 = Convert.ToDouble(str2[2]), 高程 = Convert.ToDouble(str2[3]) });
                            }
                        }

                    }
                    if (ks == 1)
                    {
                        fw.Add(ck.Count - 2);
                        dataGridView1.DataSource = dw;                      
                    }

                }
            }
            catch(Exception x)
            {
                MessageBox.Show("文件格式错误"+x.ToString());
            }
            finally
            {


            }
        }
        private void ZDM()                                                               //纵断面计算     
        {
            double x1 = 0, x2 = 0, y1 = 0, y2 = 0, xi, yi, h, Si = 0, sum0, sum1, Di, di, i0 = 0, l = 0, km = 0, K = 0;
            DD.Add(0);
            for (int i = 0; i < yzd.Count - 1; i++)
            {
                gc = new List<double>();
                x1 = Convert.ToDouble(yzd[i].X坐标);
                y1 = Convert.ToDouble(yzd[i].Y坐标);
                x2 = Convert.ToDouble(yzd[i + 1].X坐标);
                y2 = Convert.ToDouble(yzd[i + 1].Y坐标);


                gc.Add(Convert.ToDouble(yzd[i].高程));

                double a0 = Math.Atan((y2 - y1) / (x2 - x1));
                //添加K0点数据

                zs.Add(new T { 点名 = "K" + i.ToString(), 里程 = i0, X坐标 = yzd[i].X坐标, Y坐标 = yzd[i].Y坐标, 高程 = yzd[i].高程 });

                if ((x2 - x1) != 0 & (y2 - y1) < 0)     //判断角度象限
                {
                    a0 = Math.PI + a0;
                }
                else if ((x2 - x1) < 0 & (y2 - y1) > 0)
                {
                    a0 = Math.PI * 2 + a0;
                }
                Di = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
                i0 = i0 + Di;
                DD.Add(Di);
                for (int j = 0; (j + 1) * 5 < Di; j++)
                {
                    l = (j + 1) * 5 + K - DD[i];
                    xi = x1 - l * Math.Cos(a0);
                    yi = y1 - l * Math.Sin(a0);
                    List<double> dd = new List<double>();
                    for (int k = 0; k < ds.Count; k++)
                    {
                        di = Math.Sqrt(Math.Pow((xi - Convert.ToDouble(ds[k].X坐标)), 2) + Math.Pow((yi - Convert.ToDouble(ds[k].Y坐标)), 2));
                        dd.Add(di);
                    }
                    List<int> ii = new List<int>();                          //循环选出五个最近点并提取对应索引
                    double m0 = dd[0], m1 = dd[0], m2 = dd[0], m3 = dd[0], m4 = dd[0];
                    int n = 1, n0 = 0, n1 = 0, n2 = 0, n3 = 0, n4 = 0;
                    while (n < dd.Count)                   //第一点
                    {
                        if (dd[n] < m0)
                        {
                            m0 = dd[n];
                            n0 = n;
                        }
                        n++;
                    }
                    n = 1;
                    while (n < dd.Count)              //第二点
                    {
                        if (dd[n] > m0 & dd[n] < m1)
                        {
                            m1 = dd[n];
                            n1 = n;                  //记录对应索引
                        }
                        n++;
                    }
                    n = 1;
                    while (n < dd.Count)
                    {
                        if (dd[n] > m1 & dd[n] < m2)
                        {
                            m2 = dd[n];
                            n2 = n;
                        }
                        n++;
                    }
                    n = 1;
                    while (n < dd.Count)
                    {
                        if (dd[n] > m2 & dd[n] < m3)
                        {
                            m3 = dd[n];
                            n3 = n;
                        }
                        n++;
                    }
                    n = 1;
                    while (n < dd.Count)
                    {
                        if (dd[n] > m3 & dd[n] < m4)
                        {
                            m4 = dd[n];
                            n4 = n;
                        }
                        n++;
                    }
                    ii.Add(n0);
                    ii.Add(n1);
                    ii.Add(n2);
                    ii.Add(n3);
                    ii.Add(n4);

                    sum0 = (Convert.ToDouble(ds[n0].高程) / m0) + (Convert.ToDouble(ds[n1].高程) / m1) + (Convert.ToDouble(ds[n2].高程) / m2) + (Convert.ToDouble(ds[n3].高程) / m3) + (Convert.ToDouble(ds[n4].高程) / m4);
                    sum1 = (1 / m0) + (1 / m1) + (1 / m2) + (1 / m3) + (1 / m4);
                    h = sum0 / sum1;
                    gc.Add(h);

                    km++;   //累加里程
                    zs.Add(new T { 点名 = "V_" + km.ToString("f0"), 里程 = (km * 5), X坐标 = xi, Y坐标 = yi, 高程 = h });
                }

                for (int j = 0; j < gc.Count; j++)
                {
                    double h0 = Convert.ToDouble(ck[1]);
                    if (j < gc.Count - 1)
                    {
                        if (i == 0 | (i > 0 & j > 0))
                        {
                            Si = ((gc[j] + gc[j + 1] - 2 * h0) / 2) * 5;
                        }
                        else if (i > 0 & j == 0)
                        {
                            Si = ((gc[j] + gc[j + 1] - 2 * h0) / 2) * (K + 5 - DD[i]);
                        }
                    }
                    else if (j == gc.Count - 1)
                    {
                        Si = ((gc[j] + Convert.ToDouble(yzd[i + 1].高程) - 2 * h0) / 2) * (DD[i + 1] - l);
                    }
                    S.Add(Si);

                }
                K = l;
            }
            zs.Add(new T { 点名 = "K" + (yzd.Count - 1).ToString(), 里程 = i0, X坐标 = yzd[yzd.Count - 1].X坐标, Y坐标 = yzd[yzd.Count - 1].Y坐标, 高程 = yzd[yzd.Count - 1].高程 });

            ht.Add(zs);

            DCBG1();
        }    
        private void HDM()                                                               //横断面计算     
        {
            ///横断面计算
            double x1 = 0, x2 = 0, y1 = 0, y2 = 0, xi, yi, h, Si = 0, sum0, sum1, di, l = 0, km = 0;
            double Xm, Ym, Xn, Yn;
            gc = new List<double>();
            for (int i = 0; i < yzd.Count - 1; i++)
            {
                // gc = new List<double>();
                hd = new List<T>();
                km = 0;
                SH = 0;
                x1 = Convert.ToDouble(yzd[i].X坐标);
                y1 = Convert.ToDouble(yzd[i].Y坐标);
                x2 = Convert.ToDouble(yzd[i + 1].X坐标);
                y2 = Convert.ToDouble(yzd[i + 1].Y坐标);

                Xm = (x1 + x2) / 2;  //横断面中点坐标
                Ym = (y1 + y2) / 2;

                // gc.Add(Convert.ToDouble(yzd[i].高程));

                double a0 = Math.Atan((y2 - y1) / (x2 - x1));
                //添加K0点数据
                string[] N = new string[] { "M", "N", "O" };
                //HD.Add(new T {点名=N[i],X坐标= });
                if ((x2 - x1) != 0 & (y2 - y1) < 0)     //判断角度象限
                {
                    a0 = Math.PI + a0;
                }
                else if ((x2 - x1) < 0 & (y2 - y1) > 0)
                {
                    a0 = Math.PI * 2 + a0;
                }
                a0 = a0 + Math.PI / 2;
                while (Math.Abs(a0) > Math.PI * 2)
                {
                    a0 = a0 - Math.Sign(a0) * Math.PI * 2;
                }
                Xn = Xm - 25 * Math.Cos(a0);
                Yn = Ym - 25 * Math.Sin(a0);
                //Di = 50;
                // i0 = i0 + Di;
                for (int j = 0; j < 11; j++)
                {
                    l = j * 5;
                    xi = Xn + l * Math.Cos(a0);
                    yi = Yn + l * Math.Sin(a0);
                    List<double> dd = new List<double>();
                    for (int k = 0; k < ds.Count; k++)
                    {
                        di = Math.Sqrt(Math.Pow((xi - Convert.ToDouble(ds[k].X坐标)), 2) + Math.Pow((yi - Convert.ToDouble(ds[k].Y坐标)), 2));
                        dd.Add(di);
                    }
                    List<int> ii = new List<int>();                          //循环选出五个最近点并提取对应索引
                    double m0 = dd[0], m1 = 0, m2 = 0, m3 = 0, m4 = 0;
                    int n = 1, n0 = 0, n1 = 0, n2 = 0, n3 = 0, n4 = 0;

                    while (n < dd.Count)
                    {
                        if (dd[n] > m1)
                        {
                            m1 = dd[n];
                            m2 = dd[n];
                            m3 = dd[n];
                            m4 = dd[n];
                        }
                        n++;
                    }
                    n = 0;
                    while (n < dd.Count)                   //第一点
                    {
                        if (dd[n] < m0)
                        {
                            m0 = dd[n];
                            n0 = n;
                        }
                        n++;
                    }
                    n = 0;
                    while (n < dd.Count)              //第二点
                    {
                        if (dd[n] > m0 & dd[n] < m1)
                        {
                            m1 = dd[n];
                            n1 = n;                  //记录对应索引
                        }
                        n++;
                    }
                    n = 0;
                    while (n < dd.Count)
                    {
                        if (dd[n] > m1 & dd[n] < m2)
                        {
                            m2 = dd[n];
                            n2 = n;
                        }
                        n++;
                    }
                    n = 0;
                    while (n < dd.Count)
                    {
                        if (dd[n] > m2 & dd[n] < m3)
                        {
                            m3 = dd[n];
                            n3 = n;
                        }
                        n++;
                    }
                    n = 0;
                    while (n < dd.Count)
                    {
                        if (dd[n] > m3 & dd[n] < m4)
                        {
                            m4 = dd[n];
                            n4 = n;
                        }
                        n++;
                    }
                    ii.Add(n0);
                    ii.Add(n1);
                    ii.Add(n2);
                    ii.Add(n3);
                    ii.Add(n4);

                    sum0 = (Convert.ToDouble(ds[n0].高程) / m0) + (Convert.ToDouble(ds[n1].高程) / m1) + (Convert.ToDouble(ds[n2].高程) / m2) + (Convert.ToDouble(ds[n3].高程) / m3) + (Convert.ToDouble(ds[n4].高程) / m4);
                    sum1 = (1 / m0) + (1 / m1) + (1 / m2) + (1 / m3) + (1 / m4);
                    h = sum0 / sum1;
                    gc.Add(h);

                    string s = (km - 5).ToString("f0");
                    if (km - 5 >= 0)
                    {
                        s = "+" + s;
                    }
                    if (l == 25)
                    {
                        HD.Add(new T { 点名 = N[i], 里程 = (km * 5), X坐标 = xi, Y坐标 = yi, 高程 = h });
                        hd.Add(new T { 点名 = N[i], 里程 = (km * 5), X坐标 = xi, Y坐标 = yi, 高程 = h });
                        bz.Add(new T { 点名 = N[i], 里程 = (km * 5), X坐标 = xi, Y坐标 = yi, 高程 = h });
                    }
                    else
                    {
                        HD.Add(new T { 点名 = "C" + s, 里程 = (km * 5), X坐标 = xi, Y坐标 = yi, 高程 = h });
                        hd.Add(new T { 点名 = "C" + s, 里程 = (km * 5), X坐标 = xi, Y坐标 = yi, 高程 = h });
                    }


                    km++;   //累加里程
                }
                for (int j = 0; j < hd.Count - 1; j++)
                {
                    double h0 = Convert.ToDouble(ck[1]);
                    Si = ((Convert.ToDouble(hd[j].高程) + Convert.ToDouble(hd[j + 1].高程) - 2 * h0) / 2) * 5;
                    SH = SH + Si;
                }
                ht.Add(hd);
                DCBG2();

            }

        }    
        private void DCBG1()                                                             //生成报告       
        {
            double SS = 0, D = 0;
            richTextBox1.Text = "           纵横断面计算报告\n------------------------------------------------\n";
            richTextBox1.Text += "  纵断面信息\n------------------------------------------------\n";
            for (int i = 0; i < S.Count; i++)
            {
                SS = SS + S[i];
            }
            for (int i = 0; i < DD.Count; i++)
            {
                D = D + DD[i];
            }
            richTextBox1.Text += "纵断面面积： " + SS.ToString("f3") + "\n";
            richTextBox1.Text += "纵断面全长： " + D.ToString("f3") + "\n线路主点：\n点名    里程（K）      X坐标（m）     Y坐标（m）     H坐标（m）\n";
            for (int i = 0; i < zs.Count; i++)
            {
                int j = zs[i].点名.Length;

                string str1 = zs[i].里程.ToString("f3");
                int k = str1.Length;
                while (j <= 4)
                {
                    zs[i].点名 = zs[i].点名 + " ";  //补齐点名后空格
                    j++;
                }
                while (k < 6)
                {
                    str1 = " " + str1; ;
                    k++;
                }

                richTextBox1.Text += zs[i].点名 + "   " + str1 + "         " + zs[i].X坐标.ToString("f3") + "       " + zs[i].Y坐标.ToString("f3") + "       " + zs[i].高程.ToString("f3") + "\n";

            }
        }    
        private void DCBG2()                                                                              
        {
            richTextBox1.Text += "横断面信息\n------------------------------------------------\n";
            richTextBox1.Text += "横断面" + k.ToString() + "\n";
            k++;

            richTextBox1.Text += "横断面面积：" + SH.ToString("f3") + "\n";
            richTextBox1.Text += "横断面全长： 50\n";
            richTextBox1.Text += "线路主点：\n";
            richTextBox1.Text += "点名     里程（K）      X坐标（m）     Y坐标（m）     H坐标（m）\n";

            for (int i = 0; i < hd.Count; i++)
            {
                while (hd[i].点名.Length < 4)
                {
                    hd[i].点名 = hd[i].点名 + " ";
                }
                string str1 = hd[i].里程.ToString("f3");
                int k = str1.Length;
                while (k < 6)
                {
                    str1 = " " + str1;
                    k++;
                }

                richTextBox1.Text += hd[i].点名 + "     " + str1 + "         " + hd[i].X坐标.ToString("f3") + "       " + hd[i].Y坐标.ToString("f3") + "       " + hd[i].高程.ToString("f3") + "\n";
            }
        }
        private void HT1()                                                                //绘制图形      
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<double> h = new List<double>();
            List<double> l = new List<double>();
            for (int i = 0; i < ds.Count; i++)
            {
                x.Add(Convert.ToDouble(ds[i].X坐标));
                y.Add(Convert.ToDouble(ds[i].Y坐标));
            }
            for (int i = 0; i < zs.Count; i++)
            {
                l.Add(zs[i].里程);
                h.Add(zs[i].高程);
            }

            chart1.ChartAreas[1].AxisX.Minimum = 0;
            chart1.ChartAreas[1].AxisX.Maximum = 90;
            chart1.Series[5].Points.DataBindXY(l, h);
            double x0 = x[0];                         //判断基本图坐标范围
            double x9 = x[0];
            double y0 = y[0];
            double y9 = y[0];
            chart1.Series[0].Points.DataBindXY(x, y);
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] < x0)
                { x0 = x[i]; }
                if (x[i] > x9)
                { x9 = x[i]; }
                if (y[i] < y0)
                { y0 = y[i]; }
                if (y[i] > y9)
                { y9 = y[i]; }
            }
            double l0 = l[0], l9 = l[0], h0 = h[0], h9 = h[9];
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i] < l0)
                { l0 = l[i]; }
                if (l[i] > l9)
                { l9 = l[i]; }
                if (h[i] < h0)
                { h0 = h[i]; }
                if (h[i] > h9)
                {h9 = h[i]; }
            }
            fw.Add(x0);
            fw.Add(x9);
            fw.Add(y0);
            fw.Add(y9);
            fw.Add(l0);
            fw.Add(l9);
            fw.Add(h0);
            fw.Add(h9);
            chart1.ChartAreas[0].AxisX.Minimum = Math.Floor(1.00123*x0/10)*10;
           //chart1.ChartAreas[0].AxisX.Maximum = Math.Floor(1.002 * x9);
            chart1.ChartAreas[0].AxisY.Minimum = Math.Floor(0.996*y0/10)*10;
            // chart1.ChartAreas[0].AxisY.Maximum = Math.Floor(1.002 * y9);
            zbz = zb.z(fw);         //坐标轴
            kd = zb.jbkd(fw);        //刻度
            zbbz = zb.bz(fw);       //刻度标注
            x = new List<double>();
            y = new List<double>();
            for (int i = 0; i < yzd.Count; i++)                 //纵断面线
            {
                x.Add(Convert.ToDouble(yzd[i].X坐标.ToString("f1")));
                y.Add(Convert.ToDouble(yzd[i].Y坐标.ToString("f1")));
                chart1.Series[1].Points.DataBindXY(x, y);
            }
            x = new List<double>();
            y = new List<double>();
            for (int i = 0; i < bz.Count; i++)
            {
                x.Add(Convert.ToDouble(bz[i].X坐标.ToString("f1")));
                y.Add(Convert.ToDouble(bz[i].Y坐标.ToString("f1")));
                chart1.Series[2].Points.DataBindXY(x, y);

            }
            chart1.Annotations.Clear();                 //清除已有标注
            for (int i = 0; i < bz.Count; i++)                     //标注点名
            {
                TextAnnotation txt = new TextAnnotation();
                txt.AnchorDataPoint = chart1.Series[2].Points[i];
                txt.Text = bz[i].点名;
                chart1.Annotations.Add(txt);
            }

        }        
        private void HT2()                                                                                
        {
         for (int j = 1; j < ht.Count; j++)           //道路基本情况横断面图    
            {
                List<double> x1 = new List<double>();
                List<double> y1 = new List<double>();
                List<double> l1 = new List<double>();
                List<double> h1 = new List<double>();
                for (int i = 0; i < ht[j].Count; i++)
                {
                    x1.Add(ht[j][i].X坐标);
                    y1.Add(ht[j][i].Y坐标);
                    l1.Add((ht[j][i].里程-25));
                    h1.Add(ht[j][i].高程);
                }
                ht1.Add(x1);
                ht1.Add(y1);
                ht1.Add(l1);
                ht1.Add(h1);
            }

            for (int i = 0; i < ht.Count-1; i++)
            {             
                chart1.Series[i+3].Points.DataBindXY(ht1[4*i], ht1[4*i+1]);
               // chart1.Series[4].Points.DataBindXY(x2, y2);
                chart1.Series[i+6].Points.DataBindXY(ht1[4*i+2], ht1[4*i+3]);
                //chart1.Series[7].Points.DataBindXY(l2, h2);
            }
            chart1.ChartAreas[1].AxisY.Minimum = 10;
            chart1.ChartAreas[2].AxisY.Minimum = 10;
            chart1.ChartAreas[3].AxisY.Minimum = 10;
            chart1.ChartAreas[1].AxisY.Maximum = 20;
            chart1.ChartAreas[2].AxisY.Maximum = 20;
            chart1.ChartAreas[3].AxisY.Maximum = 20;
            chart1.ChartAreas[2].AxisX.Crossing = 0;                 //设置Y轴位置
            chart1.ChartAreas[2].AxisY.IsMarksNextToAxis = true;     //刻度随轴变化
            chart1.ChartAreas[3].AxisX.Crossing = 0;
            chart1.ChartAreas[3].AxisY.IsMarksNextToAxis = true;
        }
        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)              //导出报告      
        {
            if (ks == 0)
            {
                MessageBox.Show("还未计算");
                return;
            }
            else
            {
                SaveFileDialog file = new SaveFileDialog();
                file.Filter = "文本文件(*.txt)|*.txt";
                if (file.ShowDialog() == DialogResult.OK && file.FileName.Length > 0)
                {
                    StreamWriter sw = new StreamWriter(file.FileName, false);
                    string s;
                    for (int i = 0; i < richTextBox1.Lines.Length; i++)
                    {
                        s = richTextBox1.Lines[i];
                        sw.Write(s + "\r\n");
                    }
                    sw.Close();
                }
            }
        }  
        private void 绘图ToolStripMenuItem_Click(object sender, EventArgs e)             //输出dxf        
        {
            if (ks == 0)
            {
                MessageBox.Show("还未计算");
                return;
            }
            else
            {
                try
                {
                    SaveFileDialog file = new SaveFileDialog();
                    file.Filter = "(*.dxf)|*.dxf";
                    if (file.ShowDialog() == DialogResult.OK && file.FileName.Length > 0)
                    {
                        StreamWriter sw = new StreamWriter(file.FileName, false);
                        dxf dxf = new 断面计算.dxf();
                        sw.Write("0\r\nSECTION\r\n2\r\nENTITIES\r\n");
                        string[] str1 = dxf.pl(yzd);
                        for (int i = 0; i < str1.Length; i++)     //纵断面平面
                        {
                            sw.Write(str1[i]);
                        }
                        string[] str2 = dxf.txt(bz);
                        for (int i = 0; i < str2.Length; i++)
                        {
                            sw.Write(str2[i]);
                        }
                        for (int k = 1; k < ht.Count; k++)    //横断面平面图
                        {
                            string[] str3 = dxf.pl(ht[k]);
                            for (int i = 0; i < str3.Length; i++)
                            {
                                sw.Write(str3[i]);
                            }
                        }
                        string[] str4 = dxf.point(ds);
                        for (int i = 0; i < ds.Count; i++)
                        {
                            sw.Write(str4[i]);
                        }
                        string[] str5 = dxf.dl(zbz);
                        for (int i = 0; i < str5.Length; i++)     //坐标轴和箭头
                        {
                            sw.Write(str5[i]);

                        }
                        string[] str6 = dxf.dl(kd);
                        for (int i = 0; i < kd.Count / 2; i++)   //刻度线
                        {
                            sw.Write(str6[i]);
                        }
                        string[] str7 = dxf.txt(zbbz);
                        for (int i = 0; i < zbbz.Count; i++)    //刻度线标注
                        {
                            sw.Write(str7[i]);
                        }
                        //坐标转换               
                        ht2 = zb.zh(fw, ht);
                        for (int i = 0; i < ht2.Count; i++)
                        {
                            string[] str8 = dxf.pl(ht2[i]);
                            for (int j = 0; j < str8.Length; j++)
                            {
                                sw.Write(str8[j]);
                            }
                        }
                        //图名
                        ht3 = zb.tm(fw);
                        string[] str9 = dxf.txt(ht3);
                        for (int i = 0; i < str9.Length; i++)
                        {
                            sw.Write(str9[i]);
                        }
                        sw.Write("0\r\nENDSEC\r\n0\r\nEOF\r\n");
                        sw.Close();
                        MessageBox.Show("导出成功");
                    }
                }
                catch(Exception x)
                {
                    MessageBox.Show("导出失败"+x.ToString());
                    return;
                }
               
            }
        }
  

        
        //private Point mouse_offset;                 //定义鼠标点击

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
           // mouse_offset = new Point(-e.X, -e.Y);//
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
           /* ((Control)sender).Cursor = Cursors.Arrow;                  //设置拖动时鼠标箭头
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);         //设置偏移
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }*/
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ZDM();
                HDM();
                HT1();
                HT2();
                MessageBox.Show("计算已完成");
            }
            catch (Exception x)
            {
                MessageBox.Show("没有数据");
            }
        }
    }
}
