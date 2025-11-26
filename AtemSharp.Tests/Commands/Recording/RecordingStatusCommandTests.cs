using AtemSharp.Commands.Recording;
using AtemSharp.State;
using AtemSharp.State.Recording;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingStatusCommandTests : SerializedCommandTestBase<RecordingStatusCommand, RecordingStatusCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsRecording { get; set; }
    }


    protected override RecordingStatusCommand CreateSut(TestCaseData testCase)
    {
        return new RecordingStatusCommand(new AtemState
        {
            Recording =
            {
                Status = testCase.Command.IsRecording ? RecordingStatus.Recording : RecordingStatus.Idle
            }
        });
    }
}
