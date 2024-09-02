using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAGD_Worshop.CAGD_Polygon3d
{
    public class Polygon3d
    {
        public List<Point3d> points = new List<Point3d>();
        string fileDestination = "";

        public Polygon3d(params Point3d[] points)
        {
            this.points = points.ToList();
            string baseDirectory = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "FilePath");
            fileDestination = baseDirectory + "\\Polygon3d.txt";
        }
        public void DrawPolygon(Polygon3d poly, double maxAreaOfTriangle = 0.4)
        {
            File.WriteAllText(fileDestination, string.Empty);
            var points = poly.points;

            foreach (var p in points)
            {
                WritePointsInAFile(p, fileDestination);
            }

            //WritePointsInAFile(poly.points[0], fileDestination);

            //DrawMeshOfTriangles(poly ,points , fileDestination, maxAreaOfTriangle);
        }

        private void WritePointsInAFile(Point3d p, string fileDest)
        {
            File.AppendAllText(fileDest, p.x.ToString() + " " + p.y.ToString() +" "+ p.z.ToString() + "\n");
        }

        public Polygon3d Get3dPolygon()
        {
            List<Point3d> points = new List<Point3d>
            {
                new Point3d(1,1,1),
                new Point3d(4,1,1),
                new Point3d(4,4,1),
                new Point3d(1,4,1),
                new Point3d(1,1,1),


                new Point3d(1,1,4),
                new Point3d(4,1,4),
                new Point3d(4,4,4),
                new Point3d(1,4,4),
                new Point3d(1,1,4),
                
                new Point3d(4,1,4),
                new Point3d(4,1,1),

                new Point3d(4,4,1),
                new Point3d(4,4,4),

                new Point3d(1,4,4),
                new Point3d(1,4,1),


            };
            Polygon3d poly3d = new Polygon3d(points.ToArray());

            return poly3d;
        }

    }
}
