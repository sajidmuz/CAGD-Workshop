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
        Point intersectionPoint = null; 
        private List<Triangle> listOfTriangles = new List<Triangle>();
        private List<Line> listOfLines = new List<Line>();
        string fileDestination = "";

        public Polygon(params Point[] points)
        {
            this.points = points.ToList();
            string baseDirectory = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "FilePath");
            fileDestination = baseDirectory + "\\Polygon.txt";
        }

        public void DrawPolygon(Polygon poly, double maxAreaOfTriangle = 0.4)
        {
            StoreListOfLines();

            File.WriteAllText(fileDestination, string.Empty);

            /*WritePointsInAFile(poly.p1, dest);
            WritePointsInAFile(poly.p2, dest);*/


            foreach (var p in points)
            {
                WritePointsInAFile(p, fileDestination);
            }

            WritePointsInAFile(poly.points[0], fileDestination);

            //DrawMeshOfTriangles(poly, fileDestination, maxAreaOfTriangle);
        }

        public void RotatePolygon(Polygon poly, float angle = 0, double maxAreaOfTriangle = 0.4)
        {
            File.WriteAllText(fileDestination, string.Empty);

            /*WritePointsInAFile(poly.p1, dest);
            WritePointsInAFile(poly.p2, dest);*/

            List<Point> ListPoints = new List<Point>();

            foreach (var p in points)
            {
                Point newPoint = new Point(Math.Round(p.x * Math.Cos(angle * (Math.PI / 180)), 4), p.y);
                ListPoints.Add(newPoint);
                //WritePointsInAFile(newPoint, fileDestination);
            }
            points = ListPoints;

            foreach (var p in points)
            {
                Point newPoint = new Point(Math.Round(p.x * Math.Cos(angle * (Math.PI / 180)), 4), p.y);
                WritePointsInAFile(newPoint, fileDestination);
            }

            WritePointsInAFile(poly.points[0], fileDestination);
            DrawMeshOfTriangles(poly, fileDestination, maxAreaOfTriangle);
        }

        private void StoreListOfLines()
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count - 1)
                    listOfLines.Add(new Line(points[i], points[i + 1]));
                else
                    listOfLines.Add(new Line(points[i], points[0]));
            }
        }

        private void WritePointsInAFile(Point p, string fileDest)
        {
            File.AppendAllText(fileDest, p.x.ToString() + " " + p.y.ToString() + "\n");
        }

        private void DrawMeshOfTriangles(Polygon poly, string fileDest, double maxAreaOfTriangle)
        {
            int midPointIndex = GetMidPointIndex(poly);

            intersectionPoint = MidPointOfPolygon(poly);

            foreach (var p in points)
            {
                WritePointsInAFile(intersectionPoint, fileDest);
                WritePointsInAFile(p, fileDest);
            }

            StoreListOfTriangles();
            WritePointsInAFile(points[0], fileDestination);
            var newTriangles = TriangulateTheTriangles(listOfTriangles, maxAreaOfTriangle);
            /*var nt = TriangulateTheTriangles(newTriangles);
            var nt2 = TriangulateTheTriangles(nt);
            TriangulateTheTriangles(nt2);*/


        }

        private int GetMidPointIndex(Polygon poly)
        {
            int numberOfPoints = poly.points.Count;
            int midPoint = 0;
            
            if(numberOfPoints % 2 == 0)
            {
                midPoint = (numberOfPoints) / 2;
            }
            else
            {
                midPoint = ((numberOfPoints) / 2) + 1;
            }

            return midPoint;
        }

        private Point GetIntersection(Line l1, Line l2)
        {
            double m1 = (l1.p2.y - l1.p1.y) / (l1.p2.x - l1.p1.x);
            double m2 = (l2.p2.y - l2.p1.y) / (l2.p2.x - l2.p1.x);

            if (m1 == m2)
            {
                return null;
            }

            double c1 = l1.p1.y - m1 * l1.p1.x;
            double c2 = l2.p1.y - m2 * l2.p1.x;

            double x = (c2 - c1) / (m1 - m2);
            double y = m1 * x + c1;

            return new Point(x, y);
        }

        private void StoreListOfTriangles()
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count - 1)
                    listOfTriangles.Add(new Triangle(intersectionPoint, points[i], points[i+1]));
                else
                    listOfTriangles.Add(new Triangle(intersectionPoint, points[i], points[0]));
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

        public void Transformation(Polygon polygon, double maxAreaOfTriangle = 0.4)
        {
            ApplyTransformation(polygon, GetTransformationMatrix(), maxAreaOfTriangle);
        }


        public Polygon ApplyTransformation(Polygon polygon, double[,] matrix, double maxAreaOfTriangle)
        {
            Polygon transformedPolygon = new Polygon();
            List<Point> transformedPoints = new List<Point>();

            foreach (var p in polygon.points)
            {
                Point transformedPoint = TransformPoint(p, matrix);
                transformedPoints.Add(transformedPoint);
            }

            points = transformedPoints;

            DrawPolygon(polygon, maxAreaOfTriangle);

            return transformedPolygon;
        }

        public Point TransformPoint(Point point, double[,] matrix)
        {
            double x = point.x;
            double y = point.y;
            double z = 0;

            double newX = matrix[0, 0] * x + matrix[0, 1] * y + matrix[0, 2] * z + matrix[0, 3];
            double newY = matrix[1, 0] * x + matrix[1, 1] * y + matrix[1, 2] * z + matrix[1, 3];
            double newZ = matrix[2, 0] * x + matrix[2, 1] * y + matrix[2, 2] * z + matrix[2, 3];

            return new Point(newX, newY);
        }

        public double[,] GetTransformationMatrix()
        {
            double[,] matrix_around_X_rotation = new double[4, 4]
            {
                { 1,    0,      0,          5 },
                { 0,    0.5235, -0.5235,    7 },
                { 0,    0.5235, 0.5235,     5 },
                { 0,    0,      0,          1 }
            };

            return matrix_around_X_rotation;
        }

        
    }
}
