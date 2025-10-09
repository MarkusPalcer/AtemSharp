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
- It also gives you a hint for the actual types of the properties as it contains data assigned to them or expected from them

## 🏗️ Phase 2: Create C# Command Classes

### 2.1 WritableCommands (Outgoing)

WritableCommands are used to send data to the ATEM device. They extend `SerializedCommand` and implement serialization logic with automatic flag management.

```csharp
using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.YourNamespace;

/// <summary>
/// Command to update [describe what this command does]
/// </summary>
[Command("CAMH")] // Use the rawName from TypeScript
public class YourCommand : SerializedCommand
{
    private double _propertyName;
    private int _anotherProperty;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if required state not available</exception>
    public YourCommand(AtemState currentState)
    {
        // If old state does not exist, set Properties (instead of backing fields) to default values,
        // so all flags are set (i.e. all values are to be applied by the ATEM)
        if (currentState.Audio?.Headphones == null)
        {
            PropertyName = 0.0;
            AnotherPropertiy = 10;
            return;
        }

        var audioData = currentState.Audio.Headphones;
        
        // Initialize from current state (direct field access = no flags set)
        _propertyName = audioData.PropertyName;
        _anotherProperty = audioData.AnotherProperty;
    }

    /// <summary>
    /// [Property description] in [units], [range]
    /// </summary>
    public double PropertyName
    {
        get => _propertyName;
        set
        {
            // Validation (if applicable)
            if (value < -60.0 || value > 6.0) {
                throw new ArgumentOutOfRangeException(nameof(value), "PropertyName must be between -60.0 and +6.0");
            }
            
            _propertyName = value;
            Flag |= 1 << 0;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// [Another property description]
    /// </summary>
    public int AnotherProperty
    {
        get => _anotherProperty;
        set
        {
            _anotherProperty = value;
            Flag |= 1 << 1;  // Automatic flag setting!
        }
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(6);
        using var writer = new BinaryWriter(memoryStream);
        
        // Write flag as single byte (matching TypeScript pattern)
        writer.Write((byte)Flag);
        writer.Pad(1); // Explicit padding when needed
        
        // Write all property values using extension methods
        writer.WriteUInt16BigEndian(AtemUtil.DecibelToUInt16BE(PropertyName));
        writer.WriteUInt16BigEndian((ushort)AnotherProperty);
        
        return memoryStream.ToArray();
    }
}
```

**Key Improvements in Modern Serialization Pattern:**
- ✅ **Return `byte[]` directly**: More efficient than returning `MemoryStream`
- ✅ **Extension methods**: Use `writer.WriteUInt16BigEndian()` instead of `BinaryWriterExtensions.WriteUInt16BE(writer, ...)`
- ✅ **Explicit padding**: Use `writer.Pad(n)` for clearer intent and readability
- ✅ **Flag as byte**: Write `(byte)Flag` to match TypeScript single-byte flag pattern
- ✅ **Simplified buffer management**: Direct `ToArray()` call eliminates `leaveOpen` complexity
- ✅ **Simplified naming**: Removed "BE" suffix since ATEM protocol is exclusively big-endian

**Available Extension Methods:**
```csharp
// From SerializationExtensions class
writer.Pad(uint length)           // Write 'length' zero bytes for padding
writer.WriteUInt16BigEndian(ushort)        // Write 16-bit unsigned (always big-endian)
writer.WriteInt16BigEndian(short)          // Write 16-bit signed (always big-endian)

// Corresponding read methods
reader.ReadUInt16BigEndian()               // Read 16-bit unsigned (always big-endian)
reader.ReadInt16BigEndian()                // Read 16-bit signed (always big-endian)
```

**Complete Serialization Example (Based on AudioMixerInputCommand):**
```csharp
public override byte[] Serialize(ProtocolVersion version)
{
    using var memoryStream = new MemoryStream(12);
    using var writer = new BinaryWriter(memoryStream);
    
    // Flag always written as single byte first
    writer.Write((byte)Flag);
    writer.Pad(1);                                    // Pad to align with TypeScript
    
    // Write index/identifier (when present)
    writer.WriteUInt16BigEndian(Index);
    
    // Write enum values as bytes with padding as needed
    writer.Write((byte)MixOption);
    writer.Pad(1);                                    // Pad before next multi-byte value
    
    // Write computed values using utilities
    writer.WriteUInt16BigEndian(AtemUtil.DecibelToUInt16BE(Gain));
    writer.WriteInt16BigEndian(AtemUtil.BalanceToInt(Balance));
    
    // Write boolean values
    writer.WriteBoolean(RcaToXlrEnabled);
    writer.Pad(1);                                    // Final padding if needed
    
    return memoryStream.ToArray();
}
```

