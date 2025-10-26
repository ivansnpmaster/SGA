namespace SimGA
{
    public partial class Multivector
    {
        /// <summary>
        /// Geometric product of multivectors
        /// </summary>
        public static Multivector operator *(Multivector a, Multivector b)
        {
            var resultCoefficients = new double[Algebra.Dimension];

            /*
                O produto geométrico entre dois multivetores é calculado como a soma dos produtos geométricos de todas as combinações de suas componentes. 
                    C = A × B = Σᵢ Σⱼ (aᵢ * bⱼ) * (bladeᵢ × bladeⱼ)
                Onde:
                    - aᵢ, bⱼ são coeficientes escalares
                    - bladeᵢ × bladeⱼ é o produto geométrico entre as blades
                O resultado é acumulado no blade apropriado com o sinal correto.
            */

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                if (a[i] == 0.0) continue;

                for (int j = 0; j < Algebra.Dimension; j++)
                {
                    if (b[j] == 0.0) continue;

                    /*
                        A blade resultante do produto geométrico é dada pelo XOR das máscaras.
                        A operação XOR (^) entre os índices das blades nos diz qual blade resulta:
                            - Bits que estão em AMBOS os operandos são zerados (contração)
                            - Bits que estão em APENAS UM operando são mantidos

                        Exemplo:
                            i = 3 (011 = e1e2), j = 5 (101 = e1e3)
                            011 ^ 101 = 110 (e2e3)
                    */
                    int resultBlade = Algebra.GetGeometricProductMask(i, j);

                    /*
                        O sinal do produto geométrico já foi pré-calculado durante a inicialização.
                        O sinal considera:
                            1. A métrica dos vetores (P, Q, R)
                            2. A ordem dos vetores (não-comutatividade)
                    */
                    double sign = Algebra.GetGeometricProductSign(i, j);

                    // A contribuição deste par específico (blade i de A × blade j de B) para
                    // o resultado final é: sinal × coeficienteA × coeficienteB
                    resultCoefficients[resultBlade] += sign * a[i] * b[j];
                }
            }

            return new Multivector(resultCoefficients);
        }

        /// <summary>
        /// Wedge product (exterior product) of multivectors
        /// </summary>
        public static Multivector operator ^(Multivector a, Multivector b)
        {
            var resultCoefficients = new double[Algebra.Dimension];

            /*
                O produto wedge (produto exterior) entre duas blades é zero se elas compartilharem qualquer vetor base. Caso contrário, é igual ao produto geométrico.

                Propriedades:
                - Anti-comutativo: A ^ B = -B ^ A
                - Nilpotente: A ^ A = 0
                - Associativo: (A ^ B) ^ C = A ^ (B ^ C)
            */

            for (int i = 0; i < Algebra.Dimension; i++)
            {
                if (a[i] == 0.0) continue;

                for (int j = 0; j < Algebra.Dimension; j++)
                {
                    if (b[j] == 0.0) continue;

                    /*
                        Verifica se as blades compartilham vetores base
                        Se (i & j) != 0, elas compartilham pelo menos um vetor base
                        Nesse caso, o produto wedge é zero
                    */
                    if ((i & j) != 0)
                    {
                        continue;
                    }

                    // Para blades sem vetores em comum, o produto wedge é igual ao produto geométrico
                    int resultBlade = Algebra.GetGeometricProductMask(i, j);
                    double sign = Algebra.GetGeometricProductSign(i, j);
                    resultCoefficients[resultBlade] += sign * a[i] * b[j];
                }
            }

            return new Multivector(resultCoefficients);
        }

        public static Multivector operator +(Multivector a, Multivector b)
        {
            var result = new double[a.Dimension];

            for (int i = 0; i < a.Dimension; i++)
            {
                result[i] = a[i] + b[i];
            }

            return new Multivector(result);
        }

        public static Multivector operator -(Multivector a)
        {
            var result = new double[a.Dimension];

            for (int i = 0; i < a.Dimension; i++)
            {
                result[i] = -a[i];
            }

            return new Multivector(result);
        }

        public static Multivector operator -(Multivector a, Multivector b)
        {
            var result = new double[a.Dimension];

            for (int i = 0; i < a.Dimension; i++)
            {
                result[i] = a[i] - b[i];
            }

            return new Multivector(result);
        }

        public static Multivector operator *(double scalar, Multivector other)
        {
            var result = new double[other.Dimension];

            for (int i = 0; i < other.Dimension; i++)
            {
                result[i] = scalar * other[i];
            }

            return new Multivector(result);
        }

        public static Multivector operator *(Multivector mv, double scalar) => scalar * mv;

        public static bool operator ==(Multivector? left, Multivector? right) => Equals(left, right);

        public static bool operator !=(Multivector? left, Multivector? right) => !Equals(left, right);

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not Multivector other)
            {
                return false;
            }

            if (Dimension != other.Dimension)
            {
                return false;
            }

            const double tolerance = 1e-10;

            for (int i = 0; i < Dimension; i++)
            {
                if (Math.Abs(_coefficients[i] - other._coefficients[i]) > tolerance)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            // Combina todos os coeficientes em uma sequência determinística
            var hash = new HashCode();

            for (int i = 0; i < Dimension; i++)
            {
                hash.Add(_coefficients[i]);
            }

            return hash.ToHashCode();
        }
    }
}