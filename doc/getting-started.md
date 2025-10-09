# Getting Started with AtemSharp

This guide provides detailed information on how to set up and use AtemSharp in your C# projects.

## Installation

```bash
dotnet add package AtemSharp
```

## Basic Usage

### Creating a Connection

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

## Connection Options

You can customize the connection behavior using the `AtemOptions` class:

```csharp
var atem = new Atem(new AtemOptions
{
    Address = "192.168.1.240",
    Port = 9910, // Default ATEM port
    DebugBuffers = false,
    MaxPacketSize = 1416 // Matching ATEM software default
});
```

### Available Options

- **Address**: IP address of the ATEM switcher
- **Port**: UDP port for communication (default: 9910)
- **DebugBuffers**: Enable detailed logging of network buffers
- **MaxPacketSize**: Maximum size of UDP packets (default: 1416 bytes)

## Next Steps

- [Architecture](architecture.md) - Understanding the library structure
- [Commands](commands.md) - Working with ATEM commands
- [Events](events.md) - Handling events and state changes