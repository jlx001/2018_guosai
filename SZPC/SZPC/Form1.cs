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
using System.Collections;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Reflection;


namespace SZPC
{
    public partial class Form1 : Form
    {
        ArrayList value1  ;      // 点号
        ArrayList value2;
        ArrayList value3;   // 高差
        ArrayList value4;
        public Form1(ArrayList value1,ArrayList value2,ArrayList value3,ArrayList value4)
        {
            InitializeComponent();
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
            this.value4 = value4;   
            if (value2 != null && value1 != null && value3 != null && value4 != null)
            {
                for (int i = 0; i < value1.Count; i++)
                {
                    richTextBox1.Text += value1[i].ToString() + "\n";
                }
                for (int i = 1; i < value2.Count; i++)
                {
                    string a=(Convert.ToDouble(value2[i])-Convert.ToDouble(value2[i-1])).ToString("f3");
                    richTextBox3.Text += a + "\n";
                }
                for (int i = 1; i < value4.Count; i++)
                {                  
                    richTextBox2.Text += (Convert.ToDouble(value4[i])-Convert.ToDouble(value4[i-1])).ToString("f") + "\n";
              }
            }
        }

        public Form1()
        {
            // TODO: Complete member initialization
            InitializeComponent();
        }
        //定义全局变量
        int i = 0, km = 0, gs = 0;           //过程变量
        double f = 0;                        //限差
        double fl = 0;                        //闭合差
        double yzd1 = 0, yzd2 = 0;            //起始点高程，终点高程
        double sum0 = 0, sum1 = 0, sum2 = 0; //累计测站数或距离，累计高差，累计改正数
        ArrayList dh = new ArrayList();      //点号
        ArrayList cz = new ArrayList();      //测站数或距离
        ArrayList gc = new ArrayList();      //高差
        ArrayList gzs = new ArrayList();     //               改正数
        ArrayList gzh = new ArrayList();    //                改正后高差
        ArrayList gzhgc = new ArrayList();   //               改正后高程

        ///清除数据，初始化变量
        /// 清除数据，初始化变量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            rb1.Checked = true;
            rb3.Checked = true;
            richTextBox5.Text = "";
            richTextBox6.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";
            comboBox1.Text = "选择路线等级";
            label6.Text = "";
            label14.Text = "";
            label15.Text = "";
            label16.Text = "";
            i = 0;
            km = 0;
            gs = 0;
            f = 0;
            fl = 0;
            yzd1 = 0;
            yzd2 = 0;
            sum0 = 0;
            sum1 = 0;
            sum2 = 0;
            ArrayList dh = new ArrayList();
            ArrayList cz = new ArrayList();
            ArrayList gc = new ArrayList();
            ArrayList gzs = new ArrayList();
            ArrayList gzh = new ArrayList();
            ArrayList gzhgc = new ArrayList();
        }
        /// 判断并计算个等级限差
        /// 判断并计算个等级限差
        /// </summary>
        private void PDDJ()
        {
            if (comboBox1.Text == "一等")
            {
                f = 2 * Math.Sqrt(sum0);
                gs = 2;
            }
            else if (comboBox1.Text == "二等")
            {
                f = 4 * Math.Sqrt(sum0);
                gs = 4;
            }
            else if (comboBox1.Text == "三等")
            {
                if (rb3.Checked == true)
                {
                    f = 4 * Math.Sqrt(sum0);
                    gs = 4;
                }
                else if (rb4.Checked == true)
                {
                    f = 12 * Math.Sqrt(sum0);
                    gs = 12;
                }
            }
            else if (comboBox1.Text == "四等")
            {
                if (rb3.Checked == true)
                {
                    f = 6 * Math.Sqrt(sum0);
                    gs = 6;
                }
                else if (rb4.Checked == true)
                {
                    f = 20 * Math.Sqrt(sum0);
                    gs = 20;
                }
            }
            else if (comboBox1.Text == "等外")
            {
                if (rb3.Checked == true)
                {
                    f = 12 * Math.Sqrt(sum0);
                    gs = 12;
                }
                else if (rb4.Checked == true)
                {
                    f = 40 * Math.Sqrt(sum0);
                    gs = 40;
                }
            }
        }
        /// 判断数据输入过程
        /// 判断数据输入过程
        /// </summary>
        private void RB()
        {
            if (textBox12.Text == null | textBox12.Text == "")
            {
                MessageBox.Show("请输入起始高程");
                km = 1;
            }
            else if (rb1.Checked & (textBox13.Text == null | textBox13.Text == ""))
            {
                MessageBox.Show("请输入终点高程");
                km = 1;
            }
            else if (comboBox1.Text == "选择路线等级")
            {
                MessageBox.Show("请选择路线等级");
                km = 1;
            }
            else if (rb1.Checked & richTextBox1.Text == null | richTextBox1.Text == "")
            {
                MessageBox.Show("请输入点号");
                km = 1;
            }
            else if (rb1.Checked & richTextBox2.Text == null | richTextBox2.Text == "")
            {
                if (rb3.Checked == true)
                {
                    MessageBox.Show("请输入测站数");
                }
                else if (rb4.Checked == true)
                {
                    MessageBox.Show("请输入距离");
                }
                km = 1;
            }
            else if (rb1.Checked & richTextBox3.Text == null | richTextBox3.Text == "")
            {
                MessageBox.Show("请输入高差");
                km = 1;
            }
            else
            {
                km = 0;
            }

        }

