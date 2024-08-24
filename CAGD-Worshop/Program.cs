using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAGD_Worshop
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello world");
            //Polygon p = new Polygon(new Point(0, 0), new Point(0, 5), new Point(5, 5), new Point(5, 0));

            //Polygon p = new Polygon(new Point(0, 0), new Point(0, 5), new Point(3, 8), new Point(6, 5), new Point(6, 0), new Point(3, -3));
            //Polygon p = new Polygon(new Point(0, 0), new Point(0, 5), new Point(3, 8), new Point(6, 5), new Point(6, 0));

            Polygon p = new Polygon(new Point(1, 1), new Point(4, 1), new Point(4, 4), new Point(1, 4));

            Polygon transfornedPolygon = Utility.GetTransFormedPolygon(p, Utility.GetTranslationMatrix(-2.5, -2.5));


            transfornedPolygon.DrawPolygon(transfornedPolygon, 0.4);

            //p.Transformation(p, .3);

            //p.RotatePolygon(p, 75, .5);
        }
    }
}
