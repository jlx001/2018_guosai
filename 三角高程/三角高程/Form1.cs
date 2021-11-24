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

namespace 三角高程
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class T 
        {
            public string 测段 { get; set; }
            public string 往返 { get; set; }
            public double 斜距 { get; set; }
            public double 垂直角 { get; set; }
            public double 仪器高 { get; set; }
            public double 目标高 { get; set; }
            public string 球气差 { get; set; }
            public double  平距 { get; set; }
            public double  高差 { get; set; }
            public double  高差平均值 { get; set; }
            public string 超限标识 { get; set; }
        }
        public class M 
        {
            public string 测段 { get; set; }
            public string 往返 { get; set; }
            public string 斜距 { get; set; }
            public string 垂直角 { get; set; }
            public string 仪器高 { get; set; }
            public string 目标高 { get; set; }
            public string 球气差 { get; set; }
            public string 平距 { get; set; }
            public string 高差 { get; set; }
            public string 高差平均值 { get; set; }
            public string 超限标识 { get; set; }
        }
        public class K 
        {
            public string 点名 { get; set; }
            public double 平距 { get; set; }
            public double 高差 { get; set; }
            public double 改正数 { get; set; }
            public double 改正后高差 { get; set; }
            public double 高程 { get; set; }
        }
        List<T> ysj = new List<T>();
        List<M> jsh=new List<M>();
        List<K> gc = new List<K>();
        List<double> H0 = new List<double>();
        List<double> y0 = new List<double>();
        List<double> vi = new List<double>();
        int ks = 0;
        string A;     //已知点名
        double H;     //已知点高程
        double R = 6378137, k = 0.15,sum0=0,sum1=0;
        double h = 0, Hi = 0, dr = 0, fh = 0, v = 0, hi = 0, u = 0, Pi = 0, mi = 0,di=0;
        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "文本文件(*.txt)|*.txt";
            ArrayList s = new ArrayList();
            if (file.ShowDialog() == DialogResult.OK & file.FileName != "")
            {
                ks = 1;
                string[] str0 = File.ReadAllLines(file.FileName);
                string[] str1 = new string[str0.Length - 2];
                for (int i = 0; i < str0.Length; i++)
                {
                    string[] str2 = str0[i].Split(new char[] { ',' });

                    if (i == 0)
                    {
                        A = str2[0];
                        H = Convert.ToDouble(str2[1]);
                    }
                    else if (i > 0 & str0[i] != "")
                    {
                        str1[i - 2] = str0[i];
                    } 
                }
                for(int i=0;i<str1.Length/3;i++)
                {   string[] str3 = str1[3 * i + 1].Split(new char[] { ',' });
                    string[] str4 = str1[3 * i + 2].Split(new char[] { ',' });
                    ysj.Add(new T { 测段 = str1[3 * i], 往返 = "往",斜距=Convert.ToDouble(str3[2]),垂直角=Convert.ToDouble(str3[3]),仪器高=Convert.ToDouble(str3[4]),目标高=Convert.ToDouble(str3[5]) });
                    ysj.Add(new T { 往返 = "返", 斜距 = Convert.ToDouble(str4[2]), 垂直角 = Convert.ToDouble(str4[3]), 仪器高 = Convert.ToDouble(str4[4]), 目标高 = Convert.ToDouble(str4[5]) });

                }
                dataGridView1.DataSource = ysj;
                
            }
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                JS();
            }
            catch(Exception x)
            {
                MessageBox.Show(x.ToString());
            }
            finally
            {               
                BG();
                HT();
            }
        }

        private void JS()
        {
            double D1 = 0, S1 = 0, a1 = 0, P1 = 0, r1 = 0, f1 = 0, h1 = 0, i1 = 0, v1 = 0;
            double D2 = 0, S2 = 0, a2 = 0, P2 = 0, r2 = 0, f2 = 0, h2 = 0, i2 = 0, v2 = 0;
            
            JDZH zh = new JDZH();
            dataGridView1.DataSource = null;
            
            for (int i=0;i<ysj.Count/2;i++)
            {
                S1 = ysj[2 * i].斜距;                    //提取数据
                S2 = ysj[2 * i + 1].斜距;
                a1 = zh.jzh(ysj[2 * i].垂直角);
                a2 = zh.jzh(ysj[2 * i + 1].垂直角);
                i1 = ysj[2 * i].仪器高;
                i2 = ysj[2 * i + 1].仪器高;
                v1 = ysj[2 * i].目标高;
                v2 = ysj[2 * i + 1].目标高;


                D1 = S1 * Math.Cos(a1);                       //往测计算
                P1 = Math.Pow(D1, 2) / (2 * R);
                r1 = (-1 * k * Math.Pow(D1, 2)) / (2 * R);
                f1 = P1 + r1;               
                h1 = D1 * Math.Tan(a1) + i1 - v1 + f1;
                
                ysj[2 * i].球气差 = (f1).ToString("f4");
                ysj[2 * i].高差 = h1;

                D2 = S2 * Math.Cos(a2);                      //返测计算
                P2 = Math.Pow(D2, 2) / (2 * R);
                r2 = (-1 * k * Math.Pow(D2, 2)) / (2 * R);
                f2 = P2 + r2;               
                h2 = D2 * Math.Tan(a2) + i2 - v2 + f2;
                ysj[2 * i].平距 = D1;
                sum1 += (D1 + D2 ) / 2;
                ysj[2 * i + 1].平距 = D2;
                ysj[2 * i + 1].球气差 = (f2).ToString("f4");
                ysj[2 * i + 1].高差 = h2;

                dr = Math.Abs(h1 - h2);                           //测段超限检查
                if (Math.Abs(dr) > 60 * Math.Sqrt(D1/1000))        //往测检查
                {
                    ysj[2 * i].超限标识 = "F";
                }
                else if (Math.Abs(dr) < 60 * Math.Sqrt(D1/1000))
                {
                    ysj[2 * i].超限标识 = "T";
                }
               /* if (Math.Abs(dr) > 60 * Math.Sqrt(D2/1000))        //返测检查
                {
                    ysj[2 * i + 1].超限标识 = "F";
                }
                else if (Math.Abs(dr) < 60 * Math.Sqrt(D2/1000))
                {
                    ysj[2 * i + 1].超限标识 = "T";
                }*/

                h = (h1 - h2) / 2;                            //高程计算
                ysj[2 * i].高差平均值 = h;
                H0.Add(h);


            }
            double sum = 0;
            for (int i = 0; i < H0.Count; i++)
            {
                sum += H0[i];                              //累计高差
            }
            fh = sum - (H - H);                           //高差闭合差

            gc.Add(new K { 点名 = ysj[0].测段.Substring(0, 1), 高程 = H });
            for (int i = 0; i < ysj.Count / 2; i++)      //计算高程
            {                              
                di = ysj[2 * i].平距;          
                hi = ysj[2 * i].高差平均值;
                v = (-1 * fh * Convert.ToDouble(di)) / sum1;       //高差改正数
                vi.Add(v);
                y0.Add(H);                             
                Hi = H + hi+v;                                                 //改正后高程
                H = Hi;
                
                gc.Add(new K { 点名 = ysj[2 * i].测段.Substring(2, 1), 平距 = di, 高差 = hi, 改正数 = v*1000, 改正后高差 = hi + v, 高程 = Hi, });
            }
            
            double pvv = 0;
            for (int i = 0; i < vi.Count; i++)
            {
                Pi = 1000 / ysj[2*i].平距;
                pvv += Pi * Math.Pow(vi[i], 2);
            }
            u = Math.Sqrt(pvv / 1);
            for (int i = 0; i < ysj.Count/2; i++)
            {              
                jsh.Add(new M { 测段 = ysj[i*2].测段, 往返 = ysj[i*2].往返, 斜距 = ysj[i*2].斜距.ToString("f4"),垂直角=ysj[i*2].垂直角.ToString(),仪器高=ysj[i*2].仪器高.ToString("f4"),目标高=ysj[i*2].目标高.ToString("f4"),球气差=ysj[i*2].球气差,平距=ysj[i*2].平距.ToString("f4"),高差=ysj[i*2].高差.ToString("f4"),高差平均值=ysj[i*2].高差平均值.ToString("f4"),超限标识=ysj[i*2].超限标识 });
                jsh.Add(new M { 测段 = ysj[i * 2+1].测段, 往返 = ysj[i * 2+1].往返, 斜距 = ysj[i * 2+1].斜距.ToString("f4"), 垂直角 = ysj[i * 2+1].垂直角.ToString(), 仪器高 = ysj[i * 2+1].仪器高.ToString("f4"), 目标高 = ysj[i * 2+1].目标高.ToString("f4"), 球气差 = ysj[i * 2+1].球气差, 平距 =  ysj[2 * i + 1].平距.ToString("f4"), 高差 = ysj[i * 2+1].高差.ToString("f4") });
            }
            dataGridView1.DataSource = jsh;
        }
        private void HT()                      
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            //double x0 = 0;
            for (int i = 0; i < ysj.Count/2; i++)
            {
                sum0 +=Convert.ToDouble(ysj[2 * i].平距);
                x.Add(sum0);
                y.Add(y0[i]);
            }
            sum0 += Convert.ToDouble(ysj[0].平距);
            x.Add(sum0);
            y.Add(y0[0]);
            double x1 = x[0], x9 = x[0], y1 = y[0], y9 = y[0];
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] < x1) { x1 = x[i]; }
                if (x[i] > x9) { x9 = x[i]; }
                if (y[i] < y1) { y1 = y[i]; }
                if (y[i] > y9) { y9 = y[i]; }
            }

            //chart1.ChartAreas.Add("x");
            chart1.ChartAreas[0].AxisX.Title = "距离";
            chart1.ChartAreas[0].AxisY.Title = "高程";
            chart1.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Lines; ;
            chart1.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Lines;
            chart1.ChartAreas[0].AxisX.Minimum =Math.Floor(x1 * 0.9);
            chart1.ChartAreas[0].AxisY.Minimum =Math.Floor(y1 * 0.99998*1000)/1000;
            chart1.Series.Add("x");
            chart1.Series.Add("m");
            chart1.Series[0].ChartType= SeriesChartType.Line;
            chart1.Series[1].ChartType = SeriesChartType.Point;
            chart1.Series[0].Points.DataBindXY(x, y);
            chart1.Series[1].Points.DataBindXY(x, y);
            chart1.Annotations.Clear();                           //清除现有标注添加新标注
            string[] str = new string[ysj.Count / 2 + 1];
            for (int i = 0; i < ysj.Count / 2; i++)
            {
                str[i] = ysj[2 * i].测段.Substring(0, 1);
            }
            str[ysj.Count / 2] = ysj[0].测段.Substring(0, 1);
            for (int i = 0; i < str.Length; i++)
            {
                TextAnnotation txt = new TextAnnotation();
                txt.AnchorDataPoint = chart1.Series[1].Points[i];
                txt.Text = str[i];
                chart1.Annotations.Add(txt);
            }
            chart1.Legends.Clear();
            chart1.Titles.Add("三角高程示意图");
            MessageBox.Show("计算已完成");
        }                   //绘制图形
        private void BG()
        {
            richTextBox1.Text = "            三角高程近似平差报告\r\n--------------------统计信息--------------------\r\n";
            richTextBox1.Text += "路线总长度： " + sum1.ToString("f4") + " m\r\n高差闭合差： "+(fh*1000).ToString("f1")+" mm\r\n测段总数： "+ysj.Count/2+"\r\n";          
            richTextBox1.Text += "-------------------测段信息表--------------------\r\n";
            richTextBox1.Text += " 侧段名      往返      斜距        垂直角      仪器高       棱镜高       球气差        平距        高差       高差平均值      超限标识\r\n";
            for (int i = 0; i < jsh.Count; i++)
            {
                string str0 = jsh[i].测段;
                string str1 = jsh[i].斜距;
                string str2 = jsh[i].垂直角;
                string str3 = jsh[i].平距;
                string str4 = jsh[i].高差;
                string str5 = jsh[i].高差平均值;
                
                int k1 = str1.Length;
                int k2 = str2.Length;
                int k3 = str3.Length;
                int k4 = str4.Length;
                while (k1 <= 7)
                {
                    str1 = " " + str1;
                    k1++;
                }
                while (k2 <= 5)
                {
                    str2 = str2 + "0";
                    k2++;
                }
                while (k2 <= 6)
                {
                    str2 = " " + str2;
                    k2++;
                }
                while (k3 <= 7)
                {
                    str3 = " " + str3;
                    k3++;
                }
                while (k4 <= 6)
                {
                    str4 = " " + str4;
                    k4++;
                }
                if (str0 != null)
                {
                    richTextBox1.Text += jsh[i].测段 + "          " + jsh[i].往返 + "      " + str1 + "      " + str2 + "     " + jsh[i].仪器高 + "       " + jsh[i].目标高 + "       " + jsh[i].球气差 + "      " + str3 + "     " + str4 + "     " + str5 + "         " + jsh[i].超限标识 + "\r\n";
                }
                else if (str0 == null)
                { 
                    richTextBox1.Text +=  "             " + jsh[i].往返 + "      " + str1 + "      " + str2 + "     " + jsh[i].仪器高 + "       " + jsh[i].目标高 + "       " + jsh[i].球气差 + "      " + str3 + "     " + str4 + "     " + str5 + "          " + jsh[i].超限标识 + "\r\n";

                }
            }
            richTextBox1.Text += "-------------------高程配赋表--------------------\r\n";
            richTextBox1.Text += " 点名       平距（m）       高差（m）       改正数（mm）       改正后高差（m）       高程（m）  \r\n";
            richTextBox1.Text += "  " + gc[0].点名 + "                                                                                  " + gc[0].高程.ToString("f4") + "\r\n";
            for (int i = 1; i < gc.Count; i++)                        //高程配赋表
            {
                string str1 = gc[i].平距.ToString("f4");
                string str2 = gc[i].高差.ToString("f4");
                string str3 = gc[i].改正后高差.ToString("f4");

                int km = str1.Length;
                int kn = str2.Length;
                int kl = str3.Length;
                while (km <= 7)
                {
                    str1 = " " + str1;
                    km++;
                }
                while (kn <= 6)
                {
                    str2 = " " + str2;
                    kn++;
                }
                while (kl <= 6)
                {
                    str3 = " " + str3;
                    kl++;
                }
                richTextBox1.Text += "            " + str1 + "        " + str2 + "           " + gc[i].改正数.ToString("f1") + "               " + str3 + "\r\n";
                richTextBox1.Text += "  " + gc[i].点名 + "                                                                                  " + gc[i].高程.ToString("f4") + "\r\n";
            }
            richTextBox1.Text+= "-------------------点位计算结果信息表--------------------\r\n";
            richTextBox1.Text += "    点名      距离        高程\r\n";
            //richTextBox1.Text += "    " + gc[0].点名 + "     " + gc[0].平距 + "     " + gc[0].高差+"\r\n";
            for (int i = 0; i < gc.Count; i++)
            {
                string str = gc[i].平距.ToString("f4");
                int k = str.Length;
                while (k <= 7)
                {
                    str = " " + str;
                    k++;
                }
                richTextBox1.Text +="    "+ gc[i].点名 + "      " + str + "      " + gc[i].高程.ToString("f4") + " \r\n";
            }
            richTextBox1.Text += "-------------------精度评定--------------------\r\n单位权中误差:  " + u.ToString("f4")+"\r\n";
            richTextBox1.Text += "各待测点高程中误差：\r\n";
            double sum2 = 0;
            for (int i = 0; i < ysj.Count / 2; i++)
            {
                sum2 += ysj[2 * i].平距;
                Pi = 1000 / sum2 + 1000 / (sum1 - sum2);
                mi = u / Math.Sqrt(Pi);
                if (i + 1 < gc.Count - 1)
                {
                    richTextBox1.Text += gc[i + 1].点名 + "   " + mi.ToString("f4") + "\r\n";
                }
            }

        }
    }
}
