using AtemSharp.Commands.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerSourceLimiterCommandTests : SerializedCommandTestBase<FairlightMixerSourceLimiterCommand, FairlightMixerSourceLimiterCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        20..24, // Threshold
        24..28, // Attack
        28..32, // Hold
        32..36  // Release
    ];

    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool LimiterEnabled { get; set; }
        public double Threshold { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }

    protected override FairlightMixerSourceLimiterCommand CreateSut(TestCaseData testCase)
    {
        var source = new AtemSharp.State.Audio.Fairlight.Source
        {
            InputId = testCase.Command.Index,
            Id = testCase.Command.SourceId,
            Dynamics =
            {
                Limiter =
                {
                    Enabled = testCase.Command.LimiterEnabled,
                    Threshold = testCase.Command.Threshold,
                    Attack = testCase.Command.Attack,
                    Hold = testCase.Command.Hold,
                    Release = testCase.Command.Release
                }
            }
        };
        return new FairlightMixerSourceLimiterCommand(source);
    }
}
