using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace AtemSharp.FrameworkAbstraction;

[ExcludeFromCodeCoverage(Justification="Testing this means testing the .NET framework")]
internal sealed class UdpClientWrapper : IUdpClient
{
    private readonly UdpClient _udpClient = new();

    public void Bind(IPEndPoint remoteEndpoint)
    {
        _udpClient.Client.Bind(remoteEndpoint);
    }

    public void Connect(IPEndPoint remoteEndPoint)
    {
        _udpClient.Connect(remoteEndPoint);
    }

    public ValueTask<int> SendAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        return _udpClient.SendAsync(data, cancellationToken);
    }

    public ValueTask<UdpReceiveResult> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        return _udpClient.ReceiveAsync(cancellationToken);
    }

    public void Dispose()
    {
        _udpClient.Dispose();
    }
}
