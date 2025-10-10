# Command Properties are Attached to the Command Directly

Command properties in the C# implementation are defined as direct properties on the command classes themselves, rather than using separate properties classes as seen in the TypeScript implementation.

## Context

The TypeScript implementation uses a pattern where commands inherit from base classes like `WritableCommand<T>` or `DeserializedCommand<T>`, where `T` represents a separate properties interface or type. The actual command data is stored in a `properties` field and accessed via methods like `updateProps()`. This approach leverages TypeScript's dynamic object nature and the `WritableCommand.MaskFlags` mechanism for change tracking.

## Problem

The TypeScript properties pattern cannot be directly translated to C# due to fundamental language differences:

1. **Type Safety**: C#'s static typing system makes it difficult to implement the generic `updateProps()` method that dynamically iterates over property names while maintaining compile-time type safety
2. **Performance**: Replicating the TypeScript approach would require reflection or dictionary-based property access, introducing runtime overhead
3. **Developer Experience**: Using separate properties classes would complicate IntelliSense support and code navigation
4. **Flag Management**: The TypeScript `MaskFlags` mechanism relies on dynamic property access that doesn't translate well to C#'s static typing

## Considered options

**Option 1: Separate Properties Classes**
Create separate classes for command properties and maintain them as a field on the command, similar to the TypeScript pattern.

Advantages:
- Closer alignment with TypeScript implementation
- Potential for shared property validation logic

Disadvantages:
- Added complexity with separate class definitions
- Loss of direct property access on commands
- Reduced IntelliSense support
- More verbose usage patterns

**Option 2: Dictionary-based Properties**
Store properties in a `Dictionary<string, object>` to mimic TypeScript's dynamic object behavior.

Advantages:
- Could enable generic property handling
- Dynamic property access similar to TypeScript

Disadvantages:
- Complete loss of compile-time type safety
- No IntelliSense support for property names
- Boxing/unboxing performance overhead
- Runtime errors for invalid property access

**Option 3: Direct Properties on Commands**
Define all properties directly as C# properties on the command classes themselves.

Advantages:
- Full compile-time type safety
- Excellent IntelliSense and debugging support
- Optimal runtime performance
- Clear, readable code structure
- Natural C# idioms

Disadvantages:
- Diverges from TypeScript implementation pattern
- Properties and command logic in same class (mixed concerns)

## Decision

**Option 3: Direct Properties on Commands** was selected.

Properties are defined directly on command classes as standard C# auto-properties (e.g., `public bool Enabled { get; set; }`). This approach prioritizes type safety, performance, and developer experience over maintaining exact structural parity with the TypeScript implementation.

The decision aligns with the [Change Flag Handling ADR](2025-10-09%20Change%20Flag%20Handling.md), which established that C# uses explicit property setters with manual flag management instead of TypeScript's dynamic `MaskFlags` approach. Since the flag mechanism already diverges from TypeScript, it's logical to also diverge in property organization to optimize for C#'s strengths.

This approach provides:
- Complete compile-time type safety ensuring correct property types
- Full IntelliSense support for property discovery and auto-completion
- Optimal runtime performance with no reflection or boxing overhead
- Clear debugging experience with direct property access
- Consistency with standard C# patterns and .NET best practices

## History

- Proposed 2025-10-10
- Accepted 2025-10-10