namespace BookWyrm.Math;
using BookWyrm.Geometry;
using System;


/// <summary>
/// Contains a collection of methods for interpolating between data-types.
/// </summary>
public class Interpolate
{
    /// <summary>
    /// A generic interpolation method that can be used to interpolate between two values of any type.
    /// </summary>
    /// <param name="a">Starting value</param>
    /// <param name="b">Ending value</param>
    /// <param name="t">Interpolation parameter</param>
    /// <param name="add">A function to add two values</param>
    /// <param name="invert">A function to invert a value</param>
    /// <param name="scale">A function to scale a value by a scalar</param>
    /// <param name="upperBounded">If true, t will be bounded to at most 1.0. </param>
    /// <param name="lowerBounded">If true, t will be bounded to at least 0.0. </param>
    private static T Lerp<T>(T a, T b, float t, Func<T, T, T> add, Func<T, T> invert, Func<T, float, T> scale, bool upperBounded, bool lowerBounded)
    {
        return add(a, scale(add(b, invert(a)),t < 0.0f && lowerBounded ? 0.0f : t > 1.0f && upperBounded ? 1.0f : t));
    }

    /// <summary>
    /// Linearly interpolates between two vectors
    /// </summary>
    /// <param name="a">Starting vector</param>
    /// <param name="b">Ending Vector</param>
    /// <param name="t">Interpolation value</param>
    /// <param name="upperBounded">If true, t will be bounded to at most 1.0. </param>
    /// <param name="lowerBounded">If true, t will be bounded to at least 0.0. </param>
    public static Vector Lerp(Vector a, Vector b, float t, bool upperBounded = false, bool lowerBounded = false) 
        => Lerp(a, b, t, (a, b) => a + b, (a) => -a, (a, c) => a * c, upperBounded, lowerBounded);

    /// <summary>
    /// Linearly interpolates between two floats.
    /// </summary>
    /// <param name="a">Starting value</param>
    /// <param name="b">Ending value</param>
    /// <param name="t">Interpolation value</param>
    /// <param name="upperBounded">If true, t will be bounded to at most 1.0. </param>
    /// <param name="lowerBounded">If true, t will be bounded to at least 0.0. </param>
    public static float Lerp(float a, float b, float t, bool upperBounded = false, bool lowerBounded = false) 
        => Lerp(a, b, t, (a, b) => a + b, (a) => -a, (a, c) => a * c, upperBounded, lowerBounded);
}