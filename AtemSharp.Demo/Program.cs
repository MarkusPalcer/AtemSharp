// See https://aka.ms/new-console-template for more information

using AtemSharp;
using Newtonsoft.Json;

Console.WriteLine("=== AtemSharp Demo ===\n");

var atem = new AtemSharp.Atem();
await atem.Connect("192.168.178.69");
Console.WriteLine("Connected, waiting 10s for data to come in ...");
Thread.Sleep(10000);
await atem.Disconnect();
var state = atem.State;
Console.WriteLine();
Console.WriteLine("Current state:");
Console.WriteLine(JsonConvert.SerializeObject(state, Formatting.Indented));
Console.WriteLine();
Console.WriteLine("Unknown commands received:");
foreach (var cmd in Atem.UnknownCommands)
{
	Console.WriteLine($"  - {cmd}");
}
