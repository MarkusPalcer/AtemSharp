using AtemSharp.Commands.SuperSource;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.SuperSource;

internal class SuperSourceBoxParametersUpdateCommandV8Tests : DeserializedCommandTestBase<SuperSourceBoxParametersUpdateCommandV8, SuperSourceBoxParametersUpdateCommandV8Tests.CommandData>
{
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

    internal override void CompareCommandProperties(SuperSourceBoxParametersUpdateCommandV8 actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.SuperSourceId, Is.EqualTo(expectedData.SSrcId));
        Assert.That(actualCommand.BoxId, Is.EqualTo(expectedData.BoxIndex));
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.Enabled));
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
        Assert.That(actualCommand.X, Is.EqualTo(expectedData.PositionX).Within(0.01));
        Assert.That(actualCommand.Y, Is.EqualTo(expectedData.PositionY).Within(0.01));
        Assert.That(actualCommand.Size, Is.EqualTo(expectedData.Size).Within(0.001));
        Assert.That(actualCommand.Cropped, Is.EqualTo(expectedData.Cropped));
        Assert.That(actualCommand.CropTop, Is.EqualTo(expectedData.CropTop).Within(0.001));
        Assert.That(actualCommand.CropBottom, Is.EqualTo(expectedData.CropBottom).Within(0.001));
        Assert.That(actualCommand.CropLeft, Is.EqualTo(expectedData.CropLeft).Within(0.001));
        Assert.That(actualCommand.CropRight, Is.EqualTo(expectedData.CropRight).Within(0.001));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.SuperSources.GetOrCreate(expectedData.SSrcId);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand =  state.Video.SuperSources[expectedData.SSrcId].Boxes[expectedData.BoxIndex];
        Assert.That(actualCommand.SuperSourceId, Is.EqualTo(expectedData.SSrcId));
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.BoxIndex));
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.Enabled));
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
        Assert.That(actualCommand.Location.X, Is.EqualTo(expectedData.PositionX).Within(0.01));
        Assert.That(actualCommand.Location.Y, Is.EqualTo(expectedData.PositionY).Within(0.01));
        Assert.That(actualCommand.Size, Is.EqualTo(expectedData.Size).Within(0.001));
        Assert.That(actualCommand.Cropped, Is.EqualTo(expectedData.Cropped));
        Assert.That(actualCommand.CropTop, Is.EqualTo(expectedData.CropTop).Within(0.001));
        Assert.That(actualCommand.CropBottom, Is.EqualTo(expectedData.CropBottom).Within(0.001));
        Assert.That(actualCommand.CropLeft, Is.EqualTo(expectedData.CropLeft).Within(0.001));
        Assert.That(actualCommand.CropRight, Is.EqualTo(expectedData.CropRight).Within(0.001));
    }
}
