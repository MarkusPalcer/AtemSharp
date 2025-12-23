using System.Drawing;
using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyPatternCommandTests : SerializedCommandTestBase<MixEffectKeyPatternCommand, MixEffectKeyPatternCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() => [
        (4..6),   // Size
        (6..8),   // Symmetry
        (8..10),  // Softness
        (10..12), // PositionX
        (12..14)  // PositionY
    ];

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public UpstreamKeyerPatternStyle Pattern { get; set; }
        public double Symmetry  { get; set; }
        public double Softness  { get; set; }
        public double XPosition  { get; set; }
        public double YPosition  { get; set; }
        public bool Inverse { get; set; }
        public double Size { get; set; }
    }

    protected override MixEffectKeyPatternCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new MixEffectKeyPatternCommand(new UpstreamKeyer
        {
            MixEffectId = testCase.Command.MixEffectIndex,
            Id = testCase.Command.KeyerIndex,
            Pattern =
            {
                Style = testCase.Command.Pattern,
                Symmetry = testCase.Command.Symmetry,
                Softness = testCase.Command.Softness,
                Location = new PointF((float)testCase.Command.XPosition, (float)testCase.Command.YPosition),
                Invert = testCase.Command.Inverse,
                Size = testCase.Command.Size
            }
        });
    }


    [Test]
    public void SettingLocation_ShouldSetPositionXAndPositionY()
    {
        var sut =  new MixEffectKeyPatternCommand(new UpstreamKeyer())
        {
            Location = new PointF((float)12.3, (float)45.6)
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.PositionX, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.PositionY, Is.EqualTo(45.6).Within(0.01));
        });
    }

    [Test]
    public void BettingLocation_ShouldGetPositionXAndPositionY()
    {
        var sut =  new MixEffectKeyPatternCommand(new UpstreamKeyer())
        {
            PositionX = 12.3,
            PositionY = 45.6
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.Location.X, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.Location.Y, Is.EqualTo(45.6).Within(0.01));
        });
    }
}
