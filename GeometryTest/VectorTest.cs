namespace GeometryTest;
using BookWyrm.Geometry;

[TestClass]
public class UnitTest1
{
    /// <summary> 1, 2, 3 </summary>
    public static Vector A = new Vector(1, 2, 3);

    /// <summary>
    /// -1, 2, -3
    /// </summary>
    public static Vector B = new Vector(-1, 2, -3);


    /// <summary>
    /// 3, 4
    /// </summary>
    public static Vector C = new Vector(3, 4);

    /// <summary>
    /// 3, 4, 5
    /// </summary>
    public static Vector E = new Vector(3, 4, 5);

    /// <summary>
    /// 2, -4
    /// </summary>
    public static Vector D = new Vector(2, -4);

    [TestMethod]
    public void ComponentTest()
    {
        TestByComponents(A, 1, 2, 3, 0);
        TestByComponents(B, -1, 2, -3);
        TestByComponents(C, 3, 4, 0);
        TestByComponents(D, 2, -4, 0);
    }

    [TestMethod]
    public void DimensionTest()
    {
        Assert.AreEqual(3, A.Dimension);
        Assert.AreEqual(3, B.Dimension);
        Assert.AreEqual(2, C.Dimension);
        Assert.AreEqual(2, D.Dimension);
    }

    [TestMethod]
    public void MagnitudeTest()
    {
        Assert.AreEqual(MathF.Sqrt(14), A.Magnitude());
        Assert.AreEqual(MathF.Sqrt(14), B.Magnitude());
        Assert.AreEqual(5, C.Magnitude());
        Assert.AreEqual(MathF.Sqrt(20), D.Magnitude());
    }

    [TestMethod]
    public void AdditionTest()
    {
        var sum = A + B;
        TestByComponents(sum, 0, 4, 0);

        sum = C + D;
        TestByComponents(sum, 5, 0);

        sum = A + C;
        TestByComponents(sum, 4, 6, 3);

        sum = B + D;
        TestByComponents(sum, 1, -2, -3);
    }

    [TestMethod]
    public void SubtractionTest()
    {
        var diff = A - B;
        TestByComponents(diff, 2, 0, 6);

        diff = C - D;
        TestByComponents(diff, 1, 8);

        diff = A - C;
        TestByComponents(diff, -2, -2, 3);

        diff = B - D;
        TestByComponents(diff, -3, 6, -3);
    }

    [TestMethod]
    public void MultiplicationTest()
    {
        var product = A * 2;
        TestByComponents(product, 2, 4, 6);

        product = C * 3;
        TestByComponents(product, 9, 12);

        product = A * 0;
        TestByComponents(product, 0, 0, 0);

        product = B * -1;
        TestByComponents(product, 1, -2, 3);
    }

    [TestMethod]
    public void DivisionTest()
    {
        var quotient = A / 2;
        TestByComponents(quotient, 0.5f, 1, 1.5f);

        quotient = C / 3;
        TestByComponents(quotient, 1, 4f / 3);

        quotient = A / 1;
        TestByComponents(quotient, 1, 2, 3);

        quotient = B / -1;
        TestByComponents(quotient, 1, -2, 3);
    }

    [TestMethod]
    public void DotProductTest()
    {
        var dot = Vector.Dot(A, B);
        Assert.AreEqual(-6, dot);

        dot = Vector.Dot(C, D);
        Assert.AreEqual(-10, dot);

        dot = Vector.Dot(A, C);
        Assert.AreEqual(11, dot);

        dot = Vector.Dot(B, D);
        Assert.AreEqual(-10, dot);
    }

    [TestMethod]
    public void ScaleTest()
    {
        var scaled = Vector.Scale(A, B);
        TestByComponents(scaled, -1, 4, -9);

        scaled = Vector.Scale(C, D);
        TestByComponents(scaled, 6, -16);

        scaled = Vector.Scale(A, C);
        TestByComponents(scaled, 3, 8);

        scaled = Vector.Scale(B, D);
        TestByComponents(scaled, -2, -8);
    }

    [TestMethod]
    public void ReflectionTest()
    {

    }

    [TestMethod]
    public void ProjectionTest()
    {
        TestByComponents(A.Projection(B), 3.0f / 7, -6.0f / 7, 9.0f / 7);
        TestByComponents(C.Projection(D), -1, 2);
    }

    [TestMethod]
    public void RejectionTest()
    {
        TestByComponents(A.Rejection(B), 4.0f / 7, 20.0f / 7, 12.0f / 7);
        TestByComponents(C.Rejection(D), 4, 2);
    }

    [TestMethod]
    public void ProjectionRejectionTest()
    {
        var projection = A.Projection(B);
        var rejection = A.Rejection(B);
        var dot = Vector.Dot(projection, rejection);

        Assert.IsTrue((dot < 0 ? -dot : dot) < 1e-6);

        projection = C.Projection(D);
        rejection = C.Rejection(D);
        dot = Vector.Dot(projection, rejection);

        Assert.IsTrue((dot < 0 ? -dot : dot) < 1e-6);
    }

