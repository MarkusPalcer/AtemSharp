using AtemSharp.Commands.SuperSource;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.SuperSource;

public class SuperSourceBorderCommandTests : SerializedCommandTestBase<SuperSourceBorderCommand, SuperSourceBorderCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() => [
        (6..8),
        (8..10),
        (14..16),
        (16..18),
        (18..20),
        (20..22),
        (22..23)
    ];

    public class CommandData : CommandDataBase
    {
        public byte SSrcId { get; set; }
        public bool Enabled  { get; set; }
        public BorderBevel Bevel { get; set; }
        public double OuterWidth  { get; set; }
        public double InnerWidth { get; set; }
        public byte OuterSoftness { get; set; }
        public byte InnerSoftness { get; set; }
        public byte BevelSoftness { get; set; }
        public byte BevelPosition  { get; set; }
        public double Hue    { get; set; }
        public double Saturation  { get; set; }
        public double Luma  { get; set; }
        public double LightSourceDirection  { get; set; }
        public double LightSourceAltitude { get; set; }
    }

    protected override SuperSourceBorderCommand CreateSut(TestCaseData testCase)
    {
        return new SuperSourceBorderCommand(new AtemSharp.State.SuperSource()
        {
            Id = testCase.Command.SSrcId,
            Border =
            {
                Enabled = testCase.Command.Enabled,
                Bevel = testCase.Command.Bevel,
                OuterWidth = testCase.Command.OuterWidth,
                InnerWidth = testCase.Command.InnerWidth,
                OuterSoftness = testCase.Command.OuterSoftness,
                InnerSoftness = testCase.Command.InnerSoftness,
                BevelSoftness = testCase.Command.BevelSoftness,
                BevelPosition = testCase.Command.BevelPosition,
                Hue = testCase.Command.Hue,
                Saturation = testCase.Command.Saturation,
                Luma = testCase.Command.Luma,
                LightSourceDirection = testCase.Command.LightSourceDirection,
                LightSourceAltitude = testCase.Command.LightSourceAltitude
            }
        });
    }
}
