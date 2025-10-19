using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands;

[Command("TlSr")]
public class TallyBySourceCommand : IDeserializedCommand
{
    internal TallyBySourceCommand(Tally[] tallyBySource)
    {
        TallyBySource = tallyBySource;
    }

    public class Tally
    {
        public bool IsInProgram { get; internal set; }
        public bool IsInPreview { get; internal set; }
        public ushort Source { get; internal set; }
    }

    public Tally[] TallyBySource { get; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        var sourceCount = rawCommand.ReadUInt16BigEndian(0);
        var tallyBySource = new Tally[sourceCount];

        for (var i = 0; i < sourceCount; i++)
        {
            var source = rawCommand.ReadUInt16BigEndian(2 + i * 3);
            var value = rawCommand.ReadUInt8(4 + i * 3);
            tallyBySource[i] = new Tally
            {
                Source = source,
                IsInProgram = (value & 0x01) > 0,
                IsInPreview = (value & 0x02) > 0,
            };
        }

        return new TallyBySourceCommand(tallyBySource);
    }

    public void ApplyToState(AtemState state)
    {
        foreach (var tally in TallyBySource)
        {
            if (!state.Video.Inputs.TryGetValue(tally.Source, out var inputChannel)) continue;

            inputChannel.IsInProgram = tally.IsInProgram;
            inputChannel.IsInPreview = tally.IsInPreview;
        }
    }
}
