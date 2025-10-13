namespace SGA.Tests.WedgeProduct
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
            Assert.Equal(1.0, e12[3], 10);  // e12
            Assert.Equal(1.0, e23[6], 10);  // e23  
            Assert.Equal(-1.0, e31[5], 10); // e31 = -e13
        }

        [Fact]
        public void WedgeProduct_AssociativeProperty_ForMultipleVectors()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);

            // Act - Testando associatividade: (A ^ B) ^ C = A ^ (B ^ C)
            var leftAssociative = (e1 ^ e2) ^ e3;
            var rightAssociative = e1 ^ (e2 ^ e3);

            // Assert - Ambos devem ser iguais a e123
            Assert.Equal(1.0, leftAssociative[7], 10);  // e123
            Assert.Equal(1.0, rightAssociative[7], 10); // e123
            Assert.Equal(leftAssociative[7], rightAssociative[7], 10);
        }

        [Fact]
        public void WedgeProduct_BivectorWithBivector_ZeroIn3D()
        {
            // Arrange
            var e12 = Multivector.CreateBaseBlade(3); // e12
            var e23 = Multivector.CreateBaseBlade(6); // e23

            // Act - Em 3D, o produto wedge de dois bivectors deve ser zero
            // pois tentaria criar um elemento de grau 4, que não existe em 3D
            var result = e12 ^ e23;

            // Assert
            Assert.True(result.IsZero(), "Produto wedge de dois bivectors em 3D deve ser zero");
        }

        [Fact]
        public void WedgeProduct_CoplanarVectors_ZeroVolume()
        {
            // Arrange - Três vetores coplanares
            var v1 = new Multivector(0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0); // e1
            var v2 = new Multivector(0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0); // e2
            var v3 = new Multivector(0.0, 2.0, 3.0, 0.0, 0.0, 0.0, 0.0, 0.0); // 2e1 + 3e2 (no plano xy)

            // Act
            var volume = v1 ^ v2 ^ v3;

            // Assert - Volume deve ser zero pois são coplanares
            Assert.True(volume.IsZero());
        }

        [Fact]
        public void WedgeProduct_CrossProductEquivalence_ForOrthogonalVectors()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e3 = Multivector.CreateBaseBlade(4);

            // Act - Em 3D, a ^ b é dual ao produto vetorial a × b
            var wedge = e1 ^ e2;

            // Assert - e1 ^ e2 = e12, que é dual a e3
            Assert.Equal(1.0, wedge[3], 10); // Componente e12
            Assert.Equal(0.0, wedge[4], 10); // Componente e3 (deve ser zero)
        }

        [Fact]
        public void WedgeProduct_LinearTransformation_PreservesStructure()
        {
            // Arrange
            var v1 = new Multivector(0.0, 1.0, 2.0, 0.0, 0.0, 0.0, 0.0, 0.0); // e1 + 2e2
            var v2 = new Multivector(0.0, 3.0, 4.0, 0.0, 0.0, 0.0, 0.0, 0.0); // 3e1 + 4e2
            var v3 = new Multivector(0.0, 0.0, 0.0, 0.0, 5.0, 0.0, 0.0, 0.0); // 5e3

            // Act
            var wedge12 = v1 ^ v2;
            var wedge23 = v2 ^ v3;
            var wedge123 = v1 ^ v2 ^ v3;

            // Assert - Estrutura deve ser preservada
            Assert.True(IsPureBivector(wedge12));
            Assert.True(IsPureBivector(wedge23));
            Assert.True(IsPureTrivector(wedge123));
        }

        [Fact]
        public void WedgeProduct_ScalarWithMultivector_ScalesAllComponents()
        {
            // Arrange
            var scalar = new Multivector(2.0);
            var multivector = new Multivector(1.0, 3.0, 0.0, 4.0, 0.0, 0.0, 0.0, 0.0); // 1 + 3e1 + 4e12

            // Act
            var result = scalar ^ multivector;

            // Assert - Escalar deve multiplicar todos os componentes
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

            // Assert - Volume do paralelepípedo unitário é 1
            Assert.Equal(1.0, volume[7], 10); // e123
        }

        [Fact]
        public void WedgeProduct_VectorWithBivector_ZeroIfNotOrthogonal()
        {
            // Arrange
            var e1 = Multivector.CreateBaseBlade(1);
            var e2 = Multivector.CreateBaseBlade(2);
            var e12 = e1 ^ e2;

            // Act - e1 está contido no plano e12, então e1 ^ e12 deve ser zero
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

            // Act - e3 é ortogonal ao plano e12
            var result = e12 ^ e3;

            // Assert - Deve criar um trivetor
            Assert.Equal(1.0, result[7], 10); // e123
        }

        private static bool IsPureBivector(Multivector m)
        {
            // Checar se somente os bivetores são diferentes de zero
            return m[0] == 0.0 && m[1] == 0.0 && m[2] == 0.0 && m[4] == 0.0 && (m[3] != 0.0 || m[5] != 0.0 || m[6] != 0.0);
        }

        private static bool IsPureTrivector(Multivector m)
        {
            // Checar se apenas o trivetor é diferente de zero
            return m[7] != 0.0 && m[0] == 0.0 && m[1] == 0.0 && m[2] == 0.0 && m[3] == 0.0 && m[4] == 0.0 && m[5] == 0.0 && m[6] == 0.0;
        }
    }
}