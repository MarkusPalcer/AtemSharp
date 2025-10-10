# Use Stack Allocated Spans for Temporary Buffers

## Context

The AtemSharp library frequently processes binary data when communicating with ATEM devices. Commands are serialized to and deserialized from byte arrays, often requiring temporary buffers for intermediate processing. Currently, the codebase uses a mix of approaches:

1. **Heap-allocated byte arrays** (`byte[]`) for temporary buffers
2. **Stack-allocated spans** (`Span<byte>`) in some newer code (e.g., `ProductIdentifierCommand`)
3. **Direct byte array operations** in serialization/deserialization methods

Performance profiling has indicated that frequent allocation of small temporary byte arrays creates garbage collection pressure, particularly in high-throughput scenarios where many commands are processed rapidly.

## Problem

The current use of heap-allocated byte arrays for temporary buffers has several performance implications:

1. **Memory allocation overhead**: Each `new byte[]` call allocates memory on the managed heap
2. **Garbage collection pressure**: Temporary arrays become short-lived objects that increase GC frequency
3. **Copy semantics**: Arrays are passed by reference to the array object, but the data may still be copied unnecessarily
4. **Memory locality**: Heap-allocated arrays may not be cache-friendly compared to stack-allocated data

Example of current problematic pattern:
```csharp
// Heap allocation for temporary buffer
var buffer = new byte[40];
reader.Read(buffer);
var result = ProcessBuffer(buffer);
```

## Considered Options

### Option 1: Continue using byte arrays
- **Pros**: Familiar syntax, works with existing APIs, no learning curve
- **Cons**: Continued GC pressure, heap allocation overhead, potential performance bottlenecks

### Option 2: Use Span<byte> with heap-allocated backing
- **Pros**: Modern memory management, better API design, some performance benefits
- **Cons**: Still requires heap allocation for the backing storage, doesn't eliminate GC pressure

### Option 3: Use stack-allocated Span<byte> (stackalloc)
- **Pros**: 
  - Zero heap allocations for small buffers
  - Excellent performance characteristics
  - Automatic memory management (stack cleanup)
  - Cache-friendly memory layout
  - Type-safe memory access
- **Cons**: 
  - Limited to small buffer sizes (stack space constraints)
  - Requires .NET Core/.NET 5+ for optimal support
  - Learning curve for developers unfamiliar with stackalloc

## Decision

We will standardize on **stack-allocated Span<byte>** for temporary buffers in the AtemSharp library.

### Implementation guidelines:

1. **Use stackalloc for small temporary buffers** (typically ≤ 1KB):
   ```csharp
   Span<byte> buffer = stackalloc byte[40];
   reader.Read(buffer);
   var result = buffer.ToNullTerminatedString();
   ```

2. **Continue using byte arrays for large buffers** or when the buffer needs to outlive the method scope

3. **Prefer Span<byte> parameters** in utility methods to support both stack and heap allocated memory:
   ```csharp
   public static string ToNullTerminatedString(this Span<byte> span)
   ```

4. **Apply to serialization/deserialization operations** where temporary buffers are commonly used

### Affected areas:
- Command deserialization methods
- Binary data processing utilities
- Protocol parsing logic
- Temporary string encoding/decoding operations

## Consequences

### Positive
- **Improved performance**: Reduced GC pressure and allocation overhead
- **Better memory locality**: Stack-allocated data is cache-friendly
- **Reduced memory fragmentation**: No heap allocations for temporary buffers
- **Future-proof**: Aligns with modern .NET performance best practices

### Negative
- **Learning curve**: Developers need to understand stackalloc limitations and usage
- **Size constraints**: Stack-allocated buffers are limited by stack space
- **Compatibility**: Requires understanding of Span<T> semantics

### Neutral
- **Code changes required**: Existing buffer usage patterns need updating
- **Testing needed**: Performance improvements should be validated through benchmarks

## Examples

### Before (heap allocation):
```csharp
public static ProductIdentifierCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
{
    using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
    
    var productIdentifierBytes = new byte[40]; // Heap allocation
    reader.Read(productIdentifierBytes);
    var productIdentifier = Encoding.UTF8.GetString(productIdentifierBytes).TrimEnd('\0');
    
    var model = (Model)reader.ReadByte();
    return new ProductIdentifierCommand { ProductIdentifier = productIdentifier, Model = model };
}
```

### After (stack allocation):
```csharp
public static ProductIdentifierCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
{
    using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
    
    Span<byte> productIdentifierBytes = stackalloc byte[40]; // Stack allocation
    reader.Read(productIdentifierBytes);
    var productIdentifier = productIdentifierBytes.ToNullTerminatedString();
    
    var model = (Model)reader.ReadByte();
    return new ProductIdentifierCommand { ProductIdentifier = productIdentifier, Model = model };
}
```

## History

- **Accepted** 2025-10-10
- **Proposed** 2025-10-10
