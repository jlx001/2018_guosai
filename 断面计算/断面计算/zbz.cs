using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 断面计算.Form1;

namespace 断面计算
{
    class zbz
    {

        public List<T> z(List<double> A)                  
        {
            List<T> zb = new List<T>();
           // List<T> jt = new List<T>();
           // List<T> re = new List<List<T>();
            double k = A[0], x0 = A[1], x9 = A[2], y0 = A[3], y9 = A[4];
            double l0 = A[5], l9 = A[6], h0 = A[7], h9 = A[8];
            double zx0 = 0, zy0 = 0;
            zx0 = Math.Floor(1.00123 * x0 / 10) * 10;
            zy0 = Math.Floor(0.996 * y0 / 10) * 10;
            for (int i = 0; i <= k; i++)
            {

                if (i == 0)
                {
                    zb.Add(new T { X坐标 = zx0, Y坐标 = y9 * 1.002 });                            //坐标轴
                    zb.Add(new T { X坐标 = zx0, Y坐标 = zy0 });
                    zb.Add(new T { X坐标 = zx0, Y坐标 = zy0 });
                    zb.Add(new T { X坐标 = x9 * 1.002, Y坐标 = zy0 });

                    zb.Add(new T { X坐标 = zx0 - y0 * 0.0005, Y坐标 = y9 * 1.0005 });            //Y轴箭头
                    zb.Add(new T { X坐标 = zx0, Y坐标 = y9 * 1.002 });
                    zb.Add(new T { X坐标 = zx0, Y坐标 = y9 * 1.002 });
                    zb.Add(new T { X坐标 = zx0 + y0 * 0.0005, Y坐标 = y9 * 1.0005 });

                    zb.Add(new T { X坐标 = x9 * 1.0005, Y坐标 = zy0 + y0 * 0.0005 });              //X轴箭头
                    zb.Add(new T { X坐标 = x9 * 1.002, Y坐标 = zy0 });
                    zb.Add(new T { X坐标 = x9 * 1.002, Y坐标 = zy0 });
                    zb.Add(new T { X坐标 = x9 * 1.0005, Y坐标 = zy0 - y0 * 0.0005 });
                }
                else if (i == 1)
                {
                    x0 = zx0 + (x9 - zx0) * 1.2;
                    x9 = x0 + (l9 - l0);
                    y9 = zy0 + (h9 - h0);
                    zb.Add(new T { X坐标 = x0, Y坐标 = y9 * 1.002 });                            //坐标轴
                    zb.Add(new T { X坐标 = x0, Y坐标 = zy0 });
                    zb.Add(new T { X坐标 = x0, Y坐标 = zy0 });
                    zb.Add(new T { X坐标 = x9 * 1.002, Y坐标 = zy0 });

                    zb.Add(new T { X坐标 = x0 - y0 * 0.0005, Y坐标 = y9 * 1.001 });          //Y轴箭头
                    zb.Add(new T { X坐标 = x0, Y坐标 = y9 * 1.002 });
                    zb.Add(new T { X坐标 = x0, Y坐标 = y9 * 1.002 });
                    zb.Add(new T { X坐标 = x0 + y0 * 0.0005, Y坐标 = y9 * 1.001 });

                    zb.Add(new T { X坐标 = x9 * 1.001, Y坐标 = zy0 + zy0 * 0.0005 });              //X轴箭头
                    zb.Add(new T { X坐标 = x9 * 1.002, Y坐标 = zy0 });
                    zb.Add(new T { X坐标 = x9 * 1.002, Y坐标 = zy0 });
                    zb.Add(new T { X坐标 = x9 * 1.001, Y坐标 = zy0 - zy0 * 0.0005 });
                }
                else if (i > 1)
                {
                    x0 = zx0 + (i % 2) * 1.2 * (A[2] - A[1]);
                    x9 = x0 + 50;
                    y0 = A[4] - (i / 2 + 1) * (A[4] - A[3]);
                    y9 = y0 + (h9 - h0);
                    zb.Add(new T { X坐标 = (x0 + x9) / 2, Y坐标 = y9 * 1.002 }); ;                           //坐标轴
                    zb.Add(new T { X坐标 = (x0 + x9) / 2, Y坐标 = y0 });
                    zb.Add(new T { X坐标 = x0, Y坐标 = y0 });
                    zb.Add(new T { X坐标 = x9 * 1.002, Y坐标 = y0 });

                    zb.Add(new T { X坐标 = (x0 + x9)/2 - y0 * 0.0005, Y坐标 = y9 * 1.001 });          //Y轴箭头
                    zb.Add(new T { X坐标 = (x0 + x9)/2, Y坐标 = y9 * 1.002 });
                    zb.Add(new T { X坐标 = (x0 + x9)/2, Y坐标 = y9 * 1.002 });
                    zb.Add(new T { X坐标 = (x0 + x9)/2 + y0 * 0.0005, Y坐标 = y9 * 1.001 });

                    zb.Add(new T { X坐标 = x9 * 1.001, Y坐标 = y0 + y0 * 0.0005 });              //X轴箭头
                    zb.Add(new T { X坐标 = x9 * 1.002, Y坐标 = y0 });
                    zb.Add(new T { X坐标 = x9 * 1.002, Y坐标 = y0 });
                    zb.Add(new T { X坐标 = x9 * 1.001, Y坐标 = y0 - y0 * 0.0005 });
                }
            }

            return zb;
        }//坐标轴
        public List<T> jbkd(List<double> A)                  
        {
            List<T> kd = new List<T>();

            double k = A[0], x0 = A[1], x9 = A[2], y0 = A[3], y9 = A[4];
            double l0 = A[5], l9 = A[6], h0 = A[7], h9 = A[8];
            double zx0 = 0, zy0 = 0;

            zx0 = Math.Floor(1.00123 * x0 / 10) * 10;
            zy0 = Math.Floor(0.996 * y0 / 10) * 10;
            for (int j = 0; j <= k; j++)
            {
                if (j == 0)
                {
                    for (int i = 0; i < (y9 - y0) / 10 + 1; i++)
                    {
                        kd.Add(new T { X坐标 = zx0 - (y9 - y0) * 0.05, Y坐标 = zy0 + i * 10 });
                        kd.Add(new T { X坐标 = zx0, Y坐标 = zy0 + i * 10 });
                    }
                    for (int i = 0; i < (x9 - x0) / 10 + 1; i++)
                    {
                        kd.Add(new T { X坐标 = zx0 + i * 10, Y坐标 = zy0 - (y9 - y0) * 0.05 });
                        kd.Add(new T { X坐标 = zx0 + i * 10, Y坐标 = zy0 });
                    }
                }
                else if (j == 1)                   //纵断面线刻度
                {
                    x0 = zx0 + (x9 - zx0) * 1.2;
                    x9 = x0 + (l9 - l0);
                    y9 = zy0 + (h9 - h0);
                    for (int i = 0; i < (h9 - h0) / 2 + 1; i++)
                    {
                        kd.Add(new T { X坐标 = x0 - (h9 - h0) * 0.2, Y坐标 = zy0 + i * 2 });
                        kd.Add(new T { X坐标 = x0, Y坐标 = zy0 + i * 2 });
                    }
                    for (int i = 0; i < (x9 - x0) / 10 + 1; i++)
                    {
                        kd.Add(new T { X坐标 = x0 + i * 10, Y坐标 = zy0 - (h9 - h0) * 0.2 });
                        kd.Add(new T { X坐标 = x0 + i * 10, Y坐标 = zy0 });
                    }
                }
                else if (j > 1)               //横断面线刻度
                {
                    x0 = zx0 + (j % 2) * 1.2 * (A[2] - A[1]);
                    x9 = x0 + 50;
                    y0 = A[4] - (j / 2 + 1) * (A[4] - A[3]);
                    y9 = y0 + (h9 - h0);
                    for (int i = 0; i < (h9 - h0) / 2 + 1; i++)
                    {
                        kd.Add(new T { X坐标 = (x0+x9)/2 - (h9 - h0) * 0.2, Y坐标 = y0 + i * 2 });
                        kd.Add(new T { X坐标 = (x0+x9)/2, Y坐标 = y0 + i * 2 });
                    }
                    for (int i = 0; i < (x9 - x0) / 5 + 1; i++)
                    {
                        kd.Add(new T { X坐标 = x0 + i * 5, Y坐标 = y0 - (h9 - h0) * 0.2 });
                        kd.Add(new T { X坐标 = x0 + i * 5, Y坐标 = y0 });
                    }
                }
            }
            return kd;

        }//刻度线
        public List<T> bz(List<double> A)                    
        {

            List<T> zbbz = new List<T>();
            double k = A[0], x0 = A[1], x9 = A[2], y0 = A[3], y9 = A[4];
            double l0 = A[5], l9 = A[6], h0 = A[7], h9 = A[8];
            double zx0 = 0, zy0 = 0;
            zx0 = Math.Floor(1.00123 * x0 / 10) * 10;
            zy0 = Math.Floor(0.996 * y0 / 10) * 10; ;
            for (int j = 0; j <= k; j++)
            {

                if (j == 0)
                {
                    for (int i = 0; i < (y9 - y0) / 10 + 1; i++)
                    {
                        zbbz.Add(new T { 点名 = (zy0 + i * 10).ToString(), X坐标 = x0 - (y9 - y0) * 0.2, Y坐标 = zy0 + i * 10 });
                    }
                    for (int i = 0; i < (x9 - x0) / 10 + 1; i++)
                    {
                        zbbz.Add(new T { 点名 = (zx0 + i * 10).ToString(), X坐标 = zx0 + i * 10, Y坐标 = zy0 - (y9 - y0) * 0.1 });
                    }
                }
                else if (j == 1)
                {
                    x0 = zx0 + (x9 - zx0) * 1.2;
                    x9 = x0 + (l9 - l0);
                    y9 = zy0 + (h9 - h0);
                    for (int i = 0; i < (h9 - h0) / 2 + 1; i++)
                    {
                        zbbz.Add(new T { 点名 = (10 + i * 2).ToString(), X坐标 = x0 - (h9 - h0) * 0.4, Y坐标 = zy0 + i * 2 });
                    }
                    for (int i = 0; i < (l9 - l0) / 10 + 1; i++)
                    {
                        zbbz.Add(new T { 点名 = (i * 10).ToString(), X坐标 = x0 + i * 10, Y坐标 = zy0 - (h9 - h0) * 0.4 });
                    }
                }
                else if (j > 1)
                {
                    x0 = zx0 + (j % 2) * 1.2 * (A[2] - A[1]);
                    x9 = x0 + 50;
                    y0 = A[4] - (j / 2 + 1) * (A[4] - A[3]);
                    y9 = y0 + (h9 - h0);
                    for (int i = 0; i < (h9 - h0) / 2 + 1; i++)
                    {
                        zbbz.Add(new T { 点名 = (10 + i * 2).ToString(), X坐标 = (x0+x9)/2 - (h9 - h0) * 0.4, Y坐标 = y0 + i * 2 });
                    }
                    for (int i = 0; i < (x9 - x0) / 5 + 1; i++)
                    {
                        zbbz.Add(new T { 点名 = (i*5 - 25).ToString(), X坐标 = x0 + i * 5, Y坐标 = y0 - (h9 - h0) * 0.4 });
                    }
                }
            }
            return zbbz;
        }//刻度线标注
        public List< List<T>> zh(List<double> A,List<List<T>> B)
        {
            List< List<T>> zbzh = new List<List<T>>();
            double k = A[0], x0 = A[1], x9 = A[2], y0 = A[3], y9 = A[4];
            double l0 = A[5], l9 = A[6], h0 = A[7], h9 = A[8];
            double zx0 = 0, zy0 = 0;
            zx0 = Math.Floor(1.00123 * x0 / 10) * 10;
            zy0 = Math.Floor(0.996 * y0 / 10) * 10; ;
            for (int j = 0; j < k; j++)
            {
                if (j == 0)
                {
                    x0 = zx0 + (x9 - zx0) * 1.2;
                    x9 = x0 + (l9 - l0);
                    y9 = zy0 + (h9 - h0);
                    List<T> b = new List<T>();
                    for (int i = 0; i<B[j].Count; i++)
                    {                       
                        b.Add(new T { X坐标 =B[j][i].里程 + x0 , Y坐标 =B[j][i].高程+zy0-10 });                      
                    }
                    zbzh.Add(b);
                }
                else if (j > 0)
                {
                    x0 = zx0 + ((j+1) % 2) * 1.2 * (A[2] - A[1]);
                    x9 = x0 + 50;
                    y0 = A[4] - ((j+1) / 2+1 ) * (A[4] - A[3]);
                    y9 = y0 + (h9 - h0);
                    List<T> b = new List<T>();
                    for (int i = 0; i < B[j].Count; i++)
                    {   
                        b.Add(new T {  X坐标 =B[j][i].里程 + x0 , Y坐标 =B[j][i].高程+y0-10});                      
                    }
                    zbzh.Add(b);
                }                
            }
            return zbzh;
        }  //坐标转换
        public List<T> tm(List<double> A)
        {
            List<T> zbzh = new List<T>();
            double k = A[0], x0 = A[1], x9 = A[2], y0 = A[3], y9 = A[4];
            double l0 = A[5], l9 = A[6], h0 = A[7], h9 = A[8];
            double zx0 = 0, zy0 = 0;
            zx0 = Math.Floor(1.00123 * x0 / 10) * 10;
            zy0 = Math.Floor(0.996 * y0 / 10) * 10; ;
            for (int j = 0; j <= k; j++)
            {
                if (j == 0)
                {
                    x0 = zx0 ;
                    x9 = x0 + (l9 - l0);
                    y0 = zy0 - (y9 - y0)*0.2;
                    zbzh.Add(new T {点名="JBT", X坐标 = (x0+x9)/2, Y坐标 = y0 });
                }
                else if (j == 1)
                {
                    x0 = zx0 + (x9 - zx0) * 1.2;
                    x9 = x0 + (l9 - l0);
                    y0 = zy0 - (y9 - A[3]) * 0.2;
                    zbzh.Add(new T {点名="ZDM", X坐标 = (x0+x9)/2, Y坐标 = y0 });
                }
                else if (j > 0)
                {
                    x0 = zx0 + (j % 2) * 1.2 * (A[2] - A[1]);
                    x9 = x0 + 50;
                    y0 = A[4] - (j / 2 + 1) * (A[4] - A[3]);
                    y9 = y0 + (h9 - h0);
                    zbzh.Add(new T { 点名 = "HDM"+(j-1).ToString(), X坐标 = (x0 + x9) / 2, Y坐标 = y0 - 7 });
                }
            }
            return zbzh;
        }  //图名
    }
}
