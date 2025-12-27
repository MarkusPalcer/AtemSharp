using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
internal class MultiViewerWindowVuMeterUpdateCommandTests : DeserializedCommandTestBase<MultiViewerWindowVuMeterUpdateCommand,
    MultiViewerWindowVuMeterUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public byte WindowIndex { get; set; }
        public bool VuEnabled { get; set; }
    }

    internal override void CompareCommandProperties(MultiViewerWindowVuMeterUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex));
        Assert.That(actualCommand.VuEnabled, Is.EqualTo(expectedData.VuEnabled));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Settings.MultiViewers.GetOrCreate(expectedData.MultiviewIndex);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Settings.MultiViewers[expectedData.MultiviewIndex].Windows[expectedData.WindowIndex];
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex));
        Assert.That(actualCommand.VuMeter, Is.EqualTo(expectedData.VuEnabled));
    }
}