    [TestMethod]
    public void Rotation2DTest()
    {
        // Test <3, 4> rotation about unit circle
        float degreeToRadian = MathF.PI / 180.0f;
        float root3 = MathF.Sqrt(3);
        float root2 = MathF.Sqrt(2);
        Vector rotated = C.Rotated2D(degreeToRadian * 30.0f);
        Vector expected = new Vector(3 * root3 - 4, 3 + 4 * root3) / 2.0f;
        TestByComparison(rotated, expected);

        rotated = C.Rotated2D(degreeToRadian * 45.0f);
        expected = new Vector(3 * root2 - 4 * root2, 3 * root2 + 4 * root2) / 2.0f;
        TestByComparison(rotated, expected);

        rotated = C.Rotated2D(degreeToRadian * 60.0f);
        expected = new Vector(3 - 4 * root3, 3 * root3 + 4) / 2.0f;
        TestByComparison(rotated, expected);

        rotated = C.Rotated2D(degreeToRadian * 90.0f);
        expected = new Vector(-4, 3);
        TestByComparison(rotated, expected);

        rotated = C.Rotated2D(degreeToRadian * 120.0f);
        expected = new Vector(-3 - 4 * root3, 3 * root3 - 4) / 2.0f;
        TestByComparison(rotated, expected);

        rotated = C.Rotated2D(degreeToRadian * -30.0f);
        expected = new Vector(3 * root3 + 4, -3 + 4 * root3) / 2.0f;
        TestByComparison(rotated, expected);
    }

    [TestMethod]
    public void Rotation3DTestXY()
    {
        float degreeToRadian = MathF.PI / 180.0f;
        float root3 = MathF.Sqrt(3);
        float root2 = MathF.Sqrt(2);

        Vector rotated = E.Rotated2D(degreeToRadian * 30.0f);
        Vector expected = new Vector(3 * root3 - 4, 3 + 4 * root3, 10.0f) / 2.0f;
        TestByComparison(rotated, expected);

        rotated = E.Rotated2D(degreeToRadian * 45.0f);
        expected = new Vector(3 * root2 - 4 * root2, 3 * root2 + 4 * root2, 10.0f) / 2.0f;
        TestByComparison(rotated, expected);

        rotated = E.Rotated2D(degreeToRadian * 60.0f);
        expected = new Vector(3 - 4 * root3, 3 * root3 + 4, 10.0f) / 2.0f;
        TestByComparison(rotated, expected);

        rotated = E.Rotated2D(degreeToRadian * 90.0f);
        expected = new Vector(-4, 3, 5);
        TestByComparison(rotated, expected);

        rotated = E.Rotated2D(degreeToRadian * 120.0f);
        expected = new Vector(-3 - 4 * root3, 3 * root3 - 4, 10.0f) / 2.0f;
        TestByComparison(rotated, expected);

        rotated = E.Rotated2D(degreeToRadian * -30.0f);
        expected = new Vector(3 * root3 + 4, -3 + 4 * root3, 10.0f) / 2.0f;
        TestByComparison(rotated, expected);
    }

    [TestMethod]
    public void Rotation3DTestAbout()
    {
        float degreeToRadian = MathF.PI / 180.0f;
        float root3 = MathF.Sqrt(3);
        float root2 = MathF.Sqrt(2);
        float root7 = MathF.Sqrt(7);
        float root14 = MathF.Sqrt(14);

        Vector projection = A.Projection(B);

        Vector rotated = A.Rotated(degreeToRadian * 0.0f, B);
        Vector expected = A;
        TestByComparison(rotated, expected);

        rotated = A.Rotated(degreeToRadian * 30.0f, B);
        expected = new Vector(2.0f * root3 / 7.0f, 10.0f * root3 / 7.0f, 6.0f * root3 / 7.0f) + new Vector(6 / root14, 0, -2 / root14) + projection;
        //TestByComparison(rotated, expected);

        rotated = A.Rotated(degreeToRadian * 45.0f, B);
        expected = new Vector(2 * root2 / 7.0f, 10 * root2 / 7.0f, 6 * root2 / 7.0f) + new Vector(6 / root7, 0, -2 / root7) + projection;
        TestByComparison(rotated, expected);
        
        rotated = A.Rotated(degreeToRadian * 60.0f, B);
        expected = new Vector(2 / 7.0f, 10 / 7.0f, 6 / 7.0f) + new Vector(6  * root3 / root14, 0, -2 * root3 / root14) + projection;
        TestByComparison(rotated, expected);

        rotated = A.Rotated(degreeToRadian * 90.0f, B);
        expected = new Vector(12 / root14, 0, -4 / root14) + projection;
        TestByComparison(rotated, expected);

        rotated = A.Rotated(degreeToRadian * 120.0f, B);
        expected = new Vector(-2 / 7.0f, -10 / 7.0f, -6 / 7.0f) + new Vector(6 * root3 / root14, 0, -2 * root3 / root14) + projection;
        TestByComparison(rotated, expected);

        rotated = A.Rotated(degreeToRadian * -30.0f, B);
        expected = new Vector(2 * root3 / 7.0f, 10 * root3 / 7.0f, 6 * root3 / 7.0f) + new Vector(-6 / root14, 0, 2 / root14) + projection;
        TestByComparison(rotated, expected);
    }

    public void TestByComponents(Vector vector, params float[] components)
    {
        for (int i = 0; i < components.Length; i++)
        {
            var diff = vector[i] - components[i];

            Assert.IsTrue((diff < 0 ? -diff : diff) < 1e-6, $"{vector}[i]: Expected {components[i]} but got {vector[i]} (diff = {diff})");
        }
    }

    public void TestByComparison(Vector given, Vector expected)
    {
        Assert.IsTrue((given - expected).SquareMagnitude() <= 1e-12, $"Difference between Given({given}) and Expected({expected}) is too large: {(given - expected).Magnitude()}");
    }
}