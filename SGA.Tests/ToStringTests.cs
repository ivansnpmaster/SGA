namespace SGA.Tests
{
    public class ToStringTests
    {
        [Fact]
        public void ToString_ZeroMultivector_ReturnsZero()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var zero = new Multivector();

            // Act
            var result = zero.ToString();

            // Assert
            Assert.Equal("0", result);
        }

        [Fact]
        public void ToString_ScalarOnly_ReturnsScalarFormat()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var scalar = new Multivector(5.5);

            // Act
            var result = scalar.ToString();

            // Assert
            Assert.Equal("5.5000*1", result);
        }

        [Fact]
        public void ToString_SingleVectorComponent_ReturnsSingleTerm()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var vector = new Multivector(0.0, 2.5);

            // Act
            var result = vector.ToString();

            // Assert
            Assert.Equal("2.5000*e1", result);
        }

        [Fact]
        public void ToString_MultipleVectorComponents_ReturnsSumFormat()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var vector = new Multivector(0.0, 1.0, 2.0, 0.0, 3.0);

            // Act
            var result = vector.ToString();

            // Assert
            Assert.Equal("1.0000*e1 + 2.0000*e2 + 3.0000*e3", result);
        }

        [Fact]
        public void ToString_BivectorComponent_ReturnsBivectorFormat()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var bivector = new Multivector(0.0, 0.0, 0.0, 2.5);

            // Act
            var result = bivector.ToString();

            // Assert
            Assert.Equal("2.5000*e12", result);
        }

        [Fact]
        public void ToString_TrivectorComponent_ReturnsTrivectorFormat()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var trivector = new Multivector(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.5);

            // Act
            var result = trivector.ToString();

            // Assert
            Assert.Equal("1.5000*e123", result);
        }

        [Fact]
        public void ToString_MixedComponents_ReturnsAllNonZeroTerms()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var multivector = new Multivector(1.0, 2.0, 0.0, 3.0, 0.0, 0.0, 0.0, 4.0);

            // Act
            var result = multivector.ToString();

            // Assert
            Assert.Equal("1.0000*1 + 2.0000*e1 + 3.0000*e12 + 4.0000*e123", result);
        }

        [Fact]
        public void ToString_NegativeCoefficients_ReturnsWithMinusSign()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var multivector = new Multivector(-1.0, 0.0, -2.5);

            // Act
            var result = multivector.ToString();

            // Assert
            Assert.Equal("-1.0000*1 + -2.5000*e2", result);
        }

        [Fact]
        public void ToString_FractionalCoefficients_ReturnsFormattedPrecision()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var multivector = new Multivector(1.23456);

            // Act
            var result = multivector.ToString();

            // Assert
            Assert.Equal("1.2346*1", result); // Deve arredondar para 4 casas decimais
        }

        [Fact]
        public void ToString_WithZerosInArray_IgnoresZeroComponents()
        {
            Algebra.Set(2, 0, 0);

            // Arrange
            var multivector = new Multivector(0.0, 1.0, 0.0, 2.0);

            // Act
            var result = multivector.ToString();

            // Assert
            Assert.Equal("1.0000*e1 + 2.0000*e12", result);
        }

        [Fact]
        public void ToString_SingleBivectorIn2D_ReturnsCorrectFormat()
        {
            Algebra.Set(2, 0, 0);

            // Arrange
            var multivector = new Multivector(0.0, 0.0, 0.0, 3.14);

            // Act
            var result = multivector.ToString();

            // Assert
            Assert.Equal("3.1400*e12", result);
        }

        [Fact]
        public void ToString_ComplexMultivector_ReturnsOrderedTerms()
        {
            Algebra.Set(3, 0, 0);

            // Arrange
            var multivector = new Multivector(
                1.0, // escalar
                0.0, // e1 (zero)
                2.0, // e2  
                0.0, // e12 (zero)
                3.0, // e3
                0.0, // e13 (zero)
                4.0, // e23
                5.0 // e123
            );

            // Act
            var result = multivector.ToString();

            // Assert
            Assert.Equal("1.0000*1 + 2.0000*e2 + 3.0000*e3 + 4.0000*e23 + 5.0000*e123", result);
        }

        [Fact]
        public void ToString_VerySmallCoefficients_FormatsCorrectly()
        {
            Algebra.Set(2, 0, 0);

            // Arrange
            var multivector = new Multivector(0.00009, 0.00005);

            // Act
            var result = multivector.ToString();

            // Assert
            // Coeficientes muito pequenos devem ser formatados com notação decimal
            Assert.Equal("0.0001*1 + 0.0001*e1", result);
        }

        [Fact]
        public void ToString_AllComponentsNonZero_ReturnsCompleteExpression()
        {
            Algebra.Set(2, 0, 0);

            // Arrange
            var multivector = new Multivector(1.1, 2.2, 3.3, 4.4);

            // Act
            var result = multivector.ToString();

            // Assert
            Assert.Equal("1.1000*1 + 2.2000*e1 + 3.3000*e2 + 4.4000*e12", result);
        }

        [Fact]
        public void ToString_AfterOperations_ReturnsConsistentFormat()
        {
            Algebra.Set(2, 0, 0);

            // Arrange
            var a = new Multivector(1.0, 2.0);
            var b = new Multivector(3.0, 4.0);
            var product = a * b;

            // Act
            var result = product.ToString();

            // Assert
            // O formato deve ser consistente mesmo após operações
            Assert.Matches(@"^-?\d+\.\d{4}\*\d?e?\d*(\s\+\s-?\d+\.\d{4}\*\d?e?\d*)*$", result);
        }
    }
}