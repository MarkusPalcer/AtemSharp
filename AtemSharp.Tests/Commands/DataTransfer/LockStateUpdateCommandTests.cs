using AtemSharp.Commands.DataTransfer;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class LockStateUpdateCommandTests : DeserializedCommandTestBase<LockStateUpdateCommand, LockStateUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public bool Locked { get; set; }
    }

    protected override void CompareCommandProperties(LockStateUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Index - it is not floating point so it needs to equal exactly
        if (!actualCommand.Index.Equals(expectedData.Index))
        {
            failures.Add($"Index: expected {expectedData.Index}, actual {actualCommand.Index}");
        }

        // Compare Locked - it is not floating point so it needs to equal exactly
        if (!actualCommand.Locked.Equals(expectedData.Locked))
        {
            failures.Add($"Locked: expected {expectedData.Locked}, actual {actualCommand.Locked}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }


    [Test]
    public void Deserialize_ShouldCorrectlyParseIndexAndLocked()
    {
        // Arrange
        var data = new byte[] { 0x1D, 0xD2, 0x00, 0x00 }; // Index: 7634, Locked: false
        using var stream = new MemoryStream(data);

        // Act
        var command = LockStateUpdateCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Index, Is.EqualTo(7634));
        Assert.That(command.Locked, Is.False);
    }

    [Test]
    public void Deserialize_ShouldCorrectlyParseIndexAndLockedTrue()
    {
        // Arrange
        var data = new byte[] { 0x2D, 0x98, 0x01, 0x00 }; // Index: 11672, Locked: true
        using var stream = new MemoryStream(data);

        // Act
        var command = LockStateUpdateCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Index, Is.EqualTo(11672));
        Assert.That(command.Locked, Is.True);
    }
}
