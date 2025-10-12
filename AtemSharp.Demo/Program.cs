// See https://aka.ms/new-console-template for more information

using AtemSharp;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

Console.WriteLine("=== AtemSharp Demo ===\n");

// Create a console logger factory and logger
using var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddConsole().SetMinimumLevel(LogLevel.Information));
var logger = loggerFactory.CreateLogger<Atem>();

var atem = new Atem(logger);

var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(5));
try
{
	await atem.ConnectAsync("192.168.178.69", cancellationToken: cts.Token);
}
catch (TaskCanceledException)
{
	Console.WriteLine("Connection timed out.");
	return;
}

Console.WriteLine("Connected, waiting 2s for data to come in ...");
await Task.Delay(TimeSpan.FromSeconds(2));
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
