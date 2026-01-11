using AtemSharp.Commands;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Batch;

internal class NonMergeableCommand(int value) : SerializedCommand
{
    public int Value { get; } = value;

    public override byte[] Serialize(ProtocolVersion version)
    {
        return [];
    }

    internal override bool TryMergeTo(SerializedCommand other)
    {
        return false;
    }
}