**Key Benefits of This Pattern:**
- ✅ **State Initialization**: Command starts with current ATEM values automatically
- ✅ **Automatic Flags**: No need to manually manage flags or call `UpdateProps`
- ✅ **Type Safety**: No nullable properties since initialization guarantees values
- ✅ **Validation**: State validation in constructor prevents invalid commands
- ✅ **Simple Usage**: Just set properties, flags are handled automatically

**Usage Pattern:**
```csharp
// Create command with current state
var command = new YourCommand(currentState);

// Change only what you want - flags set automatically
command.PropertyName = newValue;      // Flag 0x01 set automatically
command.AnotherProperty = newValue2;  // Flag 0x02 set automatically

// Send command
await atem.SendCommand(command);
```

### 2.2 DeserializedCommands (Incoming)

DeserializedCommands are used to receive and parse data from the ATEM device. They inherit from `DeserializedCommand`.

```csharp
using AtemSharp.Commands;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.YourNamespace;

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
            PropertyName = AtemUtil.UInt16BEToDecibel(BinaryReaderExtensions.ReadUInt16BE(reader)),
            AnotherProperty = BinaryReaderExtensions.ReadUInt16BE(reader),
            // Read other properties as needed
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Validate state prerequisites (same pattern as TypeScript update commands)
        if (state.Audio == null)
        {
            throw new InvalidIdError("Classic Audio", "");
        }

        // Update the state object (mirroring TypeScript applyToState logic)
        if (state.Audio.Headphones == null)
            state.Audio.Headphones = new ClassicAudioHeadphoneOutputChannel();
            
        state.Audio.Headphones.PropertyName = PropertyName;
        state.Audio.Headphones.AnotherProperty = AnotherProperty;
        
        // Return the state path that was modified for change tracking
        return new[] { "audio.headphones.propertyName", "audio.headphones.anotherProperty" };
    }
}
```

## 🧪 Phase 3: Create Test Classes

### 3.1 WritableCommand Tests

```csharp
using YourCommand = AtemSharp.Commands.YourNamespace.YourCommand;

namespace AtemSharp.Tests.Commands;

[TestFixture]
public class YourCommandTests : SerializedCommandTestBase<YourCommand, YourCommandTests.CommandData>
{
    /// </inheritdoc>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return [
            2..4, // Some field
            6..9 // Another field
        ]; 
    }

    public class CommandData : CommandDataBase
    {
        public double PropertyName { get; set; }
        public int AnotherProperty { get; set; }
    }

    protected override YourCommand CreateSut(TestCaseData testCase)
    {
        var command = new YourCommand(CreateMinimalStateForTesting());

        // Set all properties from test data (flags are set from the base class)
        command.PropertyName = testCase.Command.PropertyName;
        command.AnotherProperty = testCase.Command.AnotherProperty;

        return command;
    }

    private static AtemState CreateMinimalStateForTesting()
    {
        return new AtemState
        {
            YourStateSection = new YourStateType
            {
                PropertyName = 0.0,  // Default values
                AnotherProperty = 0  // Default values
            }
        };
    }
}
```

### 3.2 DeserializedCommand Tests

```csharp
using AtemSharp.Commands.YourNamespace;

namespace AtemSharp.Tests.Commands;

[TestFixture]
public class YourUpdateCommandTests : IDeserializedCommandTestBase<YourUpdateCommand, YourUpdateCommandTests.CommandData>
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

        // Compare PropertyName - it is a floating point value so we approximate
        if (!AreApproximatelyEqual(actualCommand.PropertyName, expectedData.PropertyName))
        {
            failures.Add($"PropertyName: expected {expectedData.PropertyName}, actual {actualCommand.PropertyName}");
        }

        // Compare AnotherProperty - it is not floating point so it needs to equal
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
    }
}
```

## 🔄 Phase 4: Data Type Conversions

### 4.1 Common TypeScript ↔ C# Mappings

