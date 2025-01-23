namespace GeometryTest;
using BookWyrm.Geometry;

[TestClass]
public class VectorTest
{
    public static Vector A = new Vector(3, 4);
    public static Vector B = new Vector(-1, 2);
    public static Vector C = new Vector(2, 1);

    [TestMethod] 
    public void TestComponents() {
        ComponentTest(A, 3, 4, 0, 0, 0);
        ComponentTest(B, -1, 2);
        ComponentTest(C, 2, 1);

        Assert.ThrowsException<IndexOutOfRangeException>(() => { float x = A[-1]; });
    }

    [TestMethod]
    public void TestValues() {
        Assert.AreEqual(A.Dimension, 2);
        Assert.AreEqual(A.Magnitude(), 5);
        Assert.AreEqual(A.SquareMagnitude(), 25);
        ComponentTest(A.Normalized(), 3.0f/5, 4.0f/5);
        //ComponentTest(A.Rotated2D(MathF.PI / 2), -4, 3);
    }

    public void TestConstants() {
        Vector a = new Vector(3, 4);
        Vector b = new Vector(-1, 2);
        Vector c = new Vector(2, 1);
    }

    public void ComponentTest(Vector v, params float[] components) {
        for(int i = 0; i < components.Length; i++) {
            Assert.AreEqual(v[i], components[i]);
        }
    }
}
