using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Communication;

namespace AtemSharp.Tests.TestUtilities;

public class AtemProtocolFake : IAtemProtocol
{
    private TaskCompletionSource _connectTcs = new();
    private TaskCompletionSource _connectionRequestTcs = new();
    private TaskCompletionSource _disconnectTcs = new();
    private TaskCompletionSource _disconnectionRequestTcs = new();
    private IPEndPoint? _remoteEndpoint;
    private readonly BufferBlock<int> _ackedTrackingIds = new();
    private readonly BufferBlock<AtemPacket> _receivedPackets = new();
    private readonly BufferBlock<AtemPacket> _sentPackets = new();

    IReceivableSourceBlock<AtemPacket> IAtemProtocol.ReceivedPackets => _receivedPackets;

    IReceivableSourceBlock<int> IAtemProtocol.AckedTrackingIds => _ackedTrackingIds;

    public IPEndPoint? RemoteEndpoint => _remoteEndpoint;

    async Task IAtemProtocol.ConnectAsync(IPEndPoint endPoint)
    {
        // Connected -> Recreate disconnection infrastructure, so disconnection success/failure can be set _before_
        //              SUT tries to disconnect
        _disconnectTcs = new TaskCompletionSource();
        _disconnectionRequestTcs = new TaskCompletionSource();
        HasConnectionRequest = true;
        _connectionRequestTcs.TrySetResult();
        await _connectTcs.Task;
        HasConnectionRequest = false;
        _remoteEndpoint = endPoint;
    }

    async Task IAtemProtocol.DisconnectAsync()
    {
        // Disconnected -> Recreate connection infrastructure, so connection success/failure can be set _before_
        //                 SUT tries to connect
        _connectTcs = new TaskCompletionSource();
        _connectionRequestTcs = new TaskCompletionSource();
        HasDisconnectionRequest = true;
        _disconnectionRequestTcs.TrySetResult();
        await _disconnectTcs.Task;
        HasDisconnectionRequest = false;
        _remoteEndpoint = null;
    }

    async Task IAtemProtocol.SendPacketsAsync(AtemPacket[] packets)
    {
        foreach (var packet in packets)
        {
            await _sentPackets.SendAsync(packet);
        }
    }

    public async ValueTask DisposeAsync() => await Task.CompletedTask;

    public bool HasConnectionRequest { get; private set; }
    public bool HasDisconnectionRequest { get; private set; }

    public Task WaitForConnectionRequestAsync() => _connectionRequestTcs.Task;
    public Task WaitForDisconnectionRequestAsync() => _disconnectionRequestTcs.Task;

    public bool IsConnected() => RemoteEndpoint != null;

    public void SucceedConnection() => _connectTcs?.TrySetResult();
    public void FailConnection(Exception ex) => _connectTcs?.TrySetException(ex);

    public void SucceedDisconnection() => _disconnectTcs?.TrySetResult();
    public void FailDisconnection(Exception ex) => _disconnectTcs?.TrySetException(ex);

    public async Task ReceivePacketAsync(AtemPacket packet) => await _receivedPackets.SendAsync(packet);
    public async Task AckPacket(AtemPacket packet) => await _ackedTrackingIds.SendAsync(packet.TrackingId);

    public async Task<AtemPacket> GetSentPacket() =>  await _sentPackets.ReceiveAsync();
}
