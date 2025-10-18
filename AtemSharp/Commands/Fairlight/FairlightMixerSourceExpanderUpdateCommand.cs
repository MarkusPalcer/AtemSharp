using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("AIXP")]
public class FairlightMixerSourceExpanderUpdateCommand : FairlightMixerSourceUpdateCommandBase
{

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerSourceExpanderUpdateCommand
        {
            ExpanderEnabled = rawCommand.ReadBoolean(16),
            GateEnabled = rawCommand.ReadBoolean(17),
            Threshold = rawCommand.ReadInt32BigEndian(20) / 100.0,
            Range = rawCommand.ReadInt16BigEndian(24) / 100.0,
            Ratio = rawCommand.ReadInt16BigEndian(26) / 100.0,
            Attack = rawCommand.ReadInt32BigEndian(28) / 100.0,
            Hold = rawCommand.ReadInt32BigEndian(32) / 100.0,
            Release = rawCommand.ReadInt32BigEndian(36) / 100.0,
        }.DeserializeIds(rawCommand);
    }

    public double Release { get; set; }

    public double Hold { get; set; }

    public double Attack { get; set; }

    public double Ratio { get; set; }

    public double Range { get; set; }

    public double Threshold { get; set; }

    public bool GateEnabled { get; set; }

    public bool ExpanderEnabled { get; set; }


    protected override void ApplyToSource(Source source)
    {
        source.Dynamics.Expander.Enabled = ExpanderEnabled;
        source.Dynamics.Expander.GateEnabled = GateEnabled;
        source.Dynamics.Expander.Threshold = Threshold;
        source.Dynamics.Expander.Range = Range;
        source.Dynamics.Expander.Ratio = Ratio;
        source.Dynamics.Expander.Attack = Attack;
        source.Dynamics.Expander.Hold = Hold;
        source.Dynamics.Expander.Release = Release;
    }
}
