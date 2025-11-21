using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Lib;

namespace AtemSharp.Communication;

public interface IAtemProtocol
{
    IReceivableSourceBlock<AtemPacket> ReceivedPackets { get; }
    IReceivableSourceBlock<int> AckedTrackingIds { get; }
    Task Connect(IPEndPoint endPoint);
    Task Disconnect();
    Task SendPackets(AtemPacket[] packets);
}
