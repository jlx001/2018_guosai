using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 规则格网
{
    class dxf
    {
       public string[] point(List<Form1.DJ> A)
        {
            string[] str = new string[A.Count];
            for (int i = 0; i < A.Count; i++)
            {
                str[i] = "0\r\nPOINT\r\n8\r\n1\r\n10\r\n" + A[i].X + "\r\n20\r\n" + A[i].Y + "\r\n";
            }
            return str;
        }
        public string[] pl(List<Form1.DJ> A)
        {
            string[] str = new string[A.Count];
            for (int i = 0; i < A.Count-1; i++)
            {
                str[i] = "0\r\nLINE\r\n8\r\n1\r\n10\r\n" + A[i].X + "\r\n20\r\n" + A[i].Y+ "\r\n11\r\n" + A[i + 1].X + "\r\n21\r\n" + A[i + 1].Y + "\r\n";
            }
            return str;
        }
        public string[] dl(List<Form1.DJ> A)     //单线
        {
            string[] str = new string[A.Count];
            for (int i = 0; i < A.Count / 2; i++)
            {
                str[i] = "0\r\nLINE\r\n8\r\n1\r\n10\r\n" + A[i * 2].X + "\r\n20\r\n" + A[i * 2].Y + "\r\n11\r\n" + A[i * 2 + 1].X + "\r\n21\r\n" + A[i * 2 + 1].Y + "\r\n";
            }
            return str;
        }
        public string[] txt(List<Form1.DJ> A)
        {
            string[] str = new string[A.Count];
            for (int i = 0; i < A.Count; i++)    //文字
            {
                str[i] = "0\r\nTEXT\r\n8\r\n0\r\n10\r\n" + A[i].X + "\r\n 20\r\n" + (A[i].Y+A[i].Y*0.0001) + "\r\n 30\r\n0.0\r\n 40\r\n0.5\r\n1\r\n" + A[i].点号 + "\r\n7\r\n等线\r\n";
            }
            return str;
        }
    }
}
