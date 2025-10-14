using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class VersionCommandTests : DeserializedCommandTestBase<VersionCommand, VersionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ProtocolVersion ProtocolVersion { get; set; }
    }

    protected override void CompareCommandProperties(VersionCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare ProtocolVersion
        if (actualCommand.Version != expectedData.ProtocolVersion)
        {
            failures.Add($"Version: expected {expectedData.ProtocolVersion}, actual {actualCommand.Version}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateApiVersion()
    {
        // Arrange
        var state = new AtemState();
        var command = new VersionCommand
        {
            Version = ProtocolVersion.V8_0
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.ApiVersion, Is.EqualTo(ProtocolVersion.V8_0));
    }

    [Test]
    public void ApplyToState_WithDifferentVersions_ShouldUpdateCorrectly()
    {
        // Arrange
        var state = new AtemState();
        var command = new VersionCommand
        {
            Version = ProtocolVersion.V9_4
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.ApiVersion, Is.EqualTo(ProtocolVersion.V9_4));
    }

    [Test]
    public void ApplyToState_OverwriteExistingVersion_ShouldUpdateCorrectly()
    {
        // Arrange
        var state = new AtemState();
        state.Info.ApiVersion = ProtocolVersion.V8_0; // Set initial version

        var command = new VersionCommand
        {
            Version = ProtocolVersion.V9_6
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.ApiVersion, Is.EqualTo(ProtocolVersion.V9_6));
    }
}
