namespace SGA.Tests.GeometricProduct
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
            Assert.Equal(-10.0, product[0], 10);   // Parte real
            Assert.Equal(11.0, product[3], 10);              // Parte imaginária (e12)

            // Verifica que outras componentes são zero
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

            // Assert - Números complexos comutam
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
            Assert.Equal(0.0, rotated[0], 10); // Parte real zero
            Assert.Equal(2.0, rotated[3], 10); // Parte imaginária 2
        }

        [Fact]
        public void GeometricProduct_ComplexNumbers_WithVectors_AntiCommute()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1); // e1
            var i = new Multivector(0, 0, 0, 1.0);  // i = e12

            // Act
            var e1_i = e1 * i;
            var i_e1 = i * e1;

            // Assert - e1 e anti-comuta com i (porque i contém e2)
            // e1 × e12 = e1 × e1e2 = (e1 × e1) × e2 = (-1) × e2 = -e2
            // e12 × e1 = e1e2 × e1 = e1e2e1 = -e1e1e2 = -(-1)×e2 = e2
            Assert.Equal(-1.0, e1_i[2], 10); // e1 × i = -e2
            Assert.Equal(1.0, i_e1[2], 10);  // i × e1 = e2
        }
    }
}