# AtemSharp

A C# library for connecting with Blackmagic Design ATEM switchers.

This is a C# port of the TypeScript [atem-connection](https://github.com/Sofie-Automation/sofie-atem-connection) library.

## Features

- Connect to ATEM switchers via UDP
- Send commands to the ATEM switcher
- Receive state updates from the ATEM switcher

## Installation

```bash
dotnet add package AtemSharp
```

## Quick Start


```csharp
using AtemSharp;
using AtemSharp.Commands.MixEffects;

// Create ATEM connection
using (var atem = new AtemSwitcher("192.168.1.240"));

// Connect to ATEM
await atem.ConnectAsync();

// Send commands
await atem.SendCommandsAsync([..., ...]);
await atem.SendCommandAsync(...);

// Access the state (Assumes that the ATEM switcher has already sent the initial values)
Console.WriteLine(atem.State.Video.MixEffects.Values.First().ProgramInput);

// Disconnect when done
await atem.DisconnectAsync();
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
- ✅ **Complete documentation** and examples
- ✅ Unit test framework setup
- 🔲 Complete command implementations
- 🔲 Unify how commands are initialized (don'T give all commands the whole AtemState but only their relevant sub-object and have that sub-object know its ID)
- 🔲 Full data transfer functionality
- 🔲 Full documentation comments
- ✅ Hardware validation (ie test with ATEM Mini ISO Pro)
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
