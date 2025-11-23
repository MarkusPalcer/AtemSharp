using AtemSharp.Commands.Fairlight.AudioRouting;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight.AudioRouting;

public class AudioRoutingOutputCommandTests : SerializedCommandTestBase<AudioRoutingOutputCommand, AudioRoutingOutputCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public uint Id { get; set; }
        public uint SourceId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    protected override AudioRoutingOutputCommand CreateSut(TestCaseData testCase)
    {
        return new AudioRoutingOutputCommand(new AudioRoutingOutput
        {
            Id = testCase.Command.Id,
        })
        {
            SourceId = testCase.Command.SourceId,
            Name =  testCase.Command.Name,
        };
    }
}
