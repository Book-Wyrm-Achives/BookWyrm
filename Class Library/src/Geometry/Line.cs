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
        public Line(Vector point, Vector direction, float distance) {
            StartPoint = point;
            EndPoint = point + direction.Normalized() * distance;
        }


        public Vector Along => EndPoint - StartPoint;
        public float Slope => GetSlope();
        private float GetSlope()
        {
            if (Along.Y == 0.0f) return 0.0f;
            if (Along.X == 0.0f) throw new VerticalLineException();
            return Along.Y / Along.X;
        }

        public Vector NearestToPoint(Vector point)
        {
            return StartPoint + (point - StartPoint).Projection(Along);
        }

        public static (Vector nearestOnA, Vector nearestOnB, bool intersect) NearestPointsOnLines (Line a, Line b)
        {
            if (a.Slope == b.Slope) {
                if(b.StartPoint.Y - a.StartPoint.Y == b.Slope * (b.StartPoint.X - a.StartPoint.X)) {
                    throw new SameLineException();
                } 
                else {
                    throw new ParallelLineException();
                }
            }

            Vector r = b.StartPoint - a.StartPoint;
            Vector dirA = a.Along.Normalized();
            Vector dirB = b.Along.Normalized();

            float dotAA = Vector.Dot(dirA, dirA);
            float dotBB = Vector.Dot(dirB, dirB);
            float dotAB = Vector.Dot(dirA, dirB);
            float dotAR = Vector.Dot(dirA, r);
            float dotBR = Vector.Dot(dirB, r);

            float denominator = dotAA * dotBB - dotAB * dotAB;

            float t = (dotAR * dotBB - dotBR * dotAB) / denominator;
            float s = (dotBR * dotAA - dotAR * dotAB) / denominator;
            
            Vector nearestA = a.StartPoint + dirA * t;
            Vector nearestB = b.StartPoint + dirB * s;

            return (nearestA, nearestB, (nearestB - nearestA).SquareMagnitude() == 0.0f);
        }
        
        public class LineException : Exception { }
        public class ParallelLineException : LineException { }
        public class SameLineException : LineException { }
        public class VerticalLineException : LineException { }
    }
}