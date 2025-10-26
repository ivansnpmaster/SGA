namespace SimGA.Tests.GeometricProduct
{
    public class GeometricProductComplexAlgebraTests
    {
        public GeometricProductComplexAlgebraTests() => Algebra.Set(0, 2, 0);

        [Fact]
        public void GeometricProduct_ComplexNumbers_ImaginaryUnitSquaresToMinusOne()
        {
            // Arrange
            var i = new Multivector(0, 0, 0, 1.0); // i = e12

            // Act
            var iSquared = i * i;

            // Assert
            Assert.Equal(-1.0, iSquared[0], 10);
            Assert.True(iSquared.IsScalar());
        }

        [Fact]
        public void GeometricProduct_ComplexNumbers_ComplexMultiplication()
        {
            // Arrange
            var z1 = new Multivector(2.0, 0, 0, 3.0); // 2 + 3i
            var z2 = new Multivector(1.0, 0, 0, 4.0); // 1 + 4i

            // Act
            var product = z1 * z2;

            // Assert
            // (2 + 3i) × (1 + 4i) = 2×1 + 2×4i + 3i×1 + 3i×4i 
            // = 2 + 8i + 3i + 12i² = 2 + 11i - 12 = -10 + 11i
            Assert.Equal(-10.0, product[0], 10); // Real part
            Assert.Equal(11.0, product[3], 10); // Imaginary part (e12)

            // Check that other components are zero
            Assert.Equal(0.0, product[1], 10); // e1
            Assert.Equal(0.0, product[2], 10); // e2
        }

        [Fact]
        public void GeometricProduct_ComplexNumbers_ComplexConjugation_Corrected()
        {
            // Arrange
            var z = new Multivector(3.0, 0, 0, 4.0); // 3 + 4i
            var conjugate = new Multivector(3.0, 0, 0, -4.0); // 3 - 4i

            // Act
            var normSquared = z * conjugate;

            // Assert
            // (3 + 4i) × (3 - 4i) = 9 - 12i + 12i - 16i² = 9 + 16 = 25
            Assert.Equal(25.0, normSquared[0], 10);
            Assert.True(normSquared.IsScalar());
        }

        [Fact]
        public void GeometricProduct_ComplexNumbers_CommutativeInEvenSubalgebra()
        {
            // Arrange
            var z1 = new Multivector(2.0, 0, 0, 3.0); // 2 + 3i
            var z2 = new Multivector(1.0, 0, 0, 4.0); // 1 + 4i

            // Act
            var z1z2 = z1 * z2;
            var z2z1 = z2 * z1;

            // Assert - Complex numbers commute
            Assert.Equal(z1z2[0], z2z1[0], 10);
            Assert.Equal(z1z2[3], z2z1[3], 10);
        }

        [Fact]
        public void GeometricProduct_ComplexNumbers_Rotation()
        {
            // Arrange
            var i = new Multivector(0, 0, 0, 1.0); // i = e12
            var realNumber = new Multivector(2.0); // 2 + 0i

            // Act
            var rotated = i * realNumber; // i × 2 = 2i

            // Assert
            Assert.Equal(0.0, rotated[0], 10); // Real part zero
            Assert.Equal(2.0, rotated[3], 10); // Imaginary part 2
        }

        [Fact]
        public void GeometricProduct_ComplexNumbers_WithVectors_AntiCommute()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1); // e1
            var i = new Multivector(0, 0, 0, 1.0); // i = e12

            // Act
            var e1_i = e1 * i;
            var i_e1 = i * e1;

            // Assert - e1 anti-commutes with i (because i contains e2)
            // e1 × e12 = e1 × e1e2 = (e1 × e1) × e2 = (-1) × e2 = -e2
            // e12 × e1 = e1e2 × e1 = e1e2e1 = -e1e1e2 = -(-1)×e2 = e2
            Assert.Equal(-1.0, e1_i[2], 10); // e1 × i = -e2
            Assert.Equal(1.0, i_e1[2], 10); // i × e1 = e2
        }

        [Fact]
        public void GeometricProduct_VectorSquares_EqualMinusOne()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1); // e1
            var e2 = Multivector.CreateBaseBlade(2); // e2

            // Act
            var e1e1 = e1 * e1;
            var e2e2 = e2 * e2;

            // Assert
            Assert.Equal(-1.0, e1e1[0], 10);
            Assert.Equal(-1.0, e2e2[0], 10);
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
        public void GeometricProduct_ScalarWithBivector_ScalesBivector()
        {
            // Arrange
            var scalar = new Multivector(2.0);
            var e12 = Multivector.CreateBaseBlade(3);

            // Act
            var result = scalar * e12;

            // Assert
            Assert.Equal(2.0, result[3], 10);
        }

        [Fact]
        public void GeometricProduct_BivectorWithScalar_ScalesBivector()
        {
            // Arrange
            var scalar = new Multivector(2.0);
            var e12 = Multivector.CreateBaseBlade(3);

            // Act
            var result = e12 * scalar;

            // Assert
            Assert.Equal(2.0, result[3], 10);
        }

        [Fact]
        public void GeometricProduct_ZeroElement_AnnihilatesAll()
        {
            // Arrange
            var zero = new Multivector();
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e12 = Multivector.CreateBaseBlade(3);

            // Act & Assert
            Assert.True((zero * e1).IsZero());
            Assert.True((e1 * zero).IsZero());
            Assert.True((zero * e2).IsZero());
            Assert.True((e2 * zero).IsZero());
            Assert.True((zero * e12).IsZero());
            Assert.True((e12 * zero).IsZero());
        }
    }
}