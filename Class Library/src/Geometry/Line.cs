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

        public static bool Intersects(Line a, Line b, out Vector intersectionPoint, int dimensionX = 0, int dimensionY = 1)
        {
            Vector pointA = a.StartPoint;
            Vector pointB = b.StartPoint;
            Vector directionA = a.Along;
            Vector directionB = b.Along;
            Vector delta = pointA - pointB;

            float directionDot = Vector.Dot(directionA, directionB);
            float directionDelta = (directionDot < 0 ? -directionDot : directionDot) - directionA.Magnitude() * directionB.Magnitude();
            // Determine parallel or same line
            if (directionDelta > -1e-6 && directionDelta < 1e-6)
            {
                if (a.ContainsPoint(pointB)) throw new SameLineException();
                else throw new ParallelLineException();
            }

            Vector dirA2D = new Vector(directionA[dimensionX], directionA[dimensionY]);
            Vector dirB2D = new Vector(directionB[dimensionX], directionB[dimensionY]);
            int maxDimension = a.Dimension > b.Dimension ? a.Dimension : b.Dimension;

            while (Vector.Cross(dirA2D, dirB2D).SquareMagnitude() < 1e-12)
            {
                dimensionY++;
                if (dimensionY >= maxDimension)
                {
                    dimensionY = ++dimensionX + 1;
                }
                if(dimensionX >= maxDimension) throw new DimensionErrorException();

                dirA2D = new Vector(directionA[dimensionX], directionA[dimensionY]);
                dirB2D = new Vector(directionB[dimensionX], directionB[dimensionY]);
            }

            // B is vertical, swap A and B
            if (dirB2D.X > -1e-6 && dirB2D.X < 1e-6)
            {
                pointA = b.StartPoint;
                pointB = a.StartPoint;

                directionA = b.Along;
                directionB = a.Along;

                delta = -delta;

                Console.WriteLine("Swap A-B");
            }

            float t = (directionB[dimensionY] * delta[dimensionX] - directionB[dimensionX] * delta[dimensionY]) / (directionB[dimensionX] * directionA[dimensionY] - directionB[dimensionY] * directionA[dimensionX]);
            float s = (delta[dimensionX] + directionA[dimensionX] * t) / directionB[dimensionX];

            Console.WriteLine($"T: {t}, S: {s}\n");

            Vector intersectA = pointA + directionA * t;
            Vector intersectB = pointB + directionB * s;

            intersectionPoint = intersectA;

            return (intersectA - intersectB).SquareMagnitude() < 1e-12;
        }

        public static (Vector nearestToA, Vector nearestToB, bool intersect) NearestPoint(Line a, Line b) {
            Vector R = a.StartPoint - b.StartPoint;
            
            float RA = Vector.Dot(R, a.Along);
            float RB = Vector.Dot(R, b.Along);
            float AB = Vector.Dot(a.Along, b.Along);
            float AA = Vector.Dot(a.Along, a.Along);
            float BB = Vector.Dot(b.Along, b.Along);

            float denom = AA * BB - AB * AB;

            if(denom == 0 || AA == 0) {
                // Parallel or same?
            }

            float t = (RB * AB + RA * BB) / denom;
            float s = (RA + AA * t) / AB;

            Vector nearestToA = a.StartPoint + a.Along * t;
            Vector nearestToB = b.StartPoint + b.Along * s;

            return (nearestToA, nearestToB, (nearestToA - nearestToB).SquareMagnitude() < 1e-12);
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