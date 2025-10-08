using AtemSharp;
using AtemSharp.Commands.MixEffects;

namespace AtemSharp.Examples;

/// <summary>
/// Advanced example demonstrating ATEM control with state management
/// </summary>
public class AdvancedExample
{
    public static async Task Main(string[] args)
    {
        // Create ATEM connection with options
        var atem = new Atem(new AtemOptions
        {
            DebugBuffers = true,
            MaxPacketSize = 1416
        });

        // Subscribe to all events
        atem.Connected += OnConnected;
        atem.Disconnected += OnDisconnected;
        atem.StateChanged += OnStateChanged;
        atem.ReceivedCommands += OnReceivedCommands;
        atem.Error += (sender, message) => Console.WriteLine($"❌ Error: {message}");
        atem.Info += (sender, message) => Console.WriteLine($"ℹ️ Info: {message}");

        Console.WriteLine("AtemSharp Advanced Example");
        Console.WriteLine("=========================");
        
        // Get ATEM IP address from user
        Console.Write("Enter ATEM IP address (default: 192.168.1.240): ");
        var input = Console.ReadLine();
        var address = string.IsNullOrWhiteSpace(input) ? "192.168.1.240" : input;

        try
        {
            Console.WriteLine($"🔌 Connecting to ATEM at {address}...");
            await atem.ConnectAsync(address);

            Console.WriteLine("✅ Connected! Waiting for initialization...");
            
            // Interactive command loop
            await RunInteractiveLoop(atem);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Connection failed: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("🔌 Disconnecting...");
            await atem.DisconnectAsync();
            await atem.DestroyAsync();
            Console.WriteLine("👋 Goodbye!");
        }
    }

    private static async Task RunInteractiveLoop(Atem atem)
    {
        Console.WriteLine("\n📋 Available Commands:");
        Console.WriteLine("  'c' - Cut transition on ME1");
        Console.WriteLine("  'a' - Auto transition on ME1");
        Console.WriteLine("  'p <source>' - Set program input (e.g., 'p 1')");
        Console.WriteLine("  'v <source>' - Set preview input (e.g., 'v 2')");
        Console.WriteLine("  's' - Show current state");
        Console.WriteLine("  'q' - Quit");
        Console.WriteLine();

        while (true)
        {
            Console.Write("ATEM> ");
            var input = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(input))
                continue;

            try
            {
                if (input.ToLower() == "q")
                {
                    break;
                }
                else if (input.ToLower() == "c")
                {
                    Console.WriteLine("🎬 Performing cut transition...");
                    await atem.SendCommandsAsync(new[] { new CutCommand(0) });
                    Console.WriteLine("✅ Cut command sent!");
                }
                else if (input.ToLower() == "a")
                {
                    Console.WriteLine("🎬 Performing auto transition...");
                    await atem.SendCommandsAsync(new[] { new AutoTransitionCommand(0) });
                    Console.WriteLine("✅ Auto transition command sent!");
                }
                else if (input.ToLower().StartsWith("p "))
                {
                    if (int.TryParse(input.Substring(2), out int source))
                    {
                        Console.WriteLine($"📺 Setting program input to {source}...");
                        await atem.SendCommandsAsync(new[] { new ProgramInputCommand(0, source) });
                        Console.WriteLine("✅ Program input command sent!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Invalid source number");
                    }
                }
                else if (input.ToLower().StartsWith("v "))
                {
                    if (int.TryParse(input.Substring(2), out int source))
                    {
                        Console.WriteLine($"👁️ Setting preview input to {source}...");
                        await atem.SendCommandsAsync(new[] { new PreviewInputCommand(0, source) });
                        Console.WriteLine("✅ Preview input command sent!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Invalid source number");
                    }
                }
                else if (input.ToLower() == "s")
                {
                    ShowCurrentState(atem);
                }
                else
                {
                    Console.WriteLine("❌ Unknown command. Type 'q' to quit.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Command failed: {ex.Message}");
            }
        }
    }

    private static void OnConnected(object? sender, EventArgs e)
    {
        Console.WriteLine("🎉 ATEM connection established!");
    }

    private static void OnDisconnected(object? sender, EventArgs e)
    {
        Console.WriteLine("💔 ATEM disconnected");
    }

    private static void OnStateChanged(object? sender, StateChangedEventArgs e)
    {
        Console.WriteLine($"🔄 State changed: {string.Join(", ", e.ChangedPaths)}");
        
        // Show specific state changes
        foreach (var path in e.ChangedPaths)
        {
            if (path.Contains("programInput"))
            {
                var me = GetMixEffectFromPath(path);
                var state = e.State.Video.MixEffects.GetValueOrDefault(me);
                if (state?.ProgramInput.HasValue == true)
                {
                    Console.WriteLine($"  📺 ME{me + 1} Program: {state.ProgramInput}");
                }
            }
            else if (path.Contains("previewInput"))
            {
                var me = GetMixEffectFromPath(path);
                var state = e.State.Video.MixEffects.GetValueOrDefault(me);
                if (state?.PreviewInput.HasValue == true)
                {
                    Console.WriteLine($"  👁️ ME{me + 1} Preview: {state.PreviewInput}");
                }
            }
            else if (path.Contains("apiVersion"))
            {
                Console.WriteLine($"  🔧 API Version: {e.State.Info.ApiVersion}");
            }
        }
    }

    private static void OnReceivedCommands(object? sender, CommandsReceivedAtemEventArgs e)
    {
        Console.WriteLine($"📨 Received {e.Commands.Length} command(s) from ATEM");
    }

    private static void ShowCurrentState(Atem atem)
    {
        var state = atem.State;
        if (state == null)
        {
            Console.WriteLine("❌ No state available yet");
            return;
        }

        Console.WriteLine("\n📊 Current ATEM State:");
        Console.WriteLine($"  Connection Status: {atem.Status}");
        
        if (state.Info.ApiVersion.HasValue)
        {
            Console.WriteLine($"  API Version: {state.Info.ApiVersion}");
        }

        if (state.Video.MixEffects.Count > 0)
        {
            Console.WriteLine("  Mix Effects:");
            foreach (var kvp in state.Video.MixEffects)
            {
                var me = kvp.Key;
                var meState = kvp.Value;
                Console.WriteLine($"    ME{me + 1}:");
                
                if (meState.ProgramInput.HasValue)
                    Console.WriteLine($"      Program: {meState.ProgramInput}");
                
                if (meState.PreviewInput.HasValue)
                    Console.WriteLine($"      Preview: {meState.PreviewInput}");
            }
        }
        else
        {
            Console.WriteLine("  No Mix Effects data available yet");
        }

        Console.WriteLine();
    }

    private static int GetMixEffectFromPath(string path)
    {
        // Extract ME number from path like "video.mixEffects.0.programInput"
        var parts = path.Split('.');
        if (parts.Length >= 3 && parts[1] == "mixEffects" && int.TryParse(parts[2], out int me))
        {
            return me;
        }
        return 0;
    }
}