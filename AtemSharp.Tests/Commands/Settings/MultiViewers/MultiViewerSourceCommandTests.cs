using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

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
        var state = new MultiViewerWindowState
        {
            WindowIndex = testCase.Command.WindowIndex,
            MultiViewerId = testCase.Command.MultiviewIndex,
            Source = testCase.Command.Source
        };

        return new MultiViewerSourceCommand(state);
	}

	[Test]
	public void ConstructorWithParameters_SetsPropertiesFromState()
	{
		// Arrange
		const int multiViewerId = 0;
		const int windowIndex = 5;
		const int source = 1234;
        var state = new MultiViewerWindowState
        {
            WindowIndex = windowIndex,
            MultiViewerId = multiViewerId,
            Source = source
        };

		// Act
		var command = new MultiViewerSourceCommand(state);

		// Assert
		Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
		Assert.That(command.WindowIndex, Is.EqualTo(windowIndex));
		Assert.That(command.Source, Is.EqualTo(source));
	}
}
