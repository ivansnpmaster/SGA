namespace SGA.Tests.GeometricProduct
{
    public class GeometricProductEuclidean3DAlgebraTests
    {
        public GeometricProductEuclidean3DAlgebraTests() => Algebra.Set(3, 0, 0);

        [Fact]
        public void GeometricProduct_VectorSquares_EqualPlusOne()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);

            // Act & Assert
            Assert.Equal(1.0, (e1 * e1)[0], 10);
            Assert.Equal(1.0, (e2 * e2)[0], 10);
            Assert.Equal(1.0, (e3 * e3)[0], 10);
        }

        [Fact]
        public void GeometricProduct_Vectors_AntiCommute()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);

            // Act & Assert
            Assert.Equal(-1.0, (e2 * e1)[3], 10); // e2e1 = -e1e2
            Assert.Equal(-1.0, (e3 * e1)[5], 10); // e3e1 = -e1e3
            Assert.Equal(-1.0, (e3 * e2)[6], 10); // e3e2 = -e2e3
        }
    }
}