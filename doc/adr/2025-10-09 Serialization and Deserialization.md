# Serialization and Deserialization

- Serialization is handled differently than in the TS original
- Writing is done via MemoryStream & BinaryWriter instead of writing to an array
- Helper methods ensure that complex values are serialized in the correct byte order (i.e. identical to the TS-code)

## Problem

The TypeScript implementation uses Node.js Buffer APIs for binary serialization, writing directly to pre-allocated byte arrays with explicit byte order handling. Direct 1:1 copying of this approach to C# would have several disadvantages:

1. **Platform differences**: Node.js Buffer methods like `writeUInt16()` don't have direct C# equivalents
2. **Memory management**: Pre-allocating fixed-size byte arrays in C# is less flexible than using streams
3. **Error-prone manual indexing**: The TS code manually tracks byte positions (e.g., `buffer.writeUInt16(value, 6)`), which is fragile and hard to maintain
4. **Endianness complexity**: Manual byte order conversion in C# would require extensive low-level bit manipulation
5. **Type safety**: Direct byte array manipulation bypasses C#'s type system benefits

## Considered options

### Option 1: Direct Buffer Array Approach (TypeScript-like)
Mimic the TypeScript implementation by pre-allocating byte arrays and writing values at specific indices.

**Pros:**
- Most similar to original implementation
- Potentially faster for small payloads

**Cons:**
- Requires manual endianness handling
- Error-prone index management
- Less idiomatic C# code
- Difficult to maintain and debug

### Option 2: MemoryStream + BinaryWriter (Selected)
Use .NET's `MemoryStream` and `BinaryWriter` with custom extension methods for proper endianness.

**Pros:**
- Leverages .NET's stream-based serialization patterns
- Automatic memory management growth; when size is fixed and known it can be passed to the MemoryStream
- Type-safe through extension methods
- Clear, readable code structure
- Consistent endianness handling through helper methods

**Cons:**
- Slight overhead compared to direct array manipulation
- Additional abstraction layer

### Option 3: Unsafe Code with Pointers
Use unsafe C# code to directly manipulate memory like in C/C++.

**Pros:**
- Maximum performance
- Direct memory control

**Cons:**
- Requires unsafe code context
- Platform-dependent
- Goes against .NET best practices
- Security implications

## Decision

**Selected Option 2: MemoryStream + BinaryWriter with Extension Methods**

We chose the MemoryStream + BinaryWriter approach because:

1. **Idiomatic C#**: Uses standard .NET patterns for binary serialization
2. **Maintainability**: Clear, readable code that's easy to debug and extend
3. **Type Safety**: Extension methods like `WriteUInt16BigEndian()` and `WriteInt16BigEndian()` ensure correct endianness while maintaining type safety
4. **Consistency**: All commands follow the same serialization pattern
5. **Reliability**: Automatic memory management eliminates buffer overflow risks
6. **Performance**: While there's theoretical overhead, real-world performance is adequate for ATEM command frequency

The implementation uses custom extension methods in `SerializationExtensions.cs` to handle:
- Big-endian byte order conversion (`WriteUInt16BigEndian`, `WriteInt16BigEndian`, etc.)
- Padding for proper command alignment (`Pad()` method)
- Consistent reading operations (`ReadUInt16BigEndian`, `ReadInt16BigEndian`, etc.)

This approach ensures that the serialized output is byte-for-byte identical to the TypeScript implementation while being maintainable and following C# best practices.

# History

- Accepted 2025-10-09
- Proposed 2025-10-09