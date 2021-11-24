using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 断面计算
{
    class dxf
    {
        public string[] txt(List<Form1.T> A)
        {
            string[] str = new string[A.Count];
            for (int i = 0; i < A.Count; i++)    //文字
            {
                str[i] = "0\r\nTEXT\r\n8\r\n0\r\n10\r\n" + A[i].X坐标 + "\r\n 20\r\n" + A[i].Y坐标 + "\r\n 30\r\n0.0\r\n 40\r\n1.0\r\n1\r\n" + A[i].点名 + "\r\n7\r\n等线\r\n";
            }
            return str;
        }
        public string[] dl(List<Form1.T> A)     //单线
        {
            string[] str = new string[A.Count];
            for (int i = 0; i < A.Count / 2; i++)
            {
                str[i] = "0\r\nLINE\r\n8\r\n1\r\n10\r\n" + A[i * 2].X坐标 + "\r\n20\r\n" + A[i * 2].Y坐标 + "\r\n11\r\n" + A[i * 2 + 1].X坐标 + "\r\n21\r\n" + A[i * 2 + 1].Y坐标 + "\r\n";
            }
            return str;
        }
        public string[] pl(List<Form1.T> A)    //多段线
        {
            string[] str = new string[A.Count];
            for (int i = 0; i < A.Count - 1; i++)
            {
                str[i] = "0\r\nLINE\r\n8\r\n1\r\n10\r\n" + A[i].X坐标 + "\r\n20\r\n" + A[i].Y坐标 + "\r\n11\r\n" + A[i + 1].X坐标 + "\r\n21\r\n" + A[i + 1].Y坐标 + "\r\n";
            }
            return str;
        }
        public string[] point(List<Form1.T> A)    //点
        {
            string[] str = new string[A.Count];
            for (int i = 0; i < A.Count; i++)
            {
                str[i] = "0\r\nPOINT\r\n8\r\n1\r\n10\r\n" + A[i].X坐标 + "\r\n20\r\n" + A[i].Y坐标 + "\r\n";
            }
            return str;
        }
        public string[] zbz(List<Form1.T> A)    //点
        {
            string[] str = new string[A.Count];
            for (int i = 0; i < A.Count/3; i++)
            {
                str[i*2] = "0\r\nLINE\r\n8\r\n1\r\n10\r\n" + A[i * 3].X坐标 + "\r\n20\r\n" + A[i * 3].Y坐标 + "\r\n11\r\n" + A[i * 3 + 1].X坐标 + "\r\n21\r\n" + A[i * 3 + 1].Y坐标 + "\r\n";
                str[i*2+1]="0\r\nLINE\r\n8\r\n1\r\n10\r\n" + A[i * 3 + 1].X坐标 + "\r\n20\r\n" + A[i * 3 + 1].Y坐标 + "\r\n11\r\n" + A[i * 3 + 2].X坐标 + "\r\n21\r\n" + A[i * 3 + 2].Y坐标 + "\r\n";
            }
            return str;
        }
    }
}
