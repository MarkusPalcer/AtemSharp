using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyOnAirCommandTests : SerializedCommandTestBase<MixEffectKeyOnAirCommand,
    MixEffectKeyOnAirCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public bool OnAir { get; set; }
    }

    protected override MixEffectKeyOnAirCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and upstream keyer
        var state = new UpstreamKeyer
        {
            Id = testCase.Command.KeyerIndex,
            MixEffectId = testCase.Command.MixEffectIndex,
            OnAir = testCase.Command.OnAir
        };

        // Create command with the mix effect and keyer IDs
        var command = new MixEffectKeyOnAirCommand(state);

        return command;
    }
}
