# Porting Commands from TypeScript to C#

This guide provides a comprehensive step-by-step process for converting TypeScript ATEM commands to C# for the AtemSharp project.

## Overview

The AtemSharp project aims to provide C# implementations of ATEM commands that are compatible with the original TypeScript implementation. There are two main types of commands:

- **WritableCommands** (Outgoing): Commands sent to the ATEM device
- **DeserializedCommands** (Incoming): Commands received from the ATEM device

## 🎯 Phase 1: Analysis & Preparation

### 1.1 Identify the TypeScript Command

- Locate the command file in `src/commands/` directory
- Note if it's a `WritableCommand` (outgoing) or `DeserializedCommand` (incoming)
- Check the `rawName` property (e.g., "CAMH", "AMHP")
- Review the command's properties and their types

### 1.2 Analyze Command Structure

**For WritableCommands:**
- **Properties**: What data does the command contain?
- **MaskFlags**: What flags are available for selective updates?
- **Serialization logic**: How is data packed into bytes?

**For DeserializedCommands:**
- **Properties**: What data is extracted from the command?
- **Deserialization logic**: How are bytes unpacked into properties?
- **State application**: How does it update the ATEM state?

### 1.3 Check Test Data

- Look for test data in the TypeScript test files or `libatem-data.json`
- This will help validate your C# implementation against known good data

## 🏗️ Phase 2: Create C# Command Classes

### 2.1 WritableCommands (Outgoing)

WritableCommands are used to send data to the ATEM device. They extend `WritableCommand<T>` and implement serialization logic.

```csharp
using AtemSharp.Commands3;
using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Commands3.YourNamespace;

/// <summary>
/// Command to update [describe what this command does]
/// </summary>
[Command("CAMH")] // Use the rawName from TypeScript
public class YourCommand : WritableCommand<YourCommand>
{
    /// <summary>
    /// [Property description] in [units], [range]
    /// </summary>
    [CommandProperty(1 << 0, 0)] // maskFlag, field index
    public double? PropertyName { get; set; }

    /// <summary>
    /// [Another property description]
    /// </summary>
    [CommandProperty(1 << 1, 1)]
    public int? AnotherProperty { get; set; }

    // Internal properties that hold the actual values to write (like TypeScript this.properties)
    public double ActualPropertyName { get; set; } = 0.0;
    public int ActualAnotherProperty { get; set; } = 0;

    /// <inheritdoc />
    public override Stream Serialize(ProtocolVersion version)
    {
        var buffer = new byte[12]; // Commands3 typically use 12-byte payloads
        
        // Calculate flag based on which properties are explicitly set (not null)
        byte flag = 0;
        if (PropertyName.HasValue) flag |= 1 << 0;
        if (AnotherProperty.HasValue) flag |= 1 << 1;
        
        // Write flag at byte 0
        buffer[0] = flag;
        
        // Write properties using appropriate converters
        BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(2), 
            AtemUtil.DecibelToUInt16BE(ActualPropertyName));
        BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(4), 
            (ushort)ActualAnotherProperty);
        
        return new MemoryStream(buffer);
    }
}
```

### 2.2 DeserializedCommands (Incoming)

DeserializedCommands are used to receive and parse data from the ATEM device. They implement `IDeserializedCommand`.

```csharp
using AtemSharp.Commands3;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands3.YourNamespace;

/// <summary>
/// Update command for [describe what this updates]
/// </summary>
[Command("AMHP")] // Use the rawName from TypeScript
public class YourUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// [Property description] in [units], [range]
    /// </summary>
    public double PropertyName { get; set; }

    /// <summary>
    /// [Another property description]
    /// </summary>
    public int AnotherProperty { get; set; }
    
    public static YourUpdateCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
        
        return new YourUpdateCommand
        {
            PropertyName = AtemUtil.UInt16BEToDecibel(reader.ReadUInt16BE()),
            AnotherProperty = reader.ReadUInt16BE(),
            // Read other properties as needed
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Validate state prerequisites
        if (state.YourStateSection == null)
        {
            throw new InvalidIdError("State Section", "identifier");
        }

        // Update the state object
        state.YourStateSection.PropertyName = PropertyName;
        state.YourStateSection.AnotherProperty = AnotherProperty;
        
        // Return the state path that was modified for change tracking
        return new[] { "yourStateSection.propertyName", "yourStateSection.anotherProperty" };
    }
}
```

