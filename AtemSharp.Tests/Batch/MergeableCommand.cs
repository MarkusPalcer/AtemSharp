using AtemSharp.Commands;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Batch;

internal class MergeableCommand(int value) : SerializedCommand
{
    public List<int> Values { get; } = [value];

    public override byte[] Serialize(ProtocolVersion version)
    {
        return [];
    }

    internal override bool TryMergeTo(SerializedCommand other)
    {
        other.As<MergeableCommand>().Values.Add(value);
        return true;
    }
}
