using AtemSharp;
using AtemSharp.Commands.MixEffects;

namespace AtemSharp.Examples;

/// <summary>
/// Simple console example demonstrating basic ATEM connection
/// </summary>
public class SimpleExample
{
    public static async Task Main(string[] args)
    {
        // Create ATEM connection
        var atem = new Atem(new AtemOptions
        {
            DebugBuffers = true
        });

        // Subscribe to events
        atem.Connected += (sender, e) => Console.WriteLine("Connected to ATEM!");
        atem.Disconnected += (sender, e) => Console.WriteLine("Disconnected from ATEM");
        atem.StateChanged += (sender, e) => Console.WriteLine($"State changed: {string.Join(", ", e.ChangedPaths)}");
        atem.Error += (sender, message) => Console.WriteLine($"Error: {message}");
        atem.Info += (sender, message) => Console.WriteLine($"Info: {message}");

        Console.WriteLine("AtemSharp Example");
        Console.WriteLine("================");
        
        // Get ATEM IP address from user
        Console.Write("Enter ATEM IP address (default: 192.168.1.240): ");
        var input = Console.ReadLine();
        var address = string.IsNullOrWhiteSpace(input) ? "192.168.1.240" : input;

        try
        {
            // Connect to ATEM
            Console.WriteLine($"Connecting to ATEM at {address}...");
            await atem.ConnectAsync(address);

            Console.WriteLine("Connected! Current status: " + atem.Status);
            
            // Wait for user input to perform actions
            Console.WriteLine("\nPress 'c' to perform a cut, 'q' to quit, or any other key to show state:");
            
            while (true)
            {
                var key = Console.ReadKey(true);
                
                if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                {
                    break;
                }
                else if (key.KeyChar == 'c' || key.KeyChar == 'C')
                {
                    try
                    {
                        Console.WriteLine("Performing cut on ME1...");
                        var cutCommand = new CutCommand(0); // 0 = ME1
                        await atem.SendCommandsAsync(new[] { cutCommand });
                        Console.WriteLine("Cut command sent successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending cut command: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Current ATEM status: {atem.Status}");
                    if (atem.State != null)
                    {
                        Console.WriteLine("ATEM state is available");
                    }
                    else
                    {
                        Console.WriteLine("ATEM state not yet received");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
        finally
        {
            // Cleanup
            Console.WriteLine("Disconnecting...");
            await atem.DisconnectAsync();
            await atem.DestroyAsync();
            Console.WriteLine("Goodbye!");
        }
    }
}