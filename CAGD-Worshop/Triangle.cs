using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAGD_Worshop
{
    public class Triangle
    {
        public Point p1 { get; }
        public Point p2 { get; }
        public Point p3 { get; }

        public Triangle(Point p1, Point p2, Point p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public void DrawTriangle(Triangle t1)
        {
            
        }
    }
}
