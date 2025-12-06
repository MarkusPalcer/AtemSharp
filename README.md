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

For a quick start there are the following simple demo applications:

- [AtemSharp.Demo](AtemSharp.Demo/Program.cs) - Single file command line application that reads the state and executs the first two macros
- [AtemSharp.Demo.DependencyInjection](AtemSharp.Demo.DependencyInjection/Program.cs) - Same as above but registers the library using DI

## License

MIT License - see LICENSE file for details.
