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
    public void CommandParser_ParseVersionCommand_ShouldUpdateParserVersion()
    {
        // Arrange
        var sut = new CommandParser();
        Span<byte> versionData = [0x00, 0x02, 0x00, 0x1C]; // V8_0

        // Act
        var command = sut.ParseCommand("_ver", versionData);

        // Assert
        Assert.That(command, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(command, Is.TypeOf<VersionCommand>());
            Assert.That(sut.Version, Is.EqualTo(ProtocolVersion.V8_0));
        });
    }


    [Test]
    public void CommandParser_ParseUnknownCommand_ShouldReturnNull()
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
    public void CommandParser_ParseInitCompleteCommand_ShouldReturnValidCommand()
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
    public void CommandParser_WithDifferentVersions_ShouldSelectCorrectVersionedCommand()
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
    public void CommandParser_WithDifferentVersions_ShouldSelectCorrectUnversionedCommand()
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
