// See https://aka.ms/new-console-template for more information

using AtemSharp;
using Newtonsoft.Json;

Console.WriteLine("=== AtemSharp Demo ===\n");

var atem = new Atem();
await atem.ConnectAsync("192.168.178.69");
Console.WriteLine("Connected, waiting 2s for data to come in ...");
Thread.Sleep(2000);
await atem.DisconnectAsync();
var state = atem.State;

// Serialize state to JSON
var stateJson = JsonConvert.SerializeObject(state, Formatting.Indented);

Console.WriteLine();
Console.WriteLine("Current state:");
Console.WriteLine(stateJson);

// Write state to file
await File.WriteAllTextAsync("state.json", stateJson);
Console.WriteLine($"State written to: {Path.GetFullPath("state.json")}");

Console.WriteLine();
Console.WriteLine("Unknown commands received:");
foreach (var cmd in Atem.UnknownCommands)
{
	Console.WriteLine($"  - {cmd}");
}

// Write unknown commands to file
var unknownCommandsText = string.Join(Environment.NewLine, Atem.UnknownCommands.Select(cmd => $"- {cmd}"));
await File.WriteAllTextAsync("unknown_commands.txt", unknownCommandsText);
Console.WriteLine($"Unknown commands written to: {Path.GetFullPath("unknown_commands.txt")}");
