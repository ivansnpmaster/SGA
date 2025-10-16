namespace SGA
{
    public static class Algebra
    {
        public static int P { get; private set; }
        public static int Q { get; private set; }
        public static int R { get; private set; }
        public static int N { get; private set; }
        public static int Dimension { get; private set; }

        // Stores the XOR operation between blade i and blade j (resulting blade mask)
        private static int[,] _geometricProductMasks;
        // Stores the sign of the geometric product between blade i and blade j
        private static int[,] _geometricProductSigns;

        public static void Set(int p, int q, int r)
        {
            P = p; // Vectors with e² = +1 (spatial)
            Q = q; // Vectors with e² = -1 (temporal)
            R = r; // Vectors with e² = 0 (nilpotent)

            // Total number of basis vectors (P + Q + R)
            N = p + q + r;

            // Dimension: Total number of blades (components) in the multivector
            // This is calculated as 2^N using bit shifting
            Dimension = 1 << N;

            // Precompute the masks and signs resulting from a geometric product between blades
            PrecomputeAllGeometricProductMasksAndSigns();
        }

        private static void PrecomputeAllGeometricProductMasksAndSigns()
        {
            _geometricProductMasks = new int[Dimension, Dimension];
            _geometricProductSigns = new int[Dimension, Dimension];

            for (int bladeA = 0; bladeA < Dimension; bladeA++)
            {
                for (int bladeB = 0; bladeB < Dimension; bladeB++)
                {
                    _geometricProductMasks[bladeA, bladeB] = bladeA ^ bladeB;
                    _geometricProductSigns[bladeA, bladeB] = ComputeSingleGeometricProductSign(bladeA, bladeB);
                }
            }
        }

        private static int ComputeSingleGeometricProductSign(int bladeA, int bladeB)
        {
            /*
                The algorithm has two parts:

                1. Reordering sign calculation:
                   For each vector in B, count how many vectors in A have a HIGHER index.
                   Each such pair contributes a factor of -1 to the sign.

                2. Contraction calculation:
                   For each vector that appears in BOTH blades, multiply by the metric of the vector (its square: +1, -1, or 0).

                Mathematical foundation:
                   The geometric product can be seen as:
                       A * B = (-1)^(N) * (product of the metrics of common vectors) * (A XOR B)
                   where N is the number of pairs (i,j) with i in A, j in B, and i > j.
            */

            // Default sign
            sbyte sign = 1;

            // Reordering sign calculation
            for (int i = 0; i < N; i++)
            {
                // Check if the i-th vector is present in B
                int bitI = 1 << i;
                bool isInBladeB = (bladeB & bitI) != 0;

                if (isInBladeB)
                {
                    // For each vector in A with index GREATER than i
                    for (int j = i + 1; j < N; j++)
                    {
                        int bitJ = 1 << j;
                        bool isInBladeA = (bladeA & bitJ) != 0;

                        if (isInBladeA)
                        {
                            /*
                                When entering this if, we find a pair that requires swapping:
                                The vector B[i] needs to pass through vector A[j] in the reordering.
                                Each swap introduces a factor of -1 in the sign.

                                In geometric algebra: e_j * e_i = -e_i * e_j when i != j
                                Therefore, every time a lower-index vector (i) needs to pass through a higher-index vector (j), the sign changes.
                            */

                            sign *= -1;

                            // Example:
                            // bladeA = e2 (bit 1), bladeB = e1 (bit 0)
                            // i=0 (e1 in B), j=1 (e2 in A) -> e1 needs to pass through e2
                            // e2 * e1 = -e1 * e2 -> sign = -1
                        }
                    }
                }
            }

            // Contraction calculation
            for (int i = 0; i < N; i++)
            {
                int bitI = 1 << i;

                // Check if the vector is present in BOTH blades
                bool isInBladeA = (bladeA & bitI) != 0;
                bool isInBladeB = (bladeB & bitI) != 0;

                if (isInBladeA && isInBladeB)
                {
                    /*
                        When entering this if, we find a common vector: its metric sign contributes to the product
                        If a vector appears in both blades, it "contracts":
                            ei * ei = Q(ei) where Q is the metric of the vector.

                        Examples:
                            If ei squares to +1: ei * ei = +1
                            If ei squares to -1: ei * ei = -1
                            If ei squares to 0:  ei * ei = 0
                    */

                    sbyte vectorSquare = GetVectorSquare(i);
                    sign *= vectorSquare;

                    /*
                         Example:
                            bladeA = e1e2, bladeB = e1e3, common = e1
                            e1 squares to -1 (in algebra (0,2,0))
                            sign = sign * (-1) = -sign
                    */
                }
            }

            return sign;
        }

        private static sbyte GetVectorSquare(int vectorIndex)
        {
            if (vectorIndex < P)
                return 1;

            if (vectorIndex < P + Q)
                return -1;

            return 0;
        }

        public static int GetGeometricProductMask(int bladeA, int bladeB) => _geometricProductMasks[bladeA, bladeB];
        public static int GetGeometricProductSign(int bladeA, int bladeB) => _geometricProductSigns[bladeA, bladeB];
    }
}