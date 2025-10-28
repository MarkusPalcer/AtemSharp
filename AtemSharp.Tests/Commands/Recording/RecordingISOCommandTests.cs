using AtemSharp.Commands.Recording;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingIsoCommandTests : SerializedCommandTestBase<RecordingIsoCommand, RecordingIsoCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool ISORecordAllInputs { get; set; }
    }

    protected override RecordingIsoCommand CreateSut(TestCaseData testCase)
    {
        var state = new AtemState
        {
            Recording =
            {
                RecordAllInputs = testCase.Command.ISORecordAllInputs
            }
        };
        return new RecordingIsoCommand(state);
    }
}
