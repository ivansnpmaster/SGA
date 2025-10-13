namespace SGA
{
    public static class Algebra
    {
        public static int P { get; private set; }
        public static int Q { get; private set; }
        public static int R { get; private set; }
        public static int N { get; private set; }
        public static int Dimension { get; private set; }

        // Armazena a operação XOR entre a blade i e a blade j (máscara resultante das blades)
        private static int[,] _geometricProductMasks;
        // Armazena o sinal do produto geométrico entre a blade i e a blade j
        private static double[,] _geometricProductSigns;

        public static void Set(int p, int q, int r)
        {
            P = p;  // Vetores com e² = +1 (espaciais)
            Q = q;  // Vetores com e² = -1 (temporais)
            R = r;  // Vetores com e² = 0 (nilpotentes)

            // Número total de vetores base (P + Q + R)
            N = p + q + r;

            // Dimension: Número total de blades (componentes) no multivector
            // Isto é calculado como 2^N usando operação de deslocamento de bits
            Dimension = 1 << N;

            // Pré-cômputo das máscaras e sinais resultantes de um produto geométrico entre as blades
            PrecomputeAllGeometricProductMasksAndSigns();
        }

        private static void PrecomputeAllGeometricProductMasksAndSigns()
        {
            _geometricProductMasks = new int[Dimension, Dimension];
            _geometricProductSigns = new double[Dimension, Dimension];

            for (int bladeA = 0; bladeA < Dimension; bladeA++)
            {
                for (int bladeB = 0; bladeB < Dimension; bladeB++)
                {
                    _geometricProductMasks[bladeA, bladeB] = bladeA ^ bladeB;
                    _geometricProductSigns[bladeA, bladeB] = ComputeSingleGeometricProductSign(bladeA, bladeB);
                }
            }
        }

        private static double ComputeSingleGeometricProductSign(int bladeA, int bladeB)
        {
            /*
                O algoritmo possui duas partes:

                1. Cálculo do sinal da reordenação:
                    Para cada vetor em B, contamos quantos vetores em A têm índice MAIOR que ele.
                    Cada um desses pares contribui com um fator -1 para o sinal.

                2. Cálculo da contração:
                    Para cada vetor que aparece em AMBAS as blades, multiplicamos pela métrica do vetor (seu "quadrado": +1, -1 ou 0).
                
                Fundamentação matemática:
                    O produto geométrico pode ser visto como:
                        A × B = (-1)^(N) × (produto das métricas dos vetores comuns) × (A XOR B)
                    onde N é o número de pares (i,j) com i em A, j em B, e i > j.
            */

            // Sinal default
            double sign = 1.0;

            // Cálculo do sinal da reordenação
            for (int i = 0; i < N; i++)
            {
                // Verifica se o i-ésimo vetor está presente em B
                int bitI = 1 << i;
                bool isInBladeB = (bladeB & bitI) != 0;

                if (isInBladeB)
                {
                    // Para cada vetor em A com índice MAIOR que i
                    for (int j = i + 1; j < N; j++)
                    {
                        int bitJ = 1 << j;
                        bool isInBladeA = (bladeA & bitJ) != 0;

                        if (isInBladeA)
                        {
                            /*
                                Ao entrar neste if, encontramos um par que requer troca:
                                    O vetor B[i] precisa passar pelo vetor A[j] na reordenação.
                                    Cada passagem introduz um fator -1 no sinal.

                                Na álgebra geométrica: e_j × e_i = -e_i × e_j quando i ≠ j
                                Portanto, cada vez que um vetor de índice menor (i) precisa passar por um vetor de índice maior (j), o sinal muda.
                            */

                            sign *= -1.0;

                            // Exemplo:
                            // bladeA = e2 (bit 1), bladeB = e1 (bit 0)
                            // i=0 (e1 em B), j=1 (e2 em A) → e1 precisa passar por e2
                            // e2 × e1 = -e1 × e2 → sinal = -1
                        }
                    }
                }
            }

            // Cálculo da contração
            for (int i = 0; i < N; i++)
            {
                int bitI = 1 << i;

                // Verifica se o vetor está presente em AMBAS as blades
                bool isInBladeA = (bladeA & bitI) != 0;
                bool isInBladeB = (bladeB & bitI) != 0;

                if (isInBladeA && isInBladeB)
                {
                    /*
                        Ao entrar neste if, encontramos um vetor comum: o sinal de sua métrica contribui no produto
                        Se um vetor aparece em ambas as blades, ele se "contrai":
                            e_i × e_i = Q(e_i) onde Q é a métrica do vetor.

                        Exemplos:
                            Se e_i tem quadrado +1: e_i × e_i = +1
                            Se e_i tem quadrado -1: e_i × e_i = -1
                            Se e_i tem quadrado 0: e_i × e_i = 0
                    */

                    double vectorSquare = GetVectorSquare(i);
                    sign *= vectorSquare;

                    /*
                         Exemplo:
                            bladeA = e1e2, bladeB = e1e3, common = e1
                            e1 tem quadrado -1 (em álgebra (0,2,0))
                            sign = sign × (-1) = -sign
                    */
                }
            }

            return sign;
        }

        private static double GetVectorSquare(int vectorIndex)
        {
            if (vectorIndex < P)
                return 1.0;

            if (vectorIndex < P + Q)
                return -1.0;

            return 0.0;
        }

        public static int GetGeometricProductMask(int bladeA, int bladeB) => _geometricProductMasks[bladeA, bladeB];
        public static double GetGeometricProductSign(int bladeA, int bladeB) => _geometricProductSigns[bladeA, bladeB];
    }
}