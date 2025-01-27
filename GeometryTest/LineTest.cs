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
        bool intersect = Line.Intersects(A, B, out Vector intersectionPoint);
        Assert.IsTrue(intersect);
        float delta = (intersectionPoint - new Vector()).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);

        // Non Zero intersection
        intersect = Line.Intersects(G, H, out intersectionPoint);
        Assert.IsTrue(intersect);
        delta = (intersectionPoint - new Vector(1, 1, 1)).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);

        // Two parallel lines throw correct exception
        Assert.ThrowsException<Line.ParallelLineException>(() => Line.Intersects(C, D, out intersectionPoint));
        Assert.ThrowsException<Line.SameLineException>(() => Line.Intersects(A, A, out intersectionPoint));

        // Works with vertical lines
        intersect = Line.Intersects(A, V, out intersectionPoint);
        Assert.IsTrue(intersect);
        delta = (intersectionPoint - new Vector()).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);

        intersect = Line.Intersects(V, A, out intersectionPoint);
        Assert.IsTrue(intersect, $"{intersectionPoint}");
        delta = (intersectionPoint - new Vector()).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);

        // Works with 2D parallel lines
        intersect = Line.Intersects(E, F, out intersectionPoint);
        Assert.IsTrue(intersect);
        delta = (intersectionPoint - new Vector()).SquareMagnitude();
        Assert.IsTrue(delta < 1e-12);
    }

    public void TestByComponents(Vector vector, params float[] components)
    {
        for (int i = 0; i < components.Length; i++)
        {
            var diff = vector[i] - components[i];

            Assert.IsTrue((diff < 0 ? -diff : diff) < 1e-6, $"{vector}[i]: Expected {components[i]} but got {vector[i]} (diff = {diff})");
        }
    }
}
