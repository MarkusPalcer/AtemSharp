using AtemSharp.State.Video;

namespace AtemSharp.Commands.Video;

/// <summary>
/// Used to set the source for an auxiliary output
/// </summary>
[Command("CAuS")]
[BufferSize(4)]
public partial class AuxSourceCommand(AuxiliaryOutput auxiliaryOutput) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _auxId = auxiliaryOutput.Id;

    [SerializedField(2, 0)] private ushort _source = auxiliaryOutput.Source;
}
