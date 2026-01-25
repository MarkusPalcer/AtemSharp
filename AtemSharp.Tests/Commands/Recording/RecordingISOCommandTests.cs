using AtemSharp.Commands.Recording;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingIsoCommandTests : TypeScriptLibrarySerializedCommandTestBase<RecordingIsoCommand, RecordingIsoCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsoRecordAllInputs { get; set; }
    }

    protected override RecordingIsoCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new RecordingIsoCommand(new AtemState
        {
            Recording =
            {
                RecordAllInputs = testCase.Command.IsoRecordAllInputs
            }
        });
    }
}
