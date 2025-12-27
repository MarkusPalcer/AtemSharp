using System.Net;
using System.Net.Sockets;

namespace AtemSharp.FrameworkAbstraction;

/// <summary>
/// Interface for UDP client operations, allowing for testability and mocking
/// </summary>
public interface IUdpClient : IDisposable
{
    void Bind(IPEndPoint remoteEndpoint);

    /// <summary>
    /// Connects the UDP client to a remote endpoint
    /// </summary>
    /// <param name="remoteEndPoint">The remote endpoint to connect to</param>
    void Connect(IPEndPoint remoteEndPoint);

    /// <summary>
    /// Asynchronously sends data to the connected remote endpoint
    /// </summary>
    /// <param name="data">The data to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    ValueTask<int> SendAsync(byte[] data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously receives data from any remote endpoint
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task containing the received data and remote endpoint</returns>
    ValueTask<UdpReceiveResult> ReceiveAsync(CancellationToken cancellationToken = default);
}
