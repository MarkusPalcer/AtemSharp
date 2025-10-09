# Events and State Management

This document details the event system and state management in AtemSharp.

## Event Types

The library provides several events for monitoring connection and state:

- **`Connected`** - Fired when connection is established
- **`Disconnected`** - Fired when connection is lost
- **`StateChanged`** - Fired when ATEM state changes
- **`ReceivedCommands`** - Fired when commands are received from ATEM
- **`Error`** - Fired when errors occur
- **`Info`** - Fired for informational messages

## Connection Events

### Connected Event
```csharp
atem.Connected += (sender, e) =>
{
    Console.WriteLine("Successfully connected to ATEM!");
    Console.WriteLine($"Model: {e.Model}");
    Console.WriteLine($"Version: {e.Version}");
};
```

### Disconnected Event
```csharp
atem.Disconnected += (sender, e) =>
{
    Console.WriteLine($"Disconnected from ATEM: {e.Reason}");
};
```

## State Change Events

### StateChanged Event
The `StateChanged` event is fired whenever the ATEM's state is modified:

```csharp
atem.StateChanged += (sender, e) =>
{
    var state = e.State;
    var changedPaths = e.ChangedPaths;
    
    Console.WriteLine($"State changed: {string.Join(", ", changedPaths)}");
    
    // Access specific state sections
    var me1 = state.Video.MixEffects.GetValueOrDefault(0);
    if (me1?.ProgramInput.HasValue == true)
        Console.WriteLine($"ME1 Program: {me1.ProgramInput}");
};
```

### Change Path Tracking
The `ChangedPaths` property contains the specific state paths that were modified:

```csharp
atem.StateChanged += (sender, e) =>
{
    foreach (var path in e.ChangedPaths)
    {
        switch (path)
        {
            case "video.mixEffects.0.programInput":
                Console.WriteLine("Program input changed");
                break;
            case "video.mixEffects.0.previewInput":
                Console.WriteLine("Preview input changed");
                break;
            case "audio.channels":
                Console.WriteLine("Audio channels updated");
                break;
        }
    }
};
```

## State Management

The ATEM state is automatically maintained as commands are received:

```csharp
atem.StateChanged += (sender, e) =>
{
    var state = e.State;
    
    // Access video state
    var videoState = state.Video;
    var mixEffect1 = videoState.MixEffects.GetValueOrDefault(0);
    
    // Access audio state
    var audioState = state.Audio;
    var audioChannels = audioState.Channels;
    
    // Access media state
    var mediaState = state.Media;
    var mediaPlayers = mediaState.Players;
};
```

### State Structure
The state object contains several main sections:

- **`Video`** - Mix effects, inputs, outputs
- **`Audio`** - Audio channels, levels, routing
- **`Media`** - Media players, stills, clips
- **`Macro`** - Macro definitions and status
- **`Recording`** - Recording state and settings
- **`Streaming`** - Streaming configuration and status

## Command Reception Events

### ReceivedCommands Event
Monitor all commands received from the ATEM:

```csharp
atem.ReceivedCommands += (sender, e) =>
{
    foreach (var command in e.Commands)
    {
        Console.WriteLine($"Received: {command.GetType().Name}");
    }
};
```

## Error and Info Events

### Error Event
Handle errors that occur during operation:

```csharp
atem.Error += (sender, e) =>
{
    Console.WriteLine($"Error: {e.Message}");
    if (e.Exception != null)
        Console.WriteLine($"Exception: {e.Exception}");
};
```

### Info Event
Receive informational messages:

```csharp
atem.Info += (sender, e) =>
{
    Console.WriteLine($"Info: {e.Message}");
};
```

## Thread Safety

All events are fired on background threads. If you need to update UI elements, marshal calls to the UI thread:

```csharp
atem.StateChanged += (sender, e) =>
{
    // For WPF applications
    Application.Current.Dispatcher.Invoke(() =>
    {
        // Update UI here
    });
    
    // For WinForms applications
    this.Invoke(() =>
    {
        // Update UI here
    });
};
```