| TypeScript Type | C# (Writable) | C# (Deserialized) | Serialization Helper | Notes |
|-----------------|---------------|-------------------|---------------------|-------|
| `number` (decibel) | `double` | `double` | `AtemUtil.DecibelToUInt16BE` / `UInt16BEToDecibel` | Audio gain values, validation in setter |
| `number` (percentage) | `double` | `double` | `/100` | Serialized as integer value with the unit 0.01% |
| `number` (integer) | `int` / `ushort` | `int` / `ushort` | `writer.WriteUInt16BigEndian()` / `reader.ReadUInt16BigEndian()` | Standard integers (always big-endian) |
| `number` (byte) | `byte` | `byte` | Direct assignment | Single byte values |
| `boolean` | `bool` | `bool` | Convert to/from byte flags | Usually packed in flag bytes |
| `enum` values | `EnumType` | `EnumType` | Cast from/to underlying type | Custom enums, validation in setter |

**Key Differences from Old Pattern:**
- ✅ **No Nullable Types**: Writable commands use non-nullable types since they're initialized from state
- ✅ **Validation in Setters**: Range validation happens when properties are set
- ✅ **Automatic Flags**: No manual flag calculation needed

### 4.2 Serialization Helpers

**Reading Data (Deserialized Commands):**
```csharp
// Read big-endian 16-bit integers using extension methods
var value = reader.ReadUInt16BigEndian();

// Read decibel values
var decibel = AtemUtil.UInt16BEToDecibel(reader.ReadUInt16BigEndian());

// Read boolean from flag
var flag = reader.ReadByte();
var boolValue = (flag & (1 << bitIndex)) != 0;
```

**Writing Data (Writable Commands):**
```csharp
// Write big-endian 16-bit integers using extension methods
writer.WriteUInt16BigEndian(value);

// Write decibel values using utility and extension methods
writer.WriteUInt16BigEndian(AtemUtil.DecibelToUInt16BE(decibelValue));

// Write single bytes with explicit padding when needed
writer.Write((byte)enumValue);
writer.Pad(1); // Add padding byte if required by protocol

// Write boolean values
writer.WriteBoolean(boolValue);

// Write flags as single byte (not full int)
writer.Write((byte)Flag);
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
- **Buffer size**: Must match exactly (typically 12-byte payloads for most commands)
- **Floating-point precision**: Allow tolerance for decibel conversions due to precision differences
- **State initialization**: Writable commands must initialize from current ATEM state
- **Flag management**: Flags are set automatically when properties change
- **State paths**: Return correct state modification paths for change tracking
- **Validation**: Property setters should validate ranges and throw appropriate exceptions

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
- The class is in the appropriate namespace under `AtemSharp.Commands`
- The class is `public` and properly inherits from the base class (`SerializedCommand` for writable, `IDeserializedCommand` for incoming)
- If a MinimalProtocolVersion is given, it is also added to the CommandAttribute like this: `[Command("RAWNAME", ProtocolVersion.V3)]`

### 6.2 Documentation

- Add comprehensive XML doc comments explaining the command's purpose
- Document property units, ranges, and special considerations
- Include any limitations or version requirements
- Reference related commands or state objects where appropriate

## 📚 Additional Resources

### Useful Base Classes and Interfaces

- `SerializedCommand` - Base for outgoing writable commands
- `IDeserializedCommand` - Interface for incoming commands
- `CommandTestBase<T>` - Common test functionality
- `SerializedCommandTestBase<TCommand, TTestData>` - Tests for WritableCommands
- `DeserializedCommandTestBase<TCommand, TTestData>` - Tests for DeserializedCommands

### Key Utility Classes

- `AtemUtil` - Conversion helpers for ATEM-specific data formats
- `BinaryWriterExtensions` / `BinaryReaderExtensions` - Big-endian read/write operations
- `InvalidIdError` - Exception for invalid state references

### Example Commands to Reference

- `AudioMixerHeadphonesCommand` - Simple WritableCommand with state initialization and automatic flags
- `AudioMixerInputCommand` - WritableCommand with validation and multiple properties
- `AudioMixerHeadphonesUpdateCommand` - Simple DeserializedCommand with state updates
- Their corresponding test classes for implementation patterns

## 🚀 Best Practices

1. **State-First Design**: Always initialize commands from current ATEM state
2. **Automatic Flag Management**: Let property setters handle flags automatically
3. **Validation in Setters**: Validate ranges and constraints when properties are set
4. **Follow Existing Patterns**: Use the established naming conventions and code structure
5. **Test-Driven Development**: Write tests based on existing test data before implementing
6. **Incremental Development**: Start with basic functionality, then add edge cases
7. **Documentation First**: Write clear XML documentation as you implement
8. **Validate Against TypeScript**: Ensure byte-for-byte compatibility with the original implementation

---

This guide should provide everything needed to successfully port TypeScript ATEM commands to C#. When in doubt, refer to existing implementations and test patterns for guidance.