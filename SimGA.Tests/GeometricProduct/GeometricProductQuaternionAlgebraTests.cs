namespace SimGA.Tests.GeometricProduct
{
    public class GeometricProductQuaternionAlgebraTests
    {
        // Quaternions correspond to the even subalgebra: scalars + bivectors
        public GeometricProductQuaternionAlgebraTests() => Algebra.Set(0, 3, 0);

        [Fact]
        public void GeometricProduct_UnitsSquareToMinusOne()
        {
            // Arrange
            var e12 = Multivector.CreateBaseBlade(3);
            var e13 = Multivector.CreateBaseBlade(5);
            var e23 = Multivector.CreateBaseBlade(6);

            // i = e2e3, j = e3e1 (represented as -e1e3), k = e1e2
            var i = e23; // e2e3
            var j = -e13; // e3e1 == -e1e3
            var k = e12; // e1e2

            // Act
            var ii = i * i;
            var jj = j * j;
            var kk = k * k;

            // Assert
            Assert.Equal(-1.0, ii[0], 10);
            Assert.Equal(-1.0, jj[0], 10);
            Assert.Equal(-1.0, kk[0], 10);

            Assert.True(ii.IsScalar());
            Assert.True(jj.IsScalar());
            Assert.True(kk.IsScalar());
        }

        [Fact]
        public void GeometricProduct_UnitsMultiplicationRules()
        {
            // Arrange
            // i = e2e3, j = e3e1 (-e13), k = e1e2
            var e12 = Multivector.CreateBaseBlade(3);
            var e13 = Multivector.CreateBaseBlade(5);
            var e23 = Multivector.CreateBaseBlade(6);

            var i = e23;
            var j = -e13;
            var k = e12;

            // Act
            var ij = i * j;
            var ji = j * i;
            var jk = j * k;
            var kj = k * j;
            var ki = k * i;
            var ik = i * k;

            // Assert
            // ij = k
            Assert.True(ij == k);
            // ji = -k
            Assert.True(ji == -k);

            // jk = i
            Assert.True(jk == i);
            // kj = -i
            Assert.True(kj == -i);

            // ki = j
            Assert.True(ki == j);
            // ik = -j
            Assert.True(ik == -j);
        }

        [Fact]
        public void GeometricProduct_TripleProductEqualsMinusOne()
        {
            // Arrange
            // i = e2e3, j = e3e1 (-e13), k = e1e2
            var e12 = Multivector.CreateBaseBlade(3);
            var e13 = Multivector.CreateBaseBlade(5);
            var e23 = Multivector.CreateBaseBlade(6);

            var i = e23;
            var j = -e13;
            var k = e12;

            // Act
            var triple = i * j * k;

            // Assert
            Assert.Equal(-1.0, triple[0], 10);
            Assert.True(triple.IsScalar());
        }

        [Fact]
        public void GeometricProduct_Associativity()
        {
            // Arrange
            var e12 = Multivector.CreateBaseBlade(3);
            var e13 = Multivector.CreateBaseBlade(5);
            var e23 = Multivector.CreateBaseBlade(6);

            // i = e2e3, j = e3e1 (-e13), k = e1e2
            var i = e23;
            var j = -e13;
            var k = e12;

            // Act
            var left = (i * j) * k;
            var right = i * (j * k);

            // Assert
            Assert.True(left == right);
        }

        [Fact]
        public void GeometricProduct_ClosedUnderMultiplication()
        {
            // Build two generic "quaternions": q = a + b*i + c*j + d*k

            // Arrange
            var q1 = new Multivector(
                1.0, // scalar a
                0.0, // e1
                0.0, // e2
                -0.5, // e12 -> d
                0.0, // e3
                0.25, // e13 -> coefficient for e13 is -c, so store -c; here set +0.25 meaning c = -0.25
                0.75, // e23 -> b
                0.0  // e123
            );

            var q2 = new Multivector(
                2.0,
                0.0,
                0.0,
                -1.5,
                0.0,
                -0.125,
                -0.375,
                0.0
            );

            // Act
            var product = q1 * q2;

            // Assert
            // Check vector components (indices 1,2,4) and trivector (index 7) are zero
            Assert.Equal(0.0, product[1], 10);
            Assert.Equal(0.0, product[2], 10);
            Assert.Equal(0.0, product[4], 10);
            Assert.Equal(0.0, product[7], 10);
        }
    }
}