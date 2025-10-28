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

## Documentation

For detailed information, see the documentation in the `doc/` folder:

- **[Getting Started](doc/getting-started.md)** - Installation, basic usage, and connection options
- **[Architecture](doc/architecture.md)** - Library structure and design patterns
- **[Commands](doc/commands.md)** - Working with ATEM commands and serialization
- **[Events & State](doc/events.md)** - Event handling and state management

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
- ✅ **Complete documentation** and examples
- ✅ Unit test framework setup
- 🔲 Complete command implementations
- 🔲 Unify how commands are initialized (don'T give all commands the whole AtemState but only their relevant sub-object and have that sub-object know its ID)
- 🔲 Full data transfer functionality
- 🔲 Full documentation comments
- 🔲 Hardware validation (ie test with ATEM Mini ISO Pro)
- 🔲 Publish repo to GitHub (and create issues for additional work)
- 🔲 Publish 0.1 version to NuGet
- 🔲 Split version aware commands
- 🔲 Add validation for ranges on serialized command property setters
- 🔲 Refactoring of the state structure
- 🔲 Abstraction layer to remove manual creation of commands
- 🔲 (Real) Sample application
- 🔲 Publish 1.0 version to NuGet



## Contributing

See the [development documentation](doc/) for detailed guides on extending the library.

## License

MIT License - see LICENSE file for details.
