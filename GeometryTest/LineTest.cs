using BookWyrm.Geometry;

namespace GeometryTest;

[TestClass]
public class LineTest
{
    [TestMethod]
    public void IntersectionTest()
    {
        Line A = Line.PointDirection(new Vector(1, 2, 3), new Vector(1, 2, 3));
        Line B = Line.PointDirection(new Vector(-2, 1, 2), new Vector(-2, 1, 2));

        Line C = Line.PointDirection(new Vector(2, 2, 1), new Vector(2, 2, 1)); // Parallel
        Line D = Line.PointDirection(new Vector(2, 2, 2), new Vector(2, 2, 1)); // Parallel

        Line V = Line.PointDirection(new Vector(0, 1, 0), new Vector(0, 1, 0));

        Line E = Line.PointDirection(new Vector(1, 2, -2), new Vector(1, 2, -2)); // Parallel in 2D
        Line F = Line.PointDirection(new Vector(1, 2, 3), new Vector(1, 2, 3)); // Parallel in 2D

        Line G = Line.PointDirection(new Vector(3, 0, 4), new Vector(2, -1, 3));
        Line H = Line.PointDirection(new Vector(3, 3, 5), new Vector(2, 2, 4));

        // Two intersecting lines give correct intersection point
        // Two intersecting lines correctly identify if an intersection has occured
        bool intersect = Line.Intersect(A, B, out Vector intersectionPoint);
        Assert.IsTrue(intersect);
        float delta = (intersectionPoint - new Vector()).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);

        // Non Zero intersection
        intersect = Line.Intersect(G, H, out intersectionPoint);
        Assert.IsTrue(intersect);
        delta = (intersectionPoint - new Vector(1, 1, 1)).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);

        // Two parallel lines throw correct exception
        Assert.ThrowsException<Line.ParallelLineException>(() => Line.Intersect(C, D, out intersectionPoint));
        Assert.ThrowsException<Line.SameLineException>(() => Line.Intersect(A, A, out intersectionPoint));

        // Works with vertical lines
        intersect = Line.Intersect(A, V, out intersectionPoint);
        Assert.IsTrue(intersect);
        delta = (intersectionPoint - new Vector()).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);

        intersect = Line.Intersect(V, A, out intersectionPoint);
        Assert.IsTrue(intersect, $"{intersectionPoint}");
        delta = (intersectionPoint - new Vector()).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);

        // Works with 2D parallel lines
        intersect = Line.Intersect(E, F, out intersectionPoint);
        Assert.IsTrue(intersect);
        delta = (intersectionPoint - new Vector()).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);
    }

    [TestMethod]
    public void NearestPointTest()
    {
        Line A = Line.PointDirection(new Vector(-1, -3, 6), new Vector(-2, -4, 4));
        Line B = Line.PointDirection(new Vector(9, -5, -5), new Vector(-5, 2, 2));

        Vector expectedA = new Vector(1, 1, 2);
        Vector expectedB = new Vector(-1, -1, -1);
        bool expectedIntersect = false;

        TestNearest(A, B, expectedA, expectedB, expectedIntersect);

        A = Line.PointDirection(new Vector(-12, -18, -9, 11), new Vector(3, 4, 3, -1));
        B = Line.PointDirection(new Vector(2, -3, -6, -2), new Vector(2, 3, 0, 2));

        expectedA = new Vector(0, -2, 3, 7);
        expectedB = new Vector(4, 0, -6, 0);
        expectedIntersect = false;

        TestNearest(A, B, expectedA, expectedB, expectedIntersect);

        A = Line.PointDirection(new Vector(-6, 1, -2, 4), new Vector(5, 1, -1, 0));
        B = Line.PointDirection(new Vector(-6, 11, 1, -4), new Vector(-3, 5, 0, -2));

        expectedA = new Vector(-1, 2, -3, 4);
        expectedB = new Vector(0, 1, 1, 0);
        expectedIntersect = false;

        TestNearest(A, B, expectedA, expectedB, expectedIntersect);

        A = Line.PointDirection(new Vector(-11, 20, -6, -2), new Vector(3, -6, 2, 1));
        B = Line.PointDirection(new Vector(-4, 0, 4, 1), new Vector(-1, -1, 2, 0));

        expectedA = new Vector(-2, 2, 0, 1);
        expectedB = new Vector(-2, 2, 0, 1);
        expectedIntersect = true;

        TestNearest(A, B, expectedA, expectedB, expectedIntersect);

        A = Line.PointDirection(new Vector(1, 2, 3), new Vector(1, -1, 2));
        B = Line.PointDirection(new Vector(-1, 2, 2), new Vector(2, -2, 4));

        Assert.ThrowsException<Line.ParallelLineException>(() => Line.NearestPoint(A, B, out var nearestA, out var nearestB));

        A = Line.PointDirection(new Vector(2, -2, 3), new Vector(-2, 1, -2));
        B = Line.PointDirection(new Vector(-2, 0, -1), new Vector(4, -2, 4));

        Assert.ThrowsException<Line.SameLineException>(() => Line.NearestPoint(A, B, out var nearestA, out var nearestB));
    }

    public void TestNearest(Line A, Line B, Vector expectedA, Vector expectedB, bool expectedIntersect)
    {
        Line.NearestPoint(A, B, out var nearestToA, out var nearestToB);
        TestByComparison(nearestToA, expectedA);
        TestByComparison(nearestToB, expectedB);
        Assert.AreEqual(expectedIntersect, (nearestToA - nearestToB).SquareMagnitude() < 1e-12, $"Expected Intersection: {expectedIntersect}, Instead: {(nearestToA - nearestToB).SquareMagnitude() < 1e-12}");
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