        /// 导入数据
        /// </summary>
        private void DR()
        {
            ArrayList dh = new ArrayList();
            ArrayList cz = new ArrayList();
            ArrayList gc = new ArrayList();
            OpenFileDialog txt_r = new OpenFileDialog();
            txt_r.InitialDirectory = "C:\\Users\\Administrator\\Desktop\\";
            txt_r.Filter = "文本文件(*.txt)|*.txt";
            txt_r.RestoreDirectory = true;
            txt_r.FilterIndex = 1;

            if (txt_r.ShowDialog() == DialogResult.OK)
            {
                string file = txt_r.FileName;                                   //得到选择的文件的完整路径
                string[] str1 = File.ReadAllLines(file);

                for (i = 0; i < str1.Length; i++)
                {
                    string[] str2 = str1[i].Split(new char[] { ',' });                //按“，”分隔字符串
                    int st = str1[i].IndexOf(",");                                    //获得本行“，”位置
                    if (str1[i].IndexOf(",") == -1 & str1[i] != null & str1[i] != "")//判断本行无“，”且非空
                    {
                        dh.Add(str2[0]);                                             //读入最后一点点号
                    }

                    else if (str1[i] != null & str1[i] != "")                        //判断本行非空
                    {
                        dh.Add(str2[0]);                                             //读入第一点至倒数第二点点号
                        cz.Add(str2[1]);                                            //读入测站数或距离
                        gc.Add(str2[2]);                                           //读入高差
                    }

                }
                for (i = 0; i < dh.Count; i++)
                {
                    richTextBox1.Text += dh[i] + "\r\n";                           //在textbox控件中显示数据
                }
                for (i = 0; i < cz.Count; i++)
                {
                    richTextBox2.Text += cz[i] + "\r\n";
                    richTextBox3.Text += gc[i] + "\r\n";
                }
            }
          
        }
        /// 判断单选按钮位置
        /// 判断单选按钮位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            label13.Visible = true;
            textBox13.Visible = true;
        }
        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            label13.Visible = false;
            textBox13.Visible = false;
        }
        private void rb3_CheckedChanged(object sender, EventArgs e)
        {
            label7.Text = "测站数";
            
            if (value4 != null)
            {
                richTextBox2.Text = "";
                for (int i = 1; i < value4.Count; i++)
                {
                    richTextBox2.Text += (Convert.ToDouble(value4[i]) - Convert.ToDouble(value4[i - 1])).ToString("f0") + "\n";
                }
            }
        }
        private void rb4_CheckedChanged(object sender, EventArgs e)
        {
            label7.Text = "距离（km）";
           
            if (value3 != null)
            {
                richTextBox2.Text = "";
                for (int i = 1; i < value3.Count; i++)
                {
                    richTextBox2.Text += ((Convert.ToDouble(value3[i]) - Convert.ToDouble(value3[i - 1])) / 1000).ToString("f") + "\n";
                }
            }
        }
        /// 计算过程    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            RB();                      //判断数据输入是否完整
            if (km == 1|km==3)         
            {
                return;
            }
            else if (km == 0)          
            { 
                //变量初始化
                km = 2;
                f = 0;
                fl = 0;
                yzd1 = 0;
                yzd2 = 0;
                sum0 = 0;
                sum1 = 0;
                sum2 = 0;
                richTextBox5.Text = "";
                richTextBox6.Text = "";               
                richTextBox4.Text = "";
                //初始化动态数组
                ArrayList dh = new ArrayList();
                ArrayList cz = new ArrayList();
                ArrayList gc = new ArrayList();
                ArrayList gzs = new ArrayList();
                ArrayList gzh = new ArrayList();
                ArrayList gzhgc = new ArrayList();
                //读取数据
                string[] dh0 = richTextBox1.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] cz0 = richTextBox2.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] gc0 = richTextBox3.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (rb1.Checked == true)                     //若选择附合
                {
                    yzd1 = Convert.ToDouble(textBox12.Text);//读取起点高程
                    yzd2 = Convert.ToDouble(textBox13.Text);//读取终点高程
                }
                else if (rb2.Checked == true)              //若选择闭合
                {
                    yzd1 = Convert.ToDouble(textBox12.Text);//读取起点高程
                    yzd2 = Convert.ToDouble(textBox12.Text);//读取起点高程
                }

                for (i = 0; i < dh0.Length; i++)
                {
                    dh.Add(dh0[i]);                        //读取点号

                }
                for (i = 0; i < cz0.Length; i++)
                { 
                    cz.Add(cz0[i]);                      //读取测站数或距离
                    gc.Add(gc0[i]);                      //读取高差   
                }
                for (i = 0; i < cz.Count; i++)
                {
                    double x1 = 0, x2 = 0;
                    x1 = Convert.ToDouble(cz[i]);
                    x2 = Convert.ToDouble(gc[i]);
                    sum0 = sum0 + x1;                   //计算累计测站数或距离
                    sum1 = sum1 + x2;                   //计算累计高差
                }

                fl = yzd1 + sum1 - yzd2;
                gzhgc.Add(yzd1);
                PDDJ();
                if (Math.Abs(fl * 1000) > Math.Abs(f))        //判断闭合差是否超限，若超限停止计算并弹出提示
                {
                    MessageBox.Show("闭合差超限");
                    return;
                }
                else
                {
                    for (i = 0; i < gc.Count; i++)
                    {
                        gzs.Add(-fl * Convert.ToDouble(cz[i]) / sum0);               //计算改正数
                    }
                    for (i = 0; i < gzs.Count; i++)
                    {
                        gzh.Add(Convert.ToDouble(gzs[i]) + Convert.ToDouble(gc[i]));//计算改正后高差
                        gzh[i] = Math.Round(Convert.ToDouble(gzh[i]), 3);
                    }
                }
                for (i = 0; i < gzh.Count; i++)                          //计算改正后各点高程
                {
                    double x1 = Convert.ToDouble(gzh[i]);
                    sum2 = sum2 + x1;
                    double x2=Math.Round(sum2,3);
                    gzhgc.Add(yzd1 + x2);
                }
                for (i = 0; i < gzs.Count; i++)
                {
                    richTextBox4.Text += Math.Round(Convert.ToDouble(gzs[i]), 3).ToString("f3") + "\r\n";//输出改正数
                    richTextBox5.Text += Math.Round(Convert.ToDouble(gzh[i]), 3).ToString("f3") + "\r\n";//输出改正后高差
                }
                for (i = 0; i < gzhgc.Count; i++)
                {
                    richTextBox6.Text += Math.Round(Convert.ToDouble(gzhgc[i]), 3).ToString("f3") + "\r\n";//输出各点高程
                }
                //输出备注信息
                if (rb3.Checked == true)
                {
                    label6.Text = "总测站数:" + sum0;
                }
                else if (rb4.Checked == true)
                {
                    label6.Text = "总距离:  " + sum0 + " km";
                }

                label14.Text = "闭合差: " + Math.Round(fl, 3) + " m";
                label15.Text = "限差：  " + Math.Round(f, 0) + " mm";
                label16.Text = "高差改正数：" + Math.Round(fl * (-1), 3) + " mm";
               
            }

        }

        ///  输出exce过程。
      /// 
      /// </summary>
        private void Excel()
        {
            if (km == 0 | km == 1)            //判断是否进行计算，若未计算返回并弹出提示
            {
                MessageBox.Show("请先进行计算");
                return;
            }
            else if (km == 2)
            {
               //读取控件中数据
                    string[] a = richTextBox1.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] b = richTextBox2.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] c = richTextBox3.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] d = richTextBox4.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] e1 = richTextBox5.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] f1 = richTextBox6.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


                    //调用excel库  ，请添加引用 microsoft.office.interop.excel
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.ApplicationClass();
                    Workbook book = excel.Workbooks.Add(Missing.Value);                           // 添加一个工作簿
                    Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveSheet;// 获取当前工作表
                    sheet.Name = "sheetName1";                                                   // 修改工作表的名字
                    Range range = null;                                                          // 创建一个空的单元格对象

                //输入数据并设置单元格格式
                    sheet.Cells[2, 1] = "点号";
                    if (rb3.Checked == true)
                    {
                        sheet.Cells[2, 2] = "测站数";
                    }
                    else if (rb4.Checked == true)
                    {
                        sheet.Cells[2, 2] = "距离L/KM";
                    }
                    sheet.Cells[2, 3] = "高差/m";
                    sheet.Cells[2, 4] = "改正数/m";
                    sheet.Cells[2, 5] = "改正后高差/m";
                    sheet.Cells[2, 6] = "各点高程/m";
                    string k = "F" + Convert.ToString(a.Length * 2 + 3);
                    range = (Range)sheet.get_Range("A1", k);
                    range.NumberFormatLocal = "@";  //设置单元格格式为文本
                    range.Borders.LineStyle = 1;
                    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    range.Columns.AutoFit();
                    string A = "B" + (2 * (a.Length + 1) + 1);
                    string F = "F" + (2 * (a.Length + 1) + 1);
                    range = (Range)sheet.get_Range(A, F);
                    range.Merge(0);     //单元格合并动作
                    range.RowHeight = 45;
                    range.ColumnWidth = 10;
                    string G = "E" + (2 * (a.Length + 1) + 1);
                    range = (Range)sheet.get_Range("E1", G);
                    range.ColumnWidth = 15;
                    range = (Range)sheet.get_Range("A1", "F1");
                    range.Merge(0);     //单元格合并动作
                    range.Font.Name = "黑体";
                    range.RowHeight = 25;
                    range.Font.Size = 12;

                    for (i = 0; i < a.Length - 1; i++)
                    {
                        int k1 = 2 * (i + 2);
                        string n = "A" + Convert.ToString(k1);
                        string n1 = "A" + Convert.ToString(k1 + 1);
                        string m = "F" + Convert.ToString(k1);
                        string m1 = "F" + Convert.ToString(k1 + 1);
                        range = (Range)sheet.get_Range(n, n1);     //获取Excel多个单元格区域：本例做为Excel表头
                        range.Merge(0);     //单元格合并动作
                        range = (Range)sheet.get_Range(m, m1);
                        range.Merge(0);     //单元格合并动作

                    }
                    for (i = 0; i < b.Length + 1; i++)
                    {
                        int k1 = 2 * (i + 2);
                        string n = "B" + Convert.ToString(k1 - 1);
                        string n1 = "B" + Convert.ToString(k1);
                        range = (Range)sheet.get_Range(n, n1);
                        range.Merge(0);     //单元格合并动作
                        string m = "C" + Convert.ToString(k1 - 1);
                        string m1 = "C" + Convert.ToString(k1);
                        range = (Range)sheet.get_Range(m, m1);
                        range.Merge(0);     //单元格合并动作
                        string j = "D" + Convert.ToString(k1 - 1);
                        string j1 = "D" + Convert.ToString(k1);
                        range = (Range)sheet.get_Range(j, j1);
                        range.Merge(0);     //单元格合并动作
                        string l = "E" + Convert.ToString(k1 - 1);
                        string l1 = "E" + Convert.ToString(k1);
                        range = (Range)sheet.get_Range(l, l1);
                        range.Merge(0);     //单元格合并动作

                    }
                    sheet.Cells[1, 1] = "水准平差计算表";     //Excel单元格赋值

                    for (i = 0; i < a.Length; i++)
                    {
                        if (i == 0)
                        {
                            sheet.Cells[i + 3, 1] = a[i];   // 不管是Sheets[i]还是Cells[i,j]都是从1开始的，而不是从0开始的！！

                            sheet.Cells[i + 3, 6] = Convert.ToDouble(f1[i]).ToString("f3");
                        }
                        else
                        {
                            sheet.Cells[(i + 1) * 2, 1] = a[i];                   
                            sheet.Cells[(i + 1) * 2, 6] = Convert.ToDouble(f1[i]).ToString("f3");
                        }

                    }
                   
                    sheet.Cells[2 * (a.Length + 1), 1] = "Σ";
                    if (rb3.Checked == true)
                    {
                        sheet.Cells[2 * (a.Length + 1) - 1, 2] = Math.Round(sum0).ToString();
                    }
                    else if (rb4.Checked == true)
                    {
                        sheet.Cells[2 * (a.Length + 1) - 1, 2] = Math.Round(sum0, 1).ToString("f1");
                    }
                    sheet.Cells[2 * (a.Length + 1) - 1, 3] = Math.Round(sum1, 3).ToString("f3");
                    sheet.Cells[2 * (a.Length + 1) - 1, 4] = Math.Round(-fl, 3).ToString("f3");
                    sheet.Cells[2 * (a.Length + 1) - 1, 5] = Math.Round(sum2, 3).ToString("f3");
                    sheet.Cells[2 * (a.Length + 1) + 1, 1] = "计算" + "\r\n" + "检核";
                    sheet.Cells[2 * (a.Length + 1) + 1, 2] = "fh=H起+Σhi-H终=" + Math.Round(fl, 3) + "m" + "\r\n" + "f限=±" + Convert.ToString(gs) + "*" + Convert.ToString(sum0) + "^(1/2)" + "=±" + Math.Round(f, 0) + "mm" + "\r\n" ;
                    for (i = 0; i < b.Length; i++)
                    {

                        if (rb3.Checked == true)
                        {
                            sheet.Cells[2 * (i + 1) + 1, 2] = Convert.ToDouble(b[i]).ToString();
                        }
                        else if (rb4.Checked == true)
                        {
                            sheet.Cells[2 * (i + 1) + 1, 2] = Convert.ToDouble(b[i]).ToString("f1");
                        }

                        sheet.Cells[2 * (i + 1) + 1, 3] = Convert.ToDouble(c[i]).ToString("f3");
                        sheet.Cells[2 * (i + 1) + 1, 4] = Convert.ToDouble(d[i]).ToString("f3");
                        sheet.Cells[2 * (i + 1) + 1, 5] = Convert.ToDouble(e1[i]).ToString("f3");

                    }
                    SaveFileDialog saveFile2 = new SaveFileDialog();  //打开保存界面并输入文件名
                    saveFile2.Filter = "Excel工作簿(.xlsx)|*.xlsx";  //保存格式 
                    saveFile2.FilterIndex =2;
                    if (saveFile2.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile2.FileName.Length > 0)//判断是否输入文件名
                    {
                        string P_str_path = saveFile2.FileName;      //读取文件名
                        excel.Visible = true;                        //显示Excel表格
                        excel.ActiveWorkbook.SaveAs(P_str_path);     //保存工作表
                    }
                }
            }
        /// 清除并退出
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        { //清除缓存，关闭窗体。
            Close();
            GC.Collect();
            System.Windows.Forms.Application.Exit();//关闭
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }
        ///读取txt格式数据
        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void 导出到txtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (km == 0 | km == 1)                             //判断是否计算
            {
                MessageBox.Show("请先进行计算");
                return;
            }
            else if (km == 2)                                  //计算完成后开始输出数据
            {
                //读取控件中数据
                string[] a = richTextBox1.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] b = richTextBox2.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] c = richTextBox3.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] d = richTextBox4.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] e1 = richTextBox5.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] f1 = richTextBox6.Text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //选择路保存径
                SaveFileDialog saveFile1 = new SaveFileDialog();
                saveFile1.Filter = "文本文件(.txt)|*.txt";
                saveFile1.FilterIndex = 1;
                if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0) //判断文件名是否输入
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile1.FileName, false);              //创建写入流
                    for (i = 0; i < b.Length; i++)
                    {
                        sw.Write(a[i] + "," + b[i] + "," + c[i] + "," + d[i] + "," + e1[i] + "," + f1[i] + "\r\n"); //逐行写入数据至倒数第二行
                    }
                    sw.Write(a[a.Length - 1] + "   ," + "   ," + "   ," + "   ," + "   ," + f1[f1.Length - 1]);     //写入最后一行数据
                    sw.Close();                                                                                       //关闭流
                } 
            }
        }

        private void 导出到ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Excel(); //调用Exce（）过程
            }
            catch { }
            finally
            {
                GC.Collect();    //清除垃圾
            }
        }

        private void 打开记录手簿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 frm = new Form2();                                     
                frm.Show();
                this.Hide();
               
            }

            catch                                                //若文件格式出错，弹出错误并结束读取。
            {
                MessageBox.Show("读取失败，请确认文本格式。");
            }    
            finally
            {

            }
        }

        private void 打开高差数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DR();
            }

            catch                                                //若文件格式出错，弹出错误并结束读取。
            {
                MessageBox.Show("读取失败，请确认文本格式。");
            }
            finally
            {
            }
            
        }

    }
}

