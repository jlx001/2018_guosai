using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 附合导线
{
    class HTCL
    {
        public Point[] sjx(Point[] pt)
        {
            Point[] pic = new Point[3];
           // pt = new Point[1];
            pt[0].X = pt[0].X + 2;
            pt[0].Y = pt[0].Y - 2;


            pic[0].X = pt[0].X - 4;
            pic[0].Y = pt[0].Y + 4;
            pic[1].X = pt[0].X + 4;
            pic[1].Y = pt[0].Y + 4;
            pic[2].X = pt[0].X;
            pic[2].Y = pt[0].Y - 4;
            return pic;
        }
        public Point[] sx(Point[] pt)
        {          
            Point[] pic = new Point[2];
           //pt = new Point[2];
            double m = Math.Abs(pt[0].X - pt[1].X);
            double n = Math.Abs(pt[0].Y - pt[1].Y);
            if (n > m)
            {
                pic[0].X = pt[0].X + 3;
                pic[0].Y = pt[0].Y - 1;
                pic[1].X = pt[1].X + 3;
                pic[1].Y = pt[1].Y - 1;
            }
            else if (m > n)
            {
                pic[0].X = pt[0].X + 1;
                pic[0].Y = pt[0].Y - 3;
                pic[1].X = pt[1].X + 1;
                pic[1].Y = pt[1].Y - 3;
            }
            return pic;
        }
    }
}
