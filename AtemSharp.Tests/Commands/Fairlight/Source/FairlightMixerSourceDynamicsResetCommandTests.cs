using FairlightMixerSourceDynamicsResetCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceDynamicsResetCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceDynamicsResetCommandTests : SerializedCommandTestBase<FairlightMixerSourceDynamicsResetCommand, FairlightMixerSourceDynamicsResetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool Dynamics { get; set; }
        public bool Expander { get; set; }
        public bool Compressor { get; set; }
        public bool Limiter { get; set; }
    }

    protected override FairlightMixerSourceDynamicsResetCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new  FairlightMixerSourceDynamicsResetCommand(new AtemSharp.State.Audio.Fairlight.Source
        {
            Id = testCase.Command.SourceId,
            InputId = testCase.Command.Index
        })
        {
            ResetDynamics = testCase.Command.Dynamics,
            ResetExpander = testCase.Command.Expander,
            ResetCompressor = testCase.Command.Compressor,
            ResetLimiter = testCase.Command.Limiter,
        };
    }
}
