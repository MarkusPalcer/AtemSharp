# Sparse Arrays in ATEM State Using Dictionary Collections

The ATEM state management system uses `Dictionary<int, TValue>` collections with a generic `GetOrCreate<TValue>(int)` extension method to implement sparse indexing that matches the behavior of the TypeScript reference implementation.

## Context

The C# ATEM implementation needs to maintain state for various indexed resources like mix effects, downstream keyers, and auxiliary outputs. These resources have numeric indices (0-based) but may not be contiguous - for example, an ATEM device might have mix effects at indices 0, 1, and 5, but not 2, 3, or 4. The original TypeScript implementation uses JavaScript arrays which naturally support sparse indexing through `undefined` entries.

## Problem

The initial C# implementation attempted to use fixed-size arrays (`MixEffect?[]`, `DownstreamKeyer?[]`, `int?[]`) with manual resizing logic to accommodate the highest accessed index. This approach had several critical issues:

### Memory Inefficiency
```csharp
// Old approach: accessing index 10 creates an array of size 11
if (state.Video.MixEffects.Length <= index) {
    var newArray = new MixEffect?[index + 1];  // Wastes slots 0-9 if only 10 is used
    Array.Copy(state.Video.MixEffects, newArray, state.Video.MixEffects.Length);
    state.Video.MixEffects = newArray;
}
```

### Performance Overhead
- Array copying on every expansion (O(n) operation)
- Multiple memory allocations and garbage collection
- Unnecessary null checks for intermediate indices

### Complexity and Error-Prone Code
- Complex array resizing logic in multiple command classes
- Inconsistent validation patterns across commands
- Difficult to maintain and debug

### Mismatch with TypeScript Behavior
The TypeScript implementation uses sparse arrays naturally:
```typescript
state.video.mixEffects[10] = mixEffect  // Only creates entry at index 10
```

The C# array approach forced dense allocation, creating performance and memory issues not present in the reference implementation.

## Considered Options

### Option 1: Fixed-Size Arrays with Manual Resizing (Original)
```csharp
public MixEffect?[] MixEffects { get; set; } = [];
// Requires complex resizing logic in every command
```

**Pros:**
- Familiar array syntax
- Direct index access

**Cons:**
- Memory waste for sparse indices
- Performance overhead from array copying
- Complex and error-prone resizing logic
- Doesn't match TypeScript behavior

### Option 2: Pre-Allocated Large Arrays
```csharp
public MixEffect?[] MixEffects { get; set; } = new MixEffect?[100];
```

**Pros:**
- No resizing needed
- Simple access pattern

**Cons:**
- Massive memory waste
- Arbitrary size limits
- Still doesn't match TypeScript sparse behavior

### Option 3: Dictionary with Individual Utility Methods
```csharp
public Dictionary<int, MixEffect> MixEffects { get; set; } = new();
public static MixEffect GetMixEffect(AtemState state, int index) { /* ... */ }
public static DownstreamKeyer GetDownstreamKeyer(AtemState state, int index) { /* ... */ }
```

**Pros:**
- Proper sparse indexing
- No memory waste

**Cons:**
- Code duplication across types
- Separate utility method for each type
- Harder to maintain

### Option 4: Dictionary with Generic Extension Method (Selected)
```csharp
public Dictionary<int, MixEffect> MixEffects { get; set; } = [];
public static T GetOrCreate<T>(this Dictionary<int, T> dict, int index) where T : new()
```

**Pros:**
- Proper sparse indexing matching TypeScript
- Memory efficient - only stores accessed indices
- Single reusable method for all types
- Clean, maintainable code
- O(1) access time

**Cons:**
- Slight learning curve for developers used to arrays

## Decision

We selected **Option 4: Dictionary with Generic Extension Method** for the following reasons:

### Perfect TypeScript Compatibility
The Dictionary approach exactly matches the TypeScript behavior:
```csharp
// C# with Dictionary
var mixEffect = state.Video.MixEffects.GetOrCreate(5);  // Only creates index 5

// TypeScript equivalent  
const mixEffect = getMixEffect(state, 5);  // Only creates index 5
```

### Memory Efficiency
Only stores objects for indices that are actually accessed:
```csharp
var dict = new Dictionary<int, MixEffect>();
dict.GetOrCreate(0);    // Dictionary contains: {0: MixEffect}
dict.GetOrCreate(100);  // Dictionary contains: {0: MixEffect, 100: MixEffect}
// Indices 1-99 don't exist - no memory waste
```

### Performance Benefits
- O(1) access time with `TryGetValue`
- No array copying or resizing
- Minimal memory allocations

### Code Reusability
Single generic method works for all state types:
```csharp
var mixEffect = state.Video.MixEffects.GetOrCreate(index);
var keyer = state.Video.DownstreamKeyers.GetOrCreate(index);
var aux = state.Video.Auxiliaries.GetOrCreate(index);  // For value types
```

### Comprehensive Implementation
```csharp
/// <summary>
/// Gets an existing value from the dictionary or creates a new instance if the key doesn't exist.
/// This method provides sparse indexing behavior similar to JavaScript arrays.
/// </summary>
public static T GetOrCreate<T>(this Dictionary<int, T> dict, int index) where T : new()
{
    ArgumentNullException.ThrowIfNull(dict);
    
    if (dict.TryGetValue(index, out var value)) return value;
    value = new T();
    dict[index] = value;
    return value;
}
```

### Usage Patterns
- **Create if needed:** `dict.GetOrCreate(index)` - Always returns an instance
- **Check existence:** `dict.TryGetValue(index, out var value)` - Doesn't create if missing
- **Direct access:** `dict[index]` - When you know the key exists

## Consequences

### Positive
- ✅ **Memory efficient** - No wasted storage for unused indices
- ✅ **Performance optimized** - O(1) access, no array copying
- ✅ **TypeScript compatible** - Identical sparse indexing behavior
- ✅ **Maintainable** - Single generic method, no duplication
- ✅ **Type safe** - Generic constraints ensure proper usage
- ✅ **Well tested** - 12 comprehensive unit tests covering all scenarios

### Considerations
- 🔄 **API Change** - Requires updating existing command classes to use new pattern
- 📚 **Documentation** - Developers need to understand the GetOrCreate pattern
- 🧵 **Thread Safety** - Dictionary is not thread-safe (same as arrays)

### Migration Pattern
Commands were updated to use the new pattern:
```csharp
// Before: Complex array resizing
if (state.Video.MixEffects.Length <= index) {
    var newArray = new MixEffect?[index + 1];
    Array.Copy(state.Video.MixEffects, newArray, state.Video.MixEffects.Length);
    state.Video.MixEffects = newArray;
}
state.Video.MixEffects[index] ??= new MixEffect();

// After: Simple dictionary access
var mixEffect = AtemStateUtil.GetMixEffect(state, index);  // Uses GetOrCreate internally
```

## History

- **Deprecated** 2025-10-28 in favor of real arrays that are filled as soon as their size is known
- **Accepted** 2025-10-11
- **Proposed** 2025-10-11