## 🧪 Phase 3: Create Test Classes

### 3.1 WritableCommand Tests

```csharp
using YourCommand = AtemSharp.Commands3.YourNamespace.YourCommand;

namespace AtemSharp.Tests.Commands3;

[TestFixture]
public class YourCommandTests : SerializedCommandTestBase<YourCommand, YourCommandTests.CommandData>
{
    /// <summary>
    /// Specify which byte ranges contain floating-point encoded data
    /// that should be compared with tolerance for precision differences
    /// </summary>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return new[] { 2..6 }; // Example: bytes 2-5 contain floating-point data
    }

    public class CommandData : CommandDataBase
    {
        public double PropertyName { get; set; }
        public int AnotherProperty { get; set; }
    }

    protected override YourCommand CreateSut(TestCaseData testCase)
    {
        var command = new YourCommand();

        // Set the actual values that should be written (like TypeScript this.properties)
        command.ActualPropertyName = testCase.Command.PropertyName;
        command.ActualAnotherProperty = testCase.Command.AnotherProperty;

        // Set nullable properties only for those indicated by mask (for flag calculation)
        if ((testCase.Command.Mask & (1 << 0)) != 0) 
            command.PropertyName = testCase.Command.PropertyName;
        if ((testCase.Command.Mask & (1 << 1)) != 0) 
            command.AnotherProperty = testCase.Command.AnotherProperty;

        return command;
    }
}
```

### 3.2 DeserializedCommand Tests

```csharp
using AtemSharp.Commands3.YourNamespace;

namespace AtemSharp.Tests.Commands3;

[TestFixture]
public class YourUpdateCommandTests : DeserializedCommandTestBase<YourUpdateCommand, YourUpdateCommandTests.CommandData>
{
    /// <summary>
    /// Specify which properties contain floating-point values that should be compared with tolerance
    /// </summary>
    protected override string[] GetFloatingPointProperties()
    {
        return new[] { "PropertyName" }; // Properties that are floating-point values
    }

    public class CommandData : CommandDataBase
    {
        public double PropertyName { get; set; }
        public int AnotherProperty { get; set; }
    }

    protected override void CompareCommandProperties(YourUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var floatingPointProps = GetFloatingPointProperties();
        var failures = new List<string>();

        // Compare PropertyName
        if (floatingPointProps.Contains("PropertyName"))
        {
            if (!AreApproximatelyEqual(actualCommand.PropertyName, expectedData.PropertyName))
            {
                failures.Add($"PropertyName: expected {expectedData.PropertyName}, actual {actualCommand.PropertyName}");
            }
        }
        else if (!actualCommand.PropertyName.Equals(expectedData.PropertyName))
        {
            failures.Add($"PropertyName: expected {expectedData.PropertyName}, actual {actualCommand.PropertyName}");
        }

        // Compare AnotherProperty
        if (!actualCommand.AnotherProperty.Equals(expectedData.AnotherProperty))
        {
            failures.Add($"AnotherProperty: expected {expectedData.AnotherProperty}, actual {actualCommand.AnotherProperty}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
        
        Assert.Pass($"All properties match for version {testCase.FirstVersion}");
    }
}
```

## 🔄 Phase 4: Data Type Conversions

### 4.1 Common TypeScript ↔ C# Mappings

| TypeScript Type | C# (Writable) | C# (Deserialized) | Serialization Helper | Notes |
|-----------------|---------------|-------------------|---------------------|-------|
| `number` (decibel) | `double?` | `double` | `AtemUtil.DecibelToUInt16BE` / `UInt16BEToDecibel` | Audio gain values |
| `number` (integer) | `int?` / `ushort?` | `int` / `ushort` | `reader.ReadUInt16BE()` / `WriteUInt16BE()` | Standard integers |
| `number` (byte) | `byte?` | `byte` | `reader.ReadByte()` / Direct assignment | Single byte values |
| `boolean` | `bool?` | `bool` | Convert to/from byte flags | Usually packed in flag bytes |
| `enum` values | `EnumType?` | `EnumType` | Cast from/to underlying type | Custom enums |

