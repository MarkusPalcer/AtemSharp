using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Lib;

namespace AtemSharp.Communication;

public interface IAtemProtocol : IAsyncDisposable
{
    IReceivableSourceBlock<AtemPacket> ReceivedPackets { get; }
    IReceivableSourceBlock<int> AckedTrackingIds { get; }
    Task ConnectAsync(IPEndPoint endPoint);
    Task DisconnectAsync();
    Task SendPacketsAsync(AtemPacket[] packets);
}
