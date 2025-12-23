using AtemSharp.Commands.SuperSource;
using AtemSharp.State.Video.SuperSource;

namespace AtemSharp.Tests.Commands.SuperSource;

public class SuperSourcePropertiesV8CommandTests : SerializedCommandTestBase<SuperSourcePropertiesV8Command,
    SuperSourcePropertiesV8CommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        (8..10),
        (10..12)
    ];

    public class CommandData : CommandDataBase
    {
        public byte SSrcId { get; set; }
        public ushort ArtFillSource { get; set; }
        public ushort ArtCutSource { get; set; }
        public ArtOption ArtOption { get; set; }
        public bool ArtPreMultiplied { get; set; }
        public double ArtClip { get; set; }
        public double ArtGain { get; set; }
        public bool ArtInvertKey { get; set; }
    }

    protected override SuperSourcePropertiesV8Command CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new SuperSourcePropertiesV8Command(new AtemSharp.State.Video.SuperSource.SuperSource
        {
            Id = testCase.Command.SSrcId,
            FillSource = testCase.Command.ArtFillSource,
            CutSource = testCase.Command.ArtCutSource,
            Option = testCase.Command.ArtOption,
            PreMultipliedKey =
            {
                Enabled = testCase.Command.ArtPreMultiplied,
                Clip = testCase.Command.ArtClip,
                Gain = testCase.Command.ArtGain,
                Inverted = testCase.Command.ArtInvertKey,
            }
        });
    }
}
