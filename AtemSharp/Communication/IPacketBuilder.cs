using AtemSharp.Commands;
using AtemSharp.State.Info;

namespace AtemSharp.Communication;

public interface IPacketBuilder
{
    ProtocolVersion ProtocolVersion { get; set; }
    void AddCommand(SerializedCommand cmd);
    IReadOnlyList<byte[]> GetPackets();
}
