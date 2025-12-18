using System.Diagnostics.CodeAnalysis;
using AtemSharp.State;

namespace AtemSharp.Commands;

// TODO #49: Capture test cases
[Command("TlSr")]
public partial class TallyBySourceCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _sourceCount;
    [CustomDeserialization] private Tally[] _tallyBySource = [];

    [ExcludeFromCodeCoverage]
    public class Tally
    {
        public bool IsInProgram { get; internal set; }
        public bool IsInPreview { get; internal set; }
        public ushort Source { get; internal set; }
    }

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        _tallyBySource = new Tally[_sourceCount];
        for (var i = 0; i < _sourceCount; i++)
        {
            var source = rawCommand.ReadUInt16BigEndian(2 + i * 3);
            var value = rawCommand.ReadUInt8(4 + i * 3);
            _tallyBySource[i] = new Tally
            {
                Source = source,
                IsInProgram = (value & 0x01) > 0,
                IsInPreview = (value & 0x02) > 0,
            };
        }
    }

    public void ApplyToState(AtemState state)
    {
        foreach (var tally in TallyBySource)
        {
            var inputChannel = state.Video.Inputs.GetOrCreate(tally.Source);
            inputChannel.IsInProgram = tally.IsInProgram;
            inputChannel.IsInPreview = tally.IsInPreview;
        }
    }
}
