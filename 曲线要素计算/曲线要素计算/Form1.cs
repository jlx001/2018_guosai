using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace 曲线要素计算
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class T             //自定义list
        {
            public string 点名 { get; set; }
            public string X坐标 { get; set; }
            public string Y坐标 { get; set; }
            public string 里程 { get; set; }

        }
        double R, ls, km, kn = 0;  //半径，缓曲线长
        List<T> dm = new List<T>();  //表格用
        List<T> dn = new List<T>();  //报告用
        List<T> bz = new List<T>();   //绘图用
        List<T> ds = new List<T>();  //输出用
        List<double> zb = new List<double>();
        List<double> bj = new List<double>();
        List<double> hqc = new List<double>();
        List<double> ys = new List<double>();

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           dm = new List<T>();  //表格用
           dn = new List<T>();  //报告用
           bz = new List<T>();   //绘图用
           ds = new List<T>();  //输出用
           zb = new List<double>();
           bj = new List<double>();
           hqc = new List<double>();
           ys = new List<double>();
            try
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "文本文件(*.txt)|*.txt";
                if (file.ShowDialog() == DialogResult.OK&file.FileName!="")
                {
                    string[] str1 = File.ReadAllLines(file.FileName);
                    km = str1.Length;
                    for (int i = 0; i < str1.Length; i++)
                    {
                        string[] str2 = str1[i].Split(new char[] { ',' });
                        zb.Add(Convert.ToDouble(str2[1]));
                        zb.Add(Convert.ToDouble(str2[2]));
                        bj.Add(Convert.ToDouble(str2[3]));
                        hqc.Add(Convert.ToDouble(str2[4]));
                        dm.Add(new T { 点名 = str2[0], X坐标 = str2[1], Y坐标 = str2[2] });
                        bz.Add(new T { 点名 = str2[0], X坐标 = str2[1], Y坐标 = str2[2] });
                    }
                    JS();
                }
            }
            catch
            {
                MessageBox.Show("文件格式错误");
                return;
            }
            finally { }
            // dgv1.DataSource = dm;
           
         
        }
        private void DCBG()     //生成报告  
        {
            if (ls != 0)
            {
                richTextBox1.Text += "     缓和曲线要素\n--------------------------------------------------\n";
                // richTextBox1.Text += "点名     X（m）        Y（m）       里程（m）\n";                  
            }
            else if (ls == 0)
            {
                richTextBox1.Text += "     圆曲线要素\n--------------------------------------------------\n";
            }
            double x = ys[0];
            double x1 = Math.Floor(x);
            double x2 = Math.Floor((x - x1) * 100);
            double x3 = Math.Floor((x - x1 - x2 / 100) * 10000);
            richTextBox1.Text += "线路转角α： " + x1.ToString() + "°" + x2.ToString() + "′" + x3.ToString() + "″\n";
            richTextBox1.Text += "切线长T：  " + ys[1].ToString("f3") + "\n";
            richTextBox1.Text += "曲线长L：  " + ys[2].ToString("f3") + "\n";
            richTextBox1.Text += "外矢距E：  " + ys[3].ToString("f3") + "\n";
            richTextBox1.Text += "切曲差q：  " + ys[4].ToString("f3") + "\n";
            richTextBox1.Text += "  各点坐标及里程\n--------------------------------------------------\n";
            richTextBox1.Text += "点名     X（m）        Y（m）       里程（m）\n";

            for (int i = 0; i < dn.Count; i++)
            {
                int j = dn[i].点名.Length;
                while (j <= 4)
                {
                    dn[i].点名 = dn[i].点名 + " ";  //补齐点名后空格
                    j++;
                }

                richTextBox1.Text += dn[i].点名 + "    " + dn[i].X坐标 + "      " + dn[i].Y坐标 + "     " + dn[i].里程 + "\n";
            }
        }
        private void HT()         //绘图
        {
          
           // Series Y = new Series();           
           
            List<double> x = new List<double>();  //图形坐标
            List<double> y = new List<double>();
            List<double> dx = new List<double>();
            List<double> dy = new List<double>();
            for (int i = 0; i < bz.Count; i++)       //标注点位
            {
                dx.Add(Convert.ToDouble(bz[i].Y坐标));
                dy.Add(Convert.ToDouble(bz[i].X坐标));

                chart1.Series[2].Points.DataBindXY(dx, dy);
            }

            for (int i = 0; i < dm.Count; i++)
            {

                if (i < km)           //已知坐标
                {
                    x.Add(Convert.ToDouble(dm[i].Y坐标));
                    y.Add(Convert.ToDouble(dm[i].X坐标));
                    chart1.Series[1].Points.DataBindXY(x, y);
                }
                else if (i >= km & dm[i].X坐标 != " ")               //计算坐标 
                {
                    x.Add(Convert.ToDouble(dm[i].Y坐标));
                    y.Add(Convert.ToDouble(dm[i].X坐标));

                    chart1.Series[0].Points.DataBindXY(x, y);
                }

            }
            double x0 = 0, x9 = 0, y0 = 0, y9 = 0;    //确定图形位置
            x0 = x[0];
            x9 = x[0];
            y0 = y[0];
            y9 = y[0];
            for (int j = 0; j < km; j++)
            {
                if (x[j] < x0) { x0 = x[j]; }
                if (x[j] > x9) { x9 = x[j]; }
                if (y[j] < y0) { y0 = y[j]; }
                if (y[j] > y9) { y9 = y[j]; }
            }
          
            chart1.ChartAreas[0].AxisX.Maximum = x9 + 0.05 * x9;  //设置坐标轴范围
            chart1.ChartAreas[0].AxisX.Minimum = x0 - 0.05 * x0;
            chart1.ChartAreas[0].AxisY.Maximum = y9 + 0.05 * y9;
            chart1.ChartAreas[0].AxisY.Minimum = y0 - 0.05 * y0;
            chart1.Annotations.Clear();  //清除已有标注
            for (int i = 0; i < bz.Count; i++)                     //标注点名
            {
                TextAnnotation txt = new TextAnnotation();                
                txt.AnchorDataPoint = chart1.Series[2].Points[i];
                txt.Text = bz[i].点名;
                chart1.Annotations.Add(txt);
            }
        }
        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        { if (kn !=1)
            {
                MessageBox.Show("还未进行计算");
            }
            else if(kn==1)
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
                        sw.WriteLine(s + "\n");
                    }
                    sw.Close();
                }
            }
        } 
        string L,N;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (chart1.Width > 5 * toolStrip1.Width)
            {
                MessageBox.Show("无法进一步放大");
            }
            else
            {
                chart1.Width = Convert.ToInt32(1.5 * chart1.Width);
                chart1.Height = Convert.ToInt32(1.5 * chart1.Height);
            }
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            if (chart1.Width < 0.5 * toolStrip1.Width)
            {
                MessageBox.Show("无法进一步缩小");
            }
            else
            {
                chart1.Width = Convert.ToInt32(0.5 * chart1.Width);
                chart1.Height = Convert.ToInt32(0.5 * chart1.Height);
            }
        }
        private Point mouse_offset;

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);//
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            ((Control)sender).Cursor = Cursors.Arrow;//设置拖动时鼠标箭头
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);//设置偏移
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            chart1.Width = toolStrip1.Width;
            chart1.Height = tab2.Height-toolStrip1.Height;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void 绘图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kn != 1)
            {
                MessageBox.Show("还未进行计算");
            }
            else if (kn == 1)
            {
                SaveFileDialog file = new SaveFileDialog();
                file.Filter = "(*.dxf)|*.dxf";
                if (file.ShowDialog() == DialogResult.OK && file.FileName.Length > 0)
                {
                    try
                    {
                        StreamWriter sw = new StreamWriter(file.FileName, false);
                        sw.WriteLine("  0\r\nSECTION\r\n  2\r\nENTITIES");       //dxf文件开始标志
                        for (int i = 0; i < ds.Count - 1; i++)                   //曲线
                        {
                            L = "  0\r\nLINE\r\n  8\r\n1\r\n 10\r\n" + ds[i].Y坐标 + "\r\n 20\r\n" + ds[i].X坐标 + "\r\n 11\r\n" + ds[i + 1].Y坐标 + "\r\n 21\r\n" + ds[i + 1].X坐标;
                            sw.WriteLine(L);
                        }
                        for (int i = 0; i < bz.Count; i++)                        //标注
                        {
                            N = "  0\r\nTEXT\r\n  8\r\n0\r\n 10\r\n" + bz[i].Y坐标 + "\r\n 20\r\n" + bz[i].X坐标 + "\r\n 30\r\n0.0\r\n 40\r\n5.0\r\n1\r\n" + bz[i].点名 + "\r\n7\r\n等线体";
                            sw.WriteLine(N);
                        }
                        for (int i = 0; i < km - 1; i++)                        //已知点连线
                        {
                            L = "  0\r\nLINE\r\n  8\r\n1\r\n 10\r\n" + dm[i].Y坐标 + "\r\n 20\r\n" + dm[i].X坐标 + "\r\n 11\r\n" + dm[i + 1].Y坐标 + "\r\n 21\r\n" + dm[i + 1].X坐标;
                            sw.WriteLine(L);
                        }
                        sw.WriteLine("  0\r\nENDSEC\r\n  0\r\nEOF");          //结束
                        sw.Close();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.ToString());
                    }
                    finally { }
                }
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void JS()
        {
            double a1, a2, a, t, l, E, q, m, P, Bo, Khy, Kqz=0, Kjd = 0, Kyh=0, Kzh=0, Khz = 0,Kzd, K1hz = 0, X1 = 0, Y1 = 0;//ZY到JD方位角，YZ到JD方位角，偏角，曲线要素及里程
            double Xzh, Yzh, Xhz = 0, Yhz = 0, Xhy = 0, Yhy = 0, Xyh=0, Yyh=0, Xqz=0, Yqz=0;      //主点坐标
            
            int K;
            double K1 = 0, Ki = 0, Li = 0, Qi, Xi, Yi, Xii = 0, Yii = 0;  //中间变量
            R = 0;ls = 0;
            richTextBox1.Text = "     道路曲线要素与里程计算\n\n";
            
            for (int i = 0; i < zb.Count / 2 - 2; i++)
            {
                dn = new List<T>();
                double x1 = zb[2 * i];           //提取ZY，JD，YZ，方向线上3点坐标p，JD，q；
                double y1 = zb[2 * i + 1];
                double x2 = zb[2 * i + 2];
                double y2 = zb[2 * i + 3];
                double x3 = zb[2 * i + 4];
                double y3 = zb[2 * i + 5];
                ys = new List<double>();
                a1 = Math.Atan((y2 - y1) / (x2 - x1));  //计算ZY到JD方向的坐标方位角
                if ((y2 - y1) != 0 & (x2 - x1) < 0)     //判断角度范围
                {
                    a1 = Math.PI + a1;
                }
                else if ((y2 - y1) < 0 & (x2 - x1) > 0)
                {
                    a1 = Math.PI * 2 + a1;
                }
                a2 = Math.Atan((y3 - y2) / (x3 - x2));  //计算YZ到JD方向的坐标方位角
                if ((y3 - y2) != 0 & (x3 - x2) < 0)         //判断角度范围
                {
                    a2 = Math.PI + a2;
                }
                else if ((y3 - y2) < 0 & (x3 - x2) > 0)
                {
                    a2 = Math.PI * 2 - a2;
                }
                JDZH o = new JDZH();
                double a10 = o.hzj(a1);
                double a20 = o.hzj(a2);
                a = a1 - a2;   //a计算偏角值；
                while (Math.Abs(a) > Math.PI)   //方位角大于360°时-360°
                {
                    a = a - Math.Sign(a) * Math.PI * 2;
                }

                double a0 = Math.Abs(a);
                double a11 = o.hzj(a0);
                R = bj[i + 1];
                ls = hqc[i + 1];                                        //计算缓和曲线参数
                m = ls / 2 - Math.Pow(ls, 3) / (240 * Math.Pow(R, 2));  
                P = Math.Pow(ls, 2) / (24 * R);
                Bo = ls / (2 * R);


                t = m + Math.Abs((R + P) * Math.Tan(a0 / 2));      //计算曲线要素
                l = Math.Abs(R * (a0 - 2 * Bo)) + 2 * ls;
                E = Math.Abs((R + P) * (1 / Math.Cos(a0 / 2)) - R);
                q = Math.Abs(2 * t - l);
                ys.Add(a11);
                ys.Add(t);
                ys.Add(l);
                ys.Add(E);
                ys.Add(q);
                if (i == 0)
                {
                    Kjd = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)); //计算主点里程
                }
                else if (i > 0)
                {
                    Kjd = Khz + Math.Sqrt(Math.Pow((x2 - Xhz), 2) + Math.Pow((y2 - Yhz), 2));
                }
                //K = 16;
                Kzh = Kjd - t;
                Khy = Kzh + ls;
                Kqz = Kzh + l / 2;
                Kyh = Kzh + l - ls;
                Khz = Kyh + ls;
                
                Xzh = x2 - t * Math.Cos(a1);
                Yzh = y2 - t * Math.Sin(a1);
                Xhz = x2 + t * Math.Cos(a2);
                Yhz = y2 + t * Math.Sin(a2);
                if (ls != 0)
                {
                    int f = 1;
                    if (a > 0)
                    {
                        f = -1;
                    }
                    Xhy = Xzh + (m + R * Math.Sin((0.5 * ls) / R)) * Math.Cos(a1) - f*(P + R * (1 - Math.Cos((0.5 * ls) / R))) * Math.Sin(a1);
                    Yhy = Yzh + (m + R * Math.Sin((0.5 * ls) / R)) * Math.Sin(a1) + f*(P + R * (1 - Math.Cos((0.5 * ls) / R))) * Math.Cos(a1);
                    Xqz = Xzh + (m + R * Math.Sin((l - ls) / 2 / R)) * Math.Cos(a1) - f*(P + R * (1 - Math.Cos((l - ls) / 2 / R))) * Math.Sin(a1);
                    Yqz = Yzh + (m + R * Math.Sin((l - ls) / 2 / R)) * Math.Sin(a1) + f*(P + R * (1 - Math.Cos((l - ls) / 2 / R))) * Math.Cos(a1);
                    Xyh = Xzh + (m + R * Math.Sin(((l - ls) - ls / 2) / R)) * Math.Cos(a1) - f*(P + R * (1 - Math.Cos(((l - ls) - ls / 2) / R))) * Math.Sin(a1);
                    Yyh = Yzh + (m + R * Math.Sin(((l - ls) - ls / 2) / R)) * Math.Sin(a1) + f*(P + R * (1 - Math.Cos(((l - ls) - ls / 2) / R))) * Math.Cos(a1);
                   // bz.Add(new T { 点名 = "ZH", X坐标 = Xzh.ToString(), Y坐标 = Yzh.ToString() });
                }
                else if (ls == 0)
                {
                    Xhy = Xzh;
                    Yhy = Yzh;
                    Xyh = Xhz;
                    Yyh = Yhz;
                    Qi = (l / 2 )/ R;
                    Xii = R * Math.Sin(Qi);
                    Yii = R * (1 - Math.Cos(Qi));
                    if (a > 0)
                    {
                        Yii = -1 * Yii;
                    }
                    Xqz = Xhy + Xii * Math.Cos(a1) - Yii * Math.Sin(a1);
                    Yqz = Yhy + Xii * Math.Sin(a1) + Yii * Math.Cos(a1);
                    
                }

                
                if (ls != 0)
                {
                    dm.Add(new T { 点名 = "ZH", X坐标 = Xzh.ToString("f3"), Y坐标 = Yzh.ToString("f3"), 里程 = Kzh.ToString("f3") });
                    dm.Add(new T { 点名 = "HY", X坐标 = Xhy.ToString("f3"), Y坐标 = Yhy.ToString("f3"), 里程 = Khy.ToString("f3") });
                    dm.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString("f3"), Y坐标 = Yqz.ToString("f3"), 里程 = Kqz.ToString("f3") });
                    dm.Add(new T { 点名 = "YH", X坐标 = Xyh.ToString("f3"), Y坐标 = Yyh.ToString("f3"), 里程 = Kyh.ToString("f3") });
                    dm.Add(new T { 点名 = "HZ", X坐标 = Xhz.ToString("f3"), Y坐标 = Yhz.ToString("f3"), 里程 = Khz.ToString("f3") });

                    dn.Add(new T { 点名 = "ZH", X坐标 = Xzh.ToString("f3"), Y坐标 = Yzh.ToString("f3"), 里程 = Kzh.ToString("f3") });
                    dn.Add(new T { 点名 = "HY", X坐标 = Xhy.ToString("f3"), Y坐标 = Yhy.ToString("f3"), 里程 = Khy.ToString("f3") });
                    dn.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString("f3"), Y坐标 = Yqz.ToString("f3"), 里程 = Kqz.ToString("f3") });
                    dn.Add(new T { 点名 = "YH", X坐标 = Xyh.ToString("f3"), Y坐标 = Yyh.ToString("f3"), 里程 = Kyh.ToString("f3") });
                    dn.Add(new T { 点名 = "HZ", X坐标 = Xhz.ToString("f3"), Y坐标 = Yhz.ToString("f3"), 里程 = Khz.ToString("f3") });
                }
                else if (ls == 0)
                {
                    dm.Add(new T { 点名 = "ZY", X坐标 = Xzh.ToString("f3"), Y坐标 = Yzh.ToString("f3"), 里程 = Kzh.ToString("f3") });
                    dm.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString("f3"), Y坐标 = Yqz.ToString("f3"), 里程 = Kqz.ToString("f3") });
                    dm.Add(new T { 点名 = "YZ", X坐标 = Xhz.ToString("f3"), Y坐标 = Yhz.ToString("f3"), 里程 = Khz.ToString("f3") });

                    dn.Add(new T { 点名 = "ZY", X坐标 = Xzh.ToString("f3"), Y坐标 = Yzh.ToString("f3"), 里程 = Kzh.ToString("f3") });
                    dn.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString("f3"), Y坐标 = Yqz.ToString("f3"), 里程 = Kqz.ToString("f3") });
                    dn.Add(new T { 点名 = "YZ", X坐标 = Xhz.ToString("f3"), Y坐标 = Yhz.ToString("f3"), 里程 = Khz.ToString("f3") });
                }
                if (ls != 0)   //缓和曲线坐标计算
                {
                    if (i == 0)
                    {
                        X1 = x1;
                        Y1 = y1;
                    }
                    for (int j = 0; j < (Kjd - t + l) / 10+1; j++)
                    {
                        if (Ki < Kzh - 10 )    //计算起点到ZH点里程坐标
                        {
                            Ki = j * 10 + K1;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xi = X1 + (Ki-K1hz)* Math.Cos(a1);
                            Yi = Y1 + (Ki-K1hz) * Math.Sin(a1);
                            dm.Add(new T { 点名 = "L" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "L" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "L" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });

                        }
                        else if (Ki > Kzh - 10 & Ki < Kzh) //计算ZH点和下一点里程
                        {
                            Ki = j * 10 + K1;
                            Li = Ki - Kzh;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xii = Li - Math.Pow(Li, 5) / (40 * Math.Pow(R, 2) * Math.Pow(ls, 2));
                            Yii = Math.Pow(Li, 3) / (6 * R * ls);
                            if (a > 0)
                            {
                                Yii = -1 * Yii;
                            }
                            Xi = Xzh + Xii * Math.Cos(a1) - Yii * Math.Sin(a1);
                            Yi = Yzh + Xii * Math.Sin(a1) + Yii * Math.Cos(a1);
                            dm.Add(new T { 点名 = "ZH", X坐标 = Xzh.ToString("f3"), Y坐标 = Yzh.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kzh - K * 1000).ToString("f3") });
                            dm.Add(new T { 点名 = "H" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            bz.Add(new T { 点名 = "ZH", X坐标 = Xzh.ToString(), Y坐标 = Yzh.ToString() });
                            dn.Add(new T { 点名 = "ZH", X坐标 = Xzh.ToString("f3"), Y坐标 = Yzh.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kzh - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "H" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "H" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                        }
                        else if (Ki > Kzh & Ki < Khy - 10)  //计算ZH点到HY点里程坐标
                        {
                            Ki = j * 10 + K1;
                            Li = Ki - Kzh;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xii = Li - Math.Pow(Li, 5) / (40 * Math.Pow(R, 2) * Math.Pow(ls, 2));
                            Yii = Math.Pow(Li, 3) / (6 * R * ls);
                            if (a > 0)
                            {
                                Yii = -1 * Yii;
                            }
                            Xi = Xzh + Xii * Math.Cos(a1) - Yii * Math.Sin(a1);
                            Yi = Yzh + Xii * Math.Sin(a1) + Yii * Math.Cos(a1);
                            dm.Add(new T { 点名 = "H" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "H" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "H" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                        }
                        else if (Ki > Khy - 10 & Ki < Khy)  //计算HY点和下一点里程坐标
                        {
                            Ki = j * 10 + K1;
                            Li = Ki - Kzh;
                            Qi = (Li - ls / 2) / R;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xii = m + R * Math.Sin(Qi);
                            Yii = P + R * (1 - Math.Cos(Qi));
                            if (a > 0)
                            {
                                Yii = -1 * Yii;
                            }
                            Xi = Xzh + Xii * Math.Cos(a1) - Yii * Math.Sin(a1);
                            Yi = Yzh + Xii * Math.Sin(a1) + Yii * Math.Cos(a1);
                            dm.Add(new T { 点名 = "HY", X坐标 = Xhy.ToString("f3"), Y坐标 = Yhy.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Khy - K * 1000).ToString("f3") });
                            dm.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            bz.Add(new T { 点名 = "HY", X坐标 = Xhy.ToString(), Y坐标 = Yhy.ToString() });
                            dn.Add(new T { 点名 = "HY", X坐标 = Xhy.ToString("f3"), Y坐标 = Yhy.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Khy - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                        }
                        else if (Ki > Khy & Ki < Kyh - 10)   //计算HY到YH点里程坐标
                        {
                            Ki = j * 10 + K1;
                            Li = Ki - Kzh;
                            Qi = (Li - ls / 2) / R;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xii = m + R * Math.Sin(Qi);
                            Yii = P + R * (1 - Math.Cos(Qi));
                            if (a > 0)
                            {
                                Yii = -1 * Yii;
                            }
                            Xi = Xzh + Xii * Math.Cos(a1) - Yii * Math.Sin(a1);
                            Yi = Yzh + Xii * Math.Sin(a1) + Yii * Math.Cos(a1);
                            dm.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            if (Ki > Kqz & Ki < Kqz+10)      //QZ点
                            {
                                dm.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString("f3"), Y坐标 = Yqz.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kqz - K * 1000).ToString("f3") });
                                dn.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString("f3"), Y坐标 = Yqz.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kqz - K * 1000).ToString("f3") });
                                bz.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString(), Y坐标 = Yqz.ToString() });
                            }
                        }
                        else if (Ki > Kyh - 10 & Ki < Khz)                   //计算YH到HZ
                        {
                            Ki = j * 10 + K1;
                            Li = Khz - Ki;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xii = Li - Math.Pow(Li, 5) / (40 * Math.Pow(R, 2) * Math.Pow(ls, 2));
                            Yii = Math.Pow(Li, 3) / (6 * R * ls);
                            if (a > 0)
                            {
                                Yii = -1 * Yii;
                            }
                            Xi = Xhz + Xii * Math.Cos(a2 + Math.PI) + Yii * Math.Sin(a2 + Math.PI);
                            Yi = Yhz + Xii * Math.Sin(a2 + Math.PI) - Yii * Math.Cos(a2 + Math.PI);
                            if (Ki > Kyh - 10 & Ki < Kyh + 10)                    //YH点
                            {
                                dm.Add(new T { 点名 = "YH", X坐标 = Xyh.ToString("f3"), Y坐标 = Yyh.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kyh - K * 1000).ToString("f3") });
                                dn.Add(new T { 点名 = "YH", X坐标 = Xyh.ToString("f3"), Y坐标 = Yyh.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kyh - K * 1000).ToString("f3") });
                                bz.Add(new T { 点名 = "YH", X坐标 = Xyh.ToString(), Y坐标 = Yyh.ToString() });
                            }
                            dm.Add(new T { 点名 = "H" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "H" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "H" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            if (Ki > Khz - 10&Ki<Khz)                               //HZ点
                            {
                                dm.Add(new T { 点名 = "HZ", X坐标 = Xhz.ToString("f3"), Y坐标 = Yhz.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Khz - K * 1000).ToString("f3") });
                                dn.Add(new T { 点名 = "HZ", X坐标 = Xhz.ToString("f3"), Y坐标 = Yhz.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Khz - K * 1000).ToString("f3") });
                                bz.Add(new T { 点名 = "HZ", X坐标 = Xhz.ToString(), Y坐标 = Yhz.ToString() });
                            }
                        }

                    }
                    dm.Add(new T { 点名 = " ", X坐标 = " ", Y坐标 = " ", 里程 = " " });    //输入空行
                    dn.Add(new T { 点名 = " ", X坐标 = " ", Y坐标 = " ", 里程 = " " });    //输入空行

                    K1 = Khz;                                                //提取本段终点 作为下一段起点
                    K1hz = Khz;
                    X1 = Xhz;                                                //返回本段终点坐标
                    Y1 = Yhz;
                    while (K1 % 10 != 0)                                    //下一段起始里程
                    {
                        K1 = K1 - (K1 % 10) + 10;
                    }
                }//
                else if (ls == 0)                         //圆曲线
                {
                    if (i == 0)     //起点坐标
                    {
                        X1 = x1;
                        Y1 = y1;
                    }
                    for (int j = 0; j < (Kjd - t + l) / 10+1; j++)
                    {
                        if (Ki < Khy - 10)
                        {
                            Ki = j * 10 + K1;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xi = X1 + (Ki - K1hz) * Math.Cos(a1);
                            Yi = Y1 + (Ki - K1hz) * Math.Sin(a1);
                            dm.Add(new T { 点名 = "L" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "L" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "L" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                        }
                        else if (Ki > Khy - 10 & Ki < Khy)
                        {
                            Ki = j * 10 + K1;
                            Li = Ki - Khy;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Qi = Li / R;
                            Xii = R * Math.Sin(Qi);
                            Yii = R * (1 - Math.Cos(Qi));
                            if (a > 0)
                            {
                                Yii = -1 * Yii;
                            }
                            Xi = Xzh + Xii * Math.Cos(a1) - Yii * Math.Sin(a1);
                            Yi = Yzh + Xii * Math.Sin(a1) + Yii * Math.Cos(a1);
                            dm.Add(new T { 点名 = "ZY", X坐标 = Xhy.ToString("f3"), Y坐标 = Yhy.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Khy - K * 1000).ToString("f3") });
                            bz.Add(new T { 点名 = "ZY", X坐标 = Xhy.ToString(), Y坐标 = Yhy.ToString() });
                            dm.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "ZY", X坐标 = Xhy.ToString("f3"), Y坐标 = Yhy.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Khy - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                        }
                        else if (Ki > Khy & Ki < Kyh - 10)
                        {
                            Ki = j * 10 + K1;
                            Li = Ki - Khy;
                            Qi = Li / R;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xii = R * Math.Sin(Qi);
                            Yii = R * (1 - Math.Cos(Qi));
                            if (a > 0)
                            {
                                Yii = -1 * Yii;
                            }
                            Xi = Xzh + Xii * Math.Cos(a1) - Yii * Math.Sin(a1);
                            Yi = Yzh + Xii * Math.Sin(a1) + Yii * Math.Cos(a1);
                            dm.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "Y" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            if (Ki > Kqz - 10 & Ki < Kqz)
                            {
                                dm.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString("f3"), Y坐标 = Yqz.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kqz - K * 1000).ToString("f3") });
                                dn.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString("f3"), Y坐标 = Yqz.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kqz - K * 1000).ToString("f3") });
                                bz.Add(new T { 点名 = "QZ", X坐标 = Xqz.ToString(), Y坐标 = Yqz.ToString() });
                            }
                        }
                        else if (Ki > Khz - 10 & Ki < Khz+10)
                        {
                            Ki = j * 10 + K1;
                            Li = Ki - Khy;
                            Qi = Li / R;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xii = R * Math.Sin(Qi);
                            Yii = R * (1 - Math.Cos(Qi));
                            if (a > 0)
                            {
                                Yii = -1 * Yii;
                            }
                            Xi = Xzh + Xii * Math.Cos(a1) - Yii * Math.Sin(a1);
                            Yi = Yzh + Xii * Math.Sin(a1) + Yii * Math.Cos(a1);
                            if (Ki > Kyh - 10 & Ki < Kyh + 10)
                            {
                                dm.Add(new T { 点名 = "YZ", X坐标 = Xyh.ToString("f3"), Y坐标 = Yyh.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kyh - K * 1000).ToString("f3") });
                                dn.Add(new T { 点名 = "YZ", X坐标 = Xyh.ToString("f3"), Y坐标 = Yyh.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Kyh - K * 1000).ToString("f3") });
                                bz.Add(new T { 点名 = "YZ", X坐标 = Xyh.ToString(), Y坐标 = Yyh.ToString() });
                            }

                        }
                    }
                    K1 = Khz;
                    K1hz = Khz;                 
                    X1 = Xyh;
                    Y1 = Yyh;
                    while (K1 % 10 != 0)
                    {
                        K1 = K1 - (K1 % 10) + 10;
                    }
                    dm.Add(new T { 点名 = " ", X坐标 = " ", Y坐标 = " ", 里程 = " " });
                    dn.Add(new T { 点名 = " ", X坐标 = " ", Y坐标 = " ", 里程 = " " });
                }
            if (i == zb.Count / 2 - 3)
                {
                    Kzd = Khz + (Math.Sqrt(Math.Pow((x3 - Xhz), 2) + Math.Pow((y3 - Yhz), 2)));   //终点里程
                    for (int j = 0; j < (Kzd - Khz) / 10; j++)
                    {
                        if (Ki < Kzd - 10)
                        {
                            Ki = j * 10 + K1;
                            K = Convert.ToInt32(Math.Floor(Ki / 1000));
                            Xi = X1 + (Ki - Khz) * Math.Cos(a2);
                            Yi = Y1 + (Ki - Khz) * Math.Sin(a2);
                            dm.Add(new T { 点名 = "L" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            dn.Add(new T { 点名 = "L" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                            ds.Add(new T { 点名 = "L" + j.ToString(), X坐标 = Xi.ToString("f3"), Y坐标 = Yi.ToString("f3"), 里程 = "K" + K.ToString() + "+" + (Ki - K * 1000).ToString("f3") });
                        }                        
                    }
                }               
                DCBG();
            }
           
            HT();
            kn = 1;
            dgv1.DataSource = dm;             //绑定datagridview控件数据源
            dgv1.Columns[0].Width = 100;     //设置格式与字体
            dgv1.Columns[1].Width = 100;
            dgv1.Columns[2].Width = 100;
            dgv1.DefaultCellStyle.Font = new Font("黑体", 10);
        }
    }

}