### 4.2 Serialization Helpers

**Reading Data (Deserialized Commands):**
```csharp
// Read big-endian 16-bit integers
var value = reader.ReadUInt16BE();

// Read decibel values
var decibel = AtemUtil.UInt16BEToDecibel(reader.ReadUInt16BE());

// Read boolean from flag
var flag = reader.ReadByte();
var boolValue = (flag & (1 << bitIndex)) != 0;
```

**Writing Data (Writable Commands):**
```csharp
// Write big-endian 16-bit integers
BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(offset), value);

// Write decibel values
BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(offset), 
    AtemUtil.DecibelToUInt16BE(decibelValue));

// Write boolean to flag
byte flag = 0;
if (boolValue) flag |= 1 << bitIndex;
buffer[0] = flag;
```

## ✅ Phase 5: Testing & Validation

### 5.1 Run Tests

```bash
# Run tests for your specific command
dotnet test --filter "FullyQualifiedName~YourCommand"

# Run all Commands3 tests
dotnet test --filter "FullyQualifiedName~Commands3"
```

### 5.2 Common Issues to Check

- **Byte order**: ATEM uses big-endian encoding for multi-byte values
- **Buffer size**: Must match exactly (Commands3 typically use 12-byte payloads)
- **Floating-point precision**: Allow tolerance for decibel conversions due to precision differences
- **Null handling**: WritableCommands use nullable properties, DeserializedCommands typically don't
- **Flag calculation**: Only set flag bits for explicitly provided properties in WritableCommands
- **State paths**: Return correct state modification paths for change tracking

### 5.3 Validation Checklist

1. ✅ **Serialization test passes** - your C# matches the expected byte output from test data
2. ✅ **Deserialization test passes** - your C# correctly parses the byte input from test data
3. ✅ **State application works** - for DeserializedCommands, state updates correctly
4. ✅ **No compile errors** - all references and types are correct
5. ✅ **Documentation complete** - XML doc comments explain the command's purpose

## 🎯 Phase 6: Integration

### 6.1 Command Registration

Commands are automatically discovered through reflection using the `[Command("RAWNAME")]` attribute. Ensure:

- The `[Command]` attribute has the correct raw name matching the TypeScript version
- The class is in the appropriate namespace under `AtemSharp.Commands3`
- The class is `public` and properly inherits from the base class

### 6.2 Documentation

- Add comprehensive XML doc comments explaining the command's purpose
- Document property units, ranges, and special considerations
- Include any limitations or version requirements
- Reference related commands or state objects where appropriate

## 📚 Additional Resources

### Useful Base Classes and Interfaces

- `WritableCommand<T>` - Base for outgoing commands
- `IDeserializedCommand` - Interface for incoming commands
- `CommandTestBase<T>` - Common test functionality
- `SerializedCommandTestBase<TCommand, TTestData>` - Tests for WritableCommands
- `DeserializedCommandTestBase<TCommand, TTestData>` - Tests for DeserializedCommands

### Key Utility Classes

- `AtemUtil` - Conversion helpers for ATEM-specific data formats
- `BinaryPrimitives` - Big-endian read/write operations
- `InvalidIdError` - Exception for invalid state references

### Example Commands to Reference

- `AudioMixerHeadphonesCommand` - Simple WritableCommand with multiple properties
- `AudioMixerHeadphonesUpdateCommand` - Simple DeserializedCommand with state updates
- Their corresponding test classes for implementation patterns

## 🚀 Best Practices

1. **Follow Existing Patterns**: Use the established naming conventions and code structure
2. **Test-Driven Development**: Write tests based on existing test data before implementing
3. **Incremental Development**: Start with basic functionality, then add edge cases
4. **Documentation First**: Write clear XML documentation as you implement
5. **Validate Against TypeScript**: Ensure byte-for-byte compatibility with the original implementation

---

This guide should provide everything needed to successfully port TypeScript ATEM commands to C#. When in doubt, refer to existing implementations and test patterns for guidance.