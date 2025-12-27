// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using AtemSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

var lib = new Library();


Console.WriteLine("=== AtemSharp Demo ===\n");
Console.WriteLine("Connects to the ATEM switcher and then captures its state");

var emergencyCts = new CancellationTokenSource();
if (!Debugger.IsAttached)
{
    emergencyCts.CancelAfter(TimeSpan.FromSeconds(20));
}

Console.CancelKeyPress += (_, _) => emergencyCts.Cancel();


var atem = lib.CreateAtemSwitcher("192.168.178.69");

Console.WriteLine("Connecting...");
await atem.ConnectAsync(cancellationToken: emergencyCts.Token);

// Serialize state to JSON
var stateJson = JsonConvert.SerializeObject(atem, Formatting.Indented,
                                            new JsonSerializerSettings
                                                { Converters = [new StringEnumConverter()], TypeNameHandling = TypeNameHandling.Auto });

// Write state to file
await File.WriteAllTextAsync("state.json", stateJson);
Console.WriteLine($"State written to: {Path.GetFullPath("state.json")}");

// Write unknown commands to file
var unknownCommandsText = string.Join(Environment.NewLine, AtemSwitcher.UnknownCommands.Select(cmd => $"- {cmd}"));
await File.WriteAllTextAsync("unknown_commands.txt", unknownCommandsText);
Console.WriteLine($"Unknown commands written to: {Path.GetFullPath("unknown_commands.txt")}");
