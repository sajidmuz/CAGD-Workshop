using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAGD_Worshop
{
    public class Utility
    {
        public static Polygon GetTransFormedPolygon(Polygon polygon, Point pointOfRotation, double rotationAngle = 0)
        {
            Polygon tPoly = new Polygon();
            var tMatrix = GetTranslationMatrix(-pointOfRotation.x, -pointOfRotation.y);
            foreach (var point in polygon.points)
            {
                tPoly.points.Add(TranslatedPoint(point, tMatrix));
            }

            Polygon rPoly = new Polygon();
            foreach (var point in tPoly.points)
            {
                rPoly.points.Add(RotatedPoint(point, GetRotationMatrix(rotationAngle)));
            }

            Polygon fPoly = new Polygon();
            var fMatrix = GetTranslationMatrix(pointOfRotation.x, pointOfRotation.y);
            foreach (var point in rPoly.points)
            {
                fPoly.points.Add(TranslatedPoint(point, fMatrix));
            }

            return fPoly;
        }

        public static Point TranslatedPoint(Point p, double[,] tMatrix)
        {
            double x = p.x;
            double y = p.y;

            double newX = tMatrix[0, 0] * x + tMatrix[0, 1] * y + tMatrix[0, 2];
            double newY = tMatrix[1, 0] * x + tMatrix[1, 1] * y + tMatrix[1, 2];

            return new Point(newX, newY);
        }

        public static double[,] GetTranslationMatrix(double x, double y)
        {
            double[,] translationMatrix = new double[3, 3]
            {
                { 1, 0, x},
                { 0, 1, y },
                { 0, 0, 1 }
            };

            return translationMatrix;
        }

        public static double[,] GetRotationMatrix(double angle)
        {
            var angleInRad = AngleInRadians(angle);
            double[,] rotationMatrix = new double[3, 3]
            {
                {Math.Cos(angleInRad), -Math.Sin(angleInRad), 0 },
                {Math.Sin(angleInRad), Math.Cos(angleInRad), 0 },
                {0,                 0,                      1 }
            };

            return rotationMatrix;
        }

        private static Point RotatedPoint(Point p, double[,] rMatrix)
        {
            double x = p.x;
            double y = p.y;

            double newX = rMatrix[0, 0] * x + rMatrix[0, 1] * y + rMatrix[0, 2];
            double newY = rMatrix[1, 0] * x + rMatrix[1, 1] * y + rMatrix[1, 2];

            return new Point(newX, newY);
        }

        private static double AngleInRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }
    }
}
