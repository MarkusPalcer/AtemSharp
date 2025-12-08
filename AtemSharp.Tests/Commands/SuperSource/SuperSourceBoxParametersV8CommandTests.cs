using System.Drawing;
using AtemSharp.Commands.SuperSource;
using AtemSharp.State.Video.SuperSource;

namespace AtemSharp.Tests.Commands.SuperSource;

public class SuperSourceBoxParametersV8CommandTests : SerializedCommandTestBase<SuperSourceBoxParametersV8Command, SuperSourceBoxParametersV8CommandTests.CommandData>
{
    // Mark all as floating point as it's version dependent where the floating points are
    protected override Range[] GetFloatingPointByteRanges() => [
        (8..10), // X
        (10..12), // Y
        (12..14), // Size
        (16..18), // CropTop
        (18..20), // CropBottom
        (20..22), // CropLeft
        (22..24), // CropRight
    ];

    public class CommandData : CommandDataBase
    {
        public byte SSrcId { get; set; }
        public byte BoxIndex { get; set; }
        public bool Enabled  { get; set; }
        public ushort Source { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Size { get; set; }
        public bool Cropped  { get; set; }
        public double CropTop { get; set; }
        public double CropBottom { get; set; }
        public double CropLeft { get; set; }
        public double CropRight { get; set; }
    }

    protected override SuperSourceBoxParametersV8Command CreateSut(TestCaseData testCase)
    {
        return new SuperSourceBoxParametersV8Command(new SuperSourceBox
        {
            Id = testCase.Command.BoxIndex,
            SuperSourceId = testCase.Command.SSrcId,
            Enabled = testCase.Command.Enabled,
            Source = testCase.Command.Source,
            Location = new PointF((float)testCase.Command.PositionX, (float)testCase.Command.PositionY),
            Size = testCase.Command.Size,
            Cropped = testCase.Command.Cropped,
            CropTop = testCase.Command.CropTop,
            CropBottom = testCase.Command.CropBottom,
            CropLeft = testCase.Command.CropLeft,
            CropRight = testCase.Command.CropRight
        });
    }


    [Test]
    public void SettingLocation_ShouldSetPositionXAndPositionY()
    {
        var sut =  new SuperSourceBoxParametersV8Command(new SuperSourceBox())
        {
            Location = new PointF((float)12.3, (float)45.6)
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.X, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.Y, Is.EqualTo(45.6).Within(0.01));
        });
    }

    [Test]
    public void BettingLocation_ShouldGetPositionXAndPositionY()
    {
        var sut =  new SuperSourceBoxParametersV8Command(new SuperSourceBox())
        {
            X = 12.3,
            Y = 45.6
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.Location.X, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.Location.Y, Is.EqualTo(45.6).Within(0.01));
        });
    }
}
