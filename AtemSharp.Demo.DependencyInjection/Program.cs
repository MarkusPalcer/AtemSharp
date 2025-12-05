// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using AtemSharp;
using AtemSharp.Commands.Macro;
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
atem.ConnectionStateChanged += (_, args) => Console.WriteLine($"Connection state changed from {args.OldState} to {args.NewState}");

Console.WriteLine("Connecting...");
await atem.ConnectAsync(cancellationToken: emergencyCts.Token);

Console.WriteLine("Waiting 2s for data to come in...");
await Task.Delay(TimeSpan.FromSeconds(2), emergencyCts.Token);

Console.WriteLine($"Executing Macro {atem.State.Macros.Macros[0].Name} ...");
await atem.SendCommandAsync(new MacroActionCommand(atem.State.Macros.Macros[0], MacroAction.Run));

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
