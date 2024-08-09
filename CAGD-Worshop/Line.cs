using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAGD_Worshop
{
    public class Line
    {
        public Point p1 { get; }
        public Point p2 { get; }

        public Line(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public void DrawLine(Line line)
        {
            string dest = @"C:\Users\Sajid\Desktop\Line.txt";
            File.WriteAllText(dest, string.Empty);


        }
    }
}
