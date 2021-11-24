using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 三角高程
{
    class JDZH
    {   double p = Math.PI / 180;
        double q = 180 / Math.PI;
        public double  jzh(double m)
        {
            double m0, x1, x2, x3;
            m0 = Math.Abs(m);
            x1 = Math.Floor(m0);
            x2 =Math.Floor((m0 - x1)*100);
            x3 = ((m0 - x1) * 100 - x2)*100;
            m =Math.Sign(m) * (x1 + x2 / 60 + x3 / 3600) * p;
            return m;
        }
        public double hzj(double m)
        {
            double m0, x1, x2, x3;
            m0 = Math.Abs(m) * q;
            x1 = Math.Floor(m0);
            x2 =Math.Floor((m0 - x1) *100);
            x3 = m0 - x1 - x2 / 100;
            m = Math.Sign(m) * (x1 + x2 / 100 * 60 + x3 / 10000 * 60);
            return m;
        }
    }
}
