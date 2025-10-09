using AtemSharp.Commands.DataTransfer;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferDataCommandSerializationTests : SerializedCommandTestBase<DataTransferDataCommand, DataTransferDataCommandSerializationTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public string Body { get; set; } = "";
    }

    protected override DataTransferDataCommand CreateSut(TestCaseData testCase)
    {
        // Convert Base64 string back to byte array
        var bodyBytes = Convert.FromBase64String(testCase.Command.Body);
        
        // Create command with the test data values
        var command = new DataTransferDataCommand(
            testCase.Command.TransferId,
            bodyBytes);

        return command;
    }

    [Test]
    public void ApplyToState_ShouldReturnEmptyArray()
    {
        // Arrange
        var command = new DataTransferDataCommand(123, new byte[] { 1, 2, 3 });
        var state = new AtemState();

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(result, Is.Empty);
    }
}