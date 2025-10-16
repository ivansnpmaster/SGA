namespace SGA.Tests.GeometricProduct
{
    public class GeometricProductRealAlgebraTests
    {
        public GeometricProductRealAlgebraTests() => Algebra.Set(0, 0, 0);

        [Fact]
        public void GeometricProduct_RealNumbers_BehavesAsRealMultiplication()
        {
            // Arrange
            var a = new Multivector(3.0); // Number 3
            var b = new Multivector(2.0); // Number 2

            // Act
            var product = a * b;

            // Assert
            Assert.Equal(6.0, product[0], 10);
            Assert.True(product.IsScalar());
        }

        [Fact]
        public void GeometricProduct_RealNumbers_IsCommutative()
        {
            // Arrange
            var a = new Multivector(4.0);
            var b = new Multivector(5.0);

            // Act
            var ab = a * b;
            var ba = b * a;

            // Assert
            Assert.Equal(ab[0], ba[0], 10);
            Assert.Equal(20.0, ab[0], 10);
        }

        [Fact]
        public void GeometricProduct_RealNumbers_HasIdentityElement()
        {
            // Arrange
            var a = new Multivector(7.0);
            var identity = new Multivector(1.0);

            // Act
            var result = a * identity;

            // Assert
            Assert.Equal(7.0, result[0], 10);
            Assert.True(a.Equals(result));
        }

        [Fact]
        public void GeometricProduct_RealNumbers_HasZeroElement()
        {
            // Arrange
            var a = new Multivector(7.0);
            var zero = new Multivector(0.0);

            // Act
            var result = a * zero;

            // Assert
            Assert.Equal(0.0, result[0], 10);
            Assert.True(result.IsScalar());
        }

        [Fact]
        public void GeometricProduct_RealNumbers_IsAssociative()
        {
            // Arrange
            var a = new Multivector(2.0);
            var b = new Multivector(3.0);
            var c = new Multivector(4.0);

            // Act
            var leftAssociative = a * b * c;
            var rightAssociative = a * (b * c);

            // Assert
            Assert.Equal(leftAssociative[0], rightAssociative[0], 10);
            Assert.Equal(24.0, leftAssociative[0], 10);
        }

        [Fact]
        public void GeometricProduct_RealNumbers_DistributiveOverAddition()
        {
            // Arrange
            var a = new Multivector(2.0);
            var b = new Multivector(3.0);
            var c = new Multivector(4.0);

            // Act
            var leftDist = a * (b + c);
            var rightDist = a * b + a * c;

            // Assert
            Assert.Equal(leftDist[0], rightDist[0], 10);
            Assert.Equal(14.0, leftDist[0], 10); // 2*(3+4) = 14
        }
    }
}