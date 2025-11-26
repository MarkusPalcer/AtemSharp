using AtemSharp.State.Audio.Fairlight;
using AudioRoutingOutputCommand = AtemSharp.Commands.Audio.Fairlight.AudioRouting.AudioRoutingOutputCommand;

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
        return new AudioRoutingOutputCommand(new AudioRoutingEntry
        {
            Id = testCase.Command.Id,
            Name = testCase.Command.Name,
        })
        {
            SourceId = testCase.Command.SourceId,
        };
    }
}
