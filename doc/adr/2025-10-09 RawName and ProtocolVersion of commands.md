# RawName and ProtocolVersion of commands

The RawName and ProtocolVersion of a command are encoded in the CommandAttribute.
If the TypeScript version of the command does not have a ProtocolVersion set, none will be set in the CommandAttribute.
Helper-functions with caching are used to retrieve them for a given type via Reflection.

## Problem

When porting commands from the TypeScript sofie-atem-connection library to C# (AtemSharp), we needed to preserve the essential metadata that the TypeScript library uses for command identification and protocol version compatibility. 

In TypeScript, each command class has static properties:
- `rawName`: A 4-character string that identifies the command in the ATEM protocol (e.g., "CCmd", "RTMS", "_ver")
- `minimumVersion`: The minimum protocol version required for this command (optional)

These properties are critical for:
1. **Command parsing**: The `CommandParser` uses `rawName` to map incoming network packets to the correct command class
2. **Protocol version handling**: Commands with `minimumVersion` are only used when the ATEM device supports that protocol version or higher
3. **Command registration**: The parser automatically discovers all command classes by iterating through their static `rawName` properties
4. **Version-specific command selection**: When multiple command classes share the same `rawName` but support different protocol versions, the parser selects the appropriate one based on the current protocol version

The problem was how to make this metadata available in C# while maintaining the same functionality and ensuring that the porting process from TypeScript to C# remains straightforward and consistent.

## Considered options

### Option 1: Static Properties (Rejected)
Follow the exact TypeScript pattern with static properties on each command class:
```csharp
public class YourCommand : SerializedCommand
{
    public static readonly string RawName = "CAMH";
    public static readonly ProtocolVersion? MinimumVersion = ProtocolVersion.V8_1_1;
}
```

**Pros:**
- Direct 1:1 mapping from TypeScript
- No reflection needed
- Compile-time safety

**Cons:**
- Inconsistent with C# conventions (attributes are the idiomatic way to attach metadata to types)
- Would require manual registration of each command class
- Less discoverable than attributes
- Doesn't leverage C#'s reflection capabilities for automatic discovery

### Option 2: Interface-based Approach (Rejected)
Define an interface that commands must implement:
```csharp
public interface ICommandMetadata
{
    string RawName { get; }
    ProtocolVersion? MinimumVersion { get; }
}
```

**Pros:**
- Enforces implementation at compile time
- Type-safe access to metadata

**Cons:**
- Requires every command to implement the interface
- Forces runtime instantiation to access metadata
- Metadata is tied to instances rather than types
- Cannot be used for command discovery before instantiation

### Option 3: CommandAttribute with Reflection and Caching (Selected)
Use a custom attribute to attach metadata to command classes, with extension methods and caching for efficient access:
```csharp
[Command("CAMH", ProtocolVersion.V8_1_1)]
public class YourCommand : SerializedCommand { }
```

**Pros:**
- Follows C# conventions for type metadata
- Enables automatic command discovery through reflection
- Caching eliminates reflection performance overhead
- Clear, declarative syntax
- Supports optional protocol version (matches TypeScript behavior)
- Type-level metadata (no instantiation required)
- Easy to validate during porting (attribute is visible and easy to verify)

**Cons:**
- Requires reflection for initial discovery
- Slightly more complex implementation than static properties

### Option 4: Configuration-based Approach (Rejected)
Maintain a separate configuration file or registry that maps command types to their metadata:
```csharp
// In some configuration
commandRegistry.Register<YourCommand>("CAMH", ProtocolVersion.V8_1_1);
```

**Pros:**
- No reflection needed
- Centralized metadata management

**Cons:**
- Separates metadata from the command class definition
- Easy to forget to register new commands
- Difficult to maintain synchronization between TypeScript and C#
- Error-prone (no compile-time validation)

## Decision

We selected **Option 3: CommandAttribute with Reflection and Caching** because it provides the best balance of C# idiomatic design, functionality, and maintainability.

### Key benefits of the chosen approach:

1. **C# Conventions**: Attributes are the standard C# mechanism for attaching metadata to types, making this approach familiar to C# developers.

2. **Automatic Discovery**: Commands can be automatically discovered through reflection, eliminating the need for manual registration and reducing the chance of human error.

3. **Performance**: The `CommandExtensions` class uses cached dictionaries (`CommandRawNameCache` and `CommandMinimumVersionCache`) to avoid repeated reflection calls, ensuring that metadata access is fast after the first lookup.

4. **Ease of Porting**: During the porting process from TypeScript to C#, developers can easily map:
   - `public static readonly rawName = 'CAMH'` → `[Command("CAMH")]`
   - `public static readonly minimumVersion = ProtocolVersion.V8_1_1` → `[Command("CAMH", ProtocolVersion.V8_1_1)]`

5. **Optional Protocol Version**: The attribute constructor supports both forms, matching the TypeScript behavior where `minimumVersion` is optional.

6. **Type Safety**: The extension methods `GetRawName()` and `GetMinimumVersion()` provide type-safe access to the metadata without requiring command instantiation.

7. **Validation**: The attribute is visible in the class declaration, making it easy to verify that the porting was done correctly.

### Implementation details:

```csharp
// Attribute definition
[Command("CAMH")]  // Without minimum version
[Command("RTMS", ProtocolVersion.V8_1_1)]  // With minimum version

// Usage through extension methods
string? rawName = command.GetRawName();
ProtocolVersion? minVersion = command.GetMinimumVersion();

// Or for types
string? rawName = CommandExtensions.GetRawName<YourCommand>();
ProtocolVersion? minVersion = CommandExtensions.GetMinimumVersion<YourCommand>();
```

This approach successfully replicates the TypeScript functionality while following C# best practices and providing excellent performance through caching.

## History

- Accepted 2025-10-09
- Proposed 2025-10-09