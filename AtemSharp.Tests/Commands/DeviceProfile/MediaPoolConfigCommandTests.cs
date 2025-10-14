using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MediaPoolConfigCommandTests : DeserializedCommandTestBase<MediaPoolConfigCommand, MediaPoolConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte StillCount { get; set; }
        public byte ClipCount { get; set; }
    }

    protected override void CompareCommandProperties(MediaPoolConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare StillCount
        if (actualCommand.StillCount != expectedData.StillCount)
        {
            failures.Add($"StillCount: expected {expectedData.StillCount}, actual {actualCommand.StillCount}");
        }

        // Compare ClipCount
        if (actualCommand.ClipCount != expectedData.ClipCount)
        {
            failures.Add($"ClipCount: expected {expectedData.ClipCount}, actual {actualCommand.ClipCount}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateInfoMediaPool()
    {
        // Arrange
        var state = new AtemState();
        var command = new MediaPoolConfigCommand
        {
            StillCount = 20,
            ClipCount = 2
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MediaPool, Is.Not.Null);
        Assert.That(state.Info.MediaPool.StillCount, Is.EqualTo(20));
        Assert.That(state.Info.MediaPool.ClipCount, Is.EqualTo(2));
    }

    [Test]
    public void ApplyToState_WithZeroValues_ShouldUpdateInfoMediaPool()
    {
        // Arrange
        var state = new AtemState();
        var command = new MediaPoolConfigCommand
        {
            StillCount = 0,
            ClipCount = 0
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MediaPool, Is.Not.Null);
        Assert.That(state.Info.MediaPool.StillCount, Is.EqualTo(0));
        Assert.That(state.Info.MediaPool.ClipCount, Is.EqualTo(0));
    }

    [Test]
    public void ApplyToState_WithMaxValues_ShouldUpdateInfoMediaPool()
    {
        // Arrange
        var state = new AtemState();
        var command = new MediaPoolConfigCommand
        {
            StillCount = 255,
            ClipCount = 255
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MediaPool, Is.Not.Null);
        Assert.That(state.Info.MediaPool.StillCount, Is.EqualTo(255));
        Assert.That(state.Info.MediaPool.ClipCount, Is.EqualTo(255));
    }

    [Test]
    public void ApplyToState_WhenCalledMultipleTimes_ShouldOverwritePreviousValues()
    {
        // Arrange
        var state = new AtemState();
        var firstCommand = new MediaPoolConfigCommand
        {
            StillCount = 10,
            ClipCount = 5
        };
        var secondCommand = new MediaPoolConfigCommand
        {
            StillCount = 20,
            ClipCount = 2
        };

        // Act
        firstCommand.ApplyToState(state);
        secondCommand.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MediaPool, Is.Not.Null);
        Assert.That(state.Info.MediaPool.StillCount, Is.EqualTo(20));
        Assert.That(state.Info.MediaPool.ClipCount, Is.EqualTo(2));
    }
}
