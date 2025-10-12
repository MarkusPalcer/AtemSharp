using System.Net;
using System.Net.Sockets;
using AtemSharp.Commands.MixEffects;
using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Lib;

[TestFixture]
[NonParallelizable]
public class UdpTransportTests
{
    private UdpTransport? _transport;
    private UdpClientFake? _udpClientFake;

    [SetUp]
    public void SetUp()
    {
        _udpClientFake = new UdpClientFake();
        _transport = new UdpTransport(_udpClientFake);
    }

    [TearDown]
    public void TearDown()
    {
        _transport?.Dispose();
        _udpClientFake?.Dispose();
    }

    [Test]
    public void Constructor_ShouldInitializeWithClosedState()
    {
        // Assert
        Assert.That(_transport!.ConnectionState, Is.EqualTo(ConnectionState.Closed));
        Assert.That(_transport.RemoteEndPoint, Is.Null);
    }

    [Test]
    public void ConnectAsync_WithInvalidIpAddress_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidAddress = "invalid-ip-address";

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _transport!.ConnectAsync(invalidAddress));
        Assert.That(ex!.ParamName, Is.EqualTo("address"));
        Assert.That(ex.Message, Does.Contain("Invalid IP address"));
    }

    [Test]
    public async Task ConnectAsync_WithValidEndpoint_ShouldEstablishConnection()
    {
        // Arrange
        var receivedPackets = new List<AtemPacket>();
        var connectionStates = new List<ConnectionState>();
        var tcs = new TaskCompletionSource();
        
        _transport!.PacketReceived += (sender, args) => receivedPackets.Add(args.Packet);
        _transport.ConnectionStateChanged += (sender, args) => 
        {
            connectionStates.Add(args.State);
            if (args.State == ConnectionState.Established)
            {
                tcs.TrySetResult();
            }
        };

        // Act
        var connectTask = _transport.ConnectAsync("127.0.0.1", 9910);
        await tcs.Task.WithTimeout();
        await connectTask.WithTimeout();

        // Assert
        Assert.That(_transport.ConnectionState, Is.EqualTo(ConnectionState.Established));
        Assert.That(_transport.RemoteEndPoint, Is.Not.Null);
        Assert.That(_transport.RemoteEndPoint!.Address, Is.EqualTo(IPAddress.Parse("127.0.0.1")));
        Assert.That(_transport.RemoteEndPoint.Port, Is.EqualTo(9910));
        Assert.That(_udpClientFake!.ConnectedEndPoint, Is.Not.Null);
        Assert.That(connectionStates, Contains.Item(ConnectionState.SynSent));
        Assert.That(connectionStates, Contains.Item(ConnectionState.Established));
    }

    [Test]
    public async Task ConnectAsync_WithCustomPort_ShouldUseSpecifiedPort()
    {
        // Arrange
        var customPort = 8888;

        // Act
        await _transport!.ConnectAsync("127.0.0.1", customPort);

        // Assert
        Assert.That(_transport.ConnectionState, Is.EqualTo(ConnectionState.Established));
        Assert.That(_transport.RemoteEndPoint!.Port, Is.EqualTo(customPort));
        Assert.That(_udpClientFake!.ConnectedEndPoint!.Port, Is.EqualTo(customPort));
    }

    [Test]
    public void ConnectAsync_WhenAlreadyConnected_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _transport!.ConnectAsync("127.0.0.1").Wait();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _transport.ConnectAsync("127.0.0.1"));
        Assert.That(ex!.Message, Does.Contain("Cannot connect when state is"));
    }

    [Test]
    public async Task SendPacketAsync_WhenConnected_ShouldSendData()
    {
        // Arrange
        await _transport!.ConnectAsync("127.0.0.1");
        var packet = AtemPacket.CreateHello();

        // Act
        await _transport.SendPacketAsync(packet);

        // Assert
        Assert.That(_udpClientFake!.SentData, Has.Count.EqualTo(1));
        var sentBytes = _udpClientFake.SentData[0];
        Assert.That(sentBytes, Is.EqualTo(packet.ToBytes()));
    }

    [Test]
    public void SendPacketAsync_WhenNotConnected_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var packet = AtemPacket.CreateHello();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _transport!.SendPacketAsync(packet));
        Assert.That(ex!.Message, Does.Contain("Cannot send packet when not connected"));
    }

    [Test]
    public async Task ReceiveLoop_ShouldProcessIncomingPackets()
    {
        // Arrange
        var receivedPackets = new List<AtemPacket>();
        var tcs = new TaskCompletionSource();
        _transport!.PacketReceived += (sender, args) => 
        {
            receivedPackets.Add(args.Packet);
            tcs.SetResult();
        };
        
        await _transport.ConnectAsync("127.0.0.1");
        
        // Create a test packet
        var testPacket = new AtemPacket
        {
            Flags = PacketFlag.AckRequest,
            Length = 12,
            SessionId = 1234,
            AckPacketId = 0,
            PacketId = 1,
            Payload = Array.Empty<byte>()
        };

        // Act
        _udpClientFake!.SimulateReceive(testPacket.ToBytes());
        await tcs.Task.WithTimeout();

        // Assert
        Assert.That(receivedPackets, Has.Count.EqualTo(1));
        var receivedPacket = receivedPackets[0];
        Assert.That(receivedPacket.Flags, Is.EqualTo(testPacket.Flags));
        Assert.That(receivedPacket.SessionId, Is.EqualTo(testPacket.SessionId));
        Assert.That(receivedPacket.PacketId, Is.EqualTo(testPacket.PacketId));
    }

    [Test]
    public async Task ReceiveLoop_WithInvalidPacket_ShouldTriggerErrorEvent()
    {
        // Arrange
        var errors = new List<Exception>();
        var tcs = new TaskCompletionSource();
        _transport!.ErrorOccurred += (_, ex) =>
        {
	        errors.Add(ex);
	        tcs.SetResult();
        };
        
        await _transport.ConnectAsync("127.0.0.1");

        // Act - send invalid packet data
        _udpClientFake!.SimulateReceive([0x01, 0x02]); // Too short to be valid
        await tcs.Task.WithTimeout();

        // Assert
        Assert.That(errors, Has.Count.EqualTo(1));
        Assert.That(errors[0], Is.TypeOf<InvalidDataException>());
    }

    [Test]
    public async Task DisconnectAsync_WhenConnected_ShouldCloseConnection()
    {
        // Arrange
        var connectionStates = new List<ConnectionState>();
        var tcs = new TaskCompletionSource();
        _transport!.ConnectionStateChanged += (sender, args) => 
        {
            connectionStates.Add(args.State);
            if (args.State == ConnectionState.Closed)
            {
                tcs.TrySetResult();
            }
        };
        
        await _transport.ConnectAsync("127.0.0.1");
        connectionStates.Clear(); // Clear connection states

        // Act
        var disconnectTask = _transport.DisconnectAsync();
        await tcs.Task.WithTimeout();
        await disconnectTask.WithTimeout();

        // Assert
        Assert.That(_transport.ConnectionState, Is.EqualTo(ConnectionState.Closed));
        Assert.That(_transport.RemoteEndPoint, Is.Null);
        Assert.That(connectionStates, Contains.Item(ConnectionState.Disconnected));
        Assert.That(connectionStates, Contains.Item(ConnectionState.Closed));
    }

    [Test]
    public async Task DisconnectAsync_WhenNotConnected_ShouldCompleteWithoutError()
    {
        // Act & Assert
        Assert.DoesNotThrowAsync(() => _transport!.DisconnectAsync());
        Assert.That(_transport!.ConnectionState, Is.EqualTo(ConnectionState.Closed));
    }

    [Test]
    public async Task SendHelloPacketAsync_ShouldSendCorrectPacket()
    {
        // Arrange
        await _transport!.ConnectAsync("127.0.0.1");

        // Act
        await _transport.SendHelloPacketAsync(CancellationToken.None);

        // Assert
        Assert.That(_udpClientFake!.SentData, Has.Count.EqualTo(1));
        
        var sentBytes = _udpClientFake.SentData[0];
        Assert.That(AtemPacket.TryParse(sentBytes.AsSpan(), out var sentPacket), Is.True);
        Assert.That(sentPacket!.Flags.HasFlag(PacketFlag.NewSessionId), Is.True);
        Assert.That(sentPacket.Flags.HasFlag(PacketFlag.AckRequest), Is.True);
    }

    [Test]
    public void Dispose_ShouldCleanupResources()
    {
        // Act
        _transport!.Dispose();

        // Assert - Should not throw when accessing after dispose
        Assert.DoesNotThrow(() => 
        {
            var state = _transport.ConnectionState;
            var endpoint = _transport.RemoteEndPoint;
        });
    }

    [Test]
    public void Dispose_MultipleCallsShouldNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() =>
        {
            _transport!.Dispose();
            _transport.Dispose(); // Second call should not throw
        });
    }
}