using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MultiviewerConfigCommandTests : DeserializedCommandTestBase<MultiviewerConfigCommand, MultiviewerConfigCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public int Count { get; set; }
		public int WindowCount { get; set; }
	}

	protected override void CompareCommandProperties(MultiviewerConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
	{
		Assert.That(actualCommand.Count, Is.EqualTo(expectedData.Count));
		Assert.That(actualCommand.WindowCount, Is.EqualTo(expectedData.WindowCount));
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
		var result = command.ApplyToState(state);

		// Assert
		Assert.That(state.MultiViewer, Is.Not.Null);
		Assert.That(state.MultiViewer.Count, Is.EqualTo(2));
		Assert.That(state.MultiViewer.WindowCount, Is.EqualTo(10));
		Assert.That(result, Is.EqualTo(new[] { "multiViewer" }));
	}

	[Test]
	public void ApplyToState_WithExistingMultiViewer_ShouldUpdateProperties()
	{
		// Arrange
		var state = new AtemState
		{
			MultiViewer = new MultiViewerState
			{
				Count = 1,
				WindowCount = 8
			}
		};
		
		var command = new MultiviewerConfigCommand
		{
			Count = 4,
			WindowCount = 16
		};

		// Act
		var result = command.ApplyToState(state);

		// Assert
		Assert.That(state.MultiViewer, Is.Not.Null);
		Assert.That(state.MultiViewer.Count, Is.EqualTo(4));
		Assert.That(state.MultiViewer.WindowCount, Is.EqualTo(16));
		Assert.That(result, Is.EqualTo(new[] { "multiViewer" }));
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
		var result = command.ApplyToState(state);

		// Assert
		Assert.That(state.MultiViewer, Is.Not.Null);
		Assert.That(state.MultiViewer.Count, Is.EqualTo(0));
		Assert.That(state.MultiViewer.WindowCount, Is.EqualTo(0));
		Assert.That(result, Is.EqualTo(new[] { "multiViewer" }));
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
		var result = command.ApplyToState(state);

		// Assert
		Assert.That(state.MultiViewer, Is.Not.Null);
		Assert.That(state.MultiViewer.Count, Is.EqualTo(255));
		Assert.That(state.MultiViewer.WindowCount, Is.EqualTo(255));
		Assert.That(result, Is.EqualTo(new[] { "multiViewer" }));
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
		var result = command.ApplyToState(state);

		// Assert
		Assert.That(state.MultiViewer, Is.Not.Null);
		Assert.That(state.MultiViewer.Count, Is.EqualTo(1));
		Assert.That(state.MultiViewer.WindowCount, Is.EqualTo(10));
		Assert.That(result, Is.EqualTo(new[] { "multiViewer" }));
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
			var result = command.ApplyToState(state);
			
			Assert.That(state.MultiViewer, Is.Not.Null);
			Assert.That(state.MultiViewer.Count, Is.EqualTo(command.Count));
			Assert.That(state.MultiViewer.WindowCount, Is.EqualTo(command.WindowCount));
			Assert.That(result, Is.EqualTo(new[] { "multiViewer" }));
		}

		// Final state should match the last command
		Assert.That(state.MultiViewer, Is.Not.Null);
		Assert.That(state.MultiViewer.Count, Is.EqualTo(4));
		Assert.That(state.MultiViewer.WindowCount, Is.EqualTo(16));
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
		var result1 = firstCommand.ApplyToState(state);
		var result2 = secondCommand.ApplyToState(state);

		// Assert
		Assert.That(state.MultiViewer, Is.Not.Null);
		Assert.That(state.MultiViewer.Count, Is.EqualTo(2)); // Should have the latest value
		Assert.That(state.MultiViewer.WindowCount, Is.EqualTo(16)); // Should have the latest value
		Assert.That(result1, Is.EqualTo(new[] { "multiViewer" }));
		Assert.That(result2, Is.EqualTo(new[] { "multiViewer" }));
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
			new { Count = 1, WindowCount = 0, Description = "No windows" },
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
			
			var result = command.ApplyToState(state);

			// Assert
			Assert.That(state.MultiViewer, Is.Not.Null, $"Failed for {testCase.Description}");
			Assert.That(state.MultiViewer.Count, Is.EqualTo(testCase.Count), $"Count failed for {testCase.Description}");
			Assert.That(state.MultiViewer.WindowCount, Is.EqualTo(testCase.WindowCount), $"WindowCount failed for {testCase.Description}");
			Assert.That(result, Is.EqualTo(new[] { "multiViewer" }), $"Result failed for {testCase.Description}");
		}
	}

	[Test]
	public void ApplyToState_ReturnValue_ShouldAlwaysReturnMultiViewerPath()
	{
		// Arrange
		var state = new AtemState();
		var commands = new[]
		{
			new MultiviewerConfigCommand { Count = 0, WindowCount = 0 },
			new MultiviewerConfigCommand { Count = 1, WindowCount = 10 },
			new MultiviewerConfigCommand { Count = 255, WindowCount = 255 }
		};

		// Act & Assert
		foreach (var command in commands)
		{
			var result = command.ApplyToState(state);
			Assert.That(result, Is.EqualTo(new[] { "multiViewer" }));
			Assert.That(result.Length, Is.EqualTo(1));
			Assert.That(result[0], Is.EqualTo("multiViewer"));
		}
	}
}