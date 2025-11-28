// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using AtemSharp;
using AtemSharp.Commands.Macro;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

Console.WriteLine("=== AtemSharp Demo ===\n");

// Create a console logger factory and logger
using var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddConsole().SetMinimumLevel(LogLevel.Debug));


var emergencyCts = new CancellationTokenSource();
if (!Debugger.IsAttached)
{
    emergencyCts.CancelAfter(TimeSpan.FromSeconds(20));
}

Console.CancelKeyPress += (_, _) => emergencyCts.Cancel();

var atem = new AtemSwitcher("192.168.178.69", loggerFactory: loggerFactory);
atem.ConnectionStateChanged += (_, args) => Console.WriteLine($"Connection state changed from {args.OldState} to {args.NewState}");

Console.WriteLine("Connecting...");
await atem.ConnectAsync(cancellationToken: emergencyCts.Token);

Console.WriteLine("Waiting 2s for data to come in...");
await Task.Delay(TimeSpan.FromSeconds(2), emergencyCts.Token);

Console.WriteLine($"Executing Macro {atem.State.Macros.Macros[0].Name} ...");
await atem.SendCommandAsync(new MacroActionCommand(atem.State.Macros.Macros[0], MacroAction.Run));

var state = atem.State;

// Serialize state to JSON
var stateJson = JsonConvert.SerializeObject(state, Formatting.Indented, new JsonSerializerSettings { Converters = [new StringEnumConverter()], TypeNameHandling = TypeNameHandling.Auto});

// Write state to file
await File.WriteAllTextAsync("state.json", stateJson);
Console.WriteLine($"State written to: {Path.GetFullPath("state.json")}");

// Write unknown commands to file
var unknownCommandsText = string.Join(Environment.NewLine, AtemSwitcher.UnknownCommands.Select(cmd => $"- {cmd}"));
await File.WriteAllTextAsync("unknown_commands.txt", unknownCommandsText);
Console.WriteLine($"Unknown commands written to: {Path.GetFullPath("unknown_commands.txt")}");

Console.WriteLine("Disconnecting...");
await atem.DisconnectAsync();

Console.WriteLine("Reconnecting...");
await atem.ConnectAsync( cancellationToken: emergencyCts.Token);

Console.WriteLine("Waiting 2s for data to come in ...");
await Task.Delay(TimeSpan.FromSeconds(2), emergencyCts.Token);

Console.WriteLine($"Executing Macro {atem.State.Macros.Macros[1].Name} ...");
await atem.SendCommandAsync(new MacroActionCommand(atem.State.Macros.Macros[1], MacroAction.Run));


Console.WriteLine("Disconnecting...");
await atem.DisconnectAsync();
