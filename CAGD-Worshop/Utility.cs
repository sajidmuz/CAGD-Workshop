using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAGD_Worshop
{
    public class Utility
    {
        public static Polygon GetTransFormedPolygon(Polygon poly, double[,] tMatrix)
        {
            Polygon nPoly = new Polygon();
            foreach (var point in poly.points)
            {
                nPoly.points.Add(TranslatedPoint(point, tMatrix));
            }

            return nPoly;
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
    }
}
