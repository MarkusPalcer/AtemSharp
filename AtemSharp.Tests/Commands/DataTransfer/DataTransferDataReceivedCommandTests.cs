using AtemSharp.Commands.DataTransfer;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DataTransfer;

/// <summary>
/// Test deserialization functionality of DataTransferDataCommand using data-driven tests
/// </summary>
[TestFixture]
public class DataTransferDataReceivedCommandTests : DeserializedCommandTestBase<DataTransferDataReceivedCommand,
    DataTransferDataReceivedCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public string Body { get; set; } = "";
    }

    protected override void CompareCommandProperties(DataTransferDataReceivedCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.TransferId, Is.EqualTo(expectedData.TransferId));
        Assert.That(actualCommand.Body, Is.EqualTo(Convert.FromBase64String(expectedData.Body)));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        // No state change
    }
}
