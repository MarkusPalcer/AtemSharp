using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

/// <summary>
/// Test deserialization functionality of DataTransferDataCommand using data-driven tests
/// </summary>
[TestFixture]
public class DataTransferDataCommandDeserializationTests : DeserializedCommandTestBase<DataTransferDataCommand,
    DataTransferDataCommandDeserializationTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public string Body { get; set; } = "";
    }

    protected override void CompareCommandProperties(DataTransferDataCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.TransferId, Is.EqualTo(expectedData.TransferId));
        Assert.That(actualCommand.Body, Is.EqualTo(Convert.FromBase64String(expectedData.Body)));
    }
}
