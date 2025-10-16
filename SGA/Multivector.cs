namespace SGA
{
    public partial class Multivector
    {
        /*
            _coefficients: Array that stores the coefficients of ALL possible blades.
            The index in the array corresponds to the blade's "type" using bitwise representation:

            Example for Algebra(3, 0, 0):

                Index | Binary | Blade  | Description
                ------|--------|--------|----------------------
                  0   | 000    | 1      | Scalar (grade 0)
                  1   | 001    | e1     | Basis vector 1 (grade 1)
                  2   | 010    | e2     | Basis vector 2 (grade 1)
                  3   | 011    | e1e2   | Bivector (grade 2)
                  4   | 100    | e3     | Basis vector 3 (grade 1)
                  5   | 101    | e1e3   | Bivector (grade 2)
                  6   | 110    | e2e3   | Bivector (grade 2)
                  7   | 111    | e1e2e3 | Trivector (grade 3)

            This representation allows each blade to be uniquely identified by an integer index, where each bit represents the presence (1) or absence (0) of a basis vector.
        */
        private readonly double[] _coefficients;

        public Multivector(params double[] coefficients)
        {
            if (coefficients.Length < Algebra.Dimension)
            {
                // Creates a new array with the exact size of the algebra (all zeros by default)
                _coefficients = new double[Algebra.Dimension];

                // Copies the provided coefficients to the beginning of the array
                // The rest remains zero automatically
                Array.Copy(coefficients, _coefficients, coefficients.Length);
            }
            else if (coefficients.Length > Algebra.Dimension)
            {
                // Array larger than the algebra dimension - this is an error
                throw new ArgumentException($"INCOMPATIBLE DIMENSIONALITY: Provided {coefficients.Length} coefficients, but the configured algebra supports only {Algebra.Dimension}. Check the algebra signature or adjust the number of coefficients.");
            }
            else
            {
                // Perfect case: array of exact size
                _coefficients = coefficients;
            }
        }

        public double this[int bladeIndex] => _coefficients[bladeIndex];

        public int Dimension => _coefficients.Length;

        public static Multivector CreateBaseBlade(int bladeIndex)
        {
            var coefficients = new double[Algebra.Dimension];
            coefficients[bladeIndex] = 1.0;

            return new Multivector(coefficients);
        }

        private static int BladeGrade(int bladeIndex)
        {
            // Count how many 1 bits appear in the blade to get the grade

            int c = 0;

            while (bladeIndex != 0)
            {
                // Remove the least significant 1 bit:
                // ex: x = 011010 -> x-1 = 011001 -> x & (x-1) = 011000
                bladeIndex &= (bladeIndex - 1);
                c++;
            }

            return c;
        }

        public Multivector GradeProjection(int k)
        {
            // Returns the homogeneous part of grade 'k' of the multivector
            // Iterates over all blades: if it has popcount == k, copies the coefficient

            var res = new double[Dimension];

            for (int blade = 0; blade < Dimension; blade++)
            {
                if (BladeGrade(blade) == k)
                    res[blade] = _coefficients[blade];
            }

            return new Multivector(res);
        }

        public override string ToString()
        {
            var parts = new List<string>();

            for (int i = 0; i < Dimension; i++)
            {
                if (_coefficients[i] != 0.0)
                {
                    string bladeName = GetBladeName(i);
                    // CultureInfo.InvariantCulture to ensure decimal point
                    parts.Add($"{_coefficients[i].ToString("F4", System.Globalization.CultureInfo.InvariantCulture)}*{bladeName}");
                }
            }

            if (parts.Count == 0)
                return "0";

            return string.Join(" + ", parts);
        }

        private static string GetBladeName(int bladeIndex)
        {
            // Converts a blade index (binary representation) to a readable name, like "e1", "e12", "e123", etc.

            // Blade 0 is always the scalar (represented as "1")
            if (bladeIndex == 0)
                return "1";

            var vectors = new List<int>();

            for (int i = 0; i < Algebra.N; i++)
            {
                // Create a mask for the i-th bit
                int bitMask = 1 << i;

                // Check if this bit is set in the blade
                bool isBitSet = (bladeIndex & bitMask) != 0;

                if (isBitSet)
                {
                    // If the bit is set, the vector e_(i+1) is present
                    // Add (i + 1) because vectors start at e1, not e0
                    vectors.Add(i + 1);
                }
            }

            // Sort the vectors by index for consistency
            vectors.Sort();

            // Build the name in the format "e" followed by the vector numbers
            return "e" + string.Join("", vectors);
        }

        public bool IsScalar()
        {
            for (int i = 1; i < Dimension; i++)
            {
                if (_coefficients[i] != 0)
                    return false;
            }

            return true;
        }

        public bool IsZero(double tolerance = 0.0)
        {
            if (tolerance == 0.0)
            {
                for (int i = 0; i < Dimension; i++)
                {
                    if (_coefficients[i] != 0.0)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                for (int i = 0; i < Dimension; i++)
                {
                    if (Math.Abs(_coefficients[i]) > tolerance)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}