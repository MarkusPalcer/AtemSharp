# Commands Reference

This document provides detailed information about working with ATEM commands in AtemSharp.

## Command Types

AtemSharp provides two main types of commands:

- **Outgoing Commands** - Sent to the ATEM to control its state
- **Incoming Commands** - Received from the ATEM reporting state changes

## Sending Commands

Commands are strongly typed classes that can be sent to the ATEM:

```csharp
// Cut command
var cut = new CutCommand(mixEffect: 0);

// Multiple commands can be sent together
await atem.SendCommandsAsync(new ISerializableCommand[] { cut });
```

## Command Examples

### Mix Effects Commands

```csharp
// Program input selection
var program = new ProgramInputCommand(mixEffect: 0, input: 1);

// Preview input selection  
var preview = new PreviewInputCommand(mixEffect: 0, input: 2);

// Cut transition
var cut = new CutCommand(mixEffect: 0);

// Auto transition
var auto = new AutoTransitionCommand(mixEffect: 0);
```

### Audio Commands

```csharp
// Audio level adjustment
var audioLevel = new AudioLevelCommand(input: 1, level: -12.0);

// Audio mute
var audioMute = new AudioMuteCommand(input: 1, muted: true);
```

## Command Serialization

AtemSharp provides convenient extension methods for consistent boolean serialization:

```csharp
// Writing boolean values in command serialization
writer.WriteBoolean(enabled);                    // Writes 1 for true, 0 for false
writer.WriteBoolean(muted, inverted: true);      // Writes 0 for true, 1 for false

// Reading boolean values in command deserialization  
var enabled = reader.ReadBoolean();              // Any non-zero value = true
```

These helpers ensure consistent boolean handling across all ATEM commands, matching the protocol specification.

## Command Implementation

When implementing new commands, follow these patterns:

### Outgoing Commands
```csharp
public class MyCommand : SerializableCommand
{
    public override string RawName => "MYCM";
    
    public int Parameter1 { get; set; }
    public bool Parameter2 { get; set; }
    
    public override void Serialize(BinaryWriter writer)
    {
        writer.WriteUInt16BigEndian((ushort)Parameter1);
        writer.WriteBoolean(Parameter2);
    }
}
```

### Incoming Commands
```csharp
public class MyStatusCommand : DeserializableCommand
{
    public override string RawName => "MySt";
    
    public int Status { get; private set; }
    
    public override void Deserialize(BinaryReader reader)
    {
        Status = reader.ReadUInt16BigEndian();
    }
}
```

## Error Handling

Commands may fail for various reasons:
- Invalid parameters
- ATEM not connected
- Protocol errors
- Network issues

Always wrap command sending in try-catch blocks:

```csharp
try
{
    await atem.SendCommandsAsync(new[] { command });
}
catch (AtemConnectionException ex)
{
    Console.WriteLine($"Connection error: {ex.Message}");
}
catch (AtemProtocolException ex)
{
    Console.WriteLine($"Protocol error: {ex.Message}");
}
```