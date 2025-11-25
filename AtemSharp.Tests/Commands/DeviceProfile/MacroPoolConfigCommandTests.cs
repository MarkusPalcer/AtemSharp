using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MacroPoolConfigCommandTests : DeserializedCommandTestBase<MacroPoolConfigCommand, MacroPoolConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MacroCount { get; set; }
    }

    protected override void CompareCommandProperties(MacroPoolConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare MacroCount
        if (actualCommand.MacroCount != expectedData.MacroCount)
        {
            failures.Add($"MacroCount: expected {expectedData.MacroCount}, actual {actualCommand.MacroCount}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateInfoMacroPool()
    {
        // Arrange
        var state = new AtemState();
        var command = new MacroPoolConfigCommand
        {
            MacroCount = 172
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MacroPool, Is.Not.Null);
        Assert.That(state.Info.MacroPool.MacroCount, Is.EqualTo(172));
    }

    [Test]
    public void ApplyToState_WithMultipleCalls_ShouldOverwritePreviousData()
    {
        // Arrange
        var state = new AtemState();
        var firstCommand = new MacroPoolConfigCommand { MacroCount = 48 };
        var secondCommand = new MacroPoolConfigCommand { MacroCount = 197 };

        // Act
        firstCommand.ApplyToState(state);
        secondCommand.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MacroPool, Is.Not.Null);
        Assert.That(state.Info.MacroPool.MacroCount, Is.EqualTo(197));
    }

    [Test]
    public void ApplyToState_WithExistingDeviceInfo_ShouldPreserveOtherInfo()
    {
        // Arrange
        var state = new AtemState();
        state.Info.Mixer = new AudioMixerInfo
        {
            Inputs = 20,
            Monitors = 2,
            Headphones = 1
        };

        var command = new MacroPoolConfigCommand { MacroCount = 213 };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MacroPool, Is.Not.Null);
        Assert.That(state.Info.MacroPool.MacroCount, Is.EqualTo(213));

        // Verify other device info is preserved
        var mixer = state.Info.Mixer.As<AudioMixerInfo>();
        Assert.That(mixer.Inputs, Is.EqualTo(20));
        Assert.That(mixer.Monitors, Is.EqualTo(2));
        Assert.That(mixer.Headphones, Is.EqualTo(1));
    }
}
