using AtemSharp.Enums;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Examples;

/// <summary>
/// Example tests demonstrating concurrent execution capabilities of MockAtemServer
/// </summary>
[TestFixture]
public class ConcurrentExecutionExampleTests
{
    [Test]
    public async Task MultipleServers_ShouldUseDifferentPorts()
    {
        // Arrange - Create multiple servers concurrently
        using var server1 = new MockAtemServer();
        using var server2 = new MockAtemServer();
        using var server3 = new MockAtemServer();
        
        server1.Start();
        server2.Start();
        server3.Start();

        // Assert - Each server should have a different port
        Assert.That(server1.Port, Is.Not.EqualTo(server2.Port));
        Assert.That(server2.Port, Is.Not.EqualTo(server3.Port));
        Assert.That(server1.Port, Is.Not.EqualTo(server3.Port));
        
        // All ports should be valid
        Assert.That(server1.Port, Is.GreaterThan(0));
        Assert.That(server2.Port, Is.GreaterThan(0));
        Assert.That(server3.Port, Is.GreaterThan(0));

        // Demonstrate connecting to different servers
        using var atem1 = new Atem();
        using var atem2 = new Atem();

        var connect1 = atem1.ConnectAsync("127.0.0.1", server1.Port);
        var connect2 = atem2.ConnectAsync("127.0.0.1", server2.Port);
        
        await Task.Delay(300); // Allow connections to establish

        // Both connections should succeed
        Assert.That(atem1.ConnectionState, Is.EqualTo(ConnectionState.Established));
        Assert.That(atem2.ConnectionState, Is.EqualTo(ConnectionState.Established));

        await atem1.DisconnectAsync();
        await atem2.DisconnectAsync();
        
        await connect1;
        await connect2;
    }

    [Test]
    public async Task ConvenienceMethod_ShouldCreateAndStartServer()
    {
        // Arrange & Act
        using var server = MockAtemServer.CreateAndStart();
        using var atem = new Atem();

        // Assert
        Assert.That(server.Port, Is.GreaterThan(0));
        
        // Should be able to connect immediately
        var connectTask = atem.ConnectAsync("127.0.0.1", server.Port);
        await Task.Delay(200);
        
        Assert.That(atem.ConnectionState, Is.EqualTo(ConnectionState.Established));
        
        await atem.DisconnectAsync();
        await connectTask;
    }

    [Test]
    public void LocalEndPoint_ShouldProvideCorrectEndpoint()
    {
        // Arrange
        using var server = new MockAtemServer();
        
        // Act
        var endpoint = server.LocalEndPoint;
        
        // Assert
        Assert.That(endpoint.Address, Is.EqualTo(System.Net.IPAddress.Loopback));
        Assert.That(endpoint.Port, Is.EqualTo(server.Port));
    }
}