using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferFileDescriptionCommandTests : SerializedCommandTestBase<DataTransferFileDescriptionCommand,
    DataTransferFileDescriptionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FileHash { get; set; } = string.Empty;
    }

    protected override DataTransferFileDescriptionCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new DataTransferFileDescriptionCommand(
            testCase.Command.TransferId,
            testCase.Command.Name,
            testCase.Command.Description,
            testCase.Command.FileHash);
    }
}
