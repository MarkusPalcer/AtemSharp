using AtemSharp.State.Info;

namespace AtemSharp.Commands;

// Base class for commands that don't send any payload
public abstract class EmptyCommand : SerializedCommand
{
    public override byte[] Serialize(ProtocolVersion version) => [];
}
