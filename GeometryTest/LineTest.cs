using BookWyrm.Geometry;

namespace GeometryTest;

[TestClass]
public class LineTest
{
    [TestMethod]
    public void IntersectionTest() {
        Line a = Line.PointDirection(new Vector(-2, -1, 0), new Vector(1, 1, 1));
        Line b = Line.PointDirection(new Vector(8, -6, -11), new Vector(-2, 3, 5));

        

        
    }

    public void TestByComponents(Vector vector, params float[] components) {
        for(int i = 0; i < components.Length; i++) {
            var diff = vector[i] - components[i];

            Assert.IsTrue((diff < 0 ? -diff : diff) < 1e-6, $"{vector}[i]: Expected {components[i]} but got {vector[i]} (diff = {diff})");
        }
    }
}
