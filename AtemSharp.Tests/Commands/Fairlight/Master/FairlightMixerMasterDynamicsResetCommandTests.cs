using AtemSharp.Commands.Fairlight.Master;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterDynamicsResetCommandTests : SerializedCommandTestBase<FairlightMixerMasterDynamicsResetCommand, FairlightMixerMasterDynamicsResetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool Dynamics { get; set; }
        public bool Expander { get; set; }
        public bool Compressor { get; set; }
        public bool Limiter { get; set; }
    }

    protected override FairlightMixerMasterDynamicsResetCommand CreateSut(TestCaseData testCase)
    {
        return new FairlightMixerMasterDynamicsResetCommand
        {
            ResetCompressor = testCase.Command.Compressor,
            ResetDynamics = testCase.Command.Dynamics,
            ResetExpander = testCase.Command.Expander,
            ResetLimiter = testCase.Command.Limiter
        };
    }
}
