using AtemSharp.Commands;
using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Commands.SuperSource;
using AtemSharp.Communication;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities.TestCommands;

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
        var registeredCommands = (IReadOnlyCollection<string>)parser.CommandRegistry.Keys;
        Assert.That(registeredCommands, Contains.Item("_ver"));
        Assert.That(registeredCommands, Contains.Item("InCm"));
    }

    [Test]
    public void GetCommandType_WithValidRawName_ShouldReturnCorrectType()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var commandType = parser.GetCommandTypeForVersion("_ver");

        // Assert
        Assert.That(commandType, Is.EqualTo(typeof(VersionCommand)));
    }

    [Test]
    public void GetCommandType_WithInvalidRawName_ShouldReturnNull()
    {
        // Arrange
        var parser = new CommandParser();

        // Act
        var commandType = parser.GetCommandTypeForVersion("XXXX");

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
        var commandTypeV7 = parser.GetCommandTypeForVersion("_ver");

        parser.Version = ProtocolVersion.V8_1_1;
        var commandTypeV8 = parser.GetCommandTypeForVersion("_ver");

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
        var versions = (IReadOnlyList<Type>)(parser.CommandRegistry.TryGetValue("_ver", out var commandTypes)
                                                 ? commandTypes.Values.AsReadOnly()
                                                 : new List<Type>().AsReadOnly());

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
        var versions = (IReadOnlyList<Type>)(parser.CommandRegistry.TryGetValue("UNKN", out var commandTypes)
                                                 ? commandTypes.Values.AsReadOnly()
                                                 : new List<Type>().AsReadOnly());

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

        var baselineCommand = parser.GetCommandTypeForVersion("SSBP");
        Assert.That(baselineCommand, Is.SameAs(typeof(SuperSourceBoxParametersUpdateCommand)));

        parser.Version = ProtocolVersion.V7_5_2;
        var oldVersionedCommand = parser.GetCommandTypeForVersion("SSBP");
        Assert.That(oldVersionedCommand, Is.SameAs(typeof(SuperSourceBoxParametersUpdateCommand)));

        parser.Version = ProtocolVersion.V9_4;
        var versionedCommand = parser.GetCommandTypeForVersion("SSBP");
        Assert.That(versionedCommand, Is.SameAs(typeof(SuperSourceBoxParametersUpdateCommandV8)));
    }


    [Test]
    public void WithDifferentVersions_ShouldSelectCorrectUnversionedCommand()
    {
        // Arrange
        var parser = new CommandParser();

        var baselineCommand = parser.GetCommandTypeForVersion("InCm");
        Assert.That(baselineCommand, Is.SameAs(typeof(InitCompleteCommand)));

        parser.Version = ProtocolVersion.V7_5_2;
        var oldVersionedCommand = parser.GetCommandTypeForVersion("InCm");
        Assert.That(oldVersionedCommand, Is.SameAs(typeof(InitCompleteCommand)));

        parser.Version = ProtocolVersion.V9_4;
        var versionedCommand = parser.GetCommandTypeForVersion("InCm");
        Assert.That(versionedCommand, Is.SameAs(typeof(InitCompleteCommand)));
    }

     [Test]
    public void ParseCommand_WithV7_2_ShouldSelectBaseline()
    {
        // Arrange
        var parser = new CommandParser { Version = ProtocolVersion.V7_2 };
        parser.AddCommandsFromAssemblyOf<TestCommandV1>();

        // Act
        var command = parser.ParseCommand("TEST", Span<byte>.Empty);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.TypeOf<TestCommandV1>());
        Assert.That(((TestCommandV1)command).Version, Is.EqualTo("V1"));
    }

    [Test]
    public void ParseCommand_WithV8_0_ShouldSelectV2()
    {
        // Arrange
        var parser = new CommandParser { Version = ProtocolVersion.V8_0 };
        parser.AddCommandsFromAssemblyOf<TestCommandV1>();

        // Act
        var command = parser.ParseCommand("TEST", Span<byte>.Empty);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.TypeOf<TestCommandV2>());
        Assert.That(((TestCommandV2)command).Version, Is.EqualTo("V2"));
    }

    [Test]
    public void ParseCommand_WithV8_1_1_ShouldSelectV3()
    {
        // Arrange
        var parser = new CommandParser { Version = ProtocolVersion.V8_1_1 };
        parser.AddCommandsFromAssemblyOf<TestCommandV1>();

        // Act
        var command = parser.ParseCommand("TEST", Span<byte>.Empty);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.TypeOf<TestCommandV3>());
        Assert.That(((TestCommandV3)command).Version, Is.EqualTo("V3"));
    }

    [Test]
    public void GetAllCommandVersions_WithTestCommand_ShouldReturnAllThreeVersions()
    {
        // Arrange
        var parser = new CommandParser();
        parser.AddCommandsFromAssemblyOf<TestCommandV1>();

        // Act
        var versions = (IReadOnlyList<Type>)(parser.CommandRegistry.TryGetValue("TEST", out var commandTypes)
                                                 ? commandTypes.Values.AsReadOnly()
                                                 : new List<Type>().AsReadOnly());

        // Assert
        Assert.That(versions, Is.EquivalentTo((Type[])[typeof(TestCommandV1), typeof(TestCommandV2), typeof(TestCommandV3)]));
    }

    [Test]
    public void VersionSelection_AfterProtocolVersionChanges_ShouldSelectCorrectVersion()
    {
        // Arrange
        var parser = new CommandParser();
        parser.AddCommandsFromAssemblyOf<TestCommandV1>();

        // Start with high version and work backwards to test dynamic selection
        parser.Version = ProtocolVersion.V8_1_1;
        var commandV3 = parser.ParseCommand("TEST", Span<byte>.Empty);

        // Change to lower version
        parser.Version = ProtocolVersion.V8_0;
        var commandV2 = parser.ParseCommand("TEST", Span<byte>.Empty);

        // Change to baseline version
        parser.Version = ProtocolVersion.V7_2;
        var commandV1 = parser.ParseCommand("TEST", Span<byte>.Empty);

        // Assert
        Assert.That(commandV3, Is.TypeOf<TestCommandV3>());
        Assert.That(commandV2, Is.TypeOf<TestCommandV2>());
        Assert.That(commandV1, Is.TypeOf<TestCommandV1>());
    }
}
