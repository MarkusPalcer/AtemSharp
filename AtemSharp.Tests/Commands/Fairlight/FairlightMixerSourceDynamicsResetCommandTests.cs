using AtemSharp.Commands.Fairlight;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

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

    protected override FairlightMixerSourceDynamicsResetCommand CreateSut(TestCaseData testCase)
    {
        return new  FairlightMixerSourceDynamicsResetCommand(new Source
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
