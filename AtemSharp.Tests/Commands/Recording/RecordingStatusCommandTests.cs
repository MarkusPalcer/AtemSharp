using AtemSharp.Commands.Recording;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingStatusCommandTests : SerializedCommandTestBase<RecordingStatusCommand, RecordingStatusCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsRecording { get; set; }
    }


    protected override RecordingStatusCommand CreateSut(TestCaseData testCase)
    {
        var state = new AtemSharp.State.AtemState
        {
            Recording =
            {
                Status = testCase.Command.IsRecording ? AtemSharp.State.RecordingStatus.Recording : AtemSharp.State.RecordingStatus.Idle
            }
        };
        return new RecordingStatusCommand(state);
    }
}
