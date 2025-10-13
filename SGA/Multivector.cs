namespace SGA
{
    public partial class Multivector
    {
        /*
            _coefficients: Array que armazena os coeficientes de TODAS as blades possíveis
            O índice no array corresponde ao "tipo" da blade usando representação por bits:

            Exemplo para Algebra(3, 0, 0):

                Índice | Binário | Blade      | Descrição
                -------|---------|------------|-----------
                  0    |   000   | 1          | Escalar (grau 0)
                  1    |   001   | e1         | Vetor base 1 (grau 1)
                  2    |   010   | e2         | Vetor base 2 (grau 1)
                  3    |   011   | e1e2       | Bivetor (grau 2)
                  4    |   100   | e3         | Vetor base 3 (grau 1)
                  5    |   101   | e1e3       | Bivetor (grau 2)
                  6    |   110   | e2e3       | Bivetor (grau 2)
                  7    |   111   | e1e2e3     | Trivetor (grau 3)

            Esta representação permite que cada blade seja identificada unicamente por um índice inteiro, onde cada bit representa a presença (1) ou ausência (0) de um vetor base.
        */
        private readonly double[] _coefficients;

        public Multivector(params double[] coefficients)
        {
            if (coefficients.Length < Algebra.Dimension)
            {
                // Cria um novo array do tamanho exato da álgebra (todos zeros por padrão)
                _coefficients = new double[Algebra.Dimension];

                // Copia os coeficientes fornecidos para o início do array
                // O restante permanece zero automaticamente
                Array.Copy(coefficients, _coefficients, coefficients.Length);
            }
            else if (coefficients.Length > Algebra.Dimension)
            {
                // Array maior que a dimensão da álgebra - isto é um erro
                throw new ArgumentException($"DIMENSIONALIDADE INCOMPATÍVEL: Fornecidos {coefficients.Length} coeficientes, mas a álgebra configurada suporta apenas {Algebra.Dimension}. Verifique a assinatura da álgebra ou ajuste o número de coeficientes.");
            }
            else
            {
                // Caso perfeito: array do tamanho exato
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

        public override string ToString()
        {
            var parts = new List<string>();

            for (int i = 0; i < Dimension; i++)
            {
                if (_coefficients[i] != 0.0)
                {
                    string bladeName = GetBladeName(i);
                    // CultureInfo.InvariantCulture para garantir ponto decimal
                    parts.Add($"{_coefficients[i].ToString("F4", System.Globalization.CultureInfo.InvariantCulture)}*{bladeName}");
                }
            }

            if (parts.Count == 0)
                return "0";

            return string.Join(" + ", parts);
        }

        private static string GetBladeName(int bladeIndex)
        {
            // Converte um índice de blade (representação binária) para um nome legível, como "e1", "e12", "e123", etc.

            // Blade 0 é sempre o escalar (representado como "1")
            if (bladeIndex == 0)
                return "1";

            var vectors = new List<int>();

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                // Cria uma máscara para o i-ésimo bit
                int bitMask = 1 << i;

                // Verifica se este bit está definido na blade
                bool isBitSet = (bladeIndex & bitMask) != 0;

                if (isBitSet)
                {
                    // Se o bit está definido, o vetor e_(i+1) está presente
                    // Adiciona (i + 1) porque os vetores começam em e1, não e0
                    vectors.Add(i + 1);
                }
            }

            // Ordena os vetores por índice para consistência
            vectors.Sort();

            // Constrói o nome no formato "e" seguido pelos números dos vetores
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
    }
}