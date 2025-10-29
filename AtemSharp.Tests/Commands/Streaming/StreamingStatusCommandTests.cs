using AtemSharp.Commands.Streaming;
using AtemSharp.State;
using AtemSharp.State.Streaming;

namespace AtemSharp.Tests.Commands.Streaming;

public class StreamingStatusCommandTests : SerializedCommandTestBase<StreamingStatusCommand, StreamingStatusCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsStreaming { get; set; }
    }

    protected override StreamingStatusCommand CreateSut(TestCaseData testCase)
    {
        return new StreamingStatusCommand(new AtemState
        {
            Streaming =
            {
                Status = testCase.Command.IsStreaming ? StreamingStatus.Streaming : StreamingStatus.Idle
            }
        });
    }
}
