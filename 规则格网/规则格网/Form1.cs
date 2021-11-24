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

namespace 规则格网
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class DJ
        {
            public string 点号 { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }
        public class H
        {
            public double h1, h2, h3, h4;
        }
        public class JK
        {
            public int ID;
            public double  e;
        }
        List<DJ> jd = new List<DJ>();  //基准点
        List<DJ> P = new List<DJ>();   //原始散点集 
        List<DJ> P0 = new List<DJ>();  //计算散点集
        List<DJ> Q = new List<DJ>();   //排序后点集
        List<JK> O = new List<JK>();   //排序过程集合
        List<DJ> B = new List<DJ>();   //顺序凸包点集
        List<DJ> R = new List<DJ>();   //原始点凸包点集
        List<H> HH = new List<H>();    //内插高程点高程集合
        List<List<double>> ZZ = new List<List<double>>();    //内插高程点设计高程
        List<H> bg = new List<H>();    //标高
        List<DJ> G = new List<DJ>();
        List<double> PX = new List<double>();
        Stack<DJ> sta1 = new Stack<DJ>();    //凸包点堆栈

        double y0 = 0;
        double x0 = 0;
        double y9 = 0;
        double x9 = 0;
        int k = 0, l = 0, jg = 0, kx = 0, ky = 0,km=0;
        double  r=0;      //内插高程半径
        double sum2 = 0, sumt = 0, sumw = 0;  //总体积
        double h0=0;     //基准高程
        double a, b, c, d;
        private void 打开ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            P = new List<DJ>();
            jd = new List<DJ>();
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "文本文件(*.txt)|*.txt";
            if (file.ShowDialog() == DialogResult.OK & file.FileName != "")
            {
                try
                {
                    string[] str = File.ReadAllLines(file.FileName);
                    for (int i = 0; i < str.Length; i++)
                    {
                        string[] str1 = str[i].Split(new char[] { ',' });
                        if (str1[0].Substring(0, 1) != "P")
                        {
                            jd.Add(new DJ { 点号 = str1[0], X = Convert.ToDouble(str1[1]), Y = Convert.ToDouble(str1[2]), Z = Convert.ToDouble(str1[3]) });
                        }
                        if (str1[0].Substring(0, 1) == "P")
                        {
                            P.Add(new DJ { 点号 = str1[0], X = Convert.ToDouble(str1[1]), Y = Convert.ToDouble(str1[2]), Z = Convert.ToDouble(str1[3]) });
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("文件格式错误");
                    return;
                }
            }
            else
            {
                return;
            }
             y0 = P[0].Y;
             x0 = P[0].X;
             y9 = P[0].Y;
             x9 = P[0].X;

            for (int i = 0; i < P.Count; i++)
            {
                if (P[i].Y < y0)                             //提取y值最小点
                {
                    y0 = P[i].Y;
                    k = i;
                }
                if (P[i].Y > y9)                             //提取y值最大点
                {
                    y9 = P[i].Y;

                }
            }
            for (int i = 0; i < P.Count; i++)
            {
                if (P[i].X < x0)                             //提取x值最小点
                {
                    x0 = P[i].X;
                    l = i;
                }
                if (P[i].X > x9)                             //提取x值最大点
                {
                    x9 = P[i].X;

                }
            }

           /* if (jd.Count != 0)
            {
                double x1 = Convert.ToDouble(jd[0].X);        //平面方程参数
                double y1 = Convert.ToDouble(jd[0].Y);
                double z1 = Convert.ToDouble(jd[0].Z);
                double x2 = Convert.ToDouble(jd[1].X);
                double y2 = Convert.ToDouble(jd[1].Y);
                double z2 = Convert.ToDouble(jd[1].Z);
                double x3 = Convert.ToDouble(jd[2].X);
                double y3 = Convert.ToDouble(jd[2].Y);
                double z3 = Convert.ToDouble(jd[2].Z);
                //double x = Convert.ToDouble(textBox10.Text);
                //double y = Convert.ToDouble(textBox11.Text);

                a = y1 * z2 + y2 * z3 + y3 * z1 - y1 * z3 - y2 * z1 - y3 * z2;
                b = -(x1 * z2 + x2 * z3 + x3 * z1 - x3 * z2 - x2 * z1 - x1 * z3);
                c = x1 * y2 + x2 * y3 + x3 * y1 - x1 * y3 - x2 * y1 - x3 * y2;
                d = -(x1 * y2 * z3 + x2 * y3 * z1 + x3 * y1 * z2 - x1 * y3 * z2 - x2 * y1 * z3 - x3 * y2 * z1);
            } */ 
            dataGridView1.DataSource = P;
          
        }
        private void HT()
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<double> n = new List<double>();
            List<double> m = new List<double>();

            chart1.Series.Clear();
            chart1.Annotations.Clear();
            for (int i = 0; i < Q.Count; i++)            //散点图
            {                
                x.Add(Q[i].X);
                y.Add(Q[i].Y);
            }
            chart1.Series.Add("0");          
            chart1.Series[0].ChartType = SeriesChartType.Point;
            chart1.Series[0].Color = Color.Red;
            chart1.Series[0].Points.DataBindXY(x, y);

            for (int i = 0; i < R.Count; i++)            //凸包
            {
                n.Add(R[i].X);
                m.Add(R[i].Y);
            }
           
            chart1.Series.Add("1");
            chart1.Series[1].ChartType = SeriesChartType.Line;
            chart1.Series[1].Color = Color.Red;
            chart1.Series[1].Points.DataBindXY(n, m);
            chart1.Series.Add("2");
            chart1.Series[2].ChartType = SeriesChartType.Point;
            chart1.Series[2].Color = Color.Red;
            chart1.Series[2].Points.DataBindXY(n, m);


            for (int i = 0; i < Q.Count; i++)                     //标注点名
            {
                TextAnnotation txt = new TextAnnotation();
                txt.AnchorDataPoint = chart1.Series[0].Points[i];
                txt.Text = Q[i].点号;
                chart1.Annotations.Add(txt);
            }

        }        //散点图

        private void 导出报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (km == 0)
            {
                MessageBox.Show("请先计算");
                return;
            }
            else if (km == 1)
            {
                try
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
                catch (Exception x)
                {
                    MessageBox.Show("导出失败" + x.ToString().Substring(0, 10));
                }
                finally { }
            }
        }

        private void 导出dxfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (km == 0)
            {
                MessageBox.Show("请先计算");
                return;
            }
            else if (km == 1)
            {
                try
                {
                    SaveFileDialog file = new SaveFileDialog();
                    file.Filter = "(*.dxf)|*.dxf";
                    if (file.ShowDialog() == DialogResult.OK && file.FileName.Length > 0)
                    {
                        StreamWriter sw = new StreamWriter(file.FileName, false);
                        dxf dxf = new dxf();
                        sw.Write("0\r\nSECTION\r\n2\r\nENTITIES\r\n");
                        string[] str1 = dxf.point(Q);                         //导出散点
                        for (int i = 0; i < str1.Length; i++)
                        {
                            sw.Write(str1[i]);
                        }
                        string[] str2 = dxf.pl(R);                          //导出凸包
                        for (int i = 0; i < str2.Length; i++)
                        {
                            sw.Write(str2[i]);
                        }
                        string[] str3 = dxf.dl(G);                         //导出格网
                        for (int i = 0; i < str3.Length; i++)
                        {
                            sw.Write(str3[i]);
                        }
                        string[] str4 = dxf.txt(Q);                         //导出格网
                        for (int i = 0; i < str4.Length; i++)
                        {
                            sw.Write(str4[i]);
                        }
                        sw.Write("0\r\nENDSEC\r\n0\r\nEOF\r\n");
                        sw.Close();
                        MessageBox.Show("导出成功");

                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show("导出失败" + x.ToString().Substring(0, 10));
                }
                finally { }

            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
          /*  int kw = dataGridView1.CurrentCell.RowIndex;
            int jw = dataGridView1.CurrentCell.ColumnIndex;
          if(jw==0)
            {
                P[kw].点号 = dataGridView1.CurrentCell.Value.ToString();
            }
          else if(jw==1)
            {
                P[kw].X =Convert.ToDouble(dataGridView1.CurrentCell.Value);
            }
            else if (jw == 2)
            {
                P[kw].Y = Convert.ToDouble(dataGridView1.CurrentCell.Value);
            }
            else if (jw == 3)
            {
                P[kw].Z = Convert.ToDouble(dataGridView1.CurrentCell.Value);
            }*/
        }

        private void TB()
        {

            Q = new List<DJ>();   //排序后点集
            O = new List<JK>();   //排序过程集合
            B = new List<DJ>();   //顺序凸包点集
            R = new List<DJ>();   //原始点凸包点集
            HH = new List<H>();    //内插高程点高程集合
            PX = new List<double>();
            sta1 = new Stack<DJ>();    //凸包点堆栈
          //  chart1 = new Chart();
          //  chart1.ChartAreas.Add("0");

            sum2 = 0;  //总体积          
            chart1.ChartAreas[0].AxisX.Minimum = Math.Floor(x0 * 0.999);
            chart1.ChartAreas[0].AxisY.Minimum = Math.Floor(y0 * 0.999);
            r = (((x9 - x0) + (y0 - y0)) / 2) * 0.4;
            h0 = Convert.ToDouble(toolStripTextBox1.Text);
            double m;            
            for (int i = 0; i < P.Count - 1; i++)              //若果有多个最小y值，则提取其中x最小值的点继续执行
            {
               if(i!=k&P[k].Y==P[i].Y)
                {
                    if (P[i].X < P[k].X)
                    {
                        k = i;
                    }
                }
            }
            for (int i = 0; i < P.Count; i++)           //计算p0与所有点与x轴夹角并排序
            {
            t1:
                if (i != k)
                {                
                    double e = Math.Atan((P[i].Y - P[k].Y) / (P[i].X - P[k].X));
                    while (e < 0)                      //角度为负数时加π
                    {
                        e = e + Math.PI;
                    }
                    PX.Add(e);                      
                    for(int j1=0;j1<O.Count;j1++)      //解决p0与其余点多点一线问题
                    {
                        if (e == O[j1].e)    
                        {
                            double d1 = Math.Sqrt(Math.Pow((P[i].X - P[k].X), 2) + Math.Pow((P[i].Y - P[k].Y), 2));
                            double d2 = Math.Sqrt(Math.Pow((P[j1].X - P[k].X), 2) + Math.Pow((P[j1].Y - P[k].Y), 2));
                            if (d1 > d2)                  //保留距离p0点距离最远的点
                            {
                                O[j1].ID = i;
                                O[j1].e = e;                             
                            }
                            i++;
                            goto t1;

                        }
                    }
                    O.Add(new JK { ID = i, e = e });              
                }
            }


            JK t;
            int j;
            for (int i = 0; i < O.Count - 1; i++)        //冒泡排序
            {
                j = i + 1;

            id:
                if (O[i].e > O[j].e)
                {
                    t = O[i];
                    O[i] = O[j];
                    O[j] = t;
                    goto id;
                }
                else 
                   if (j < O.Count - 1)
                    {
                    j++;
                    goto id;
                    }

            }

                Q.Add(new DJ { 点号 = P[k].点号, X = P[k].X, Y = P[k].Y, Z = P[k].Z });           //排序后新点集
            for (int i = 0; i < O.Count; i++)
                {
                    Q.Add(new DJ { 点号 = P[O[i].ID].点号, X = P[O[i].ID].X, Y = P[O[i].ID].Y, Z = P[O[i].ID].Z });
                }
            //生成凸包
            sta1.Push(Q[0]);                         //p0,p1,p2进栈
            sta1.Push(Q[1]);
            sta1.Push(Q[2]);
            for (int i = 3; i < Q.Count; i++)
            {
                to:
                DJ o = new DJ();
                double x1, x2, x3, y1, y2, y3;
                x2 = sta1.Peek().X;
                y2 = sta1.Peek().Y;
                o = sta1.Pop();
                x1 = sta1.Peek().X;
                y1 = sta1.Peek().Y;
                sta1.Push(o);
                x3 = Q[i].X;
                y3 = Q[i].Y;

                m = (x1 - x2) * (y3 - y2) - (y1 - y2) * (x3 - x2);     //求<p2-p1>,<p2-p3>,叉积
                if (m > 0)
                {
                    sta1.Pop();
                    //i++;
                    goto to;                 
                }
                else if(m<0)
                {
                    sta1.Push(Q[i]);
                }
            }
            R.Add(Q[0]);

            for (int i = 0; i < sta1.Count;)
            {
                DJ r1 = sta1.Pop();
                R.Add(r1);
            }
                       
        }        //凸包
        private void GW()
        { ///生成格网
          ///     
           jg =Convert.ToInt32(toolStripComboBox1.Text.Substring(0,2));   //格网间隔   
           ky = Convert.ToInt32(Math.Floor((y9 - y0) / jg) + 1);
           kx = Convert.ToInt32(Math.Floor((x9 - x0) / jg) + 1);
            //生成最小外包矩形

            List<double> x = new List<double>();
            List<double> y = new List<double>();
            x.Add(x0);
            x.Add(x0);
            x.Add(x9);
            x.Add(x9);
            x.Add(x0);

            y.Add(y9);
            y.Add(y0);
            y.Add(y0);
            y.Add(y9);
            y.Add(y9);
            chart1.Series.Add("3");
            chart1.Series[3].ChartType = SeriesChartType.Line;
            chart1.Series[3].Color = Color.Red;
            chart1.Series[3].Points.DataBindXY(x, y);
            chart1.Series[3].Enabled = false;
            //生成格网
            List<List<double>> lx = new List<List<double>>();
            List<List<double>> ly = new List<List<double>>();
            List<List<double>> hx = new List<List<double>>();
            List<List<double>> hy = new List<List<double>>();
            
            //竖直格网线
            double y10 = ky * jg + y0;
            for (int i=0;i<Math.Floor((x9-x0)/jg)+2;i++)
            {
                double h = i * jg;
                List<double> x1 = new List<double>();
                List<double> y1 = new List<double>();
                x1.Add(x0 + h);
                x1.Add(x0 + h);
                y1.Add(y0);
                y1.Add(y10);
                lx.Add(x1);
                ly.Add(y1);
                chart1.Series.Add((i + 4).ToString());
                chart1.Series[i + 4].ChartType = SeriesChartType.Line;
                chart1.Series[i + 4].Color = Color.Black;
                chart1.Series[i + 4].Points.DataBindXY(lx[i], ly[i]);
            }
            for (int i = 0; i < lx.Count; i++)
            {
                for (int j = 0; j < lx[i].Count; j++)
                {
                    G.Add(new DJ { X = lx[i][j], Y = ly[i][j] });
                }
            }
            //水平格网线
            double x10 = kx * jg + x0;
            for(int i=0;i<(y9-y0)/jg+1;i++)
            {
                double l = i * jg;
                List<double> x1 = new List<double>();
                List<double> y1 = new List<double>();
                x1.Add(x0);
                x1.Add(x10);
                y1.Add(y0 + l);
                y1.Add(y0 + l);
                hx.Add(x1);
                hy.Add(y1);
                chart1.Series.Add((i + kx + 5).ToString());
                chart1.Series[i + kx + 5].ChartType = SeriesChartType.Line;
                chart1.Series[i + kx + 5].Color = Color.Black;
                chart1.Series[i + kx + 5].Points.DataBindXY(hx[i], hy[i]);
            }
            for (int i = 0; i < hx.Count; i++)
            {
                for (int j = 0; j < hx[i].Count; j++)
                {
                    G.Add(new DJ { X = hx[i][j], Y = hy[i][j] });
                }
            }
        }        //格网
        private void ZS()           //方格中心位置判断
        {
            List<double> zx = new List<double>();    //所有格网中心点
            List<double> zy = new List<double>();
            List<double> zhx = new List<double>();  //在凸包内的格网中心点
            List<double> zhy = new List<double>();
            ZZ = new List<List<double>>();
            double jg2 = Convert.ToDouble(jg) / 2;
            for (int i = 0; i < ky; i++)             //计算方格网中心点坐标
            {
                for (int j = 0; j < kx; j++)
                {
                    if (j == 0)
                    {
                        zx.Add(x0 + jg2);
                        zy.Add(y0 + jg2 + i * jg);
                    }
                    else if (j > 0)
                    {
                        zx.Add(x0 + jg2 + j * jg);
                        zy.Add(y0 + jg2 + i * jg);
                    }
                }
            }
            for (int i = 0; i < zx.Count; i++)     //方格网中心位置判断
            {

                double cot = 0, xi;
                double x = zx[i];
                double y = zy[i];

                for (int j = 0; j < R.Count - 1; j++)
                {
                    double x1 = R[j].X;           //取两相邻凸包点
                    double y1 = R[j].Y;
                    double x2 = R[j + 1].X;
                    double y2 = R[j + 1].Y;

                    if ((y > y1 & y < y2) | (y < y1 & y > y2))       //判断中心点y值是否在两凸包点y值之间
                    {
                        xi = ((x2 - x1) / (y2 - y1)) * (y - y1) + x1;  //判断中心点x值与两凸包点连线位置
                        if (xi > x)                                    //中心点在左侧时 +1
                        {
                            cot++;
                        }
                    }
                }
                if (cot % 2 == 1)                    //累计单边交点单数时 中心点在凸包内
                {
                    double hx1, hx2, hx3, hx4;       //计算方格网定点内插高程
                    double hy1, hy2, hy3, hy4;
                    List<double> hx0 = new List<double>();
                    List<double> hy0 = new List<double>();
                    hx1 = x - jg2;  //左下
                    hy1 = y - jg2;
                    hx2 = x + jg2;  //右下
                    hy2 = y - jg2;
                    hx3 = x + jg2;  //右上
                    hy3 = y + jg2;
                    hx4 = x - jg2;  //左上
                    hy4 = y + jg2;

                    hx0.Add(hx1);
                    hx0.Add(hx2);
                    hx0.Add(hx3);
                    hx0.Add(hx4);

                    hy0.Add(hy1);
                    hy0.Add(hy2);
                    hy0.Add(hy3);
                    hy0.Add(hy4);


                    double di = 0;
                    double sum0 = 0, sum1 = 0;
                    List<double> zz = new List<double>();         //标高
                    for (int j = 0; j < hx0.Count; j++)           //计算顶点到离散点距离
                    {                                             //取小于r的离散点计算高程
                        double xh = hx0[j];
                        double yh = hy0[j];
                        double zh = 0;


                        for (int k = 0; k < P.Count; k++)         //计算角点到离散点距离
                        {
                            di = Math.Sqrt(Math.Pow((xh - P[k].X), 2) + Math.Pow((yh - P[k].Y), 2));
                            if (di < r)                           //
                            {
                                sum0 += (P[k].Z / di);
                                sum1 += 1 / di;
                            }

                        }
                        zh = ((-1) * d - a * xh - b * yh) / c;    //计算设计高程
                        double h = sum0 / sum1;                   //实际高程
                        if (j == 0)
                        {
                            HH.Add(new H { h1 = h });
                            zz.Add(zh - h);                       //标高
                        }
                        else if (j == 1)
                        {
                            HH[HH.Count - 1].h2 = h;
                            zz.Add(zh - h);
                        }
                        else if (j == 2)
                        {
                            HH[HH.Count - 1].h3 = h;
                            zz.Add(zh - h);
                        }
                        else if (j == 3)
                        {
                            HH[HH.Count - 1].h4 = h;
                            zz.Add(zh - h);
                        }
                    }

                    zhx.Add(x);        //中心点加入集合
                    zhy.Add(y);
                    ZZ.Add(zz);        //标高加入集合              
                }
            }
            chart1.Series.Add((kx + ky + 6).ToString());
            chart1.Series[kx + ky + 6].ChartType = SeriesChartType.Point;
            chart1.Series[kx + ky + 6].Color = Color.Blue;
            chart1.Series[kx + ky + 6].Points.DataBindXY(zhx, zhy);
            
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {              
                TB();
                HT();
                GW();
                ZS();
                if(jd.Count!=0)
                {
                    XM();
                }
                else  if(jd.Count==0)
                {
                    PM();
                }
                BG();
                km = 1;
            }
            catch(Exception x)
            {
                MessageBox.Show(x.ToString());
                return;
            } 
            finally
            {
                tabControl1.SelectTab(1); 
               
                //MessageBox.Show("1"); 
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void PM()      //平面体积计算
        {
            
            for (int i = 0; i < HH.Count; i++)
            {
                double V = ((HH[i].h1 + HH[i].h2 + HH[i].h3 + HH[i].h4) / 4 - h0) * Math.Pow(jg, 2);
                sum2 += V;
            }
        }
        private void XM()              
        {
            double Vt = 0;
            double Vw = 0;
            sumt = 0;sumw = 0;
            int k = 0;
            for (int i = 0; i < ZZ.Count; i++)
            {
                Vt = 0;
                Vw = 0;
                k = 0;
                for (int j = 0; j < ZZ[i].Count; j++)
                {
                    if (ZZ[i][j] > 0)
                    {
                        k++;
                    }
                    else if (ZZ[i][j] < 0)
                    {
                        k--;
                    }
                }
                if (Math.Abs(k) == 4)   //全填方或全挖方
                {
                    double ha = ZZ[i][0];
                    double hb = ZZ[i][1];
                    double hc = ZZ[i][2];
                    double hd = ZZ[i][3];
                    if (k > 0)
                    {
                        Vt = Math.Pow(jg, 2) * ((ha + hb + hc + hd) / 4);
                    }
                    else if (k < 0)
                    {
                        Vw = Math.Pow(jg, 2) * ((ha + hb + hc + hd) / 4);
                    }
                }
                else if (k == 2)          //一点挖方  
                {
                    double ha = 0;
                    double hb = 0;
                    double hc = 0;
                    double hd = 0;
                    int j0 = 0;
                    for (int j = 0; j < ZZ[i].Count; j++)
                    {
                        if (ZZ[i][j] < 0)
                        {
                            ha = Math.Abs(ZZ[i][j]);
                            j0 = j;
                            // ZZ.Remove(ZZ[j]);
                        }
                    }
                    if (j0 == 0)
                    {
                        hb = Math.Abs(ZZ[i][1]);
                        hc = Math.Abs(ZZ[i][2]);
                        hd = Math.Abs(ZZ[i][3]);
                    }
                    else if (j0 == 1)
                    {
                        hb = Math.Abs(ZZ[i][2]);
                        hc = Math.Abs(ZZ[i][3]);
                        hd = Math.Abs(ZZ[i][0]);
                    }
                    else if (j0 == 2)
                    {
                        hb = Math.Abs(ZZ[i][3]);
                        hc = Math.Abs(ZZ[i][0]);
                        hd = Math.Abs(ZZ[i][1]);
                    }
                    else if (j0 == 3)
                    {
                        hb = Math.Abs(ZZ[i][0]);
                        hc = Math.Abs(ZZ[i][1]);
                        hd = Math.Abs(ZZ[i][2]);
                    }
                    Vt = ((2 * hd + 2 * hb + hc - ha) / 6) * Math.Pow(jg, 2);
                    Vw = (Math.Pow(ha, 3) / (6 * (ha + hd) * (ha + hb))) * Math.Pow(jg, 2);
                   }
                else if (k == -2)                    //一点填方
                {
                    double ha = 0;
                    double hb = 0;
                    double hc = 0;
                    double hd = 0;
                    int j0 = 0;
                    for (int j = 0; j < ZZ[i].Count; j++)
                    {
                        if (ZZ[i][j] > 0)
                        {
                            ha = Math.Abs(ZZ[i][j]);
                            j0 = j;
                            //  ZZ.Remove(ZZ[j]);
                        }
                    }
                    if (j0 == 0)
                    {
                        hb = Math.Abs(ZZ[i][1]);
                        hc = Math.Abs(ZZ[i][2]);
                        hd = Math.Abs(ZZ[i][3]);
                    }
                    else if (j0 == 1)
                    {
                        hb = Math.Abs(ZZ[i][2]);
                        hc = Math.Abs(ZZ[i][3]);
                        hd = Math.Abs(ZZ[i][0]);
                    }
                    else if (j0 == 2)
                    {
                        hb = Math.Abs(ZZ[i][3]);
                        hc = Math.Abs(ZZ[i][0]);
                        hd = Math.Abs(ZZ[i][1]);
                    }
                    else if (j0 == 3)
                    {
                        hb = Math.Abs(ZZ[i][0]);
                        hc = Math.Abs(ZZ[i][1]);
                        hd = Math.Abs(ZZ[i][2]);
                    }
                    Vt = (Math.Pow(ha, 3) / (6 * (ha + hd) * (ha + hb))) * Math.Pow(jg, 2);
                    Vw = ((2 * hd + 2 * hb + hc - ha) / 6) * Math.Pow(jg, 2);
                }
                else if (Math.Abs(k) == 0)  //两点填方或挖方
                {
                    double ha = ZZ[i][0];
                    double hb = ZZ[i][1];
                    double hc = ZZ[i][2];
                    double hd = ZZ[i][3];
                    if (Math.Sign(ha * hb) != Math.Sign(ha * hd))   //相邻
                    {
                        if (ha < 0 & hd < 0)
                        {
                            ha = Math.Abs(ZZ[i][0]);
                            hb = Math.Abs(ZZ[i][1]);
                            hc = Math.Abs(ZZ[i][2]);
                            hd = Math.Abs(ZZ[i][3]);
                            Vt = Math.Pow((hb + hc), 2) / (4 * (ha + hb + hc + hd)) * Math.Pow(jg, 2);
                            Vw = Math.Pow((ha + hd), 2) / (4 * (ha + hb + hc + hd)) * Math.Pow(jg, 2);
                        }
                        else if (ha > 0 & hd > 0)
                        {
                            ha = Math.Abs(ZZ[i][0]);
                            hb = Math.Abs(ZZ[i][1]);
                            hc = Math.Abs(ZZ[i][2]);
                            hd = Math.Abs(ZZ[i][3]);
                            Vt = Math.Pow((ha + hd), 2) / (4 * (ha + hb + hc + hd)) * Math.Pow(jg, 2);
                            Vw = Math.Pow((hb + hc), 2) / (4 * (ha + hb + hc + hd)) * Math.Pow(jg, 2);
                        }
                        else if (ha > 0 & hb > 0)
                        {
                            ha = Math.Abs(ZZ[i][0]);
                            hb = Math.Abs(ZZ[i][1]);
                            hc = Math.Abs(ZZ[i][2]);
                            hd = Math.Abs(ZZ[i][3]);
                            Vt = Math.Pow((ha + hb), 2) / (4 * (ha + hb + hc + hd)) * Math.Pow(jg, 2);
                            Vw = Math.Pow((hc + hd), 2) / (4 * (ha + hb + hc + hd)) * Math.Pow(jg, 2);
                        }
                        else if (ha < 0 & hb < 0)
                        {
                            ha = Math.Abs(ZZ[i][0]);
                            hb = Math.Abs(ZZ[i][1]);
                            hc = Math.Abs(ZZ[i][2]);
                            hd = Math.Abs(ZZ[i][3]);
                            Vt = Math.Pow((hc + hd), 2) / (4 * (ha + hb + hc + hd)) * Math.Pow(jg, 2);
                            Vw = Math.Pow((ha + hb), 2) / (4 * (ha + hb + hc + hd)) * Math.Pow(jg, 2);
                        }
                    }
                    if (Math.Sign(ha * hb) == Math.Sign(ha * hd))   //对角
                    {

                        if (ha < 0 & hd > 0)
                        {
                            ha = Math.Abs(ZZ[i][0]);
                            hb = Math.Abs(ZZ[i][1]);
                            hc = Math.Abs(ZZ[i][2]);
                            hd = Math.Abs(ZZ[i][3]);
                            Vt = (2 * hd + 2 * hb - hc - ha) / 6 * Math.Pow(jg, 2);
                            Vw = ((Math.Pow(hc, 3) / ((hc + hd) * (hc + hb))) + (Math.Pow(ha, 3) / ((ha + hd) * (ha + hb)))) * (Math.Pow(jg, 2) / 6);
                        }
                        else if (hb < 0 & hc > 0)
                        {
                            ha = Math.Abs(ZZ[i][0]);
                            hb = Math.Abs(ZZ[i][1]);
                            hc = Math.Abs(ZZ[i][2]);
                            hd = Math.Abs(ZZ[i][3]);
                            Vt = (2 * ha + 2 * hc - hd - hb) / 6 * Math.Pow(jg, 2);
                            Vw = ((Math.Pow(hd, 3) / ((hd + ha) * (hd + hc))) + (Math.Pow(hb, 3) / ((hb + ha) * (hb + hc)))) * (Math.Pow(jg, 2) / 6);
                        }

                    }
                }

                sumt += Vt;
                sumw += Vw;
            }
        }            //
        private void BG()
        {
            richTextBox1.Text = "            格网法体积计算\r\n";
            richTextBox1.Text += "----------基本信息----------\r\n";          
            richTextBox1.Text += "单位格网边长： " + jg.ToString() + "\r\n";
            richTextBox1.Text +="格网纵向个数： "+ ky.ToString() + "\r\n";
            richTextBox1.Text +="格网横向个数： "+ kx.ToString() + "\r\n";
            richTextBox1.Text +="单位格网总数： "+ (kx * ky).ToString() + "\r\n";
            if (jd.Count == 0)
            {
                richTextBox1.Text += "基准高程： " + h0.ToString() + "\r\n";
                richTextBox1.Text += "总体积： " + sum2.ToString("f4") + "\r\n";              
            }
            else if(jd.Count!=0)
            {
                richTextBox1.Text += "总填方： " + sumt.ToString("f4") + "\r\n";
                richTextBox1.Text += "总挖方： " + sumw.ToString("f4") + "\r\n";
            }
            richTextBox1.Text += "----------凸包点信息----------\r\n";
            richTextBox1.Text += "  点名        X坐标          Y坐标          H高程\r\n";
            for (int i=0;i<R.Count-1; i++)
            {
                string str1 = R[i].点号;
                while(str1.Length<4)
                {
                    str1 = str1 + " ";
                }
                richTextBox1.Text += "  " + str1 + "       " + R[i].X + "       " + R[i].Y + "       " + R[i].Z + "\r\n";
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.Location = new Point(0, 50);
            tabControl1.Width = toolStrip1.Width;
            toolStripComboBox1.Items.Add("1 m");
            toolStripComboBox1.Items.Add("5 m");
            toolStripComboBox1.Items.Add("10 m");
            toolStripComboBox1.Text = "1 m";
        }
        private void chart1_Click(object sender, EventArgs e)
        {
           
        }

    }
}
