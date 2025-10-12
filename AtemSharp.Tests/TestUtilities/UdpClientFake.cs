using System.Net;
using System.Net.Sockets;
using AtemSharp.Lib;

namespace AtemSharp.Tests.TestUtilities;

/// <summary>
/// Mock implementation of IUdpClient for testing purposes
/// </summary>
public class UdpClientFake : IUdpClient
{
    private readonly Queue<byte[]> _receivedData = new();
    private readonly List<byte[]> _sentData = new();
    private bool _disposed;
    private IPEndPoint? _connectedEndPoint;
    private readonly Socket _fakeSocket;
    private TaskCompletionSource<UdpReceiveResult>? _receiveTaskCompletionSource;

    /// <summary>
    /// Gets the underlying Socket for configuration
    /// </summary>
    public Socket Client => _fakeSocket;

    /// <summary>
    /// Gets the data that was sent through this fake client
    /// </summary>
    public IReadOnlyList<byte[]> SentData => _sentData.AsReadOnly();

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

    /// <summary>
    /// Asynchronously sends data to the connected remote endpoint
    /// </summary>
    /// <param name="data">The data to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    public Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        _sentData.Add((byte[])data.Clone());
        return Task.CompletedTask;
    }

    /// <summary>
    /// Asynchronously receives data from any remote endpoint
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task containing the received data and remote endpoint</returns>
    public async Task<UdpReceiveResult> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        
        // If we have immediate data available, return it
        if (_receivedData.Count > 0)
        {
            var data = _receivedData.Dequeue();
            return new UdpReceiveResult(data, _connectedEndPoint ?? new IPEndPoint(IPAddress.Loopback, 9910));
        }

        // Otherwise, wait for data to be provided via SimulateReceive
        _receiveTaskCompletionSource = new TaskCompletionSource<UdpReceiveResult>();
        
        // Wait for either data to arrive or cancellation
        using var registration = cancellationToken.Register(() => 
            _receiveTaskCompletionSource?.TrySetCanceled(cancellationToken));
            
        return await _receiveTaskCompletionSource.Task;
    }

    /// <summary>
    /// Simulates receiving data from the remote endpoint
    /// This will complete any pending ReceiveAsync operation
    /// </summary>
    /// <param name="data">The data to simulate receiving</param>
    public void SimulateReceive(byte[] data)
    {
        ThrowIfDisposed();
        
        if (_receiveTaskCompletionSource != null)
        {
            // Complete the pending receive operation
            var result = new UdpReceiveResult((byte[])data.Clone(), _connectedEndPoint ?? new IPEndPoint(IPAddress.Loopback, 9910));
            _receiveTaskCompletionSource.TrySetResult(result);
            _receiveTaskCompletionSource = null;
        }
        else
        {
            // Queue the data for later retrieval
            _receivedData.Enqueue((byte[])data.Clone());
        }
    }

    /// <summary>
    /// Simulates a receive error
    /// </summary>
    /// <param name="exception">The exception to throw from ReceiveAsync</param>
    public void SimulateReceiveError(Exception exception)
    {
        ThrowIfDisposed();
        
        if (_receiveTaskCompletionSource != null)
        {
            _receiveTaskCompletionSource.TrySetException(exception);
            _receiveTaskCompletionSource = null;
        }
    }

    /// <summary>
    /// Clears all sent data history
    /// </summary>
    public void ClearSentData()
    {
        _sentData.Clear();
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