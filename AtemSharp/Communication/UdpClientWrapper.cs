using System.Net;
using System.Net.Sockets;

namespace AtemSharp.Lib;

/// <summary>
/// Concrete implementation of IUdpClient that wraps the .NET Framework UdpClient
/// </summary>
public sealed class UdpClientWrapper : IUdpClient
{
    private readonly UdpClient _udpClient;
    private bool _disposed;

    /// <summary>
    /// Gets the underlying Socket for configuration
    /// </summary>
    public Socket Client => _udpClient.Client;

    /// <summary>
    /// Initializes a new instance of the UdpClientWrapper class
    /// </summary>
    public UdpClientWrapper()
    {
        _udpClient = new UdpClient();
    }

    /// <summary>
    /// Initializes a new instance of the UdpClientWrapper class with the specified UdpClient
    /// This constructor is useful for testing scenarios where you want to inject a specific UdpClient
    /// </summary>
    /// <param name="udpClient">The UdpClient to wrap</param>
    internal UdpClientWrapper(UdpClient udpClient)
    {
        _udpClient = udpClient ?? throw new ArgumentNullException(nameof(udpClient));
    }

    /// <summary>
    /// Connects the UDP client to a remote endpoint
    /// </summary>
    /// <param name="remoteEndPoint">The remote endpoint to connect to</param>
    public void Connect(IPEndPoint remoteEndPoint)
    {
        ThrowIfDisposed();
        _udpClient.Connect(remoteEndPoint);
    }

    /// <summary>
    /// Asynchronously sends data to the connected remote endpoint
    /// </summary>
    /// <param name="data">The data to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        await _udpClient.SendAsync(data, cancellationToken);
    }

    /// <summary>
    /// Asynchronously receives data from any remote endpoint
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task containing the received data and remote endpoint</returns>
    public async Task<UdpReceiveResult> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return await _udpClient.ReceiveAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes the wrapped UdpClient
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _udpClient.Dispose();
            _disposed = true;
        }
    }

    /// <summary>
    /// Throws ObjectDisposedException if the wrapper has been disposed
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(UdpClientWrapper));
        }
    }
}