namespace SimGA.Tests.WedgeProduct
{
    public class WedgeProductEuclidean3DAlgebraTests
    {
        public WedgeProductEuclidean3DAlgebraTests() => Algebra.Set(3, 0, 0);

        [Fact]
        public void WedgeProduct_BasisVectors_CreateAllBivectors()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);

            // Act
            var e12 = e1 ^ e2;
            var e23 = e2 ^ e3;
            var e31 = e3 ^ e1;

            // Assert
            Assert.Equal(1.0, e12[3], 10); // e12
            Assert.Equal(1.0, e23[6], 10); // e23  
            Assert.Equal(-1.0, e31[5], 10); // e31 = -e13
        }

        [Fact]
        public void WedgeProduct_AssociativeProperty_ForMultipleVectors()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);

            // Act - Testing associativity: (A ^ B) ^ C = A ^ (B ^ C)
            var leftAssociative = (e1 ^ e2) ^ e3;
            var rightAssociative = e1 ^ (e2 ^ e3);

            // Assert - Both should be equal to e123
            Assert.Equal(1.0, leftAssociative[7], 10); // e123
            Assert.Equal(1.0, rightAssociative[7], 10); // e123
            Assert.Equal(leftAssociative[7], rightAssociative[7], 10);
        }

        [Fact]
        public void WedgeProduct_BivectorWithBivector_ZeroIn3D()
        {
            // Arrange
            var e12 = Multivector.CreateBaseBlade(3); // e12
            var e23 = Multivector.CreateBaseBlade(6); // e23

            // Act - In 3D, the wedge product of two bivectors must be zero
            // because it would try to create a grade 4 element, which does not exist in 3D
            var result = e12 ^ e23;

            // Assert
            Assert.True(result.IsZero(), "Wedge product of two bivectors in 3D must be zero");
        }

        [Fact]
        public void WedgeProduct_CoplanarVectors_ZeroVolume()
        {
            // Arrange - Three coplanar vectors
            var v1 = new Multivector(0.0, 1.0); // e1
            var v2 = new Multivector(0.0, 0.0, 1.0); // e2
            var v3 = new Multivector(0.0, 2.0, 3.0); // 2e1 + 3e2 (in the xy plane)

            // Act
            var volume = v1 ^ v2 ^ v3;

            // Assert - Volume must be zero because they are coplanar
            Assert.True(volume.IsZero());
        }

        [Fact]
        public void WedgeProduct_CrossProductEquivalence_ForOrthogonalVectors()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);

            // Act - In 3D, a ^ b is dual to the cross product a * b
            var wedge = e1 ^ e2;

            // Assert - e1 ^ e2 = e12, which is dual to e3
            Assert.Equal(1.0, wedge[3], 10); // Component e12
            Assert.Equal(0.0, wedge[4], 10); // Component e3 (should be zero)
        }

        [Fact]
        public void WedgeProduct_LinearTransformation_PreservesStructure()
        {
            // Arrange
            var v1 = new Multivector(0.0, 1.0, 2.0); // e1 + 2e2
            var v2 = new Multivector(0.0, 3.0, 4.0); // 3e1 + 4e2
            var v3 = new Multivector(0.0, 0.0, 0.0, 0.0, 5.0); // 5e3

            // Act
            var wedge12 = v1 ^ v2;
            var wedge23 = v2 ^ v3;
            var wedge123 = v1 ^ v2 ^ v3;

            // Assert - Structure must be preserved
            Assert.True(IsPureBivector(wedge12));
            Assert.True(IsPureBivector(wedge23));
            Assert.True(IsPureTrivector(wedge123));
        }

        [Fact]
        public void WedgeProduct_ScalarWithMultivector_ScalesAllComponents()
        {
            // Arrange
            var scalar = new Multivector(2.0);
            var multivector = new Multivector(1.0, 3.0, 0.0, 4.0); // 1 + 3e1 + 4e12

            // Act
            var result = scalar ^ multivector;

            // Assert - Scalar must multiply all components
            Assert.Equal(2.0, result[0], 10); // scalar
            Assert.Equal(6.0, result[1], 10); // e1
            Assert.Equal(8.0, result[3], 10); // e12
        }

        [Fact]
        public void WedgeProduct_TripleProduct_ComputesVolume()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);

            // Act
            var volume = e1 ^ e2 ^ e3;

            // Assert - Volume of the unit parallelepiped is 1
            Assert.Equal(1.0, volume[7], 10); // e123
        }

        [Fact]
        public void WedgeProduct_VectorWithBivector_ZeroIfNotOrthogonal()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e12 = e1 ^ e2;

            // Act - e1 is contained in the plane e12, so e1 ^ e12 must be zero
            var result = e1 ^ e12;

            // Assert
            Assert.True(result.IsZero());
        }

        [Fact]
        public void WedgeProduct_VectorWithOrthogonalBivector_CreatesTrivector()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);
            var e12 = e1 ^ e2;

            // Act - e3 is orthogonal to the plane e12
            var result = e12 ^ e3;

            // Assert - Should create a trivector
            Assert.Equal(1.0, result[7], 10); // e123
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
        public void WedgeProduct_VectorWithItself_IsZero()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);

            // Act & Assert
            Assert.True((e1 ^ e1).IsZero());
            Assert.True((e2 ^ e2).IsZero());
            Assert.True((e3 ^ e3).IsZero());
        }

        [Fact]
        public void WedgeProduct_BivectorWithTrivector_IsZero()
        {
            // Arrange
            var e12 = Multivector.CreateBaseBlade(3);
            var e123 = Multivector.CreateBaseBlade(7);

            // Act
            var result = e12 ^ e123;

            // Assert
            Assert.True(result.IsZero());
        }

        [Fact]
        public void WedgeProduct_TrivectorWithTrivector_IsZero()
        {
            // Arrange
            var e123 = Multivector.CreateBaseBlade(7);

            // Act
            var result = e123 ^ e123;

            // Assert
            Assert.True(result.IsZero());
        }

        [Fact]
        public void WedgeProduct_ScalarWithTrivector_ScalesTrivector()
        {
            // Arrange
            var scalar = new Multivector(5.0);
            var e123 = Multivector.CreateBaseBlade(7);

            // Act
            var result = scalar ^ e123;

            // Assert
            Assert.Equal(5.0, result[7], 10);
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
            Assert.True((v1 ^ v2 ^ v3).IsZero());
        }

        private static bool IsPureBivector(Multivector m)
        {
            // Check if only the bivectors are nonzero
            return m[0] == 0.0 && m[1] == 0.0 && m[2] == 0.0 && m[4] == 0.0 && (m[3] != 0.0 || m[5] != 0.0 || m[6] != 0.0);
        }

        private static bool IsPureTrivector(Multivector m)
        {
            // Check if only the trivector is nonzero
            return m[7] != 0.0 && m[0] == 0.0 && m[1] == 0.0 && m[2] == 0.0 && m[3] == 0.0 && m[4] == 0.0 && m[5] == 0.0 && m[6] == 0.0;
        }
    }
}