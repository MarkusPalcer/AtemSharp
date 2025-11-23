using AtemSharp.Commands.Fairlight.AudioRouting;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight.AudioRouting;

public class AudioRoutingSourceCommandTests : SerializedCommandTestBase<AudioRoutingSourceCommand, AudioRoutingSourceCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public uint Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    protected override AudioRoutingSourceCommand CreateSut(TestCaseData testCase)
    {
        return new AudioRoutingSourceCommand(new AudioRoutingEntry()
        {
            Id = testCase.Command.Id,
            Name = testCase.Command.Name
        });
    }
}
