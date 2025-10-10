using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Enums;
using AtemSharp.State;

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
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MacroPool, Is.Not.Null);
        Assert.That(state.Info.MacroPool.MacroCount, Is.EqualTo(172));
        
        Assert.That(result, Is.EqualTo(new[] { "info.macroPool" }));
    }

    [Test]
    public void Deserialize_WithTypicalValues_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0xAC }; // MacroCount: 172 (0xAC in hex)
        using var stream = new MemoryStream(data);

        // Act
        var command = MacroPoolConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.MacroCount, Is.EqualTo(172));
    }

    [Test]
    public void Deserialize_WithMaxValue_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0xFE }; // MacroCount: 254 (0xFE in hex)
        using var stream = new MemoryStream(data);

        // Act
        var command = MacroPoolConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.MacroCount, Is.EqualTo(254));
    }

    [Test]
    public void Deserialize_WithMinValue_ShouldDeserializeCorrectly()
    {
        // Arrange
        var data = new byte[] { 0x00 }; // MacroCount: 0
        using var stream = new MemoryStream(data);

        // Act
        var command = MacroPoolConfigCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.MacroCount, Is.EqualTo(0));
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
        var result = secondCommand.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MacroPool, Is.Not.Null);
        Assert.That(state.Info.MacroPool.MacroCount, Is.EqualTo(197));
        
        Assert.That(result, Is.EqualTo(new[] { "info.macroPool" }));
    }

    [Test]
    public void ApplyToState_WithExistingDeviceInfo_ShouldPreserveOtherInfo()
    {
        // Arrange
        var state = new AtemState();
        state.Info.AudioMixer = new AudioMixerInfo
        {
            Inputs = 20,
            Monitors = 2,
            Headphones = 1
        };

        var command = new MacroPoolConfigCommand { MacroCount = 213 };

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.MacroPool, Is.Not.Null);
        Assert.That(state.Info.MacroPool.MacroCount, Is.EqualTo(213));
        
        // Verify other device info is preserved
        Assert.That(state.Info.AudioMixer, Is.Not.Null);
        Assert.That(state.Info.AudioMixer.Inputs, Is.EqualTo(20));
        Assert.That(state.Info.AudioMixer.Monitors, Is.EqualTo(2));
        Assert.That(state.Info.AudioMixer.Headphones, Is.EqualTo(1));
        
        Assert.That(result, Is.EqualTo(new[] { "info.macroPool" }));
    }
}