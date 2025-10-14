using System.Reflection;
using AtemSharp.Commands;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands;

[TestFixture]
// The test data does not contain test cases for this command since it has no data to deserialize
public class InitCompleteCommandTests
{
    [Test]
    public void TestDeserialization()
    {
        // Act - Deserialize the command
        var command = InitCompleteCommand.Deserialize(Span<byte>.Empty, ProtocolVersion.V7_2);

        // Assert - Verify the command was created successfully
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.InstanceOf<InitCompleteCommand>());
    }

    [Test]
    public void TestCommandAttribute()
    {
        // Arrange & Act - Get the command attribute from the class
        var commandAttribute = typeof(InitCompleteCommand).GetCustomAttribute<CommandAttribute>();

        // Assert - Verify the command has the correct raw name
        Assert.That(commandAttribute, Is.Not.Null);
        Assert.That(commandAttribute.RawName, Is.EqualTo("InCm"));
    }
}
