using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 附和水准路线
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class T        //测站数据表
        {
            public string 测站编号 { get; set; }
            public string 后视点名 { get; set; }
            public string 前视点名 { get; set; }
            public double 后距1 { get; set; }
            public double 后距2 { get; set; }
            public double 前距1 { get; set; }
            public double 前距2 { get; set; }
            public double 距离差1 { get; set; }
            public double 距离差2 { get; set; }
            public double 距离差 { get; set; }
            public double Σd { get; set; }
            public double 后视中丝1 { get; set; }
            public double 后视中丝2 { get; set; }
            public double 前视中丝1 { get; set; }
            public double 前视中丝2 { get; set; }
            public double 后视中丝差 { get; set; }
            public double 前视中丝差 { get; set; }
            public double 中丝差 { get; set; }
            public double 高差1 { get; set; }
            public double 高差2 { get; set; }
            public double 高差 { get; set; }
        }
        List<T> yssj = new List<T>();
        List<T> jh = new List<T>();
        string[] str0;   //读取数据
        double qdgc;     //起点高程
        double zdgc;     //终点高程  
        double fh;       //闭合差
        double Sumh;     //累计高差    
        double Sumd;     //总距离
        int t;           //测段数
        int km = 0;
        List<double> Li = new List<double>();  //每测段路线长度
        List<double> vi = new List<double>();  //测段改正数
        List<double> hi = new List<double>();  //高差平差值
        List<double> Hi = new List<double>();  //待定点高程
        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog path = new OpenFileDialog();
            path.Filter = "文本文件(*.txt)|*.txt";
            if(path.ShowDialog()==DialogResult.OK&path.FileName!="")
            {
                 str0 = File.ReadAllLines(path.FileName);
                string[] str2 = str0[0].Split(new char[] { ',' });
                string[] str3 = str0[1].Split(new char[] { ',' });
                qdgc = Convert.ToDouble(str2[1]);
                zdgc = Convert.ToDouble(str3[1]);
                for (int i = 3; i < str0.Length; i++)
                {
                    string[] str1 = str0[i].Split(new char[] { ',' });
                    yssj.Add(new T
                    {
                        测站编号 = (i - 3).ToString(),
                        后视点名 = str1[0],
                        前视点名 = str1[1],
                        后距1 = Convert.ToDouble(str1[2]),
                        后距2 = Convert.ToDouble(str1[8]),
                        前距1 = Convert.ToDouble(str1[4]),
                        前距2 = Convert.ToDouble(str1[6]),
                        后视中丝1 = Convert.ToDouble(str1[3]),
                        后视中丝2 = Convert.ToDouble(str1[9]),
                        前视中丝1 = Convert.ToDouble(str1[5]),
                        前视中丝2 = Convert.ToDouble(str1[7])
                    });
                    jh.Add(new T
                    {
                        测站编号 = (i - 3).ToString(),
                        后视点名 = str1[0],
                        前视点名 = str1[1],
                        后距1 = Convert.ToDouble(str1[2]),
                        后距2 = Convert.ToDouble(str1[8]),
                        前距1 = Convert.ToDouble(str1[4]),
                        前距2 = Convert.ToDouble(str1[6]),
                        后视中丝1 = Convert.ToDouble(str1[3]),
                        后视中丝2 = Convert.ToDouble(str1[9]),
                        前视中丝1 = Convert.ToDouble(str1[5]),
                        前视中丝2 = Convert.ToDouble(str1[7])
                    });
                }
            }            
            dataGridView1.DataSource = yssj;           
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {  
        }
        private void JianHe()
        {
            
            for (int i=0;i<jh.Count;i++)
            {
                yssj[i].前视中丝差 = yssj[i].前视中丝1 - yssj[i].前视中丝2;
                yssj[i].后视中丝差 = yssj[i].后视中丝1 - yssj[i].后视中丝2;
                yssj[i].中丝差 = yssj[i].后视中丝差 - yssj[i].前视中丝差;

                jh[i].前视中丝差 = Math.Round((jh[i].前视中丝1 - jh[i].前视中丝2), 4);
                jh[i].后视中丝差 = Math.Round((jh[i].后视中丝1 - jh[i].后视中丝2), 4);
                jh[i].中丝差 = Math.Round((jh[i].后视中丝差 - jh[i].前视中丝差), 4);

                yssj[i].距离差1 = yssj[i].后距1 - yssj[i].前距1;
                yssj[i].距离差2 = yssj[i].后距2 - yssj[i].前距2;
                yssj[i].距离差 = (yssj[i].距离差1 + yssj[i].距离差2) / 2;
                jh[i].距离差1 =Math.Round(( jh[i].后距1 - jh[i].前距1),4);
                jh[i].距离差2 =Math.Round(( jh[i].后距2 - jh[i].前距2),4);
                jh[i].距离差 =Math.Round(( (jh[i].距离差1 + jh[i].距离差2) / 2),4);


                if (i==0)
                {
                    yssj[i].Σd = yssj[i].距离差;
                    jh[i].Σd =Math.Round( jh[i].距离差,4);
                }
                else if(i>0)
                {
                    yssj[i].Σd = yssj[i].距离差 + yssj[i - 1].Σd;
                    jh[i].Σd = Math.Round((jh[i].距离差 + jh[i - 1].Σd), 4);
                }

                yssj[i].高差1 = yssj[i].后视中丝1 - yssj[i].前视中丝1;
                yssj[i].高差2 = yssj[i].后视中丝2 - yssj[i].前视中丝2;
                yssj[i].高差= (yssj[i].高差1+ yssj[i].高差2)/2;
                jh[i].高差1 = Math.Round((jh[i].后视中丝1 - jh[i].前视中丝1), 4);
                jh[i].高差2 = Math.Round((jh[i].后视中丝2 - jh[i].前视中丝2), 4);
                jh[i].高差 = Math.Round(((jh[i].高差1 + jh[i].高差2) / 2), 4);

                Sumh +=yssj[i].高差;
                Sumd += (yssj[i].后距1 + yssj[i].后距2) / 2 + (yssj[i].前距1 + yssj[i].前距2) / 2;

                double s1 = 0, s2 = 0;
                for (int j = 0; j < Li.Count; j++)
                {
                    s1 += Li[j];  //本段前所有段距离和高差
                    s2 += hi[j];
                }
                if (yssj[i].前视点名 != "-1")
                {
                    Li.Add(Sumd - s1);  //各测段距离
                    hi.Add(Sumh - s2);  //各测段高差
                    t++;
                }
               
            }
        }
        private void PingCha()
        {
            fh = Sumh - (zdgc - qdgc);
            for (int i = 1; i < Li.Count; i++)
            {
                vi.Add((-1) * (fh / Sumd) * Li[i]);
            }
            for (int i = 1; i < hi.Count; i++)
            {
                hi[i] = hi[i] + vi[i-1];
                if(i==1)
                {
                    Hi.Add(qdgc + hi[i]);
                }
                else if(i>1)
                {
                    Hi.Add(Hi[Hi.Count - 1] + hi[i]);
                }
            }

        }
        private void 测站检核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (km == 0)
                {
                    MessageBox.Show("没有数据");
                    return;
                }
                dataGridView1.DataSource = null;
                Li.Add(0);
                hi.Add(0);
                JianHe();
                dataGridView1.DataSource = jh;
                MessageBox.Show("检核完成");
            }
            catch
            {
                MessageBox.Show("数据错误或没有数据");
            }
            finally { }
        }

        private void 平差计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (km == 0)
                {
                    MessageBox.Show("没有数据");
                    return;
                }

                PingCha();
            }
            catch
            {
                MessageBox.Show("还未检核");
            }
            finally { }
        }
    }
}
