using AtemSharp.State.Audio.Fairlight;
using FairlightMixerMasterLimiterCommand = AtemSharp.Commands.Fairlight.Master.FairlightMixerMasterLimiterCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterLimiterCommandTests : SerializedCommandTestBase<FairlightMixerMasterLimiterCommand, FairlightMixerMasterLimiterCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        4..8, // Threshold
        8..12, // Attack
        12..16, // Hold
        16..20  // Release
    ];

    protected override FairlightMixerMasterLimiterCommand CreateSut(TestCaseData testCase)
    {
        var master = new MasterProperties()
        {
            Dynamics =
            {
                Limiter =
                {
                    Attack = testCase.Command.Attack,
                    Hold = testCase.Command.Hold,
                    Release = testCase.Command.Release,
                    Threshold = testCase.Command.Threshold,
                    Enabled = testCase.Command.LimiterEnabled
                }
            }
        };

        return new  FairlightMixerMasterLimiterCommand(master);
    }

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
}
