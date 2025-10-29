using AtemSharp.Commands.Streaming;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Streaming;

public class StreamingServiceCommandTests : SerializedCommandTestBase<StreamingServiceCommand, StreamingServiceCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public string ServiceName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public uint[] Bitrates { get; set; } = [];
    }


    protected override StreamingServiceCommand CreateSut(TestCaseData testCase)
    {
        var state = new AtemState
        {
            Streaming =
            {
                ServiceName = testCase.Command.ServiceName,
                Url = testCase.Command.Url,
                Key = testCase.Command.Key,
                VideoBitrates =
                {
                    Low = testCase.Command.Bitrates[0],
                    High = testCase.Command.Bitrates[1],
                }
            }
        };

        return new StreamingServiceCommand(state);
    }
}
