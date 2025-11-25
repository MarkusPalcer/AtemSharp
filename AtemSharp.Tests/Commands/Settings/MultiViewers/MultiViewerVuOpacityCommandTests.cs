using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerVuOpacityCommandTests : SerializedCommandTestBase<MultiViewerVuOpacityCommand,
    MultiViewerVuOpacityCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public byte Opacity { get; set; }
    }

    protected override MultiViewerVuOpacityCommand CreateSut(TestCaseData testCase)
    {
        var state = new MultiViewer
        {
            Id = testCase.Command.MultiviewIndex,
            VuOpacity = testCase.Command.Opacity
        };

        return new MultiViewerVuOpacityCommand(state);
    }
}
