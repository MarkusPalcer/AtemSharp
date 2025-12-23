using AtemSharp.Commands.MixEffects;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects;

[TestFixture]
public class ProgramInputCommandTests : SerializedCommandTestBase<ProgramInputCommand,
    ProgramInputCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public ushort Source { get; set; }
    }

    protected override ProgramInputCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new ProgramInputCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            ProgramInput = testCase.Command.Source
        });
    }
}
