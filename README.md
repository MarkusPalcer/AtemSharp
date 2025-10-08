# AtemSharp

A C# library for connecting with Blackmagic Design ATEM switchers.

This is a C# port of the TypeScript [atem-connection](https://github.com/Sofie-Automation/sofie-atem-connection) library.

## Features

- Connect to ATEM switchers via UDP
- Send commands to control video switching
- Receive state updates from the ATEM
- Comprehensive enum definitions for ATEM protocols
- Event-driven architecture using .NET events
- Modern async/await patterns
- .NET 9.0 target framework

## Installation

```bash
dotnet add package AtemSharp
```

## Quick Start

```csharp
using AtemSharp;
using AtemSharp.Commands.MixEffects;

// Create ATEM connection
var atem = new Atem();

// Subscribe to events
atem.Connected += (sender, e) => Console.WriteLine("Connected to ATEM!");
atem.StateChanged += (sender, e) => 
{
    Console.WriteLine($"State changed: {string.Join(", ", e.ChangedPaths)}");
    
    // Access current state
    var me1 = e.State.Video.MixEffects.GetValueOrDefault(0);
    if (me1?.ProgramInput.HasValue == true)
        Console.WriteLine($"ME1 Program: {me1.ProgramInput}");
};

// Connect to ATEM
await atem.ConnectAsync("192.168.1.240");

// Send commands
await atem.SendCommandsAsync(new[] { 
    new CutCommand(0),  // Cut on ME1
    new ProgramInputCommand(0, 1),  // Set program to input 1
    new PreviewInputCommand(0, 2),  // Set preview to input 2
    new AutoTransitionCommand(0)    // Auto transition
});

// Disconnect when done
await atem.DisconnectAsync();
await atem.DestroyAsync();
```

## Architecture

The library is organized into several key namespaces:

- `AtemSharp` - Main connection classes (`Atem`, `BasicAtem`)
- `AtemSharp.Commands` - Command classes for controlling ATEM
- `AtemSharp.State` - State management classes
- `AtemSharp.Enums` - Enumerations for ATEM protocols and settings
- `AtemSharp.Net` - Low-level networking and socket handling

## Connection Options

```csharp
var atem = new Atem(new AtemOptions
{
    Address = "192.168.1.240",
    Port = 9910, // Default ATEM port
    DebugBuffers = false,
    MaxPacketSize = 1416 // Matching ATEM software default
});
```

## Events

The library provides several events for monitoring connection and state:

- `Connected` - Fired when connection is established
- `Disconnected` - Fired when connection is lost
- `StateChanged` - Fired when ATEM state changes
- `ReceivedCommands` - Fired when commands are received from ATEM
- `Error` - Fired when errors occur
- `Info` - Fired for informational messages

## Commands

Commands are strongly typed classes that can be sent to the ATEM:

```csharp
// Cut command
var cut = new CutCommand(mixEffect: 0);

// Multiple commands can be sent together
await atem.SendCommandsAsync(new ISerializableCommand[] { cut });
```

## State Management

The ATEM state is automatically maintained as commands are received:

```csharp
atem.StateChanged += (sender, e) =>
{
    var state = e.State;
    // Access current ATEM state
    // e.ChangedPaths contains the paths that were modified
};
```

## Status

🎉 **Major Update: Core Functionality Implemented!**

This is now a **functional** port of the TypeScript library with significant capabilities:

- ✅ Complete project structure and build configuration
- ✅ All core enum definitions ported (50+ enums)
- ✅ Command architecture with base classes implemented
- ✅ **Working networking layer** with UDP socket handling
- ✅ **Command parsing and serialization** 
- ✅ **Core ATEM commands**: Cut, Auto, Program/Preview Input
- ✅ **State management** for Mix Effects and device info
- ✅ **Version detection and handshake protocol**
- ✅ **Complete examples** and documentation
- ✅ Unit test framework setup
- 🔲 Complete command implementations (hundreds more commands)
- 🔲 Full data transfer functionality
- 🔲 Hardware validation

## Contributing

This library is part of the Sofie TV Automation system. Contributions are welcome!

## License

MIT License - see LICENSE file for details.