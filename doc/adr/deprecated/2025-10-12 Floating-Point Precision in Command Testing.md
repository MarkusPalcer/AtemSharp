# Floating-Point Precision in Command Testing

Test floating-point comparisons must account for precision limitations when ATEM protocol values are scaled or converted, 
using the appropriate `.Within()` overload with decimal place rounding to match the actual precision available in the binary protocol.

## Context

ATEM commands frequently contain floating-point values that are serialized as integers with various scaling factors 
#(e.g., values multiplied by 10, 100, or converted through decibel calculations). When testing deserialized commands, 
direct floating-point equality comparisons fail due to precision differences between the original test data values and the
values reconstructed from the binary protocol representation.

## Problem

The TypeScript reference implementation's test data contains floating-point values with high precision 
(e.g., `77.22753723022879`), but the ATEM binary protocol stores these as scaled integers with limited precision.
When the C# implementation deserializes these values, it produces different precision (e.g., `77.2`) 
causing test failures even when the implementation is correct.

Without proper precision handling:
- Tests fail incorrectly due to precision mismatches
- Developers might implement incorrect scaling factors trying to match exact precision
- Test maintenance becomes difficult when precision requirements vary by value type
- Command compatibility verification becomes unreliable

## Considered options

1. **Exact equality comparison**: Compare floating-point values using direct equality
2. **Fixed tolerance comparison**: Use a single tolerance value (±0.01) for all floating-point comparisons
3. **Decimal place rounding**: Round both values to a specific number of decimal places before comparison
4. **Type-specific precision**: Use different precision strategies based on the data type and scaling factor
5. **Ignore precision entirely**: Accept any floating-point value as correct

## Decision

We selected **decimal place rounding** (#3) as the primary approach with **type-specific precision** 
(#4) as guidance for choosing the appropriate decimal places:

- Use `AreApproximatelyEqual(actual, expected, decimals)` overload for scaled values
- Choose decimal places based on the scaling factor used in the binary protocol:
  - **1 decimal place**: Values scaled by 10 (common for clip/gain values)
  - **2 decimal places**: Values scaled by 100 (common for percentage values)
  - **Default tolerance**: Decibel values and complex conversions (±0.01)
- Document the precision choice in test code comments when non-obvious

This approach:
- Matches the actual precision available in the binary protocol
- Provides predictable and maintainable test behavior
- Allows different precision requirements for different value types
- Maintains compatibility verification while avoiding false negatives

## History

- Deprecated 2025-10-28 in favor of `Assert.That(x, Is.EqualTo(y).Within(z))`
- Proposed 2025-10-12
- Approved 2025-10-12