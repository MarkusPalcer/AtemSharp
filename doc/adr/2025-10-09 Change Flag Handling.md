# Change Flag Handling

The C# implementation uses explicit property setters with manual flag management in SerializedCommand classes. Each property setter automatically sets the corresponding bit in the `Flag` field using bitwise OR operations (e.g., `Flag |= 1 << 0`). This provides compile-time type safety while maintaining binary compatibility with the ATEM protocol's change flag system.

## Problem

The TypeScript implementation leverages JavaScript's dynamic object nature where objects function as dictionaries, allowing the `WritableCommand` base class to generically iterate over property names and automatically set flags using a static `MaskFlags` mapping. However, C# is a statically typed language where:

- Objects cannot be treated as dictionaries without sacrificing type safety
- Using a `Dictionary<string, object>` would eliminate compile-time type checking and IntelliSense support
- Reflection-based solutions would be complex and have performance implications
- The `updateProps` method in TypeScript relies on dynamic property access that doesn't translate well to C#'s static typing

## Considered options

**Option 1: Dictionary-based approach**
Use `Dictionary<string, object>` with string keys for property names and boxed values.

Advantages:
- Direct translation of TypeScript approach
- Could enable generic flag handling

Disadvantages:
- Loss of compile-time type safety
- No IntelliSense support for property names
- Performance overhead from boxing/unboxing
- Runtime errors for invalid property names or types

**Option 2: Reflection-based approach**
Use reflection to iterate over properties and automatically set flags based on attributes or naming conventions.

Advantages:
- Maintains some type safety for property values
- Could provide automated flag management

Disadvantages:
- Significant complexity in implementation
- Performance overhead from reflection
- Potential runtime errors if reflection setup is incorrect
- Debugging difficulties

**Option 3: Explicit property setters with manual flag management**
Each property has an explicit setter that manually sets the corresponding flag bit.

Advantages:
- Full compile-time type safety
- Clear, readable code that's easy to understand and debug
- No performance overhead
- IntelliSense support for all properties
- Direct control over flag bit assignments

Disadvantages:
- More verbose than TypeScript implementation
- Manual flag management in each setter
- Potential for developer errors in flag bit assignments

## Decision

**Option 3: Explicit property setters with manual flag management** was chosen.

This approach prioritizes type safety and maintainability over brevity. Each SerializedCommand subclass implements properties with explicit setters that set the appropriate flag bits (e.g., `Flag |= 1 << 0`). While more verbose than the TypeScript implementation, this approach provides:

- Complete compile-time type safety ensuring property types are correct
- Clear debugging and development experience with full IntelliSense support
- Optimal runtime performance with no reflection or boxing overhead
- Explicit control over the binary protocol, making the flag assignments clearly visible in code
- Consistency with C# language idioms and .NET best practices

# History

- Accepted 2025-10-09
- Proposed 2025-10-09