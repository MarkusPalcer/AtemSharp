using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerSourceCommandTests : SerializedCommandTestBase<MultiViewerSourceCommand,
	MultiViewerSourceCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public byte MultiviewIndex { get; set; }
		public byte WindowIndex { get; set; }
		public ushort Source { get; set; }
	}

	protected override MultiViewerSourceCommand CreateSut(TestCaseData testCase)
	{
        return new MultiViewerSourceCommand(new MultiViewerWindowState
        {
            WindowIndex = testCase.Command.WindowIndex,
            MultiViewerId = testCase.Command.MultiviewIndex,
            Source = testCase.Command.Source
        });
	}
}
