using AtemSharp.Commands;
using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Commands.SuperSource;
using AtemSharp.Communication;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Communication;

[TestFixture]
[Parallelizable(ParallelScope.None)]
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

    [Test]
    public void ParseUnknownCommand_ShouldReturnNull()
    {
        // Arrange
        var parser = new CommandParser();
        Span<byte> unknownData = [0x01, 0x02];

        // Act
        var command = parser.ParseCommand("XXXX", unknownData);

        // Assert
        Assert.That(command, Is.Null);
    }

    [Test]
    public void ParseInitCompleteCommand_ShouldReturnValidCommand()
    {
        // Arrange
        var parser = new CommandParser();
        var data = Span<byte>.Empty;

        // Act
        var command = parser.ParseCommand("InCm", data);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.TypeOf<InitCompleteCommand>());
    }


    [Test]
    public void WithDifferentVersions_ShouldSelectCorrectVersionedCommand()
    {
        // Arrange
        var parser = new CommandParser();

        var baselineCommand = parser.GetCommandType("SSBP");
        Assert.That(baselineCommand, Is.SameAs(typeof(SuperSourceBoxParametersUpdateCommand)));

        parser.Version = ProtocolVersion.V7_5_2;
        var oldVersionedCommand = parser.GetCommandType("SSBP");
        Assert.That(oldVersionedCommand, Is.SameAs(typeof(SuperSourceBoxParametersUpdateCommand)));

        parser.Version = ProtocolVersion.V9_4;
        var versionedCommand = parser.GetCommandType("SSBP");
        Assert.That(versionedCommand, Is.SameAs(typeof(SuperSourceBoxParametersUpdateCommandV8)));
    }


    [Test]
    public void WithDifferentVersions_ShouldSelectCorrectUnversionedCommand()
    {
        // Arrange
        var parser = new CommandParser();

        var baselineCommand = parser.GetCommandType("InCm");
        Assert.That(baselineCommand, Is.SameAs(typeof(InitCompleteCommand)));

        parser.Version = ProtocolVersion.V7_5_2;
        var oldVersionedCommand = parser.GetCommandType("InCm");
        Assert.That(oldVersionedCommand, Is.SameAs(typeof(InitCompleteCommand)));

        parser.Version = ProtocolVersion.V9_4;
        var versionedCommand = parser.GetCommandType("InCm");
        Assert.That(versionedCommand, Is.SameAs(typeof(InitCompleteCommand)));
    }
}
