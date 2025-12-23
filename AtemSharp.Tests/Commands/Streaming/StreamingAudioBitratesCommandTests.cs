using AtemSharp.Commands.Streaming;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Streaming;

public class StreamingAudioBitratesCommandTests : SerializedCommandTestBase<StreamingAudioBitratesCommand, StreamingAudioBitratesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public uint LowBitrate { get; set; }
        public uint HighBitrate { get; set; }
    }

    protected override StreamingAudioBitratesCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new StreamingAudioBitratesCommand(new AtemState
        {
            Streaming =
            {
                AudioBitrates =
                {
                    Low = testCase.Command.LowBitrate,
                    High = testCase.Command.HighBitrate
                }
            }
        });
    }
}
