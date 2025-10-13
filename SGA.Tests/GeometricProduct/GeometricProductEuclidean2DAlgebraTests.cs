namespace SGA.Tests.GeometricProduct
{
    public class GeometricProductEuclidean2DAlgebraTests
    {
        public GeometricProductEuclidean2DAlgebraTests() => Algebra.Set(2, 0, 0);

        [Fact]
        public void GeometricProduct_VectorSquares_EqualPlusOne()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);

            // Act
            var e1e1 = e1 * e1;
            var e2e2 = e2 * e2;

            // Assert
            Assert.Equal(1.0, e1e1[0], 10);
            Assert.Equal(1.0, e2e2[0], 10);
        }

        [Fact]
        public void GeometricProduct_Vectors_AntiCommute()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);

            // Act
            var e1e2 = e1 * e2;
            var e2e1 = e2 * e1;

            // Assert
            Assert.Equal(1.0, e1e2[3], 10); // e1e2 = +e12
            Assert.Equal(-1.0, e2e1[3], 10); // e2e1 = -e12
        }

        [Fact]
        public void GeometricProduct_BivectorSquares_ToMinusOne()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e12 = e1 * e2;

            // Act
            var e12e12 = e12 * e12;

            // Assert
            Assert.Equal(-1.0, e12e12[0], 10);
        }

        [Fact]
        public void GeometricProduct_VectorReflection()
        {
            // Arrange
            var vector = new Multivector(0, 3.0, 4.0, 0); // 3e1 + 4e2
            var mirror = new Multivector(0, 1.0, 0, 0);   // e1 (espelho no eixo y)

            // Act - Reflexão: v' = -m × v × m
            var reflected = (-mirror) * vector * mirror;

            // Assert - Reflexão no eixo y deve inverter componente x
            Assert.Equal(-3.0, reflected[1], 10); // Componente x invertida
            Assert.Equal(4.0, reflected[2], 10);  // Componente y mantida
        }
    }
}