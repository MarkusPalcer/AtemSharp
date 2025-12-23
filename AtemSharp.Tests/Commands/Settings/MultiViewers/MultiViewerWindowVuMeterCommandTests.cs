using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerWindowVuMeterCommandTests : SerializedCommandTestBase<MultiViewerWindowVuMeterCommand,
    MultiViewerWindowVuMeterCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public byte WindowIndex { get; set; }
        public bool VuEnabled { get; set; }
    }

    protected override MultiViewerWindowVuMeterCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new MultiViewerWindowVuMeterCommand(new MultiViewerWindowState
        {
            MultiViewerId = testCase.Command.MultiviewIndex,
            WindowIndex = testCase.Command.WindowIndex,
            VuMeter = testCase.Command.VuEnabled
        });
    }
}
