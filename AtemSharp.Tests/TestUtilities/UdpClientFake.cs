using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Communication;

namespace AtemSharp.Tests.TestUtilities;

/// <summary>
/// Mock implementation of IUdpClient for testing purposes
/// </summary>
public class UdpClientFake : IUdpClient
{
    private readonly BufferBlock<UdpReceiveResult> _receivedData = new();
    public BufferBlock<byte[]> SentData { get; } = new();
    private bool _disposed;
    private IPEndPoint? _connectedEndPoint;
    private readonly Socket _fakeSocket;
    private TaskCompletionSource<UdpReceiveResult>? _receiveTaskCompletionSource;

    /// <summary>
    /// Gets the underlying Socket for configuration
    /// </summary>
    public Socket Client => _fakeSocket;

    /// <summary>
    /// Gets the endpoint this client is connected to
    /// </summary>
    public IPEndPoint? ConnectedEndPoint => _connectedEndPoint;

    /// <summary>
    /// Initializes a new instance of the UdpClientFake class
    /// </summary>
    public UdpClientFake()
    {
        // Create a fake socket that can be bound
        _fakeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    }

    /// <summary>
    /// Connects the UDP client to a remote endpoint
    /// </summary>
    /// <param name="remoteEndPoint">The remote endpoint to connect to</param>
    public void Connect(IPEndPoint remoteEndPoint)
    {
        ThrowIfDisposed();
        _connectedEndPoint = remoteEndPoint;
    }

    /// <inheritdoc />
    Task IUdpClient.SendAsync(byte[] data, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        SentData.Post((byte[])data.Clone());

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    async Task<UdpReceiveResult> IUdpClient.ReceiveAsync(CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        return await _receivedData.ReceiveAsync(cancellationToken);
    }

    /// <summary>
    /// Simulates receiving data from the remote endpoint
    /// This will complete any pending ReceiveAsync operation
    /// </summary>
    /// <param name="data">The data to simulate receiving</param>
    public void SimulateReceive(byte[] data)
    {
        ThrowIfDisposed();

        if (_connectedEndPoint is null)
        {
            throw new InvalidOperationException("Not connected");
        }

        _receivedData.Post(new UdpReceiveResult((byte[])data.Clone(), _connectedEndPoint));
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
        if (!_disposed)
        {
            // Cancel any pending receive operations
            _receiveTaskCompletionSource?.TrySetCanceled();
            _receiveTaskCompletionSource = null;

            _fakeSocket.Dispose();
            _disposed = true;
        }
    }

    /// <summary>
    /// Throws ObjectDisposedException if the fake client has been disposed
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(UdpClientFake));
        }
    }
}
