# SGA - Simple Geometric Algebra

A .NET 8 library for Geometric Algebra computation with arbitrary $(p,q,r)$ signature.

## Features
- Multivector operations: addition, subtraction, geometric product (`*`), wedge product (`^`), negation
- Support for custom algebra signatures: Euclidean, pseudo-Euclidean, degenerate
- Dense array representation for all blades, indexed by bitwise basis vector presence
- Operator overloading for natural C# syntax
- Grade projection and blade extraction
- Human-readable output for multivectors
- Unit tests ![??](https://img.shields.io/badge/69/69%20passing-brightgreen)

## Example usage
```csharp
using SGA;

Algebra.Set(3, 0, 0); // 3D Euclidean algebra
var e1 = Multivector.CreateBaseBlade(1);
var e2 = Multivector.CreateBaseBlade(2);
var e3 = Multivector.CreateBaseBlade(4);

var bivector = e1 ^ e2; // e12
var trivector = e1 ^ e2 ^ e3; // e123
var gp = e1 * e2; // geometric product

Console.WriteLine(bivector); // Output: 1.0000*e12
```

## Internal representation
Multivectors are stored as arrays of coefficients, indexed by the blade's bit pattern. For $\mathcal{C}\ell(3,0,0)$:

| Index | Binary | Blade     | Description    |
|-------|--------|-----------|----------------|
| 0     | 000    | $1$       | Scalar         |
| 1     | 001    | $e_1$     | Basis vector 1 |
| 2     | 010    | $e_2$     | Basis vector 2 |
| 3     | 011    | $e_{12}$  | Bivector       |
| 4     | 100    | $e_3$     | Basis vector 3 |
| 5     | 101    | $e_{13}$  | Bivector       |
| 6     | 110    | $e_{23}$  | Bivector       |
| 7     | 111    | $e_{123}$ | Trivector      |

Each blade is uniquely identified by its index, with each bit representing the presence (1) or absence (0) of a basis vector.

## Main API
- `Algebra.Set(int p, int q, int r)` - Set algebra signature
- `Multivector(params double[] coefficients)` - Create a multivector
- `Multivector.CreateBaseBlade(int index)` - Create a basis blade
- `GradeProjection(int k)` - Extract homogeneous part of grade $k$
- `ToString()` - Human-readable output
- Operators: `+`, `-`, `*`, `^`

## Testing
```bash
dotnet test
```
