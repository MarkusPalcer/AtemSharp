using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerSourceCommandTests : SerializedCommandTestBase<MultiViewerSourceCommand,
	MultiViewerSourceCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public int MultiviewIndex { get; set; } // Match TypeScript property name
		public int WindowIndex { get; set; }
		public int Source { get; set; }
	}

	protected override MultiViewerSourceCommand CreateSut(TestCaseData testCase)
	{
		// Create state with the required MultiViewer
		var state = CreateStateWithMultiViewer(testCase.Command.MultiviewIndex);

		// Create command with the MultiViewer ID
		var command = new MultiViewerSourceCommand(testCase.Command.MultiviewIndex, state);

		// Set the actual values that should be written
		command.WindowIndex = testCase.Command.WindowIndex;
		command.Source = testCase.Command.Source;

		return command;
	}

	/// <summary>
	/// Creates an AtemState with a valid MultiViewer at the specified index
	/// </summary>
	private static AtemState CreateStateWithMultiViewer(int multiViewerId)
	{
		var multiViewers = new Dictionary<int, MultiViewer>
		{
			{ multiViewerId, new MultiViewer(multiViewerId) }
		};

		return new AtemState
		{
			Settings = new SettingsState
			{
				MultiViewers = multiViewers
			},
			Info = new DeviceInfo
			{
				MultiViewer = new MultiViewerInfo
				{
					Count = multiViewerId + 1,
					WindowCount = 10
				}
			}
		};
	}

	[Test]
	public void Constructor_WithEmptyState_ThrowsInvalidIdError()
	{
		// Arrange
		const int multiViewerId = 1;
		var state = new AtemState(); // Empty state

		// Act & Assert
		Assert.Throws<InvalidIdError>(() => _ = new MultiViewerSourceCommand(multiViewerId, state));
	}

	[Test]
	public void Constructor_WithValidState_InitializesWithDefaults()
	{
		// Arrange
		const int multiViewerId = 1;
		var state = CreateStateWithMultiViewer(multiViewerId);

		// Act
		var command = new MultiViewerSourceCommand(multiViewerId, state);

		// Assert
		Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
		Assert.That(command.WindowIndex, Is.EqualTo(0));
		Assert.That(command.Source, Is.EqualTo(0));
	}

	[Test]
	public void ConstructorWithParameters_SetsPropertiesCorrectly()
	{
		// Arrange
		const int multiViewerId = 0;
		const int windowIndex = 5;
		const int source = 1234;
		var state = CreateStateWithMultiViewer(multiViewerId);

		// Act
		var command = new MultiViewerSourceCommand(multiViewerId, windowIndex, source, state);

		// Assert
		Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
		Assert.That(command.WindowIndex, Is.EqualTo(windowIndex));
		Assert.That(command.Source, Is.EqualTo(source));
	}

	[Test]
	public void WindowIndex_WhenSet_UpdatesCorrectly()
	{
		// Arrange
		const int multiViewerId = 0;
		var state = CreateStateWithMultiViewer(multiViewerId);
		var command = new MultiViewerSourceCommand(multiViewerId, state);

		// Act
		command.WindowIndex = 7;

		// Assert
		Assert.That(command.WindowIndex, Is.EqualTo(7));
	}

	[Test]
	public void Source_WhenSet_UpdatesCorrectly()
	{
		// Arrange
		const int multiViewerId = 0;
		var state = CreateStateWithMultiViewer(multiViewerId);
		var command = new MultiViewerSourceCommand(multiViewerId, state);

		// Act
		command.Source = 2000;

		// Assert
		Assert.That(command.Source, Is.EqualTo(2000));
	}

	[Test]
	public void Serialize_CreatesCorrectPacketStructure()
	{
		// Arrange
		const int multiViewerId = 2;
		const int windowIndex = 8;
		const int source = 1500;
		var state = CreateStateWithMultiViewer(multiViewerId);
		var command = new MultiViewerSourceCommand(multiViewerId, windowIndex, source, state);

		// Act
		var result = command.Serialize(ProtocolVersion.V8_0);

		// Assert
		Assert.That(result.Length, Is.EqualTo(4), "Serialized command should be 4 bytes");
		Assert.That(result[0], Is.EqualTo(multiViewerId), "First byte should be MultiViewer ID");
		Assert.That(result[1], Is.EqualTo(windowIndex), "Second byte should be window index");

		// Source is big-endian UInt16 at bytes 2-3
		var actualSource = (result[2] << 8) | result[3];
		Assert.That(actualSource, Is.EqualTo(source), "Bytes 2-3 should contain source as big-endian UInt16");
	}
}