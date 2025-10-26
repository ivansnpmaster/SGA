namespace SimGA.Tests.WedgeProduct
{
    public class WedgeProductEuclidean2DAlgebraTests
    {
        public WedgeProductEuclidean2DAlgebraTests() => Algebra.Set(2, 0, 0);

        [Fact]
        public void WedgeProduct_BasisVectors_CreatesBivector()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);

            // Act
            var e1e2 = e1 ^ e2;
            var pureBivector = e1e2[0] == 0.0 && e1e2[1] == 0.0 && e1e2[2] == 0.0 && e1e2[3] != 0.0;

            // Assert
            Assert.Equal(1.0, e1e2[3], 10); // e12
            Assert.True(pureBivector);
        }

        [Fact]
        public void WedgeProduct_AntiCommutativeProperty_HoldsForAllVectors()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);

            // Act
            var e1e2 = e1 ^ e2;
            var e2e1 = e2 ^ e1;

            // Assert
            Assert.Equal(-e2e1, e1e2);
        }

        [Fact]
        public void WedgeProduct_NilpotentProperty_VectorsSquareToZero()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);

            // Act & Assert
            Assert.True((e1 ^ e1).IsZero());
            Assert.True((e2 ^ e2).IsZero());
        }

        [Fact]
        public void WedgeProduct_ScalarMultiplication_CommutativeWithVectors()
        {
            // Arrange
            var scalar = new Multivector(3.0);
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);

            // Act
            var left = scalar ^ (e1 ^ e2);
            var right = (scalar ^ e1) ^ e2;
            var right2 = e1 ^ (scalar ^ e2);

            // Assert
            Assert.Equal(left, right);
            Assert.Equal(left, right2);
            Assert.Equal(3.0, left[3], 10); // 3 * e12
        }

        [Fact]
        public void WedgeProduct_LinearlyDependentVectors_ReturnsZero()
        {
            // Arrange
            var v1 = new Multivector(0.0, 1.0, 2.0); // e1 + 2e2
            var v2 = new Multivector(0.0, 2.0, 4.0); // 2e1 + 4e2
            var v3 = new Multivector(0.0, 3.0, 6.0); // 3e1 + 6e2

            // Act & Assert
            Assert.True((v1 ^ v2).IsZero());
            Assert.True((v2 ^ v3).IsZero());
            Assert.True((v1 ^ v3).IsZero());
        }

        [Fact]
        public void WedgeProduct_AreaComputation_CorrectForParallelogram()
        {
            // Arrange
            var v1 = new Multivector(0.0, 3.0); // Base = 3
            var v2 = new Multivector(0.0, 1.0, 4.0); // Height = 4

            // Act
            var areaBivector = v1 ^ v2;

            // Assert - area = 3*4 = 12
            Assert.Equal(12.0, areaBivector[3], 10);
        }

        [Fact]
        public void WedgeProduct_DistributiveProperty_Holds()
        {
            // Arrange
            var a = new Multivector(0.0, 1.0, 2.0);
            var b = new Multivector(0.0, 3.0, 4.0);
            var c = new Multivector(0.0, 5.0, 6.0);

            // Act
            var left = a ^ (b + c);
            var right = (a ^ b) + (a ^ c);

            // Assert
            Assert.Equal(left, right);
        }

        [Fact]
        public void WedgeProduct_AssociativeProperty_HoldsForThreeVectors()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);

            // Act
            var doubleWedge = (e1 ^ e2) ^ e1; // Should be zero
            var alternative = e1 ^ (e2 ^ e1); // Should be zero

            // Assert
            Assert.True(doubleWedge.IsZero());
            Assert.True(alternative.IsZero());
            Assert.Equal(doubleWedge, alternative);
        }

        [Fact]
        public void WedgeProduct_MixedGradeOperations_BehaveCorrectly()
        {
            // Arrange
            var scalar = new Multivector(2.0);
            var vector = new Multivector(0.0, 3.0);
            var bivector = new Multivector(0.0, 0.0, 0.0, 4.0);

            // Act & Assert
            Assert.True((vector ^ bivector).IsZero()); // Vector & bivector with common basis
            Assert.Equal(6.0, (scalar ^ vector)[1], 10); // Scalar times vector
            Assert.Equal(8.0, (scalar ^ bivector)[3], 10); // Scalar times bivector
        }

        [Fact]
        public void WedgeProduct_ZeroVector_AnnihilatesAll()
        {
            // Arrange
            var zero = new Multivector();
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e12 = e1 ^ e2;

            // Act & Assert
            Assert.True((zero ^ e1).IsZero());
            Assert.True((zero ^ e2).IsZero());
            Assert.True((zero ^ e12).IsZero());
            Assert.True((e1 ^ zero).IsZero());
            Assert.True((e12 ^ zero).IsZero());
        }

        [Fact]
        public void WedgeProduct_BivectorWithBivector_IsZeroIn2D()
        {
            // Arrange
            var e12 = Multivector.CreateBaseBlade(3); // only bivector in 2D

            // Act
            var result = e12 ^ e12;

            // Assert
            Assert.True(result.IsZero());
        }

        [Fact]
        public void WedgeProduct_ScalarWithScalar_IsScalar()
        {
            // Arrange
            var s1 = new Multivector(2.0);
            var s2 = new Multivector(3.0);

            // Act
            var result = s1 ^ s2;

            // Assert
            Assert.Equal(6.0, result[0], 10); // 2 * 3 = 6
        }

        [Fact]
        public void WedgeProduct_VectorWithBivector_OrthogonalProducesZero()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e12 = Multivector.CreateBaseBlade(3);

            // Act
            var result = e1 ^ e12;

            // Assert
            Assert.True(result.IsZero());
        }

        [Fact]
        public void WedgeProduct_BivectorWithVector_OrthogonalProducesZero()
        {
            // Arrange
            var e2 = Multivector.CreateBaseBlade(2);
            var e12 = Multivector.CreateBaseBlade(3);

            // Act
            var result = e12 ^ e2;

            // Assert
            Assert.True(result.IsZero());
        }
    }
}