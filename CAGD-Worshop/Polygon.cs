using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAGD_Worshop
{
    public class Polygon
    {
        private Point p1;
        private Point p2;
        private Point[] points;

        public Polygon(Point p1, Point p2, params Point[] points)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.points = points;
        }

        public void DrawPolygon(Polygon poly)
        {
            string dest = @"C:\Users\mdsaj\OneDrive\Desktop\Polygon.txt.txt";
            File.WriteAllText(dest, string.Empty);

            WritePointsInAFile(poly.p1, dest);
            WritePointsInAFile(poly.p2, dest);

            foreach (var p in points)
            {
                WritePointsInAFile(p, dest);
            }

            WritePointsInAFile(poly.p1, dest);

            DrawMeshOfTriangles(poly, dest);
        }

        private void WritePointsInAFile(Point p, string fileDest)
        {
            File.AppendAllText(fileDest, p.x.ToString() + " " + p.y.ToString() + "\n");
        }

        private void DrawMeshOfTriangles(Polygon poly, string fileDest)
        {
            /*WritePointsInAFile(poly.p1, fileDest);
            for (int i = 0; i < points.Length; i++)
            {
                WritePointsInAFile(points[i], fileDest);
                if(i+2 < points.Length)
                {
                    WritePointsInAFile(points[i + 2], fileDest);
                }
                else
                {
                    WritePointsInAFile(poly.p2, fileDest);
                }
            }*/

            /*foreach (var p in points)
            {
                if (isOdd)
                {
                    WritePointsInAFile(p, fileDest);
                    isOdd = false;
                }
                else
                {
                    isOdd = true;
                }
            }*/

            int midPointIndex = GetMidPointIndex(poly);

            Point intersectionPoint = GetIntersection(new Line(poly.p1, points[midPointIndex]), new Line(poly.p2, points[midPointIndex + 1]));

            WritePointsInAFile(intersectionPoint, fileDest);
            WritePointsInAFile(poly.p1, fileDest);

            WritePointsInAFile(intersectionPoint, fileDest);
            WritePointsInAFile(poly.p2, fileDest);

            foreach (var p in points)
            {
                WritePointsInAFile(intersectionPoint, fileDest);
                WritePointsInAFile(p, fileDest);
            }

        }

        private int GetMidPointIndex(Polygon poly)
        {
            int numberOfPoints = 2 + poly.points.Length;
            int midPoint = 0;
            
            if(numberOfPoints % 2 == 0)
            {
                midPoint = (numberOfPoints-2) / 2;
            }
            else
            {
                midPoint = ((numberOfPoints-2) / 2) + 1;
            }

            return midPoint;
        }

        private Point GetIntersection(Line l1, Line l2)
        {
            double m1 = (l1.p2.y - l1.p1.y) / (l1.p2.x - l1.p1.x);
            double m2 = (l2.p2.y - l2.p1.y) / (l2.p2.x - l2.p1.y);

            if (m1 == m2)
            {
                return null;
            }

            double c1 = l1.p1.y - m1 * l1.p1.x;
            double c2 = l2.p1.y - m2 * l2.p1.y;

            double x = (c2 - c1) / (m1 - m2);
            double y = m1 * x + c1;

            return new Point(x, y);
        }
    }
}
