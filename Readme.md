# SGA - Simple Geometric Algebra

A .NET 8 library for Geometric Algebra computation with arbitrary signatures.

## Features
- Multivector operations: addition, subtraction, geometric product (`*`), wedge product (`^`), negation
- Support for custom algebra signatures: Euclidean, pseudo-Euclidean, degenerate
- Dense array representation for all blades, indexed by bitwise basis vector presence
- Operator overloading for natural C# syntax
- Grade projection and blade extraction
- Human-readable output for multivectors
- Comprehensive unit tests

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

| Index | Binary | Blade   | Description        |
|-------|--------|---------|-------------------|
| 0     | 000    | 1       | Scalar            |
| 1     | 001    | e1      | Basis vector 1    |
| 2     | 010    | e2      | Basis vector 2    |
| 3     | 011    | e1e2    | Bivector          |
| 4     | 100    | e3      | Basis vector 3    |
| 5     | 101    | e1e3    | Bivector          |
| 6     | 110    | e2e3    | Bivector          |
| 7     | 111    | e1e2e3  | Trivector         |

Each blade is uniquely identified by its index, with each bit representing the presence (1) or absence (0) of a basis vector.

## Main API
- `Algebra.Set(int p, int q, int r)` - Set algebra signature
- `Multivector(params double[] coefficients)` - Create a multivector
- `Multivector.CreateBaseBlade(int index)` - Create a basis blade
- `GradeProjection(int k)` - Extract homogeneous part of grade k
- `ToString()` - Human-readable output
- Operators: `+`, `-`, `*`, `^`

## Testing
```bash
dotnet test
```

## Status
Stable - Version 1.0