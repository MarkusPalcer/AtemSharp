// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using AtemSharp;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection().AddAtemSharp().BuildServiceProvider();
var lib = serviceProvider.GetRequiredService<IAtemSharpLibrary>();


Console.WriteLine("=== AtemSharp Demo ===\n");

var emergencyCts = new CancellationTokenSource();
if (!Debugger.IsAttached)
{
    emergencyCts.CancelAfter(TimeSpan.FromSeconds(20));
}

Console.CancelKeyPress += (_, _) => emergencyCts.Cancel();

var atem = lib.CreateAtemSwitcher("192.168.178.69");

Console.WriteLine("Connecting...");
await atem.ConnectAsync(cancellationToken: emergencyCts.Token);

Console.WriteLine($"Executing Macro {atem.Macros[0].Name} ...");
await atem.Macros[0].Run();

Console.WriteLine("Disconnecting...");
await atem.DisconnectAsync();

Console.WriteLine("Waiting 2s before reconnecting...");
await Task.Delay(TimeSpan.FromSeconds(2));

Console.WriteLine("Reconnecting...");
await atem.ConnectAsync( cancellationToken: emergencyCts.Token);

Console.WriteLine($"Executing Macro {atem.Macros[1].Name} ...");
await atem.Macros[1].Run();

Console.WriteLine("Disconnecting...");
await atem.DisconnectAsync();

