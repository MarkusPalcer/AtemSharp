# Serialization and Deserialization

Superseeds [the ADR from 2025-10-09](superseeded/2025-10-09%20Serialization%20and%20Deserialization.md)

- 80% of serialization and deserialization is handled by generated code
- Custom (de-)serialization logic can be added by adding `SerializeInternal` or `DeserializeInternal` methods
- Generated code is controlled by attributes on private fields
  - `SerializedField(<write-offset>, <flag-position>)` and `DeserializedField(<read-offset>)` to mark a field as (de-)serialized
  - `NoProperty` skips auto-generation of properties
  - `CustomSerialization` / `CustomDeserialization` skips generation of (de-)serialization code
  - `SerializedType(typeof(<type>))` controls which type is written onto the wire (especially for floating point numbers)
  - `ScalingFactor(<number>)` can be used to divide / multiply the value on the wire to derive the double value
  - `CustomScaling` can be used to use a custom scaling function _in addition to the scaling factor_
  

## Problem

The ATEM protocol is binary and heavily indexed: many command types are expressed as fixed byte offsets with mixed integer widths, custom endianness and scaled numeric values. The TypeScript implementation relied on Node.js `Buffer` APIs and manual index arithmetic to produce and consume wire-format commands. 

## Considered options

We evaluated three high-level approaches:

1. TypeScript-style manual buffer writes 
  - Pros: minimal abstraction; potentially slightly lower per-message overhead.
  - Cons: error-prone indexing, manual endianness conversions, large duplicated boilerplate in many command classes; not idiomatic C#.

2. Stream-based serialization helpers (MemoryStream + BinaryWriter + explicit endianness helpers)
  - Pros: idiomatic .NET, easier to reason about, clear extension points for custom types.
  - Cons: small allocation/abstraction overhead vs manual arrays.

3. Unsafe / pointer-based memory manipulation
  - Pros: best theoretical performance for hot paths.
  - Cons: unsafe, platform-sensitive, hard to audit and maintain; not worth the complexity for the command frequency we expect.

## Selected approach: combination
Use generated code for the majority of serialization/deserialization and standard .NET stream helpers for the manual parts, while providing small, well-defined extension points (attributes + optional `SerializeInternal` / `DeserializeInternal` methods) for the remaining special cases.

### Rationale

- The code generator encodes offsets, types, scaling and property-generation rules using attributes such as `SerializedField(writeOffset [, flagPosition])`, `DeserializedField(readOffset)`, `SerializedType(typeof(...))`, `ScalingFactor(...)`, `CustomSerialization` and `CustomDeserialization`. These are already widely used across command implementations (see many examples under `AtemSharp/Commands/**`).
- Generator-side templates (for example `AtemSharp.CodeGenerators/SerializedField_Serialization.sbn` and `DeserializedField_Deserialization.sbn`) already emit calls to extension methods like `buffer.Write{{extensionMethod}}(...)` and readers like `rawCommand.Read{{extensionMethod}}(...)` so the generated code targets a small set of wire-read/write primitives.
- The generator also enforces a small set of diagnostics to avoid configuration mistakes (see `AtemSharp.CodeGenerators/DiagnosticDescriptors.cs`).

Choosing generation + small runtime primitives yields:

- maintainable, auditable code with most of the complexity moved to a small generator and a suite of helpers,
- deterministic wire output (the templates and helpers are stable),
- clear contract for custom cases.

## Implementation notes (what to implement / how to maintain)

Contract (short)
- Inputs: a command instance (C# class) annotated with the attributes listed below.
- Outputs: a byte array / MemoryStream containing a command encoded according to the ATEM wire format.
- Error modes: attribute misuse (caught by source-generator diagnostics), insufficient buffer lengths at runtime (throw ArgumentException/InvalidOperation), incompatible `CustomSerialization` signatures (diagnostics).

Testing and verification
Unit tests should assert that serialization is byte-for-byte identical to a canonical fixture (we already have many tests under `AtemSharp.Tests`). For this try to use the base classes that use existing test data from the typescript reference implementation

## History

- Accepted 2028-10-28
- Proposed 2025-10-28