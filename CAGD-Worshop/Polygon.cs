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
        public List<Point> points = new List<Point>();
        Point midPointOfPolygon = null; 
        private List<Triangle> listOfTriangles = new List<Triangle>();
        string fileDestination = "";

        public Polygon(params Point[] points)
        {
            this.points = points.ToList();
            string baseDirectory = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "FilePath");
            fileDestination = baseDirectory + "\\Polygon.txt";
        }

        public void DrawPolygon(Polygon poly, double maxAreaOfTriangle = 0.4)
        {
            File.WriteAllText(fileDestination, string.Empty);
            var points = poly.points;

            foreach (var p in points)
            {
                WritePointsInAFile(p, fileDestination);
            }

            WritePointsInAFile(poly.points[0], fileDestination);

            //DrawMeshOfTriangles(poly ,points , fileDestination, maxAreaOfTriangle);
        }

        private void WritePointsInAFile(Point p, string fileDest)
        {
            File.AppendAllText(fileDest, p.x.ToString() + " " + p.y.ToString() + "\n");
        }

        private void DrawMeshOfTriangles(Polygon poly,List<Point> points, string fileDest, double maxAreaOfTriangle)
        {
            midPointOfPolygon = MidPointOfPolygon(poly);

            foreach (var p in points)
            {
                WritePointsInAFile(midPointOfPolygon, fileDest);
                WritePointsInAFile(p, fileDest);
            }

            StoreListOfTriangles(points, midPointOfPolygon);
            WritePointsInAFile(points[0], fileDestination);
            var newTriangles = TriangulateTheTriangles(listOfTriangles, maxAreaOfTriangle);
        }

        private void StoreListOfTriangles(List<Point> points, Point midPointOfPolygon)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count - 1)
                    listOfTriangles.Add(new Triangle(midPointOfPolygon, points[i], points[i+1]));
                else
                    listOfTriangles.Add(new Triangle(midPointOfPolygon, points[i], points[0]));
            }
        }

        private List<Triangle> TriangulateTheTriangles(List<Triangle> triangles, double maxAreaOfTriangle)
        {
            List<Triangle> listOfTriangles = new List<Triangle>();
            foreach (var triangle in triangles)
            {
                if (AreaOfTriangle(triangle) < maxAreaOfTriangle)
                    continue;

                Line gLine = GreatestEdge(triangle, out Point thirdPoint);
                Point midPointOfLine = MidPoint(gLine);

                WritePointsInAFile(midPointOfLine, fileDestination);
                WritePointsInAFile(thirdPoint, fileDestination);
                WritePointsInAFile(new Point(double.NaN, double.NaN), fileDestination);

                if (triangle.p1.x != thirdPoint.x || triangle.p1.y != thirdPoint.y)
                {
                    listOfTriangles.Add(new Triangle(midPointOfLine, thirdPoint, triangle.p1));
                }
                if (triangle.p2.x != thirdPoint.x || triangle.p2.y != thirdPoint.y)
                {
                    listOfTriangles.Add(new Triangle(midPointOfLine, thirdPoint, triangle.p2));
                }
                if (triangle.p3.x != thirdPoint.x || triangle.p3.y != thirdPoint.y)
                {
                    listOfTriangles.Add(new Triangle(midPointOfLine, thirdPoint, triangle.p3));
                }
            }
            if (listOfTriangles.Any())
            {
                TriangulateTheTriangles(listOfTriangles, maxAreaOfTriangle);
            }
                

            return listOfTriangles;
        }

        private Point MidPoint(Line line)
        {
            return new Point(((line.p1.x + line.p2.x) / 2), ((line.p1.y + line.p2.y) / 2));
        }

        private Line GreatestEdge(Triangle triangle, out Point thirdPoint)
        {
            double edgeLengthAB = EdgeLength(triangle.p1, triangle.p2);
            double edgeLengthBC = EdgeLength(triangle.p2, triangle.p3);
            double edgeLengthCA = EdgeLength(triangle.p3, triangle.p1);

            if (edgeLengthAB >= edgeLengthBC && edgeLengthAB >= edgeLengthCA)
            {
                thirdPoint = triangle.p3;
                return new Line(triangle.p1, triangle.p2);
            }else if(edgeLengthBC >= edgeLengthAB && edgeLengthBC >= edgeLengthCA)
            {
                thirdPoint = triangle.p1;
                return new Line(triangle.p2, triangle.p3);
            }else
            {
                thirdPoint = triangle.p2;
                return new Line(triangle.p3, triangle.p1);
            }
        }

        private double EdgeLength(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
        }

        private Point MidPointOfPolygon(Polygon polygon)
        {
            double sumOfX = 0;
            double sumOfY = 0;

            for (int i = 0; i < polygon.points.Count; i++)
            {
                sumOfX += polygon.points[i].x;
                sumOfY += polygon.points[i].y;
            }

            return new Point(sumOfX / polygon.points.Count, sumOfY / polygon.points.Count);
        }

        private double AreaOfTriangle(Triangle t)
        {
            return 0.5 * Math.Abs(t.p1.x * (t.p2.y - t.p3.y) + t.p2.x * (t.p3.y - t.p1.y) + t.p3.x * (t.p1.y - t.p2.y));
        }

    }
}
