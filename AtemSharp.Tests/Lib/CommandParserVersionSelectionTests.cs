using AtemSharp.Lib;
using AtemSharp.Enums;
using AtemSharp.Tests.Lib.TestCommands;

namespace AtemSharp.Tests.Lib;

[TestFixture]
public class CommandParserVersionSelectionTests
{
    [SetUp]
    public void Setup()
    {
        // Reinitialize the registry to pick up test commands
        CommandParser.ReinitializeForTesting();
    }

    [Test]
    public void ParseCommand_WithV7_2_ShouldSelectBaseline()
    {
        // Arrange
        var parser = new CommandParser { Version = ProtocolVersion.V7_2 };
        using var stream = new MemoryStream();

        // Act
        var command = parser.ParseCommand("TEST", stream);

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
        using var stream = new MemoryStream();

        // Act
        var command = parser.ParseCommand("TEST", stream);

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
        using var stream = new MemoryStream();

        // Act
        var command = parser.ParseCommand("TEST", stream);

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

        // Act
        var versions = parser.GetAllCommandVersions("TEST");

        // Assert
        Assert.That(versions, Has.Count.EqualTo(3));
        Assert.That(versions, Contains.Item(typeof(TestCommandV1)));
        Assert.That(versions, Contains.Item(typeof(TestCommandV2)));
        Assert.That(versions, Contains.Item(typeof(TestCommandV3)));
    }

    [Test]
    public void VersionSelection_AfterProtocolVersionChanges_ShouldSelectCorrectVersion()
    {
        // Arrange
        var parser = new CommandParser();
        using var stream = new MemoryStream();

        // Start with high version and work backwards to test dynamic selection
        parser.Version = ProtocolVersion.V8_1_1;
        var commandV3 = parser.ParseCommand("TEST", stream);

        // Change to lower version
        parser.Version = ProtocolVersion.V8_0;
        stream.Position = 0; // Reset stream
        var commandV2 = parser.ParseCommand("TEST", stream);

        // Change to baseline version
        parser.Version = ProtocolVersion.V7_2;
        stream.Position = 0; // Reset stream
        var commandV1 = parser.ParseCommand("TEST", stream);

        // Assert
        Assert.That(commandV3, Is.TypeOf<TestCommandV3>());
        Assert.That(commandV2, Is.TypeOf<TestCommandV2>());
        Assert.That(commandV1, Is.TypeOf<TestCommandV1>());
    }
}