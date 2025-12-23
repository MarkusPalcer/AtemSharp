using System.Net;
using System.Threading.Tasks.Dataflow;

namespace AtemSharp.Communication;

internal interface IAtemProtocol : IAsyncDisposable
{
    IReceivableSourceBlock<AtemPacket> ReceivedPackets { get; }
    IReceivableSourceBlock<int> AckedTrackingIds { get; }
    Task ConnectAsync(IPEndPoint endPoint);
    Task DisconnectAsync();
    Task SendPacketsAsync(AtemPacket[] packets);
}
