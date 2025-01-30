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

        public bool ContainsPoint(Vector point)
        {
            var startDelta = point - StartPoint;
            var endDelta = point - EndPoint;

            return Vector.Dot(startDelta, Along) >= 0.0f && Vector.Dot(endDelta, Along) <= 0.0f && startDelta.Rejection(Along).SquareMagnitude() < 1e-12;
        }


        public static bool Intersect(Line a, Line b, out Vector intersectionPoint)
        {
            NearestPoint(a, b, out var nearestToA, out var nearestToB);
            intersectionPoint = nearestToA;
            return (nearestToA - nearestToB).SquareMagnitude() < 1e-12;
        }

        public static (bool withinA, bool withinB) IntersectWithin(Line a, Line b) {
            if(Intersect(a, b, out Vector point)) {
                return (a.ContainsPoint(point), b.ContainsPoint(point));
            }

            return (false, false);
        }

        public static (bool withinA, bool withinB) NearestWithin(Line a, Line b) {
            NearestPoint(a, b, out Vector nearestToA, out Vector nearestToB);

            return (a.ContainsPoint(nearestToA), b.ContainsPoint(nearestToB));
        }

        public static Vector NearestPoint(Line line, Vector point) => line.StartPoint + (point - line.StartPoint).Projection(line.Along);

        public static void NearestPoint(Line a, Line b, out Vector nearestToA, out Vector nearestToB)
        {
            Vector R = a.StartPoint - b.StartPoint;

            float RA = Vector.Dot(R, a.Along);
            float RB = Vector.Dot(R, b.Along);
            float AB = Vector.Dot(a.Along, b.Along);
            float AA = Vector.Dot(a.Along, a.Along);
            float BB = Vector.Dot(b.Along, b.Along);
            float determinate = AB * AB - AA * BB;

            if (determinate == 0)
            {
                if (Vector.Cross(a.Along, b.StartPoint - a.StartPoint).SquareMagnitude() < 1e-12) throw new SameLineException();
                else throw new ParallelLineException();
            }

            float t = (BB * RA - AB * RB) / determinate;
            float s = (AB * RA - AA * RB) / determinate;

            nearestToA = a.StartPoint + a.Along * t;
            nearestToB = b.StartPoint + b.Along * s;
        }

        public class LineException : Exception { }

        public class ParallelLineException : LineException { }
        public class SameLineException : LineException { }
        public class VerticalLineException : LineException { }

        public static Line PointDirection(Vector point, Vector direction) => new Line(point, point + direction);
        public static Line PointDirection(Vector point, Vector direction, float distance) => PointDirection(point, direction.Normalized() * distance);
        public static Line StandardForm(float A, float B, float C)
        {
            if (B == 0.0f) return new Line(new Vector(-C / A, 0.0f), new Vector(-C / A, 1.0f));
            else if (A == 0.0f) return new Line(new Vector(0.0f, -C / B), new Vector(1.0f, -C / B));
            else return new Line(new Vector(0.0f, -C / B), new Vector(1.0f, (-C - A) / B));
        }
    }
}