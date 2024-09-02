using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAGD_Worshop.CAGD_Polygon3d
{
    public class Point3d
    {
        public double x { get; }
        public double y { get; }
        public double z { get; }

        public Point3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
