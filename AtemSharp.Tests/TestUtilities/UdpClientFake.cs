using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
using AtemSharp.FrameworkAbstraction;

namespace AtemSharp.Tests.TestUtilities;

/// <summary>
/// Mock implementation of IUdpClient for testing purposes
/// </summary>
public class UdpClientFake : IUdpClient
{
    private readonly BufferBlock<UdpReceiveResult> _receivedData = new();
    public BufferBlock<byte[]> SentData { get; } = new();
    private IPEndPoint? _connectedEndPoint;

    /// <summary>
    /// Gets the underlying Socket for configuration
    /// </summary>
    public Socket Client { get; }

    /// <summary>
    /// Initializes a new instance of the UdpClientFake class
    /// </summary>
    public UdpClientFake()
    {
        // Create a fake socket that can be bound
        Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    }

    public void Bind(IPEndPoint remoteEndpoint)
    {
        // Nothing to do
    }

    /// <summary>
    /// Connects the UDP client to a remote endpoint
    /// </summary>
    /// <param name="remoteEndPoint">The remote endpoint to connect to</param>
    public void Connect(IPEndPoint remoteEndPoint)
    {
        _connectedEndPoint = remoteEndPoint;
    }

    /// <inheritdoc />
    async ValueTask<int> IUdpClient.SendAsync(byte[] data, CancellationToken cancellationToken)
    {
        await SentData.SendAsync((byte[])data.Clone(), cancellationToken);
        return data.Length;
    }

    /// <inheritdoc />
    async ValueTask<UdpReceiveResult> IUdpClient.ReceiveAsync(CancellationToken cancellationToken)
    {
        return await _receivedData.ReceiveAsync(cancellationToken);
    }

    /// <summary>
    /// Simulates receiving data from the remote endpoint
    /// This will complete any pending ReceiveAsync operation
    /// </summary>
    /// <param name="data">The data to simulate receiving</param>
    public async Task SimulateReceive(byte[] data)
    {
        if (_connectedEndPoint is null)
        {
            throw new InvalidOperationException("Not connected");
        }

        await _receivedData.SendAsync(new UdpReceiveResult((byte[])data.Clone(), _connectedEndPoint));
    }

    /// <summary>
    /// Clears all sent data history
    /// </summary>
    public void ClearSentData()
    {
        SentData.TryReceiveAll(out _);
    }

    /// <summary>
    /// Disposes the fake UDP client
    /// </summary>
    public void Dispose()
    {
    }
}
