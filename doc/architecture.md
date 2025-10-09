# AtemSharp Architecture

This document describes the internal architecture and organization of the AtemSharp library.

## Namespace Organization

The library is organized into several key namespaces:

- **`AtemSharp`** - Main connection classes (`Atem`, `BasicAtem`)
- **`AtemSharp.Commands`** - Command classes for controlling ATEM
- **`AtemSharp.State`** - State management classes
- **`AtemSharp.Enums`** - Enumerations for ATEM protocols and settings
- **`AtemSharp.Net`** - Low-level networking and socket handling

## Core Components

### Connection Layer
- **`Atem`** - High-level connection manager with full state tracking
- **`BasicAtem`** - Lightweight connection for simple command sending
- **Socket Management** - UDP socket handling and packet management

### Command System
- **Base Classes** - `SerializableCommand`, `DeserializableCommand`
- **Command Parsing** - Binary protocol parsing and serialization
- **Type Safety** - Strongly typed command parameters

### State Management
- **Automatic Updates** - ATEM state is maintained automatically
- **Change Tracking** - Detailed change notifications with paths
- **Thread Safety** - Concurrent access protection

### Protocol Implementation
- **Version Detection** - Automatic ATEM protocol version detection
- **Handshake Process** - Connection establishment and initialization
- **Error Handling** - Robust error recovery and reporting

## Design Patterns

### Event-Driven Architecture
The library uses .NET events for asynchronous communication:
- Connection state changes
- ATEM state updates
- Command reception
- Error notifications

### Async/Await Support
All I/O operations use modern async patterns:
- Non-blocking connection establishment
- Asynchronous command sending
- Background state monitoring

### Command Pattern
Commands are implemented as discrete objects:
- Encapsulation of command data
- Serialization/deserialization logic
- Type-safe parameter validation