using System.Collections.Generic;
using System;

namespace BookWyrm.Geometry
{
    public struct Vector
    {
        public readonly float[] Components;

        public float X => this[0];
        public float Y => this[1];
        public float Z => this[2];

        public Vector(params float[] components)
        {
            Components = components;
        }

        public Vector()
        {
            Components = Array.Empty<float>();
        }

        public float this[int index]
        {
            get
            {
                if (index >= Components.Length)
                {
                    return 0.0f;
                }

                return Components[index];
            }
        }

        public int Dimension => GetDimension();
        private int GetDimension()
        {
            int maxNonZeroIndex = -1;
            for (int i = 0; i < Components.Length; i++)
            {
                if (Components[i] != 0.0f) maxNonZeroIndex = i;
            }
            return maxNonZeroIndex + 1;
        }

        public static Vector Add(Vector a, Vector b)
        {
            int maxDimension = a.Dimension > b.Dimension ? a.Dimension : b.Dimension;
            float[] components = new float[maxDimension];

            for (int i = 0; i < maxDimension; i++)
            {
                components[i] = a[i] + b[i];
            }

            return new Vector(components);
        }

        public static Vector MultiplyScalar(Vector v, float scalar)
        {
            float[] components = new float[v.Dimension];
            for (int i = 0; i < v.Dimension; i++)
            {
                components[i] = v[i] * scalar;
            }

            return new Vector(components);
        }

        public static Vector operator +(Vector a, Vector b) => Add(a, b);
        public static Vector operator *(Vector v, float scalar) => MultiplyScalar(v, scalar);
        public static Vector operator *(float scalar, Vector v) => MultiplyScalar(v, scalar);
        public static Vector operator -(Vector v) => -1.0f * v;
        public static Vector operator -(Vector a, Vector b) => a + (-b);
        public static Vector operator /(Vector v, float scalar) => v * (1.0f / scalar);

        public static float Dot(Vector a, Vector b)
        {
            int minDimension = a.Dimension < b.Dimension ? a.Dimension : b.Dimension;

            float sum = 0;
            for (int i = 0; i < minDimension; i++)
            {
                sum += a[i] * b[i];
            }

            return sum;
        }

        public static Vector Scale(Vector a, Vector b)
        {
            int minDimension = a.Dimension < b.Dimension ? a.Dimension : b.Dimension;

            float[] components = new float[minDimension];
            for (int i = 0; i < minDimension; i++)
            {
                components[i] = a[i] * b[i];
            }

            return new Vector(components);
        }

        public static Vector Cross(Vector a, Vector b)
        {
            if (a.Dimension > 3 || b.Dimension > 3) throw new DimensionOutOfBoundsException($"Cross product is limited to at most 3 dimensional vectors. Dimensions are A: {a.Dimension} and B: {b.Dimension}");

            return new Vector(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public class DimensionOutOfBoundsException : Exception { public DimensionOutOfBoundsException(string comment) : base(comment) { } }
        public float Magnitude() => MathF.Sqrt(SquareMagnitude());

        public float SquareMagnitude()
        {
            float sum = 0;
            foreach (float component in Components)
            {
                sum += component * component;
            }

            return sum;
        }

        public Vector Normalized() => this / Magnitude();

        public Vector Projection(Vector onto)
        {
            return Dot(this, onto) * onto / onto.SquareMagnitude();
        }

        public Vector Rejection(Vector onto)
        {
            return this - Projection(onto);
        }

        public Vector Reflection(Vector over)
        {
            return this - 2 * Rejection(over);
        }

        public Vector Rotated(float angle, Vector normal)
        {
            Vector Vn = Rejection(normal);
            Vector Pn = Cross(normal.Normalized(), Vn);
            Vector Rn = MathF.Cos(angle) * Vn + MathF.Sin(angle) * Pn;
            return Rn + Projection(normal);
        }
        public Vector Rotated(Vector scaledNormal) => Rotated(scaledNormal.Magnitude(), scaledNormal);

        public Vector Rotated2D(float angle) => Rotated(angle, new Vector(0, 0, 1));

        public static Vector Unit(int dimension)
        {
            float[] components = new float[dimension];
            for (int i = 0; i < dimension - 1; i++)
            {
                components[i] = 0.0f;
            }
            components[dimension - 1] = 1.0f;

            return new Vector(components);
        }

        public static Vector One(int dimension)
        {
            float[] components = new float[dimension];
            for (int i = 0; i < dimension; i++)
            {
                components[i] = 1.0f;
            }

            return new Vector(components);
        }

        public static Vector Zero => new Vector();

        public static Vector Right => new Vector(1);
        public static Vector Left => new Vector(-1);
        public static Vector Up => new Vector(0, 1);
        public static Vector Down => new Vector(0, -1);

        private const float EQUIVALENCE_RANGE = 1e-6f;
        public override bool Equals(object? obj)
        {
            if (obj is Vector v)
            {
                return (this - v).SquareMagnitude() < EQUIVALENCE_RANGE * EQUIVALENCE_RANGE;
            }

            return false;
        }
        public static bool operator ==(Vector a, Vector b) => a.Equals(b);
        public static bool operator !=(Vector a, Vector b) => !a.Equals(b);

        public override int GetHashCode() => Components.GetHashCode();

        public override string ToString()
        {
            string result = "(";
            for (int i = 0; i < Dimension; i++)
            {
                result += Components[i];
                if (i < Dimension - 1) result += ", ";
            }
            result += ")";
            return result;
        }
    }
}