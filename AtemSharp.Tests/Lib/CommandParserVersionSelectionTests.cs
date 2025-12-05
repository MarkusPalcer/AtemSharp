using AtemSharp.Communication;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities.TestCommands;

namespace AtemSharp.Tests.Lib;

[TestFixture]
public class CommandParserVersionSelectionTests
{
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
        var versions = parser.GetAllCommandVersions("TEST");

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
