# SGA - Simple Geometric Algebra

A .NET library for Geometric Algebra computation.

## Features

- Sparse multivector implementation and basic operations
- Support for different signatures ($p$, $q$, $r$)
- Overloaded operators for intuitive syntax
- Pre-computed tables for performance
- Unit tests

## Basic usage

```csharp
using SGA;

// Configure 3D Euclidean algebra
Algebra.Set(3, 0, 0);

// Create basis vectors
var e1 = Multivector.CreateBaseBlade(1);
var e2 = Multivector.CreateBaseBlade(2);

// Operations
var bivector = e1 * e2;
var sum = e1 + e2;
var negated = -e1;

// Create custom multivector
var mv = new Multivector(1.0, 2.0, 3.0); // scalar + 2*e1 + 3*e2
```

## Main API

### Multivector
- `Multivector(params double[] coefficients)` - Constructor
- `this[int bladeIndex]` - Coefficient accessor
- `Dimension` - Multivector dimension
- `IsScalar()` - Checks if purely scalar
- `ToString()` - Human-readable representation

### Operators
- `+`, `-` - Addition and subtraction
- `*` - Geometric product and scalar multiplication
- `-` (unary) - Negation

### Algebra
- `Set(int p, int q, int r)` - Configure signature
- `Dimension` - Total algebra dimension
- `P, Q, R, N` - Signature parameters

## Internal representation

The multivector is stored as a sparse array of all possible blade coefficients, indexed by the blade's bit representation. For example, in $Cl(3,0,0)$:

- Index 0 (000): scalar
- Index 1 (001): e1
- Index 2 (010): e2
- Index 3 (011): e1e2
- Index 4 (100): e3
- Index 5 (101): e1e3
- Index 6 (110): e2e3
- Index 7 (111): e1e2e3

This sparse representation is efficient for low to medium dimensions.

## Example

```csharp
// Configure 2D algebra
Algebra.Set(2, 0, 0);

// Create vectors
var v1 = new Multivector(0.0, 1.0); // e1
var v2 = new Multivector(0.0, 0.0, 1.0); // e2

// Geometric product
var result = v1 * v2; // e1e2

Console.WriteLine(result); // "1.0000*e12"
```

## Testing

```bash
dotnet test
```

## Status

Version 1.0 - Basic features implemented
