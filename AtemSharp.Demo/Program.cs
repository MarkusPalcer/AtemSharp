// See https://aka.ms/new-console-template for more information

using AtemSharp;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

Console.WriteLine("=== AtemSharp Demo ===\n");

// Create a console logger factory and logger
using var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddConsole().SetMinimumLevel(LogLevel.Information));
var logger = loggerFactory.CreateLogger<Atem>();

var atem = new Atem(logger);

var emergencyCts = new CancellationTokenSource();
emergencyCts.CancelAfter(TimeSpan.FromSeconds(10));
Console.CancelKeyPress += (_, _) => emergencyCts.Cancel();

Console.WriteLine("Attempting to connect to 192.168.178.69:9910...");
await atem.ConnectAsync("192.168.178.69", cancellationToken: emergencyCts.Token);
Console.WriteLine("Connected, waiting 2s for data to come in ...");
await Task.Delay(TimeSpan.FromSeconds(2), emergencyCts.Token);

var state = atem.State;

// Serialize state to JSON
var stateJson = JsonConvert.SerializeObject(state, Formatting.Indented, new JsonSerializerSettings { Converters = [new StringEnumConverter()], TypeNameHandling = TypeNameHandling.Auto});

// Write state to file
await File.WriteAllTextAsync("state.json", stateJson);
Console.WriteLine($"State written to: {Path.GetFullPath("state.json")}");

// Write unknown commands to file
var unknownCommandsText = string.Join(Environment.NewLine, Atem.UnknownCommands.Select(cmd => $"- {cmd}"));
await File.WriteAllTextAsync("unknown_commands.txt", unknownCommandsText);
Console.WriteLine($"Unknown commands written to: {Path.GetFullPath("unknown_commands.txt")}");

await atem.DisconnectAsync();
