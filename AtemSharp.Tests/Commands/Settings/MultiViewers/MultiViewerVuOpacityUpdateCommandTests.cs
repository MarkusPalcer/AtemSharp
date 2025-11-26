using AtemSharp.Commands.Settings.MultiViewers;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerVuOpacityUpdateCommandTests : DeserializedCommandTestBase<MultiViewerVuOpacityUpdateCommand,
    MultiViewerVuOpacityUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public byte Opacity { get; set; }
    }

    protected override void CompareCommandProperties(MultiViewerVuOpacityUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Opacity, Is.EqualTo(expectedData.Opacity));
    }
}
