using AtemSharp.Commands;
using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Lib;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Lib;

[TestFixture]
public class CommandParserTests
{
    [Test]
    public void Constructor_ShouldRegisterCommands()
    {
        // Arrange & Act
        var parser = new CommandParser();

        // Assert
        var registeredCommands = parser.GetRegisteredCommands();
        Assert.That(registeredCommands, Contains.Item("_ver"));
        Assert.That(registeredCommands, Contains.Item("InCm"));
    }

    [Test]
    public void GetCommandType_WithValidRawName_ShouldReturnCorrectType()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var commandType = parser.GetCommandType("_ver");

        // Assert
        Assert.That(commandType, Is.EqualTo(typeof(VersionCommand)));
    }

    [Test]
    public void GetCommandType_WithInvalidRawName_ShouldReturnNull()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var commandType = parser.GetCommandType("XXXX");

        // Assert
        Assert.That(commandType, Is.Null);
    }

    [Test]
    public void ParseCommand_WithUnknownCommand_ShouldTrackInUnknownCommands()
    {
        // Arrange
        var parser = new CommandParser();
        AtemSwitcher.UnknownCommands.Clear();

        // Act
        var result = parser.ParseCommand("UNKN", Span<byte>.Empty);

        // Assert
        Assert.That(result, Is.Null);
        Assert.That(AtemSwitcher.UnknownCommands, Contains.Item("UNKN"));
    }

    [Test]
    public void ParseCommand_WithVersionCommand_ShouldUpdateParserVersion()
    {
        // Arrange
        var parser = new CommandParser();

        // Create a stream with version data (protocol version in big-endian format)
        var versionBytes = BitConverter.GetBytes((uint)ProtocolVersion.V8_1_1);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(versionBytes);
        }

        // Act
        var initialVersion = parser.Version;
        var result = parser.ParseCommand("_ver", versionBytes.AsSpan());

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<VersionCommand>());
        Assert.That(parser.Version, Is.EqualTo(ProtocolVersion.V8_1_1));
        Assert.That(parser.Version, Is.Not.EqualTo(initialVersion));
    }

    [Test]
    public void GetCommandTypeForVersion_WithMultipleVersions_ShouldSelectCorrectVersion()
    {
        // Arrange
        var parser = new CommandParser();

        // Test with different protocol versions to ensure correct command selection
        parser.Version = ProtocolVersion.V7_2;
        var commandTypeV7 = parser.GetCommandType("_ver");

        parser.Version = ProtocolVersion.V8_1_1;
        var commandTypeV8 = parser.GetCommandType("_ver");

        // Assert
        Assert.That(commandTypeV7, Is.Not.Null);
        Assert.That(commandTypeV8, Is.Not.Null);
        // Both should be VersionCommand since there's likely only one version
        Assert.That(commandTypeV7, Is.EqualTo(typeof(VersionCommand)));
        Assert.That(commandTypeV8, Is.EqualTo(typeof(VersionCommand)));
    }

    [Test]
    public void GetAllCommandVersions_ShouldReturnAllVersionsForCommand()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var versions = parser.GetAllCommandVersions("_ver");

        // Assert
        Assert.That(versions, Is.Not.Empty);
        Assert.That(versions.All(t => typeof(IDeserializedCommand).IsAssignableFrom(t)), Is.True);
    }

    [Test]
    public void GetAllCommandVersions_WithUnknownCommand_ShouldReturnEmptyList()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var versions = parser.GetAllCommandVersions("UNKN");

        // Assert
        Assert.That(versions, Is.Empty);
    }
}
