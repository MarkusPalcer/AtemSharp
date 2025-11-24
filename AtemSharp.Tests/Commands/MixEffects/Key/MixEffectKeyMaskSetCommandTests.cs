using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyMaskSetCommandTests : SerializedCommandTestBase<AtemSharp.Commands.MixEffects.Key.MixEffectKeyMaskSetCommand, MixEffectKeyMaskSetCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() => [
        (4..6),
        (6..8),
        (8..10),
        (10..12)
    ];

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    protected override AtemSharp.Commands.MixEffects.Key.MixEffectKeyMaskSetCommand CreateSut(TestCaseData testCase)
    {
        return new AtemSharp.Commands.MixEffects.Key.MixEffectKeyMaskSetCommand(new UpstreamKeyer
        {
            Id = testCase.Command.KeyerIndex,
            MixEffectId = testCase.Command.MixEffectIndex,
            Mask =
            {
                Enabled = testCase.Command.MaskEnabled,
                Top = testCase.Command.MaskTop,
                Bottom = testCase.Command.MaskBottom,
                Left = testCase.Command.MaskLeft,
                Right = testCase.Command.MaskRight
            }
        });
    }
}
