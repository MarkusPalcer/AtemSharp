using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerWindowSafeAreaCommandTests : SerializedCommandTestBase<MultiViewerWindowSafeAreaCommand,
    MultiViewerWindowSafeAreaCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; } // Match TypeScript property name
        public byte WindowIndex { get; set; }
        public bool SafeAreaEnabled { get; set; }
    }

    protected override MultiViewerWindowSafeAreaCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new MultiViewerWindowSafeAreaCommand(new MultiViewerWindowState
        {
            MultiViewerId = testCase.Command.MultiviewIndex,
            WindowIndex = testCase.Command.WindowIndex,
            SafeTitle = testCase.Command.SafeAreaEnabled
        });
    }
}
