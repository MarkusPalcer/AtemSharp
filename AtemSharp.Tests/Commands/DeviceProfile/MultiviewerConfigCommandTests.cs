using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MultiviewerConfigCommandTests : DeserializedCommandTestBase<MultiviewerConfigCommand, MultiviewerConfigCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public int Count { get; set; }
		public int WindowCount { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool CanRouteInputs { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool CanSwapPreviewProgram { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool CanToggleSafeArea { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool SupportsVuMeters { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool SupportsQuadrants { get; set; }

        // TODO: Check how this is deserialized and applied
        public bool CanChangeLayout { get; set; }
	}

	protected override void CompareCommandProperties(MultiviewerConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actualCommand.Count, Is.EqualTo(expectedData.Count));
            Assert.That(actualCommand.WindowCount, Is.EqualTo(expectedData.WindowCount));
        });
    }

	[Test]
	public void ApplyToState_WithNullMultiViewer_ShouldInitializeMultiViewer()
	{
		// Arrange
		var state = new AtemState();
		var command = new MultiviewerConfigCommand
		{
			Count = 2,
			WindowCount = 10
		};

		// Act
		command.ApplyToState(state);

		// Assert
		Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(2));
		Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(10));
	}

	[Test]
	public void ApplyToState_WithExistingMultiViewer_ShouldUpdateProperties()
	{
		// Arrange
		var state = new AtemState
		{
            Info = {
			    MultiViewer =
			    {
				    Count = 1,
				    WindowCount = 8
			    }
            }
		};

		var command = new MultiviewerConfigCommand
		{
			Count = 4,
			WindowCount = 16
		};

		// Act
		command.ApplyToState(state);

		// Assert
		Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(4));
		Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(16));
	}

	[Test]
	public void ApplyToState_WithZeroValues_ShouldSetZeroValues()
	{
		// Arrange
		var state = new AtemState();
		var command = new MultiviewerConfigCommand
		{
			Count = 0,
			WindowCount = 0
		};

		// Act
		command.ApplyToState(state);

		// Assert
		Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(0));
		Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(0));
	}

	[Test]
	public void ApplyToState_WithMaxValues_ShouldSetMaxValues()
	{
		// Arrange
		var state = new AtemState();
		var command = new MultiviewerConfigCommand
		{
			Count = 255,
			WindowCount = 255
		};

		// Act
		command.ApplyToState(state);

		// Assert
		Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(255));
		Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(255));
	}

	[Test]
	public void ApplyToState_WithTypicalAtemConfiguration_ShouldUpdateCorrectly()
	{
		// Arrange - test with typical ATEM multiviewer values
		var state = new AtemState();
		var command = new MultiviewerConfigCommand
		{
			Count = 1,
			WindowCount = 10
		};

		// Act
		command.ApplyToState(state);

		// Assert
		Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(1));
		Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(10));
	}

	[Test]
	public void ApplyToState_MultipleCallsOnSameState_ShouldOverwritePreviousValues()
	{
		// Arrange
		var state = new AtemState();

		var commands = new[]
		{
			new MultiviewerConfigCommand { Count = 1, WindowCount = 4 },
			new MultiviewerConfigCommand { Count = 2, WindowCount = 8 },
			new MultiviewerConfigCommand { Count = 4, WindowCount = 16 }
		};

		// Act & Assert each step
		foreach (var command in commands)
		{
			command.ApplyToState(state);

			Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(command.Count));
			Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(command.WindowCount));
		}

		// Final state should match the last command
		Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(4));
		Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(16));
	}

	[Test]
	public void ApplyToState_SequentialUpdates_ShouldUpdateStateCorrectly()
	{
		// Arrange
		var state = new AtemState();

		var firstCommand = new MultiviewerConfigCommand
		{
			Count = 1,
			WindowCount = 8
		};

		var secondCommand = new MultiviewerConfigCommand
		{
			Count = 2,
			WindowCount = 16
		};

		// Act
		firstCommand.ApplyToState(state);
		secondCommand.ApplyToState(state);

		// Assert
		Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(2)); // Should have the latest value
		Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(16)); // Should have the latest value
	}

	[Test]
	public void ApplyToState_WithBoundaryValues_ShouldHandleEdgeCases()
	{
		// Arrange
		var state = new AtemState();

		// Test cases for different boundary scenarios
		var testCases = new[]
		{
			new { Count = 0, WindowCount = 1, Description = "Minimum windows" },
			new { Count = 255, WindowCount = 1, Description = "Maximum count" },
			new { Count = 1, WindowCount = 255, Description = "Maximum windows" }
		};

		foreach (var testCase in testCases)
		{
			// Act
			var command = new MultiviewerConfigCommand
			{
				Count = testCase.Count,
				WindowCount = testCase.WindowCount
			};

			command.ApplyToState(state);

			// Assert
			Assert.That(state.Info.MultiViewer.Count, Is.EqualTo(testCase.Count), $"Count failed for {testCase.Description}");
			Assert.That(state.Info.MultiViewer.WindowCount, Is.EqualTo(testCase.WindowCount), $"WindowCount failed for {testCase.Description}");
		}
	}
}
