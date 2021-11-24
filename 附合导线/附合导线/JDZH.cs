using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 附合导线
{
    class JDZH
    {
       
        public const double PI = Math.PI;
        public double hzj(double m)
        {
            double fh = Math.Sign(m);
            double x, x1, x2, x3;
            m = (m * 180 / PI);
            m = Math.Abs(m) + 0.000000000001;
            x = Math.Floor(m);
            x1 = (m - x) * 60;
            x2 = Math.Floor(x1);
            x3 = (x1 - x2) * 60;
            return m = fh * (x + x2 / 100 + x3 / 10000);
        }
        public double jzh(double n)
        {
            double fh = Math.Sign(n);
            n = Math.Abs(n);
            double x, x1, x2, x3;
            n = n + 0.000000000001;
            x = Math.Floor(n);              //对度数取整。

            x1 = (n - x) * 100;
            x2 = Math.Floor(x1);            //分。
            x3 = (x1 - x2) * 100;           //秒。

            return n = fh * (x + x2 / 60 + x3 / 3600) / 180 * PI;
        }
    }
}
