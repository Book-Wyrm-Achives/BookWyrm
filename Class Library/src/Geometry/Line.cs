using System;

namespace BookWyrm.Geometry
{
    public struct Line
    {
        public readonly Vector StartPoint;
        public readonly Vector EndPoint;

        public Line(Vector start, Vector end)
        {
            StartPoint = start;
            EndPoint = end;
        }

        public Vector Along => EndPoint - StartPoint;

        public int Dimension => StartPoint.Dimension > EndPoint.Dimension ? StartPoint.Dimension : EndPoint.Dimension;

        public Vector NearestToPoint(Vector point)
        {
            return StartPoint + (point - StartPoint).Projection(Along);
        }

        public bool ContainsPoint(Vector point)
        {
            var startDelta = point - StartPoint;
            var endDelta = point - EndPoint;

            return Vector.Dot(startDelta, Along) >= 0.0f && Vector.Dot(endDelta, Along) <= 0.0f && startDelta.Rejection(Along).SquareMagnitude() < 1e-12;
        }

        public static bool Intersects(Line a, Line b, out Vector intersectionPoint, int dimensionX = 0, int dimensionY = 1, int dimensionZ = 2)
        {
            

            Vector pointA = a.StartPoint;
            Vector pointB = b.StartPoint;
            Vector directionA = a.Along.Normalized();
            Vector directionB = b.Along.Normalized();

            float t = (directionB[dimensionY] * (pointA[dimensionX] - pointB[dimensionX]) - directionB[dimensionX] * (pointA[dimensionY] - pointB[dimensionY])) / (directionB[dimensionX] * directionA[dimensionY] - directionB[dimensionY] * directionA[dimensionX]);
            float s = (pointA[dimensionX] - pointB[dimensionX] + directionA[dimensionX] * t) / directionB[dimensionX];
        
            intersectionPoint = pointA + directionA * t;
            float pointDelta = pointA[dimensionZ] + directionA[dimensionZ] * t - (pointB[dimensionZ] + directionB[dimensionZ] * s);
        
            return pointDelta > -1e-6 && pointDelta < 1e-6;
        }

        public class LineException : Exception { }
        
        public class ParallelLineException : LineException { }
        public class SameLineException : LineException { }
        public class VerticalLineException : LineException { }
        public class DimensionErrorException : LineException { }

        public static Line PointDirection(Vector point, Vector direction, float distance = 1.0f) => new Line(point, point + direction.Normalized() * distance);
        public static Line StandardForm(float A, float B, float C)
        {
            if (B == 0.0f) return new Line(new Vector(-C / A, 0.0f), new Vector(-C / A, 1.0f));
            else if (A == 0.0f) return new Line(new Vector(0.0f, -C / B), new Vector(1.0f, -C / B));
            else return new Line(new Vector(0.0f, -C / B), new Vector(1.0f, (-C - A) / B));
        }
    }
}